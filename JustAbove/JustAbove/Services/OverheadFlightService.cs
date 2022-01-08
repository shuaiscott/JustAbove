using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Flurl;
using Flurl.Http;
using JustAbove.Models;
using JustAbove.Utilities;
using Newtonsoft.Json;

namespace JustAbove.Services
{
    public class OverheadFlightService
    {
        private const string OpenSkyDomain = "https://opensky-network.org";
        private const string OpenSkyAllStatesPath = "/api/states/all";

        public static async Task<List<Flight>> GetOverheadFlights(int searchRadius = 10)
        {
            // TODO Get GPS coordinates

            // TODO Expand coordinates by {searchRadius} NM in compass directions to get MinLat, MinLong, MaxLat, MaxLong


            // Submit web request to get flights in search area
            var result = await OpenSkyDomain
                .AppendPathSegment(OpenSkyAllStatesPath)
                .SetQueryParams(new
                {
                    lamin = 40,
                    lamax = 41,
                    lomin = -112,
                    lomax = -111,
                })
                .GetAsync();

            var openSkyResponse = JsonConvert.DeserializeObject<OpenSkyStateResponse>(await result.ResponseMessage.Content.ReadAsStringAsync(), new OpenSkyStateJsonConverter());

            return OpenSkyStateResponse.MapToFlights(openSkyResponse);
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
                        callsign: state.Callsign,
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
