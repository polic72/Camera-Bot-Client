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
        public Settings_Window()
        {
            InitializeComponent();
        }


        #region Finish Buttons

        private void cancel_button_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }


        private void enter_button_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        #endregion Finish Buttons
    }
}
