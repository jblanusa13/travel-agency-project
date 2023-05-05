﻿using ProjectSims.Domain.Model;
using ProjectSims.Domain.RepositoryInterface;
using ProjectSims.Observer;
using ProjectSims.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectSims.Service
{
    public class TourRequestService
    {
        private ITourRequestRepository tourRequestRepository;
        private TourService tourService;
        public TourRequestService()
        {
            tourRequestRepository = Injector.CreateInstance<ITourRequestRepository>();
            tourService = new TourService();
        }
        public int GetNextId()
        {
            return tourRequestRepository.GetNextId();
        }
        public List<TourRequest> GetAllRequests()
        {
            return tourRequestRepository.GetAll();
        }
        public List<TourRequest> GetWaitingRequests()
        {
            return tourRequestRepository.GetWaitingRequests();
        }
        public TourRequest GetById(int id)
        {
            return tourRequestRepository.GetById(id);
        }
        public List<TourRequest> GetByGuest2Id(int guest2Id)
        {
            return tourRequestRepository.GetByGuest2Id(guest2Id);
        }
        public List<TourRequest> GetWantedRequests(string location,string language,string maxNumberGuests,DateTime dateRangeStart,DateTime dateRangeEnd)
        {
            List<TourRequest> wantedRequests = new List<TourRequest>();
            List<TourRequest> tourRequestsOnLocation = tourRequestRepository.GetAll();
            List<TourRequest> tourRequestsOnLanguage = tourRequestRepository.GetAll();
            List<TourRequest> tourRequestsWithMaxNumberGuests = tourRequestRepository.GetAll();
            List<TourRequest> tourRequestsInDateRange = tourRequestRepository.GetAll();
            if (location != "")
                tourRequestsOnLocation = tourRequestRepository.GetByLocation(location);
            if (language != "")
                tourRequestsOnLanguage = tourRequestRepository.GetByLanguage(language);
            if(maxNumberGuests != "")
                tourRequestsWithMaxNumberGuests = tourRequestRepository.GetByMaxNumberGuests(int.Parse(maxNumberGuests));
            tourRequestsInDateRange = tourRequestRepository.GetRequestsInDateRange(DateOnly.FromDateTime(dateRangeStart),DateOnly.FromDateTime(dateRangeEnd.Date));
            foreach(TourRequest request in tourRequestRepository.GetAll())
            {
                if(tourRequestsOnLocation.Contains(request) && tourRequestsOnLanguage.Contains(request) && tourRequestsWithMaxNumberGuests.Contains(request) && tourRequestsInDateRange.Contains(request))
                    wantedRequests.Add(request);
            }
            return wantedRequests;
        }
        public void Create(TourRequest tourRequest)
        {
            tourRequestRepository.Create(tourRequest);
        }
        public void Delete(TourRequest tourRequest)
        {
            tourRequestRepository.Remove(tourRequest);
        }
        public void Update(TourRequest tourRequest)
        {
            tourRequestRepository.Update(tourRequest);
        }
        public void Subscribe(IObserver observer)
        {
            tourRequestRepository.Subscribe(observer);
        }
    }
}
