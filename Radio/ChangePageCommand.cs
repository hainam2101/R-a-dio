using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Windows.Input;

namespace Radio
{
    class ChangePageCommand
    {

        static RoutedUICommand _changePage;

        public static RoutedUICommand ChangePage
        {
            get { return _changePage; }
        }

        static ChangePageCommand()
        {
            InputGestureCollection gesture = new InputGestureCollection();
            gesture.Add(new KeyGesture(Key.Space, ModifierKeys.None, "Space"));
            gesture.Add(new KeyGesture(Key.MediaPlayPause, ModifierKeys.None, "PlayPause"));

            //_playOrStop = new RoutedUICommand("PlayOrStop", "PlayOrStop", typeof(PlayOrStopCommand), gesture);
        }
    }
}
