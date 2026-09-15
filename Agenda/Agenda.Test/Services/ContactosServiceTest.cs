using Agenda.Cache;
using Agenda.Errors;
using Agenda.Errors.Common;
using Agenda.Model;
using Agenda.Repository.Common;
using Agenda.Service;
using CSharpFunctionalExtensions;
using FluentAssertions;
using Moq;

namespace Agenda.Test.Services;

[TestFixture]
public class ContactosServiceTest {
    [TestFixture]
    public class CasosPositivos {
        [SetUp]
        public void SetUp() {
            _repositoryMock = new Mock<IRepository>();
            _cacheByIdMock = new Mock<ICache<int, Contactos>>();

            _service = new ContactosService(
                _repositoryMock.Object,
                _cacheByIdMock.Object
            );
            _listaValida = new List<Contactos> {
                new() {
                    Id = 1,
                    Nombre = "Antoine",
                    Alias = "Antu",
                    Telefono = 654357159,
                    Email = "antukiller@yahoo.com"
                },
                new() {
                    Id = 2,
                    Nombre = "Diego",
                    Alias = "PussyCat",
                    Telefono = 671493852,
                    Email = "diecgochu@yahoo.com"
                }
            };
        }

        private ContactosService _service = null!;
        private Mock<IRepository> _repositoryMock = null!;
        private Mock<ICache<int, Contactos>> _cacheByIdMock = null!;
        private List<Contactos> _listaValida = null!;

        [Test]
        public void GetAll_SinParametros_DeberiaRetornarTodasLasCitas() {
            // Arrange
            _repositoryMock.Setup(r => r.GetAll()).Returns(_listaValida);

            // Act
            var resultado = _service.GetAll().ToList();

            // Assert
            resultado.Should().HaveCount(2);
            _repositoryMock.Verify(r => r.GetAll(), Times.Once);
        }

        [Test]
        public void TotalContacto_RetornarTotal() {
            // Arrange
            _repositoryMock.Setup(r => r.GetAll(1, int.MaxValue))
                .Returns(_listaValida);

            // Act
            var resultado = _service.TotalContactos;

            // Assert
            resultado.Should().Be(2);
            _repositoryMock.Verify(r => r.GetAll(1, int.MaxValue), Times.Once);
        }

        [Test]
        public void GetById_ConCache_DeberiaRetornarDeCache() {
            // Arrange
            _cacheByIdMock.Setup(c => c.Get(1)).Returns(_listaValida[0]);

            // Act
            var resultado = _service.GetById(1);

            // Assert
            resultado.IsSuccess.Should().BeTrue();
            resultado.Value.Nombre.Should().Be("Antoine");
            _cacheByIdMock.Verify(c => c.Get(1), Times.Once);
            _repositoryMock.Verify(r => r.GetById(It.IsAny<int>()), Times.Never);
        }

        [Test]
        public void GetById_SinCache_DeberiaBuscarEnRepositorioYAgregarACache() {
            // Arrange
            _cacheByIdMock.Setup(c => c.Get(1)).Returns((Contactos?)null);
            _repositoryMock.Setup(r => r.GetById(1)).Returns(_listaValida[0]);

            // Act
            var resultado = _service.GetById(1);

            // Assert
            resultado.IsSuccess.Should().BeTrue();
            resultado.Value.Nombre.Should().Be("Antoine");
            _cacheByIdMock.Verify(c => c.Get(1), Times.Once);
            _cacheByIdMock.Verify(c => c.Add(1, _listaValida[0]), Times.Once);
            _repositoryMock.Verify(r => r.GetById(1), Times.Once);
        }

        [Test]
        public void GetByAlias_ConContactoExistente_DeberiaRetornarCita() {
            // Arrange
            _repositoryMock.Setup(r => r.GetByAlias("Antu")).Returns(_listaValida[0]);

            // Act
            var resultado = _service.GetByAlias("Antu");

            // Assert
            resultado.IsSuccess.Should().BeTrue();
            resultado.Value.Nombre.Should().Be("Antoine");
            _repositoryMock.Verify(r => r.GetByAlias("Antu"), Times.Once);
        }

        [Test]
        public void Create_ConContactoValido_DeberiaGuardarCorrectamente() {
            // Arrange
            _repositoryMock.Setup(r => r.Create(It.IsAny<Contactos>()))
                .Returns((Contactos p) => Result.Success<Contactos, DomainError>(p));

            // Act
            var resultado = _service.Create(_listaValida[0]);

            // Assert
            resultado.IsSuccess.Should().BeTrue();
            _repositoryMock.Verify(r => r.Create(It.IsAny<Contactos>()), Times.Once);
        }

        [Test]
        public void Update_ConContactoExistente_DeberiaActualizarYLimpiarCache() {
            // Arrange
            var actualizada = new Contactos {
                Id = 1,
                Nombre = "Lucia",
                Alias = "Lulu",
                Telefono = 654357159,
                Email = "lulu@yahoo.com"
            };

            _repositoryMock.Setup(r => r.Update(1, It.IsAny<Contactos>()))
                .Returns((int id, Contactos p) => Result.Success<Contactos, DomainError>(p));

            // Act
            var resultado = _service.Update(1, actualizada);

            // Assert
            resultado.IsSuccess.Should().BeTrue();
            _cacheByIdMock.Verify(c => c.Remove(1), Times.Once);
            _repositoryMock.Verify(r => r.Update(1, It.IsAny<Contactos>()), Times.Once);
        }

        [Test]
        public void Delete_ConContactoExistente_DeberiaEliminarYLimpiarCache() {
            // Arrange
            _repositoryMock.Setup(r => r.GetById(1)).Returns(_listaValida[0]);
            _repositoryMock.Setup(r => r.Delete(1)).Returns(_listaValida[0]);

            // Act
            var resultado = _service.Delete(1);

            // Assert
            resultado.IsSuccess.Should().BeTrue();
            _cacheByIdMock.Verify(c => c.Remove(1), Times.Once);
            _repositoryMock.Verify(r => r.Delete(1), Times.Once);
        }

        [Test]
        public void CountContactos_DeberiaRetornarConteoCorrecto() {
            // Arrange
            _repositoryMock.Setup(r => r.CountContactos()).Returns(5);

            // Act
            var resultado = _service.CountContactos();

            // Assert
            resultado.Should().Be(5);
            _repositoryMock.Verify(r => r.CountContactos(), Times.Once);
        }
    }

    [TestFixture]
    public class CasosNegativos {
        [SetUp]
        public void SetUp() {
            _repositoryMock = new Mock<IRepository>();
            _cacheByIdMock = new Mock<ICache<int, Contactos>>();

            _service = new ContactosService(
                _repositoryMock.Object,
                _cacheByIdMock.Object
            );
            _listaValida = new List<Contactos> {
                new() {
                    Id = 1,
                    Nombre = "Antoine",
                    Alias = "Antu",
                    Telefono = 654357159,
                    Email = "antukiller@yahoo.com"
                },
                new() {
                    Id = 2,
                    Nombre = "Diego",
                    Alias = "PussyCat",
                    Telefono = 671493852,
                    Email = "diecgochu@yahoo.com"
                }
            };
        }

        private ContactosService _service = null!;
        private Mock<IRepository> _repositoryMock = null!;
        private Mock<ICache<int, Contactos>> _cacheByIdMock = null!;
        private List<Contactos> _listaValida = null!;

        [Test]
        public void GetById_ConContactoNoExistente_DeberiaRetornarErrorNotFound() {
            // Arrange
            _cacheByIdMock.Setup(c => c.Get(1)).Returns((Contactos?)null);
            _repositoryMock.Setup(r => r.GetById(1)).Returns((Contactos?)null);

            // Act
            var resultado = _service.GetById(1);

            // Assert
            resultado.IsFailure.Should().BeTrue();
            resultado.Error.Should().BeOfType<ContactoError.IdNotFound>();
            _cacheByIdMock.Verify(c => c.Get(1), Times.Once);
            _repositoryMock.Verify(r => r.GetById(1), Times.Once);
        }

        [Test]
        public void GetByAlias_ConContactoNoExistente_DeberiaRetornarErrorNotFound() {
            // Arrange
            _repositoryMock.Setup(r => r.GetByAlias("Antu")).Returns((Contactos?)null);

            // Act
            var resultado = _service.GetByAlias("Antu");

            // Assert
            resultado.IsFailure.Should().BeTrue();
            resultado.Error.Should().BeOfType<ContactoError.AliasNotFound>();
            _repositoryMock.Verify(r => r.GetByAlias("Antu"), Times.Once);
        }

        [Test]
        public void Create_ConTelefonoDuplicado_DeberiaRetornarErrorMatriculaAlreadyExists() {
            // Arrange
            _repositoryMock.Setup(r => r.Create(It.IsAny<Contactos>()))
                .Returns(Result.Failure<Contactos, DomainError>(
                    ContactoErrors.TelefonoAlreadyExists(654357159)));

            // Act
            var resultado = _service.Create(_listaValida[0]);

            // Assert
            resultado.IsFailure.Should().BeTrue();
            resultado.Error.Should().BeOfType<ContactoError.TelefonoAlreadyExists>();
            _repositoryMock.Verify(r => r.Create(It.IsAny<Contactos>()), Times.Once);
        }

        [Test]
        public void Update_ConContactoNoExistente_DeberiaRetornarErrorNotFound() {
            // Arrange
            _repositoryMock.Setup(r => r.Update(999, It.IsAny<Contactos>()))
                .Returns(Result.Failure<Contactos, DomainError>(ContactoErrors.IdNotFound(999)));

            // Act
            var resultado = _service.Update(999, new Contactos { Telefono = 056548445 });

            // Assert
            resultado.IsFailure.Should().BeTrue();
            resultado.Error.Should().BeOfType<ContactoError.IdNotFound>();
            resultado.Error.Message.Should().Contain("999");
            _cacheByIdMock.Verify(c => c.Remove(It.IsAny<int>()), Times.Never);
        }

        [Test]
        public void Delete_ConContactoNoExistente_DeberiaRetornarErrorNotFound() {
            // Arrange
            _repositoryMock.Setup(r => r.Delete(999))
                .Returns(Result.Failure<Contactos, DomainError>(ContactoErrors.IdNotFound(999)));

            // Act
            var resultado = _service.Delete(999);

            // Assert
            resultado.IsFailure.Should().BeTrue();
            resultado.Error.Should().BeOfType<ContactoError.IdNotFound>();
            resultado.Error.Message.Should().Contain("999");
            _repositoryMock.Verify(r => r.Delete(999), Times.Once);
            _cacheByIdMock.Verify(c => c.Remove(It.IsAny<int>()), Times.Never);
        }

        [Test]
        public void Update_ConTelefonoDuplicad_DeberiaRetornarError() {
            // Arrange
            var actualizada = new Contactos { Id = 1, Telefono = 654357159 };

            _repositoryMock.Setup(r => r.GetById(1)).Returns(_listaValida[0]);
            _repositoryMock.Setup(r => r.Update(1, It.IsAny<Contactos>()))
                .Returns(Result.Failure<Contactos, DomainError>(ContactoErrors.TelefonoAlreadyExists(654357159)));

            // Act
            var resultado = _service.Update(1, actualizada);

            // Assert
            resultado.IsFailure.Should().BeTrue();
            resultado.Error.Should().BeOfType<ContactoError.TelefonoAlreadyExists>();
        }
    }
}