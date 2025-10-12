using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class ProfileInfo
    {

        // represents the Id of the application
        public string? ClientId { get; set; } = null;
        // represents the profile (customer) Id which the application is for
        public string? ProfileId { get; set; } = null;
        public string? UserId { get; set; } = null;

        public ProfileInfo()
        {
        }

        public ProfileInfo(string profileId, string clientId, string userId = null)
        {
            this.ProfileId = profileId;
            this.ClientId = clientId;
            this.UserId = userId;
        }
    }
}
