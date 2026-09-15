# Agenda de Contactos DWES
## 1. Introducción
Se requiere una agenda de contactos testeada y con los requisitos declarados en las siguientes secciones.

---

## 2. Requisitos Funcionales
### 2.1 Gestión de Contactos
Cada contacto deberá incluir obligatoriamente la siguiente información:
- Nombre
- Alias/Mote
- Tlf.
- Email

### 2.2 Sistema de Búsqueda
El sistema deberá permitir:
- Búsqueda general paginada
- Búsqueda por ID
- Búsqueda por alias/mote

---

## 3. Requisitos Técnicos
### 3.1 Persistencia de Datos
Sistema de almacenamiento Persistente:
- Entity Framework Core
    Justificación
    - Mayor productividad: CRUD y consultas se realizan con LINQ y entidades tipadas.
    - Mantenimiento más sencillo: los cambios en el modelo se gestionan mediante migraciones.
    - Seguridad: las consultas parametrizadas reducen el riesgo de inyección SQL.
    - Independencia del motor de BD: el mismo código puede funcionar con SQL Server, SQLite, PostgreSQL, etc., cambiando el proveedor.
    - Integración nativa con .NET: encaja con la inyección de dependencias, configuración y herramientas del ecosistema.

Sistema de almacenamiento RAM:
- Cache LRU (Last Recently Used)
    Justificación:
    - Mejora el rendimiento: evita repetir consultas costosas (base de datos, API o cálculos) reutilizando datos recientes.
    - Uso eficiente de memoria: al tener un tamaño máximo, la caché no crece indefinidamente.
    - Aprovecha la localidad temporal: en muchas aplicaciones los datos consultados recientemente tienen más probabilidades de volver a usarse.
    - Implementación sencilla y predecible: ofrece un equilibrio muy bueno entre complejidad y eficacia.

### 3.2 Gestión de Datos y Archivos
Sistema de logging:
- Consola

Sistema de borrado configurable:
- Borrado físico (DELETE)

### 3.3 Interfaz de Usuario
El programa no contará con UI.

---

## 4. Calidad del Software
### 4.1 Control de Versiones
Uso obligatorio de Git con GitFlow

Historial de commits evaluable

### 4.2 Documentación (UML)
El proyecto deberá incluir:
- Descripción del problema
- Requisitos funcionales y no funcionales
- Requisitos de información
- Justificación del uso de tecnologías

### 4.3 Testing
Tests unitarios obligatorios para:
- Lógica de negocio
- Acceso a datos