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


namespace Pharmacies.Server.Services
{
    public class UsersService : IUsersService
    {
        private PharmaciesServerContext _context;

        public UsersService(PharmaciesServerContext context)
        {
            _context = context;
        }

        public async Task<ICollection<ApplicationUser>> GetUsers()
        {
            var adminRoleId = _context.Roles.Where(x => x.Name == "Admin").Select(x => x.Id).FirstOrDefault();
            var usersIds = _context.UserRoles.Where(x => x.RoleId != adminRoleId).Select(x => x.UserId).ToList();
            return await _context.Users.Where(x => usersIds.Contains(x.Id)).ToListAsync();
        }

        public async Task VerifyAccount(string id)
        {
            var record = _context.UserRoles.FirstOrDefault(x => x.UserId == id);
            if (record != null)
            {
                _context.UserRoles.Remove(record);
                var verifiedRole = _context.Roles.Where(x => x.Name == "VerifiedUser").FirstOrDefault();
                var newUserRole = new IdentityUserRole<string> { RoleId = verifiedRole.Id, UserId = id };
                _context.UserRoles.Add(newUserRole);
                await _context.SaveChangesAsync();
            }

        }
        public async Task DemoteUser(string id)
        {
            var record = _context.UserRoles.FirstOrDefault(x => x.UserId == id);
            if (record != null)
            {
                _context.UserRoles.Remove(record);
                var userRole = _context.Roles.Where(x => x.Name == "User").FirstOrDefault();
                var newUserRole = new IdentityUserRole<string> { RoleId = userRole.Id, UserId = id };
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

        public async Task DeleteUser(string id)
        {
            var user = _context.Users.Where(x => x.Id == id).FirstOrDefault();
            _context.Users.Remove(user);
            var userRole = _context.UserRoles.Where(x => x.UserId == user.Id).FirstOrDefault();
            _context.UserRoles.Remove(userRole);

            await _context.SaveChangesAsync();
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

    }


}
