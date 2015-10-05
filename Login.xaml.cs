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

namespace ProjectChallenge
{
    // Auteur:  Thomas Ven
    // Datum:   12/04/2015

    public partial class Login : Window
    {
        private Gebruiker gebruiker;
        public Login()
        {
            InitializeComponent();

            this.Left = 400;
            this.Top = 200;
        }

        #region events
        private void loginButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                checkLogin();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void registreerButton_Click(object sender, RoutedEventArgs e)
        {
            NieuweUser nieuweUser = new NieuweUser();
            nieuweUser.Left = 400;
            nieuweUser.Top = 200;
            nieuweUser.Show();
            this.Hide();
        }

        private void sluitButton_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void OnKeyDownHandler(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                try
                {
                    checkLogin();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }
        #endregion

        #region methods
        // Gaat zoeken in de txt naar het email adres dat in de textbox staan en de lijn eronder controleren op het wachtwoord.
        private void checkLogin()
        {
            try
            {
                if (emailTextBox.Text.Length == 0) { throw new EmptyFieldException("Vul een emailadres in!"); }
                if (passwordPasswordBox.Password.Length == 0) { throw new EmptyFieldException("Vul een wachtwoord in!"); }

                string mail = Convert.ToString(emailTextBox.Text).ToLower();
                int lijnNummer = ZoekLijnNummer(mail);

                // we hashen de ingave in password box en vergelijken die met het wachtwoord die we vinden door de lijnnummer van het gevonden emailadres met 1 te verhogen.
                if (Hashing.HashIt(passwordPasswordBox.Password).Equals(File.ReadLines("Users/Users.txt").Skip(lijnNummer + 1).Take(1).First()))
                {
                    string naam = File.ReadLines("Users/Users.txt").Skip(lijnNummer - 2).Take(1).First();
                    string achternaam = File.ReadLines("Users/Users.txt").Skip(lijnNummer - 1).Take(1).First();
                    string email = File.ReadLines("Users/Users.txt").Skip(lijnNummer).Take(1).First();
                    string directory = File.ReadLines("Users/Users.txt").Skip(lijnNummer + 2).Take(1).First();
                    int tijd = Convert.ToInt32((File.ReadLines("Users/Users.txt").Skip(lijnNummer + 3).Take(1).First()));
                    gebruiker = new Gebruiker(naam, achternaam, directory, email, tijd);
                    Startscherm start = new Startscherm(gebruiker);
                    start.Left = 400;
                    start.Top = 200;
                    start.Show();
                    this.Hide();

                }
                else
                {
                    MessageBox.Show("Foutief wachtwoord!");
                }
            }
            catch (FileNotFoundException)
            {
                MessageBox.Show("file not found !");
            }
        }

        private int ZoekLijnNummer(string e)
        {
            int teller = 0;
            int lijnNummer = 0;
            string email = e;
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
                return lijnNummer;
            }
            catch (FileNotFoundException)
            {
                MessageBox.Show("file users.txt not found");
                return lijnNummer;
            }
        }
        #endregion
    }
}
