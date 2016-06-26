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
        // Holds the views between song info and controls.
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

        /// <summary>
        /// Gets a reference of MainWindow to allow switching from views and for
        /// the bindings.
        /// Note that it also calls the SetBindings function.
        /// </summary>
        /// <param name="MW">Reference to MainWindow</param>
        public void SetOtherView(MainWindow MW)
        {
            mw = MW;
            SetBindings();
        }

        /// <summary>
        /// This sets the Bindings from MainWindow to the MiniPlayer.
        /// </summary>
        public void SetBindings()
        {
            // Sets the name
            Binding songBdg = new Binding();
            songBdg.Source = mw.tbSong;
            songBdg.Path = new PropertyPath("Text");
            songName.SetBinding(TextBlock.TextProperty, songBdg);

            // Sets the duration of song
            Binding durationBdg = new Binding();
            durationBdg.Source = mw.tbLastSecond;
            durationBdg.Path = new PropertyPath("Text");
            songDuration.SetBinding(TextBlock.TextProperty, durationBdg);

            // Sets the current second of the play bar
            Binding barBdg = new Binding();
            barBdg.Source = mw.pBar;
            barBdg.Path = new PropertyPath("Value");
            playedBar.SetBinding(ProgressBar.ValueProperty, barBdg);

            // Sets the maximum value of the play bar
            Binding maximumBarBdg = new Binding();
            maximumBarBdg.Source = mw.pBar;
            maximumBarBdg.Path = new PropertyPath("Maximum");
            playedBar.SetBinding(ProgressBar.MaximumProperty, maximumBarBdg);
        }

        /// <summary>
        /// Event to allows drag the mini player window.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Grid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            bool wasLeftClick = e.ChangedButton == MouseButton.Left;

            if (wasLeftClick && e.Source is Grid
                || wasLeftClick && e.OriginalSource is Visual)
            {
                this.DragMove();
            }
        }

        /// <summary>
        /// Changes to the Play/Pause, and Volume controls.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void miniPlayerMain_MouseEnter(object sender, MouseEventArgs e)
        {
            miniPlayerMain.Template = miniPlay;
        }

        /// <summary>
        /// Change to the Song info, Played bar, and duration song elements.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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

        /// <summary>
        /// Closes the application the small x button.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Close_Click(object sender, RoutedEventArgs e)
        {
            // This only closes the Window
            //this.Close(); 
            Application.Current.Shutdown();
        }

        /// <summary>
        /// Allows us to change between the MainWindow and MiniPlayer.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ChangeView_Click(object sender, RoutedEventArgs e)
        {
            mw.ChangeWindow();
        }
    }
}
