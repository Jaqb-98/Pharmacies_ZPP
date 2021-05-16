using Pharmacies.Server.Data;
using Pharmacies.Server.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Identity;
using Pharmacies.Server.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Components.Forms;
using System.IO;
using Microsoft.AspNetCore.Hosting;

namespace Pharmacies.Server.Services
{
    public class UsersService : IUsersService
    {
        private PharmaciesServerContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public UsersService(PharmaciesServerContext context, IWebHostEnvironment hostEnvironment)
        {
            _context = context;
            _webHostEnvironment = hostEnvironment;
        }

        /// <summary>
        /// Returns all verified and unverified users
        /// </summary>
        /// <returns></returns>
        public async Task<ICollection<ApplicationUser>> GetUsers()
        {
            var adminRoleId = _context.Roles.Where(x => x.Name == "Admin").Select(x => x.Id).FirstOrDefault();
            var inactiveRoleId = _context.Roles.Where(x => x.Name == "Inactive").Select(x => x.Id).FirstOrDefault();
            var usersIds = _context.UserRoles.Where(x => x.RoleId != adminRoleId && x.RoleId != inactiveRoleId).Select(x => x.UserId).ToList();
            return await _context.Users.Where(x => usersIds.Contains(x.Id)).ToListAsync();
        }

        /// <summary>
        /// Changes users role to VerifiedUser
        /// </summary>
        /// <param name="id">Users id</param>
        public void VerifyAccount(string id)
        {
            var record = _context.UserRoles.FirstOrDefault(x => x.UserId == id);
            if (record != null)
            {
                _context.UserRoles.Remove(record);
                var verifiedRole = _context.Roles.Where(x => x.Name == "VerifiedUser").FirstOrDefault();
                var newUserRole = new IdentityUserRole<string> { RoleId = verifiedRole.Id, UserId = id };
                _context.UserRoles.Add(newUserRole);
                _context.SaveChanges();
            }

        }

        /// <summary>
        /// Changes users role to User
        /// </summary>
        /// <param name="id">Users id</param>
        public void DemoteUser(string id)
        {
            var record = _context.UserRoles.FirstOrDefault(x => x.UserId == id);
            if (record != null)
            {
                _context.UserRoles.Remove(record);
                var userRole = _context.Roles.Where(x => x.Name == "User").FirstOrDefault();
                var newUserRole = new IdentityUserRole<string> { RoleId = userRole.Id, UserId = id };
                _context.UserRoles.Add(newUserRole);
                _context.SaveChanges();
            }
        }

        /// <summary>
        /// Disactivates users account
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task DeleteUser(string id)
        {
            var record = _context.UserRoles.FirstOrDefault(x => x.UserId == id);
            if (record != null)
            {
                _context.UserRoles.Remove(record);
                var inactiveRole = _context.Roles.Where(x => x.Name == "Inactive").FirstOrDefault();
                var newUserRole = new IdentityUserRole<string> { RoleId = inactiveRole.Id, UserId = id };
                _context.UserRoles.Add(newUserRole);
                await _context.SaveChangesAsync();
            }
        }


        /// <summary>
        /// Returns the role of the user with the given id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<IdentityUserRole<string>> GetUserRole(string id)
        {
            return await _context.UserRoles.Where(x => x.UserId == id).FirstOrDefaultAsync();
        }


        /// <summary>
        /// Returns user with the given id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ApplicationUser> GetUser(string id)
        {
            return await _context.Users.Where(x => x.Id == id).FirstOrDefaultAsync();
        }

        /// <summary>
        /// Returns role with the given id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<IdentityRole> GetRole(string id)
        {
            return await _context.Roles.Where(x => x.Id == id).FirstOrDefaultAsync();
        }


        /// <summary>
        /// Adds pharmacy to user with the given id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="pharmacy"></param>
        /// <returns></returns>
        public async Task AddPharmacy(string id, PharmacyModel pharmacy)
        {
            var user = _context.Users.Where(x => x.Id == id).FirstOrDefault();
            user.UsersPharamcies.Add(pharmacy);
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Returns list of all pharmacies assigned to user with the given id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ICollection<PharmacyModel>> GetUsersPharmacies(string id)
        {
            var pharmacies = _context.Users.Where(x => x.Id == id).Include(x => x.UsersPharamcies).Select(x => x.UsersPharamcies).FirstOrDefault();

            var pharmaciesList = pharmacies.ToList();

            return pharmaciesList;
        }

        /// <summary>
        /// Uploads image with unique name to the server 
        /// <param name="image"></param>
        /// <returns></returns>
        public string UploadImageOnServer(IFormFile image)
        {
            string uniqueFileName = null;
            if (image != null)
            {
                string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images");
                uniqueFileName = Guid.NewGuid().ToString() + "_" + image.Name;
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    image.CopyTo(fileStream);
                }

            }

            return uniqueFileName;

        }

        /// <summary>
        /// Adds new pharmacy object to user with the given id
        /// </summary>
        /// <param name="pharmacy"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task SavePharmacy(PharmacyModel pharmacy, string userId)
        {
            var user = _context.Users.Where(x => x.Id == userId).FirstOrDefault();
            user.UsersPharamcies.Add(pharmacy);
            _context.SaveChanges();
        }


        /// <summary>
        /// Returns all places that are in range of given coordinates
        /// </summary>
        /// <param name="lat">Latitude</param>
        /// <param name="lng">Longtitude</param>
        /// <param name="rangeInMeters">Search radius in meters</param>
        /// <returns></returns>
        public List<PharmacyModel> GetPlacesInRange(double lat, double lng, int rangeInMeters)
        {

            var pharmacies = _context.Pharmacies.FromSqlRaw(
                $"SELECT Id, Name, Vicinity, lat, lng, PhotoReference,ApplicationUserId " +
                $"FROM (" +
                    $"SELECT Id, Name, Vicinity, lat, lng, PhotoReference,ApplicationUserId, ( 3959 * acos( cos( radians({lat}) ) * cos( radians( lat ) ) * cos( radians( lng ) - radians({lng}) ) + sin( radians({lat}) ) * sin( radians( lat ) ) ) )*1609.34 AS distance FROM dbo.Pharmacies) as temp " +
                $"WHERE distance < {rangeInMeters}").ToList();

            return pharmacies;
        }



    }


}
