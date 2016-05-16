using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Radio
{
    class PlayOrStopCommand
    {
        static RoutedUICommand _playOrStop;

        public static RoutedUICommand PlayOrStop
        {
            get { return _playOrStop; }
        }

        static PlayOrStopCommand()
        {
            InputGestureCollection gesture = new InputGestureCollection();
            gesture.Add(new KeyGesture(Key.Space, ModifierKeys.None, "Space"));
            gesture.Add(new KeyGesture(Key.MediaPlayPause, ModifierKeys.None, "PlayPause"));

            _playOrStop = new RoutedUICommand("PlayOrStop", "PlayOrStop", typeof(PlayOrStopCommand), gesture);
        }
    }
}
