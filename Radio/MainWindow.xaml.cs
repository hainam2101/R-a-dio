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
using System.Data.SQLite;

using System.Diagnostics;

namespace Radio
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        bool isMainShowed;
        bool isPlaying;
        bool isFavorite;
        bool existsDB;

        SQLiteConnection DBConn;

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

            // Command for the favorite button
            favOrUnfavSong.Command = FavoriteCommand.Favorite;
            CommandBinding bindingFav = new CommandBinding();
            bindingFav.Command = FavoriteCommand.Favorite;
            bindingFav.Executed += Favorite_Execute;
            bindingFav.CanExecute += Favorite_CanExecute;
            CommandBindings.Add(bindingFav);

            RadioUpdater();
        }

        

        void RadioUpdater()
        {
            mp = new MiniPlayer();

            // Pass this window
            mp.SetOtherView(this);

            // Database exists?
            if (Database.ExistsDB())
            {
                existsDB = true;   
            }
            else
            {
                // Ask the user to create one: I think the creating dialog should be done when the user press the favorite button.
                /*Database.CreateDBFileAndTable();
                existsDB = true;*/
                existsDB = false;
            }

            System.Windows.Forms.MessageBox.Show(Database.CurrentPath);

            if (existsDB)
            {
                DBConn = Database.CreateDBConnection();
                DBConn.Open();
            }

            Timer t = new Timer();
            t.Interval = (int) Player.TickMode.NormalMode;
            Song playingNow = new Song();

            Updater.NeedToUpdate(playingNow, tbSong,
                     tbDJName/*, textBlockListenersValue*/,
                     tbCurrentSecond, tbLastSecond, pBar, imgDJ, t, favOrUnfavSong);

            t.Tick += new EventHandler((sender, e) => Updater.NeedToUpdate(playingNow, tbSong,
                     tbDJName/*, textBlockListenersValue*/,
                     tbCurrentSecond, tbLastSecond, pBar, imgDJ, t, favOrUnfavSong));
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

        public void Favorite_Execute(object sender, ExecutedRoutedEventArgs args)
        {
            if (Updater.DBConnection == null)
            {
                // Currently setting one silently.
                if (!Database.ExistsDB())
                {
                    Database.CreateDBFileAndTable();
                }
                
                Updater.ConnectToDB();
                //return; // Here we will ask for the user to create a DB or create one silently.
            }

            if (Database.ExistsRecord(tbSong.Text, Updater.DBConnection))
            {
                if (Database.ExistsRecordAndIsFavorite(tbSong.Text, Updater.DBConnection))
                {
                    Database.UpdateRecord(tbSong.Text, false, Updater.DBConnection);
                    favOrUnfavSong.Content = Updater.NoFavorite;

                    Debug.Print("ExistsRecordAndIsFavorite make Favorite to false");
                }
                else
                {
                    Database.UpdateRecord(tbSong.Text, true, Updater.DBConnection);
                    favOrUnfavSong.Content = Updater.Favorite;

                    Debug.Print("ExistsRecord and is not favorite, make it favorite");
                }
            }
            else
            {
                Database.InsertRecord(tbSong.Text, Updater.DBConnection);
                favOrUnfavSong.Content = Updater.Favorite;

                Debug.Print("Record doesn't exists, creating one.");
            }
        }

        //System.Windows.Forms.MessageBox.Show("Added to favorites!"); // Dummy placeholder
        /*if (isFavorite)
        {
            favOrUnfavSong.Content = Updater.NoFavorite;
        }
        else
        {
            favOrUnfavSong.Content = Updater.Favorite;
        }

        isFavorite = !isFavorite;*/

        public void Favorite_CanExecute(object sender, CanExecuteRoutedEventArgs args)
        {
            args.CanExecute = true;
        }
    }
}
