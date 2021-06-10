﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using Screen = System.Windows.Forms.Screen;


namespace Camera_Bot___Client
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private static Color Normal_Color = new Color();


        private byte[] buffer = new byte[1024];


        public const string SETTINGS_PATH = "CameraBot_Settings.config";
        private Settings user_settings;


        private IPEndPoint server_endPoint;
        private Socket communicator;


        public MainWindow()
        {
            InitializeComponent();

            
            Normal_Color.R = 221;
            Normal_Color.G = 221;
            Normal_Color.B = 221;
            Normal_Color.A = 255;


            WpfScreen this_screen = WpfScreen.GetScreenFrom(this);

            Top = 0 + (this_screen.WorkingArea.Height / 8);
            Left = (this_screen.WorkingArea.Width / 2) - (Width / 2);


            user_settings = new Settings();

            if (File.Exists(SETTINGS_PATH))
            {
                user_settings = Settings.FromFile(SETTINGS_PATH);
            }
            else
            {
                Settings_Window settings_window = new Settings_Window(user_settings);
                settings_window.ShowDialog();

                if (settings_window.WasCancelled)
                {
                    Close();
                }
            }

            PrepareNewConnection();
        }


        #region Buttons

        private void settings_button_Click(object sender, RoutedEventArgs e)
        {
            Settings_Window settings_window = new Settings_Window(user_settings);
            settings_window.ShowDialog();


            PrepareNewConnection();
        }


        private void pin_button_Click(object sender, RoutedEventArgs e)
        {
            Topmost = !Topmost;

            if (!Topmost)
            {
                pin_button.Background = new SolidColorBrush(Normal_Color);
            }
            else
            {
                pin_button.Background = new SolidColorBrush(Colors.Gray);
            }
        }


        private void connect_button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!communicator.Connected)
                {
                    communicator.Connect(server_endPoint);
                }
                else
                {
                    HandleKnownResponse(SendCommand("Break"));
                }
            }
            catch (ArgumentNullException ane)
            {
                MessageBox.Show(ane.ToString(), "ArgumentNullException", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK);
            }
            catch (SocketException se)
            {
                MessageBox.Show(se.ToString(), "SocketException", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), ex.GetType().ToString(), MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK);
            }
        }

        #endregion Buttons


        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (!e.IsRepeat)
            {
                if (communicator.Connected)
                {
                    try
                    {
                        //Can't use switch :< gotta Yandere Dev it.
                        if (e.Key == user_settings.Up)
                        {
                            HandleKnownResponse(SendCommand("Up"));
                        }
                        else if (e.Key == user_settings.Down)
                        {
                            HandleKnownResponse(SendCommand("Down"));
                        }
                        else if (e.Key == user_settings.Left)
                        {
                            HandleKnownResponse(SendCommand("Left"));
                        }
                        else if (e.Key == user_settings.Right)
                        {
                            HandleKnownResponse(SendCommand("Right"));
                        }
                        else if (e.Key == Key.Escape)
                        {
                            Close();
                        }
                    }
                    catch (ArgumentNullException ane)
                    {
                        MessageBox.Show(ane.ToString(), "ArgumentNullException", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK);
                    }
                    catch (SocketException se)
                    {
                        MessageBox.Show(se.ToString(), "SocketException", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.ToString(), ex.GetType().ToString(), MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK);
                    }
                }
            }
        }


        #region Connection Helpers

        /// <summary>
        /// Prepares a new connection to the server. Should be run after any edit to the user settings.
        /// </summary>
        private void PrepareNewConnection()
        {
            server_endPoint = new IPEndPoint(user_settings.IPAddress, user_settings.Port);

            communicator = new Socket(user_settings.IPAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
        }


        /// <summary>
        /// Sends a command to the server.
        /// </summary>
        /// <param name="command">The command to send to the server.</param>
        /// <returns>The response you get back from the server.</returns>
        private string SendCommand(string command)
        {
            communicator.Send(Encoding.ASCII.GetBytes(command));

            int count = communicator.Receive(buffer);
            return Encoding.ASCII.GetString(buffer, 0, count);
        }


        /// <summary>
        /// Handles a known response from the server.
        /// </summary>
        /// <param name="response">The response received from the server.</param>
        private void HandleKnownResponse(string response)
        {
            switch (response)
            {
                case "Ok":
                    //The command was processed.
                    break;


                case "Goodbye":
                    communicator.Shutdown(SocketShutdown.Both);
                    communicator.Close();

                    PrepareNewConnection();
                    break;


                case "Bad":
                    throw new InvalidOperationException("The given command is not allowed.");
            }
        }

        #endregion Connection Helpers


        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (communicator.Connected)
            {
                communicator.Shutdown(SocketShutdown.Both);
                communicator.Close();
            }
        }
    }
}
