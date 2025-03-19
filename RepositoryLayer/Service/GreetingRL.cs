using RepositoryLayer.Entity;
using RepositoryLayer.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ModelLayer.Model;
using RepositoryLayer.Context;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;

namespace RepositoryLayer.Service
{
    public class GreetingRL : IGreetingRL
    {

        private readonly GreetingAppContext _context;
        private readonly ILogger<GreetingRL> _logger;

        public GreetingRL(GreetingAppContext context, ILogger<GreetingRL> logger)
        {
            _context = context;
            _logger = logger;
        }

        public string GetGreetingByIdRL(GreetByIdModel greetingID)
        {
            try
            {
                var greeting = _context.GreetingEntities.FirstOrDefault(g => g.GreetingID == greetingID.Id);
                if (greeting != null)
                {
                    _logger.LogInformation("Greeting message fetched successfully");
                    return greeting.GreetingMessage;
                }
                else
                {
                    _logger.LogError("There's no greeting message corresponding to that ID");
                    return "Greeting message not found";
                }

            }
            catch(Exception e)
            {
                _logger.LogError($"Exception Occured while fetching greeting message by ID {e.Message}");
                return "There's some error occured while trying to fetch greeting message by ID";
            } 
        }

        public string GetGreetingRL()
        {
            _logger.LogInformation("Greetings from repository layer using GetGreeting get request from controller");
            return "Greetings from repository layer using GetGreeting get request from controller";
        }

        public string SaveGreetingRL(SaveGreetingModel greeting, int userId)
        {
            _logger.LogInformation("Greetings from repository layer using SaveGreeting to save greetings in our database");
            try
            {
                _logger.LogInformation("Trying to save greeting message to the Database");
                GreetingEntity newGreeting = new GreetingEntity()
                {
                    GreetingMessage = greeting.GreetingMessage,
                    UserId = userId
                };
                _context.GreetingEntities.Add(newGreeting);
                _context.SaveChanges();
                return greeting.GreetingMessage;

            }
            catch (Exception e)
            {
                _logger.LogError($"Exception Occured while saving greeting message to the Database {e.Message}");
                return "There's some error occured while trying to save greeting message to the Database";
            }
        }
        /// <summary>
        /// Method to retrieve all greeting messages from the Database
        /// </summary>
        /// <returns>returns all the greetings in the database</returns>
        public List<GreetingEntity> RetrieveAllGreetingsRL()
        {
            try
            {
                _logger.LogInformation("Trying to retrieve all greeting messages from the Database");
                var greetings = _context.GreetingEntities.ToList();
                if (greetings.Count > 0)
                {
                    _logger.LogInformation("Greeting messages fetched successfully");
                    return greetings;
                }
                else
                {
                    _logger.LogError("There's no greeting messages in the Database");
                    return null;
                }
            }
            catch (Exception e)
            {
                _logger.LogError($"Exception Occured while fetching all greeting messages from the Database {e.Message}");
                throw new Exception(e.Message);
            }
        }
        /// <summary>
        /// Method to update the greeting message in the Database
        /// </summary>
        /// <param name="id"></param>
        /// <param name="modifiedGreeting"></param>
        /// <returns>returns true after successfull updation else false</returns>
        public bool UpdateGreetingMessageRL(int id, SaveGreetingModel modifiedGreeting)
        {
            try
            {
                _logger.LogInformation($"Trying to update greeting message in the Database for ID: {id}");
                var greeting = _context.GreetingEntities.FirstOrDefault(g => g.GreetingID == id);
                if (greeting == null)
                {
                    _logger.LogWarning($"No greeting found with ID: {id}");
                    return false;
                }
                greeting.GreetingMessage = modifiedGreeting.GreetingMessage;
                _context.SaveChanges();
              
                return true;
            }
            catch (Exception e)
            {
                _logger.LogError($"Exception occurred while updating greeting message in the Database: {e.Message}");
                return false;
            }
        }
        /// <summary>
        /// Method to delete the greeting message in the Database
        /// </summary>
        /// <param name="id"></param>
        /// <returns>returns true if greeting is successfully deleted</returns>
        public bool DeleteGreetingMessageRL(int id)
        {
            try
            {
                _logger.LogInformation($"Trying to delete greeting message in the Database for ID: {id}");
                var greeting = _context.GreetingEntities.FirstOrDefault(g => g.GreetingID == id);
                if (greeting == null)
                {
                    _logger.LogWarning($"No greeting found with ID: {id}");
                    return false;
                }
                _context.GreetingEntities.Remove(greeting);
                _context.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                _logger.LogError($"Exception occurred while deleting greeting message in the Database: {e.Message}");
                return false;
            }
        }

    }
}
