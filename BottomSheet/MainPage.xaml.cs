using System;
using System.Collections.Generic;
using DarkIce.Toolkit.Core.Utilities;
using Xamarin.Forms;

namespace BottomSheet
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        async void Button_Clicked(System.Object sender, System.EventArgs e)
        {
            try
            {
                await Sheet.OpenSheet();
            }
            catch (Exception ex)
            {
                ex.Log();
            }
        }
    }
}
