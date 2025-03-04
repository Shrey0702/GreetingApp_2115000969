using BusinessLayer.Interface;
using RepositoryLayer.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Service
{
    public class GreetingBL : IGreetingBL
    {
        /// <summary>
        /// Creating referance of IGreetingRL
        /// </summary>
        private readonly IGreetingRL greetingRL;

        /// <summary>
        /// Constructor for dependency injection
        /// </summary>
        /// <param name="greetingRL"></param>
        public GreetingBL(IGreetingRL greetingRL)
        {
            this.greetingRL = greetingRL;
        }


        public string GetGreetingBL()
        {
            return greetingRL.GetGreetingRL();
        }
    }
}
