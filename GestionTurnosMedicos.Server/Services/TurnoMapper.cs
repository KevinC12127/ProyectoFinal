using System;

public static class TurnoMapper
{
    // Regla explícita de transformación relacional -> documento
    public static TurnoDocument ToDocument(Turno entity)
    {
        return new TurnoDocument
        {
            SyncKey = entity.SyncKey,
            NombrePaciente = entity.NombrePaciente,
            Cedula = entity.Cedula,
            Especialidad = entity.Especialidad,
            Fecha = entity.Fecha,
            Hora = entity.Hora,
            FechaRegistro = entity.FechaRegistro,
            UpdatedAt = entity.UpdatedAt
        };
    }

    // Regla explícita de transformación documento -> relacional
    public static Turno ToEntity(TurnoDocument doc)
    {
        return new Turno
        {
            SyncKey = doc.SyncKey,
            NombrePaciente = doc.NombrePaciente,
            Cedula = doc.Cedula,
            Especialidad = doc.Especialidad,
            Fecha = doc.Fecha,
            Hora = doc.Hora,
            FechaRegistro = doc.FechaRegistro,
            UpdatedAt = doc.UpdatedAt
        };
    }

    public static string BuildSyncKey(string cedula, DateTime fecha, string hora, string especialidad)
    {
        return $"{cedula}|{fecha:yyyy-MM-dd}|{hora}|{especialidad}";
    }
}
