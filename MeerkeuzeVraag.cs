using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;


namespace ProjectChallenge
{
    class MeerkeuzeVraag : Vraag
    {
        private List<string> vraagOpties;

        public MeerkeuzeVraag(string vraag)
        :base(vraag)
        {
            MaakVraagOpties();
        }

        public void MaakVraagOpties()
        {
            vraagOpties = new List<string>();

            for (int i = 1; i <= vraagDelen.Count() - 1; i++)
            {
                // check of de inhoud van de array een digit is of niet, als het een digit is, gebruik het als index voor het juist antwoord
                if (!(Regex.IsMatch(vraagDelen[i], @"^\d+$")))
                {
                    vraagOpties.Add(vraagDelen[i]);
                }
                else
                {
                    juistAntwoord = vraagDelen[Convert.ToInt32(vraagDelen[i])];
                }
            }
        }


        public List<string> VraagOpties
        {
            get { return vraagOpties; }
            set { vraagOpties = value; }
        }
        
    }
}
