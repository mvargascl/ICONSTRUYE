using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShortURLDTE.Domain.Entities;

namespace ShortURLDTE.Application.UseCases
{
    public interface IConsultarFacturaUseCase
    {
        Task<Factura?> HandleAsync(string shortUrl);
    }
}
