using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Windows.Media.Imaging;
namespace Radio
{
    class DjImage
    {

        // Fields
        /// TODO: Field to save image.
        BitmapImage _image;
        bool _loadImage;
        /// TODO: Http Connection, maybe use a delegate or event, and the timer in Songs.cs should control
        /// when to fetch the image again. Optimization wise: If the new Dj name fetched is the same as the
        /// already Dj name, there's no need to fetch again the image.

        // Constructors
        public DjImage() {}

        // Properties
        /// <summary>
        /// Dj's Id
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Returns the BitmapImage
        /// </summary>
        public BitmapImage Image
        {
            get { return _image; }
        }

        /// <summary>
        /// True if need to download a new image, false otherwise.
        /// </summary>
        private bool NeedToDownloadImage
        {
            get { return _loadImage; }
        }


        // Methods
        /// <summary>
        /// Compares the current Dj Id with a new Id, if they're different that means a new dj is playing.
        /// Sets _loadImage to true and the new Id.
        /// </summary>
        /// <param name="DjName">Dj's Id</param>
        public void IsNewDjPlaying(string DjName)
        {
            if (String.IsNullOrEmpty(Id) || Id != DjName)
            {
                _loadImage = true;
                Id = DjName;
            }
            else
            {
                _loadImage = false;
            }
        }

        /// <summary>
        /// Checks if there's a new DJ playing, and fetchs the new image.
        /// </summary>
        public void LoadNewImage()
        {
            if (!NeedToDownloadImage)
            {
                return;
            }
            _image = new BitmapImage();
            _image.BeginInit();
            _image.UriSource = new Uri("https://r-a-d.io/api/dj-image/" + Id + ".jpg",
                    UriKind.Absolute);
            _image.EndInit();
        }
    }
}
