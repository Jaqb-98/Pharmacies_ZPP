using Pharmacies.Server.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Pharmacies.Server.Interfaces
{
    public interface IPharmacyService
    {
        Task<Rootobject> SearchPharmacies(double lat, double lng, int range, string type = "pharmacy");
        Task<Location> GetSearchedLocation(string input);
        Rootobject GetObj();


    }
}
