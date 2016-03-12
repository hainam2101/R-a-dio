using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

using System.Windows.Controls;

namespace Radio
{
    /// <summary>
    /// Serves to update the TextBox in the application, and also calls the update counter
    /// of the Song object (TickerAndUpdate())
    /// </summary>
    static class Updater
    {
        static bool _hasStarted = false;

        /// <summary>
        /// Calls the counter update, and checks the _hasStarted variable to run the if for the first time.
        /// </summary>
        /// <param name="Current"></param>
        /// <param name="Song"></param>
        /// <param name="DJ"></param>
        /// <param name="Listeners"></param>
        /// <param name="StartTime"></param>
        /// <param name="CurrentTime"></param>
        /// <param name="EndTime"></param>
        /// <param name="Sldr"></param>
        public static void NeedToUpdate(ref Song Current, ref TextBlock Song, ref TextBlock DJ, ref TextBlock Listeners, ref TextBlock CurrentTime, ref TextBlock EndTime, ref Slider Sldr)
        {
            if (Current.TickerAndUpdate() || !_hasStarted)
            {
                Song.Text = Current.Name;
                DJ.Text = Current.Dj;
                Listeners.Text = Current.Listeners.ToString();
                CurrentTime.Text = Current.CurrentTime;
                EndTime.Text = Current.EndTime;
                Sldr.Maximum = Current.DoubleEndTime;
                Sldr.Value = 0;
                _hasStarted = true;
            }
            else
            {
                CurrentTime.Text = Current.CurrentTime;
                Sldr.Value = Current.DoubleCurrentTime;
            }
        }
    }
}
