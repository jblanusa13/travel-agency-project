﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
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
using ProjectSims.Domain.Model;
using ProjectSims.Observer;
using ProjectSims.Service;

namespace ProjectSims.View
{
    /// <summary>
    /// Interaction logic for DateChangeRequest.xaml
    /// </summary>
    public partial class DateChangeRequest : Window, IObserver, INotifyPropertyChanged
    {
        private RequestService requestService;
        private AccommodationReservationService reservationService;

        private int reservationId;
        public Accommodation Accommodation { get; set; }
        public DateOnly CheckInDate { get; set; }
        public DateOnly CheckOutDate { get; set; }
        public int GuestNumber { get; set; }

        public DateOnly DateChange { get; set; }

        public DateChangeRequest(AccommodationReservation selectedReservation)
        {
            InitializeComponent();
            DataContext = this;

            requestService = new RequestService();
            reservationService = new AccommodationReservationService();

            reservationService.Subscribe(this);

            reservationId = selectedReservation.Id;
            Accommodation = selectedReservation.Accommodation;
            CheckInDate = selectedReservation.CheckInDate;
            CheckOutDate = selectedReservation.CheckOutDate;
            GuestNumber = selectedReservation.GuestNumber;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void SendRequest_Click(object sender, RoutedEventArgs e)
        {
            DateChange = DateOnly.FromDateTime((DateTime)DateChangePicker.SelectedDate);
            if (DateChange != null)
            {
                requestService.CreateRequest(reservationId, DateChange);
                Close();
            }
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        public void Update()
        {
            throw new NotImplementedException();
        }
    }
}