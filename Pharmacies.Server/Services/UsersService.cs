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

    /// <inheritdoc/>
    public class UsersService : IUsersService
    {
        private PharmaciesServerContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public UsersService(PharmaciesServerContext context, IWebHostEnvironment hostEnvironment)
        {
            _context = context;
            _webHostEnvironment = hostEnvironment;
        }


        public async Task<ICollection<ApplicationUser>> GetUsers()
        {
            var adminRoleId = _context.Roles.Where(x => x.Name == "Admin").Select(x => x.Id).FirstOrDefault();
            var inactiveRoleId = _context.Roles.Where(x => x.Name == "Inactive").Select(x => x.Id).FirstOrDefault();
            var usersIds = _context.UserRoles.Where(x => x.RoleId != adminRoleId && x.RoleId != inactiveRoleId).Select(x => x.UserId).ToList();
            return await _context.Users.Where(x => usersIds.Contains(x.Id)).ToListAsync();
        }


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


 
        public async Task<IdentityUserRole<string>> GetUserRole(string id)
        {
            return await _context.UserRoles.Where(x => x.UserId == id).FirstOrDefaultAsync();
        }


 
        public async Task<ApplicationUser> GetUser(string id)
        {
            return await _context.Users.Where(x => x.Id == id).FirstOrDefaultAsync();
        }


        public async Task<IdentityRole> GetRole(string id)
        {
            return await _context.Roles.Where(x => x.Id == id).FirstOrDefaultAsync();
        }


 
        public async Task AddPharmacy(string id, PharmacyModel pharmacy)
        {
            var user = _context.Users.Where(x => x.Id == id).FirstOrDefault();
            user.UsersPharamcies.Add(pharmacy);
            await _context.SaveChangesAsync();
        }


        public async Task<ICollection<PharmacyModel>> GetUsersPharmacies(string id)
        {
            var pharmacies = _context.Users.Where(x => x.Id == id).Include(x => x.UsersPharamcies).Select(x => x.UsersPharamcies).FirstOrDefault();

            var pharmaciesList = pharmacies.ToList();

            return pharmaciesList;
        }

 
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

    
        public async Task SavePharmacy(PharmacyModel pharmacy, string userId)
        {
            var user = _context.Users.Where(x => x.Id == userId).FirstOrDefault();
            user.UsersPharamcies.Add(pharmacy);
            _context.SaveChanges();
        }


 
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
