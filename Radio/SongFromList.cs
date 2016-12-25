using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Windows;
using System.Windows.Input;
using System.ComponentModel;

namespace Radio
{
    public class SongFromList : INotifyPropertyChanged
    {

        static RelayCommand _deleteSong;
        static RelayCommand _toggleFavoriteSong;

        public bool EmptyFavorites;
        static List<SongFromList> _currentList;
        public event PropertyChangedEventHandler PropertyChanged;

        #region Public Properties

        public string Name { get; set; }
        public int ID { get; set; }
        public bool IsFavorite { get; set; }
        // Allows binding for a trigger
        public string Favorite { get { return IsFavorite.ToString(); } }

        // Properties

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

        static public List<SongFromList> CurrentList
        {
            get { return _currentList; }
            set { _currentList = value; }
        }

        #endregion // Public Properties

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

        #region Private Methods

        static void DeleteSong(object id)
        {
            int val = (int)id;
            /*
            var msg = String.Format("DeleteSong command, id is: {0}", val.ToString());
            MessageBox.Show(msg);*/
            Database.DeleteRecordAsync(val, Updater.DBConnection);
        }

        /// <summary>
        /// Toggles the favorite of the song specified by id. Note that it propagates the changes to the UI.
        /// </summary>
        /// <param name="id">A non-negative number, here we don't check for an in range int.</param>
        static void ToggleFavorite(object id)
        {
            int val = (int)id;

            Database.ToggleFavoriteByIDAsync(val, Updater.DBConnection);

            // Used to Notify the changes in the favorites list view.
            var itemToggled = (from itm in CurrentList where itm.ID == val select itm).ToList();

            if (itemToggled.Count() > 0)
            {
                //MessageBox.Show("Favorite toggled for id: " + itemToggled[0].ID);
                itemToggled[0].IsFavorite = !(itemToggled[0].IsFavorite);
                itemToggled[0].OnPropertyChanged("Favorite");
            }
        }

        static bool CanExecute()
        {
            return (Updater.DBConnection == null) ? false : true;
        }

        protected void OnPropertyChanged(string name)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(name));
            }
        }

        #endregion // Private Methods

    }
}
