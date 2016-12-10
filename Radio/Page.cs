using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Windows.Input;
using System.Windows.Forms;
using System.Windows.Controls;

namespace Radio
{
    class Page
    {

        static RelayCommand _changePage;
        static ItemsControl _controlList;

        public int Number { get; set; }
        public bool IsSelected { get; set; }
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

        static public int CurrentNumber { get; set; } = 1;

        static Page()
        {
            
        }

        public Page(ItemsControl list)
        {
            _controlList = list;
            // Used to populate the default index
            ChangePage_Execute(CurrentNumber);
        }

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

        static void ChangePage_Execute(object id)
        {
            int val = (int)id;
            /*var msg = String.Format("Page to go is: {0}.", val.ToString());
            MessageBox.Show(msg);*/
            _controlList.ItemsSource = Database.GetRangeOfRecords(val, Updater.DBConnection).Result;
        }

        static bool ChangePage_CanExecute()
        {
            // TODO: Change this logic.
            return true;
        }

        #endregion // Commands

    }
}
