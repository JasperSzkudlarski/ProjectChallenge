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
using Microsoft.Win32;
using System.Text.RegularExpressions;

namespace ProjectChallenge
{
    //auteur: jasper szkudlarski
    //datum: 25/04/2015

    public partial class Overzicht : Window
    {
        private Gebruiker gebruiker;
        private string vak;
        private string[] seplijn;
        private int linenr;
        private int maxlinenr;
        private string path;
        private string lijn;
        private List<string> vragen;

        public Overzicht(Gebruiker gebruiker)
        {
            InitializeComponent();

            this.gebruiker = gebruiker;
            linenr = 0;
            maxlinenr = 0;
            path = "";

            vragen = new List<string>();
        }

        private void zoekButton_Click(object sender, RoutedEventArgs e)
        {
            string naam;
            StreamReader reader = null;
            string lijn;
            try
            {
                Reset();
                fileTextBlock.Text = "";
                linenr = 0;
                vragen.RemoveRange(0, vragen.Count);

                naam = leerlingNaamTextBox.Text;
                vak = vakComboBox.SelectedValue.ToString();
                if (vak == "Alles")
                {
                    vak = "";
                }

                OpenFileDialog dialog = new OpenFileDialog();
                dialog.InitialDirectory = System.IO.Path.Combine("Rapporten", naam, vak);
                dialog.Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*";
                Nullable<bool> res = dialog.ShowDialog();
                if (res == true)
                {
                    path = dialog.FileName;
                }

                reader = new StreamReader(path);
                lijn = reader.ReadLine();
                while (lijn != null)
                {
                    vragen.Add(lijn);
                    lijn = reader.ReadLine();
                }

            }
            catch (FileNotFoundException)
            {
                MessageBox.Show("file not found!");
            }
            catch (ArgumentException)
            {
                MessageBox.Show("argument moet meegeven worden");
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }
            }



            lijn = File.ReadLines(path).Skip(linenr).Take(1).First();
            fileTextBlock.Text = lijn;
            maxlinenr = vragen.Count - 3;

            geefOverzicht();

        }

        private void geefOverzicht()
        {
            if (linenr < maxlinenr)
            {
                linenr++;
            }
            else
            {
                MessageBox.Show("laatste vraag in de reeks, kan niet verder");
            }
            lijn = File.ReadLines(path).Skip(linenr).Take(1).First();
            seplijn = lijn.Split('^');

            Label vraagLabel = new Label();
            vraagLabel.Content = seplijn[0];
            vraagLabel.Margin = new Thickness(10, 10, 0, 0);
            vraagLabel.Height = 33;
            vraagLabel.Width = 560;
            vraagLabel.HorizontalAlignment = HorizontalAlignment.Left;
            vraagLabel.VerticalAlignment = VerticalAlignment.Top;
            overzichtGrid.Children.Add(vraagLabel);

            if (seplijn.Length == 4 || seplijn.Length == 5 && seplijn[2].Split('.')[1] == "gif")
            {
                InvulVraagLabels();
            }
            else
            {
                meerkeuzeVraagLabels();
            }
        }

        private void InvulVraagLabels()
        {
            if (seplijn.Length == 4)
            {
                //Controle toevoegen of invulvraag een Afbeelding bevat.
                Label juistAntwoord = new Label();
                juistAntwoord.Content = "juist antwoord: " + seplijn[1];
                juistAntwoord.Margin = new Thickness(10, 43, 0, 0);
                juistAntwoord.Height = 33;
                juistAntwoord.Width = 560;
                juistAntwoord.HorizontalAlignment = HorizontalAlignment.Left;
                juistAntwoord.VerticalAlignment = VerticalAlignment.Top;
                overzichtGrid.Children.Add(juistAntwoord);

                Label geantwoord = new Label();
                geantwoord.Content = "Antwoord van de leerling: " + seplijn[2];
                geantwoord.Margin = new Thickness(10, 76, 0, 0);
                geantwoord.Height = 33;
                geantwoord.Width = 560;
                geantwoord.HorizontalAlignment = HorizontalAlignment.Left;
                geantwoord.VerticalAlignment = VerticalAlignment.Top;
                overzichtGrid.Children.Add(geantwoord);

                Label juist = new Label();
                juist.Content = "Vraag: " + seplijn[3] + " beantwoord";
                juist.Margin = new Thickness(10, 109, 0, 0);
                juist.Height = 33;
                juist.Width = 560;
                juist.HorizontalAlignment = HorizontalAlignment.Left;
                juist.VerticalAlignment = VerticalAlignment.Top;
                overzichtGrid.Children.Add(juist);
            }
            else
            {
                Label juistAntwoord = new Label();
                juistAntwoord.Content = "juist antwoord: " + seplijn[1];
                juistAntwoord.Margin = new Thickness(10, 43, 0, 0);
                juistAntwoord.Height = 33;
                juistAntwoord.Width = 560;
                juistAntwoord.HorizontalAlignment = HorizontalAlignment.Left;
                juistAntwoord.VerticalAlignment = VerticalAlignment.Top;
                overzichtGrid.Children.Add(juistAntwoord);

                Label geantwoord = new Label();
                geantwoord.Content = "Antwoord van de leerling: " + seplijn[3];
                geantwoord.Margin = new Thickness(10, 76, 0, 0);
                geantwoord.Height = 33;
                geantwoord.Width = 560;
                geantwoord.HorizontalAlignment = HorizontalAlignment.Left;
                geantwoord.VerticalAlignment = VerticalAlignment.Top;
                overzichtGrid.Children.Add(geantwoord);

                Label juist = new Label();
                juist.Content = "Vraag: " + seplijn[4] + " beantwoord";
                juist.Margin = new Thickness(10, 109, 0, 0);
                juist.Height = 33;
                juist.Width = 560;
                juist.HorizontalAlignment = HorizontalAlignment.Left;
                juist.VerticalAlignment = VerticalAlignment.Top;
                overzichtGrid.Children.Add(juist);
            }

        }

        private void meerkeuzeVraagLabels()
        {
            int index = 1;

            Label antwoorden = new Label();
            antwoorden.Content = "Mogelijke antwoorden: ";
            antwoorden.Margin = new Thickness(10, 43, 0, 0);
            antwoorden.Height = 33;
            antwoorden.Width = 560;
            antwoorden.HorizontalAlignment = HorizontalAlignment.Left;
            antwoorden.VerticalAlignment = VerticalAlignment.Top;
            overzichtGrid.Children.Add(antwoorden);

            try
            {
                while (!(Regex.IsMatch(seplijn[index], @"^\d+$")))
                {
                    antwoorden.Content = antwoorden.Content + seplijn[index] + ", ";
                    index++;
                }
            }
            catch (NullReferenceException)
            {
                MessageBox.Show("mag niet null zijn");
            }


            if (Regex.IsMatch(seplijn[index], @"^\d+$"))
            {
                Label juistAntwoord = new Label();
                juistAntwoord.Content = "juist antwoord: " + seplijn[index];
                juistAntwoord.Margin = new Thickness(10, 76, 0, 0);
                juistAntwoord.Height = 33;
                juistAntwoord.Width = 560;
                juistAntwoord.HorizontalAlignment = HorizontalAlignment.Left;
                juistAntwoord.VerticalAlignment = VerticalAlignment.Top;
                overzichtGrid.Children.Add(juistAntwoord);
            }

            Label geantwoord = new Label();
            geantwoord.Content = "Antwoord van de leerling: " + seplijn[index + 1];
            geantwoord.Margin = new Thickness(10, 109, 0, 0);
            geantwoord.Height = 33;
            geantwoord.Width = 560;
            geantwoord.HorizontalAlignment = HorizontalAlignment.Left;
            geantwoord.VerticalAlignment = VerticalAlignment.Top;
            overzichtGrid.Children.Add(geantwoord);

            Label juist = new Label();
            juist.Content = "Vraag: " + seplijn[index + 2] + " beantwoord";
            juist.Margin = new Thickness(10, 142, 0, 0);
            juist.Height = 33;
            juist.Width = 560;
            juist.HorizontalAlignment = HorizontalAlignment.Left;
            juist.VerticalAlignment = VerticalAlignment.Top;
            overzichtGrid.Children.Add(juist);
        }

        private void Reset()
        {
            List<Label> labelremove = new List<Label>();

            if (overzichtGrid.Children.Count != 0)
            {
                foreach (Label t in overzichtGrid.Children)
                {
                    labelremove.Add(t);
                }

                for (int i = 0; i <= labelremove.Count - 1; i++)
                {
                    overzichtGrid.Children.Remove(labelremove[i]);
                }
            }

        }

        private void vorigeButton_Click(object sender, RoutedEventArgs e)
        {
            Reset();
            if (linenr > 1)
            {
                linenr -= 2;
            }
            else
            {
                MessageBox.Show("eerste vraag in de reeks, kan niet terug");
            }
            geefOverzicht();
        }

        private void volgendeButton_Click(object sender, RoutedEventArgs e)
        {
            Reset();
            geefOverzicht();
        }

        private void terugButton_Click(object sender, RoutedEventArgs e)
        {
            Startscherm start = new Startscherm(gebruiker);
            start.Left = 400;
            start.Top = 200;
            start.Show();
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

    }
}
