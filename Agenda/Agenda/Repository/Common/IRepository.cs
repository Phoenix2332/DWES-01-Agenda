using Agenda.Errors.Common;
using Agenda.Model;
using CSharpFunctionalExtensions;

namespace Agenda.Repository.Common;

/// <summary>
///     Define las operaciones de búsqueda, persistencia y validación de identidad.
/// </summary>
public interface IRepository {
    /// <summary>
    ///     Obtiene todos los contactos de forma paginada.
    /// </summary>
    IEnumerable<Contactos> GetAll(int page = 1, int pageSize = 10);

    /// <summary>
    ///     Obtiene una contacto por su ID.
    /// </summary>
    Contactos? GetById(int id);

    /// <summary>
    ///     Realiza una búsqueda por la matrícula del vehículo.
    /// </summary>
    Contactos? GetByAlias(string alias);

    /// <summary>
    ///     Crea un nuevo contacto en el sistema.
    /// </summary>
    /// <returns>Result con el contacto creado o error de dominio.</returns>
    Result<Contactos, DomainError> Create(Contactos contacto);

    /// <summary>
    ///     Actualiza un contacto existente.
    /// </summary>
    /// <returns>Result con el contacto actualizado o error de dominio.</returns>
    Result<Contactos, DomainError> Update(int id, Contactos contacto);

    /// <summary>
    ///     Elimina un contacto.
    /// </summary>
    Result<Contactos, DomainError> Delete(int id);

    /// <summary>
    ///     Obtiene el número total de contactos.
    /// </summary>
    int CountContactos();
}