using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ProjectChallenge
{
    /// <summary>
    /// auteur: Thomas Ven
    /// TO DO 
    /// DONE?
    /// </summary>
    public class Gebruiker
    {
        private string naam;
        private string achternaam;
        private string directory;
        private string email;
        int tijd;

        public Gebruiker(string n, string l, string d,string e, int t)
        {
            naam = n;
            achternaam = l;
            directory = d;
            email = e;
            tijd = t;
        }

        public string Naam
        {
            get { return naam; }
        }

        public string Achternaam
        {
            get { return achternaam; }
        }

        public string Directory
        {
            get { return directory; }
        }

        public string Email
        {
            get { return email; }
        }

        public int Tijd
        {
            get { return tijd; }
            set { tijd = value; }
        }
        
        public void SchrijfTijd(string email, int tijd)
        {
            int teller = 0;
            int lijnNummer = 0;
            string lijn;
            try
            {
                // users.txt lijn per lijn lezen en zoeken naar de mailTextBox.Text
                using (StreamReader file = new StreamReader("Users/Users.txt"))
                {
                    while ((lijn = file.ReadLine()) != null)
                    {
                        if (lijn.Equals(email))
                        {
                            lijnNummer = lijnNummer + teller;
                        }
                        teller++;
                    }
                }
            }
            catch(FileNotFoundException)
            {
                MessageBox.Show("file users.txt not found");
            }
            
            // Gaat de txt inlezen dat in de array lines zetten dan overschrijf ik in de array het element
            // waar tijd inzit hierna overschrijf ik de hele Users.txt met de array
            string[] lines = System.IO.File.ReadAllLines("Users/Users.txt");
            lines[lijnNummer + 3] = Convert.ToString(tijd);
            System.IO.File.WriteAllLines("Users/Users.txt", lines);
        }

        
    }
}
