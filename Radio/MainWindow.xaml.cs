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

        Player StreamMp3 = new Player("https://stream.r-a-d.io/main.mp3");

        void RadioUpdater()
        {
            

            Timer t = new Timer();
            t.Interval = 1000;
            Song playingNow = new Song();
            playingNow.GetDatafromApi();

            // Call it here, otherwise it will take 1 more second to show data because Tick.
            Updater.NeedToUpdate(ref playingNow, ref textBlockSongValue,
                    ref textBlockDJValue, ref textBlockListenersValue,
                    ref textBlockCurrentTimeValue, ref textBlockEndTimeSecondsValue, ref slider);

            t.Tick += new EventHandler((sender, e) => Updater.NeedToUpdate(ref playingNow, ref textBlockSongValue,
                    ref textBlockDJValue, ref textBlockListenersValue,
                    ref textBlockCurrentTimeValue, ref textBlockEndTimeSecondsValue, ref slider));
            t.Start();
        }

        private void buttonPlay_Click(object sender, RoutedEventArgs e)
        {
            StreamMp3.buttonPlay_Click(sender, e);
        }

        private void buttonStop_Click(object sender, RoutedEventArgs e)
        {
            StreamMp3.buttonStop_Click(sender, e);
        }
    }
}
