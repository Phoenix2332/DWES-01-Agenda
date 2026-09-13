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
Sistema de almacenamiento:
- Entity Framework Core

### 3.2 Gestión de Datos y Archivos
Sistema de logging:
- Consola y fichero

Sistema de borrado configurable:
- Borrado físico (DELETE)
- Borrado lógico (flag)

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