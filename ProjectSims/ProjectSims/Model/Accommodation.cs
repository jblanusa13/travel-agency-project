﻿using ProjectSims.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace ProjectSims.Model
{
    public enum AccommodationType { Kuca, Apartman, Koliba };
    public class Accommodation : ISerializable
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Location { get; set; }
        public AccommodationType Type { get; set; }
        public int GuestsMaximum { get; set; }
        public int MinimumReservationDays { get; set; }
        public int DismissalDays { get; set; }
        public List<string> Images { get; set; }
        public Owner Owner { get; set; }
        public int IdOwner { get; set; }
        public Accommodation() {
            DismissalDays = 1;
            Images = new List<string>();
        }

        public Accommodation(int id, string name, string location, AccommodationType type, int guestsMaximum, 
            int minimumReservationDays, int dismissalDays, List<string> images, Owner owner, int idOwner) {
            //Id = id;
            Name = name;
            Location = location;
            Type = type;
            GuestsMaximum = guestsMaximum;
            MinimumReservationDays = minimumReservationDays;
            DismissalDays = dismissalDays;
            Images = images;
            Owner = owner;
            //IdOwner = idOwner;
        }

        public void FromCSV(string[] values)
        {
            Id = Convert.ToInt32(values[0]);
            Name = values[1];
            Location = values[2];
            Type = Enum.Parse<AccommodationType>(values[3]);
            GuestsMaximum = Convert.ToInt32(values[4]);
            MinimumReservationDays = Convert.ToInt32(values[5]);
            DismissalDays = Convert.ToInt32(values[6]);
            foreach (string image in values[7].Split(","))
            {
                Images.Add(image);
            }
            IdOwner = Convert.ToInt32(values[8]);
        }
        public string[] ToCSV()
        {
            string ImageString = "";
            foreach (string image in Images)
            {
                if (image != Images.Last())
                {
                    ImageString += image + ",";
                }
            }
            ImageString += Images.Last();

            string[] csvValues =
            { 
                Id.ToString(), 
                Name, 
                Location,
                Type.ToString(),
                GuestsMaximum.ToString(),
                MinimumReservationDays.ToString(),
                DismissalDays.ToString(),
                ImageString,
                IdOwner.ToString() 
            };
            return csvValues;
        }
    }
}
