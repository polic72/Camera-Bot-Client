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
