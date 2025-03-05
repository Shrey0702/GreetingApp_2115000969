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
    }
}
