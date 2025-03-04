using Microsoft.AspNetCore.Mvc;
using ModelLayer.Model;

namespace HelloGreetingApplication.Controllers;

[ApiController]
[Route("[controller]")]
public class HelloGreetingController : ControllerBase
{


    [HttpGet]
    public  IActionResult Get()
    {
        ResponseModel<string> response = new ResponseModel<string>();
        response.Message = "Get method successfully applied";
        response.Success = true;
        response.Data = "Hello Greeting";
        return Ok(response);
    }

    [HttpPost]
    public IActionResult Post(UserRegistrationModel userRegistration)
    {
        ResponseModel<string> response = new ResponseModel<string>();
        response.Message = "Post method successfully applied";
        response.Success = true;
        response.Data = $"user first name: {userRegistration.FirstName}, last name:{userRegistration.LastName}, email: {userRegistration.Email}";
        return Ok(response);
    }

    [HttpPut]
    public IActionResult Put(UserRegistrationModel userUpdate)
    {
        ResponseModel<string> response = new ResponseModel<string>();
        response.Message = "Put method successfully applied";
        response.Success = true;
        response.Data = $"The data after updating user at the database-- user first name: {userUpdate.FirstName}, last name:{userUpdate.LastName}, email: {userUpdate.Email}";
        return Ok(response);
    }

    [HttpPatch]
    public IActionResult Patch(UserUpdationModel userPatch)
    {
        ResponseModel<string> response = new ResponseModel<string>();
        response.Message = "Patch method successfully applied";
        response.Success = true;
        response.Data = $"The data after patching user data at the database-- user first name: {userPatch.FirstName}, last name:{userPatch.LastName}, email: {userPatch.Email}";
        return Ok(response);
    }
    

    [HttpDelete]
    public IActionResult Delete(UserRegistrationModel userDeletion)
    {
        ResponseModel<string> response = new ResponseModel<string>();
        response.Message = "Delete method successfully applied";
        response.Success = true;
        response.Data = $"The user with email: {userDeletion.Email} is deleted";
        return Ok(response);
    }

    
}
