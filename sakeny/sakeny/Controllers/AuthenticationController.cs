using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace sakeny.Controllers
{
    [Route("api/authentication")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        public class AuthenticationRequestBody
        {

            public string? UserName { get; set; }
            public string? Password { get; set; }
        }

        [HttpPost("{authenticate}")]
        public ActionResult<string> Authenticate (AuthenticationRequestBody authenticationRequestBody)
        {
            var user = validateUserCredentials
                (authenticationRequestBody.UserName, authenticationRequestBody.Password);
        }

        private UserInfo validateUserCredentials(string? userName, string? password)
        {
            
        }

        private class UserInfo
        {
            public UserInfo(int id, string firstName, string lastName, string city)
            {
                Id = id;
                FirstName = firstName;
                LastName = lastName;
                City = city;
            }

            public int Id { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string City { get; set; }

        }

        
    }

    

    
}
