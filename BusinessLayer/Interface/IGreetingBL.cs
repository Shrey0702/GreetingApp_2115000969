using ModelLayer.Model;
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
    }
}
