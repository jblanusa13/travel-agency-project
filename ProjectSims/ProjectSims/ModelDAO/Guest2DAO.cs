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
    class Guest2DAO : ISubject
    {
        private Guest2FileHandler guest2FileHandler;
        private List<Guest2> guests;

        private List<IObserver> observers;

        public Guest2DAO()
        {
            guest2FileHandler = new Guest2FileHandler();
            guests = guest2FileHandler.Load();
            observers = new List<IObserver>();
        }

        public int NextId()
        {
            return guests.Max(t => t.Id) + 1;
        }

        public void Add(Guest2 guest)
        {
            guest.Id = NextId();
            guests.Add(guest);
            guest2FileHandler.Save(guests);
            NotifyObservers();
        }

        public void Remove(Guest2 guest)
        {
            guests.Remove(guest);
            guest2FileHandler.Save(guests);
            NotifyObservers();
        }

        public void Update(Guest2 guest)
        {
            int index = guests.FindIndex(g => guest.Id == g.Id);
            if (index != -1)
            {
                guests[index] = guest;
            }
            guest2FileHandler.Save(guests);
            NotifyObservers();
        }


        public List<Guest2> GetAll()
        {
            return guest2FileHandler.Load();
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
        public Guest2 FindById(int id)
        {
            return guests.Find(guest => guest.Id == id);
        }
    }
}