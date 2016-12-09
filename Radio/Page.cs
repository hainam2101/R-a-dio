using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Radio
{
    class Page
    {
        public int Number { get; set; }
        public bool IsSelected { get; set; }
        public string Selected
        {
            get
            {
                if (IsSelected)
                {
                    return "Bold";
                }
                else
                {
                    return "Normal";
                }
            }
        }
    }
}
