using Microsoft.AspNetCore.Hosting;
using Pharmacies.Server.Data;
using Pharmacies.Server.Models;
using Pharmacies.Server.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Pharmacies.Server.Services
{
    public class AreaService : IAreaService
    {
        private PharmaciesServerContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public AreaService(PharmaciesServerContext context, IWebHostEnvironment hostEnvironment)
        {
            _context = context;
            _webHostEnvironment = hostEnvironment;
        }

        public async Task AddNewArea(AreaModel area)
        {
            _context.Areas.Add(area);
            await _context.SaveChangesAsync();
        }

        public async Task<List<AreaModel>> GetAreas()
        {
            var areas = await _context.Areas.ToListAsync();
            return areas;
        }
    }
}
