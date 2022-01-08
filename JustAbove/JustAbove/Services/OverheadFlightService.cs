using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using JustAbove.Models;
using JustAbove.Utilities;
using Newtonsoft.Json;
using Xamarin.Essentials;

namespace JustAbove.Services
{
    public class OverheadFlightService
    {
        private const string OpenSkyDomain = "https://opensky-network.org";
        private const string OpenSkyAllStatesPath = "/api/states/all";
        private static readonly GeolocationRequest HighGeolocationRequest = new GeolocationRequest(GeolocationAccuracy.High);

        public static async Task<List<Flight>> GetOverheadFlights(int searchRadius = 10)
        {
            // TODO Get GPS coordinates
            try
            {
                var location = await Geolocation.GetLocationAsync(HighGeolocationRequest);

                if (location != null)
                {
                    Console.WriteLine($"Latitude: {location.Latitude}, Longitude: {location.Longitude}, Altitude: {location.Altitude}");
                }
            }
            catch (FeatureNotSupportedException fnsEx)
            {
                // Handle not supported on device exception
            }
            catch (FeatureNotEnabledException fneEx)
            {
                // Handle not enabled on device exception
            }
            catch (PermissionException pEx)
            {
                // Handle permission exception
            }
            catch (Exception ex)
            {
                // Unable to get location
            }

            // TODO Expand coordinates by {searchRadius} NM in compass directions to get MinLat, MinLong, MaxLat, MaxLong


            HttpClient client = new HttpClient();

            var uriBuilder = new UriBuilder(OpenSkyDomain);
            uriBuilder.Path = OpenSkyAllStatesPath;
            var query = HttpUtility.ParseQueryString(uriBuilder.Query);
            query["lamin"] = "39";
            query["lamax"] = "42";
            query["lomin"] = "-113";
            query["lomax"] = "-110";
            uriBuilder.Query = query.ToString();

            var result = await client.GetAsync(uriBuilder.ToString());
            if (result.IsSuccessStatusCode)
            {
                var openSkyResponse = JsonConvert.DeserializeObject<OpenSkyStateResponse>(await result.Content.ReadAsStringAsync(), new OpenSkyStateJsonConverter());

                return OpenSkyStateResponse.MapToFlights(openSkyResponse);
            }

            return new List<Flight>();

        }


        public class OpenSkyStateResponse
        {
            public int Time { get; set; }
            public List<OpenSkyState> States { get; set; }

            public static List<Flight> MapToFlights(OpenSkyStateResponse response)
            {
                var flights = new List<Flight>(response.States.Count);
            
                foreach (var state in response.States)
                {

                    var flight = Flight.Create(
                        icao24: state.ICAO24,
                        callsign: state.Callsign.Trim(),
                        originCountry: state.OriginCountry,
                        lastContact: state.LastContact,
                        onGround: state.OnGround,
                        positionSource: state.PositionSource,
                        longitude: state.Longitude,
                        latitude: state.Latitude,
                        barometricAltitude: state.BarometricAltitude,
                        velocity: state.Velocity,
                        trueTrack: state.TrueTrack,
                        verticalRate: state.VerticalRate,
                        timePosition: state.TimePosition,
                        geometricAltitude: state.GeometricAltitude
                    );
                    flights.Add(flight);
                }
            
                return flights;
            }
        }

        public class OpenSkyState
        {
            public string ICAO24 { get; set; }
            public string Callsign { get; set; }
            public string OriginCountry { get; set; }
            public DateTime? TimePosition { get; set; }
            public DateTime LastContact { get; set; }
            public double? Longitude { get; set; }
            public double? Latitude { get; set; }
            public double? BarometricAltitude { get; set; }
            public bool OnGround { get; set; }
            public double? Velocity { get; set; }
            public double? TrueTrack { get; set; }
            public double? VerticalRate { get; set; }
            public double? GeometricAltitude { get; set; }
            public int PositionSource { get; set; }
        }
    }
}
