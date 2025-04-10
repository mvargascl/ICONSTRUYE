using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShortURLDTE.Application.Interfaces;
using ShortURLDTE.Domain.Entities;

namespace ShortURLDTE.Application.UseCases
{
    public class ConsultarFacturaUseCase : IConsultarFacturaUseCase
    {
        private readonly IFacturaRepository _repositorio;

        public ConsultarFacturaUseCase(IFacturaRepository repositorio)
        {
            _repositorio = repositorio;
        }

        public async Task<Factura?> HandleAsync(string shortUrl)
        {
            var factura = await _repositorio.ObtenerPorShortUrlAsync(shortUrl);

            if (factura == null || factura.UsosRestantes <= 0)
                return null;

            await _repositorio.IncrementarUsoAsync(shortUrl);
            return factura;
        }
    }

}
