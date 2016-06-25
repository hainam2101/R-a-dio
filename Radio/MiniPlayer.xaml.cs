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

namespace Radio
{
    /// <summary>
    /// Lógica de interacción para MiniPlayer.xaml
    /// </summary>
    public partial class MiniPlayer : Window
    {

        ControlTemplate miniPlay;
        ControlTemplate miniSong;

        MainWindow mw;

        public MiniPlayer()
        {
            InitializeComponent();
            miniPlay = (ControlTemplate)FindResource("miniPlay");
            miniSong = (ControlTemplate)this.FindResource("miniSong");
            miniPlayerMain.Template = miniSong;
        }

        public void SetOtherView(MainWindow MW)
        {
            mw = MW;
        }

        private void Grid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            bool wasLeftClick = e.ChangedButton == MouseButton.Left;

            if (wasLeftClick && e.Source is Grid
                || wasLeftClick && e.OriginalSource is Visual)
            {
                this.DragMove();
            }
        }

        private void miniPlayerMain_MouseEnter(object sender, MouseEventArgs e)
        {

            miniPlayerMain.Template = miniPlay;

        }

        private void miniPlayerMain_MouseLeave(object sender, MouseEventArgs e)
        {
            miniPlayerMain.Template = miniSong;


        }

        private void buttonPlay_Click(object sender, RoutedEventArgs e)
        {
            if (miniPlayerMain.Content.Equals("Play"))
            {
                miniPlayerMain.Content = "Stop";
            }
            else
            {
                miniPlayerMain.Content = "Play";
            }
        }

        /*private void buttonPlay_Click(object sender, RoutedEventArgs e)
        {
            if (miniPlayerMain.Content.Equals("Play"))
            {
                miniPlayerMain.Content = "Stop";
            }
            else
            {
                miniPlayerMain.Content = "Play";
            }
        }*/

        /*private void buttonPlay_Click(object sender, RoutedEventArgs e)
        {
            var tempc = miniPlayerMain.Template.LoadContent() as FrameworkElement;
            var cb = tempc.FindName("buttonPlay") as CheckBox;
            btnCont = (string)cb.Content;
            cb.Content = "Stop";
            debug =(string) cb.Content;
        }*/

        // This handlers set the checkBox Content to wished, but it weirdly returns to it's originall text
        // apparently the else is never reached.
        // I think it returns to it's originall content since we change the template.
        /* private void buttonPlay_Click(object sender, RoutedEventArgs e)
         {

             CheckBox playStopButton = sender as CheckBox;
             btnCont = (string)playStopButton.Content;
             if (playStopButton.Content == null)
             { playStopButton.Content = "Play"; }
             if (playStopButton.Content.Equals("Play"))
             {
                 playStopButton.Content = "Stop";
                 debug += "stop";
             }
             else
             {
                 playStopButton.Content = "Play";
                 debug += "play";
             }


         }*/

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            // This only closes the Window
            //this.Close(); 
            Application.Current.Shutdown();
        }

        private void ChangeView_Click(object sender, RoutedEventArgs e)
        {
            mw.ChangeWindow();
        }
    }
}
