using ModelLayer.Model;
using RepositoryLayer.Entity;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BusinessLayer.Interface
{
    public interface IGreetingBL
    {
        Task<string> GetGreetingBL();
        Task<string> DisplayGreetingBL(GreetUserModel greetUserModel);
        Task<string> SaveGreetingBL(SaveGreetingModel greeting, int userId);
        Task<string> GetGreetingByIdBL(GreetByIdModel iD);
        Task<List<GreetingEntity>> GetAllGreetingsBL();
        Task<bool> UpdateGreetingMessageBL(int id, SaveGreetingModel modifiedGreeting);
        Task<bool> DeleteGreetingMessageBL(int id);
    }
}
