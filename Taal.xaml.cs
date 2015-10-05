using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
    // Datum:   06/04/2015

    public partial class Taal : Window
    {
        private int vraagNummer;
        private string graad;
        private int scoreTaal;
        private int maxVraag;

        private DispatcherTimer timer;

        private string fileToSearch;
        private int aantalVragen;
        private int randomNumber;
        private string filename;

        private List<string> antwoorden;
        private List<Vraag> vragen;
        private List<Vraag> gevraagd;

        private Gebruiker gebruiker;

        public Taal(int waarde, Gebruiker gebruiker)
        {
            InitializeComponent();

            int volgnummer = 1;

            this.gebruiker = gebruiker;
            antwoorden = new List<string>();
            vragen = new List<Vraag>();
            gevraagd = new List<Vraag>();
            timer = new DispatcherTimer();
            vraagNummer = 1;
            scoreTaal = 0;
            aantalVragen = 1;

            statusProgressBar.Value = 0;
            statusProgressBar.Maximum = 1500;
            timer.Interval = TimeSpan.FromMilliseconds(1);

            if (waarde == 1 || waarde == 0)
            {
                statusProgressBar.Maximum = 600;
                graad = "makkelijk";
            }
            else if (waarde == 2)
            {
                statusProgressBar.Maximum = 600;
                graad = "moeilijk";
            }

            filename = "Rapporten/" + gebruiker.Naam + " " + gebruiker.Achternaam + "/Taal" + "/rapport-" + gebruiker.Naam + Convert.ToString(DateTime.Now.Day) + "-" + Convert.ToString(DateTime.Now.Month) + "-" + Convert.ToString(DateTime.Now.Year);

            while (File.Exists(filename + ".txt"))
            {

                if (File.Exists(filename + "(" + Convert.ToString(volgnummer) + ")" + ".txt"))
                {
                    volgnummer++;
                }
                else
                {
                    filename = filename + "(" + volgnummer + ")";
                }

            }
            filename = filename + ".txt";

            GenereerLijstVragen(waarde);

            maxVraag = vragen.Count;

            timer.Tick += timer_Tick;
            timer.Start();

            AlgemeneVraagmaken();
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            if (statusProgressBar.Value == statusProgressBar.Maximum)
            {
                if (vragen[randomNumber].VraagDelen.Length == 2 || vragen[randomNumber].VraagDelen.Length == 3 && vragen[randomNumber].VraagDelen[2].Split('.')[1] == "gif")
                {  
                    foreach (TextBox t in antwoordGrid.Children)
                    {
                        t.IsEnabled = false;
                    }
                }
                else
                {

                    foreach (RadioButton r in antwoordGrid.Children)
                    {
                        r.IsEnabled = false;
                    }

                }
                timer.Stop();
            }
            else
            {
                statusProgressBar.Value += 1;
            }
        }

        private void GenereerLijstVragen(int waarde)
        {
            StreamReader inputStream = null;
            Vraag v;
            string vraag;

            try
            {
                if (waarde == 0 || waarde == 1)
                {
                    fileToSearch = System.IO.Path.Combine(@"Taal/Vragenmakkelijk.txt");
                    inputStream = new StreamReader(fileToSearch);
                }
                else
                {
                    fileToSearch = System.IO.Path.Combine(@"Taal/Vragenmoeilijk.txt");
                    inputStream = new StreamReader(fileToSearch);
                }
                vraag = inputStream.ReadLine();

                while (vraag != null)
                {
                    if(vraag.Split(',').Length == 2 || vraag.Split(',').Length == 3 && vraag.Split(',')[2].Split('.')[1] == "gif")
                    {
                        v = new Invulvraag(vraag, "Taal");
                    }
                    else
                    {
                        v = new MeerkeuzeVraag(vraag);
                    }
                    vragen.Add(v);
                    vraag = inputStream.ReadLine();
                }
            }
            catch (FileNotFoundException)
            {
                MessageBox.Show("Error: File not found.");
            }

            finally
            {
                inputStream.Close();
            }


        }

        private void volgendeButton_Click(object sender, RoutedEventArgs e)
        {
            if (graad == "makkelijk")
            {
                VerbeterVraag(randomNumber);
                Reset();
                aantalVragen++;
                timer.Start();
                AlgemeneVraagmaken();
            }
            else
            {
                VerbeterVraagMoeilijk(randomNumber);
                Reset();
                aantalVragen++;
                timer.Start();
                AlgemeneVraagmaken();

            }
        }

        private void Reset()
        {
            vraagNummer++;
            TextBox[] textremove = new TextBox[1];
            RadioButton[] radioremove = new RadioButton[5];
            int index = 0;

            if (vragen[randomNumber].VraagDelen.Length == 2 || vragen[randomNumber].VraagDelen.Length == 3 && vragen[randomNumber].VraagDelen[2].Split('.')[1] == "gif")
            {
                foreach (TextBox t in antwoordGrid.Children)
                {
                    textremove[index] = t;
                }

                for (int i = 0; i <= textremove.Length - 1; i++)
                {
                    antwoordGrid.Children.Remove(textremove[i]);
                }
            }
            else
            {
                foreach (RadioButton r in antwoordGrid.Children)
                {
                    radioremove[index] = r;
                    index++;
                }

                for (int i = 0; i <= radioremove.Length - 1; i++)
                {
                    antwoordGrid.Children.Remove(radioremove[i]);
                }
            }

            statusProgressBar.Value = 0;
            timer.Stop();

            if (aantalVragen > 7)
            {
                Resultaat res = new Resultaat(scoreTaal, gevraagd, antwoorden, filename, graad, gebruiker);
                res.Left = 400;
                res.Top = 200;
                res.Show();
                this.Close();
            }

        }

        private void AlgemeneVraagmaken()
        {
            vraagNummerLabel.Content = "vraag " + vraagNummer;
            char[] separator = new char[1];
            separator[0] = ',';

            Random random = new Random();


            randomNumber = random.Next(0, maxVraag);

            if (gevraagd.Count < vragen.Count)
            {
                while (gevraagd.IndexOf(vragen[randomNumber]) != -1)
                {
                    randomNumber = random.Next(0, maxVraag);
                }
                gevraagd.Add(vragen[randomNumber]);
            }
            else
            {
                MessageBox.Show("Teveel vragen gevraagd");
            }

            vraagTextBlock.Text = vragen[randomNumber]._Vraag;


            if (vragen[randomNumber].VraagDelen.Length == 2 || vragen[randomNumber].VraagDelen.Length == 3 && vragen[randomNumber].VraagDelen[2].Split('.')[1] == "gif")
            {
                MaakVraagInvul();
            }
            else
            {
                MaakVraagMeerkeuze();
            }

        }

        private void MaakVraagMeerkeuze()
        {
            MeerkeuzeVraag v = (MeerkeuzeVraag)vragen[randomNumber];

            for (int i = 0; i <= v.VraagOpties.Count - 1; i++)
            {
                    RadioButton radio = new RadioButton();
                    antwoordGrid.Children.Add(radio);
                    radio.Name = "meerkeuzeRadioButton" + Convert.ToString(i);
                    radio.Content = v.VraagOpties[i];
                    radio.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
                    radio.VerticalAlignment = System.Windows.VerticalAlignment.Top;
                    radio.Margin = new Thickness(20, 5 + (i * 30), 0, 0);
                    radio.Height = 18;
            }
        }

        private void MaakVraagInvul()
        {
            Invulvraag v = (Invulvraag)vragen[randomNumber];

            TextBox antwoordTextBox = new TextBox();
            antwoordTextBox.Height = 20;
            antwoordTextBox.Width = 200;
            antwoordGrid.Children.Add(antwoordTextBox);
            antwoordTextBox.Margin = new Thickness(-230, -165, 0, 0);

            try
            {
                if (vragen[randomNumber].VraagDelen.Length == 3 && vragen[randomNumber].VraagDelen[2].Split('.')[1] == "gif")
                {
                    vraagImage.Source = v.Src;
                }
            }
            catch { }
        }

        private void VerbeterVraag(int randomNumber)
        {
            bool juist = false;
            string antwoord = "";

            foreach (RadioButton r in antwoordGrid.Children)
                {
                    if(r.IsChecked == true)
                    {
                        antwoord = Convert.ToString(r.Content);
                    }
                }
            
            if (antwoord != "")
            {
                antwoorden.Add(antwoord);
            }
            else
            {
                antwoorden.Add(" ");
            }
            
            juist = vragen[randomNumber].CheckJuist(antwoord);

            if(juist)
            {
                scoreTaal++;
            }
        }

        private void VerbeterVraagMoeilijk(int randomNumber)
        {
            string antwoord = "";

            foreach (TextBox t in antwoordGrid.Children)
            {
                if (t.Text.ToLower().Trim() == vragen[randomNumber].JuistAntwoord.ToLower().Trim())
                {
                    scoreTaal++;
                    antwoord = t.Text;
                }
            }

            if (antwoord != "")
            {
                antwoorden.Add(antwoord);
            }
            else
            {
                antwoorden.Add(" ");
            }

        }

        private void ExitItem_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void hoofdmenuItem_Click(object sender, RoutedEventArgs e)
        {
            Startscherm s = new Startscherm(gebruiker);
            s.Left = 400;
            s.Top = 200;
            s.Show();
            this.Close();
        }
    }
}
