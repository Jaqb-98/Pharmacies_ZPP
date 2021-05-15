using Pharmacies.Server.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Pharmacies.Server.Models;
using Microsoft.AspNetCore.Http;

namespace Pharmacies.Server.Interfaces
{
    interface IUsersService
    {
        Task<ICollection<ApplicationUser>> GetUsers();

        Task VerifyAccount(string id);

        Task DemoteUser(string id);

        Task<IdentityUserRole<string>> GetUserRole(string id);

        Task<ApplicationUser> GetUser(string id);

        Task DeleteUser(string id);

        Task<IdentityRole> GetRole(string id);

        Task AddPharmacy(string id, PharmacyModel pharmacy);

        Task<ICollection<PharmacyModel>> GetUsersPharmacies(string id);

        string UploadImageOnServer(IFormFile image);

        Task SavePharmacy(PharmacyModel pharmacy, string userId);

        List<PharmacyModel> GetPlacesInRange(double lat, double lng, int rangeInKm);


    }
}
