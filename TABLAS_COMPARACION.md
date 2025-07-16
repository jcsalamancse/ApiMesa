# Comparación de Tablas: Sistema Original (PHP/XAMPP) vs Sistema Nuevo (.NET)

Este documento presenta una comparación detallada entre las tablas de la base de datos del sistema original desarrollado en PHP/XAMPP y el nuevo sistema implementado en .NET.

## Resumen de Tablas

| Sistema Original (PHP/XAMPP) | Sistema Nuevo (.NET) | Estado | Observaciones |
|------------------------------|---------------------|--------|---------------|
| [Completar nombre tabla] | sys_usuarios | ✅ Migrada | Tabla de usuarios con campos mejorados |
| [Completar nombre tabla] | pro_rol | ✅ Migrada | Tabla de roles de usuario |
| [Completar nombre tabla] | pro_solicitud | ✅ Migrada | Tabla de solicitudes/tickets |
| [Completar nombre tabla] | pro_pasosolicitud | ✅ Migrada | Pasos de solicitud |
| [Completar nombre tabla] | pro_comentarios | ✅ Migrada | Comentarios en solicitudes |
| [Completar nombre tabla] | pro_datossolicitud | ✅ Migrada | Datos adicionales de solicitudes |
| [Completar nombre tabla] | pro_adjuntos | ✅ Migrada | Archivos adjuntos |
| [Completar nombre tabla] | sys_sessiones | ✅ Migrada | Sesiones de usuario |
| [Completar nombre tabla] | pro_workflow | ✅ Nueva | Definición de workflows |
| [Completar nombre tabla] | pro_workflowstep | ✅ Nueva | Pasos de workflow |

## Comparación Detallada por Tabla

### Tabla de Usuarios

| Sistema Original (PHP/XAMPP) | Sistema Nuevo (.NET) |
|------------------------------|---------------------|
| **Nombre Tabla:** [Completar] | **Nombre Tabla:** sys_usuarios |
| **Campos:** | **Campos:** |
| [Completar campo] | idusuario (INT, PK, IDENTITY) |
| [Completar campo] | name (NVARCHAR(100), NOT NULL) |
| [Completar campo] | email (NVARCHAR(255), NOT NULL, UNIQUE) |
| [Completar campo] | login (NVARCHAR(50), NOT NULL, UNIQUE) |
| [Completar campo] | password_hash (NVARCHAR(255), NOT NULL) |
| [Completar campo] | is_active (BIT, NOT NULL, DEFAULT 1) |
| [Completar campo] | phone (NVARCHAR(20), NULL) |
| [Completar campo] | department (NVARCHAR(100), NULL) |
| [Completar campo] | position (NVARCHAR(100), NULL) |
| [Completar campo] | created_at (DATETIME2, NOT NULL) |
| [Completar campo] | updated_at (DATETIME2, NULL) |
| [Completar campo] | created_by (NVARCHAR(50), NULL) |
| [Completar campo] | updated_by (NVARCHAR(50), NULL) |
| [Completar campo] | is_deleted (BIT, NOT NULL, DEFAULT 0) |
| **Índices:** | **Índices:** |
| [Completar índice] | PK_sys_usuarios (idusuario) |
| [Completar índice] | UQ_sys_usuarios_email (email) |
| [Completar índice] | UQ_sys_usuarios_login (login) |
| [Completar índice] | IX_sys_usuarios_is_active (is_active) |

### Tabla de Roles

| Sistema Original (PHP/XAMPP) | Sistema Nuevo (.NET) |
|------------------------------|---------------------|
| **Nombre Tabla:** [Completar] | **Nombre Tabla:** pro_rol |
| **Campos:** | **Campos:** |
| [Completar campo] | idrol (INT, PK, IDENTITY) |
| [Completar campo] | rol (NVARCHAR(100), NOT NULL, UNIQUE) |
| [Completar campo] | descripcion (NVARCHAR(500), NULL) |
| [Completar campo] | activo (BIT, NOT NULL, DEFAULT 1) |
| [Completar campo] | created_at (DATETIME2, NOT NULL) |
| [Completar campo] | updated_at (DATETIME2, NULL) |
| [Completar campo] | created_by (NVARCHAR(50), NULL) |
| [Completar campo] | updated_by (NVARCHAR(50), NULL) |
| [Completar campo] | is_deleted (BIT, NOT NULL, DEFAULT 0) |
| **Índices:** | **Índices:** |
| [Completar índice] | PK_pro_rol (idrol) |
| [Completar índice] | UQ_pro_rol_rol (rol) |
| [Completar índice] | IX_pro_rol_activo (activo) |

### Tabla de Solicitudes

| Sistema Original (PHP/XAMPP) | Sistema Nuevo (.NET) |
|------------------------------|---------------------|
| **Nombre Tabla:** [Completar] | **Nombre Tabla:** pro_solicitud |
| **Campos:** | **Campos:** |
| [Completar campo] | idsolicitud (INT, PK, IDENTITY) |
| [Completar campo] | descripcion (NVARCHAR(2000), NOT NULL) |
| [Completar campo] | estado (INT, NOT NULL, DEFAULT 1) |
| [Completar campo] | prioridad (INT, NOT NULL, DEFAULT 2) |
| [Completar campo] | categoria (NVARCHAR(100), NULL) |
| [Completar campo] | subcategoria (NVARCHAR(100), NULL) |
| [Completar campo] | idusuario_solicitante (INT, NOT NULL, FK) |
| [Completar campo] | idusuario_asignado (INT, NULL, FK) |
| [Completar campo] | fecha_vencimiento (DATETIME2, NULL) |
| [Completar campo] | fecha_completado (DATETIME2, NULL) |
| [Completar campo] | created_at (DATETIME2, NOT NULL) |
| [Completar campo] | updated_at (DATETIME2, NULL) |
| [Completar campo] | created_by (NVARCHAR(50), NULL) |
| [Completar campo] | updated_by (NVARCHAR(50), NULL) |
| [Completar campo] | is_deleted (BIT, NOT NULL, DEFAULT 0) |
| **Índices:** | **Índices:** |
| [Completar índice] | PK_pro_solicitud (idsolicitud) |
| [Completar índice] | IX_pro_solicitud_estado (estado) |
| [Completar índice] | IX_pro_solicitud_prioridad (prioridad) |
| [Completar índice] | IX_pro_solicitud_idusuario_solicitante (idusuario_solicitante) |
| [Completar índice] | IX_pro_solicitud_idusuario_asignado (idusuario_asignado) |
| [Completar índice] | IX_pro_solicitud_created_at (created_at) |
| **Relaciones:** | **Relaciones:** |
| [Completar relación] | FK_pro_solicitud_sys_usuarios_solicitante |
| [Completar relación] | FK_pro_solicitud_sys_usuarios_asignado |

### Tabla de Pasos de Solicitud

| Sistema Original (PHP/XAMPP) | Sistema Nuevo (.NET) |
|------------------------------|---------------------|
| **Nombre Tabla:** [Completar] | **Nombre Tabla:** pro_pasosolicitud |
| **Campos:** | **Campos:** |
| [Completar campo] | idpaso (INT, PK, IDENTITY) |
| [Completar campo] | idsolicitud (INT, NOT NULL, FK) |
| [Completar campo] | pasoproceso (NVARCHAR(100), NOT NULL) |
| [Completar campo] | tipopaso (NVARCHAR(50), NULL) |
| [Completar campo] | orden (INT, NOT NULL) |
| [Completar campo] | estado (NVARCHAR(20), NOT NULL, DEFAULT 'Pending') |
| [Completar campo] | realizadopor (INT, NULL, FK) |
| [Completar campo] | idrol (INT, NULL, FK) |
| [Completar campo] | fecha (DATETIME2, NULL) |
| [Completar campo] | notas (NVARCHAR(500), NULL) |
| [Completar campo] | created_at (DATETIME2, NOT NULL) |
| [Completar campo] | updated_at (DATETIME2, NULL) |
| [Completar campo] | created_by (NVARCHAR(50), NULL) |
| [Completar campo] | updated_by (NVARCHAR(50), NULL) |
| [Completar campo] | is_deleted (BIT, NOT NULL, DEFAULT 0) |
| **Índices:** | **Índices:** |
| [Completar índice] | PK_pro_pasosolicitud (idpaso) |
| [Completar índice] | IX_pro_pasosolicitud_idsolicitud (idsolicitud) |
| [Completar índice] | IX_pro_pasosolicitud_realizadopor (realizadopor) |
| [Completar índice] | IX_pro_pasosolicitud_idrol (idrol) |
| [Completar índice] | IX_pro_pasosolicitud_orden (orden) |
| **Relaciones:** | **Relaciones:** |
| [Completar relación] | FK_pro_pasosolicitud_pro_solicitud |
| [Completar relación] | FK_pro_pasosolicitud_sys_usuarios |
| [Completar relación] | FK_pro_pasosolicitud_pro_rol |

### Tabla de Comentarios

| Sistema Original (PHP/XAMPP) | Sistema Nuevo (.NET) |
|------------------------------|---------------------|
| **Nombre Tabla:** [Completar] | **Nombre Tabla:** pro_comentarios |
| **Campos:** | **Campos:** |
| [Completar campo] | idcomentario (INT, PK, IDENTITY) |
| [Completar campo] | idsolicitud (INT, NOT NULL, FK) |
| [Completar campo] | idusuario (INT, NOT NULL, FK) |
| [Completar campo] | comentario (NVARCHAR(2000), NOT NULL) |
| [Completar campo] | es_interno (BIT, NOT NULL, DEFAULT 0) |
| [Completar campo] | fecha (DATETIME2, NOT NULL) |
| [Completar campo] | updated_at (DATETIME2, NULL) |
| [Completar campo] | created_by (NVARCHAR(50), NULL) |
| [Completar campo] | updated_by (NVARCHAR(50), NULL) |
| [Completar campo] | is_deleted (BIT, NOT NULL, DEFAULT 0) |
| **Índices:** | **Índices:** |
| [Completar índice] | PK_pro_comentarios (idcomentario) |
| [Completar índice] | IX_pro_comentarios_idsolicitud (idsolicitud) |
| [Completar índice] | IX_pro_comentarios_idusuario (idusuario) |
| [Completar índice] | IX_pro_comentarios_fecha (fecha) |
| **Relaciones:** | **Relaciones:** |
| [Completar relación] | FK_pro_comentarios_pro_solicitud |
| [Completar relación] | FK_pro_comentarios_sys_usuarios |

### Tabla de Datos de Solicitud

| Sistema Original (PHP/XAMPP) | Sistema Nuevo (.NET) |
|------------------------------|---------------------|
| **Nombre Tabla:** [Completar] | **Nombre Tabla:** pro_datossolicitud |
| **Campos:** | **Campos:** |
| [Completar campo] | iddato (INT, PK, IDENTITY) |
| [Completar campo] | idsolicitud (INT, NOT NULL, FK) |
| [Completar campo] | nombre (NVARCHAR(100), NOT NULL) |
| [Completar campo] | dato (NVARCHAR(500), NOT NULL) |
| [Completar campo] | tipo (NVARCHAR(50), NULL) |
| [Completar campo] | created_at (DATETIME2, NOT NULL) |
| [Completar campo] | updated_at (DATETIME2, NULL) |
| [Completar campo] | created_by (NVARCHAR(50), NULL) |
| [Completar campo] | updated_by (NVARCHAR(50), NULL) |
| [Completar campo] | is_deleted (BIT, NOT NULL, DEFAULT 0) |
| **Índices:** | **Índices:** |
| [Completar índice] | PK_pro_datossolicitud (iddato) |
| [Completar índice] | IX_pro_datossolicitud_idsolicitud (idsolicitud) |
| [Completar índice] | IX_pro_datossolicitud_nombre (nombre) |
| **Relaciones:** | **Relaciones:** |
| [Completar relación] | FK_pro_datossolicitud_pro_solicitud |

### Tabla de Adjuntos

| Sistema Original (PHP/XAMPP) | Sistema Nuevo (.NET) |
|------------------------------|---------------------|
| **Nombre Tabla:** [Completar] | **Nombre Tabla:** pro_adjuntos |
| **Campos:** | **Campos:** |
| [Completar campo] | idadjunto (INT, PK, IDENTITY) |
| [Completar campo] | idsolicitud (INT, NOT NULL, FK) |
| [Completar campo] | nombre_archivo (NVARCHAR(255), NOT NULL) |
| [Completar campo] | ruta_archivo (NVARCHAR(500), NOT NULL) |
| [Completar campo] | tipo_contenido (NVARCHAR(100), NOT NULL) |
| [Completar campo] | tamano (BIGINT, NOT NULL) |
| [Completar campo] | created_at (DATETIME2, NOT NULL) |
| [Completar campo] | updated_at (DATETIME2, NULL) |
| [Completar campo] | created_by (NVARCHAR(50), NULL) |
| [Completar campo] | updated_by (NVARCHAR(50), NULL) |
| [Completar campo] | is_deleted (BIT, NOT NULL, DEFAULT 0) |
| **Índices:** | **Índices:** |
| [Completar índice] | PK_pro_adjuntos (idadjunto) |
| [Completar índice] | IX_pro_adjuntos_idsolicitud (idsolicitud) |
| [Completar índice] | IX_pro_adjuntos_nombre_archivo (nombre_archivo) |
| **Relaciones:** | **Relaciones:** |
| [Completar relación] | FK_pro_adjuntos_pro_solicitud |

### Tabla de Sesiones de Usuario

| Sistema Original (PHP/XAMPP) | Sistema Nuevo (.NET) |
|------------------------------|---------------------|
| **Nombre Tabla:** [Completar] | **Nombre Tabla:** sys_sessiones |
| **Campos:** | **Campos:** |
| [Completar campo] | idsession (INT, PK, IDENTITY) |
| [Completar campo] | idusuario (INT, NOT NULL, FK) |
| [Completar campo] | sid (NVARCHAR(255), NOT NULL) |
| [Completar campo] | login (NVARCHAR(50), NOT NULL) |
| [Completar campo] | expira (DATETIME2, NOT NULL) |
| [Completar campo] | activo (BIT, NOT NULL, DEFAULT 1) |
| [Completar campo] | created_at (DATETIME2, NOT NULL) |
| [Completar campo] | updated_at (DATETIME2, NULL) |
| [Completar campo] | created_by (NVARCHAR(50), NULL) |
| [Completar campo] | updated_by (NVARCHAR(50), NULL) |
| [Completar campo] | is_deleted (BIT, NOT NULL, DEFAULT 0) |
| **Índices:** | **Índices:** |
| [Completar índice] | PK_sys_sessiones (idsession) |
| [Completar índice] | IX_sys_sessiones_sid (sid) |
| [Completar índice] | IX_sys_sessiones_login (login) |
| [Completar índice] | IX_sys_sessiones_idusuario (idusuario) |
| [Completar índice] | IX_sys_sessiones_activo (activo) |
| **Relaciones:** | **Relaciones:** |
| [Completar relación] | FK_sys_sessiones_sys_usuarios |

### Tabla de Workflows (Nueva)

| Sistema Original (PHP/XAMPP) | Sistema Nuevo (.NET) |
|------------------------------|---------------------|
| **Nombre Tabla:** [No existe] | **Nombre Tabla:** pro_workflow |
| **Campos:** | **Campos:** |
| [No existe] | idworkflow (INT, PK, IDENTITY) |
| [No existe] | nombre (NVARCHAR(100), NOT NULL) |
| [No existe] | descripcion (NVARCHAR(500), NULL) |
| [No existe] | categoria (NVARCHAR(100), NULL) |
| [No existe] | activo (BIT, NOT NULL, DEFAULT 1) |
| [No existe] | created_at (DATETIME2, NOT NULL) |
| [No existe] | updated_at (DATETIME2, NULL) |
| [No existe] | created_by (NVARCHAR(50), NULL) |
| [No existe] | updated_by (NVARCHAR(50), NULL) |
| [No existe] | is_deleted (BIT, NOT NULL, DEFAULT 0) |
| **Índices:** | **Índices:** |
| [No existe] | PK_pro_workflow (idworkflow) |
| [No existe] | IX_pro_workflow_activo (activo) |
| [No existe] | IX_pro_workflow_categoria (categoria) |

### Tabla de Pasos de Workflow (Nueva)

| Sistema Original (PHP/XAMPP) | Sistema Nuevo (.NET) |
|------------------------------|---------------------|
| **Nombre Tabla:** [No existe] | **Nombre Tabla:** pro_workflowstep |
| **Campos:** | **Campos:** |
| [No existe] | idstep (INT, PK, IDENTITY) |
| [No existe] | idworkflow (INT, NOT NULL, FK) |
| [No existe] | nombre_paso (NVARCHAR(100), NOT NULL) |
| [No existe] | tipo_paso (NVARCHAR(50), NULL) |
| [No existe] | orden (INT, NOT NULL) |
| [No existe] | idrol (INT, NULL, FK) |
| [No existe] | created_at (DATETIME2, NOT NULL) |
| [No existe] | updated_at (DATETIME2, NULL) |
| [No existe] | created_by (NVARCHAR(50), NULL) |
| [No existe] | updated_by (NVARCHAR(50), NULL) |
| [No existe] | is_deleted (BIT, NOT NULL, DEFAULT 0) |
| **Índices:** | **Índices:** |
| [No existe] | PK_pro_workflowstep (idstep) |
| [No existe] | IX_pro_workflowstep_idworkflow (idworkflow) |
| [No existe] | IX_pro_workflowstep_idrol (idrol) |
| [No existe] | IX_pro_workflowstep_orden (orden) |
| **Relaciones:** | **Relaciones:** |
| [No existe] | FK_pro_workflowstep_pro_workflow |
| [No existe] | FK_pro_workflowstep_pro_rol |

## Mejoras en la Estructura de Base de Datos

### Mejoras Generales
1. **Campos de Auditoría**: Todas las tablas incluyen campos de auditoría consistentes (created_at, updated_at, created_by, updated_by, is_deleted)
2. **Eliminación Lógica**: Implementación de borrado lógico con el campo is_deleted
3. **Índices Optimizados**: Índices adicionales para mejorar el rendimiento de consultas frecuentes
4. **Integridad Referencial**: Restricciones de clave foránea para garantizar la integridad de datos
5. **Nomenclatura Consistente**: Convención de nombres coherente en todas las tablas y campos

### Nuevas Funcionalidades
1. **Sistema de Workflows**: Nuevas tablas para definir y gestionar workflows personalizados
2. **Pasos de Workflow**: Capacidad para definir secuencias de pasos con roles asignados
3. **Datos Adicionales**: Estructura flexible para almacenar datos personalizados por solicitud
4. **Gestión de Adjuntos**: Sistema mejorado para el manejo de archivos adjuntos
5. **Control de Sesiones**: Gestión avanzada de sesiones de usuario

## Notas sobre la Migración de Datos

Para completar la migración de datos del sistema original al nuevo sistema, se recomienda:

1. **Mapeo de Campos**: Completar este documento con los nombres de tablas y campos del sistema original
2. **Script de Migración**: Desarrollar scripts ETL para transferir datos entre sistemas
3. **Validación de Datos**: Implementar validaciones para garantizar la integridad durante la migración
4. **Migración por Fases**: Considerar una migración por fases, comenzando con datos maestros (usuarios, roles)
5. **Pruebas de Verificación**: Realizar pruebas exhaustivas para verificar la correcta migración de datos

## Próximos Pasos

1. **Completar este Documento**: Añadir la información del sistema original
2. **Desarrollar Scripts de Migración**: Crear scripts SQL para la transferencia de datos
3. **Ejecutar Migración en Ambiente de Pruebas**: Validar el proceso en un entorno controlado
4. **Planificar Migración a Producción**: Establecer un plan detallado para la migración final