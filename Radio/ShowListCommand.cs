using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Windows.Input;

namespace Radio
{
    class ShowListCommand
    {
        static RoutedUICommand _showList;

        public static RoutedUICommand ShowList
        {
            get { return _showList; }
        }

        static ShowListCommand()
        {
            InputGestureCollection gesture = new InputGestureCollection();
            gesture.Add(new KeyGesture(Key.L, ModifierKeys.Control | ModifierKeys.Shift, "ShowList"));

            _showList = new RoutedUICommand("ShowList", "ShowList",
                typeof(ShowListCommand), gesture);
        }
    }
}
