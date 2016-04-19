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

using System.Net;
using System.Windows.Forms;

namespace Radio
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            RadioUpdater();
        }

        HandleException myerr;
        
        Player StreamMp3 = new Player("https://stream.r-a-d.io/main.mp3");

        void RadioUpdater()
        {
            myerr += NoInternetWebException_Handler;
            Timer t = new Timer();
            t.Interval = (int)Player.TickMode.NormalMode;
            Song playingNow = new Song();
            try
            {
                /*Updater.NeedToUpdate(playingNow, textBlockSongValue,
                     textBlockDJValue, textBlockListenersValue,
                     textBlockCurrentTimeValue, textBlockEndTimeSecondsValue, slider, image, t, myerr);*/

            t.Tick += new EventHandler((sender, e) => Updater.NeedToUpdate(playingNow, textBlockSongValue,
                     textBlockDJValue, textBlockListenersValue,
                     textBlockCurrentTimeValue, textBlockEndTimeSecondsValue, slider, image, t, myerr));
           
                t.Start();
           }
           catch (Exception)
           {
                System.Windows.MessageBox.Show("Couldn't connect to server", "WebException", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void buttonPlay_Click(object sender, RoutedEventArgs e)
        {
            StreamMp3.buttonPlay_Click(sender, e);
        }

        private void buttonStop_Click(object sender, RoutedEventArgs e)
        {
            StreamMp3.buttonStop_Click(sender, e);
        }

        private void NoInternetWebException_Handler()
        {
            System.Windows.MessageBox.Show("Couldn't connect to server", "WebException", MessageBoxButton.OK, MessageBoxImage.Error);
        }

    }
}
