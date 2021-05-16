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
        /// <summary>
        /// Returns all verified and unverified users
        /// </summary>
        /// <returns></returns>
        Task<ICollection<ApplicationUser>> GetUsers();

        /// <summary>
        /// Changes users role to VerifiedUser
        /// </summary>
        /// <param name="id">Users id</param>
        void VerifyAccount(string id);


        /// <summary>
        /// Changes users role to User
        /// </summary>
        /// <param name="id">Users id</param>
        void DemoteUser(string id);

        /// <summary>
        /// Returns the role of the user with the given id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<IdentityUserRole<string>> GetUserRole(string id);

        /// <summary>
        /// Returns user with the given id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<ApplicationUser> GetUser(string id);

        /// <summary>
        /// Disactivates users account
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task DeleteUser(string id);

        /// <summary>
        /// Returns role with the given id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<IdentityRole> GetRole(string id);

        /// <summary>
        /// Adds pharmacy to user with the given id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="pharmacy"></param>
        /// <returns></returns>
        Task AddPharmacy(string id, PharmacyModel pharmacy);

        /// <summary>
        /// Returns list of all pharmacies assigned to user with the given id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<ICollection<PharmacyModel>> GetUsersPharmacies(string id);

        /// <summary>
        /// Uploads image with unique name to the server 
        /// <param name="image"></param>
        /// <returns></returns>
        string UploadImageOnServer(IFormFile image);

        /// <summary>
        /// Adds new pharmacy object to user with the given id
        /// </summary>
        /// <param name="pharmacy"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task SavePharmacy(PharmacyModel pharmacy, string userId);

        /// <summary>
        /// Returns all places that are in range of given coordinates
        /// </summary>
        /// <param name="lat">Latitude</param>
        /// <param name="lng">Longtitude</param>
        /// <param name="rangeInMeters">Search radius in meters</param>
        /// <returns></returns>
        List<PharmacyModel> GetPlacesInRange(double lat, double lng, int rangeInKm);


    }
}
