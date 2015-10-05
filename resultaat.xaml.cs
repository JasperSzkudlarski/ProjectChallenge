using System;
using System.Collections.Generic;
using System.IO;
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
using System.Windows.Threading;

namespace ProjectChallenge
{
    // auteur:  Nele De Backer
    // Datum:   10/04/2015 en 11/04/2015

    public partial class Resultaat : Window
    {
        private BitmapImage src = new BitmapImage();
        private Gebruiker gebruiker;
        private int score;
        private int punten;

        //Taal, kennis, metend rekenen en meetkunde
        public Resultaat(int score, List<Vraag> gevraagd, List<string>antwoorden, string filename , string moeilijkheid, Gebruiker gebruiker)
        {
            InitializeComponent();
            this.gebruiker = gebruiker;
            SchrijfWeg(score, gevraagd, antwoorden, filename, moeilijkheid);
            this.score = score;
            


            switch(moeilijkheid)
            {
                case "makkelijk":
                    punten = score * 10;
                    break;
                case "middelmatig":
                    punten = score * 20;
                    break;
                case "moeilijk":
                    punten = score * 30;
                    break;
            }

            if (score < gevraagd.Count/2)
            {
                resultaatTextBlock.Text = "Je bent niet geslaagd met een score van " + score + " op " + gevraagd.Count + ". Volgende keer beter. Je hebt " + punten + " seconden speeltijd";
                ToonAfbeelding(score, gevraagd.Count / 2);
            }
            else
            {
                resultaatTextBlock.Text = "Je bent geslaagd met een score van " + score + " op " + gevraagd.Count + ". Goed gedaan ! Je hebt " + punten + " seconden speeltijd";
                ToonAfbeelding(score, gevraagd.Count / 2);
            }

            try
            {
                gebruiker.Tijd = gebruiker.Tijd + punten;
                gebruiker.SchrijfTijd(gebruiker.Email, gebruiker.Tijd);
            }
            catch (FileNotFoundException)
            {
                MessageBox.Show("file: " + gebruiker.Directory + " niet gevonden.");
            }
        }
        //Jannes Houben
        //Hoofdrekenen
        public Resultaat(int score, List<string> juisteOplossing, List<string> juistOfFout, List<string> gevraagd, List<string> antwoorden, string filename, string moeilijkheid, Gebruiker gebruiker)
        {
            InitializeComponent();
            this.gebruiker = gebruiker;
            SchrijfWegHoofdrekenen(score, juisteOplossing, juistOfFout, gevraagd, antwoorden, filename, moeilijkheid);
            this.score = score;

            switch (moeilijkheid)
            {
                case "makkelijk":
                    punten = score * 10;
                    break;
                case "middelmatig":
                    punten = score * 20;
                    break;
                case "moeilijk":
                    punten = score * 30;
                    break;
            }

            if (score < gevraagd.Count / 2)
            {
                resultaatTextBlock.Text = "Je bent niet geslaagd met een score van " + score + " op " + gevraagd.Count + ". Volgende keer beter. Je hebt " + punten + " seconden speeltijd";
                ToonAfbeelding(score, gevraagd.Count/2);
            }
            else
            {
                resultaatTextBlock.Text = "Je bent geslaagd met een score van " + score + " op " + gevraagd.Count + ". Goed gedaan ! Je hebt " + punten + " seconden speeltijd";
                ToonAfbeelding(score, gevraagd.Count / 2);
            }

            try
            {
                gebruiker.Tijd = gebruiker.Tijd + punten;
                gebruiker.SchrijfTijd(gebruiker.Email, gebruiker.Tijd);
            }
            catch (FileNotFoundException)
            {
                MessageBox.Show("file: " + gebruiker.Directory + " niet gevonden.");
            }
        }
        //jasper Szkudlarski
        //Taal, kennis, metend rekenen en meetkunde
        private void SchrijfWeg(int score, List<Vraag> gevraagd, List<string> antwoorden, string filename, string moeilijkheid)
        {
            string res = "fout";
            string rapport = "";
            StreamWriter writer;

            
            for (int i = 0; i <= gevraagd.Count - 1; i++ )
            {
                for(int y = 0 ; y <= gevraagd[i].VraagDelen.Length - 1 ; y++)
                {
                    rapport = rapport + gevraagd[i].VraagDelen[y];
                    if(!(y == gevraagd[i].VraagDelen.Length - 1))
                    {
                        rapport = rapport + "^";
                    }
                }
                
                if(gevraagd[i].JuistAntwoord == antwoorden[i])
                {
                    res = "juist";
                }
                else
                {
                    res = "fout";
                }

                rapport = rapport + "^" + antwoorden[i] + "^" + res;

                if (File.Exists(filename))
                {
                    writer = File.AppendText(filename);
                    writer.WriteLine(rapport);
                }
                else
                {
                    writer = File.CreateText(filename);
                    writer.WriteLine("Rapport-" + gebruiker.Naam + " " + Convert.ToString(DateTime.Now.Day) + "/" + Convert.ToString(DateTime.Now.Month) + "/" + Convert.ToString(DateTime.Now.Year) + " " + Convert.ToString(DateTime.Now.Hour) + ":" + Convert.ToString(DateTime.Now.Minute) + " " + moeilijkheid + ":");
                    writer.WriteLine(rapport);
                }

                writer.Close();
                rapport = "";
            }

            writer = File.AppendText(filename);
            writer.WriteLine(Convert.ToString(score) + "/" + Convert.ToString(gevraagd.Count) + " juist");
            writer.WriteLine("******************************************************************************************************");
            writer.Close();
            
        }
        //jannes houben
        //Hoofdrekenen
        private void SchrijfWegHoofdrekenen(int score, List<string> juisteOplossing, List<string> juistOfFout, List<string> gevraagdHoofdrekenen, List<string> antwoordenHoofdrekenen, string filename, string moeilijkheid)
        {
            string rapport = "";
            StreamWriter writer;

            for (int i = 0; i <= juisteOplossing.Count - 1; i++)
            {
                rapport = rapport + gevraagdHoofdrekenen[i] + "^" + juisteOplossing[i] + "^" + antwoordenHoofdrekenen[i] + "^" + juistOfFout[i];
                if (File.Exists(filename))
                {
                    writer = File.AppendText(filename);
                    writer.WriteLine(rapport);
                }
                else
                {
                    writer = File.CreateText(filename);
                    writer.WriteLine("Rapport-" + gebruiker.Naam + " " + Convert.ToString(DateTime.Now.Day) + "/" + Convert.ToString(DateTime.Now.Month) + "/" + Convert.ToString(DateTime.Now.Year) + " " + Convert.ToString(DateTime.Now.Hour) + ":" + Convert.ToString(DateTime.Now.Minute) + " " + moeilijkheid + ":");
                    writer.WriteLine(rapport);
                }
                writer.Close();
                rapport = "";
            }
            
            writer = File.AppendText(filename);
            writer.WriteLine(Convert.ToString(score) + "/" + Convert.ToString(gevraagdHoofdrekenen.Count) + " juist");
            writer.WriteLine("******************************************************************************************************");
            writer.Close();
        }
        
        private void ToonAfbeelding(int score, int minimum)
        {
            src.BeginInit();
            if (score < minimum)
            {
                src.UriSource = new Uri("droevig.gif", UriKind.Relative);
            }
            else
            {
                src.UriSource = new Uri("goed.gif", UriKind.Relative);
            }

            src.CacheOption = BitmapCacheOption.OnLoad;
            src.EndInit();
            afbeeldingImage.Source = src;

        }

        private void spelButton_Click(object sender, RoutedEventArgs e)
        {
            Game game = new Game(gebruiker);
            game.Left = 400;
            game.Top = 200;
            game.Show();
            this.Close();
        }

        private void ExitItem_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void hoofdmenuItem_Click(object sender, RoutedEventArgs e)
        {
            Startscherm start = new Startscherm(gebruiker);
            start.Left = 400;
            start.Top = 200;
            start.Show();
            this.Close();
        }

        private void startschermButton_Click(object sender, RoutedEventArgs e)
        {
            Startscherm s = new Startscherm(gebruiker);
            s.Left = 400;
            s.Top = 200;
            s.Show();
            this.Close();
        }

    }
}
