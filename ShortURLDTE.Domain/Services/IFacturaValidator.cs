using ShortURLDTE.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShortURLDTE.Domain.Services
{
    public interface IFacturaValidator
    {
        Task<bool> ValidarAsync(string xmlContent);
    }
}
