using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShortURLDTE.Domain.Entities;

namespace ShortURLDTE.Application.Interfaces
{
    public interface IFacturaRepository
    {
        Task GuardarAsync(Factura factura);
        Task<Factura?> ObtenerPorShortUrlAsync(string shortUrl);
        Task<bool> IncrementarUsoAsync(string shortUrl);
    }
}
