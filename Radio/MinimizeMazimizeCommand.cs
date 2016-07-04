using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Windows.Input;

namespace Radio
{
    class MinimizeMazimizeCommand
    {
        static RoutedUICommand _minimizeOrMaximize;

        public static RoutedUICommand MinimizeOrMaximize
        {
            get { return _minimizeOrMaximize; }
        }

        static MinimizeMazimizeCommand()
        {
            InputGestureCollection gesture = new InputGestureCollection();
            gesture.Add(new KeyGesture(Key.M, ModifierKeys.Control | ModifierKeys.Shift, "MinimizeMaximize"));
            //gesture.Add(new KeyGesture(Key.M, ModifierKeys.None, "MinimizeMaximize"));

            _minimizeOrMaximize = new RoutedUICommand("MinimizeMaximize", "MinimizeMaximize",
                typeof(MinimizeMazimizeCommand), gesture);
        }
    }
}
