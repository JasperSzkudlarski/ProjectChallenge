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
using System.IO;
using System.Text.RegularExpressions;
using System.Windows.Threading;

namespace ProjectChallenge
{
    // auteur:  Jasper Szkudlarski
    // Datum:   09/04/2015

    public partial class Kennis : Window
    {
        private List<Vraag> vragen;
        private List<Vraag> gevraagd;
        private List<string> antwoorden;
        private DispatcherTimer timer;
        private int aantalJuist;
        private int aantalVragen;
        private int vraagNr;
        private Gebruiker gebruiker;
        private int maxVraag;
        private string filename;
        private int random;
        private string graad;
       
        public Kennis(int moeilijkheid, Gebruiker gebruiker)
        {
            InitializeComponent();

                int volgnummer = 1;
                this.gebruiker = gebruiker; ;

                vragen = new List<Vraag>();
                gevraagd = new List<Vraag>();
                antwoorden = new List<string>();

                aantalJuist = 0;
                aantalVragen = 10;
                vraagNr = 0;
                graad = "makkelijk";

                timer = new DispatcherTimer();
                timer.Interval = TimeSpan.FromSeconds(1);
                timer.Tick += timer_Tick;

                filename = "Rapporten/" + gebruiker.Naam + " " + gebruiker.Achternaam + "/Kennis" + "/rapport-" + gebruiker.Naam + " " + Convert.ToString(DateTime.Now.Day) + "-" + Convert.ToString(DateTime.Now.Month) + "-" + Convert.ToString(DateTime.Now.Year);

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

                //zet de tijd naar gelang de moeilijkheidsgraad
                if (moeilijkheid == 1)
                {
                    aantalVragen = 20;
                    statusProgressBar.Maximum = 20;
                    graad = "middelmatig";
                }
                else if (moeilijkheid == 2)
                {
                    aantalVragen = 30;
                    statusProgressBar.Maximum = 12;
                    graad = "moeilijk";
                }

                maakVraagLijst();
                maxVraag = vragen.Count;
                MaakVraag();

        }

        private void volgendeButton_Click(object sender, RoutedEventArgs e)
        {
            CheckJuist();
            Reset();
            MaakVraag();
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            if(statusProgressBar.Value != statusProgressBar.Maximum)
            { 
            statusProgressBar.Value++;
            }
            else
            {
                if (vragen[random].VraagDelen.Length == 2 || vragen[random].VraagDelen.Length == 3 && vragen[random].VraagDelen[2].Split('.')[1] == "gif")
                {
                    foreach (TextBox t in antwoordGrid.Children)
                    {
                        t.IsEnabled = false;
                    }
                }
                else
                {
                    foreach(RadioButton r in antwoordGrid.Children)
                    {
                        r.IsEnabled = false;
                    }
                timer.Stop();
                }
            }
        }

        private void maakVraagLijst()
        {
            StreamReader reader = null;
            string tempVraag;
            Vraag v;

            try
            {

                reader = new StreamReader("Kennis/Vragen.txt");
                tempVraag = reader.ReadLine();

                while (tempVraag != null)
                {
                    if (tempVraag.Split(',').Length == 2 || tempVraag.Split(',').Length == 3 && tempVraag.Split(',')[2].Split('.')[1] == "gif")
                    {
                        v = new Invulvraag(tempVraag, "Kennis");
                    }
                    else
                    {
                        v = new MeerkeuzeVraag(tempVraag);
                    }
                    vragen.Add(v);
                    tempVraag = reader.ReadLine();
                }
            }
            catch (FileNotFoundException)
            {
                MessageBox.Show("vragen.txt is not found in Kennis");
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }
            }
        }

        private void MaakVraag()
        {
            Random rnd = new Random();
            random = rnd.Next(0, maxVraag);

            if(gevraagd.Count < aantalVragen)
            {
                while (gevraagd.IndexOf(vragen[random]) != -1)
                {
                random = rnd.Next(0, maxVraag);
                }
                gevraagd.Add(vragen[random]);
            }
            else
            {
                Resultaat r = new Resultaat(aantalJuist, gevraagd, antwoorden, filename, graad, gebruiker);
                r.Show();
                r.Left = 400;
                r.Top = 200;
                this.Close();
            }

            vraagTextblock.Text = vragen[random]._Vraag;

            // check of het 3de deel van de string eidigt in .gif, zo wordt herkend dat het een invulvraag met afbeelding is,
            // zonder dit kan zou een meerkeuzevraag altijd minstens 3 keuzes moeten bevatten(omdat er dan alleen gechecked wordt op lengte van de array)
            if (vragen[random].VraagDelen.Length == 2 || vragen[random].VraagDelen.Length == 3 && vragen[random].VraagDelen[2].Split('.')[1] == "gif")
            {
                maakInvulVraag();
            }
            else 
            {
                maakMeerkeuzevraag();
            }
            
            timer.Start();
            vraagNr++;
        }

        private void maakInvulVraag()
        {
            //een invullvraag met 1 mogelijk antwoord
            Invulvraag v = (Invulvraag)vragen[random];

            TextBox antwoordTextBox = new TextBox();

            antwoordTextBox.Height = 20;
            antwoordTextBox.Width = 200;
            antwoordGrid.Children.Add(antwoordTextBox);
            antwoordTextBox.Margin = new Thickness(-230, -165, 0, 0);

            try
            {
                if (vragen[random].VraagDelen.Length == 3 && vragen[random].VraagDelen[2].Split('.')[1] == "gif")
                {
                vraagImage.Source = v.Src;
                }
            }
            catch { }

        }

        private void maakMeerkeuzevraag()
        {
            //een meerkeuzevraag met variabele aantal aan keuzes
            MeerkeuzeVraag v = (MeerkeuzeVraag)vragen[random];

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

        private void Reset()
        {
            TextBox[] textremove = new TextBox[1];
            RadioButton[] radioremove = new RadioButton[5];
            int index = 0;

            if (vragen[random].VraagDelen.Length == 2 || vragen[random].VraagDelen.Length == 3)
            {
                foreach (TextBox t in antwoordGrid.Children)
                {
                    textremove[index] = t;
                }

                for (int i = 0; i <= textremove.Length - 1; i++)
                {
                    antwoordGrid.Children.Remove(textremove[i]);
                }

                if (vraagImage.Source != null)
                {
                    vraagImage.Source = null;
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
            timer.Stop();
            statusProgressBar.Value = 0;
        }

        private void CheckJuist()
        {
            string antwoord = "";
            bool juist = false;

            if (vragen[random].VraagDelen.Length == 2 || vragen[random].VraagDelen.Length == 3 && vragen[random].VraagDelen[2].Split('.')[1] == "gif")
            {
                foreach (TextBox t in antwoordGrid.Children)
                {
                    antwoord = t.Text;
                    
                    if (antwoord != "")
                    {
                        antwoorden.Add(antwoord);
                    }
                    else
                    {
                        antwoorden.Add(" ");
                    }
                }
            }
            else
            {
                foreach (RadioButton r in antwoordGrid.Children)
                {
                    if (r.IsChecked == true)
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
            }
            juist = vragen[random].CheckJuist(antwoord);

            if (juist)
            {
                aantalJuist++;
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
