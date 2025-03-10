﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ModelLayer.Model;
using RepositoryLayer.Entity;

namespace RepositoryLayer.Interface
{
    public interface IGreetingRL
    {
        string GetGreetingByIdRL(GreetByIdModel greetingID);
        public string GetGreetingRL();
        public string SaveGreetingRL(SaveGreetingModel greeting);
        public List<GreetingEntity> RetrieveAllGreetingsRL();
        public bool UpdateGreetingMessageRL(int id, SaveGreetingModel modifiedGreeting);

        public bool DeleteGreetingMessageRL(int id);

    }
}
