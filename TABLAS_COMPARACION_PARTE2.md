### Tabla de Comentarios

| Sistema Legacy (PHP) | Sistema Nuevo (.NET) | InventarioAPI |
|---------------------|---------------------|--------------|
| **Nombre Tabla:** hd_comentarios | **Nombre Tabla:** pro_comentarios | **Nombre Tabla:** pro_comentarios |
| **Campos:** | **Campos:** | **Campos:** |
| idcomentario (INT, PK, IDENTITY) | idcomentario (INT, PK, IDENTITY) | id (INT, PK, IDENTITY) |
| idticket (INT, NOT NULL, FK) | idsolicitud (INT, NOT NULL, FK) | idsolicitud (INT, NOT NULL, FK) |
| comentario (TEXT, NOT NULL) | comentario (NVARCHAR(2000), NOT NULL) | comentario (TEXT, NOT NULL) |
| fecha (DATETIME, NULL) | fecha (DATETIME2, DEFAULT GETUTCDATE()) | fecha (DATETIME, NULL) |
| idusuario (INT, NOT NULL, FK) | idusuario (INT, NOT NULL, FK) | idusuario (INT, NOT NULL, FK) |
| visiblecliente (CHAR(2), NULL) | es_interno (BIT, DEFAULT 0) | No existe |
| tipo (VARCHAR(20), NULL) | No existe | No existe |
| No existe | updated_at (DATETIME2, NULL) | No existe |
| No existe | created_by (NVARCHAR(50), NULL) | No existe |
| No existe | updated_by (NVARCHAR(50), NULL) | No existe |
| No existe | is_deleted (BIT, NOT NULL, DEFAULT 0) | No existe |

### Tabla de Archivos Adjuntos

| Sistema Legacy (PHP) | Sistema Nuevo (.NET) | InventarioAPI |
|---------------------|---------------------|--------------|
| **Nombre Tabla:** hd_archivos | **Nombre Tabla:** pro_adjuntos | **Nombre Tabla:** No existe |
| **Campos:** | **Campos:** | **Campos:** |
| idarchivo (INT, PK, IDENTITY) | idadjunto (INT, PK, IDENTITY) | No existe |
| idticket (INT, NOT NULL, FK) | idsolicitud (INT, NOT NULL, FK) | No existe |
| nombre_archivo (VARCHAR(255), NOT NULL) | nombre_archivo (NVARCHAR(255), NOT NULL) | No existe |
| ruta_archivo (VARCHAR(500), NOT NULL) | ruta_archivo (NVARCHAR(500), NOT NULL) | No existe |
| tamaño (BIGINT, NULL) | tamano (BIGINT, NOT NULL) | No existe |
| tipo_mime (VARCHAR(100), NULL) | tipo_contenido (NVARCHAR(100), NOT NULL) | No existe |
| idusuario (INT, NOT NULL, FK) | No existe | No existe |
| fecha (DATETIME, NULL) | created_at (DATETIME2, NOT NULL) | No existe |
| No existe | updated_at (DATETIME2, NULL) | No existe |
| No existe | created_by (NVARCHAR(50), NULL) | No existe |
| No existe | updated_by (NVARCHAR(50), NULL) | No existe |
| No existe | is_deleted (BIT, NOT NULL, DEFAULT 0) | No existe |

### Tabla de Sesiones

| Sistema Legacy (PHP) | Sistema Nuevo (.NET) | InventarioAPI |
|---------------------|---------------------|--------------|
| **Nombre Tabla:** sys_sessiones | **Nombre Tabla:** sys_sessiones | **Nombre Tabla:** sys_sessiones |
| **Campos:** | **Campos:** | **Campos:** |
| id (INT, PK, IDENTITY) | idsession (INT, PK, IDENTITY) | id (INT, PK, IDENTITY) |
| login (VARCHAR(50), NOT NULL) | login (NVARCHAR(50), NOT NULL) | login (VARCHAR(50), NOT NULL) |
| sid (VARCHAR(255), NOT NULL) | sid (NVARCHAR(255), NOT NULL) | sid (VARCHAR(255), NOT NULL) |
| fechainicio (DATETIME, NULL) | No existe | fechainicio (DATETIME, NULL) |
| fechaactividad (DATETIME, NULL) | No existe | fechaactividad (DATETIME, NULL) |
| ip_address (VARCHAR(45), NULL) | No existe | ip_address (VARCHAR(45), NULL) |
| user_agent (TEXT, NULL) | No existe | user_agent (TEXT, NULL) |
| No existe | idusuario (INT, NOT NULL, FK) | No existe |
| No existe | expira (DATETIME2, NOT NULL) | No existe |
| No existe | activo (BIT, DEFAULT 1) | No existe |
| No existe | created_at (DATETIME2, NOT NULL) | No existe |
| No existe | updated_at (DATETIME2, NULL) | No existe |
| No existe | created_by (NVARCHAR(50), NULL) | No existe |
| No existe | updated_by (NVARCHAR(50), NULL) | No existe |
| No existe | is_deleted (BIT, NOT NULL, DEFAULT 0) | No existe |

### Tabla de Pasos de Solicitud

| Sistema Legacy (PHP) | Sistema Nuevo (.NET) | InventarioAPI |
|---------------------|---------------------|--------------|
| **Nombre Tabla:** No existe | **Nombre Tabla:** pro_pasosolicitud | **Nombre Tabla:** pro_pasosolicitud |
| **Campos:** | **Campos:** | **Campos:** |
| No existe | idpaso (INT, PK, IDENTITY) | id (INT, PK, IDENTITY) |
| No existe | idsolicitud (INT, NOT NULL, FK) | idsolicitud (INT, NOT NULL, FK) |
| No existe | pasoproceso (NVARCHAR(100), NOT NULL) | pasoproceso (VARCHAR(255), NULL) |
| No existe | tipopaso (NVARCHAR(50), NULL) | idtipopaso (INT, NULL, FK) |
| No existe | orden (INT, NOT NULL) | orden (INT, NULL) |
| No existe | estado (NVARCHAR(20), DEFAULT 'Pending') | estado (VARCHAR(20), NULL) |
| No existe | realizadopor (INT, NULL, FK) | realizadopor (INT, NULL, FK) |
| No existe | idrol (INT, NULL, FK) | idrol (INT, NULL, FK) |
| No existe | fecha (DATETIME2, NULL) | fecha (DATETIME, NULL) |
| No existe | notas (NVARCHAR(500), NULL) | No existe |
| No existe | created_at (DATETIME2, NOT NULL) | No existe |
| No existe | updated_at (DATETIME2, NULL) | No existe |
| No existe | created_by (NVARCHAR(50), NULL) | No existe |
| No existe | updated_by (NVARCHAR(50), NULL) | No existe |
| No existe | is_deleted (BIT, NOT NULL, DEFAULT 0) | No existe |

### Tabla de Datos de Solicitud

| Sistema Legacy (PHP) | Sistema Nuevo (.NET) | InventarioAPI |
|---------------------|---------------------|--------------|
| **Nombre Tabla:** No existe | **Nombre Tabla:** pro_datossolicitud | **Nombre Tabla:** pro_datossolicitud |
| **Campos:** | **Campos:** | **Campos:** |
| No existe | iddato (INT, PK, IDENTITY) | id (INT, PK, IDENTITY) |
| No existe | idsolicitud (INT, NOT NULL, FK) | idsolicitud (INT, NOT NULL, FK) |
| No existe | nombre (NVARCHAR(100), NOT NULL) | nombre (VARCHAR(100), NULL) |
| No existe | dato (NVARCHAR(500), NOT NULL) | dato (TEXT, NULL) |
| No existe | tipo (NVARCHAR(50), NULL) | No existe |
| No existe | created_at (DATETIME2, NOT NULL) | No existe |
| No existe | updated_at (DATETIME2, NULL) | No existe |
| No existe | created_by (NVARCHAR(50), NULL) | No existe |
| No existe | updated_by (NVARCHAR(50), NULL) | No existe |
| No existe | is_deleted (BIT, NOT NULL, DEFAULT 0) | No existe |

### Tabla de Workflows (Nueva)

| Sistema Legacy (PHP) | Sistema Nuevo (.NET) | InventarioAPI |
|---------------------|---------------------|--------------|
| **Nombre Tabla:** No existe | **Nombre Tabla:** pro_workflow | **Nombre Tabla:** No existe |
| **Campos:** | **Campos:** | **Campos:** |
| No existe | idworkflow (INT, PK, IDENTITY) | No existe |
| No existe | nombre (NVARCHAR(100), NOT NULL) | No existe |
| No existe | descripcion (NVARCHAR(500), NULL) | No existe |
| No existe | categoria (NVARCHAR(100), NULL) | No existe |
| No existe | activo (BIT, NOT NULL, DEFAULT 1) | No existe |
| No existe | created_at (DATETIME2, NOT NULL) | No existe |
| No existe | updated_at (DATETIME2, NULL) | No existe |
| No existe | created_by (NVARCHAR(50), NULL) | No existe |
| No existe | updated_by (NVARCHAR(50), NULL) | No existe |
| No existe | is_deleted (BIT, NOT NULL, DEFAULT 0) | No existe |

### Tabla de Pasos de Workflow (Nueva)

| Sistema Legacy (PHP) | Sistema Nuevo (.NET) | InventarioAPI |
|---------------------|---------------------|--------------|
| **Nombre Tabla:** No existe | **Nombre Tabla:** pro_workflowstep | **Nombre Tabla:** No existe |
| **Campos:** | **Campos:** | **Campos:** |
| No existe | idstep (INT, PK, IDENTITY) | No existe |
| No existe | idworkflow (INT, NOT NULL, FK) | No existe |
| No existe | nombre_paso (NVARCHAR(100), NOT NULL) | No existe |
| No existe | tipo_paso (NVARCHAR(50), NULL) | No existe |
| No existe | orden (INT, NOT NULL) | No existe |
| No existe | idrol (INT, NULL, FK) | No existe |
| No existe | created_at (DATETIME2, NOT NULL) | No existe |
| No existe | updated_at (DATETIME2, NULL) | No existe |
| No existe | created_by (NVARCHAR(50), NULL) | No existe |
| No existe | updated_by (NVARCHAR(50), NULL) | No existe |
| No existe | is_deleted (BIT, NOT NULL, DEFAULT 0) | No existe |