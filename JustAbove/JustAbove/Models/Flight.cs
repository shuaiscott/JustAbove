using System;
using System.Collections.Generic;
using System.Text;

namespace JustAbove.Models
{
    public class Flight
    {
        public static Flight Create(
            string icao24,
            string callsign,
            string originCountry,
            DateTime lastContact,
            bool onGround,
            int positionSource,
            double? longitude = null,
            double? latitude = null,
            double? barometricAltitude = null,
            double? velocity = null,
            double? trueTrack = null,
            double? verticalRate = null,
            DateTime? timePosition = null,
            double? geometricAltitude = null)
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
            double? longitude = null,
            double? latitude = null,
            double? barometricAltitude = null,
            double? velocity = null,
            double? trueTrack = null,
            double? verticalRate = null,
            DateTime? timePosition = null,
            double? geometricAltitude = null
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
        public double? Longitude { get; private set; }
        public double? Latitude { get; private set; }
        public double? BarometricAltitude { get; private set; }
        public bool OnGround { get; private set; }
        public double? Velocity { get; private set; }
        public double? TrueTrack { get; private set; }
        public double? VerticalRate { get; private set; }
        public double? GeometricAltitude { get; private set; }
        public int PositionSource { get; private set; }
    }
}
