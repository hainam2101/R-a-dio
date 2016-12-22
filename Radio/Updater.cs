using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Controls;

using System.Data.SQLite;

namespace Radio
{
    /// <summary>
    /// Serves to update the TextBoxes in the application.
    /// </summary>
    static class Updater
    {
        static bool _hasStarted = false;
        static AsyncException<Exception> errorWhileFetching = new AsyncException<Exception>();

        public static string Favorite = "Favorite";
        public static string NoFavorite = "NoFavorite";

        /// <summary>
        /// The current DBConnection, check for null before using.
        /// </summary>
        public static SQLiteConnection DBConnection;

        static Updater()
        {
            if (Database.ExistsDB())
            {
                // TODO: Fix this error.
                /* The following line causes a clash error, it does after in deployment (when the app in in /Program Files/ folder.
                 *  I think is due the DB (favorites.sql) not being with write (and modify) permissions.
                 *  The funny thing is that it doesn't throws any exceptios, it just crashes.
                 *  Just to be more exact, the problem is in Database.CreateDBConnection method.
                 */
                ConnectToDB();
            }
        }

        /// <summary>
        /// Checks if the songs has finished to update the showed data, if not, just update the slider.
        /// Note: The _hasStarted var serves to allow to fetch the data for the first time (aka, when running the app).
        /// </summary>
        /// <param name="Current"></param>
        /// <param name="Song"></param>
        /// <param name="DJ"></param>
        /// <param name="Listeners"></param>
        /// <param name="StartTime"></param>
        /// <param name="CurrentTime"></param>
        /// <param name="EndTime"></param>
        /// <param name="Sldr"></param>
        public static async void NeedToUpdate(Song Current, TextBlock Song, TextBlock DJ/*, TextBlock Listeners*/,
            TextBlock CurrentTime, TextBlock EndTime, System.Windows.Controls.ProgressBar Sldr, Image Img, Timer timer,
            System.Windows.Controls.CheckBox songFavorite)
        {

            if (Current.ShouldUpdateSong() || !_hasStarted)
            {
                await Current.GetNewSongData(errorWhileFetching);
                if (errorWhileFetching.WasRaised)
                {
                    timer.Stop();
                    DialogResult response;
                    response = MessageBox.Show(errorWhileFetching.Error.Message + "\nDo you wanna retry?", "Error", MessageBoxButtons.YesNo, MessageBoxIcon.Error);
                    if (response == DialogResult.Yes)
                    {
                        timer.Start();
                    }
                    errorWhileFetching.WasRaised = false;
                }
                Song.Text = Current.Name;
                DJ.Text = Current.Dj;
                //Listeners.Text = Current.Listeners.ToString();
                CurrentTime.Text = Current.CurrentTime;
                EndTime.Text = Current.EndTime;
                Sldr.Maximum = Current.DoubleEndTime;
                Sldr.Value = 0;
                Current.Image.IsNewDjPlaying(Current.DjId);
                Current.Image.LoadNewImage();
                Img.Source = Current.Image.Image;
                _hasStarted = true;

                if (DBConnection != null && await Database.ExistsRecordAndIsFavoriteAsync(Song.Text, DBConnection))
                {
                    songFavorite.Content = Favorite;
                }
                else
                {
                    songFavorite.Content = NoFavorite;
                }

            }
            else
            {
                CurrentTime.Text = Current.CurrentTime;
                Sldr.Value = Current.DoubleCurrentTime;
            }
        }

        public static void ConnectToDB()
        {
            /*  Update about th DB permissions error:
             *  So with the catch we found that there's some DLLs that aren't included in the deploy for some reasons,
             *  those are the SQLite interops (both for X64 and X86). See: http://puu.sh/sWVx7/064a79ff07.png
             *  Also, finally found that we don't have write permissions for the DB. See: http://puu.sh/sWW0P/20e7d7793f.png
             *   */
            try
            {
                DBConnection = Database.CreateDBConnection();
                DBConnection.Open();
#if DEBUG
                MessageBox.Show("No Exceptions, connection opened");
#endif
            }
            catch (Exception err)
            {
                MessageBox.Show("Exception found, can't connect to DB.\n" + err.Message);
            }
            /*DBConnection = Database.CreateDBConnection();
            DBConnection.Open();*/
        }
    }
}
