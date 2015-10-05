using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace ProjectChallenge
{
    class Hoofdrekenen
    {

        private double getal;
        private int operatorNumber;
        private string operatorKarakter;
        private Random random = new Random();
        private bool uitkomst = false;

        public double geefGetal()
        {
            getal = random.Next(0, 4);
          
            return getal;
        }

        public double geefGetal2()
        {
            getal = random.Next(1, 4);

            return getal;
        }

        public string geefOperator()
        {
            operatorNumber = random.Next(0, 4);

            switch (operatorNumber)
            {
                case 0: operatorKarakter = "+"; break;
                case 1: operatorKarakter = "-"; break;
                case 2: operatorKarakter = "*"; break;
                case 3: operatorKarakter = "/"; break;
            }

            return operatorKarakter;
        }

        public bool controleer(double l1, Label l2, double l3, double t1)
        {
            operatorKarakter = Convert.ToString(l2.Content);

            switch (operatorKarakter)
            {
                case "+": if ((l1 + l3) == t1)
                    {
                        uitkomst = true;
                    }
                    else
                    {
                        uitkomst = false;
                    };
                    break;

                case "-": if ((l1 - l3) == t1)
                    {
                        uitkomst = true;
                    }
                    else
                    {
                        uitkomst = false;
                    };
                    break;

                case "*": if ((l1 * l3) == t1)
                    {
                        uitkomst = true;
                    }
                    else
                    {
                        uitkomst = false;
                    };
                    break;

                case "/": if (Math.Round((l1 / l3), 2) == Math.Round(t1, 2))
                    {
                        uitkomst = true;
                    }
                    else
                    {
                        uitkomst = false;
                    };
                    break;

                default: uitkomst = false;
                    break;
            }
            return uitkomst;
        }

    }
}
