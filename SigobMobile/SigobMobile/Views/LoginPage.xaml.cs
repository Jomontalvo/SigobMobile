﻿using System;
using System.Collections.Generic;

using Xamarin.Forms;
//using Xamarin.Forms.PlatformConfiguration;
//using Xamarin.Forms.PlatformConfiguration.iOSSpecific;

namespace SigobMobile.Views
{
    public partial class LoginPage : ContentPage
    {
        public LoginPage()
        {
            InitializeComponent();
        }

        void Handle_Focused(object sender, Xamarin.Forms.FocusEventArgs e)
        {
            IconViewPassword.IsVisible = e.IsFocused;
        }
    }
}