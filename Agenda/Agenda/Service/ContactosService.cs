using Agenda.Cache;
using Agenda.Errors;
using Agenda.Errors.Common;
using Agenda.Model;
using Agenda.Repository.Common;
using CSharpFunctionalExtensions;
using Serilog;

namespace Agenda.Service;

/// <summary>
///     Implementación del servicio de gestión de Contactos.
/// </summary>
public class ContactosService(
    IRepository repository,
    ICache<int, Contactos> cacheById
) : IContactosService {
    private readonly ILogger _logger = Log.ForContext<ContactosService>();

    /// <inheritdoc />
    public int TotalContactos => repository.GetAll(1, int.MaxValue).Count();


    /// <inheritdoc />
    public IEnumerable<Contactos> GetAll(int page = 1, int pageSize = 10) {
        _logger.Debug("[SERV-GET] Obteniendo contactos: página {Page}, tamaño {PageSize}", page, pageSize);
        return repository.GetAll(page, pageSize);
    }

    /// <inheritdoc />
    public Result<Contactos, DomainError> GetById(int id) {
        _logger.Debug("[SERV-GET] Obteniendo contacto con ID {Id}", id);
        if (cacheById.Get(id) is { } cached)
            return Result.Success<Contactos, DomainError>(cached);

        if (repository.GetById(id) is not { } contacto)
            return Result.Failure<Contactos, DomainError>(ContactoErrors.IdNotFound(id));

        cacheById.Add(id, contacto);
        return Result.Success<Contactos, DomainError>(contacto);
    }

    /// <inheritdoc />
    public Result<Contactos, DomainError> GetByAlias(string alias) {
        _logger.Debug("[SERV-GET] Obteniendo contacto con Alias {Alias}", alias);
        if (repository.GetByAlias(alias) is not { } contacto)
            return Result.Failure<Contactos, DomainError>(ContactoErrors.AliasNotFound(alias));
        return Result.Success<Contactos, DomainError>(contacto);
    }

    /// <inheritdoc />
    public Result<Contactos, DomainError> Create(Contactos contacto) {
        _logger.Debug("[SERV-ADD] Creando nuevo contacto {Telefono}", contacto.Telefono);
        var resultado = repository.Create(contacto);

        if (resultado.IsSuccess) cacheById.Add(resultado.Value.Id, resultado.Value);

        return resultado;
    }

    /// <inheritdoc />
    public Result<Contactos, DomainError> Update(int id, Contactos contacto) {
        _logger.Debug("[SERV-ADD] Actualizando contacto con ID {Id}", id);
        var resultado = repository.Update(id, contacto);

        if (resultado.IsSuccess) cacheById.Remove(id);

        return resultado;
    }

    /// <inheritdoc />
    public Result<Contactos, DomainError> Delete(int id) {
        _logger.Debug("[SERV-REMOVE] Eliminando contacto con ID {Id}", id);
        var resultado = repository.Delete(id);

        if (resultado.IsSuccess) cacheById.Remove(id);

        return resultado;
    }

    /// <inheritdoc />
    public int CountContactos() {
        _logger.Debug("[SERV-GET] Contando contactos");
        return repository.CountContactos();
    }
}