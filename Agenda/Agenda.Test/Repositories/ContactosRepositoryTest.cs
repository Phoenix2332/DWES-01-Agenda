using Agenda.Entity;
using Agenda.Errors;
using Agenda.Model;
using Agenda.Repository;
using FluentAssertions;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace Agenda.Test.Repositories;

[TestFixture]
public class ContactosRepositoryTest {
    [TestFixture]
    public class CasosPositivos {
        [SetUp]
        public void SetUp() {
            _connection = new SqliteConnection("Data Source=:memory:");
            _connection.Open();

            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseSqlite(_connection)
                .Options;
            _context = new AppDbContext(options);
            _context.Database.EnsureCreated();
            _repository = new ContactosRepository(_context);
        }

        [TearDown]
        public void TearDown() {
            _context.Database.EnsureDeleted();
            _context.Dispose();
            _connection.Close();
            _connection.Dispose();
        }

        private SqliteConnection _connection = null!;
        private AppDbContext _context = null!;
        private ContactosRepository _repository = null!;

        [Test]
        public void Create_ContactoValido_DeberiaCrearCorrectamente() {
            // Arrange
            var contacto = new Contactos {
                Id = 1,
                Nombre = "Antoine",
                Alias = "Antu",
                Telefono = 654357159,
                Email = "antukiller@yahoo.com"
            };

            // Act
            var resultado = _repository.Create(contacto);

            // Assert
            resultado.IsSuccess.Should().BeTrue();
            resultado.Value.Id.Should().Be(1);
            resultado.Value.Nombre.Should().Be("Antoine");
        }

        [Test]
        public void GetById_CuandoExiste_DeberiaRetornarCita() {
            // Arrange
            var contacto = new Contactos {
                Id = 1,
                Nombre = "Antoine",
                Alias = "Antu",
                Telefono = 654357159,
                Email = "antukiller@yahoo.com"
            };

            // Arrange
            _repository.Create(contacto);

            // Act
            var resultado = _repository.GetById(1);

            // Assert
            resultado.Should().NotBeNull();
            resultado.Id.Should().Be(1);
            resultado.Nombre.Should().Be("Antoine");
        }

        [Test]
        public void GetByAlias_CuandoExiste_DeberiaRetornarCita() {
            // Arrange
            var contacto = new Contactos {
                Id = 1,
                Nombre = "Antoine",
                Alias = "Antu",
                Telefono = 654357159,
                Email = "antukiller@yahoo.com"
            };

            // Arrange
            _repository.Create(contacto);

            // Act
            var resultado = _repository.GetByAlias("Antu");

            // Assert
            resultado.Should().NotBeNull();
            resultado.Nombre.Should().Be("Antoine");
        }

        [Test]
        public void GetAll_ConPaginacion_DeberiaRetornarPagina() {
            // Arrange
            for (var i = 1; i <= 5; i++)
                _repository.Create(new Contactos {
                    Id = 1,
                    Nombre = "Antoine",
                    Alias = "Antu",
                    Telefono = 654357150 + i,
                    Email = "antukiller@yahoo.com"
                });

            // Act
            var resultado = _repository.GetAll(1, 3).ToList();

            // Assert
            resultado.Should().HaveCount(3);
        }

        [Test]
        public void Update_ConDatosValidos_DeberiaActualizar() {
            // Arrange
            var contacto = new Contactos {
                Id = 1,
                Nombre = "Antoine",
                Alias = "Antu",
                Telefono = 654357159,
                Email = "antukiller@yahoo.com"
            };
            _repository.Create(contacto);

            var actualizado = new Contactos {
                Id = 1,
                Nombre = "Diego",
                Alias = "PussyCat",
                Telefono = 671493852,
                Email = "diecgochu@yahoo.com"
            };

            // Act
            var resultado = _repository.Update(1, actualizado);

            // Assert
            resultado.IsSuccess.Should().BeTrue();
            resultado.Value.Nombre.Should().Be("Diego");
            resultado.Value.Alias.Should().Be("PussyCat");
        }

        [Test]
        public void Delete_CuandoExiste_DeberiaRetornarCita() {
            // Arrange
            var contacto = new Contactos {
                Id = 1,
                Nombre = "Antoine",
                Alias = "Antu",
                Telefono = 654357159,
                Email = "antukiller@yahoo.com"
            };
            _repository.Create(contacto);

            // Act
            var resultado = _repository.Delete(1);

            // Assert
            resultado.Should().NotBeNull();
            _repository.GetById(1).Should().BeNull();
        }

        [Test]
        public void CountContactos_DeberiaContarTodo() {
            // Arrange
            var contacto1 = new Contactos {
                Id = 1,
                Nombre = "Antoine",
                Alias = "Antu",
                Telefono = 654357159,
                Email = "antukiller@yahoo.com"
            };

            var contacto2 = new Contactos {
                Id = 2,
                Nombre = "Diego",
                Alias = "PussyCat",
                Telefono = 671493852,
                Email = "diecgochu@yahoo.com"
            };
            _repository.Create(contacto1);
            var contacto = _repository.Create(contacto2).Value;
            _repository.Delete(contacto.Id);

            // Act
            var resultado = _repository.CountContactos();

            // Assert
            resultado.Should().Be(1);
        }
    }

    [TestFixture]
    public class CasosNegativos {
        [SetUp]
        public void SetUp() {
            _connection = new SqliteConnection("Data Source=:memory:");
            _connection.Open();

            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseSqlite(_connection)
                .Options;
            _context = new AppDbContext(options);
            _context.Database.EnsureCreated();
            _repository = new ContactosRepository(_context);
            _contacto = new Contactos {
                Id = 1,
                Nombre = "Antoine",
                Alias = "Antu",
                Telefono = 654357159,
                Email = "antukiller@yahoo.com"
            };
        }

        [TearDown]
        public void TearDown() {
            _context.Database.EnsureDeleted();
            _context.Dispose();
            _connection.Close();
            _connection.Dispose();
        }

        private SqliteConnection _connection = null!;
        private AppDbContext _context = null!;
        private ContactosRepository _repository = null!;
        private Contactos _contacto = null!;

        [Test]
        public void Create_ConTelefonoExistente_DeberiaRetornarFailure() {
            // Arrange
            var contacto1 = _contacto;
            var contacto2 = new Contactos {
                Id = 1,
                Nombre = "Diego",
                Alias = "PussyCat",
                Telefono = 654357159,
                Email = "diecgochu@yahoo.com"
            };
            _repository.Create(contacto1);

            // Act
            var resultado = _repository.Create(contacto2);

            // Assert
            resultado.IsFailure.Should().BeTrue();
            resultado.Error.Should().BeOfType<ContactoError.TelefonoAlreadyExists>();
            (resultado.Error as ContactoError.TelefonoAlreadyExists)?.Telefono.Should().Be(654357159);
        }

        [Test]
        public void GetById_CuandoNoExiste_DeberiaRetornarNull() {
            // Act
            var resultado = _repository.GetById(999);

            // Assert
            resultado.Should().BeNull();
        }

        [Test]
        public void GetByAlias_CuandoNoExiste_DeberiaRetornarNull() {
            // Act
            var resultado = _repository.GetByAlias("Lulu");

            // Assert
            resultado.Should().BeNull();
        }

        [Test]
        public void Update_CuandoNoExiste_DeberiaRetornarFailure() {
            // Act
            var resultado = _repository.Update(999, _contacto);

            // Assert
            resultado.IsFailure.Should().BeTrue();
            resultado.Error.Should().BeOfType<ContactoError.IdNotFound>();
            (resultado.Error as ContactoError.IdNotFound)?.Id.Should().Be(999);
        }

        [Test]
        public void Update_ConTelefonoExistenteEnOtro_DeberiaRetornarFailure() {
            // Arrange
            var contacto2 = new Contactos {
                Id = 2,
                Nombre = "Diego",
                Alias = "PussyCat",
                Telefono = 671493852,
                Email = "diecgochu@yahoo.com"
            };
            _repository.Create(_contacto);
            _repository.Create(contacto2);

            // Act
            var resultado = _repository.Update(2, _contacto);

            // Assert
            resultado.IsFailure.Should().BeTrue();
            resultado.Error.Should().BeOfType<ContactoError.TelefonoAlreadyExists>();
            (resultado.Error as ContactoError.TelefonoAlreadyExists)?.Telefono.Should().Be(654357159);
        }

        [Test]
        public void Delete_CuandoNoExiste_DeberiaRetornarNull() {
            // Act
            var resultado = _repository.Delete(999);

            // Assert
            resultado.Error.Should().BeOfType<ContactoError.IdNotFound>();
        }
    }
}