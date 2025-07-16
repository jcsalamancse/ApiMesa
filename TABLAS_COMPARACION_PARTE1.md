# Comparación de Tablas: Sistema Legacy (PHP) vs Sistema Nuevo (.NET) vs InventarioAPI

Este documento presenta una comparación detallada entre las tablas de la base de datos del sistema original desarrollado en PHP/XAMPP, el nuevo sistema implementado en .NET, y la base de datos InventarioAPI.

## Resumen de Tablas

| Sistema Legacy (PHP) | Sistema Nuevo (.NET) | InventarioAPI | Estado | Observaciones |
|---------------------|---------------------|--------------|--------|---------------|
| sys_usuarios | sys_usuarios | sys_usuarios | ✅ Presente en todos | Tabla de usuarios con estructura similar |
| sys_rol | pro_rol | sys_rol | ⚠️ Nombres diferentes | Tabla de roles con estructura similar pero nombres diferentes |
| hd_ticket | pro_solicitud | hd_ticket | ⚠️ Nombres diferentes | Tickets/solicitudes con estructura similar |
| No existe | pro_pasosolicitud | pro_pasosolicitud | ✅ Nueva en .NET | Pasos de solicitud para workflows |
| hd_comentarios | pro_comentarios | pro_comentarios | ✅ Presente en todos | Comentarios en tickets/solicitudes |
| No existe | pro_datossolicitud | pro_datossolicitud | ✅ Nueva en .NET | Datos adicionales de solicitudes |
| hd_archivos | pro_adjuntos | No existe | ⚠️ Parcial | Archivos adjuntos |
| sys_sessiones | sys_sessiones | sys_sessiones | ✅ Presente en todos | Sesiones de usuario |
| No existe | pro_workflow | No existe | 🆕 Nueva | Definición de workflows |
| No existe | pro_workflowstep | No existe | 🆕 Nueva | Pasos de workflow |
| cap_curso | No existe | cap_curso | ❌ No migrada | Cursos de capacitación |
| cap_actividad | No existe | cap_actividad | ❌ No migrada | Actividades de capacitación |
| cap_evaluacion | No existe | cap_evaluacion | ❌ No migrada | Evaluaciones de capacitación |
| cap_evaluacion_usuario | No existe | cap_evaluacion_usuario | ❌ No migrada | Resultados de evaluaciones |
| hd_servidor | No existe | hd_servidor | ❌ No migrada | Información de servidores |
| hd_discos_servidor | No existe | hd_discos_servidor | ❌ No migrada | Discos de servidores |
| hd_servicios_servidor | No existe | hd_servicios_servidor | ❌ No migrada | Servicios de servidores |
| pro_tipopaso | No existe | pro_tipopaso | ❌ No migrada | Tipos de pasos en procesos |
| pro_accion | No existe | pro_accion | ❌ No migrada | Acciones de procesos |
| sys_parametros | No existe | sys_parametros | ❌ No migrada | Parámetros del sistema |
| sys_logtrans | No existe | sys_logtrans | ❌ No migrada | Log de transacciones |

## Comparación Detallada por Tabla

### Tabla de Usuarios

| Sistema Legacy (PHP) | Sistema Nuevo (.NET) | InventarioAPI |
|---------------------|---------------------|--------------|
| **Nombre Tabla:** sys_usuarios | **Nombre Tabla:** sys_usuarios | **Nombre Tabla:** sys_usuarios |
| **Campos:** | **Campos:** | **Campos:** |
| idusuario (INT, PK, IDENTITY) | idusuario (INT, PK, IDENTITY) | idusuario (INT, PK, IDENTITY) |
| login (VARCHAR(50), NOT NULL) | login (NVARCHAR(50), NOT NULL, UNIQUE) | login (VARCHAR(50), NOT NULL) |
| password (VARCHAR(255), NOT NULL) | password_hash (NVARCHAR(255), NOT NULL) | password (VARCHAR(255), NOT NULL) |
| name (VARCHAR(100), NOT NULL) | name (NVARCHAR(100), NOT NULL) | name (VARCHAR(100), NOT NULL) |
| mail (VARCHAR(100), NULL) | email (NVARCHAR(255), NOT NULL, UNIQUE) | mail (VARCHAR(100), NULL) |
| activo (CHAR(2), NULL) | is_active (BIT, NOT NULL, DEFAULT 1) | activo (CHAR(2), NULL) |
| interno (CHAR(2), NULL) | No existe | interno (CHAR(2), NULL) |
| No existe | phone (NVARCHAR(20), NULL) | No existe |
| No existe | department (NVARCHAR(100), NULL) | No existe |
| No existe | position (NVARCHAR(100), NULL) | No existe |
| fechacreacion (DATETIME, NULL) | created_at (DATETIME2, NOT NULL) | fechacreacion (DATETIME, NULL) |
| fechamodificacion (DATETIME, NULL) | updated_at (DATETIME2, NULL) | fechamodificacion (DATETIME, NULL) |
| No existe | created_by (NVARCHAR(50), NULL) | No existe |
| No existe | updated_by (NVARCHAR(50), NULL) | No existe |
| No existe | is_deleted (BIT, NOT NULL, DEFAULT 0) | No existe |

### Tabla de Roles

| Sistema Legacy (PHP) | Sistema Nuevo (.NET) | InventarioAPI |
|---------------------|---------------------|--------------|
| **Nombre Tabla:** sys_rol | **Nombre Tabla:** pro_rol | **Nombre Tabla:** sys_rol |
| **Campos:** | **Campos:** | **Campos:** |
| idrol (INT, PK, IDENTITY) | idrol (INT, PK, IDENTITY) | idrol (INT, PK, IDENTITY) |
| rol (VARCHAR(100), NOT NULL) | rol (NVARCHAR(100), NOT NULL, UNIQUE) | rol (VARCHAR(100), NOT NULL) |
| descripcion (TEXT, NULL) | descripcion (NVARCHAR(500), NULL) | descripcion (TEXT, NULL) |
| activo (CHAR(2), NULL) | activo (BIT, NOT NULL, DEFAULT 1) | activo (CHAR(2), NULL) |
| interno (CHAR(2), NULL) | No existe | interno (CHAR(2), NULL) |
| fechacreacion (DATETIME, NULL) | created_at (DATETIME2, NOT NULL) | fechacreacion (DATETIME, NULL) |
| No existe | updated_at (DATETIME2, NULL) | No existe |
| No existe | created_by (NVARCHAR(50), NULL) | No existe |
| No existe | updated_by (NVARCHAR(50), NULL) | No existe |
| No existe | is_deleted (BIT, NOT NULL, DEFAULT 0) | No existe |

### Tabla de Tickets/Solicitudes

| Sistema Legacy (PHP) | Sistema Nuevo (.NET) | InventarioAPI |
|---------------------|---------------------|--------------|
| **Nombre Tabla:** hd_ticket | **Nombre Tabla:** pro_solicitud | **Nombre Tabla:** hd_ticket |
| **Campos:** | **Campos:** | **Campos:** |
| idticket (INT, PK, IDENTITY) | idsolicitud (INT, PK, IDENTITY) | idticket (INT, PK, IDENTITY) |
| ticket (VARCHAR(255), NOT NULL) | No existe | ticket (VARCHAR(255), NOT NULL) |
| descripcion (TEXT, NULL) | descripcion (NVARCHAR(2000), NOT NULL) | descripcion (TEXT, NULL) |
| estado (VARCHAR(20), NULL) | estado (INT, NOT NULL, DEFAULT 1) | estado (VARCHAR(20), NULL) |
| prioridad (VARCHAR(10), NULL) | prioridad (INT, NOT NULL, DEFAULT 2) | prioridad (VARCHAR(10), NULL) |
| categoria (VARCHAR(100), NULL) | categoria (NVARCHAR(100), NULL) | categoria (VARCHAR(100), NULL) |
| subcategoria (VARCHAR(100), NULL) | subcategoria (NVARCHAR(100), NULL) | subcategoria (VARCHAR(100), NULL) |
| usuariocreacion (INT, FK) | idusuario_solicitante (INT, NOT NULL, FK) | usuariocreacion (INT, FK) |
| usuarioactual (INT, FK, NULL) | idusuario_asignado (INT, NULL, FK) | usuarioactual (INT, FK, NULL) |
| usuariocierre (INT, FK, NULL) | No existe | usuariocierre (INT, FK, NULL) |
| No existe | No existe | tipoproceso (VARCHAR(100), NULL) |
| fechacreacion (DATETIME, NULL) | created_at (DATETIME2, NOT NULL) | fechacreacion (DATETIME, NULL) |
| fechaasignacion (DATETIME, NULL) | No existe | fechaasignacion (DATETIME, NULL) |
| fechasolicitudcierre (DATETIME, NULL) | No existe | fechasolicitudcierre (DATETIME, NULL) |
| fechacierre (DATETIME, NULL) | fecha_completado (DATETIME2, NULL) | fechacierre (DATETIME, NULL) |
| tiempoestimado (INT, NULL) | No existe | tiempoestimado (INT, NULL) |
| tiemporeal (INT, NULL) | No existe | tiemporeal (INT, NULL) |
| No existe | fecha_vencimiento (DATETIME2, NULL) | No existe |
| No existe | updated_at (DATETIME2, NULL) | fechamodificacion (DATETIME, NULL) |
| No existe | created_by (NVARCHAR(50), NULL) | No existe |
| No existe | updated_by (NVARCHAR(50), NULL) | No existe |
| No existe | is_deleted (BIT, NOT NULL, DEFAULT 0) | No existe |