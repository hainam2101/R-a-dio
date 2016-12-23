using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Windows;
using System.Windows.Input;

namespace Radio
{
    public class SongFromList
    {

        static RelayCommand _deleteSong;
        static RelayCommand _toggleFavoriteSong;
        public bool EmptyFavorites;

        public string Name { get; set; }
        public int ID { get; set; }
        public bool IsFavorite { get; set; }
        // Allows binding for a trigger
        public string Favorite { get { return IsFavorite.ToString(); } }
        // Binds to the song's buttons to hide them in the case of showing the default message ("No favorites yet".)
        public string IsVisible
        {
            get
            {
                if (EmptyFavorites)
                {
                    return "Collapsed";
                }
                else
                {
                    return "Visible";
                }
            }
        }

        #region Commands

        static public ICommand DeleteCommand
        {
            get
            {
                if (_deleteSong == null)
                {
                    _deleteSong = new RelayCommand(
                        param => DeleteSong(param),
                        param => CanExecute()
                        );
                }

                return _deleteSong;
            }
        }

        static public ICommand FavoriteCommand
        {
            get
            {
                if (_toggleFavoriteSong == null)
                {
                    _toggleFavoriteSong = new RelayCommand(
                        param => ToggleFavorite(param),
                        param => CanExecute()
                        );
                }

                return _toggleFavoriteSong;
            }
        }

        #endregion // Commands

        static void DeleteSong(object id)
        {
            int val = (int)id;
            /*
            var msg = String.Format("DeleteSong command, id is: {0}", val.ToString());
            MessageBox.Show(msg);*/
            Database.DeleteRecordAsync(val, Updater.DBConnection);
        }

        static void ToggleFavorite(object id)
        {
            int val = (int)id;
            var msg = String.Format("ToggleFavorite command, id is: {0}", val.ToString());
            MessageBox.Show(msg);
        }

        static bool CanExecute()
        {
            return (Updater.DBConnection == null) ? false : true;
        }

    }
}
