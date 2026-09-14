namespace Agenda.Models;

/// <summary>
///     Representa un contacto en el sistema
/// </summary>
public record Contactos {
    public int Id { get; init; }
    public string Nombre { get; init; } = string.Empty;
    public string Alias { get; init; } = string.Empty;
    public int Telefono { get; init; }
    public string Email { get; init; } = string.Empty;
    public DateTime CreateAt { get; init; } = DateTime.UtcNow;
    public DateTime UpdateAt { get; init; } = DateTime.UtcNow;
    
    /// <summary>
    ///     Determina si dos contactos son idénticos comparando los teléfonos.
    /// </summary>
    /// <param name="other">Instancia de contacto a comparar.</param>
    /// <returns>True si los teléfonos coinciden.</returns>
    public virtual bool Equals(Contactos? other) {
        return other is not null && Telefono == other.Telefono;
    }

    public override string ToString() {
        return $"Contacto: [Id-{Id}, Nombre-{Nombre}, Alias-{Alias}, Teléfono-{Telefono}, Email-{Email}, " +
               $"Fecha de creacion-{CreateAt}, Fecha de última modificacion-{UpdateAt}]";
    }

    /// <summary>
    ///     Calcula el código hash basado exclusivamente en el teléfono para mantener coherencia con la igualdad.
    /// </summary>
    /// <returns>Código hash entero.</returns>
    public override int GetHashCode() {
        return HashCode.Combine(Telefono);
    }
}