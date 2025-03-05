using ModelLayer.Model;
using RepositoryLayer.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Interface
{
    public interface IGreetingBL
    {
        public string GetGreetingBL();
        public string DisplayGreetingBL(GreetUserModel greetUserModel);
        public string SaveGreetingBL(SaveGreetingModel greeting);
        public string GetGreetingByIdBL(GreetByIdModel iD);
        public List<GreetingEntity> GetAllGreetingsBL();
        public bool UpdateGreetingMessageBL(int id, SaveGreetingModel modifiedGreeting);
    }
}
