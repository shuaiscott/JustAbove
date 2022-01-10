using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
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

        private const int FlightListSize = 8;

        public FlightsViewModel()
        {
            Flights = new List<Flight>();
            Device.BeginInvokeOnMainThread(async () => await LoadOverheadFlightsAsync());

            Device.StartTimer(TimeSpan.FromSeconds(5), () =>
            {
                Device.BeginInvokeOnMainThread(async () => await LoadOverheadFlightsAsync());
                return true;
            });
        }

        public async Task LoadOverheadFlightsAsync()
        {
            var flights = await OverheadFlightService.GetOverheadFlights();

            Flights = flights;
        }

        public List<Flight> Flights
        {
            set
            {
                if (_flights == value) return;

                _flights = value;

                
                if (_flights.Count < FlightListSize)
                {
                    var blankFlight = Flight.Create("", "                      ", "", DateTime.Now, false, 1);
                    for (int i = _flights.Count; i < FlightListSize; i++)
                    {
                        _flights.Add(blankFlight);
                    }
                }

                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Flights"));
            }
            get => _flights;
        }
    }
}
