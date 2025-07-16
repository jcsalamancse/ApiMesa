## Tablas Adicionales en InventarioAPI (No Migradas al Sistema .NET)

### Tablas de Capacitación

#### Tabla cap_curso

| Campo | Tipo | Nulo | Clave | Descripción |
|-------|------|------|-------|------------|
| idcurso | INT | No | PK | Identificador del curso |
| curso | VARCHAR(255) | No | | Nombre del curso |
| descripcion | TEXT | Sí | | Descripción del curso |
| tipo | VARCHAR(20) | Sí | | Tipo de curso |
| alcance | VARCHAR(20) | Sí | | Alcance del curso |
| activo | CHAR(2) | Sí | | Estado del curso |
| fechacreacion | DATETIME | Sí | | Fecha de creación |

#### Tabla cap_actividad

| Campo | Tipo | Nulo | Clave | Descripción |
|-------|------|------|-------|------------|
| idactividad | INT | No | PK | Identificador de la actividad |
| idcurso | INT | No | FK | Curso al que pertenece |
| actividad | VARCHAR(255) | No | | Nombre de la actividad |
| descripcion | TEXT | Sí | | Descripción de la actividad |
| orden | INT | Sí | | Orden de la actividad |
| activo | CHAR(2) | Sí | | Estado de la actividad |
| fechacreacion | DATETIME | Sí | | Fecha de creación |

#### Tabla cap_evaluacion

| Campo | Tipo | Nulo | Clave | Descripción |
|-------|------|------|-------|------------|
| idevaluacion | INT | No | PK | Identificador de la evaluación |
| idactividad | INT | No | FK | Actividad a la que pertenece |
| evaluacion | VARCHAR(255) | No | | Nombre de la evaluación |
| descripcion | TEXT | Sí | | Descripción de la evaluación |
| activo | CHAR(2) | Sí | | Estado de la evaluación |
| fechacreacion | DATETIME | Sí | | Fecha de creación |

#### Tabla cap_evaluacion_usuario

| Campo | Tipo | Nulo | Clave | Descripción |
|-------|------|------|-------|------------|
| id | INT | No | PK | Identificador del registro |
| idevaluacion | INT | No | FK | Evaluación realizada |
| idusuario | INT | No | FK | Usuario que realizó la evaluación |
| calificacion | DECIMAL(5,2) | Sí | | Calificación obtenida |
| estado | VARCHAR(20) | Sí | | Estado de la evaluación |
| fecha_inicio | DATETIME | Sí | | Fecha de inicio |
| fecha_fin | DATETIME | Sí | | Fecha de finalización |
| fechacreacion | DATETIME | Sí | | Fecha de creación del registro |

### Tablas de Monitoreo de Servidores

#### Tabla hd_servidor

| Campo | Tipo | Nulo | Clave | Descripción |
|-------|------|------|-------|------------|
| idservidor | INT | No | PK | Identificador del servidor |
| Servidor | VARCHAR(100) | No | | Nombre del servidor |
| cpu | DECIMAL(5,2) | Sí | | Uso de CPU |
| ram | DECIMAL(5,2) | Sí | | Uso de RAM |
| totalRAM | DECIMAL(9,2) | Sí | | Total de RAM |
| usoRAM | DECIMAL(9,2) | Sí | | RAM en uso |
| dispoRAM | DECIMAL(9,2) | Sí | | RAM disponible |
| fechahoraServidor | DATETIME | Sí | | Fecha y hora del servidor |
| fechahora | DATETIME | Sí | | Fecha y hora del registro |
| activo | CHAR(2) | Sí | | Estado del servidor |

#### Tabla hd_discos_servidor

| Campo | Tipo | Nulo | Clave | Descripción |
|-------|------|------|-------|------------|
| id | INT | No | PK | Identificador del registro |
| idservidor | INT | No | FK | Servidor al que pertenece |
| drive | VARCHAR(10) | Sí | | Letra de unidad |
| hdd | DECIMAL(5,2) | Sí | | Uso de disco |
| totalHDD | DECIMAL(15,2) | Sí | | Tamaño total del disco |
| usoHDD | DECIMAL(15,2) | Sí | | Espacio usado |
| dispoHDD | DECIMAL(15,2) | Sí | | Espacio disponible |
| WriteHDD | DECIMAL(15,2) | Sí | | Velocidad de escritura |
| ReadHDD | DECIMAL(15,2) | Sí | | Velocidad de lectura |
| fechahoraServidor | DATETIME | Sí | | Fecha y hora del servidor |
| fechahora | DATETIME | Sí | | Fecha y hora del registro |

#### Tabla hd_servicios_servidor

| Campo | Tipo | Nulo | Clave | Descripción |
|-------|------|------|-------|------------|
| id | INT | No | PK | Identificador del registro |
| idservidor | INT | No | FK | Servidor al que pertenece |
| servicio | VARCHAR(255) | Sí | | Nombre del servicio |
| estado | VARCHAR(50) | Sí | | Estado del servicio |
| tipo | VARCHAR(10) | Sí | | Tipo de servicio |
| fechahoraServidor | DATETIME | Sí | | Fecha y hora del servidor |
| fechahora | DATETIME | Sí | | Fecha y hora del registro |

### Otras Tablas del Sistema

#### Tabla pro_tipopaso

| Campo | Tipo | Nulo | Clave | Descripción |
|-------|------|------|-------|------------|
| idtipopaso | INT | No | PK | Identificador del tipo de paso |
| tipopaso | VARCHAR(100) | No | | Nombre del tipo de paso |
| descripcion | TEXT | Sí | | Descripción del tipo de paso |
| activo | CHAR(2) | Sí | | Estado del tipo de paso |

#### Tabla pro_accion

| Campo | Tipo | Nulo | Clave | Descripción |
|-------|------|------|-------|------------|
| id | INT | No | PK | Identificador de la acción |
| idrol | INT | No | FK | Rol asociado |
| idusuario | INT | No | FK | Usuario asociado |
| idcurso | INT | Sí | | Curso asociado (opcional) |
| fechaasignacion | DATETIME | Sí | | Fecha de asignación |

#### Tabla sys_parametros

| Campo | Tipo | Nulo | Clave | Descripción |
|-------|------|------|-------|------------|
| id | INT | No | PK | Identificador del parámetro |
| parametro | VARCHAR(100) | No | | Nombre del parámetro |
| valor | TEXT | Sí | | Valor del parámetro |
| descripcion | TEXT | Sí | | Descripción del parámetro |
| gruposoportenivel1 | INT | Sí | | Grupo de soporte nivel 1 |
| fechamodificacion | DATETIME | Sí | | Fecha de modificación |

#### Tabla sys_logtrans

| Campo | Tipo | Nulo | Clave | Descripción |
|-------|------|------|-------|------------|
| id | INT | No | PK | Identificador del log |
| fecha | DATETIME | Sí | | Fecha del log |
| accion | TEXT | No | | Acción registrada |
| idusuario | INT | Sí | FK | Usuario que realizó la acción |
| ip_address | VARCHAR(45) | Sí | | Dirección IP |

## Análisis de la Migración

### Tablas Migradas Correctamente
- **sys_usuarios**: Presente en todos los sistemas con estructura similar
- **sys_sessiones**: Presente en todos los sistemas con estructura similar
- **pro_comentarios/hd_comentarios**: Presente en todos los sistemas con estructura similar

### Tablas con Cambios de Nombre
- **sys_rol** (Legacy/InventarioAPI) → **pro_rol** (.NET)
- **hd_ticket** (Legacy/InventarioAPI) → **pro_solicitud** (.NET)
- **hd_archivos** (Legacy) → **pro_adjuntos** (.NET)

### Tablas Nuevas en el Sistema .NET
- **pro_workflow**: Nueva tabla para definir workflows
- **pro_workflowstep**: Nueva tabla para definir pasos de workflow

### Tablas No Migradas al Sistema .NET
- **Módulo de Capacitación**: cap_curso, cap_actividad, cap_evaluacion, cap_evaluacion_usuario
- **Módulo de Monitoreo**: hd_servidor, hd_discos_servidor, hd_servicios_servidor, hd_servidor_historico, hd_discos_servidor_historico, hd_servicios_servidor_historico
- **Tablas de Configuración**: pro_tipopaso, pro_accion, sys_parametros
- **Tablas de Auditoría**: sys_logtrans

## Mejoras en la Estructura de Base de Datos

### Mejoras Generales
1. **Campos de Auditoría**: El sistema .NET añade campos de auditoría consistentes (created_at, updated_at, created_by, updated_by, is_deleted)
2. **Eliminación Lógica**: Implementación de borrado lógico con el campo is_deleted
3. **Tipos de Datos**: Uso de NVARCHAR en lugar de VARCHAR para mejor soporte de caracteres Unicode
4. **Restricciones**: Mayor uso de restricciones NOT NULL y valores DEFAULT
5. **Integridad Referencial**: Mejor definición de relaciones entre tablas

### Nuevas Funcionalidades
1. **Sistema de Workflows**: Nuevas tablas para definir y gestionar workflows personalizados
2. **Pasos de Workflow**: Capacidad para definir secuencias de pasos con roles asignados
3. **Datos Adicionales**: Estructura flexible para almacenar datos personalizados por solicitud

## Recomendaciones para Completar la Migración

1. **Módulos Pendientes**: Evaluar si los módulos no migrados (capacitación, monitoreo) son necesarios en el nuevo sistema
2. **Migración de Datos**: Desarrollar scripts ETL para transferir datos entre sistemas
3. **Validación de Datos**: Implementar validaciones para garantizar la integridad durante la migración
4. **Migración por Fases**: Considerar una migración por fases, comenzando con datos maestros (usuarios, roles)
5. **Pruebas de Verificación**: Realizar pruebas exhaustivas para verificar la correcta migración de datos

## Próximos Pasos

1. **Decidir sobre Módulos Pendientes**: Determinar si se migrarán los módulos de capacitación y monitoreo
2. **Desarrollar Scripts de Migración**: Crear scripts SQL para la transferencia de datos
3. **Ejecutar Migración en Ambiente de Pruebas**: Validar el proceso en un entorno controlado
4. **Planificar Migración a Producción**: Establecer un plan detallado para la migración final