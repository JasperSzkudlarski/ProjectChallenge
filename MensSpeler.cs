using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace ProjectChallenge
{
    // Nele De Backer
    // 1/05/2015 - 2/05/2015
    //jasper szkudlarski
    // 4/05/2012

    class MensSpeler : SpelEntiteit, IBeweegbaar
    {
        private Ellipse bol;
        private Random xRand;
        private Random yRand;
             

        public MensSpeler()
            :base()
        {
            kleur = "Red";

            bol = new Ellipse();
            bol.Fill = new SolidColorBrush((Color)ColorConverter.ConvertFromString(kleur));
            
            bol.Width = grote;
            bol.Height = grote;

            xRand = new Random();
            yRand = new Random();

            positie.X = xRand.Next(0, 631);
            positie.Y = yRand.Next(0, 278);

            xChange = xRand.Next(-2, 2);
            while(xChange == 0)
            {
                xChange = xRand.Next(-2, 2);
            }
            yChange = yRand.Next(-2, 2);
            while (xChange == 0)
            {
                yChange = yRand.Next(-2, 2);
            }
        }

        public void Beweeg()
        {
            if ((positie.X <= 0) || (positie.X >= 631 - 20))
            {
                xChange = -xChange;
            }
            if ((positie.Y <= 0) || (positie.Y >= 278 - 20))
            {
                yChange = -yChange;
            }

            positie.X += xChange;
            positie.Y += yChange;

            bol.Margin = new Thickness(positie.X, positie.Y, 0, 0);

        }

        public void MaakVrij()
        {
            if (X == X || Y + 15 == Y)
            {
                Kleur = "Red";
            }
        }

        public override void CheckHit(SpelEntiteit shape)
        {
            if((positie.X < shape.Positie.X && positie.X + grote > shape.Positie.X) && (positie.Y < shape.Positie.Y && positie.Y + grote > shape.Positie.Y) || 
                (positie.X < shape.Positie.X && positie.X + grote > shape.Positie.X) && (positie.Y < shape.Positie.Y + grote && positie.Y + grote > shape.Positie.Y + shape.Grote) ||
                (positie.X < shape.Positie.X + shape.Grote && positie.X + grote > shape.Positie.X + shape.Grote) && (positie.Y < shape.Positie.Y && positie.Y + grote > shape.Positie.Y) ||
                (positie.X < shape.Positie.X + shape.Grote && positie.X + grote > shape.Positie.X + shape.Grote) && (positie.Y < shape.Positie.Y + shape.Grote && positie.Y + grote > shape.Positie.Y + shape.Grote))
            {
                bounce(shape);

                if(shape.GetType() == typeof(ComputerSpeler))
                {
                    if(geraakt == false)
                    {
                        kleur = "Green";
                        bol.Fill = new SolidColorBrush((Color)ColorConverter.ConvertFromString(kleur));
                        geraakt = true;
                    }
                }
                else
                {
                    if(geraakt == true)
                    {
                        kleur = "Red";
                        bol.Fill = new SolidColorBrush((Color)ColorConverter.ConvertFromString(kleur));
                        geraakt = false;
                    }
                }
            }

            
        }

        public void bounce(SpelEntiteit shape)
        {
          xChange = -xChange;
          yChange = -yChange;
        }

        public double X
        {
            get { return positie.X; }
            set { positie.X = value; }
        }

        public double Y
        {
            get { return positie.Y; }
            set { positie.Y = value; }
        }

        public Ellipse Bol
        {
            get { return bol;}
        }
    }
}
