using BusinessLayer.Interface;
using ModelLayer.Model;
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

        /// <summary>
        /// Method to display the greeting message with user first name and last name
        /// </summary>
        /// <param name="greetUserModel"></param>
        /// <returns></returns>

        public string DisplayGreetingBL(GreetUserModel greetUserModel)
        {
            if(greetUserModel.FirstName == string.Empty && greetUserModel.LastName == string.Empty)
            {
                return "Hello World!!";
            }
            else
            {
                return $"Hello {greetUserModel.FirstName} {greetUserModel.LastName}";
            }
        }
    }
}
