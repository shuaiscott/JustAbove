using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using System.Windows.Input;
using JustAbove.Models;
using JustAbove.Services;
using Xamarin.Forms;

namespace JustAbove.ViewModels
{
    internal class FlightsViewModel : INotifyPropertyChanged
    {
        private List<Flight> _flights;

        public event PropertyChangedEventHandler PropertyChanged;

        public FlightsViewModel()
        {
            Flights = new List<Flight>();

            LoadOverheadFlightsCommand = new Command(async () => await LoadOverheadFlightsAsync());
        }

        public async Task LoadOverheadFlightsAsync()
        {
            Flights = await OverheadFlightService.GetOverheadFlights();
        }

        public ICommand LoadOverheadFlightsCommand { protected set; get; }

        public List<Flight> Flights
        {
            set
            {
                if (_flights == value) return;

                _flights = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Flights"));
            }
            get => _flights;
        }
    }
}
