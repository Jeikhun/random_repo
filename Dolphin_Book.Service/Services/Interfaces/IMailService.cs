using System.Globalization;

namespace Dolphin_Book.Service.Services.Interfaces
{
    public interface IMailService
    {
        public Task Send(string from,string to,string subject,string text,string link,string name);
    }
}
