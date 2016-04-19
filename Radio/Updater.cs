using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using System.Threading;
using System.Windows.Forms;
using System.Windows.Controls;

namespace Radio
{
    /// <summary>
    /// Serves to update the TextBoxes in the application.
    /// </summary>
    static class Updater
    {
        static bool _hasStarted = false;
        /// <summary>
        /// Checks if the songs has finished to update the showed data, if not, just update the slider.
        /// Note: The _hasStarted var serves to allow to fetch the data for the first time (aka, when running the app).
        /// </summary>
        /// <param name="Current"></param>
        /// <param name="Song"></param>
        /// <param name="DJ"></param>
        /// <param name="Listeners"></param>
        /// <param name="StartTime"></param>
        /// <param name="CurrentTime"></param>
        /// <param name="EndTime"></param>
        /// <param name="Sldr"></param>
        public static async void NeedToUpdate(Song Current, TextBlock Song, TextBlock DJ, TextBlock Listeners, TextBlock CurrentTime, TextBlock EndTime, Slider Sldr, Image Img, Timer timer, HandleException err)
        {   
            if (Current.ShouldUpdateSong() /*|| !_hasStarted*/)
            {

                await Current.GetNewSongData(err);
                
                Song.Text = Current.Name;
                DJ.Text = Current.Dj;
                Listeners.Text = Current.Listeners.ToString();
                CurrentTime.Text = Current.CurrentTime;
                EndTime.Text = Current.EndTime;
                Sldr.Maximum = Current.DoubleEndTime;
                Sldr.Value = 0;
                Current.Image.IsNewDjPlaying(Current.DjId);
                Current.Image.LoadNewImage();
                Img.Source = Current.Image.Image;
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
