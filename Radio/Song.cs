using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.IO;
using Newtonsoft.Json;
using System.Windows;
using System.Windows.Media.Imaging;
using System.Windows.Controls;
using System.Windows.Forms;

namespace Radio
{
    /// TODO: Get album image from last.fm
    /// TODO: Make scrobbling to last.fm possible.
    class Song
    {
        public string Name { get; set; }
        public string Dj { get; set; }
        public string DjId { get; private set; }
        public int Listeners { get; set; }

        private int _startTime;
        private int _endTime;
        private int _currentTime;

        public DjImage Image;

        /// <summary>
        /// CurrentTime holds the current second of the song playing.
        /// </summary>
        public string CurrentTime
        {
            get
            {
                string time = SecondsToMinutesAndSeconds(Math.Abs(_startTime - _currentTime));
                return time;
            }
            private set { _currentTime = int.Parse(value); }
        }

        /// <summary>
        /// The time when the song finishes.
        /// </summary>
        public string EndTime
        {
            get { return SecondsToMinutesAndSeconds(_endTime - _startTime); }
            private set { _endTime = int.Parse(value); }
        }

        /// <summary>
        /// The time when the song started.
        /// </summary>
        public string StartTime
        {
            get { return SecondsToMinutesAndSeconds(_startTime); }
            private set { _startTime = int.Parse(value); }
        }

        /// <summary>
        /// Length of song, stated in seconds.
        /// </summary>
        public double DoubleEndTime
        {
            get { return TotalSeconds(_endTime - _startTime); }
            private set { }
        }

        /// <summary>
        /// Current position of the second within song.
        /// </summary>
        public double DoubleCurrentTime
        {
            get { return TotalSeconds(Math.Abs(_startTime - _currentTime)); }
            private set { }
        }

        public Song()
        {
            Image = new DjImage();
        }

        /// <summary>
        /// Calculates the total seconds.
        /// </summary>
        /// <param name="Seconds"></param>
        /// <returns></returns>
        static double TotalSeconds(int Seconds)
        {
            int totalSeconds = 0;
            int secondsObtained = Seconds % 60;
            int minutesObtained = Seconds / 60;
            totalSeconds = secondsObtained + minutesObtained * 60;
            return (double)totalSeconds;
        }

        /// <summary>
        /// This function gets the data from r/a/dio api by doing an HTTP request.
        /// It updates the "important" fields od the class.
        /// </summary>
        public Task GetNewSongData()
        {
            Task t = Task.Run(() =>
            {
                try
                {
                    HttpWebRequest pageRequest = (HttpWebRequest)WebRequest.Create("https://r-a-d.io/api");
                    pageRequest.Method = "GET";

                    pageRequest.KeepAlive = false;

                    HttpWebResponse pageResponse = (HttpWebResponse)pageRequest.GetResponse();

                    Stream streamResponse = pageResponse.GetResponseStream();
                    StreamReader streamRead = new StreamReader(streamResponse);

                    string rawPage = streamRead.ReadToEnd();

                    pageResponse.Close();
                    streamResponse.Close();
                    streamRead.Close();


                    dynamic jsonData = JsonConvert.DeserializeObject(rawPage);

                    Name = jsonData.main.np;
                    Dj = jsonData.main.dj.djname;
                    Listeners = jsonData.main.listeners;
                    StartTime = jsonData.main.start_time;
                    CurrentTime = jsonData.main.current;
                    EndTime = jsonData.main.end_time;
                    DjId = jsonData.main.dj.id;
                }
                catch (Exception)
                {
                    throw;
                }
            });
            return t;
        }

        // WARNING: Remember: there's a bug when a DJ is playing, sometimes CurrentTime ends up being a lot more than
        // EndTime (since EndTime is 0:00); that will cause to call GetNewSongData() every second, which
        // is not very performance wise!
        // But since that runs on other thread it doesn't causes a glitch in the audio play.
        /// <summary>
        /// Updated the _currentTime counter, returning true if the song has finished therefore needs to update.
        /// false otherwise.
        /// </summary>
        /// <returns></returns>
        public bool ShouldUpdateSong()
        {
            ++_currentTime;
            if (_currentTime >= _endTime)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Converts Seconds (an int of the form 1456894731) to a string of minutes and seconds (in the form
        /// of MIN:SECSEC 9:06, or 4:23, for example.
        /// </summary>
        static string SecondsToMinutesAndSeconds(int Seconds)
        {
            int minutes = 0;
            int secondsObtained = Seconds % 60;
            string secondsStrObtained = secondsObtained.ToString();

            // This makes posible MM:SS, instead of MM:S (3:09, instead of 3:9)
            if (secondsObtained > -1 && secondsObtained < 10)
            {
                secondsStrObtained = String.Format("0{0}", secondsObtained);
            }

            minutes = Seconds / 60;
            string result = String.Format("{0}:{1}", minutes, secondsStrObtained);
            return result;
        }
    }
}
