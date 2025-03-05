using BusinessLayer.Interface;
using Microsoft.Extensions.Logging;
using ModelLayer.Model;
using RepositoryLayer.Entity;
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
        /// <summary>
        /// Method to get the greeting message by Id
        /// </summary>
        /// <param name="iD"></param>
        /// <returns>returns the data which is saved in the database</returns>
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
        /// <summary>
        /// Method to get all the greeting messages
        /// </summary>
        /// <returns>returns a string of all Greeting Messages</returns>
        public List<GreetingEntity> GetAllGreetingsBL()
        {
            try
            {
                _logger.LogInformation("Trying to get all the greeting messages");
                return greetingRL.RetrieveAllGreetingsRL();

            }
            catch (Exception e)
            {
                _logger.LogError($"Exception Occured while getting all greeting messages {e.Message}");
                throw new Exception(e.Message);
            }
        }
        /// <summary>
        /// Method to update the greeting message by Id
        /// </summary>
        /// <param name="modifyById"></param>
        /// <param name="modifiedGreeting"></param>
        /// <returns>returns bool true if data is successfully modified</returns>
        public bool UpdateGreetingMessageBL(int id, SaveGreetingModel modifiedGreeting)
        {
            try
            {
                _logger.LogInformation("Trying to update greeting message by id");
                return greetingRL.UpdateGreetingMessageRL(id, modifiedGreeting);

            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return false;
            }
        }

        public bool DeleteGreetingMessageBL(int id)
        {
            try
            {
                _logger.LogInformation("Trying to delete greeting message by id");
                return greetingRL.DeleteGreetingMessageRL(id);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return false;
            }
        }

    }
}
