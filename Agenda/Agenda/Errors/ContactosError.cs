using Agenda.Errors.Common;

namespace Agenda.Errors;

/// <summary>
///     Contenedor de errores específicos del dominio de Contactos.
/// </summary>
public abstract record ContactoError(string Message, int Code) : DomainError(Message, Code) {
    /// <summary>
    ///     Error: contacto no encontrado por ID. → 404
    /// </summary>
    public sealed record IdNotFound(int Id)
        : ContactoError($"No se encontró el contacto con ID {Id}", 404);

    /// <summary>
    ///     Error: contacto no encontrado por ID. → 404
    /// </summary>
    public sealed record AliasNotFound(string Alias)
        : ContactoError($"No se encontró el contacto con Alias {Alias}", 404);

    /// <summary>
    ///     Error: ya existe un contacto con ese alias. → 409
    /// </summary>
    public sealed record AliasAlreadyExists(string Alias)
        : ContactoError($"Ya existe un contacto con el alias {Alias}.", 409);

    /// <summary>
    ///     Error: el teléfono ya está registrado en otro contacto. → 409
    /// </summary>
    public sealed record TelefonoAlreadyExists(int Telefono)
        : ContactoError($"Ya existe un contacto con el teléfono {Telefono}.", 409);

    /// <summary>
    ///     Error de base de datos. → 500
    /// </summary>
    public sealed record Database(string Details)
        : ContactoError($"Error de base de datos: {Details}", 500);
}

/// <summary>
///     Factory para crear errores de dominio de Contacto.
/// </summary>
public static class ContactoErrors {
    /// <summary>
    ///     Crea un error de contacto no encontrado.
    /// </summary>
    public static DomainError IdNotFound(int id) {
        return new ContactoError.IdNotFound(id);
    }

    /// <summary>
    ///     Crea un error de contacto no encontrado.
    /// </summary>
    public static DomainError AliasNotFound(string alias) {
        return new ContactoError.AliasNotFound(alias);
    }

    /// <summary>
    ///     Crea un error de alias duplicado.
    /// </summary>
    public static DomainError AliasAlreadyExists(string alias) {
        return new ContactoError.AliasAlreadyExists(alias);
    }

    /// <summary>
    ///     Crea un error de teléfono duplicado.
    /// </summary>
    public static DomainError TelefonoAlreadyExists(int telefono) {
        return new ContactoError.TelefonoAlreadyExists(telefono);
    }

    /// <summary>
    ///     Crea un error de la base de datos.
    /// </summary>
    public static DomainError DatabaseError(string details) {
        return new ContactoError.Database(details);
    }
}