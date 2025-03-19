using BusinessLayer.Interface;
using BusinessLayer.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ModelLayer.Model;
using RepositoryLayer.Entity;

namespace HelloGreetingApplication.Controllers;

[ApiController]
[Route("api/[controller]")]
public class HelloGreetingController : ControllerBase
{
    private readonly IGreetingBL _greetingBl;
    private readonly ILogger<HelloGreetingController> _logger;
    private readonly RabbitMQService _rabbitMQService;

    public HelloGreetingController(IGreetingBL greetingBl, ILogger<HelloGreetingController> logger, RabbitMQService rabbitMQService)
    {
        _greetingBl = greetingBl;
        _logger = logger;
        _rabbitMQService = rabbitMQService;
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        _logger.LogInformation("Get Greeting method called");

        var greeting = await _greetingBl.GetGreetingBL();
        _rabbitMQService.PublishMessage("Fetched a greeting");

        return Ok(new ResponseModel<string>
        {
            Message = "Get method successfully applied",
            Success = true,
            Data = greeting
        });
    }

    [HttpPost]
    public IActionResult Post(UserRegistrationModel userRegistration)
    {
        _logger.LogInformation("Post method called");
        _rabbitMQService.PublishMessage("User registered: " + userRegistration.Email);

        return Ok(new ResponseModel<string>
        {
            Message = "Post method successfully applied",
            Success = true,
            Data = $"User First Name: {userRegistration.FirstName}, Last Name: {userRegistration.LastName}, Email: {userRegistration.Email}"
        });
    }

    [HttpPost("GreetUser")]
    public async Task<IActionResult> GreetUser(GreetUserModel greetUserModel)
    {
        _logger.LogInformation("Trying to greet a user");
        var greeting = await _greetingBl.DisplayGreetingBL(greetUserModel);
        _rabbitMQService.PublishMessage("User greeted: " + greetUserModel.FirstName);

        return Ok(new ResponseModel<string>
        {
            Message = "User greeted successfully",
            Success = true,
            Data = greeting
        });
    }

    [Authorize]
    [HttpPost("SaveGreetings")]
    public async Task<IActionResult> SaveGreetings(SaveGreetingModel greeting)
    {
        _logger.LogInformation("Saving the greeting message");
        var userId = int.Parse(User.FindFirst("UserId")?.Value ?? "0");
        if (userId == 0)
        {
            return Unauthorized("Invalid User");
        }

        var savedGreeting = await _greetingBl.SaveGreetingBL(greeting, userId);
        _rabbitMQService.PublishMessage("Greeting saved for user: " + userId);

        return Ok(new ResponseModel<string>
        {
            Message = "Greeting message saved successfully",
            Success = true,
            Data = savedGreeting
        });
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteGreeting(int id)
    {
        _logger.LogInformation($"Deleting greeting with ID: {id}");
        bool result = await _greetingBl.DeleteGreetingMessageBL(id);

        if (!result)
        {
            return NotFound(new ResponseModel<string>
            {
                Success = false,
                Message = "No greeting message found with that ID",
                Data = "Create a new greeting message before deleting"
            });
        }

        _rabbitMQService.PublishMessage("Greeting deleted with ID: " + id);
        return Ok(new ResponseModel<string>
        {
            Success = true,
            Message = "Greeting deleted successfully",
            Data = $"Deleted greeting with ID {id}"
        });
    }
}
