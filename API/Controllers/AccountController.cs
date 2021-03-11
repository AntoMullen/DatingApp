using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using API.Data;
using API.DTOs;
using API.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    public class AccountController :BaseApiController
    {
        private readonly DataContext _context;

        public AccountController(DataContext context)
        {
            _context = context;
        }

        [HttpPost("register")]
        public async Task<ActionResult<AppUser>> Register(RegisterDTO registerdto)
        {
            //If the username alread exists return error saying that this already is taken
            if(await UserExists(registerdto.Username)) return BadRequest("Username is taken");

            //we will use this object to hash the password.
            using var hmac = new HMACSHA512();

            var user = new AppUser
            {
                UserName = registerdto.Username.ToLower(),//Using ToLower because thats what our UserExists compares to
                PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerdto.Password)),//The password hash is using the hmac.Key to generate hash. Remeber they are both byte arrays.
                PasswordSalt = hmac.Key//The HMAC will generate a unique key for every object it creates. we will save this in the database for this user/password
            };

            //now add the user to ef
            _context.Users.Add(user);
            //Now insert into db async
            await _context.SaveChangesAsync();

            return user;
        }

        private async Task<bool> UserExists(string username)
        {
            //Pass in anyomous method (lamba) get the user from database and return the comaprison of UserName to string username
            return await _context.Users.AnyAsync(user => user.UserName == username.ToLower());
        }

        [HttpPost("login")]
        
        public async Task<ActionResult<AppUser>> Login(LoginDTO logindto)
        {
            var user = await _context.Users.
                    SingleOrDefaultAsync(user => user.UserName == logindto.Username.ToLower());
            
            if(user == null) return Unauthorized("Invalid username");

            //Create the hmac with the orignal users password salt
            using var hmac = new HMACSHA512(user.PasswordSalt);

            //Compute hash for password user entered at login.
            var computedhash = hmac.ComputeHash(Encoding.UTF8.GetBytes(logindto.Password));

            //now compare each byte in array in database user password to computed hash
            for( int i = 0; i < computedhash.Length; i++)
            {
                if(computedhash[i] != user.PasswordHash[i]) return Unauthorized("Invalid password");
            }

            return user;
        }


    }
}