# MichelPage Technical Test — Backend

Proyecto backend desarrollado en **.NET 9** (API REST), encargado de proveer los endpoints de autenticación y gestión de tareas para la prueba técnica.

---

## Requisitos Previos

Para ejecutar y probar este proyecto necesitarás:

- **.NET 9 SDK** o superior instalado en tu máquina.
- **SQL Server**  para la base de datos .
- IDE recomendado: Visual Studio 2022 o Visual Studio Code.

---

## Configuración de la Base de Datos

El proyecto se comunica con SQL Server usando  Dapper . Como parte de la prueba técnica, se adjunta el modelo de base de datos completo.

### 1. Ejecutar el script SQL
En la carpeta raíz principal (`d:\Prueba Tecnica User Task\`), encontrarás el archivo **`Script Sql Server.sql`**. Necesitas:
1. Crear una base de datos nueva en tu servidor SQL (por ejemplo: `DbRenzo` o `TaskSystemDB`).
2. Abrir y ejecutar todo el contenido de **`Script Sql Server.sql`** dentro de la base de datos nueva.
   - *Este script creará las tablas `Users` y `Tasks`, sus llaves foráneas, restricciones para los estados (`Pending`, `InProgress`, `Done`), e insertará algunos datos de prueba iniciales para los logins (`juan@example.com` / `123456`).*

### 2. Configurar la cadena de conexión
Una vez que tu base de datos esté creada y tenga las tablas, necesitas apuntar el proyecto a tu entorno local.

En el archivo base **`appsettings.json`** o **`appsettings.Development.json`**, modifica la sección de `ConnectionStrings`:

```json
  "ConnectionStrings": {
    "defaultConnection": "Data Source=TU-SERVIDOR;Initial Catalog=DbRenzo;Integrated Security=true;TrustServerCertificate=True"
  }
```

- Cambia `TU-SERVIDOR` por la instancia local de tu SQL Server (ej. `.\SQLEXPRESS` o `localhost`).

---

## Instalación y Ejecución

Restaura las dependencias y ejecuta el proyecto con los siguientes comandos desde tu terminal (en la carpeta `MichelPage-TechnicalTest-Back`):

```bash
# 1. Limpiar y restaurar paquetes NuGet
dotnet clean
dotnet restore

# 2. Compilar el proyecto
dotnet build

# 3. Levantar la API
dotnet run
```

La aplicación publicará por defecto en los puertos configurados (usualmente `https://localhost:7014/api/`). Si el puerto cambia, recuerda actualizar el archivo global `environment.ts` en el proyecto Frontend.

---

## Endpoints Principales

- **`POST /api/Users/Login`**: Autenticación de usuario.
- **`POST /api/Users/CreateUser`**: Registro de un nuevo usuario en la BD.
- **`GET /api/Tasks/TaskList`**: Obtención de tareas activas (con Inner Join a `Users` y lectura de JSON `Informacion`).
- **`POST /api/Tasks/CreateTask`**: Inserción de tareas guardando atributos extra (`prioridad`, `fechaEstimada`) en un campo JSON validado desde SQL.
- **`PUT /api/Tasks/UpdateStatus`**: Modificación de estado estricto (Pending, InProgress, Done) auditable con id de usuario mod.
- **`DELETE /api/Tasks/DeleteTask/{id}?usuarioMod={userId}`**: Eliminación lógica de una tarea.

