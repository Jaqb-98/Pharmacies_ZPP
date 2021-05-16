using Pharmacies.Server.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Pharmacies.Server.Interfaces
{
    public interface IPharmacyService
    {
        /// <summary>
        /// Returns places that are in range of given coordinates (GOOGLE API)
        /// </summary>
        /// <param name="lat"></param>
        /// <param name="lng"></param>
        /// <param name="range"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        Task<Rootobject> SearchPharmacies(double lat, double lng, int range, string type = "pharmacy");

        /// <summary>
        /// Transforms given address to coordinates (GOOGLE API)
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<Location> GetSearchedLocation(string input);

        /// <summary>
        /// Returns details of the place with given id (GOOGLE API)
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<Rootobject> GetPlaceDetails(string id);

        /// <summary>
        /// Returns photo from given reference (GOOGLE API)
        /// </summary>
        /// <param name="photoRef"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <returns></returns>
        string GetPhoto(string photoRef, int width, int height);


    }
}
