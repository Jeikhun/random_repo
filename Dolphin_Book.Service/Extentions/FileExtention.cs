using System;
using Microsoft.AspNetCore.Http;

namespace Dolphin_Book.Service.Extentions
{
    public static class FileUpload
    {
        public static string CreateImage(this IFormFile file, string root, string path)
        {
            string FileName = Guid.NewGuid().ToString() + file.FileName;
            string FullPath = Path.Combine(root, path, FileName);
            using (FileStream filestream = new FileStream(FullPath, FileMode.Create))
            {
                file.CopyTo(filestream);
            }
            return FileName;
        }
    }
}

