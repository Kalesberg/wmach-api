using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API.Models
{
    public class EquipmentGPSData
    {
        public EquipmentHeader EquipmentHeader { get; set; }
        public Location Location { get; set; }
        public CumulativeOperatingHours CumulativeOperatingHours { get; set; }
        public FuelUsedLast24 FuelUsedLast24 { get; set; }
        public Distance Distance { get; set; }
        public string NearestPlace { get; set; }
        public string Address { get; set; }
    }

    public class EquipmentHeader
    {
        public string Make { get; set; }
        public string Model { get; set; }
        public string EquipmentID { get; set; }
        public string SerialNumber { get; set; }
    }

    public class Location
    {
        public string datetime { get; set; }
        public float Latitude { get; set; }
        public float Longitude { get; set; }
        public Altitude Altitude { get; set; }
        public string AltitudeUnits { get; set; }
    }

    public class Altitude
    {
        public string AltitudeUnits { get; set; }
        public int Meters { get; set; }
    }

    public class CumulativeOperatingHours
    {
        public string datetime { get; set; }
        public string Hour { get; set; }
    }

    public class FuelUsedLast24
    {
        public string datetime { get; set; }
        public string FuelUnits { get; set; }
        public int FuelConsumed { get; set; }
    }

    public class Distance
    {
        public string datetime { get; set; }
        public string OdometerUnits { get; set; }
        public int Odometer { get; set; }
        public string ResetDateTime { get; set; }
    }


}