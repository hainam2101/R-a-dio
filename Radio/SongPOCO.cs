using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Radio
{
    public class SongPOCO
    {
        public string Name { get; set; }
        public int ID { get; set; }
        public bool IsFavorite { get; set; }
        // Allows binding for a trigger
        public string Favorite { get { return IsFavorite.ToString(); } }
    }
}
