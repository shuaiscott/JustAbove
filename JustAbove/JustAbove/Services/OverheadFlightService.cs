using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;
using System.Threading.Tasks;
using Flurl;
using Flurl.Http;
using JustAbove.Models;

namespace JustAbove.Services
{
    internal class OverheadFlightService
    {
        private const string OpenSkyDomain = "https://opensky-network.org";
        private const string OpenSkyAllStatesPath = "/api/states/all";

        public static async Task<List<Flight>> GetOverheadFlights(int searchRadius = 10)
        {
            // Get GPS coordinates

            // Expand coordinates by {searchRadius} NM in compass directions to get MinLat, MinLong, MaxLat, MaxLong


            // Submit web request to get flights in search area
            var result = await OpenSkyDomain
                .AppendPathSegment(OpenSkyAllStatesPath)
                .SetQueryParams(new
                {
                    lamin = 40.3350301,
                    lamax = 40.4542622,
                    lomin = -111.9644165,
                    lomax = -111.7694092,
                })
                .GetJsonAsync<OpenSkyStateResponse>();

            return new List<Flight>();
        }


        private class OpenSkyStateResponse
        {
            public DateTime Time { get; set; }
            public List<List<object>> States { get; set; }
        }
    }
}
