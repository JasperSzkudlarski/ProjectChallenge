using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace ProjectChallenge
{
    class Invulvraag : Vraag
    {
        private BitmapImage src;

        public Invulvraag(string vraag, string vak)
        :base(vraag)
        {
            juistAntwoord = vraagDelen[1];

            if (vraagDelen.Length == 3 && vraagDelen[2].Split('.')[1] == "gif")
            {
                string dir;
                src = new BitmapImage();

                dir = System.IO.Path.Combine(vak, "afbeeldingen", vraagDelen[2]);
                src.BeginInit();
                src.UriSource = new Uri(dir, UriKind.Relative);
                src.CacheOption = BitmapCacheOption.OnLoad;
                src.EndInit();
            }
        }

        public BitmapImage Src
        {
            get { return src; }
            set { src = value; }
        }
        

    }
}
