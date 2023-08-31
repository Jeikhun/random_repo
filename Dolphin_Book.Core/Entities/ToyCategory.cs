using Dolphin_Book.Core.Entities.BaseEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dolphin_Book.Core.Entities
{
    public class ToyCategory:BaseEntity
    {
        public int ToyId { get; set; }
        public int CategoryId { get; set; }
        public Toy? Toy { get; set; }
        public Category? Category { get; set; }
    }
}
