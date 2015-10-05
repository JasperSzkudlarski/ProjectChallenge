using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

///jasper szkudlarski
///05/05/2015

namespace ProjectChallenge
{
    public abstract class SpelEntiteit
    {
        protected double snelheid;
        protected bool geraakt;
        protected Point positie;
        protected string kleur;
        protected int grote;
        protected int xChange;
        protected int yChange;

        public SpelEntiteit()
        {
            positie = new Point();
            snelheid = 0;
            geraakt = false;
            kleur = "";
            grote = 15;
        }

        public abstract void CheckHit(SpelEntiteit shape);

        public int XChange
        {
            get { return xChange; }
            set { xChange = value; }
        }
        
        public int YChange
        {
            get { return yChange; }
            set { yChange = value; }
        }

        public double Snelheid
        {
            get { return snelheid; }
            set { snelheid = value; }
        }

        public bool Geraakt
        {
            get { return geraakt; }
            set { geraakt = value; }
        }

        public Point Positie 
        {
            get { return positie; }
            set { positie = value; }
        }

        public string Kleur
        {
            get { return kleur; }
            set { kleur = value; }
        }

        public int Grote
        {
            get { return grote; }
            set { grote = value; }
        }
        
    }
}
