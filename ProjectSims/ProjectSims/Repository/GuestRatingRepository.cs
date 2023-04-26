﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProjectSims.Domain.Model;
using ProjectSims.FileHandler;
using ProjectSims.Observer;

namespace ProjectSims.Repository
{
    public class GuestRatingRepository : ISubject
    {
        private GuestRatingFileHandler guestRatingFileHandler;
        private List<GuestRating> guestRatings;
        private readonly List<IObserver> observers;

        public GuestRatingRepository()
        {
            guestRatingFileHandler = new GuestRatingFileHandler();
            guestRatings = guestRatingFileHandler.Load();
            observers = new List<IObserver>();
        }
        
        public int NextId()
        {
            return guestRatings.Max(t => t.Id) + 1;
        }

        public List<GuestRating> GetAll()
        {
            return guestRatings;
        }

        public GuestRating Get(int id)
        {
            return guestRatings.Find(r => r.Id == id);
        }

        public void Add(GuestRating guestRating)
        {
            guestRating.Id = NextId();
            guestRatings.Add(guestRating);
            guestRatingFileHandler.Save(guestRatings);
            NotifyObservers();
        }
        public void Remove(GuestRating guestRating)
        {
            guestRatings.Remove(guestRating);
            guestRatingFileHandler.Save(guestRatings);
            NotifyObservers();
        }
        public void Update(GuestRating guestRating)
        {
            int index = guestRatings.FindIndex(k => guestRating.Id == k.Id);
            if (index != -1)
            {
                guestRatings[index] = guestRating;
            }
            guestRatingFileHandler.Save(guestRatings);
            NotifyObservers();
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
    }
}
