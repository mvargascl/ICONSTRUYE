using ShortURLDTE.Domain;
using System.Text;

namespace ShortURLDTE.Application.Interfaces
{
    public interface IShortUrlService
    {
        string GenerarShortUrl(string id);
    }
}
