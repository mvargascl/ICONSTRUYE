using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShortURLDTE.Domain.Entities;
public class Factura
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string Emisor { get; set; } = string.Empty;
    public string Receptor { get; set; } = string.Empty;
    public decimal Monto { get; set; }
    public DateTime Fecha { get; set; }
    public string XmlOriginal { get; set; } = string.Empty;
    public string ShortUrl { get; set; } = string.Empty;
    public int UsosRestantes { get; set; } = 3;
    public int VecesConsultada { get; private set; } = 0;

    public bool PuedeSerConsultada()
    {
        return VecesConsultada < 3;
    }
    public void RegistrarConsulta()
    {
        if (!PuedeSerConsultada())
            throw new InvalidOperationException("La factura ya no puede ser consultada.");

        VecesConsultada++;
    }
}
