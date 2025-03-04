using RepositoryLayer.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryLayer.Service
{
    public class GreetingRL : IGreetingRL
    {
        public string GetGreetingRL()
        {
            return "Greetings from repository layer using GetGreeting get request from controller";
        }
    }
}
