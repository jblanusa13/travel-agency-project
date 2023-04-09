﻿using ProjectSims.Service;
using ProjectSims.Domain.Model;
using ProjectSims.View.Guest2View.Pages;
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

namespace ProjectSims.View.Guest2View
{
    /// <summary>
    /// Interaction logic for Guest2StartingView.xaml
    /// </summary>
    public partial class Guest2StartingView : Window
    {
        private TourService tourService;
        private ReservationTourService reservationTourService;
        public Guest2 guest2 { get; set; }
        public Guest2StartingView(Guest2 g)
        {
            InitializeComponent();
            guest2 = g;
            UserBox.Text = "Korisnik : " + guest2.Name + " " + guest2.Surname;

            reservationTourService = new ReservationTourService();
            tourService = new TourService();

            int tourId = reservationTourService.GetTourIdWhereGuestIsWaiting(guest2);
            if (tourId != null)
            {
                Tour tour = tourService.GetTourById(tourId);
                MessageBoxResult answer = MessageBox.Show("Da li ste prisutni na turi " + tour.Name + "?", "", MessageBoxButton.YesNo);
                if (answer == MessageBoxResult.Yes)
                {
                    reservationTourService.UpdateGuestState(guest2,tour,Guest2State.Present);
                }
            }
        }

        private void ButtonSearchTour(object sender, RoutedEventArgs e)
        {
            ChangeTab(0);
        }

        private void ButtonShowFinishedTours(object sender, RoutedEventArgs e)
        {
            ChangeTab(1);
        }

        public void ChangeTab(int tabNum)
        {
            switch (tabNum)
            {
                case 0:
                    {
                        SelectedTab.Content = new SearchTourView(guest2);
                        break;
                    }
                case 1:
                    {
                        SelectedTab.Content = new FinishedToursView(guest2);
                        break;
                    }
            }
        }

        private void LogOut(object sender, RoutedEventArgs e)
        {
            var startView = new MainWindow();
            startView.Show();
            Close();
        }
    }
}
