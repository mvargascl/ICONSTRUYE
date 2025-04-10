using ShortURLDTE.Domain;
using ShortURLDTE.Domain.Entities;
using Xunit;

namespace ShortURLDTE.Tests
{
    public class FacturaTests
    {
        [Fact]
        public void Factura_PuedeSerConsultada_TresVeces()
        {
            // Arrange
            var factura = new Factura();

            // Act & Assert
            Assert.True(factura.PuedeSerConsultada());
            factura.RegistrarConsulta();

            Assert.True(factura.PuedeSerConsultada());
            factura.RegistrarConsulta();

            Assert.True(factura.PuedeSerConsultada());
            factura.RegistrarConsulta();

            Assert.False(factura.PuedeSerConsultada());
        }

        [Fact]
        public void Factura_NoPermiteMasDeTresConsultas()
        {
            var factura = new Factura();
            factura.RegistrarConsulta();
            factura.RegistrarConsulta();
            factura.RegistrarConsulta();

            Assert.Throws<InvalidOperationException>(() => factura.RegistrarConsulta());
        }
    }
}