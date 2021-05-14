using Microsoft.AspNetCore.Identity;
using Pharmacies.Server.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pharmacies.Server.Data
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public virtual ICollection<PharmacyModel> UsersPharamcies { get; set; }

        public ApplicationUser()
        {
            UsersPharamcies = new HashSet<PharmacyModel>();
        }
    }
}
