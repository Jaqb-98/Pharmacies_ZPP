using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Pharmacies.Server.Models
{
    public class PharmacyModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public float lat { get; set; }
        public float lng { get; set; }
        public string Vicinity { get; set; }
        public string PhotoReference { get; set; }

    }
}
