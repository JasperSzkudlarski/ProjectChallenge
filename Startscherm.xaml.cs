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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ProjectChallenge
{

    // auteur:  Nele De Backer
    // Datum:   08/04/2015


    public partial class Startscherm : Window
    {
        public int getal = 0;
        private Gebruiker gebruiker;

        public Startscherm(Gebruiker g)
        {
            InitializeComponent();
            gebruiker = g;

            if(g.Email != "admin@admin.com")
            {
                voegToeButton.Visibility = System.Windows.Visibility.Hidden;
                verwijderButton.Visibility = System.Windows.Visibility.Hidden;
                zoekLabel.Visibility = System.Windows.Visibility.Hidden;
                zoekButton.Visibility = System.Windows.Visibility.Hidden;
                voegtoeMenuItem.IsEnabled = false;
                verwijderMenuItem.IsEnabled = false;
                rapportenMenuItem.IsEnabled = false;
            }

        }

        private void wiskundeButton_Click(object sender, RoutedEventArgs e)
        {
            Wiskunde wisk = new Wiskunde(getal, gebruiker);
            wisk.Left = 400;
            wisk.Top = 200;
            wisk.Show();
            this.Close();
        }

        private void moeilijkheidsSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            getal = Convert.ToInt32(moeilijkheidsSlider.Value);

            if( getal == 0)
            {
                graadLabel.Content = "Gemakkelijk";
            }
            else if (getal == 1 )
            {
                graadLabel.Content = "Normaal";
            }
            else{
                graadLabel.Content = "Moeilijk";
            }
        }

        private void taalButton_Click(object sender, RoutedEventArgs e)
        {
            Taal taal = new Taal(getal, gebruiker);
            taal.Left = 400;
            taal.Top = 200;
            taal.Show();
            this.Close();

        }

        private void kennisButton_Click(object sender, RoutedEventArgs e)
        {
            Kennis kennis = new Kennis(Convert.ToInt32(moeilijkheidsSlider.Value), gebruiker);
            kennis.Left = 400;
            kennis.Top = 200;
            kennis.Show();
            this.Close();
        }

        private void afsluitButten_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void hoofdMenuItem_Click(object sender, RoutedEventArgs e)
        {
            Startscherm start = new Startscherm(gebruiker);
            start.Left = 400;
            start.Top = 200;
            start.Show();
            this.Close();
        }

        private void wiskundeMenuItem_Click(object sender, RoutedEventArgs e)
        {
            Wiskunde wisk = new Wiskunde(getal, gebruiker);
            wisk.Left = 400;
            wisk.Top = 200;
            wisk.Show();
            this.Close();
        }

        private void taalMenuItem_Click(object sender, RoutedEventArgs e)
        {
            Taal taal = new Taal(getal, gebruiker);
            taal.Left = 400;
            taal.Top = 200;
            taal.Show();
            this.Close();

        }

        private void kennisMenuItem_Click(object sender, RoutedEventArgs e)
        {
            Kennis kennis = new Kennis(Convert.ToInt32(moeilijkheidsSlider.Value), gebruiker);
            kennis.Left = 400;
            kennis.Top = 200;
            kennis.Show();
            this.Close();
        }

        private void exitItem_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void zoekButton_Click(object sender, RoutedEventArgs e)
        {
            Overzicht o = new Overzicht(gebruiker);
            o.Left = 400;
            o.Top = 200;
            o.Show();
            this.Close();
        }

        private void voegToeButton_Click(object sender, RoutedEventArgs e)
        {
            NieuweVraag nv = new NieuweVraag(gebruiker);
            nv.Left = 400;
            nv.Top = 200;
            nv.Show();
            this.Close();
        }

        private void verwijderButton_Click(object sender, RoutedEventArgs e)
        {
            VerwijderVraag v = new VerwijderVraag(gebruiker);
            v.Left = 400;
            v.Top = 200;
            v.Show();
            this.Close();
        }

        private void voegtoeMenuItem_Click(object sender, RoutedEventArgs e)
        {
            NieuweVraag n = new NieuweVraag(gebruiker);
            n.Left = 400;
            n.Top = 200;
            n.Show();
            this.Close();
        }

        private void verwijderMenuItem_Click(object sender, RoutedEventArgs e)
        {
            VerwijderVraag v = new VerwijderVraag(gebruiker);
            v.Left = 400;
            v.Top = 200;
            v.Show();
            this.Close();
        }

        private void rapportenMenuItem_Click(object sender, RoutedEventArgs e)
        {
            Overzicht o = new Overzicht(gebruiker);
            o.Left = 400;
            o.Top = 200;
            o.Show();
            this.Close();
        }

    }
}
