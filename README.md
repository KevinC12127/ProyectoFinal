# Gestión de Turnos Médicos

## Resumen
Este proyecto integra un backend en **ASP.NET Core (.NET 8)** con **PostgreSQL** y **MongoDB** para gestionar turnos médicos y sincronizar datos entre una base relacional y una documental.

Se implementó y validó la sincronización **SQL → Mongo**, **Mongo → SQL** y **Bidireccional** con política **LastWriteWins** (gana la fecha más reciente).

---

## Tecnologías utilizadas
- **Backend:** ASP.NET Core (.NET 8)
- **ORM:** Entity Framework Core + Npgsql
- **Base de datos relacional:** PostgreSQL
- **Base de datos documental:** MongoDB
- **Frontend (dev):** React + Vite (solo para desarrollo)
- **Swagger/OpenAPI:** Documentación y pruebas de endpoints

---

## Estructura relevante
```
GestionTurnosMedicos.Server/
  Controllers/
    SyncController.cs
    TurnosController.cs
    MedicosController.cs
    PacientesController.cs
  Data/
    TurnosContext.cs
  Models/
    Turno.cs
    TurnoDocument.cs
    SyncLog.cs
    Medico.cs
    Paciente.cs
    DetalleTurno.cs
  Services/
    TurnoSyncService.cs
    TurnoMapper.cs
    MongoTurnoRepository.cs
    IMongoTurnoRepository.cs
```

---

## Cambios y ajustes realizados

### 1) Modelos alineados a PostgreSQL
Se actualizó el modelo `Turno` para coincidir con la estructura real de la tabla en PostgreSQL:
- `paciente_id`
- `medico_id`
- `fecha`
- `hora`
- `estado`
- `ultima_actualizacion`

Además:
- `hora` se mapea como **TimeSpan** (tipo `time` en PostgreSQL).
- Se marcaron `SyncKey` y `UpdatedAt` como **NotMapped** (no existen en SQL).

### 2) Nuevos modelos
Se crearon los modelos:
- `Medico` (id, nombre, especialidad)
- `Paciente` (id, nombre, cedula, telefono)
- `DetalleTurno` (colección Mongo `detalle_turno`)

### 3) Nuevos controladores
Se agregaron controladores CRUD:
- `MedicosController`
- `PacientesController`

### 4) Repositorio Mongo
Se incluyó el acceso a la colección `detalle_turno`:
- `MongoTurnoRepository.GetAllDetalleAsync()`

### 5) Sincronización
Se ajustó la sincronización para:
- Usar `ultima_actualizacion` de SQL como referencia de actualización.
- Sincronizar **Mongo → SQL** desde la colección `detalle_turno` usando `turno_id`.

### 6) Endpoints de sincronización
- `POST /api/sync/sql-to-mongo`
- `POST /api/sync/mongo-to-sql`
- `POST /api/sync/bidirectional`
- `POST /api/sync/mongo-a-postgres` (alias del anterior)
- `GET /api/sync/logs`

---

## Pruebas rápidas (Swagger)
URL: `http://localhost:5252/swagger`

### Insertar datos en PostgreSQL
Ejecuta en pgAdmin:

```
INSERT INTO paciente (nombre, cedula, telefono) VALUES
('Juan Pérez', '1234567890', '555-0101');

INSERT INTO medico (nombre, especialidad) VALUES
('Dr. Roberto Sánchez', 'Cardiología');

INSERT INTO turno (paciente_id, medico_id, fecha, hora, estado, ultima_actualizacion) VALUES
(1, 1, '2026-02-10', '09:00:00', 'pendiente', NOW());
```

### Probar sincronización
- `POST /api/sync/sql-to-mongo`
- `POST /api/sync/mongo-to-sql`
- `POST /api/sync/bidirectional`

---

## Colecciones MongoDB usadas
- **turnos** (colección principal de turnos)
- **detalle_turno** (detalle sincronizado desde Mongo → SQL)

---

## Notas
- **LastWriteWins**: si hay conflicto, gana el registro con fecha más reciente.
- Para probar **Mongo → SQL**, actualiza en MongoDB:
  - `estado`
  - `ultima_actualizacion = new Date()`

---

## Estado actual
✅ SQL → Mongo funcionando
✅ Mongo → SQL funcionando
✅ Bidireccional funcionando
