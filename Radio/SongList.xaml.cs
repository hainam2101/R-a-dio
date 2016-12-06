using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Radio
{
    /// <summary>
    /// Lógica de interacción para SongListTwo.xaml
    /// </summary>
    public partial class SongList : Window
    {

        public string item = "Hello";

        public SongPOCO[] items = { new SongPOCO() { Name = "ChouCho - Authentic Symphony", ID = 1, IsFavorite = true},
            new SongPOCO() { Name = "GRANRODEO - Summer GT09", ID = 2, IsFavorite = false},
            new SongPOCO() { Name = "Rica Matsumoto - Alive A life", ID = 3, IsFavorite = true},
            new SongPOCO() { Name = "Yoko Kanno - Arcadia", ID = 4, IsFavorite = false},
            new SongPOCO() { Name = "in feat. IA - Ayano's Theory of Happiness", ID = 5, IsFavorite = true},
        };
        public SongList()
        {
            InitializeComponent();
            newCont.ItemsSource = items;
            /*favCont.ItemsSource = items;
            delCont.ItemsSource = items;*/
            /*SongContainer.DataContext = items;
            SongContainer.ItemsSource = items;*/
        }
    }
}
