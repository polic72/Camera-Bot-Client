using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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


        public Settings_Window(Settings settings)
        {
            InitializeComponent();


            user_settings = settings;


            IPv4_textBox.Text = user_settings.IPAddress?.ToString();
        }


        #region Check Legal

        #region IsLegal

        /// <summary>
        /// Checks whether or not the current state of the Settings_Window is legal.
        /// </summary>
        /// <returns>True if legal. False otherwise.</returns>
        private bool IsLegal()
        {
            return IsLegal_IPv4();  //&& others
        }


        /// <summary>
        /// Checks whether or not the current text in the IPv4_textBox is legal.
        /// </summary>
        /// <returns>True if legal. False otherwise.</returns>
        private bool IsLegal_IPv4()
        {
            return Settings.IPv4_regex.IsMatch(IPv4_textBox.Text);
        }

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

        #endregion Check Legal


        #region Finish Buttons

        private void cancel_button_Click(object sender, RoutedEventArgs e)
        {
            WasCancelled = true;

            Close();
        }


        private void enter_button_Click(object sender, RoutedEventArgs e)
        {
            user_settings.IPAddress = Settings.IPv4_FromString(IPv4_textBox.Text);


            WasCancelled = false;

            Close();
        }

        #endregion Finish Buttons
    }
}
