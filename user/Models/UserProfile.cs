using Microsoft.AspNetCore.Identity;

namespace App.Models
{
    public class UserProfile  : IdentityUser
    {
        public UserProfile(string cid,string firstname,string lastname)
        {
            Cid = cid;
            FirtName = firstname;
            LastName = lastname;
        }
        public string? Cid { get; set; }
        public string? FirtName { get; set; }
        public string? LastName { get; set; }
    }

}