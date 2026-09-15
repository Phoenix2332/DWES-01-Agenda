using Agenda.Entity;
using Agenda.Mapper;
using Agenda.Model;
using FluentAssertions;

namespace Agenda.Test.Mappers;

[TestFixture]
public class ContactoMapperTest {
    [TestFixture]
    public class CasosPositivos {
        [SetUp]
        public void SetUp() {
            _contactos = new Contactos {
                Id = 1,
                Nombre = "Antoine",
                Alias = "Antu",
                Telefono = 654357159,
                Email = "antukiller@yahoo.com",
                CreateAt = new DateTime(2024, 1, 1, 10, 0, 0),
                UpdateAt = new DateTime(2024, 1, 2, 12, 0, 0)
            };

            _contactosEntity = new ContactosEntity {
                Id = 1,
                Nombre = "Antoine",
                Alias = "Antu",
                Telefono = 654357159,
                Email = "antukiller@yahoo.com",
                CreateAt = new DateTime(2024, 1, 1, 10, 0, 0),
                UpdateAt = new DateTime(2024, 1, 2, 12, 0, 0)
            };
        }

        private Contactos _contactos = null!;
        private ContactosEntity _contactosEntity = null!;

        [Test]
        public void ToModel_ContactosEntity_DeberiaConvertirCorrectamente() {
            // Act
            var resultado = _contactosEntity.ToModel();

            // Assert
            resultado.Should().NotBeNull();
            resultado.Id.Should().Be(1);
            resultado.Nombre.Should().Be("Antoine");
        }

        [Test]
        public void ToEntity_Contactos_DeberiaConvertirCorrectamente() {
            // Act
            var resultado = _contactos.ToEntity();

            // Assert
            resultado.Should().NotBeNull();
            resultado.Id.Should().Be(1);
            resultado.Nombre.Should().Be("Antoine");
        }

        [Test]
        public void ToModel_ListaEntities_DeberiaConvertirTodos() {
            // Arrange
            var entities = new List<ContactosEntity> { _contactosEntity };

            // Act
            var resultado = entities.ToModel();

            // Assert
            resultado.Should().HaveCount(1);
        }

        [Test]
        public void ToModel_EntityNulo_DeberiaRetornarNull() {
            // Act
            var resultado = ((ContactosEntity?)null).ToModel();

            // Assert
            resultado.Should().BeNull();
        }
    }
}