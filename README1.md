# Mesa API - Sistema de Gestión de Solicitudes

Una API REST moderna desarrollada en .NET 8 para la gestión de solicitudes y mesa de ayuda, basada en el sistema PHP existente.

## 🏗️ Arquitectura

La API está construida siguiendo los principios de **Clean Architecture** con las siguientes capas:

- **Domain**: Entidades, interfaces y lógica de negocio
- **Application**: Casos de uso, comandos, consultas y DTOs
- **Infrastructure**: Implementación de repositorios, servicios externos y acceso a datos
- **API**: Controladores, middleware y configuración

## 🚀 Características

### Patrones y Tecnologías Implementadas

- ✅ **Clean Architecture** - Separación clara de responsabilidades
- ✅ **CQRS** con **MediatR** - Separación de comandos y consultas
- ✅ **Repository Pattern** - Abstracción del acceso a datos
- ✅ **Unit of Work** - Gestión de transacciones
- ✅ **JWT Authentication** - Autenticación segura
- ✅ **AutoMapper** - Mapeo automático de objetos
- ✅ **FluentValidation** - Validación robusta
- ✅ **Entity Framework Core** - ORM moderno
- ✅ **Serilog** - Logging estructurado
- ✅ **Swagger/OpenAPI** - Documentación automática
- ✅ **Soft Delete** - Eliminación lógica
- ✅ **Global Exception Handling** - Manejo centralizado de errores

### Funcionalidades Principales

- 🔐 **Autenticación JWT** con refresh tokens
- 👥 **Gestión de Usuarios** (CRUD completo)
- 📋 **Sistema de Solicitudes** con estados y prioridades
- 📝 **Comentarios** en solicitudes
- 📎 **Adjuntos** de archivos
- 🔄 **Flujo de Pasos** configurables
- 👔 **Roles y Permisos**
- 📊 **Auditoría** completa (creación, modificación, eliminación)

## 🛠️ Configuración

### Prerrequisitos

- .NET 8 SDK
- SQL Server (compatible con la BD existente)
- Visual Studio 2022 o VS Code

### Instalación

1. **Clonar y navegar al proyecto**
```bash
cd mesaapi
```

2. **Restaurar paquetes NuGet**
```bash
dotnet restore
```

3. **Configurar la base de datos**
   - Actualizar `appsettings.json` con tu cadena de conexión
   - La API está configurada para usar la misma BD que el sistema PHP existente

4. **Ejecutar migraciones** (cuando estén listas)
```bash
dotnet ef database update --project src/MesaApi.Infrastructure --startup-project src/MesaApi.Api
```

5. **Ejecutar la aplicación**
```bash
dotnet run --project src/MesaApi.Api
```

La API estará disponible en:
- **HTTPS**: `https://localhost:7001`
- **HTTP**: `http://localhost:5001`
- **Swagger UI**: `https://localhost:7001` (raíz)

## 📚 Uso de la API

### Autenticación

```bash
# Login
POST /api/auth/login
{
  "login": "usuario",
  "password": "contraseña"
}

# Respuesta
{
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
  "refreshToken": "base64-refresh-token",
  "expiresAt": "2024-01-01T12:00:00Z",
  "user": {
    "id": 1,
    "name": "Usuario Ejemplo",
    "email": "usuario@ejemplo.com",
    "login": "usuario"
  }
}
```

### Crear Usuario

```bash
POST /api/users
Authorization: Bearer {token}
{
  "name": "Nuevo Usuario",
  "email": "nuevo@ejemplo.com",
  "login": "nuevousuario",
  "password": "contraseña123",
  "department": "IT",
  "position": "Desarrollador"
}
```

## 🗂️ Estructura del Proyecto

```
mesaapi/
├── src/
│   ├── MesaApi.Api/              # Capa de presentación
│   │   ├── Controllers/          # Controladores REST
│   │   ├── Middleware/           # Middleware personalizado
│   │   └── Extensions/           # Extensiones de configuración
│   ├── MesaApi.Application/      # Capa de aplicación
│   │   ├── Features/             # Casos de uso (CQRS)
│   │   ├── Common/               # Modelos y interfaces comunes
│   │   └── Mappings/             # Perfiles de AutoMapper
│   ├── MesaApi.Domain/           # Capa de dominio
│   │   ├── Entities/             # Entidades de negocio
│   │   ├── Enums/                # Enumeraciones
│   │   └── Interfaces/           # Contratos del dominio
│   └── MesaApi.Infrastructure/   # Capa de infraestructura
│       ├── Data/                 # Contexto y configuraciones EF
│       ├── Repositories/         # Implementación de repositorios
│       └── Services/             # Servicios externos
└── README.md
```

## 🔧 Configuración Avanzada

### Variables de Entorno

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=intranet;User Id=root;Password=***;TrustServerCertificate=true"
  },
  "JwtSettings": {
    "SecretKey": "tu-clave-secreta-de-al-menos-32-caracteres",
    "Issuer": "MesaApi",
    "Audience": "MesaApiUsers",
    "ExpirationInMinutes": 60
  }
}
```

### Logging

La API utiliza **Serilog** para logging estructurado:
- Logs en consola para desarrollo
- Logs en archivos rotativos (`logs/mesaapi-YYYY-MM-DD.txt`)
- Configuración por ambiente

## 🧪 Testing

```bash
# Ejecutar tests unitarios (cuando estén implementados)
dotnet test

# Ejecutar con cobertura
dotnet test --collect:"XPlat Code Coverage"
```

## 📈 Próximas Funcionalidades

- [ ] **Queries** completas para todas las entidades
- [ ] **Paginación** avanzada con filtros
- [ ] **Notificaciones** por email
- [ ] **Dashboard** con métricas
- [ ] **Reportes** en PDF/Excel
- [ ] **WebSockets** para actualizaciones en tiempo real
- [ ] **Tests unitarios** e integración
- [ ] **Docker** containerization
- [ ] **CI/CD** pipeline

## 🤝 Contribución

1. Fork el proyecto
2. Crear una rama para tu feature (`git checkout -b feature/nueva-funcionalidad`)
3. Commit tus cambios (`git commit -am 'Agregar nueva funcionalidad'`)
4. Push a la rama (`git push origin feature/nueva-funcionalidad`)
5. Crear un Pull Request

## 📄 Licencia

Este p