using Dolphin_Book.Core.Entities.BaseEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dolphin_Book.Core.Entities
{
    public class ToyImage:BaseEntity
    {
        public bool isMain { get; set; }
        public string? Image { get; set; }
        public int ToyId { get; set; }
        public Toy? Toy { get; set; }
    }
}
