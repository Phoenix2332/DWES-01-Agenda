using Agenda.Entity;
using Agenda.Models;

namespace Agenda.Mapper;

public static class ContactosMapper {
    public static Contactos? ToModel(this ContactosEntity? entity) {
        if (entity == null) return null;

        return new Contactos {
            Id = entity.Id,
            Nombre = entity.Nombre,
            Alias = entity.Alias,
            Telefono = entity.Telefono,
            Email = entity.Email,
            CreateAt = entity.CreateAt,
            UpdateAt = entity.UpdateAt
        };
    }

    public static IEnumerable<Contactos> ToModel(this IEnumerable<ContactosEntity> entities) {
        return entities.Select(ToModel).OfType<Contactos>();
    }

    public static ContactosEntity ToEntity(this Contactos model) {
        return new ContactosEntity {
            Id = model.Id,
            Nombre = model.Nombre,
            Alias = model.Alias,
            Telefono = model.Telefono,
            Email = model.Email,
            CreateAt = model.CreateAt,
            UpdateAt = model.UpdateAt
        };
    }
}