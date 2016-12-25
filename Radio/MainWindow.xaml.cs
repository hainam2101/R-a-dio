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

using System.ComponentModel;
using System.Windows.Forms;
using System.Data.SQLite;

using System.Security.Permissions;

namespace Radio
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        bool isMainShowed;
        bool isPlaying;

        bool isListShowed;

        SQLiteConnection DBConn; // TODO: Unused

        Player StreamMp3 = new Player("https://stream.r-a-d.io/main.mp3");
        SongList _favoritesList;

        public static MiniPlayer mp;
        public MainWindow()
        {
            InitializeComponent();
#if DEBUG
            ShowDBPath();
            CheckDbPermissions();
#endif


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

            // Command for the show list button
            showSongList.Command = ShowListCommand.ShowList;
            CommandBinding bindingShowList = new CommandBinding();
            bindingShowList.Command = ShowListCommand.ShowList;
            bindingShowList.Executed += ShowList_Execute;
            bindingShowList.CanExecute += ShowList_CanExecute;
            CommandBindings.Add(bindingShowList);

            /*_favoritesList = new SongList();
            _favoritesList.Closing += ToggleSongList;*/

            RadioUpdater();
        }

        void RadioUpdater()
        {
            mp = new MiniPlayer();

            /*var list = new SongList();
            list.Show();*/

            // Pass this window
            mp.SetOtherView(this);

            Timer t = new Timer();
            t.Interval = (int)Player.TickMode.NormalMode;
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

        // Also: Do a heavy cleaning of the Database class and here.
        public async void Favorite_Execute(object sender, ExecutedRoutedEventArgs args)
        {
            if (Updater.DBConnection == null)
            {
                // Currently setting one silently.
                if (!Database.ExistsDB())
                {
                    await Database.CreateDBFileAndTableAsync();
                }

                Updater.ConnectToDB();
                //return; // Here we will ask for the user to create a DB or create one silently.
            }

            if (await Database.ExistsRecordAsync(tbSong.Text, Updater.DBConnection))
            {
                if (await Database.ExistsRecordAndIsFavoriteAsync(tbSong.Text, Updater.DBConnection))
                {
                    await Database.UpdateRecordAsync(tbSong.Text, false, Updater.DBConnection);
                    favOrUnfavSong.Content = Updater.NoFavorite;
                }
                else
                {
                    await Database.UpdateRecordAsync(tbSong.Text, true, Updater.DBConnection);
                    favOrUnfavSong.Content = Updater.Favorite;
                }
            }
            else
            {
                await Database.InsertRecordAsync(tbSong.Text, Updater.DBConnection);
                favOrUnfavSong.Content = Updater.Favorite;
            }
        }

        public void Favorite_CanExecute(object sender, CanExecuteRoutedEventArgs args)
        {
            args.CanExecute = true;
        }

        // Original, working

        public void ShowList_Execute(object sender, ExecutedRoutedEventArgs args)
        {
            var list = new SongList();
            list.Show();
            isListShowed = true;
            list.Closing += ToggleSongList;
        }
        // new approach to avoid creating a songlist everytime
        /*public void ShowList_Execute(object sender, ExecutedRoutedEventArgs args)
        {
            _favoritesList.CreatePaginationAsync();
            _favoritesList.Show();
            isListShowed = true;
        }*/

        /*public async void ShowList_Execute(object sender, ExecutedRoutedEventArgs args)
        {
            this.Dispatcher.Invoke(() =>
            {
                var list = new SongList();
                list.Show();
                isListShowed = true;
                list.Closing += ToggleSongList;
            });

            TaskCompletionSource<bool?> completion = new TaskCompletionSource<bool?>();
            this.Dispatcher.BeginInvoke(new Action(() => completion.SetResult(wrapper())));
            bool? result = await completion.Task; ;
        }*/

        /*public void ShowList_Execute(object sender, ExecutedRoutedEventArgs args)
        {


            _favoritesList.Show();
            isListShowed = true;
            _favoritesList.Closing += ToggleSongList;
        }*/

        bool wrapper()
        {
            var list = new SongList();
            list.Show();
            isListShowed = true;
            list.Closing += ToggleSongList;
            return true;
        }

        public async void ShowList_Executes(object sender, ExecutedRoutedEventArgs args)
        {
            /*var list = new SongList();
            list.Show();
            isListShowed = true;
            list.Closing += ToggleSongList;*/
            // TODO: This causes a heavy blocking in the UI thread.
            //SongList list = null;
            List<Page> list = null;
            Page._controlList = _favoritesList.currPageList;

            /*Task t = Task.Run(() =>
            {
                //list = new SongList();
                //_favoritesList.CreatePagination();
                list = Page.GetNewButtonList(_favoritesList.currPageList);
                //System.Windows.Forms.MessageBox.Show("Running task");
            });*/

            /*this.Dispatcher.Invoke(() =>
            {
                try
                {
                    list = Page.GetNewButtonList();
                }
                catch (Exception exc)
                {
                    System.Windows.Forms.MessageBox.Show("Exception raised: " + exc.Message);
                }
            });*/
                Task t = Task.Run(() =>
                {
                    //list = new SongList();
                    //_favoritesList.CreatePagination();
                    //list = Page.GetNewButtonList(_favoritesList.currPageList);
                    try
                    {
                        list = Page.GetNewButtonList();
                    }
                    catch (Exception exc)
                    {
                        System.Windows.Forms.MessageBox.Show("Exception raised: " + exc.Message);
                    }
                    //System.Windows.Forms.MessageBox.Show("Running task");
                });
                await t;


            //            await t;

            if (list != null)
            {
                _favoritesList.PaginationNumber.ItemsSource = list;
                _favoritesList.Show();
                isListShowed = true;
                _favoritesList.Closing += ToggleSongList;
            }


            //System.Windows.Forms.MessageBox.Show("Running task if");

            /*if (list != null)
            {
                list.Show();
                isListShowed = true;
                list.Closing += ToggleSongList;
                System.Windows.Forms.MessageBox.Show("Running task if");
            }*/

            //System.Windows.Forms.MessageBox.Show("Running task");

            //t.Wait();
            /*return t;*/
        }

        async void ShowFavoriteList()
        {

        }

        public void ShowList_CanExecute(object sender, CanExecuteRoutedEventArgs args)
        {
            //args.CanExecute = !isListShowed;
            args.CanExecute = !isListShowed && Updater.DBConnection != null;
        }

        void ToggleSongList(object sender, CancelEventArgs args)
        {
            isListShowed = false;
        }

        #region Debug Methods

        void ShowDBPath()
        {
            System.Windows.Forms.MessageBox.Show(Database._fullDBPath);
        }

        void CheckDbPermissions()
        {
            try
            {
                System.Windows.Forms.MessageBox.Show("Checking DB permissions...");
                new FileIOPermission(FileIOPermissionAccess.Read, Database._fullDBPath).Demand();
                System.Windows.Forms.MessageBox.Show("Permissions checked...");
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);
            }
        }

        #endregion // Debug Methods

    }
}
