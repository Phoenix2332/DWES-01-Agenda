using Agenda.Entity;
using Agenda.Errors;
using Agenda.Errors.Common;
using Agenda.Mapper;
using Agenda.Model;
using Agenda.Repository.Common;
using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace Agenda.Repository;

public class ContactosRepository(AppDbContext context) : IRepository {
    private readonly ILogger _logger = Log.ForContext<ContactosRepository>();

    /// <inheritdoc />
    public IEnumerable<Contactos> GetAll(int page = 1, int pageSize = 10) {
        _logger.Debug("[REPO-GET] Obteniendo contactos: página {Page}, tamaño {PageSize}", page, pageSize);

        try {
            var query = context.Contacto.AsNoTracking();

            var entities = query
                .OrderBy(p => p.Id)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            return entities.ToModel();
        }
        catch (Exception ex) {
            _logger.Error(ex, "Error al obtener contactos");
            return [];
        }
    }

    /// <inheritdoc />
    public Contactos? GetById(int id) {
        _logger.Debug("[REPO-GET] Obteniendo contacto con ID {Id}", id);
        try {
            var entity = context.Contacto.AsNoTracking().FirstOrDefault(p => p.Id == id);
            return entity.ToModel();
        }
        catch (Exception ex) {
            _logger.Error(ex, "Error al obtener contacto por ID {Id}", id);
            return null;
        }
    }

    /// <inheritdoc />
    public Contactos? GetByAlias(string alias) {
        _logger.Debug("[REPO-GET] Obteniendo contacto con Alias {Alias}", alias);
        try {
            var entity = context.Contacto.AsNoTracking().FirstOrDefault(p => p.Alias == alias);
            return entity.ToModel();
        }
        catch (Exception ex) {
            _logger.Error(ex, "Error al obtener contacto con Alias {Alias}", alias);
            return null;
        }
    }

    /// <inheritdoc />
    public Result<Contactos, DomainError> Create(Contactos contacto) {
        _logger.Debug("[REPO-ADD] Creando nuevo contacto {Telefono}", contacto.Telefono);
        if (ExisteTelefono(contacto.Telefono)) {
            _logger.Warning("[REPO-ADD] No se puede crear: Telefono {Telefono} ya existe", contacto.Telefono);
            return Result.Failure<Contactos, DomainError>(
                ContactoErrors.TelefonoAlreadyExists(contacto.Telefono));
        }

        contacto = contacto with {
            Id = 0,
            CreateAt = DateTime.UtcNow,
            UpdateAt = DateTime.UtcNow
        };

        try {
            var entity = contacto.ToEntity();
            context.Contacto.Add(entity);
            context.SaveChanges();
            _logger.Information("[REPO-ADD] Contacto creado con ID {Id}", GetById(entity.Id)!);
            return Result.Success<Contactos, DomainError>(GetById(entity.Id)!);
        }
        catch (Exception ex) {
            _logger.Error(ex, "Error al crear nuevo contacto");
            return Result.Failure<Contactos, DomainError>(ContactoErrors.DatabaseError(ex.Message));
        }
    }

    /// <inheritdoc />
    public Result<Contactos, DomainError> Update(int id, Contactos contacto) {
        _logger.Debug("[REPO-ADD] Actualizando contacto con ID {Id}", id);
        var contactoAntiguoEntity = context.Contacto.FirstOrDefault(p => p.Id == id);
        if (contactoAntiguoEntity == null)
            return Result.Failure<Contactos, DomainError>(ContactoErrors.IdNotFound(id));

        var contactoAntiguo = contactoAntiguoEntity.ToModel();
        if (contactoAntiguo == null)
            return Result.Failure<Contactos, DomainError>(ContactoErrors.IdNotFound(id));

        if (contacto.Telefono != contactoAntiguo.Telefono && ExisteTelefono(contacto.Telefono)) {
            _logger.Warning("[REPO-ADD] No se puede crear: Telefono {Telefono} ya existe", contacto.Telefono);
            return Result.Failure<Contactos, DomainError>(
                ContactoErrors.TelefonoAlreadyExists(contacto.Telefono));
        }

        contactoAntiguoEntity.Nombre = contacto.Nombre;
        contactoAntiguoEntity.Alias = contacto.Alias;
        contactoAntiguoEntity.Telefono = contacto.Telefono;
        contactoAntiguoEntity.Email = contacto.Email;
        contactoAntiguoEntity.UpdateAt = contacto.UpdateAt;

        try {
            context.SaveChanges();
            return Result.Success<Contactos, DomainError>(GetById(id)!);
        }
        catch (Exception ex) {
            _logger.Error(ex, "Error al actualizar el contacto");
            return Result.Failure<Contactos, DomainError>(ContactoErrors.DatabaseError(ex.Message));
        }
    }

    /// <inheritdoc />
    public Result<Contactos, DomainError> Delete(int id) {
        _logger.Debug("[REPO-REMOVE] Eliminando contacto con ID {Id}", id);
        try {
            var actual = context.Contacto.FirstOrDefault(p => p.Id == id);
            if (actual == null)
                return Result.Failure<Contactos, DomainError>(ContactoErrors.IdNotFound(id));

            context.Contacto.Remove(actual);
            context.SaveChanges();
            return Result.Success<Contactos, DomainError>(actual.ToModel()!);
        }
        catch (Exception ex) {
            _logger.Error(ex, "Error al eliminar contacto");
            return Result.Failure<Contactos, DomainError>(ContactoErrors.DatabaseError(ex.Message));
        }
    }

    /// <inheritdoc />
    public int CountContactos() {
        _logger.Debug("[REPO-GET] Contando contactos");
        try {
            var query = context.Contacto;
            return query.Count();
        }
        catch (Exception ex) {
            _logger.Error(ex, "Error al contar contactos");
            return 0;
        }
    }

    private bool ExisteTelefono(int telefono) {
        return context.Contacto.Any(c => c.Telefono == telefono);
    }
}