﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using ProjectSims.Domain.Model;
using ProjectSims.Observer;
using ProjectSims.Service;
using ProjectSims.WPF.View.Guest1View.RatingPages;
using ProjectSims.WPF.View.Guest1View;
using ProjectSims.WPF.View.Guest1View.MainPages;
using ProjectSims.WPF.ViewModel.Guest1ViewModel;
using ProjectSims.View.Guest1View;

namespace ProjectSims.WPF.View.Guest1View.MainPages
{
    /// <summary>
    /// Interaction logic for AccommodationReservationView.xaml
    /// </summary>
    public partial class GuestAccommodationsView : Page
    {
        public GuestAccommodationsViewModel ViewModel { get; set; }
        public Guest1 Guest { get; set; }
        public Accommodation SelectedAccommodation { get; set; }

        public GuestAccommodationsView(Guest1 guest)
        {
            InitializeComponent();

            ViewModel = new GuestAccommodationsViewModel(guest);
            this.DataContext = ViewModel;

            Guest = guest;

            HelpButton.Focus();
        }
        private void Theme_Click(object sender, RoutedEventArgs e)
        {
            App app = (App)Application.Current;

            if(App.IsDark)
            {
                app.ChangeTheme(new Uri("Themes/Light.xaml", UriKind.Relative));
                App.IsDark = false;
            }
            else
            {
                app.ChangeTheme(new Uri("Themes/Dark.xaml", UriKind.Relative));
                App.IsDark = true;
            }
        }

        public void Anywhere_Click(object sender, RoutedEventArgs e)
        {
            ChangeTab(1);
        }

        private void MyReservations_Click(object sender, RoutedEventArgs e)
        {
            ChangeTab(2);
        }
    
        private void ShowRatings_Click(object sender, RoutedEventArgs e)
        {
            ChangeTab(4);
        }

        public void LogOut_Click(object sender, RoutedEventArgs e)
        {
            ChangeTab(6);
        }

        private void RateAccommodation_Click(object sender, RoutedEventArgs e)
        {
            RatingStartView accommodationForRating = new RatingStartView(Guest);
            accommodationForRating.Show();    
        }

        private void MyProfile_Click(object sender, RoutedEventArgs e)
        {
            ChangeTab(5);
        }

        private void Forum_Click(object sender, RoutedEventArgs e)
        {
            ChangeTab(0);
        }

        public void ChangeTab(int tabNum)
        {
            switch (tabNum)
            {
                case 0:
                    {
                        NavigationService.Navigate(new Forum(Guest));
                        break;
                    }
                case 1:
                    {
                        NavigationService.Navigate(new AnytimeAnywhere(Guest));
                        break;
                    }
                case 2:
                    {
                        NavigationService.Navigate(new MyReservations(Guest));
                        break;
                    }
                case 4:
                    {
                        NavigationService.Navigate(new RatingsView(Guest));
                        break;
                    }
                case 5:
                    {
                        NavigationService.Navigate(new Profile(Guest));
                        break;
                    }
                case 6:
                    {
                        var login = new MainWindow();
                        login.Show();
                        Window parentWindow = Window.GetWindow(this);
                        parentWindow.Close();
                        break;
                    }
            }
        }

        private void Accommodations_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SelectedAccommodation = (Accommodation)AccommodationsTable.SelectedItem;
        }

        private void OpenReservationView(object sender, KeyEventArgs e)
        {
            if (SelectedAccommodation != null)
            {
                if ((e.Key.Equals(Key.Enter)) || (e.Key.Equals(Key.Return)))
                {
                    NavigationService.Navigate(new AccommodationReservationView(SelectedAccommodation, Guest));
                }
            }
        }


        public void Search_Click(object sender, RoutedEventArgs e)
        {
           ViewModel.Search(TextboxName.Text, TextboxCity.Text, TextboxCountry.Text, TextboxType.Text, TextboxGuests.Text, TextboxDays.Text);
        }
    }

}

