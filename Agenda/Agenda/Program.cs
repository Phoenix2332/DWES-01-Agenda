using Agenda.Cache;
using Agenda.Entity;
using Agenda.Errors.Common;
using Agenda.Model;
using Agenda.Repository;
using Agenda.Service;
using CSharpFunctionalExtensions;
using Serilog;
using static System.Console;

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Debug()
    .WriteTo.Console(
        outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}")
    .CreateLogger();

Main();

WriteLine("👋 Presiona una tecla para salir...");
ReadKey();
return;

void Main() {
    var db = new AppDbContext("Data Source=agenda.db");
    db.EnsureCreated();
    var cacheId = new Cache<int, Contactos>(5);
    IContactosService service = new ContactosService(new ContactosRepository(db), cacheId);

    WriteLine();
    WriteLine("== POST /contactos ==");
    Mostrar("Crear", 201, service.Create(new Contactos {
        Nombre = "Antoine", Alias = "Antu", Telefono = 654357159, Email = "antukiller@yahoo.com"
    }));
    Mostrar("Crear", 201, service.Create(new Contactos {
        Nombre = "Lucía", Alias = "Lulu", Telefono = 654753951, Email = "lulu@gmail.com"
    }));
    Mostrar("Crear", 201, service.Create(new Contactos {
        Nombre = "Laura", Alias = "Lau", Telefono = 671493852, Email = "laurasm@hotmail.com"
    }));
    // Error 409
    Mostrar("Crear", 201, service.Create(new Contactos {
        Nombre = "Shapesifter", Alias = "Metamorfo", Telefono = 654357159, Email = "shape@gmail.com"
    }));

    WriteLine();
    WriteLine("== GET /contactos/{id} ==");
    Mostrar("Obtener", 200, service.GetById(1));
    Mostrar("Obtener", 200, service.GetById(1));
    // Error 404
    Mostrar("Obtener", 200, service.GetById(999));

    WriteLine();
    WriteLine("== GET /contactos/alias/{alias} ==");
    Mostrar("Obtener", 200, service.GetByAlias("Antu"));
    // Error 404
    Mostrar("Obtener", 200, service.GetByAlias("PussyCat"));

    WriteLine();
    WriteLine("== GET /contactos ==");
    foreach (var c in service.GetAll())
        WriteLine($"  {c.Id} | {c.Nombre} | {c.Alias} | {c.Telefono}");
    WriteLine($"(Nº Contactos: {service.TotalContactos})");

    WriteLine();
    WriteLine("== PUT /contactos/{id} ==");
    Mostrar("Actualizar", 200, service.Update(2, new Contactos {
        Nombre = "Adrián", Alias = "Adri", Telefono = 654753951, Email = "adrikael@gmail.com"
    }));
    // Error 409
    Mostrar("Actualizar", 200, service.Update(3, new Contactos {
        Nombre = "Laura", Alias = "Lau", Telefono = 654357159, Email = "laurasm@mail.com"
    }));

    WriteLine();
    WriteLine("== DELETE /contactos/{id} ==");
    Mostrar("Eliminar", 200, service.Delete(3));
    // Error 404
    Mostrar("Eliminar", 200, service.Delete(999));
}

void Mostrar(string accion, int code, Result<Contactos, DomainError> resultado) {
    WriteLine();
    WriteLine(resultado.IsSuccess
        ? $"[{Verbo(accion)} {code} {CodeNombre(code)}] {resultado.Value.Id} | {resultado.Value.Nombre} | {resultado.Value.Alias} | {resultado.Value.Telefono}"
        : $"[{Verbo(accion)} {resultado.Error.Code} {CodeNombre(code)}] {resultado.Error.Message}");
    WriteLine();
}

string Verbo(string accion) {
    return accion switch {
        "Crear" => "POST",
        "Obtener" => "GET",
        "Actualizar" => "PUT",
        "Eliminar" => "DELETE",
        _ => string.Empty
    };
}

string CodeNombre(int code) {
    return code switch {
        200 => "Ok",
        201 => "Created",
        400 => "BadRequest",
        404 => "NotFound",
        409 => "Conflict",
        500 => "InternalServerError",
        _ => string.Empty
    };
}