using ShortURLDTE.Application.Interfaces;
using ShortURLDTE.Domain.Entities;
using System.Collections.Concurrent;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace ShortURLDTE.Infrastructure.Persistence
{
    public class InMemoryFacturaRepository : IFacturaRepository
    {
        private readonly ConcurrentDictionary<string, Factura> _db = new();

        public Task GuardarAsync(Factura factura)
        {
            _db[factura.ShortUrl] = factura;
            return Task.CompletedTask;
        }

        public Task<Factura?> ObtenerPorShortUrlAsync(string shortUrl)
        {
            _db.TryGetValue(shortUrl, out var factura);
            return Task.FromResult(factura);
        }

        public Task<bool> IncrementarUsoAsync(string shortUrl)
        {
            if (_db.TryGetValue(shortUrl, out var factura))
            {
                if (factura.UsosRestantes > 0)
                {
                    factura.UsosRestantes--;
                    return Task.FromResult(true);
                }
            }
            return Task.FromResult(false);
        }
    }
}
