using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Camera_Bot___Client
{
    /// <summary>
    /// Interaction logic for Settings_Window.xaml
    /// </summary>
    public partial class Settings_Window : Window
    {
        /// <summary>
        /// Whether or not the settings were cancelled. 
        /// </summary>
        public bool WasCancelled { get; private set; }


        private Settings user_settings;


        private bool waiting_for_key = false;

        private Button waiting_button;


        private static readonly Regex movementButton_regex = new Regex(@"\[ (.+) \]", RegexOptions.Compiled);


        public Settings_Window(Settings settings)
        {
            InitializeComponent();


            user_settings = settings;


            IPv4_textBox.Text = user_settings.IPAddress?.ToString();
            port_textBox.Text = (user_settings.Port != 0) ? user_settings.Port.ToString() : "";

            up_button.Content = "[ " + user_settings.Up.ToString() + " ]";
            down_button.Content = "[ " + user_settings.Down.ToString() + " ]";
            left_button.Content = "[ " + user_settings.Left.ToString() + " ]";
            right_button.Content = "[ " + user_settings.Right.ToString() + " ]";
        }


        #region Check Legal

        #region IsLegal

        /// <summary>
        /// Checks whether or not the current state of the Settings_Window is legal.
        /// </summary>
        /// <returns>True if legal. False otherwise.</returns>
        private bool IsLegal()
        {
            return IsLegal_IPv4() && IsLegal_Port() && IsLegal_UpButton() && IsLegal_DownButton() && IsLegal_LeftButton() && IsLegal_RightButton();
        }


        /// <summary>
        /// Checks whether or not the current text in the IPv4_textBox is legal.
        /// </summary>
        /// <returns>True if legal. False otherwise.</returns>
        private bool IsLegal_IPv4()
        {
            return Settings.IPv4_regex.IsMatch(IPv4_textBox.Text);
        }


        /// <summary>
        /// Checks whether or not the current text in the port_textBox is legal.
        /// </summary>
        /// <returns>True if legal. False otherwise.</returns>
        private bool IsLegal_Port()
        {
            try
            {
                int port = int.Parse(port_textBox.Text);

                return port >= Settings.MinPort && port <= Settings.MaxPort;
            }
            catch
            {
                return false;
            }
        }


        #region Movement Buttons

        /// <summary>
        /// Checks whether or not the current text in the up_button is legal.
        /// </summary>
        /// <returns>True if legal. False otherwise.</returns>
        private bool IsLegal_UpButton()
        {
            return (string)up_button.Content != "[ None ]";
        }


        /// <summary>
        /// Checks whether or not the current text in the down_button is legal.
        /// </summary>
        /// <returns>True if legal. False otherwise.</returns>
        private bool IsLegal_DownButton()
        {
            return (string)down_button.Content != "[ None ]";
        }


        /// <summary>
        /// Checks whether or not the current text in the left_button is legal.
        /// </summary>
        /// <returns>True if legal. False otherwise.</returns>
        private bool IsLegal_LeftButton()
        {
            return (string)left_button.Content != "[ None ]";
        }


        /// <summary>
        /// Checks whether or not the current text in the right_button is legal.
        /// </summary>
        /// <returns>True if legal. False otherwise.</returns>
        private bool IsLegal_RightButton()
        {
            return (string)right_button.Content != "[ None ]";
        }

        #endregion Movement Buttons

        #endregion IsLegal


        private void IPv4_textBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (IsLegal_IPv4())
            {
                IPv4_textBlock.Foreground = new SolidColorBrush(Colors.Black);
            }
            else
            {
                IPv4_textBlock.Foreground = new SolidColorBrush(Colors.Red);
            }


            enter_button.IsEnabled = IsLegal();
        }


        private void port_textBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (IsLegal_Port())
            {
                port_textBlock.Foreground = new SolidColorBrush(Colors.Black);
            }
            else
            {
                port_textBlock.Foreground = new SolidColorBrush(Colors.Red);
            }


            enter_button.IsEnabled = IsLegal();
        }

        #endregion Check Legal


        #region Movement Buttons

        private void up_button_Click(object sender, RoutedEventArgs e)
        {
            AssignMovementKey(up_button);
        }


        private void down_button_Click(object sender, RoutedEventArgs e)
        {
            AssignMovementKey(down_button);
        }


        private void left_button_Click(object sender, RoutedEventArgs e)
        {
            AssignMovementKey(left_button);
        }


        private void right_button_Click(object sender, RoutedEventArgs e)
        {
            AssignMovementKey(right_button);
        }


        /// <summary>
        /// Goes through the process of assigning the given button a key.
        /// </summary>
        /// <param name="button">The button to assign the movement to.</param>
        private void AssignMovementKey(Button button)
        {
            button.Content = "...";


            enter_button.IsEnabled = false;
            cancel_button.IsEnabled = false;

            IPv4_textBox.IsEnabled = false;
            port_textBox.IsEnabled = false;

            up_button.IsEnabled = false;
            down_button.IsEnabled = false;
            left_button.IsEnabled = false;
            right_button.IsEnabled = false;


            waiting_for_key = true;

            waiting_button = button;
        }


        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (waiting_for_key)
            {
                waiting_button.Content = "[ " + e.Key.ToString() + " ]";

                enter_button.IsEnabled = IsLegal();
                cancel_button.IsEnabled = true;

                IPv4_textBox.IsEnabled = true;
                port_textBox.IsEnabled = true;

                up_button.IsEnabled = true;
                down_button.IsEnabled = true;
                left_button.IsEnabled = true;
                right_button.IsEnabled = true;


                waiting_for_key = false;
            }
        }

        #endregion Movement Buttons


        #region Finish Buttons

        private void cancel_button_Click(object sender, RoutedEventArgs e)
        {
            WasCancelled = true;

            Close();
        }


        private void enter_button_Click(object sender, RoutedEventArgs e)
        {
            user_settings.IPAddress = Settings.IPv4_FromString(IPv4_textBox.Text);

            user_settings.Port = int.Parse(port_textBox.Text);

            user_settings.Up = (Key)Enum.Parse(typeof(Key), movementButton_regex.Match((string)up_button.Content).Groups[1].Value);

            user_settings.Down = (Key)Enum.Parse(typeof(Key), movementButton_regex.Match((string)down_button.Content).Groups[1].Value);

            user_settings.Left = (Key)Enum.Parse(typeof(Key), movementButton_regex.Match((string)left_button.Content).Groups[1].Value);

            user_settings.Right = (Key)Enum.Parse(typeof(Key), movementButton_regex.Match((string)right_button.Content).Groups[1].Value);


            user_settings.SaveToFile(MainWindow.SETTINGS_PATH);


            WasCancelled = false;

            Close();
        }

        #endregion Finish Buttons
    }
}
