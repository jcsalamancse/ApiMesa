# Mesa de Ayuda API

Sistema integral de mesa de ayuda con capacidades avanzadas de automatización, integración con Azure DevOps y Microsoft Teams.

## 🚀 Características Principales

### Core Features
- ✅ Gestión completa de solicitudes de soporte
- ✅ Sistema de usuarios y roles
- ✅ Flujos de trabajo configurables
- ✅ Comentarios y seguimiento de solicitudes
- ✅ Gestión de archivos adjuntos
- ✅ Reportes y estadísticas

### Características Avanzadas (En Desarrollo)
- 🔄 **Automatización de Documentos**: Generación automática de actas y documentos de pase a producción
- 🔄 **Integración Azure DevOps**: Sincronización bidireccional de tareas y work items
- 🔄 **Gestión Basada en Perfiles**: Asignación inteligente de tareas según perfil del usuario
- 🔄 **Integración Microsoft Teams**: Bot y tabs embebidas para colaboración
- 🔄 **Notificaciones en Tiempo Real**: SignalR para actualizaciones instantáneas
- 🔄 **IA y Machine Learning**: Clasificación automática y análisis predictivo
- 🔄 **Arquitectura Híbrida**: Soporte para SQL y NoSQL (CosmosDB)

## 🏗️ Arquitectura

### Tecnologías Utilizadas
- **Backend**: .NET 8, ASP.NET Core Web API
- **Base de Datos**: SQL Server con Entity Framework Core
- **Autenticación**: JWT con soporte para OAuth 2.0
- **Documentación**: Swagger/OpenAPI
- **Logging**: Serilog
- **Patrones**: Clean Architecture, CQRS, Repository Pattern

### Estructura del Proyecto
```
src/
├── MesaApi.Api/              # Capa de presentación (Controllers, Middleware)
├── MesaApi.Application/      # Lógica de aplicación (Commands, Queries, Handlers)
├── MesaApi.Domain/          # Entidades de dominio e interfaces
└── MesaApi.Infrastructure/   # Implementaciones de infraestructura (DB, Servicios)
```

## 🛠️ Configuración del Entorno

### Prerrequisitos
- .NET 8 SDK
- SQL Server (LocalDB o instancia completa)
- Visual Studio 2022 o VS Code

### Configuración Inicial
1. Clonar el repositorio
2. Configurar la cadena de conexión en `appsettings.json`
3. Ejecutar migraciones de base de datos
4. Configurar variables de entorno para JWT

### Variables de Entorno
```bash
JWT_SECRET_KEY=tu_clave_secreta_aqui
JWT_ISSUER=MesaApiIssuer
JWT_AUDIENCE=MesaApiAudience
CONNECTION_STRING=tu_cadena_conexion_aqui
```

## 🚀 Ejecución

### Desarrollo Local
```bash
cd src/MesaApi.Api
dotnet run
```

### Docker
```bash
docker-compose up -d
```

## 📋 Roadmap de Desarrollo

### Fase 1: Fundamentos (✅ Completado)
- [x] API Core con autenticación JWT
- [x] Gestión de solicitudes y usuarios
- [x] Sistema de comentarios y archivos
- [x] Reportes básicos

### Fase 2: Automatización y Integración (🔄 En Progreso)
- [ ] Sistema de plantillas para documentos
- [ ] Integración con Azure DevOps
- [ ] Notificaciones en tiempo real (SignalR)
- [ ] Flujos de trabajo automatizados

### Fase 3: Colaboración y BI (📋 Planificado)
- [ ] Integración con Microsoft Teams
- [ ] Dashboard avanzado con métricas
- [ ] Base de conocimiento integrada
- [ ] Análisis predictivo básico

### Fase 4: IA y Escalabilidad (🔮 Futuro)
- [ ] Arquitectura de microservicios
- [ ] Integración con servicios cognitivos
- [ ] Soporte para CosmosDB
- [ ] Event Sourcing para auditoría completa

## 🔧 APIs Disponibles

### Autenticación
- `POST /api/auth/login` - Iniciar sesión
- `POST /api/auth/refresh` - Renovar token

### Solicitudes
- `GET /api/requests` - Listar solicitudes
- `POST /api/requests` - Crear solicitud
- `GET /api/requests/{id}` - Obtener solicitud por ID
- `PUT /api/requests/{id}/status` - Actualizar estado
- `POST /api/requests/{id}/assign` - Asignar solicitud

### Usuarios
- `GET /api/users` - Listar usuarios
- `POST /api/users` - Crear usuario
- `PUT /api/users/{id}` - Actualizar usuario
- `DELETE /api/users/{id}` - Eliminar usuario

### Reportes
- `GET /api/reports/requests` - Reporte de solicitudes
- `GET /api/stats/dashboard` - Estadísticas del dashboard

## 🧪 Testing

```bash
# Ejecutar todas las pruebas
dotnet test

# Ejecutar con cobertura
dotnet test --collect:"XPlat Code Coverage"
```

## 📖 Documentación

La documentación completa de la API está disponible en:
- **Swagger UI**: `https://localhost:5001/swagger`
- **OpenAPI Spec**: `https://localhost:5001/swagger/v1/swagger.json`

## 🤝 Contribución

1. Fork el proyecto
2. Crear una rama para tu feature (`git checkout -b feature/AmazingFeature`)
3. Commit tus cambios (`git commit -m 'Add some AmazingFeature'`)
4. Push a la rama (`git push origin feature/AmazingFeature`)
5. Abrir un Pull Request

## 📝 Convenciones de Código

- Seguir las convenciones de C# y .NET
- Usar Clean Architecture principles
- Implementar pruebas unitarias para nueva funcionalidad
- Documentar APIs con XML comments
- Seguir patrones SOLID

## 🔒 Seguridad

- Autenticación JWT con refresh tokens
- Validación de entrada en todos los endpoints
- Logging de auditoría para operaciones críticas
- Cifrado de datos sensibles en base de datos
- Rate limiting para prevenir abuso

## 📊 Monitoreo y Observabilidad

- Logging estructurado con Serilog
- Health checks para dependencias
- Métricas de rendimiento
- Alertas para errores críticos

## 🌐 Despliegue

### Azure App Service
```bash
# Configurar Azure CLI y desplegar
az webapp deployment source config-zip --resource-group myResourceGroup --name myAppName --src release.zip
```

### Docker
```dockerfile
# Ver Dockerfile para configuración completa
FROM mcr.microsoft.com/dotnet/aspnet:8.0
```

## 📞 Soporte

Para soporte técnico o preguntas:
- Crear un issue en GitHub
- Contactar al equipo de desarrollo
- Revisar la documentación en `/docs`

## 📄 Licencia

Este proyecto está bajo la Licencia MIT - ver el archivo [LICENSE](LICENSE) para detalles.

---

**Desarrollado con ❤️ para optimizar la gestión de soporte técnico**