using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShortURLDTE.Application.Models
{
    public class FacturaDto
    {
        public string Emisor { get; set; }
        public string Receptor { get; set; }
        public decimal Monto { get; set; }
        public DateTime Fecha { get; set; }
        public string XmlOriginal { get; set; }
    }

}
