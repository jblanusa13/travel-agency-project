﻿using System;
using ProjectSims.Domain.Model;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;
using ProjectSims.FileHandler;
using Microsoft.Win32;
using System.Security;
using System.Collections.ObjectModel;
using ProjectSims.WPF.ViewModel.OwnerViewModel;
using ProjectSims.WPF.View.OwnerView;

namespace ProjectSims.View.OwnerView.Pages
{
    /// <summary>
    /// Interaction logic for AccommodationRegistrationView.xaml
    /// </summary>
    public partial class AccommodationRegistrationView : Page, INotifyPropertyChanged
    {
        public Owner Owner { get; set; }
        public AccommodationRegistrationViewModel accommodationRegistrationViewModel { get; set; }

        public List<string> paths = new List<string>();

        private string _accommodationName;
        public string AccommodationName
        {
            get => _accommodationName;

            set
            {
                if (value != _accommodationName)
                {
                    _accommodationName = value;
                    OnPropertyChanged();
                }
            }
        }

        private string _location;
        public string Location
        {
            get => _location;

            set
            {
                if (value != _location)
                {
                    _location = value;
                    OnPropertyChanged();
                }
            }
        }

        private AccommodationType _type;
        public AccommodationType Type
        {
            get => _type;

            set
            {
                if (value != _type)
                {
                    _type = value;
                    OnPropertyChanged();
                }
            }
        }

        private int _guestsMaximum;
        public int GuestsMaximum
        {
            get => _guestsMaximum;

            set
            {
                if (value != _guestsMaximum)
                {
                    _guestsMaximum = value;
                    OnPropertyChanged();
                }
            }
        }

        private int _minimumReservationDays;
        public int MinimumReservationDays
        {
            get => _minimumReservationDays;

            set
            {
                if (value != _minimumReservationDays)
                {
                    _minimumReservationDays = value;
                    OnPropertyChanged();
                }
            }
        }

        private int _dismissalDays = 1;
        public int DismissalDays
        {
            get => _dismissalDays;

            set
            {
                if (value != _dismissalDays)
                {
                    _dismissalDays = value;
                    OnPropertyChanged();
                }
            }
        }

        private Image _images;
        public Image Images
        {
            get => _images;

            set
            {
                if (value != _images)
                {
                    _images = value;
                    OnPropertyChanged();
                }
            }
        }

        public AccommodationRegistrationView(Owner o)
        {
            InitializeComponent();
            Owner = o;
            accommodationRegistrationViewModel = new AccommodationRegistrationViewModel(Owner);
            this.DataContext = accommodationRegistrationViewModel;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        private void RegisterAccommodation_Click(object sender, RoutedEventArgs e)
        {
            List<string> Pics = new List<string>();
            foreach (string path in paths)
            {
                Pics.Add(path);
            }
            accommodationRegistrationViewModel.RegisterAccommodation(LocationTextBox.Text, Pics, AccommodationName, Type, GuestsMaximum, MinimumReservationDays, DismissalDays);
            OwnerStartingView ownerStartingView = (OwnerStartingView)Window.GetWindow(this);
            ownerStartingView.ChangeTab(4);
        }

        private void LoadImages_Click(object sender, RoutedEventArgs e)
        {
            InitializeOpenFileDialog();
        }

        private void InitializeOpenFileDialog()
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.Multiselect = true;
            bool? success = fileDialog.ShowDialog();

            if (success == true)
            {
                foreach (var filename in fileDialog.FileNames)
                {
                    paths.Add(filename);
                    InitializeImages(filename);
                }
            }
            else
            {
                //Didn't pick anything
            }
        }

        private void InitializeImages(string path)
        {
            BitmapImage bitmap = new BitmapImage();
            bitmap.BeginInit();
            bitmap.UriSource = new Uri(path, UriKind.Absolute);
            bitmap.EndInit();
            Images = new Image();
            Images.Source = bitmap;
            Images.Width = 170;
            Images.Height = 100;
            ImageList.Items.Add(Images);
        }
    }
}