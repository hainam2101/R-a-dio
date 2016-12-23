using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Windows.Input;
using System.Windows.Forms;
using System.Windows.Controls;
using System.ComponentModel;

namespace Radio
{
    /* TODO:
     * Knowed bugs:
     * Fix:
     * This awful codebase.
     */
    class Page : INotifyPropertyChanged
    {

        #region Static Private Members

        static RelayCommand _changePage;
        static ItemsControl _controlList;

        static bool init;
        static List<Page> _currentList;
        /// <summary>
        /// The page requested.
        /// </summary>
        static int CurrentNumber { get; set; } = 1;

        #endregion // Static Private Members

        public event PropertyChangedEventHandler PropertyChanged;

        #region Public Properties

        public int Number { get; set; }
        public bool IsSelected { get; set; }
        // For binding the FontWeight property and let know the User is the button currently selected.
        public string Selected
        {
            get
            {
                if (IsSelected)
                {
                    return "Bold";
                }
                else
                {
                    return "Normal";
                }
            }
        }

        #endregion // Public Properties

        #region Constructors

        /// <summary>
        /// This constructor initializes the first page (the default one).
        /// </summary>
        public Page()
        {
            if (CurrentNumber == 1 && !init)
            {
                ChangePage_Execute(CurrentNumber);
                init = true;
            }
        }

        #endregion // Constructors

        #region Commands

        static public ICommand ChangePage
        {
            get
            {
                if (_changePage == null)
                {
                    _changePage = new RelayCommand(
                        param => ChangePage_Execute(param),
                        param => ChangePage_CanExecute()
                        );
                }

                return _changePage;
            }
        }

        #endregion // Commands

        #region Public Methods

        /// <summary>
        /// Gets the quantity of buttons based on the number of pages of songs favorites.
        /// Note that new Page()... initializes the list for the first page. (See constructor).
        /// </summary>
        /// <param name="currPageList"></param>
        /// <returns></returns>
        static public List<Page> GetNewButtonList(ItemsControl currPageList)
        {
            _controlList = currPageList;
            init = false;
            CurrentNumber = 1;
            var items = new List<Page>();

            var pagesqt = Database.NumberOfPages(Updater.DBConnection).Result;
            for (int i = 0; i < pagesqt; ++i)
            {
                items.Add(new Page() { Number = i + 1, IsSelected = (i == 0) ? true : false });
            }
            _currentList = items;

            if (_currentList.Count() == 0)
            {
                var emptyListMessage = new List<SongFromList>();
                emptyListMessage.Add(new SongFromList() { Name = "No favorites yet.", EmptyFavorites = true });
                _controlList.ItemsSource = emptyListMessage;
                //_controlList.
            }

            return items;
        }

        #endregion // Public Methdos

        #region Private Methods

        /// <summary>
        /// Sets the list of favorite songs with the page selected.
        /// </summary>
        /// <param name="id">The page number, a non negative number.</param>
        static void ChangePage_Execute(object id)
        {
            int val = (int)id;

            if (init)
            {
                    _currentList[CurrentNumber - 1].IsSelected = false;
                    _currentList[CurrentNumber - 1].OnPropertyChanged("Selected");
                    _currentList[val - 1].IsSelected = true;
                    _currentList[val - 1].OnPropertyChanged("Selected");

                    CurrentNumber = val;
            }

            _controlList.ItemsSource = Database.GetRangeOfRecords(val, Updater.DBConnection).Result;
        }

        static bool ChangePage_CanExecute()
        {
            // TODO: Change this logic.
            return true;
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
