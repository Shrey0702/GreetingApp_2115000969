using BusinessLayer.Interface;
using BusinessLayer.Service;
using Microsoft.AspNetCore.Mvc;
using ModelLayer.Model;
using RepositoryLayer.Entity;

namespace HelloGreetingApplication.Controllers;

[ApiController]
[Route("[controller]")]
public class HelloGreetingController : ControllerBase
{
    private readonly IGreetingBL _greetingBl;
    private readonly ILogger<HelloGreetingController> _logger;

    public HelloGreetingController(IGreetingBL greetingBl, ILogger<HelloGreetingController> logger)
    {
        _greetingBl = greetingBl;
        _logger = logger;
    }

    [HttpGet]
    public IActionResult Get()
    {
        _logger.LogInformation("Get Greeting method called");
        ResponseModel<string> response = new ResponseModel<string>
        {
            Message = "Get method successfully applied",
            Success = true,
            Data = _greetingBl.GetGreetingBL()
        };
        return Ok(response);
    }

    [HttpPost]
    public IActionResult Post(UserRegistrationModel userRegistration)
    {
        _logger.LogInformation("Post method called");
        ResponseModel<string> response = new ResponseModel<string>
        {
            Message = "Post method successfully applied",
            Success = true,
            Data = $"user first name: {userRegistration.FirstName}, last name:{userRegistration.LastName}, email: {userRegistration.Email}"
        };
        return Ok(response);
    }

    [HttpPut]
    public IActionResult Put(UserRegistrationModel userUpdate)
    {
        _logger.LogInformation("Put method called");
        ResponseModel<string> response = new ResponseModel<string>
        {
            Message = "Put method successfully applied",
            Success = true,
            Data = $"Updated user: {userUpdate.FirstName}, {userUpdate.LastName}, {userUpdate.Email}"
        };
        return Ok(response);
    }

    [HttpPatch]
    public IActionResult Patch(UserUpdationModel userPatch)
    {
        _logger.LogInformation("Patch method called");
        ResponseModel<string> response = new ResponseModel<string>
        {
            Message = "Patch method successfully applied",
            Success = true,
            Data = $"Patched user data: {userPatch.FirstName}, {userPatch.LastName}, {userPatch.Email}"
        };
        return Ok(response);
    }

    [HttpDelete]
    public IActionResult Delete(UserRegistrationModel userDeletion)
    {
        _logger.LogInformation("Delete method called");
        ResponseModel<string> response = new ResponseModel<string>
        {
            Message = "Delete method successfully applied",
            Success = true,
            Data = $"User with email {userDeletion.Email} is deleted"
        };
        return Ok(response);
    }

    [HttpPost("GreetUser")]
    public IActionResult Post(GreetUserModel greetUserModel)
    {
        _logger.LogInformation("Trying to greet a user");
        ResponseModel<string> response = new ResponseModel<string>
        {
            Message = "User greeted successfully",
            Success = true,
            Data = _greetingBl.DisplayGreetingBL(greetUserModel)
        };
        return Ok(response);
    }

    [HttpPost("SaveGreetings")]
    public IActionResult Post(SaveGreetingModel greeting)
    {
        _logger.LogInformation("Saving the greeting message");
        ResponseModel<string> response = new ResponseModel<string>
        {
            Message = "Greeting message saved successfully",
            Success = true,
            Data = $"Saved greeting: {_greetingBl.SaveGreetingBL(greeting)}"
        };
        return Ok(response);
    }

    [HttpPost("GetGreetingById")]
    public IActionResult Post(GreetByIdModel iD)
    {
        _logger.LogInformation("Getting greeting by ID");
        ResponseModel<string> response = new ResponseModel<string>
        {
            Message = "Get method successfully applied",
            Success = true,
            Data = _greetingBl.GetGreetingByIdBL(iD)
        };
        return Ok(response);
    }

    [HttpGet("RetrieveAllGreetings")]
    public IActionResult GetGreetings()
    {
        _logger.LogInformation("Getting all greetings");
        ResponseModel<List<GreetingEntity>> response = new ResponseModel<List<GreetingEntity>>
        {
            Message = "Get method successfully applied",
            Success = true,
            Data = _greetingBl.GetAllGreetingsBL()
        };
        return Ok(response);
    }

    [HttpPut("{id}")]
    public IActionResult PutGreetings(int id, [FromBody] SaveGreetingModel modifiedGreeting)
    {
        _logger.LogInformation($"Updating greeting with ID: {id}");
        bool result = _greetingBl.UpdateGreetingMessageBL(id, modifiedGreeting);

        if (!result)
        {
            return NotFound(new ResponseModel<string>
            {
                Success = false,
                Message = "No greeting message found with that ID",
                Data = "Create a new greeting message before modifying"
            });
        }

        return Ok(new ResponseModel<string>
        {
            Success = true,
            Message = "Greeting updated successfully",
            Data = modifiedGreeting.GreetingMessage
        });
    }

    [HttpDelete("{id}")]
    public IActionResult DeleteGreeting(int id)
    {
        _logger.LogInformation($"Deleting greeting with ID: {id}");
        bool result = _greetingBl.DeleteGreetingMessageBL(id);

        if (!result)
        {
            return NotFound(new ResponseModel<string>
            {
                Success = false,
                Message = "No greeting message found with that ID",
                Data = "Create a new greeting message before deleting"
            });
        }

        return Ok(new ResponseModel<string>
        {
            Success = true,
            Message = "Greeting deleted successfully",
            Data = $"Deleted greeting with ID {id}"
        });
    }
}
