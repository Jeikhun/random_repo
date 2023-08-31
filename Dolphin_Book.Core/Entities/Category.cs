using Dolphin_Book.Core.Entities;
using Dolphin_Book.Core.Entities.BaseEntities;
using System;
using System.ComponentModel.DataAnnotations;

namespace Dolphin_Book.Core.Entities
{
	public class Category : BaseEntity
	{
		public string Name { get; set; }
        public List<BookCategory>? BookCategories { get; set; }
		public List<ToyCategory>? ToyCategories { get; set; }

    }
}

