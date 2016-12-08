using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Windows;
using System.Windows.Input;

namespace Radio
{
    public class SongPOCO
    {

        RelayCommand _deleteSong;
        RelayCommand _toggleFavoriteSong;

        public string Name { get; set; }
        public int ID { get; set; }
        public bool IsFavorite { get; set; }
        // Allows binding for a trigger
        public string Favorite { get { return IsFavorite.ToString(); } }


        #region Commands

        public ICommand DeleteCommand
        {
            get
            {
                if (_deleteSong == null)
                {
                    _deleteSong = new RelayCommand(
                        param => this.DeleteSong(param),
                        param => this.CanExecute()
                        );
                }

                return _deleteSong;
            }
        }

        public ICommand FavoriteCommand
        {
            get
            {
                if (_toggleFavoriteSong == null)
                {
                    _toggleFavoriteSong = new RelayCommand(
                        param => this.ToggleFavorite(param),
                        param => this.CanExecute()
                        );
                }

                return _toggleFavoriteSong;
            }
        }

        #endregion // Commands

        void DeleteSong(object id)
        {
            int val = (int)id;
            /*
            var msg = String.Format("DeleteSong command, id is: {0}", val.ToString());
            MessageBox.Show(msg);*/
            Database.DeleteRecordAsync(val, Updater.DBConnection);
        }

        void ToggleFavorite(object id)
        {
            int val = (int)id;
            var msg = String.Format("ToggleFavorite command, id is: {0}", val.ToString());
            MessageBox.Show(msg);
        }

        bool CanExecute()
        {
            return (Updater.DBConnection == null) ? false : true;
        }

    }
}
