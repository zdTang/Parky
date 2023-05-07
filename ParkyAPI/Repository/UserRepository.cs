using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using ParkyAPI.Data;
using ParkyAPI.Models;
using ParkyAPI.Repository.IRepository;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ParkyAPI.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _db;
        private readonly AppSettings _appSettings; // To retrieve value from AppSetting.json, we can have a better way
        public UserRepository(ApplicationDbContext db, IOptions<AppSettings> appSettings)
        {
            _db = db;
            _appSettings = appSettings.Value;
        }

        public User? Authenticate(string userName, string password)
        {
            var user = _db.Users.SingleOrDefault(u => u.UserName == userName && u.Password == password);
            if (user == null) return null;
            //if user was found generate JWT Token
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret!);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.Id.ToString()),  // ClaimType--Name
                    new Claim(ClaimTypes.Role,user.Role!)            // ClaimType--Role : The trick is here, how to add role to User
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            user.Token = tokenHandler.WriteToken(token);
            user.Password = "";// this value has been used, we don't show it again
            return user;
        }

        public bool IsUniqueUser(string username)
        {
            var user = _db.Users.SingleOrDefault(t => t.UserName == username);
            if (user == null) return true;  // Not find in DB, means the coming NAME is good to use
            return false;
        }

        public User Register(string userName, string password)
        {
            var userObj = new User
            {
                UserName = userName,
                Password = password,
                Role = "Admin"
            };
            _db.Users.Add(userObj);
            _db.SaveChanges();
            userObj.Password = "";// empty the password
            return userObj;
        }
    }
}
