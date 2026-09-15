namespace Agenda.Errors.Common;

/// <summary>
///     Base de TODOS los errores del dominio de la agenda.
/// </summary>
public abstract record DomainError(string Message, int Code);