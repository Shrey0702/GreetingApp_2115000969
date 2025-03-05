﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ModelLayer.Model;

namespace RepositoryLayer.Interface
{
    public interface IGreetingRL
    {
        string GetGreetingByIdRL(GreetByIdModel greetingID);
        public string GetGreetingRL();
        public string SaveGreetingRL(SaveGreetingModel greeting);
    }
}
