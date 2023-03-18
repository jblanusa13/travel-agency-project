﻿using ProjectSims.Model;
using ProjectSims.ModelDAO;
using ProjectSims.Observer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectSims.Controller
{
    public class KeyPointController
    {
        private KeyPointDAO keyPoints;

        public KeyPointController()
        {
            keyPoints= new KeyPointDAO();
        }
        public List<KeyPoint> GetAllKeyPoints()
        {
            return keyPoints.GetAll();
        }
        public void Create(KeyPoint keyPoint)
        {
            keyPoints.Add(keyPoint);
        }

        public void Delete(KeyPoint keyPoint)
        {
            keyPoints.Remove(keyPoint);
        }
        public string FindNameById(int id)
        {
            List<KeyPoint> allKeyPoints = keyPoints.GetAll();
            return allKeyPoints.Find(k => k.Id == id).Name;
        }
        public void Update(KeyPoint keyPoint)
        {
            keyPoints.Update(keyPoint);
        }
        public void Subscribe(IObserver observer)
        {
            keyPoints.Subscribe(observer);
        }


    }
}