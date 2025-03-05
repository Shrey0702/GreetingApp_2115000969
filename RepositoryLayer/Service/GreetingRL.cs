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

        public string SaveGreetingRL(SaveGreetingModel greeting)
        {
            _logger.LogInformation("Greetings from repository layer using SaveGreeting to save greetings in our database");
            try
            {
                _logger.LogInformation("Trying to save greeting message to the Database");
                GreetingEntity newGreeting = new GreetingEntity()
                {
                    GreetingMessage = greeting.GreetingMessage
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
    }
}
