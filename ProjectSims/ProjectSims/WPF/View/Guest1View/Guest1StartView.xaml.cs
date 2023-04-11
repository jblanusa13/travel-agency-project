﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using ProjectSims.Service;
using ProjectSims.Domain.Model;
using ProjectSims.Observer;
using ProjectSims.View;
using ProjectSims.WPF.View.Guest1View.Pages;

namespace ProjectSims.View.Guest1View
{
    /// <summary>
    /// Interaction logic for Guest1View.xaml
    /// </summary>
    public partial class Guest1StartView : Window
    {
        public Guest1StartView(Guest1 guest)
        {
            InitializeComponent();
            SelectedTab.Content = new GuestAccommodationsView(guest, SelectedTab);
        }
    }
}