using Agenda.Errors.Common;
using Agenda.Model;
using CSharpFunctionalExtensions;

namespace Agenda.Service;

/// <summary>
///     Define el contrato para la gestión de contactos en el sistema.
/// </summary>
public interface IContactosService {
    /// <summary>
    ///     Obtiene el número total de contactos registrados.
    /// </summary>
    /// <returns>Total de contactos.</returns>
    int TotalContactos { get; }

    /// <summary>
    ///     Obtiene todos los contactos de forma paginada.
    /// </summary>
    /// <param name="page">Número de página.</param>
    /// <param name="pageSize">Tamaño de página.</param>
    /// <returns>Enumerable de contactos.</returns>
    IEnumerable<Contactos> GetAll(int page = 1, int pageSize = 10);

    /// <summary>
    ///     Obtiene una contacto por su identificador único.
    /// </summary>
    /// <param name="id">Identificador del contacto.</param>
    /// <returns>Result con el contacto encontrado o error</returns>
    Result<Contactos, DomainError> GetById(int id);

    /// <summary>
    ///     Obtiene una contacto por el alias.
    /// </summary>
    /// <param name="alias">Alias.</param>
    /// <returns>Result con el contacto encontrado o error</returns>
    Result<Contactos, DomainError> GetByAlias(string alias);

    /// <summary>
    ///     Crea un nuevo contacto en el sistema.
    /// </summary>
    /// <param name="contacto">Datos del contacto a crear.</param>
    /// <returns>Result con el contacto creado o error</returns>
    Result<Contactos, DomainError> Create(Contactos contacto);

    /// <summary>
    ///     Actualiza un contacto existente.
    /// </summary>
    /// <param name="id">Identificador del contacto a actualizar.</param>
    /// <param name="contacto">Nuevos datos del contacto.</param>
    /// <returns>Result con el contacto actualizado o error</returns>
    Result<Contactos, DomainError> Update(int id, Contactos contacto);

    /// <summary>
    ///     Elimina una contacto del sistema.
    /// </summary>
    /// <param name="id">Identificador del contacto a eliminar.</param>
    /// <returns>Result con el contacto eliminado o error</returns>
    Result<Contactos, DomainError> Delete(int id);

    /// <summary>
    ///     Cuenta el número de contactos registrados.
    /// </summary>
    /// <returns>Total de contactos.</returns>
    int CountContactos();
}