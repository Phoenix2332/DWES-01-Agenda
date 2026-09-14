using Agenda.Cache;
using Agenda.Errors;
using Agenda.Errors.Common;
using Agenda.Model;
using Agenda.Repository.Common;
using CSharpFunctionalExtensions;

namespace Agenda.Service;

/// <summary>
///     Implementación del servicio de gestión de Contactos.
/// </summary>
public class ContactosService(
    IRepository repository,
    ICache<int, Contactos> cacheById,
    ICache<string, Contactos> cacheByAlias
) : IContactosService {
    /// <inheritdoc />
    public int TotalContactos => repository.GetAll(1, int.MaxValue).Count();

    /// <inheritdoc />
    public IEnumerable<Contactos> GetAll(int page = 1, int pageSize = 10) {
        return repository.GetAll(page, pageSize);
    }

    /// <inheritdoc />
    public Result<Contactos, DomainError> GetById(int id) {
        if (cacheById.Get(id) is { } cached)
            return Result.Success<Contactos, DomainError>(cached);

        if (repository.GetById(id) is not { } contacto)
            return Result.Failure<Contactos, DomainError>(ContactoErrors.NotFound(id.ToString()));

        cacheById.Add(id, contacto);
        return Result.Success<Contactos, DomainError>(contacto);
    }

    /// <inheritdoc />
    public Result<Contactos, DomainError> GetByAlias(string alias) {
        if (cacheByAlias.Get(alias) is { } cached)
            return Result.Success<Contactos, DomainError>(cached);

        if (repository.GetByAlias(alias) is not { } contacto)
            return Result.Failure<Contactos, DomainError>(ContactoErrors.NotFound(alias));

        cacheByAlias.Add(alias, contacto);
        return Result.Success<Contactos, DomainError>(contacto);
    }

    /// <inheritdoc />
    public Result<Contactos, DomainError> Create(Contactos contacto) {
        var resultado = repository.Create(contacto);

        if (resultado.IsSuccess) {
            cacheById.Add(resultado.Value.Id, resultado.Value);
            cacheByAlias.Add(resultado.Value.Alias, resultado.Value);
        }

        return resultado;
    }

    /// <inheritdoc />
    public Result<Contactos, DomainError> Update(int id, Contactos contacto) {
        var resultado = repository.Update(id, contacto);

        if (resultado.IsSuccess) {
            cacheById.Remove(id);
            cacheByAlias.Remove(resultado.Value.Alias);
        }

        return resultado;
    }

    /// <inheritdoc />
    public Result<Contactos, DomainError> Delete(int id) {
        var resultado = repository.Delete(id);

        if (resultado.IsSuccess) {
            cacheById.Remove(id);
            cacheByAlias.Remove(resultado.Value.Alias);
        }

        return resultado;
    }

    /// <inheritdoc />
    public int CountContactos() {
        return repository.CountContactos();
    }
}