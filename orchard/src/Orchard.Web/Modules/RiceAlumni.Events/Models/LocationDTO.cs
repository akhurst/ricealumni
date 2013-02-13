using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RiceAlumni.Events.Models
{
    public class LocationDTO
    {
        public LocationDTO() { }
        public LocationDTO(LocationPart locationPart)
            : this()
        {
            if (locationPart == null) return;

            Name = locationPart.Name;
            Building = locationPart.Building;
            Room = locationPart.Room;
            Address1 = locationPart.Address1;
            Address2 = locationPart.Address2;
            City = locationPart.City;
            State = locationPart.State;
            Zip = locationPart.Zip;
            MapUrl = locationPart.MapUrl;
            Latitude = locationPart.Latitude;
            Longitude = locationPart.Longitude;
        }

        public LocationDTO(LocationPartRecord locationPartRecord)
            : this()
        {
            if (locationPartRecord == null) return;

            Name = locationPartRecord.Name;
            Building = locationPartRecord.Building;
            Room = locationPartRecord.Room;
            Address1 = locationPartRecord.Address1;
            Address2 = locationPartRecord.Address2;
            City = locationPartRecord.City;
            State = locationPartRecord.State;
            Zip = locationPartRecord.Zip;
            MapUrl = locationPartRecord.MapUrl;
            Latitude = locationPartRecord.Latitude;
            Longitude = locationPartRecord.Longitude;
        }

        public string Name { get; set; }
        public string Building { get; set; }
        public string Room { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
        public string MapUrl { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }
}