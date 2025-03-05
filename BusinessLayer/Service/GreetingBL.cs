using BusinessLayer.Interface;
using Microsoft.Extensions.Logging;
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
        private readonly ILogger<GreetingBL> _logger;

        /// <summary>
        /// Creating referance of IGreetingRL
        /// </summary>
        private readonly IGreetingRL greetingRL;

        /// <summary>
        /// Constructor for dependency injection
        /// </summary>
        /// <param name="greetingRL"></param>
        public GreetingBL(IGreetingRL greetingRL, ILogger<GreetingBL> logger)
        {
            this.greetingRL = greetingRL;
            this._logger = logger;

        }


        public string GetGreetingBL()
        {
            _logger.LogInformation("Trying to get the greeting message");
            return greetingRL.GetGreetingRL();
        }

        /// <summary>
        /// Method to display the greeting message with user first name and last name
        /// </summary>
        /// <param name="greetUserModel"></param>
        /// <returns></returns>

        public string DisplayGreetingBL(GreetUserModel greetUserModel)
        {
            _logger.LogInformation("Trying to display the greeting message");
            if (greetUserModel.FirstName == string.Empty && greetUserModel.LastName == string.Empty)
            {
                return "Hello World!!";
            }
            else
            {
                return $"Hello {greetUserModel.FirstName} {greetUserModel.LastName}";
            }
        }
        /// <summary>
        /// Method to save the greeting message to the Database
        /// </summary>
        /// <param name="greeting"></param>
        /// <returns>returns the data which is saved in our database</returns>
        public string SaveGreetingBL(SaveGreetingModel greeting)
        {
            try
            {
                _logger.LogInformation("Trying to save the greeting message");
                return greetingRL.SaveGreetingRL(greeting);
            }
            catch (Exception e)
            {
                _logger.LogError($"Exception Occured while saving greeting message to the Database {e.Message}");
                return e.ToString();
            }
        }

        public string GetGreetingByIdBL(GreetByIdModel iD)
        {
            try
            {
                _logger.LogInformation("Trying to get the greeting message by Id");
                return greetingRL.GetGreetingByIdRL(iD);
            }
            catch (Exception e)
            {
                _logger.LogError($"Exception Occured while getting greeting message by Id {e.Message}");
                return e.ToString();
            }
        }
    }
}
