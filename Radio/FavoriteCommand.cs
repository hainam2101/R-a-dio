using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Windows.Input;

namespace Radio
{
    class FavoriteCommand
    {
        static RoutedUICommand _favorite;

        public static RoutedUICommand Favorite
        {
            get { return _favorite; }
        }

        static FavoriteCommand()
        {
            InputGestureCollection gesture = new InputGestureCollection();
            gesture.Add(new KeyGesture(Key.F, ModifierKeys.Control | ModifierKeys.Shift, "Favorite"));

            _favorite = new RoutedUICommand("Favorite", "Favorite",
                typeof(FavoriteCommand), gesture);
        }
    }
}
