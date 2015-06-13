using System;
using Windows.Devices.Gpio;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace LEDSwitch
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private const int LED_PIN = 6;
        private const int SW_PIN = 5;
        private GpioPin ledpin;
        private GpioPin swpin;

        public MainPage()
        {
            this.InitializeComponent();

            Unloaded += MainPage_Unloaded;

            InitGPIO();
        }

        private void MainPage_Unloaded(object sender, RoutedEventArgs e)
        {
            ledpin.Dispose();
            swpin.Dispose();
        }

        private void InitGPIO()
        {
            var gpio = GpioController.GetDefault();

            ledpin = gpio.OpenPin(LED_PIN, GpioSharingMode.Exclusive);

            ledpin.SetDriveMode(GpioPinDriveMode.Output);

            swpin = gpio.OpenPin(SW_PIN, GpioSharingMode.Exclusive);

            swpin.SetDriveMode(GpioPinDriveMode.Input);
            swpin.DebounceTimeout = new TimeSpan(200);

            swpin.ValueChanged += Swpin_ValueChanged;
        }

        private void Swpin_ValueChanged(GpioPin sender, GpioPinValueChangedEventArgs args)
        {
            if (args.Edge != GpioPinEdge.RisingEdge) return;

            var ledval = ledpin.Read() == GpioPinValue.High ? GpioPinValue.Low : GpioPinValue.High;

            ledpin.Write(ledval);
        }
    }
}
