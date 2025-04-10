using ShortURLDTE.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShortURLDTE.Infrastructure.ShortUrl
{
    public class Base62Shortener : IShortUrlService
    {
        private const string Base62Chars = "0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";

        public string GenerarShortUrl(string id)
        {
            var bytes = Encoding.UTF8.GetBytes(id);
            var number = BitConverter.ToUInt64(bytes, 0);
            var sb = new StringBuilder();

            while (number > 0)
            {
                sb.Insert(0, Base62Chars[(int)(number % 62)]);
                number /= 62;
            }

            return sb.ToString();
        }
    }

}
