using System;
using System.Collections.Generic;
using System.Text;
using JustAbove.Services;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace JustAbove.Utilities
{
    public class OpenSkyStateJsonConverter : JsonConverter<OverheadFlightService.OpenSkyState>
    {
        public override void WriteJson(JsonWriter writer, OverheadFlightService.OpenSkyState value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override OverheadFlightService.OpenSkyState ReadJson(JsonReader reader, Type objectType, OverheadFlightService.OpenSkyState existingValue,
            bool hasExistingValue, JsonSerializer serializer)
        {
            var array = JArray.Load(reader);

            var timePosition = array[3] != null ? DateTimeOffset.FromUnixTimeSeconds((long) array[3]).DateTime : (DateTime?) null;
            var lastContact = DateTimeOffset.FromUnixTimeSeconds((long)array[4]).DateTime;

            return new OverheadFlightService.OpenSkyState
            {
                ICAO24 = (string)array[0],
                Callsign = (string)array[1],
                OriginCountry = (string)array[2],
                TimePosition = timePosition,
                LastContact = lastContact,
                Longitude = (double?)array[5],
                Latitude = (double?)array[6],
                BarometricAltitude = (double?)array[7],
                OnGround = (bool)array[8],
                Velocity = (double?)array[9],
                TrueTrack = (double?)array[10],
                VerticalRate = (double?)array[11],
                GeometricAltitude = (double?)array[13],
                PositionSource = (int)array[16],
            };
        }
    }
}
