namespace Agenda.Enum;

/// <summary>
///     Códigos de estado HTTP "devueltos" por el servicio.
/// </summary>
public enum HttpCodigos {
    Ok = 200,
    Created = 201,
    BadRequest = 400,
    NotFound = 404,
    Conflict = 409,
    InternalServerError = 500
}