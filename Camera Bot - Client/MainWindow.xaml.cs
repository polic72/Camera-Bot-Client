using System;
using System.Collections.Generic;
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



        }


        #region Buttons

        private void settings_button_Click(object sender, RoutedEventArgs e)
        {
            Settings_Window settings_window = new Settings_Window();
            settings_window.ShowDialog();
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

        #endregion Buttons


        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            IPHostEntry hostEntry = Dns.GetHostEntry(Dns.GetHostName());
            IPAddress IP_address = hostEntry.AddressList[0];
            IPEndPoint server_endPoint = new IPEndPoint(IP_address, 11000);

            


            Socket data_sender = new Socket(IP_address.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

            try
            {
                data_sender.Connect(server_endPoint);

                byte[] msg = Encoding.ASCII.GetBytes("This is a test.");

                data_sender.Send(msg);


                data_sender.Shutdown(SocketShutdown.Both);
                data_sender.Close();
            }
            catch (ArgumentNullException ane)
            {
                Console.WriteLine("ArgumentNullException : {0}", ane.ToString());
            }
            catch (SocketException se)
            {
                Console.WriteLine("SocketException : {0}", se.ToString());
            }
            catch (Exception ex)
            {
                Console.WriteLine("Unexpected exception : {0}", ex.ToString());
            }


            switch (e.Key)
            {
                case Key.Up:
                    break;


                case Key.Right:
                    break;


                case Key.Down:
                    break;


                case Key.Left:
                    break;
            }
        }
    }
}
