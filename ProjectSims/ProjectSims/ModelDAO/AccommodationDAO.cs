﻿using ProjectSims.FileHandler;
using ProjectSims.Model;
using ProjectSims.Observer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectSims.ModelDAO
{
    class AccommodationDAO : ISubject
    {
        private readonly AccommodationFileHandler accommodationFileHandler;
        private readonly List<Accommodation> accommodations; 
        private readonly OwnerFileHandler ownerFileHandler;
        private readonly List<Owner> owners;

        private readonly List<IObserver> observers;

        public OwnerDAO OwnerDao { get; set; }

        public AccommodationDAO()
        {
            accommodationFileHandler = new AccommodationFileHandler();
            accommodations = accommodationFileHandler.Load();
            observers = new List<IObserver>();
        }

        public int NextId()
        {
            return accommodations.Max(a => a.Id) + 1;
        }

        public void Add(Accommodation accommodation)
        {
            accommodation.Id = NextId();
            accommodations.Add(accommodation);
            accommodationFileHandler.Save(accommodations);
            NotifyObservers();
        }

        public void Remove(Accommodation accommodation)
        {
            accommodations.Remove(accommodation);
            accommodationFileHandler.Save(accommodations);
            NotifyObservers();
        }

        public void Update(Accommodation accommodation)
        {
            int index = accommodations.FindIndex(a => accommodation.Id == a.Id);
            if (index != -1)
            {
                accommodations[index] = accommodation;
            }
            accommodationFileHandler.Save(accommodations);
            NotifyObservers();
        }


        public List<Accommodation> GetAll()
        {
            return accommodations;
        }

        public void Subscribe(IObserver observer)
        {
            observers.Add(observer);
        }

        public void Unsubscribe(IObserver observer)
        {
            observers.Remove(observer);
        }

        public void NotifyObservers()
        {
            foreach (var observer in observers)
            {
                observer.Update();
            }
        }
        public void ConnectAccommodationWithOwner() 
        {
            foreach (Accommodation accommodation in accommodations)
            {
                Owner owner = OwnerDao.FindById(accommodation.IdOwner);
                if (owner != null) 
                {
                    owner.OwnersAccommodations.Add(accommodation);
                    accommodation.Owner = owner;
                }
            }
        }

    }
}
