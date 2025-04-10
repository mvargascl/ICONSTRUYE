using ShortURLDTE.Application.Interfaces;
using ShortURLDTE.Application.Models;
using ShortURLDTE.Domain.Entities;
using Microsoft.AspNetCore.Http;
using System.Xml;

namespace ShortURLDTE.Application.UseCases
{
    public interface ISubirFacturaUseCase
    {
        Task<(bool Success, string? ShortUrl, string? ErrorMessage)> HandleAsync(IFormFile file);
        Task<(bool Success, string? ShortUrl, string? ErrorMessage)> HandleRawXmlAsync(string xmlContent);
    }

}
