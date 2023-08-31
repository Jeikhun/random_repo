using Dolphin_Book.Core.Entities.BaseEntities;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dolphin_Book.Core.Entities
{
    public class Toy:BaseEntity
    {
        public string Name { get; set; }
        public string PurshacePrice { get; set; }
        public string SalePrice { get; set; }
        public string Description { get; set; }
        public Seller? Seller { get; set; }
        public Publisher? Publisher { get; set; }

        public List<ToyImage>? toyImages { get; set; }
        [NotMapped]
        public List<IFormFile>? FormFiles { get; set; }
        [NotMapped]
        public List<int>? CategoryIds { get; set; }
        public int? PublisherId { get;set; }
        public int? SellerId { get; set; }

        public List<ToyCategory>? ToyCategories { get; set; }

    }
}
