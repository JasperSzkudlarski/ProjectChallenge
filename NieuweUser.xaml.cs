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
using System.ComponentModel.DataAnnotations;


namespace ProjectChallenge
{
    // Auteur:  Thomas Ven
    // Datum:   12/04/2015

    /// <summary>
    /// TO DO
    /// FileNotFoundExceptions
    //m:   11/04/2015


    public partial class NieuweUser : Window
    {
        public NieuweUser()
        {
            InitializeComponent();
        }

        #region events
        private void terugButton_Click(object sender, RoutedEventArgs e)
        {
            Login l = new Login();
            l.Left = 400;
            l.Top = 200;
            l.Show();
            this.Close();
        }
        
        private void voegToeButton_Click(object sender, RoutedEventArgs e)
        {
            string directory = MaakPad();
            try
            {
                Check();
                if (IsMailGebruikt())
                {
                    MessageBox.Show("email is used!!!");
                }
                else
                {
                    System.IO.Directory.CreateDirectory(directory);
                    System.IO.Directory.CreateDirectory(System.IO.Path.Combine(directory, "Kennis"));
                    System.IO.Directory.CreateDirectory(System.IO.Path.Combine(directory, "Taal"));
                    System.IO.Directory.CreateDirectory(System.IO.Path.Combine(directory, "Wiskunde"));
                    SchrijfTxt(directory);
                    Login back = new Login();
                    back.Left = 400;
                    back.Top = 200;
                    back.Show();
                    this.Hide();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void exitItem_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
        #endregion

        #region Methods
        // geschreven op  17/04
        private bool IsMailGebruikt()
        {
            int teller = 0;
            bool gebruikt = false;
            string lijn;
            // users.txt lijn per lijn lezen en zoeken naar de mailTextBox.Text
            StreamReader file =null;

            try
            {
                file = new StreamReader("Users/Users.txt");
                while ((lijn = file.ReadLine()) != null)
                {
                    if (lijn.Contains(emailTextBox.Text.ToLower()))
                    {
                        gebruikt = true;
                    }
                    teller++;
                }
            }
            catch (FileNotFoundException)
            {
                MessageBox.Show("file users.txt in users not found!");
            }
            finally
            {
                if (file != null)
                {
                    file.Close();
                }
            }
            if (gebruikt)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        // geschreven op 15/04
        private void SchrijfTxt(string directory)
        {
            // schrijven in de txt met de naam users.txt
            using (StreamWriter user = File.AppendText("Users/Users.txt"))
            {
                user.WriteLine("*****************************************************");
                user.WriteLine(voornaamTextBox.Text);
                user.WriteLine(achternaamTextBox.Text);
                user.WriteLine(emailTextBox.Text.ToLower());
                user.WriteLine(Hashing.HashIt(paswoordPasswordBox.Password));
                user.WriteLine(directory);
                user.WriteLine("0");
            }
        }

        // geschreven op 25/04
        private string MaakPad()
        {
            int teller = 1;
            string filePath = voornaamTextBox.Text + " " + achternaamTextBox.Text;
            string combinedPath = System.IO.Path.Combine("Rapporten", filePath);
            while (System.IO.Directory.Exists(combinedPath))
            {
                if (System.IO.Directory.Exists(combinedPath + Convert.ToString(teller)))
                {
                    teller++;
                }
                else
                {
                    combinedPath = combinedPath + Convert.ToString(teller);
                }
            }
            return combinedPath;
        }

        // geschreven op 17/04
        private void Check()
        {
            //controles op naam, voornaam en email met exceptions
            if (voornaamTextBox.Text.Length == 0) { throw new EmptyFieldException("Vul een naam in!"); }
            if (achternaamTextBox.Text.Length == 0) { throw new EmptyFieldException("Vul een achternaam in!"); }
            if (emailTextBox.Text.Length == 0) { throw new EmptyFieldException("Vul een emailadres in!"); }
            if (new EmailAddressAttribute().IsValid(emailTextBox.Text) == false) { throw new InvalidMailException("Vul een geldig emailadres in!"); }
            if (paswoordPasswordBox.Password.Length == 0) { throw new EmptyFieldException("Vul een wachtwoord in!"); }
        }
        #endregion



        
    }
}
