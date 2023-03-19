﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using ProjectSims.FileHandler;
using ProjectSims.Model;
using ProjectSims.Observer;

namespace ProjectSims.ModelDAO
{
    public class AccommodationReservationDAO : ISubject
    {
        private AccommodationReservationFileHandler _reservationFileHandler;
        private List<AccommodationReservation> _reservations;

        private Guest1FileHandler _guest1FileHandler;
        private List<Guest1> _guests;

        private List<DateRanges> _availableDates;
        private List<DateRanges> _unavailableDates;
        private List<DateRanges> _candidatesForDeletion;
        private DateOnly _firstDate;
        private DateOnly _lastDate;

        private readonly List<IObserver> _observers;



        public AccommodationReservationDAO()
        {
            _reservationFileHandler = new AccommodationReservationFileHandler();
            _reservations = _reservationFileHandler.Load();

            _guest1FileHandler = new Guest1FileHandler();
            _guests = _guest1FileHandler.Load();

            _availableDates = new List<DateRanges>();
            _unavailableDates = new List<DateRanges>();
            _candidatesForDeletion = new List<DateRanges>();

            _observers = new List<IObserver>();

            _firstDate = new DateOnly();
            _lastDate = new DateOnly();
        }

        public Guest1 GetGuestByUsername(string username)
        {
            return _guests.Find(g => g.Username == username);       
        }

        public List<AccommodationReservation> GetAll()
        {
            return _reservations;
        }

        public List<DateRanges> FindDates(DateOnly firstDate, DateOnly lastDate, int accommodationId)
        {
            _availableDates.Clear();
            _availableDates.Add(new DateRanges(firstDate,lastDate));
            _availableDates.Add(new DateRanges(firstDate.AddDays(accommodationId), lastDate.AddDays(accommodationId)));

            return _availableDates;
        }

        // finds available dates for chosen accommodation in given date range
        public List<DateRanges> FindAvailableDates(DateOnly firstDate, DateOnly lastDate, int daysNumber, int accommodationId)
        {
            _availableDates.Clear();

            _firstDate = firstDate;
            _lastDate = lastDate;

            FindAllDates(daysNumber);
            FindUnavailableDates(accommodationId);

            bool isFirstBoundaryCase, isInRangeCase, isLastBoundaryCase;
            foreach(DateRanges unavailableDate in _unavailableDates)
            {
                // Date range for search: 15.03-22.03, checkIn = 14.03, checkOut = 17.03
                isFirstBoundaryCase = !IsInRange(unavailableDate.CheckIn, _firstDate, _lastDate) && IsInRange(unavailableDate.CheckOut, _firstDate, _lastDate);

                // Date range for search: 15.03-22.03, checkIn = 15.03, checkOut = 17.03
                isInRangeCase = IsInRange(unavailableDate.CheckIn, _firstDate, _lastDate) && IsInRange(unavailableDate.CheckOut, _firstDate, _lastDate);

                // Date range for search: 15.03-22.03, checkIn = 20.03, checkOut = 25.03
                isLastBoundaryCase = IsInRange(unavailableDate.CheckIn, _firstDate, _lastDate) && !IsInRange(unavailableDate.CheckOut, _firstDate, _lastDate);

                if (isFirstBoundaryCase)
                {
                    CheckFirstBoundaryCase(unavailableDate);
                }
                else if (isInRangeCase)
                {
                    CheckIsInRangeCase(unavailableDate);
                }
                else if (isLastBoundaryCase)
                {
                    CheckLastBoundaryCase(unavailableDate);
                }
            }

            foreach (DateRanges dates in _candidatesForDeletion)
            {
                _availableDates.Remove(dates);
            }

            if(_availableDates.Count == 0)
            {
                FindAlternativeDates(daysNumber);
            }
            return _availableDates;
        }

        // checks if date is between firstDate and lastDate
        public bool IsInRange(DateOnly date, DateOnly firstDate, DateOnly lastDate)
        {
            return date >= firstDate && date <= lastDate;
        }

        public void CheckFirstBoundaryCase(DateRanges unavailableDate)
        {
            bool checkOutIsInRange, containsUnavailableDates;

            foreach (DateRanges availableDate in _availableDates)
            {
                checkOutIsInRange = unavailableDate.CheckOut > availableDate.CheckIn && unavailableDate.CheckOut <= availableDate.CheckOut;
                containsUnavailableDates = availableDate.CheckIn < unavailableDate.CheckOut && availableDate.CheckOut < unavailableDate.CheckOut;
                if (checkOutIsInRange || containsUnavailableDates)
                {
                    _candidatesForDeletion.Add(availableDate);
                }
            }

        }

        public void CheckLastBoundaryCase(DateRanges unavailableDate)
        {
            bool checkInIsInRange, containsUnavailableDates;
            foreach (DateRanges availableDate in _availableDates)
            {
                checkInIsInRange = unavailableDate.CheckIn >= availableDate.CheckIn && unavailableDate.CheckIn < availableDate.CheckOut;
                containsUnavailableDates = availableDate.CheckIn > unavailableDate.CheckIn && availableDate.CheckOut > unavailableDate.CheckIn;
                if (checkInIsInRange || containsUnavailableDates)
                {
                    _candidatesForDeletion.Add(availableDate);
                }
            }
        }

        public void CheckIsInRangeCase(DateRanges unavailableDate)
        {
            bool checkInIsInRange, checkOutIsInRange, containsUnavailableDates;
            foreach (DateRanges availableDate in _availableDates)
            {
                checkInIsInRange = unavailableDate.CheckIn >= availableDate.CheckIn && unavailableDate.CheckIn < availableDate.CheckOut;
                checkOutIsInRange = unavailableDate.CheckOut > availableDate.CheckIn && unavailableDate.CheckOut <= availableDate.CheckOut;
                containsUnavailableDates = availableDate.CheckIn > unavailableDate.CheckIn && availableDate.CheckOut < unavailableDate.CheckOut;
                if (checkInIsInRange || checkOutIsInRange || containsUnavailableDates)
                {
                    _candidatesForDeletion.Add(availableDate);
                }
            }
        }

        // calculates all possible dates, for given date range
        public void FindAllDates(int daysNumber)
        {
            DateOnly startDate = _firstDate;
            DateOnly endDate = _firstDate.AddDays(daysNumber);

            while (endDate <= _lastDate) {
            _availableDates.Add(new DateRanges(startDate, endDate));
            startDate = startDate.AddDays(1);
            endDate = endDate.AddDays(1);
            }
        }

        // finds unavailable dates for chosen accommodation in given date range
        public void FindUnavailableDates(int accommodationId)
        {
            foreach(AccommodationReservation reservation in _reservations)
            {
                if(reservation.AccommodationId == accommodationId)
                {
                    if(IsUnavailable(reservation.CheckInDate, reservation.CheckOutDate))
                    {
                        _unavailableDates.Add(new DateRanges(reservation.CheckInDate, reservation.CheckOutDate));
                    }
                }
            }
        }

        // checks if given date ranges are unavailable
        public bool IsUnavailable(DateOnly checkIn, DateOnly checkOut)
        {
            if(IsBefore(checkIn) && IsBefore(checkOut))
            {
                return false;
            }
            else if(IsAfter(checkIn) && IsAfter(checkOut))
            {
                return false;
            }

            return true;

        }
        public bool IsBefore(DateOnly date)
        {
            return date <= _firstDate;
        }

        public bool IsAfter(DateOnly date)
        {
            return date >= _lastDate;
        }

        public void FindAlternativeDates(int daysNumber)
        {
            DateOnly startDateBefore = _firstDate.AddDays(-1);
            DateOnly endDateBefore = startDateBefore.AddDays(daysNumber);

            DateOnly endDateAfter = _lastDate.AddDays(1);
            DateOnly startDateAfter = endDateAfter.AddDays(-daysNumber);

            while (_availableDates.Count != 4)
            {
                if (IsAlternativeDateAvailible(startDateBefore, endDateBefore))
                {
                    _availableDates.Add(new DateRanges(startDateBefore, endDateBefore));
                }
                startDateBefore = startDateBefore.AddDays(-1);
                endDateBefore = endDateBefore.AddDays(-1);

                if (IsAlternativeDateAvailible(startDateAfter, endDateAfter))
                {
                    _availableDates.Add(new DateRanges(startDateAfter, endDateAfter));
                }
                startDateAfter = startDateAfter.AddDays(1);
                endDateAfter= endDateAfter.AddDays(1);
            }
        }

        public bool IsAlternativeDateAvailible(DateOnly checkIn, DateOnly checkOut)
        {
            bool checkInIsInRange, checkOutIsInRange, containsUnavailableDates;
            foreach (DateRanges unavailableDate in _unavailableDates)
            {
                checkInIsInRange = unavailableDate.CheckIn >= checkIn && unavailableDate.CheckIn < checkOut;
                checkOutIsInRange = unavailableDate.CheckOut > checkIn && unavailableDate.CheckOut <= checkOut;
                containsUnavailableDates = checkIn > unavailableDate.CheckIn && checkOut < unavailableDate.CheckOut;
                if (checkInIsInRange || checkOutIsInRange || containsUnavailableDates)
                {
                    return false;
                }
            }

            return true;
        }


        public void Subscribe(IObserver observer)
        {
            _observers.Add(observer);
        }

        public void Unsubscribe(IObserver observer)
        {
            _observers.Remove(observer);
        }

        public void NotifyObservers()
        {
            foreach (var observer in _observers)
            {
                observer.Update();
            }
        }
    }
}
