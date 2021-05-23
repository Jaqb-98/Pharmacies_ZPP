using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pharmacies.Server.Models
{
    public class AreaModel
    {
        public string Id { get; set; }
        public string AreaName { get; set; }
        public string Description { get; set; }
        public string BoundariesCoords { get; set; }
    }
}
