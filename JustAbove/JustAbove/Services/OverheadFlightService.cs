using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
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

        public static async Task<List<Flight>> GetOverheadFlights(int searchRadius = 3)
        {
            // Get GPS coordinates
            Location location = null;

            try
            {
                location = await Geolocation.GetLocationAsync(HighGeolocationRequest);

                #if DEBUG
                if (location != null)
                    Console.WriteLine(
                        $"Latitude: {location.Latitude}, Longitude: {location.Longitude}, Altitude: {location.Altitude}");
                #endif
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
            finally
            {
                if (location == null)
                    // DFW
                    location = new Location(32.899811, -97.040337);
            }

            // Expand coordinates by {searchRadius} NM in compass directions to get MinLat, MinLong, MaxLat, MaxLong

            var conversionFactor = (double) searchRadius / 69;
            var minimumLatitude = location.Latitude - conversionFactor;
            var maximumLatitude = location.Latitude + conversionFactor;
            var minimumLongitude = location.Longitude - conversionFactor;
            var maximumLongitude = location.Longitude + conversionFactor;

            #if DEBUG
            Console.WriteLine("Plot on Map: https://mobisoftinfotech.com/tools/plot-multiple-points-on-map/");
            Console.WriteLine("Geopoints:");
            Console.WriteLine($"{minimumLatitude},{maximumLongitude},red,marker");
            Console.WriteLine($"{minimumLatitude},{minimumLongitude},red,marker");
            Console.WriteLine($"{maximumLatitude},{minimumLongitude},red,marker");
            Console.WriteLine($"{maximumLatitude},{maximumLongitude},red,marker");
            Console.WriteLine($"{minimumLatitude},{maximumLongitude},red,marker");
            Console.WriteLine("");
            Console.WriteLine($"Center on: {location.Latitude},{location.Longitude}");
            #endif

            var client = new HttpClient();

            var uriBuilder = new UriBuilder(OpenSkyDomain)
            {
                Path = OpenSkyAllStatesPath
            };
            var query = HttpUtility.ParseQueryString(uriBuilder.Query);
            query["lamin"] = minimumLatitude.ToString(CultureInfo.InvariantCulture);
            query["lamax"] = maximumLatitude.ToString(CultureInfo.InvariantCulture);
            query["lomin"] = minimumLongitude.ToString(CultureInfo.InvariantCulture);
            query["lomax"] = maximumLongitude.ToString(CultureInfo.InvariantCulture);
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
                var flights = new List<Flight>(response.States?.Count ?? 0);

                if (response.States == null)
                    return flights;

                flights.AddRange(response.States.Select(state => 
                    Flight.Create(
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
                        )));

                return flights;
            }
        }

        public class OpenSkyState
        {
            // ReSharper disable once InconsistentNaming
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
