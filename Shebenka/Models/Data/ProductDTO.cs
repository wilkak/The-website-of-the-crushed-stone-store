using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

//using System.Device.Location;
namespace Shebenka.Models.Data
{
    [Table("TableProducts")]
    public class ProductDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Slug { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string ImageName { get; set; }
        public decimal OldPrice { get; set; }
        public bool View { get; set; }
    }
}
