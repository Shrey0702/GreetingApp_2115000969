using BusinessLayer.Interface;
using BusinessLayer.Service;
using Microsoft.AspNetCore.Mvc;
using ModelLayer.Model;

namespace HelloGreetingApplication.Controllers;

[ApiController]
[Route("[controller]")]
public class HelloGreetingController : ControllerBase
{

    /// <summary>
    /// Creating referance of IGreetingBL
    /// </summary>
    IGreetingBL _greetingBl;
    ILogger<HelloGreetingController> _logger;

    public HelloGreetingController(IGreetingBL greetingBl, ILogger<HelloGreetingController> logger)
    {
        _greetingBl = greetingBl;
        _logger = logger;
    }


    /// <summary>
    /// Get method to get the greeting message
    /// </summary>
    /// <returns>Hello Greeting</returns>
    [HttpGet]
    public IActionResult Get()
    {
        try
        {
            _logger.LogInformation("Get Greeting method called");
            ResponseModel<string> response = new ResponseModel<string>();
            string greeting = _greetingBl.GetGreetingBL();
            response.Message = "Get method successfully applied";
            response.Success = true;
            response.Data = greeting;
            return Ok(response);
        }
        catch (Exception e)
        {
            _logger.LogError($"Exception Occured in GetGreeting {e.Message}");
            ResponseModel<string> response = new ResponseModel<string>();
            response.Message = "Get method failed";
            response.Success = false;
            response.Data = e.Message;
            return BadRequest(response);
        }
    }
    /// <summary>
    /// Post method to register the user
    /// </summary>
    /// <param name="userRegistration"></param>
    [HttpPost]
    public IActionResult Post(UserRegistrationModel userRegistration)
    {
        try
        {
            _logger.LogInformation("Post method called");
            ResponseModel<string> response = new ResponseModel<string>();
            response.Message = "Post method successfully applied";
            response.Success = true;
            response.Data = $"user first name: {userRegistration.FirstName}, last name:{userRegistration.LastName}, email: {userRegistration.Email}";
            return Ok(response);
        }
        catch (Exception e)
        {
            _logger.LogError($"Exception Occured in Post method {e.Message}");
            ResponseModel<string> response = new ResponseModel<string>();
            response.Message = "Post method failed";
            response.Success = false;
            response.Data = e.Message;
            return BadRequest(response);
        }
    }
    /// <summary>
    /// Put method to update the user data
    /// </summary>
    /// <param name="userUpdate"></param>
    [HttpPut]
    public IActionResult Put(UserRegistrationModel userUpdate)
    {
        try
        {
            _logger.LogInformation("Put method called");
            ResponseModel<string> response = new ResponseModel<string>();
            response.Message = "Put method successfully applied";
            response.Success = true;
            response.Data = $"The data after updating user at the database-- user first name: {userUpdate.FirstName}, last name:{userUpdate.LastName}, email: {userUpdate.Email}";
            return Ok(response);
        }
        catch (Exception e)
        {
            _logger.LogError($"Exception Occured in Put method {e.Message}");
            ResponseModel<string> response = new ResponseModel<string>();
            response.Message = "Put method failed";
            response.Success = false;
            response.Data = e.Message;
            return BadRequest(response);
        }
    }
    /// <summary>
    /// Patch method to update the user data
    /// </summary>
    /// <param name="userPatch"></param>
    /// <returns></returns>
    [HttpPatch]
    public IActionResult Patch(UserUpdationModel userPatch)
    {
        try
        {
            _logger.LogInformation("Patch method called");
            ResponseModel<string> response = new ResponseModel<string>();
            response.Message = "Patch method successfully applied";
            response.Success = true;
            response.Data = $"The data after patching user data at the database-- user first name: {userPatch.FirstName}, last name:{userPatch.LastName}, email: {userPatch.Email}";
            return Ok(response);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Exception Occured in Patch method {ex.Message}");
            ResponseModel<string> response = new ResponseModel<string>();
            response.Message = "Patch method failed";
            response.Success = false;
            response.Data = ex.Message;
            return BadRequest(response);
        }
    }
    /// <summary>
    /// Delete method to delete the user data
    /// </summary>
    /// <param name="userDeletion"></param>
    /// <returns></returns>

    [HttpDelete]
    public IActionResult Delete(UserRegistrationModel userDeletion)
    {
        try
        {
            _logger.LogInformation("Delete method called");
            ResponseModel<string> response = new ResponseModel<string>();
            response.Message = "Delete method successfully applied";
            response.Success = true;
            response.Data = $"The user with email: {userDeletion.Email} is deleted";
            return Ok(response);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Exception Occured in Delete method {ex.Message}");
            ResponseModel<string> response = new ResponseModel<string>();
            response.Message = "Delete method failed";
            response.Success = false;
            response.Data = ex.Message;
            return BadRequest(response);
        }
    }

    /// <summary>
    /// Post method to greet the user
    /// </summary>
    /// <param name="greetUserModel"></param>
    /// <returns></returns>

    [HttpPost]
    [Route("GreetUser")]
    public IActionResult Post(GreetUserModel greetUserModel)
    {
        try
        {
            _logger.LogInformation("Trying to run greet method to greet the particualr user");
            ResponseModel<string> response = new ResponseModel<string>();
            string greetMessage = _greetingBl.DisplayGreetingBL(greetUserModel);
            response.Message = "User Greeted successfully";
            response.Success = true;
            response.Data = greetMessage;
            return Ok(response);
        }
        catch (Exception e)
        {
            _logger.LogError($"Exception Occured Greet User Method {e.Message}");
            ResponseModel<string> response = new ResponseModel<string>();
            response.Message = "There is some error while trying to fetch greet message";
            response.Success = false;
            response.Data = e.Message;
            return BadRequest(response);
        }
    }
    /// <summary>
    /// Post method to save the greeting message to the Database
    /// </summary>
    /// <param name="greeting"></param>
    /// <returns>returns a Greeting message which is savend in the database</returns>
    [HttpPost]
    [Route("SaveGreetings")]
    public IActionResult Post(SaveGreetingModel greeting)
    {
        _logger.LogInformation("Post method called to save the greeting message");
        try
        {
            _logger.LogInformation("Trying to save the greeting message");
            ResponseModel<string> response = new ResponseModel<string>();
            string result = _greetingBl.SaveGreetingBL(greeting);
            response.Message = "Greeting message saved successfully";
            response.Success = true;
            response.Data = $"The greeting message saved in the Database is: {result}";
            return Ok(response);
        }
        catch (Exception e)
        {
            _logger.LogError($"Exception Occured in Save Greeting Method {e.Message}");
            ResponseModel<string> response = new ResponseModel<string>();
            response.Message = "There is some error while trying to save the greeting message";
            response.Success = false;
            response.Data = e.Message;
            return BadRequest(response);
        }
    }

    [HttpPost]
    [Route("GetGreetingById")]
    public IActionResult Post(GreetByIdModel iD)
    {
        try
        {
            _logger.LogInformation("Get Greeting By Id method called");
            ResponseModel<string> response = new ResponseModel<string>();
            string greeting = _greetingBl.GetGreetingByIdBL(iD);
            response.Message = "Get method successfully applied";
            response.Success = true;
            response.Data = greeting;
            return Ok(response);
        }
        catch (Exception e)
        {
            _logger.LogError($"Exception Occured in GetGreeting {e.Message}");
            ResponseModel<string> response = new ResponseModel<string>();
            response.Message = "Get method failed";
            response.Success = false;
            response.Data = e.Message;
            return BadRequest(response);
        }
    }
}
