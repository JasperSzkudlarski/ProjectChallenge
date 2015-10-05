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
    /*
     * Jannes Houben
     * 1TinD
     */

    public partial class Wiskunde : Window
    {

        #region Alle variabele van hoofdrekenen
        //Layout voor hoofdrekenen
        private Label getal1Label;
        private Label getal2Label;
        private Label operatorLabel;
        private Label gelijkheidsLabel;
        private TextBox uitkomstTextBox;
        private Button volgendeHoofdrekenButton;
        private Hoofdrekenen hr;
        private DispatcherTimer timer2;
        private List<string> gevraagdHoofdrekenen;
        private List<string> juisteOplossing;
        private List<string> antwoordenHoofdrekenen;       
        private List<string> juistOfFoutList;


        //Random getal-generator voor random getallen te maken en een operator te kiezen (ook random)
        private Random random1;

        //Houd de score van de leerling bij.
        private int score = 0;

        //aantal vragen per sessie is 10
        private int aantalVragenHoofdrekenen = 0;
        #endregion

        #region Alle variabelen van meetkunde   
        private Button volgendeMeetkundeButton;       
        #endregion

        #region Alle variabelen van metend rekenen
        private Button volgendeMetendRekenenButton; 
        #endregion

        #region Alle variabelen meetkunde en metend rekenen
        private List<Vraag> vragen;
        private List<Vraag> gevraagd;
        private List<string> antwoorden;
        private DispatcherTimer timer;
        private int aantalVragen;
        private int vraagNr;
        private Gebruiker gebruiker;
        private int maxVraag;
        private string filename;
        private int random;    
        private string graad;

        private TextBlock vraagTextBlock;  
        #endregion

        private int speelTijd;

        public Wiskunde(int moeilijkheid, Gebruiker gebruiker)
        {
            InitializeComponent();

            #region Hoofrekenen initialiseren
            //Object van klasse "Hoofdrekenen" aanmaken.
            hr = new Hoofdrekenen();

            //Random generator declareren
            random1 = new Random();

            //Dispatchertimer
            timer2 = new DispatcherTimer();
            timer2.Interval = TimeSpan.FromSeconds(1);
            timer2.Tick +=timer2_Tick;
           
            gevraagdHoofdrekenen = new List<string>();
            juisteOplossing = new List<string>();
            antwoordenHoofdrekenen = new List<string>();
            juistOfFoutList = new List<string>();

            //zet de tijd naar gelang de moeilijkheidsgraad
            if (moeilijkheid == 2)
            {
                statusProgressBar.Maximum = 30;
            }
            else if (moeilijkheid == 1)
            {
                statusProgressBar.Maximum = 60;
            }
            #endregion

            #region HoofdrekenLayout aanmaken
            //Layout voor hoofrekenen maken
            maakHoofdrekenLayout();
            #endregion

            #region HoofdrekenLayout onzichtbaar maken
            maakHoofdrekenLayoutOnzichtbaar();
            #endregion
          
            #region MeetkundeLayout aanmaken
            maakMeetkundeLayout();
            #endregion

            #region Meetkunde onzichtbaar maken
            maakMeetkundeOnzichtbaar();
            #endregion

            #region MetendRekenenLayout aanmaken
            maakMetendRekenenLayout();
            #endregion

            #region MetendRekenen onzichtbaar maken
            maakMetendRekenenOnzichtbaar();
            #endregion

            #region initialiseren vragenlijst
            int volgnummer = 1;
            this.gebruiker = gebruiker;

            vragen = new List<Vraag>();
            gevraagd = new List<Vraag>();
            antwoorden = new List<string>();

            aantalVragen = 10;
            vraagNr = 0;
            graad = "makkelijk";

            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += timer_Tick;

            filename = "Rapporten/" + gebruiker.Naam + " " + gebruiker.Achternaam + "/Wiskunde" + "/rapport-" + gebruiker.Naam + " " + Convert.ToString(DateTime.Now.Day) + "-" + Convert.ToString(DateTime.Now.Month) + "-" + Convert.ToString(DateTime.Now.Year);

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
            #endregion  
        }

        private void hoofdrekenButton_Click(object sender, RoutedEventArgs e)
        {
            //Enkel de juiste elementen zichtbaar maken.
            metendRekenenButton.Visibility = Visibility.Hidden;
            meetkundeButton.Visibility = Visibility.Hidden;
            hoofdrekenButton.Visibility = Visibility.Hidden;

            //HoofdrekenLayout zichtbaar maken
            maakHoofdrekenLayoutZichtbaar();

            //Als je 10 hoofdrekenvragen beantwoord hebt is het gedaan, en krijg je je score.
            aantalVragenHoofdrekenen++;

            getal1Label.Content = hr.geefGetal();
            operatorLabel.Content = hr.geefOperator();
            if (Convert.ToString(operatorLabel.Content) == "/")
            {
                getal2Label.Content = hr.geefGetal2();
            }
            else
            {
                getal2Label.Content = hr.geefGetal();
            }

            gelijkheidsLabel.Content = "=";

            timer2.Start();
           
        }

        private void volgendeHoofdrekenButton_Click(object sender, RoutedEventArgs e)
        {
            //FORMATEXCEPTION OPVANGEN!!!
            try
            {
                double getal1 = Convert.ToDouble(getal1Label.Content);
                double getal2 = Convert.ToDouble(getal2Label.Content);
                double uitkomst = Convert.ToDouble(uitkomstTextBox.Text);

                if (aantalVragenHoofdrekenen <= 10)
                {
                    if (hr.controleer(getal1, operatorLabel, getal2, uitkomst).Equals(true))
                    {
                        score++;
                        speelTijd = speelTijd + 5;

                        juistOfFoutList.Add("Juist");
                        
                    }
                    else 
                    {
                        juistOfFoutList.Add("Fout");
                    }

                    //GevraagdHoofdrekenen
                    string opgave = Convert.ToString(getal1Label.Content) + " " + Convert.ToString(operatorLabel.Content) + " " + Convert.ToString(getal2Label.Content) + " =";
                    gevraagdHoofdrekenen.Add(opgave);

                    //VOEGT DE JUISTE OPLOSSING TOE AAN DE LIST "juisteOplossing"
                    double oplossing;
                    switch (Convert.ToString(operatorLabel.Content))
                    {
                        case "+": oplossing = Convert.ToDouble(getal1Label.Content) + Convert.ToDouble(getal2Label.Content); break;
                        case "-": oplossing = Convert.ToDouble(getal1Label.Content) - Convert.ToDouble(getal2Label.Content); break;
                        case "*": oplossing = Convert.ToDouble(getal1Label.Content) * Convert.ToDouble(getal2Label.Content); break;
                        case "/": oplossing = Convert.ToDouble(getal1Label.Content) / Convert.ToDouble(getal2Label.Content); break;
                        default: oplossing = 0; break;
                    }
                    juisteOplossing.Add(Convert.ToString(oplossing));

                    //AntwoordenHoofdrekenen
                    antwoordenHoofdrekenen.Add(uitkomstTextBox.Text);

                    aantalVragenHoofdrekenen++;

                    getal1Label.Content = hr.geefGetal();
                    operatorLabel.Content = hr.geefOperator();
                    if (Convert.ToString(operatorLabel.Content) == "/")
                    {
                        getal2Label.Content = hr.geefGetal2();
                    }
                    else
                    {
                        getal2Label.Content = hr.geefGetal();
                    }                   
                    uitkomstTextBox.Text = "0";

                    

                    timer2.Start();
                }
                else
                {
                    Resultaat r = new Resultaat(score, juisteOplossing, juistOfFoutList, gevraagdHoofdrekenen, antwoordenHoofdrekenen, filename, graad, gebruiker);
                    r.Left = 400;
                    r.Top = 200;
                    r.Show();
                    this.Close();
                }

                statusProgressBar.Value = 0;
               
                uitkomstTextBox.IsEnabled = true;
            }
            catch
            {
                MessageBox.Show("U dient een getal in te geven. Geen letters of symbolen");
            }
        }
 
        private void maakHoofdrekenLayout()
        {
            //Nieuwe labels maken
            getal1Label = new Label();
            getal2Label = new Label();
            operatorLabel = new Label();
            gelijkheidsLabel = new Label();

            //Nieuwe textbox maken
            uitkomstTextBox = new TextBox();

            //Nieuwe button maken
            volgendeHoofdrekenButton = new Button();

            //Labels en textbox toevoegen aan grid
            antwoordGrid.Children.Add(getal1Label);
            antwoordGrid.Children.Add(getal2Label);
            antwoordGrid.Children.Add(operatorLabel);
            antwoordGrid.Children.Add(gelijkheidsLabel);
            antwoordGrid.Children.Add(uitkomstTextBox);
            antwoordGrid.Children.Add(volgendeHoofdrekenButton);

            //GetalLabels, uitkomsttextbox en volgendeButton een juiste hoogte, breedte, ... geven.
            getal1Label.Height = 50;
            getal1Label.FontWeight = FontWeights.Bold;
            getal1Label.Width = 50;
            getal1Label.FontSize = 16;
            getal1Label.Margin = new Thickness(-260, -100, 0, 0);
            

            operatorLabel.Height = 50;
            operatorLabel.Width = 50;
            operatorLabel.FontSize = 16;
            operatorLabel.FontWeight = FontWeights.Bold;
            operatorLabel.Margin = new Thickness(-180, -100, 0, 0);
            

            getal2Label.Height = 50;
            getal2Label.Width = 50;
            getal2Label.FontSize = 16;
            getal2Label.FontWeight = FontWeights.Bold;
            getal2Label.Margin = new Thickness(-100, -100, 0, 0);

            gelijkheidsLabel.Height = 50;
            gelijkheidsLabel.Width = 50;
            gelijkheidsLabel.FontSize = 16;
            gelijkheidsLabel.FontWeight = FontWeights.Bold;
            gelijkheidsLabel.Margin = new Thickness(-20, -100, 0, 0);

            uitkomstTextBox.Height = 30;
            uitkomstTextBox.Width = 120;
            uitkomstTextBox.FontSize = 15;
            uitkomstTextBox.Margin = new Thickness(160, -113, 0, 0);
            
            uitkomstTextBox.Text = "0";

            volgendeHoofdrekenButton.Height = 30;
            volgendeHoofdrekenButton.Width = 100;
            volgendeHoofdrekenButton.FontSize = 12;
            volgendeHoofdrekenButton.Margin = new Thickness(-220, 0, 0, 0);
            volgendeHoofdrekenButton.Content = "Volgende";
            
            volgendeHoofdrekenButton.Click += volgendeHoofdrekenButton_Click;
        }

        private void maakHoofdrekenLayoutOnzichtbaar()
        {
            getal1Label.Visibility = Visibility.Hidden;
            operatorLabel.Visibility = Visibility.Hidden;
            getal2Label.Visibility = Visibility.Hidden;
            gelijkheidsLabel.Visibility = Visibility.Hidden;
            uitkomstTextBox.Visibility = Visibility.Hidden;
            volgendeHoofdrekenButton.Visibility = Visibility.Hidden;
        }

        private void maakHoofdrekenLayoutZichtbaar()
        {
            getal1Label.Visibility = Visibility.Visible;
            operatorLabel.Visibility = Visibility.Visible;
            getal2Label.Visibility = Visibility.Visible;
            gelijkheidsLabel.Visibility = Visibility.Visible;
            uitkomstTextBox.Visibility = Visibility.Visible;
            volgendeHoofdrekenButton.Visibility = Visibility.Visible;
        }

        private void meetkundeButton_Click(object sender, RoutedEventArgs e)
        {
            maakVraagLijst("Vragenmeetkunde.txt");
            maxVraag = vragen.Count;
            MaakVraag();

            hoofdrekenButton.Visibility = Visibility.Hidden;
            meetkundeButton.Visibility = Visibility.Hidden;
            metendRekenenButton.Visibility = Visibility.Hidden;

            maakMeetkundezichtbaar();
        }

        private void maakMeetkundeLayout()
        {
            vraagTextBlock = new TextBlock();
            volgendeMeetkundeButton = new Button();
  
            antwoordGrid.Children.Add(vraagTextBlock);
            antwoordGrid.Children.Add(volgendeMeetkundeButton);     

            vraagTextBlock.Width = 700;
            vraagTextBlock.FontWeight = FontWeights.Bold;
            vraagTextBlock.Height = 30;
            vraagTextBlock.Text = "";
            vraagTextBlock.Margin = new Thickness(0,-250,0,0);

            volgendeMeetkundeButton.Height = 30;
            volgendeMeetkundeButton.Width = 100;
            volgendeMeetkundeButton.FontSize = 12;
            volgendeMeetkundeButton.Margin = new Thickness(-220, 180, 0, 0);
            volgendeMeetkundeButton.Content = "Volgende";

            volgendeMeetkundeButton.Click += volgendeButton_Click;
        }

        private void maakMeetkundeOnzichtbaar()
        {
            vraagTextBlock.Visibility = Visibility.Hidden;
            volgendeMeetkundeButton.Visibility = Visibility.Hidden;
        }

        private void maakMeetkundezichtbaar()
        {
            vraagTextBlock.Visibility = Visibility.Visible;
            volgendeMeetkundeButton.Visibility = Visibility.Visible;
        }

        private void metendRekenenButton_Click(object sender, RoutedEventArgs e)
        {
            maakVraagLijst("VragenmetendRekenen.txt");
            maxVraag = vragen.Count;
            MaakVraag();

            hoofdrekenButton.Visibility = Visibility.Hidden;
            meetkundeButton.Visibility = Visibility.Hidden;
            metendRekenenButton.Visibility = Visibility.Hidden;

            maakMetendRekenenzichtbaar();
        }

        private void maakMetendRekenenLayout()
        {
            vraagTextBlock = new TextBlock();
            volgendeMetendRekenenButton = new Button();

            antwoordGrid.Children.Add(vraagTextBlock);
            antwoordGrid.Children.Add(volgendeMetendRekenenButton);

            vraagTextBlock.Width = 400;
            vraagTextBlock.Height = 15;
            vraagTextBlock.FontWeight = FontWeights.Bold;
            vraagTextBlock.Text = "";
            vraagTextBlock.Margin = new Thickness(0, -250, 0, 0);

            volgendeMetendRekenenButton.Height = 30;
            volgendeMetendRekenenButton.Width = 100;
            volgendeMetendRekenenButton.FontSize = 12;
            volgendeMetendRekenenButton.Margin = new Thickness(-220, 180, 0, 0);
            volgendeMetendRekenenButton.Content = "Volgende";

            volgendeMetendRekenenButton.Click += volgendeButton_Click;
        }

        private void maakMetendRekenenOnzichtbaar()
        {
            vraagTextBlock.Visibility = Visibility.Hidden;
            volgendeMetendRekenenButton.Visibility = Visibility.Hidden;
        }

        private void maakMetendRekenenzichtbaar()
        {
            vraagTextBlock.Visibility = Visibility.Visible;
            volgendeMetendRekenenButton.Visibility = Visibility.Visible;
        }

        private void volgendeButton_Click(object sender, RoutedEventArgs e)
        {
            CheckJuist();
            Reset();
            MaakVraag();
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            if (statusProgressBar.Value != statusProgressBar.Maximum)
            {
                statusProgressBar.Value++;
            }
            else
            {
                uitkomstTextBox.IsEnabled = false;
                timer2.Stop();
            }

        }

        private void timer_Tick(object sender, EventArgs e)
        {
            if (statusProgressBar.Value != statusProgressBar.Maximum)
            {
                statusProgressBar.Value++;
            }
            else
            {
                if (vragen[random].VraagDelen.Length == 2 || vragen[random].VraagDelen.Length == 3 && vragen[random].VraagDelen[2].Split('.')[1] == "gif")
                {
                    foreach (TextBox t in antwoordGrid2.Children)
                    {
                        t.IsEnabled = false;
                    }
                }
                else
                {
                    foreach (RadioButton r in antwoordGrid2.Children)
                    {
                        r.IsEnabled = false;
                    }
                }
                timer.Stop();
            }
        }

        private void maakVraagLijst(string bestandsnaam)
        {
            StreamReader reader = null;
            string tempVraag;
            Vraag v;

            try
            {
                reader = new StreamReader("Wiskunde/" + bestandsnaam);
                tempVraag = reader.ReadLine();

                while (tempVraag != null)
                {
                    if (tempVraag.Split(',').Length == 2 || tempVraag.Split(',').Length == 3 && tempVraag.Split(',')[2].Split('.')[1] == "gif")
                    {
                        v = new Invulvraag(tempVraag, "Wiskunde");
                    }
                    else
                    {
                        v = new MeerkeuzeVraag(tempVraag);
                    }
                    vragen.Add(v);
                    tempVraag = reader.ReadLine();
                }
            }
            catch(FileNotFoundException ex)
            {
                MessageBox.Show("file not found " + ex);
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

            if (gevraagd.Count < aantalVragen)
            {
                while (gevraagd.IndexOf(vragen[random]) != -1)
                {
                    random = rnd.Next(0, maxVraag);
                }
                gevraagd.Add(vragen[random]);

                vraagTextBlock.Text = vragen[random]._Vraag;


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
            else
            {
                Resultaat r = new Resultaat(score, gevraagd, antwoorden, filename, graad, gebruiker);
                r.Left = 400;
                r.Top = 200;
                r.Show();
                this.Close();
            }

           
        }
  
        private void maakInvulVraag()
        {
            //een invullvraag met 1 mogelijk antwoord
            Invulvraag v = (Invulvraag)vragen[random];

            TextBox antwoordTextBox = new TextBox();

            antwoordTextBox.Height = 20;
            antwoordTextBox.Width = 200;
            antwoordGrid2.Children.Add(antwoordTextBox);
            antwoordTextBox.Margin = new Thickness(-125, -200, 0, 0);

            if (vragen[random].VraagDelen.Length == 3 && vragen[random].VraagDelen[2].Split('.')[1] == "gif")
                {
                    vraagImage.Source = v.Src;
                }
            
        }

        private void maakMeerkeuzevraag()
        {
           //een meerkeuzevraag met variabele aantal aan keuzes
            MeerkeuzeVraag v = (MeerkeuzeVraag)vragen[random];
            
            for (int i = 0; i <= v.VraagOpties.Count() -1; i++)
            {
                RadioButton radio = new RadioButton();
                antwoordGrid2.Children.Add(radio);
                radio.Name = "radio" + Convert.ToString(i);
                radio.Content = v.VraagOpties[i];
                radio.Margin = new Thickness(30, -200 + (i * 60), 0, 0);
                radio.Height = 18;
            }
        }

        private void CheckJuist()
        {
            string antwoord = "";
            bool juist = false;

            if (vragen[random].VraagDelen.Length == 2 || vragen[random].VraagDelen.Length == 3 && vragen[random].VraagDelen[2].Split('.')[1] == "gif")
            {
                foreach (TextBox t in antwoordGrid2.Children)
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
                foreach (RadioButton r in antwoordGrid2.Children)
                {
                    antwoord = Convert.ToString(r.Content);

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

            juist = vragen[random].CheckJuist(antwoord);

            if (juist)
            {
                score++;
            }
        }

        private void Reset()
        {
            TextBox[] textremove = new TextBox[1];
            RadioButton[] radioremove = new RadioButton[5];
            int index = 0;

            if (vragen[random].VraagDelen.Length == 2 || vragen[random].VraagDelen.Length == 3)
            {
                foreach (TextBox t in antwoordGrid2.Children)
                {
                    textremove[index] = t;
                }

                for (int i = 0; i <= textremove.Length - 1; i++)
                {
                    antwoordGrid2.Children.Remove(textremove[i]);
                }

                if (vraagImage.Source != null)
                {
                    vraagImage.Source = null;
                }
            }
            else
            {
                foreach (RadioButton r in antwoordGrid2.Children)
                {
                    radioremove[index] = r;
                    index++;
                }

                for (int i = 0; i <= radioremove.Length - 1; i++)
                {
                    antwoordGrid2.Children.Remove(radioremove[i]);
                }
            }
            timer.Stop();
            statusProgressBar.Value = 0;
            
        }

        private void exitItem_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void hoofdMenuItem_Click(object sender, RoutedEventArgs e)
        {
            Startscherm s = new Startscherm(gebruiker);
            s.Left = 400;
            s.Top = 200;
            s.Show();
            this.Close();
        }
  
    }
}
