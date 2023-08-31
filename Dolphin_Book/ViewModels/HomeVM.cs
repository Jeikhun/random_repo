using Dolphin_Book.Core.Entities;

namespace Dolphin_Book.ViewModels
{
    public class HomeVM
    {
        public IEnumerable<Book> books { get; set; }
        public IEnumerable<Slide> slides { get; set; }
    }
}
