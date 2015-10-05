using System;
using System.Collections;
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
using System.Windows.Threading;

namespace ProjectChallenge
{
    // Nele De Backer 
    // 1/05/2015 - 2/05/2015
    //jasper szkudlarski
    // 4/05/2012

    public partial class Game : Window
    {
        private DispatcherTimer timer;
        private DispatcherTimer animatietimer;
        List<MensSpeler> bolList;
        List<SpelEntiteit> vierkantList;
        List<SpelEntiteit> entiteitList;
        private int aantalGeraakt = 0;
        private Gebruiker gebruiker;

        public Game(Gebruiker gebruiker)
        {
            InitializeComponent();
            timer = new DispatcherTimer();
            timer.Interval = new TimeSpan(0, 0, 1);

            animatietimer = new DispatcherTimer();
            animatietimer.Interval = new TimeSpan(10000);

            vierkantList = new List<SpelEntiteit>();
            bolList = new List<MensSpeler>();
            entiteitList = new List<SpelEntiteit>();


            this.gebruiker = gebruiker;

            statusProgressBar.Maximum = gebruiker.Tijd;
            timer.Tick += timer_Tick;
            animatietimer.Tick += animatietimer_Tick;
            animatietimer.Start();
            timer.Start();

        }

        private void animatietimer_Tick(object sender, EventArgs e)
        {
            foreach (ComputerSpeler item in vierkantList)
            {
                item.Beweeg();
                //item.rect.Margin = new Thickness(item.X, item.Y, 0, 0);
            }
            foreach (MensSpeler item in bolList)
            {
                item.Beweeg();
                //item.Bol.Margin = new Thickness(item.X, item.Y, 0, 0);
            }

            for (int i = 0; i < entiteitList.Count; i++)
            {
                for (int j = 0; j < entiteitList.Count; j++)
                {
                    if (i != j)
                    {
                        entiteitList[i].CheckHit(entiteitList[j]);
                        if (entiteitList[i].GetType() == typeof(MensSpeler))
                        {
                            if (entiteitList[i].Geraakt == true)
                            {
                                aantalGeraakt++;
                            }
                        }
                    }
                }

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
                timer.Stop();
                MessageBoxResult res = MessageBox.Show("Je hebt " + aantalGeraakt + " keer geraakt");
                if (res == MessageBoxResult.OK)
                {
                    gebruiker.SchrijfTijd(gebruiker.Email, gebruiker.Tijd);
                    Startscherm start = new Startscherm(gebruiker);
                    start.Show();
                    start.Left = 400;
                    start.Top = 200;
                    this.Close();
                }
            }

            gebruiker.Tijd--;
        }

        private void bolletjeButton_Click(object sender, RoutedEventArgs e)
        {
            MensSpeler bol = new MensSpeler();
            entiteitList.Add(bol);
            bolList.Add(bol);
            ballCanvas.Children.Add(bol.Bol);
        }

        private void vierkantButton_Click(object sender, RoutedEventArgs e)
        {
            ComputerSpeler vrk = new ComputerSpeler();
            entiteitList.Add(vrk);
            vierkantList.Add(vrk);
            ballCanvas.Children.Add(vrk.Rect);
        }

        private void startButton_Click(object sender, RoutedEventArgs e)
        {
            timer.Stop();
            animatietimer.Stop();
            MessageBoxResult res = MessageBox.Show("Je hebt " + aantalGeraakt + " keer geraakt");
            if (res == MessageBoxResult.OK)
            {
                gebruiker.SchrijfTijd(gebruiker.Email, gebruiker.Tijd);
                Startscherm start = new Startscherm(gebruiker);
                start.Show();
                start.Left = 400;
                start.Top = 200;
                this.Close();
            }


        }
    }

}
