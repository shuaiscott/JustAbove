using System;
using System.ComponentModel;
using Xamarin.Forms;

namespace JustAbove.ViewModels
{
    internal class FlightsViewModel : INotifyPropertyChanged
    {
        private string _flightNum;

        public event PropertyChangedEventHandler PropertyChanged;

        public FlightsViewModel()
        {
            this.FlightNum = "12345";

            Device.StartTimer(TimeSpan.FromSeconds(1), () =>
            {
                this.FlightNum = new Random().Next(10000, 90000).ToString();
                return true;
            });
        }

        public string FlightNum
        {
            set
            {
                if (_flightNum == value) return;

                _flightNum = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("FlightNum"));
            }
            get => _flightNum;
        }
    }
}
