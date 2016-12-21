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
     * 1. If we go from the first button to the third (or second), both remain selected (bold). (I think is due the checking on the list .Count() method).
     *    This happens only in the second or following instances of the song list window, so prolly is due init variable?
     * 2. If we close the songlist view, and open it again, the list isn't showed.
     *    The bugs above are solved by the assignments to default for init and CurrentNumber variables in GetNewButtonList method.
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
        static int _lastID;
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

        static Page()
        {

        }

        public Page(ItemsControl currPageList)
        {
            _controlList = currPageList;
            // Used to populate the default index
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

        static public List<Page> GetNewButtonList(ItemsControl currPageList)
        {
            init = false;
            CurrentNumber = 1;
            var items = new List<Page>();

            var pagesqt = Database.NumberOfPages(Updater.DBConnection).Result;
            for (int i = 0; i < pagesqt; ++i)
            {
                items.Add(new Page(currPageList) { Number = i + 1, IsSelected = (i == 0) ? true : false });
            }
            _currentList = items;
            return items;
        }

        #endregion // Public Methdos

        #region Private Methods

        static void ChangePage_Execute(object id)
        {
            int val = (int)id;

            // Allows change of page only if there's more than 2 pagination buttons
            if (init /*&& _currentList.Count() > 2*/)
            {
            /*{
                _lastID = 1;
            }
            else
            {
                if (_currentList.Count() > 2)
                {*/
                    /*_currentList[0].Number = 10;
                    _currentList[1].Number = 20;
                    _currentList[0].OnPropertyChanged("Number");
                    _currentList[1].OnPropertyChanged("Number");
                    MessageBox.Show("Commit changes");*/
                    _currentList[CurrentNumber - 1].IsSelected = false;
                    _currentList[CurrentNumber - 1].OnPropertyChanged("Selected");
                    _currentList[val - 1].IsSelected = true;
                    _currentList[val - 1].OnPropertyChanged("Selected");

                    CurrentNumber = val;
                //}
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
