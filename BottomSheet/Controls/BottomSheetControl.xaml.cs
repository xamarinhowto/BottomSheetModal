using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DarkIce.Toolkit.Core.Utilities;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace BottomSheet.Controls
{
    public partial class BottomSheetControl : ContentView
    {
        #region Constructors & initialisation

        public BottomSheetControl()
        {
            InitializeComponent();
        }

        protected override void OnBindingContextChanged()
        {
            try
            {
                base.OnBindingContextChanged();
                PanContainerRef.Content.TranslationY = SheetHeight + 60;
            }
            catch (Exception ex)
            {
                ex.Log();
            }
        }

        #endregion


        #region Properties

        public static BindableProperty SheetHeightProperty = BindableProperty.Create(
            nameof(SheetHeight),
            typeof(double),
            typeof(BottomSheetControl),
            defaultValue: default(double),
            defaultBindingMode: BindingMode.TwoWay);

        public double SheetHeight
        {
            get { return (double)GetValue(SheetHeightProperty); }
            set { SetValue(SheetHeightProperty, value); OnPropertyChanged(); }
        }

        public static BindableProperty SheetContentProperty = BindableProperty.Create(
            nameof(SheetContent),
            typeof(View),
            typeof(BottomSheetControl),
            defaultValue: default(View),
            defaultBindingMode: BindingMode.TwoWay);

        public View SheetContent
        {
            get { return (View)GetValue(SheetContentProperty); }
            set { SetValue(SheetContentProperty, value); OnPropertyChanged(); }
        }

        #endregion

        uint duration = 250;
        double openPosition = (DeviceInfo.Platform == DevicePlatform.Android) ? 20 : 60;
        double currentPosition = 0;

        public async void PanGestureRecognizer_PanUpdated(object sender, PanUpdatedEventArgs e)
        {
            try
            {
                if (e.StatusType == GestureStatus.Running)
                {
                    currentPosition = e.TotalY;
                    if (e.TotalY > 0)
                    {
                        PanContainerRef.Content.TranslationY = openPosition + e.TotalY;
                    }
                }
                else if (e.StatusType == GestureStatus.Completed)
                {
                    var threshold = SheetHeight * 0.55;

                    if (currentPosition < threshold)
                    {
                        await OpenSheet();
                    }
                    else
                    {
                        await CloseSheet();
                    }
                }
            }
            catch (Exception ex)
            {
                ex.Log();
            }
        }

        public async Task OpenSheet()
        {
            try
            {
                await Task.WhenAll
                (
                    Backdrop.FadeTo(0.4, length: duration),
                    Sheet.TranslateTo(0, openPosition, length: duration, easing: Easing.SinIn)
                );

                BottomSheetRef.InputTransparent = Backdrop.InputTransparent = false;
            }
            catch (Exception ex)
            {
                ex.Log();
            }
        }

        public async Task CloseSheet()
        {
            try
            {
                await Task.WhenAll
                (
                    Backdrop.FadeTo(0, length: duration),
                    PanContainerRef.Content.TranslateTo(x: 0, y: SheetHeight + 60, length: duration, easing: Easing.SinIn)
                );

                BottomSheetRef.InputTransparent = Backdrop.InputTransparent = true;
            }
            catch (Exception ex)
            {
                ex.Log();
            }
        }

        async void TapGestureRecognizer_Tapped(System.Object sender, System.EventArgs e)
        {
            try
            {
                await CloseSheet();
            }
            catch (Exception ex)
            {
                ex.Log();
            }
        }
    }
}
