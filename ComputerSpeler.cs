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

    class ComputerSpeler : SpelEntiteit, IBeweegbaar
    {
       
        public Rectangle rect;
        private Random xRand;
        private Random yRand;

        public ComputerSpeler()
        {
            rect = new Rectangle();
            rect.Fill = new SolidColorBrush(Colors.Blue);
            grote = 15;
            rect.Width = grote;
            rect.Height = grote;

            xRand = new Random();
            yRand = new Random();

            positie.X = xRand.Next(0, 631);
            positie.Y = yRand.Next(0, 278);

            xChange = xRand.Next(-2, 2);
            while (xChange == 0)
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

            positie.X = positie.X + xChange;
            positie.Y = positie.Y + yChange;

            rect.Margin = new Thickness(positie.X, positie.Y, 0, 0);
        }

        public void MaakVrij()
        {

        }

        public override void CheckHit(SpelEntiteit shape)
        {
            if ((positie.X < shape.Positie.X && positie.X + grote > shape.Positie.X) && (positie.Y < shape.Positie.Y && positie.Y + grote > shape.Positie.Y) ||
                (positie.X < shape.Positie.X && positie.X + grote > shape.Positie.X) && (positie.Y < shape.Positie.Y + grote && positie.Y + grote > shape.Positie.Y + shape.Grote) ||
                (positie.X < shape.Positie.X + shape.Grote && positie.X + grote > shape.Positie.X + shape.Grote) && (positie.Y < shape.Positie.Y && positie.Y + grote > shape.Positie.Y) ||
                (positie.X < shape.Positie.X + shape.Grote && positie.X + grote > shape.Positie.X + shape.Grote) && (positie.Y < shape.Positie.Y + shape.Grote && positie.Y + grote > shape.Positie.Y + shape.Grote))
            {
                bounce(shape);
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

        public Rectangle Rect
        {
            get { return rect; }
            set { rect = value; }
        }
        
        
    }
}
