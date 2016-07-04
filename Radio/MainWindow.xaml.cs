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
        bool isMainShowed;
        bool isPlaying;
        Player StreamMp3 = new Player("https://stream.r-a-d.io/main.mp3");

        public static MiniPlayer mp;
        public MainWindow()
        {
            InitializeComponent();

            // Adds the event handler to close all the windows from MainWindow
            this.Closing += new System.ComponentModel.CancelEventHandler(closeApp);

            isMainShowed = true;

            // Initialize and bind the command for Play/Stop
            buttonPlay.Command = PlayOrStopCommand.PlayOrStop;
            CommandBinding binding = new CommandBinding();
            binding.Command = PlayOrStopCommand.PlayOrStop;
            binding.Executed += PlayOrStop_Execute;
            binding.CanExecute += PlayOrStop_CanExecute;
            CommandBindings.Add(binding);

            // Command for the Minimize button
            showMPlayer.Command = MinimizeMazimizeCommand.MinimizeOrMaximize;
            CommandBinding bindingView = new CommandBinding();
            bindingView.Command = MinimizeMazimizeCommand.MinimizeOrMaximize;
            bindingView.Executed += MinimizeOrMaximize_Execute;
            bindingView.CanExecute += MinimizeOrMaximize_CanExecute;
            CommandBindings.Add(bindingView);

            RadioUpdater();
        }

        

        void RadioUpdater()
        {
            mp = new MiniPlayer();

            // Pass this window
            mp.SetOtherView(this);

            Timer t = new Timer();
            t.Interval = (int) Player.TickMode.NormalMode;
            Song playingNow = new Song();

            Updater.NeedToUpdate(playingNow, tbSong,
                     tbDJName/*, textBlockListenersValue*/,
                     tbCurrentSecond, tbLastSecond, pBar, imgDJ, t);

            t.Tick += new EventHandler((sender, e) => Updater.NeedToUpdate(playingNow, tbSong,
                     tbDJName/*, textBlockListenersValue*/,
                     tbCurrentSecond, tbLastSecond, pBar, imgDJ, t));
            t.Start();
        }

        public void PlayOrStop_Execute(object sender, ExecutedRoutedEventArgs args)
        {
            if (isPlaying)
            {
                StreamMp3.buttonStop_Click(sender, args);
                buttonPlay.Content = "Play";
                isPlaying = false;
            }
            else
            {
                StreamMp3.buttonPlay_Click(sender, args);
                buttonPlay.Content = "Stop";
                isPlaying = true;
            }
        }

        public void PlayOrStop_CanExecute(object sender, CanExecuteRoutedEventArgs args)
        {
            /// TODO: Add two things:
            /// 1. Execute this command only when: internet are on and server is online.
            /// 2. Make sure this can run only few times in a short time span; this is because
            /// we can leave the Space button pressed and therefore calling the command a lot
            /// of times every second. (Now I've actually tested the second one by leaving pressed
            /// the space bar and it isn't that much of a big problem, we could leave it there)
            args.CanExecute = true;
        }

        private void sldrVolume_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            StreamMp3.ChangeVolume((float)sldrVolume.Value);
        }

        public void ChangeWindow()
        {
            if (isMainShowed)
            {
                this.Hide();
                mp.Show();
                isMainShowed = false;
            }
            else
            {
                mp.Hide();
                this.Show();
                isMainShowed = true;
            }
        }

        void closeApp(object sender, System.ComponentModel.CancelEventArgs e)
        {
            System.Windows.Application.Current.Shutdown();
        }

        public void MinimizeOrMaximize_Execute(object sender, ExecutedRoutedEventArgs args)
        {
            ChangeWindow();
        }

        public void MinimizeOrMaximize_CanExecute(object sender, CanExecuteRoutedEventArgs args)
        {
            args.CanExecute = true;
        }
    }
}
