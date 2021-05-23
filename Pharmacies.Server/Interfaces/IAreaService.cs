using Pharmacies.Server.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pharmacies.Server.Interfaces
{
    interface IAreaService
    {
        Task AddNewArea(AreaModel area);

        Task<List<AreaModel>> GetAreas();
    }
}
