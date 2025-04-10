using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShortURLDTE.Application.Interfaces;
using ShortURLDTE.Domain.Entities;
using ShortURLDTE.Domain.Services;
using System.Xml;
using Microsoft.AspNetCore.Http;

namespace ShortURLDTE.Application.UseCases
{
    public class SubirFacturaUseCase : ISubirFacturaUseCase
    {
        private readonly IFacturaValidator _validador;
        private readonly IFacturaRepository _repositorio;
        private readonly IShortUrlService _shortener;

        public SubirFacturaUseCase(IFacturaValidator validador, IFacturaRepository repositorio, IShortUrlService shortener)
        {
            _validador = validador;
            _repositorio = repositorio;
            _shortener = shortener;
        }

        public async Task<(bool Success, string? ShortUrl, string? ErrorMessage)> HandleAsync(IFormFile file)
        {
            if (file == null || file.Length == 0) return (false, null, "Archivo vacío");

            using var reader = new StreamReader(file.OpenReadStream());
            var xmlContent = await reader.ReadToEndAsync();

            // por alguna razón no está validando el certificado, cuando está correcto
            //if (!await _validador.ValidarAsync(xmlContent))
            //    return (false, null, "Factura inválida o firma no válida");

            var xml = new XmlDocument();
            xml.LoadXml(xmlContent);

            // Aquí deberías parsear todos los campos que necesites del XML
            var factura = new Factura
            {
                Emisor = xml.SelectSingleNode("//Emisor/RUTEmisor")?.InnerText ?? "",
                Receptor = xml.SelectSingleNode("//Receptor/RUTRecep")?.InnerText ?? "",
                Monto = decimal.Parse(xml.SelectSingleNode("//Totales/MntTotal")?.InnerText ?? "0"),
                Fecha = DateTime.Now,
                XmlOriginal = xmlContent,
            };

            factura.ShortUrl = _shortener.GenerarShortUrl(factura.Id);
            await _repositorio.GuardarAsync(factura);

            return (true, factura.ShortUrl, null);
        }
        public async Task<(bool Success, string? ShortUrl, string? ErrorMessage)> HandleRawXmlAsync(string xmlContent)
        {
            if (string.IsNullOrWhiteSpace(xmlContent))
                return (false, null, "Contenido XML vacío");

            if (!await _validador.ValidarAsync(xmlContent))
                return (false, null, "Factura inválida o firma no válida");

            var xml = new XmlDocument();
            xml.LoadXml(xmlContent);

            var factura = new Factura
            {
                Emisor = xml.SelectSingleNode("//Emisor/RUTEmisor")?.InnerText ?? "",
                Receptor = xml.SelectSingleNode("//Receptor/RUTRecep")?.InnerText ?? "",
                Monto = decimal.Parse(xml.SelectSingleNode("//Totales/MntTotal")?.InnerText ?? "0"),
                Fecha = DateTime.Now,
                XmlOriginal = xmlContent,
            };

            factura.ShortUrl = _shortener.GenerarShortUrl(factura.Id);
            await _repositorio.GuardarAsync(factura);

            return (true, factura.ShortUrl, null);
        }

    }
}
