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

namespace ProjectChallenge
{
    ///auteur: jasper szkudlarski
    ///datum : 30/04/2015

    public partial class NieuweVraag : Window
    {
        private string sufix;
        private string pad;
        private Gebruiker gebruiker;

        public NieuweVraag(Gebruiker gebruiker)
        {
            InitializeComponent();

            this.gebruiker = gebruiker;
            sufix = "";
            pad = "";
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

        private void voegToeButton_Click(object sender, RoutedEventArgs e)
        {
            string vraag;

            pad = System.IO.Path.Combine(vakComboBox.SelectedValue.ToString(), "Vragen" + sufix + ".txt");
            vraag = MaakString();

            try
            {
                StreamWriter writer = File.AppendText(pad);
                writer.WriteLine(vraag);
                writer.Close();

                MessageBox.Show("Nieuwe vraag aangemaakt");
            }
            catch(FileNotFoundException)
            {
                MessageBox.Show("error: file not found");
            }
            catch(NullReferenceException )
            {
                MessageBox.Show("het invoerveld is null");
            }
        }

        private void aantalOptiesComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (aantalOptiesComboBox.SelectedValue.ToString() != "1")
            {
                soortLabel.Content = "(meerkeuzevraag)";

                afbeeldingTextBox.Visibility = System.Windows.Visibility.Hidden;
                afbeeldingLabel.Visibility = System.Windows.Visibility.Hidden;
                afbeeldingButton.Visibility = System.Windows.Visibility.Hidden;

                afbeeldingTextBox.Text = "C:\\";
            }
            else
            {
                soortLabel.Content = "(invulvraag)";

                afbeeldingTextBox.Visibility = System.Windows.Visibility.Visible;
                afbeeldingLabel.Visibility = System.Windows.Visibility.Visible;
                afbeeldingButton.Visibility = System.Windows.Visibility.Visible;
            }

            Reset();
            VulGrid();
        }

        private void VulGrid()
        {
            int aantal = Convert.ToInt32(aantalOptiesComboBox.SelectedValue.ToString());

            if (aantal != 1)
            {
                for (int i = 0; i <= aantal-1; i++)
                {
                    Label l = new Label();
                    l.Margin = new Thickness(10, 36 * i, 0, 0);
                    l.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
                    l.VerticalAlignment = System.Windows.VerticalAlignment.Top;
                    l.Content = "Optie " + (i + 1) + ":";
                    labelGrid.Children.Add(l);


                    TextBox t = new TextBox();
                    t.Margin = new Thickness(0, 36 * i, 0, 0);
                    t.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
                    t.VerticalAlignment = System.Windows.VerticalAlignment.Top;
                    t.Text = "";
                    t.Width = 365;
                    t.Height = 23;
                    optiesGrid.Children.Add(t);
                }
            }
        }

        private void Reset()
        {
            List<TextBox> textlist = new List<TextBox>();
            List<Label> labellist = new List<Label>();

            if (optiesGrid.Children.Count != 0 && labelGrid.Children.Count != 0)
            {
                foreach (TextBox t in optiesGrid.Children)
                {
                    textlist.Add(t);
                }

                foreach (Label l in labelGrid.Children)
                {
                    labellist.Add(l);
                }

                for (int i = 0; i <= textlist.Count - 1; i++)
                {
                    optiesGrid.Children.Remove(textlist[i]);
                    labelGrid.Children.Remove(labellist[i]);
                }
            }
        }

        private string MaakString()
        {
            string vraag = vraagTextBox.Text;
            List<TextBox> textlist = new List<TextBox>();

            if (optiesGrid.Children.Count != 0 && labelGrid.Children.Count != 0)
            {
                foreach (TextBox t in optiesGrid.Children)
                {
                    textlist.Add(t);
                }
            }

                for (int i = 0; i <= textlist.Count - 1; i++)
                {
                    vraag = vraag + "," + textlist[i].Text;
                }

                if(textlist.Count > 1)
                {
                    string juistindex = "";

                    for (int i = 0; i <= textlist.Count - 1; i++)
                    {
                      if(textlist[i].Text == antwoordTextBox.Text)
                      {
                          juistindex = Convert.ToString(i+1);
                      }
                    }

                    vraag = vraag + "," + juistindex;
                }
                else
                {
                    vraag = vraag + "," + antwoordTextBox.Text;
                }

                if(afbeeldingTextBox.Text != "C:\\")
                {
                    vraag = vraag + "," + afbeeldingTextBox.Text;
                }
            

            return vraag;
        }

        private void afbeeldingButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);
            dialog.Filter = "Images (*.gif)|*.gif";
            Nullable<bool> res = dialog.ShowDialog();
            if (res == true)
            {
                afbeeldingTextBox.Text = dialog.FileName;
            }
        }
        
        private void terugButton_Click(object sender, RoutedEventArgs e)
        {
            Startscherm s = new Startscherm(gebruiker);
            s.Show();
            this.Close();
        }


        private void ExitItem_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}
