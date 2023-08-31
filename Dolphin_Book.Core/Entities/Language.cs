using Dolphin_Book.Core.Entities.BaseEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dolphin_Book.Core.Entities
{
    public class Language:BaseEntity
    {
        public string Name { get; set; }
        public List<Book>? Books { get; set; }
    }
}
