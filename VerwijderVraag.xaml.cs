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

namespace ProjectChallenge
{
    /// Auteur: jasper Szkudlarski
    /// datum: 30/04/2015
   
    public partial class VerwijderVraag : Window
    {
        private string sufix;
        private string pad;
        private List<string> vragen;
        private int vraagnr;
        private Gebruiker gebruiker;

        public VerwijderVraag(Gebruiker gebruiker)
        {
            InitializeComponent();

            this.gebruiker = gebruiker;

            sufix = "";
            pad = "";
            vraagnr = 0;
            vragen = new List<string>();
        }

        private void vakComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            if (vakComboBox.SelectedValue.ToString() == "Taal")
            {
                metendRekenenRadioButton.Visibility = System.Windows.Visibility.Hidden;
                meetkundeRadioButton.Visibility = System.Windows.Visibility.Hidden;

                makkelijkRadioButton.Visibility = System.Windows.Visibility.Visible;
                moeilijkRadioButton.Visibility = System.Windows.Visibility.Visible;
            }
            else if (vakComboBox.SelectedValue.ToString() == "Wiskunde")
            {
                makkelijkRadioButton.Visibility = System.Windows.Visibility.Hidden;
                moeilijkRadioButton.Visibility = System.Windows.Visibility.Hidden;

                metendRekenenRadioButton.Visibility = System.Windows.Visibility.Visible;
                meetkundeRadioButton.Visibility = System.Windows.Visibility.Visible;
            }
            else
            {
                makkelijkRadioButton.Visibility = System.Windows.Visibility.Hidden;
                moeilijkRadioButton.Visibility = System.Windows.Visibility.Hidden;

                metendRekenenRadioButton.Visibility = System.Windows.Visibility.Hidden;
                meetkundeRadioButton.Visibility = System.Windows.Visibility.Hidden;

                makkelijkRadioButton.IsChecked = false;
                moeilijkRadioButton.IsChecked = false;
                meetkundeRadioButton.IsChecked = false;
                meetkundeRadioButton.IsChecked = false;

                sufix = "";
            }
        
        }

        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            RadioButton button = (RadioButton)sender;
            sufix = Convert.ToString(button.Content);
        }

        private void maakVraagLijst()
        {
            StreamReader reader =null;
            string tempvraag;

            vragen.RemoveRange(0, vragen.Count);

            try
            {
                pad = System.IO.Path.Combine(vakComboBox.SelectedValue.ToString(), "Vragen" + sufix + ".txt");
                reader = new StreamReader(pad);
                tempvraag = reader.ReadLine();

                while (tempvraag != null)
                {
                    vragen.Add(tempvraag);
                    tempvraag = reader.ReadLine();
                }
            }
            catch (FileNotFoundException)
            {
                MessageBox.Show("file not found " + pad);
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }
            }
            
        }

        private void MaakVraag(int index)
        {
            string[] vraag = new string[10];

            vraag = vragen[index].Split(',');
            vraagTextblock.Text = vraag[0];

            
            if (vraag.Length == 2 || vraag.Length == 3 && vraag[2].Split('.')[1] == "gif")
            {
                Label l = new Label();
                l.Margin = new Thickness(8, 11 * 1, 0, 0);
                l.VerticalAlignment = System.Windows.VerticalAlignment.Top;
                l.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
                l.Height = 26;
                l.Width = 430;
                l.Content = "juist antwoord: " + vraag[1];
                vraagGrid.Children.Add(l);

                if(vraag.Length == 3 && vraag[2].Split('.')[1] == "gif")
                {
                string dir;
                BitmapImage src = new BitmapImage();

                dir= System.IO.Path.Combine(vakComboBox.SelectedValue.ToString(), "afbeeldingen", vraag[2]);
                src.BeginInit();
                src.UriSource = new Uri(dir, UriKind.Relative);
                src.CacheOption = BitmapCacheOption.OnLoad;
                src.EndInit();

                vraagImage.Source = src;
                }
            }
            else 
            {
                for (int i = 1; i <= vraag.Length - 1; i++)
            {
                if (!(Regex.IsMatch(vraag[i], @"^\d+$")))
                {
                    Label l = new Label();
                    l.Margin = new Thickness(8, 11*(i*2), 0, 0);
                    l.VerticalAlignment = System.Windows.VerticalAlignment.Top;
                    l.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
                    l.Height = 26;
                    l.Width = 430;
                    l.Content = "Optie " + i + ": " + vraag[i];
                    vraagGrid.Children.Add(l);
                }
                else
                {
                    Label l = new Label();
                    l.Margin = new Thickness(8, 11 * (i*2), 0, 0);
                    l.VerticalAlignment = System.Windows.VerticalAlignment.Top;
                    l.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
                    l.Height = 26;
                    l.Width = 430;
                    l.Content = "juist antwoord: " + vraag[Convert.ToInt32(vraag[i])];
                    vraagGrid.Children.Add(l);
                }
            }
            }
            
        }

        private void Reset()
        {
            List<Label> labelremove = new List<Label>();

            if (vraagGrid.Children.Count != 0)
            {
                foreach (Label t in vraagGrid.Children)
                {
                    labelremove.Add(t);
                }

                for (int i = 0; i <= labelremove.Count - 1; i++)
                {
                    vraagGrid.Children.Remove(labelremove[i]);
                }
            }

            if(vraagImage.Source != null)
            {
                vraagImage.Source = null;
            }

        }

        private void vorigeButton_Click(object sender, RoutedEventArgs e)
        {
            Reset();
            if (vraagnr > 0)
            {
                vraagnr--;
            }
            else
            {
                MessageBox.Show("eerste vraag in de reeks, kan niet terug");
            }
            MaakVraag(vraagnr);
        }

        private void volgendeButton_Click(object sender, RoutedEventArgs e)
        {
            Reset();
            if (vraagnr < vragen.Count - 1)
            {
                vraagnr++;
            }
            else
            {
                MessageBox.Show("laatste vraag in de reeks, kan niet verder");
            }
            MaakVraag(vraagnr);
        }

        private void terugButton_Click(object sender, RoutedEventArgs e)
        {
            Startscherm start = new Startscherm(gebruiker);
            start.Left = 400;
            start.Top = 200;
            start.Show();
            this.Close();
        }

        private void verwijderButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult res = MessageBox.Show("Weet U zeker dat U deze vraag wilt verwijderen?", "Verwijderen", MessageBoxButton.YesNoCancel, MessageBoxImage.Exclamation);
            
            if (res == MessageBoxResult.Yes)
            {
                vragen.RemoveAt(vraagnr);

                StreamWriter writer = File.CreateText(pad);
                for(int i=0 ; i<=vragen.Count-1 ;i++)
                {
                    writer.WriteLine(vragen[i]);
                }
                writer.Close();

                if(vraagnr != 0)
                {
                    vraagnr--;
                }
                Reset();
                MaakVraag(vraagnr);

            }
        }

        private void leesButton_Click(object sender, RoutedEventArgs e)
        {
            vraagnr = 0;

            Reset();
            maakVraagLijst();
            MaakVraag(vraagnr);
        }

        private void ExitItem_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void hoofdmenuItem_Click(object sender, RoutedEventArgs e)
        {
            Startscherm start = new Startscherm(gebruiker);
            start.Show();
            this.Close();
        }
    }
}
