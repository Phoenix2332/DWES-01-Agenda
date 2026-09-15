using Agenda.Model;
using FluentAssertions;

namespace Agenda.Test.Models;

[TestFixture]
public class ContactosTest {
    [TestFixture]
    public class CasosPositivos {
        [Test]
        public void ToString_DeberiaRetornarFormatoCorrecto() {
            // Arrange
            var contacto = new Contactos {
                Id = 1,
                Nombre = "Antoine",
                Alias = "Antu",
                Telefono = 654357159,
                Email = "antukiller@yahoo.com",
                CreateAt = new DateTime(2024, 1, 1, 10, 0, 0),
                UpdateAt = new DateTime(2024, 1, 2, 12, 0, 0)
            };

            // Act
            var resultado = contacto.ToString();

            // Assert
            resultado.Should().Contain("Antoine");
            resultado.Should().Contain("Antu");
            resultado.Should().Contain("654357159");
            resultado.Should().Contain("antukiller@yahoo.com");
        }

        [Test]
        public void Equals_MismoTelefono_DeberiaSerIgual() {
            // Arrange
            var contacto1 = new Contactos { Telefono = 654357159 };
            var contacto2 = new Contactos { Telefono = 654357159 };

            // Act
            var resultado = contacto1.Equals(contacto2);

            // Assert
            resultado.Should().BeTrue();
        }

        [Test]
        public void Equals_TelefonoDiferente_DeberiaSerDistinto() {
            // Arrange
            var contacto1 = new Contactos { Telefono = 654357159 };
            var contacto2 = new Contactos { Telefono = 671493852 };

            // Act
            var resultado = contacto1.Equals(contacto2);

            // Assert
            resultado.Should().BeFalse();
        }

        [Test]
        public void GetHashCode_MismoTelefono_MismoHashCode() {
            // Arrange
            var contacto1 = new Contactos { Telefono = 654357159 };
            var contacto2 = new Contactos { Telefono = 654357159 };

            // Act
            var hash1 = contacto1.GetHashCode();
            var hash2 = contacto2.GetHashCode();

            // Assert
            hash1.Should().Be(hash2);
        }
    }

    [TestFixture]
    public class CasosNegativos {
        [Test]
        public void Equals_Nulo_DeberiaRetornarFalse() {
            // Arrange
            var contacto = new Contactos { Telefono = 671493852 };

            // Act
            var resultado = contacto.Equals(null);

            // Assert
            resultado.Should().BeFalse();
        }
    }
}