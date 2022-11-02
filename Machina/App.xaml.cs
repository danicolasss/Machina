﻿using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Machina
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

             MainPage = new NavigationPage(new MainPage());
            //MainPage = new ScannerPage(null);
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
