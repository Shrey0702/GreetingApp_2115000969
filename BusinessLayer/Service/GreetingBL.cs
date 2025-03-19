using BusinessLayer.Interface;
using Microsoft.Extensions.Logging;
using ModelLayer.Model;
using RepositoryLayer.Entity;
using RepositoryLayer.Interface;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BusinessLayer.Service
{
    public class GreetingBL : IGreetingBL
    {
        private readonly ILogger<GreetingBL> _logger;
        private readonly IGreetingRL greetingRL;
        private readonly IRedisCacheService _redisCacheService;

        public GreetingBL(IGreetingRL greetingRL, ILogger<GreetingBL> logger, IRedisCacheService redisCacheService)
        {
            this.greetingRL = greetingRL;
            this._logger = logger;
            this._redisCacheService = redisCacheService;
        }

        public async Task<string> GetGreetingBL()
        {
            _logger.LogInformation("Trying to get the greeting message");

            string cacheKey = "Greeting_Message";
            var cachedGreeting = await _redisCacheService.GetCacheAsync<string>(cacheKey);

            if (cachedGreeting != null)
            {
                _logger.LogInformation("Returning greeting from Redis Cache");
                return cachedGreeting;
            }

            var greeting = await Task.Run(() => greetingRL.GetGreetingRL()); // Ensure async execution
            await _redisCacheService.SetCacheAsync(cacheKey, greeting, TimeSpan.FromMinutes(10));

            return greeting;
        }

        public async Task<string> DisplayGreetingBL(GreetUserModel greetUserModel)
        {
            _logger.LogInformation("Trying to display the greeting message");

            if (string.IsNullOrWhiteSpace(greetUserModel.FirstName) && string.IsNullOrWhiteSpace(greetUserModel.LastName))
            {
                return "Hello World!!";
            }

            return await Task.FromResult($"Hello {greetUserModel.FirstName} {greetUserModel.LastName}");
        }

        public async Task<string> SaveGreetingBL(SaveGreetingModel greeting, int userId)
        {
            try
            {
                _logger.LogInformation("Trying to save the greeting message");

                string savedGreeting = await Task.Run(() => greetingRL.SaveGreetingRL(greeting, userId));

                await _redisCacheService.RemoveCacheAsync("All_Greetings"); // Invalidate cache
                return savedGreeting;
            }
            catch (Exception e)
            {
                _logger.LogError($"Exception occurred while saving greeting: {e.Message}");
                return e.ToString();
            }
        }

        public async Task<string> GetGreetingByIdBL(GreetByIdModel iD)
        {
            try
            {
                _logger.LogInformation("Trying to get greeting message by Id");

                string cacheKey = $"Greeting_{iD.Id}";
                var cachedGreeting = await _redisCacheService.GetCacheAsync<string>(cacheKey);

                if (cachedGreeting != null)
                {
                    _logger.LogInformation("Returning greeting from Redis Cache");
                    return cachedGreeting;
                }

                var greeting = await Task.Run(() => greetingRL.GetGreetingByIdRL(iD));
                await _redisCacheService.SetCacheAsync(cacheKey, greeting, TimeSpan.FromMinutes(10));

                return greeting;
            }
            catch (Exception e)
            {
                _logger.LogError($"Exception occurred while getting greeting by Id: {e.Message}");
                return e.ToString();
            }
        }

        public async Task<List<GreetingEntity>> GetAllGreetingsBL()
        {
            try
            {
                _logger.LogInformation("Trying to get all greeting messages");

                string cacheKey = "All_Greetings";
                var cachedGreetings = await _redisCacheService.GetCacheAsync<List<GreetingEntity>>(cacheKey);

                if (cachedGreetings != null)
                {
                    _logger.LogInformation("Returning greetings from Redis Cache");
                    return cachedGreetings;
                }

                var greetings = await Task.Run(() => greetingRL.RetrieveAllGreetingsRL());
                await _redisCacheService.SetCacheAsync(cacheKey, greetings, TimeSpan.FromMinutes(10));

                return greetings;
            }
            catch (Exception e)
            {
                _logger.LogError($"Exception occurred while getting all greetings: {e.Message}");
                throw new Exception(e.Message);
            }
        }

        public async Task<bool> UpdateGreetingMessageBL(int id, SaveGreetingModel modifiedGreeting)
        {
            try
            {
                _logger.LogInformation("Trying to update greeting message by id");

                bool result = await Task.Run(() => greetingRL.UpdateGreetingMessageRL(id, modifiedGreeting));

                if (result)
                {
                    await _redisCacheService.RemoveCacheAsync($"Greeting_{id}");
                    await _redisCacheService.RemoveCacheAsync("All_Greetings"); // Invalidate cache
                }

                return result;
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return false;
            }
        }

        public async Task<bool> DeleteGreetingMessageBL(int id)
        {
            try
            {
                _logger.LogInformation("Trying to delete greeting message by id");

                bool result = await Task.Run(() => greetingRL.DeleteGreetingMessageRL(id));

                if (result)
                {
                    await _redisCacheService.RemoveCacheAsync($"Greeting_{id}");
                    await _redisCacheService.RemoveCacheAsync("All_Greetings"); // Invalidate cache
                }

                return result;
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return false;
            }
        }
    }
}
