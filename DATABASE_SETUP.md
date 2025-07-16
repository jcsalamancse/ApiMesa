# Configuración de Base de Datos para Mesa de Ayuda API

Esta guía explica cómo configurar y conectar la base de datos SQL Server para la API de Mesa de Ayuda.

## Opciones de Configuración

Tienes tres opciones para configurar la base de datos:

1. **SQL Server Local**: Instalar y configurar SQL Server en tu máquina local
2. **Docker**: Usar el contenedor Docker incluido en el proyecto
3. **SQL Server en la Nube**: Conectar a una instancia de SQL Server en Azure o similar

## Opción 1: SQL Server Local

### Requisitos Previos
- SQL Server 2019 o posterior instalado
- SQL Server Management Studio (SSMS) o Azure Data Studio

### Pasos de Configuración

1. **Crear la Base de Datos**:
   - Abre SSMS y conéctate a tu instancia local
   - Crea una nueva base de datos llamada `intranet` (o el nombre que prefieras)

2. **Configurar la Cadena de Conexión**:
   - Abre el archivo `appsettings.json` (o `appsettings.Development.json` para entorno de desarrollo)
   - Modifica la cadena de conexión según tu configuración:

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=localhost;Database=intranet;User Id=sa;Password=TuContraseña;TrustServerCertificate=true;MultipleActiveResultSets=true"
}
```

3. **Ejecutar Migraciones**:
   - Abre una terminal en la carpeta raíz del proyecto
   - Ejecuta los siguientes comandos:

```bash
cd src/MesaApi.Api
dotnet ef database update
```

Si prefieres ejecutar el script SQL manualmente:
```bash
# Copia el script SQL
cd src/MesaApi.Infrastructure/Data/Migrations
# Ejecuta InitialMigrationScript.sql en SSMS
```

## Opción 2: Docker (Recomendado para Desarrollo)

### Requisitos Previos
- Docker y Docker Compose instalados

### Pasos de Configuración

1. **Iniciar los Contenedores**:
   - Abre una terminal en la carpeta raíz del proyecto
   - Ejecuta:

```bash
docker-compose up -d
```

Esto iniciará:
- Un contenedor SQL Server en el puerto 1433
- Un contenedor para la API en el puerto 8080 (HTTP) y 8443 (HTTPS)

2. **Verificar la Conexión**:
   - La cadena de conexión ya está configurada en el archivo `docker-compose.yml`
   - La API se conectará automáticamente a la base de datos

3. **Acceder a la Base de Datos**:
   - Conéctate a la base de datos usando SSMS o Azure Data Studio:
     - Servidor: `localhost,1433`
     - Usuario: `sa`
     - Contraseña: `YourStrong@Passw0rd` (definida en docker-compose.yml)

## Opción 3: SQL Server en la Nube (Azure SQL)

### Requisitos Previos
- Una cuenta de Azure con una instancia de Azure SQL Database

### Pasos de Configuración

1. **Crear Base de Datos en Azure**:
   - Crea una nueva base de datos SQL en el portal de Azure
   - Configura las reglas de firewall para permitir conexiones desde tu IP

2. **Configurar la Cadena de Conexión**:
   - Obtén la cadena de conexión desde el portal de Azure
   - Actualiza el archivo `appsettings.json`:

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=tu-servidor.database.windows.net;Database=intranet;User Id=tu-usuario;Password=tu-contraseña;TrustServerCertificate=true;MultipleActiveResultSets=true"
}
```

3. **Ejecutar Migraciones**:
   - Sigue los mismos pasos que en la Opción 1 para ejecutar las migraciones

## Estructura de la Base de Datos

La base de datos incluye las siguientes tablas principales:

- **sys_usuarios**: Usuarios del sistema
- **pro_rol**: Roles de usuario
- **pro_solicitud**: Solicitudes de soporte
- **pro_pasosolicitud**: Pasos de workflow para solicitudes
- **pro_comentarios**: Comentarios en solicitudes
- **pro_datossolicitud**: Datos adicionales de solicitudes
- **pro_adjuntos**: Archivos adjuntos a solicitudes
- **sys_sessiones**: Sesiones de usuario

## Usuario Administrador Predeterminado

El script de migración crea un usuario administrador por defecto:

- **Usuario**: `admin`
- **Contraseña**: `Admin123!`
- **Email**: `admin@dataifx.com`

## Verificación de la Configuración

Para verificar que la base de datos está correctamente configurada:

1. **Ejecutar la API**:
```bash
cd src/MesaApi.Api
dotnet run
```

2. **Probar el Endpoint de Login**:
   - Usa Postman o Insomnia para hacer una solicitud POST a `/api/auth/login`
   - Usa las credenciales del administrador predeterminado

```json
{
  "login": "admin",
  "password": "Admin123!"
}
```

3. **Verificar la Respuesta**:
   - Deberías recibir un token JWT si la conexión a la base de datos es correcta

## Solución de Problemas

### Error de Conexión a la Base de Datos

Si recibes errores de conexión:

1. **Verifica la Cadena de Conexión**:
   - Asegúrate de que el servidor, usuario y contraseña sean correctos
   - Verifica que la base de datos exista

2. **Verifica el Firewall**:
   - Si usas Azure SQL, asegúrate de que tu IP esté permitida
   - Si usas SQL Server local, verifica que el servicio esté en ejecución

3. **Logs de la Aplicación**:
   - Revisa los logs en la carpeta `logs/` para obtener más detalles sobre el error

### Error en las Migraciones

Si las migraciones fallan:

1. **Ejecuta el Script Manualmente**:
   - Usa el archivo `InitialMigrationScript.sql` directamente en SSMS

2. **Verifica los Permisos**:
   - Asegúrate de que el usuario tenga permisos para crear tablas

## Notas Adicionales

- La aplicación usa Entity Framework Core para acceder a la base de datos
- Se implementa el patrón Repository y Unit of Work para abstraer el acceso a datos
- Las configuraciones de entidades se encuentran en `MesaApi.Infrastructure/Data/Configurations/`
- El contexto de base de datos está en `MesaApi.Infrastructure/Data/ApplicationDbContext.cs`