using System;
using System.Collections.Generic;
using System.Text;

namespace JustAbove.Models
{
    internal class Flight
    {
        public static Flight Create(
            string icao24,
            string callsign,
            string originCountry,
            DateTime lastContact,
            bool onGround,
            int positionSource,
            float? longitude = null,
            float? latitude = null,
            float? barometricAltitude = null,
            float? velocity = null,
            float? trueTrack = null,
            float? verticalRate = null,
            DateTime? timePosition = null,
            float? geometricAltitude = null)
        {
            // TODO - Add validations here

            return new Flight(
                icao24: icao24,
                callsign: callsign,
                originCountry: originCountry,
                lastContact: lastContact,
                onGround: onGround,
                positionSource: positionSource,
                longitude: longitude,
                latitude: latitude,
                barometricAltitude: barometricAltitude,
                velocity: velocity,
                trueTrack: trueTrack,
                verticalRate: verticalRate,
                timePosition: timePosition,
                geometricAltitude: geometricAltitude);
        }

        private Flight(
            string icao24,
            string callsign,
            string originCountry,
            DateTime lastContact,
            bool onGround,
            int positionSource,
            float? longitude = null,
            float? latitude = null,
            float? barometricAltitude = null,
            float? velocity = null,
            float? trueTrack = null,
            float? verticalRate = null,
            DateTime? timePosition = null,
            float? geometricAltitude = null
            )
        {
            ICAO24 = icao24;
            Callsign = callsign;
            OriginCountry = originCountry;
            LastContact = lastContact;
            OnGround = onGround;
            PositionSource = positionSource;
            Longitude = longitude;
            Latitude = latitude;
            BarometricAltitude = barometricAltitude;
            Velocity = velocity;
            TrueTrack = trueTrack;
            VerticalRate = verticalRate;
            TimePosition = timePosition;
            GeometricAltitude = geometricAltitude;
        }

        public string ICAO24 { get; private set; } 
        public string Callsign { get; private set; }
        public string OriginCountry { get; private set; }
        public DateTime? TimePosition { get; private set; }
        public DateTime LastContact { get; private set; }
        public float? Longitude { get; private set; }
        public float? Latitude { get; private set; }
        public float? BarometricAltitude { get; private set; }
        public bool OnGround { get; private set; }
        public float? Velocity { get; private set; }
        public float? TrueTrack { get; private set; }
        public float? VerticalRate { get; private set; }
        public float? GeometricAltitude { get; private set; }
        public int PositionSource { get; private set; }
    }
}
