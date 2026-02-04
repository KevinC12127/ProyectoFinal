using System;

public static class TurnoMapper
{
    // Regla explícita de transformación relacional -> documento
    public static TurnoDocument ToDocument(Turno entity)
    {
        return new TurnoDocument
        {
            SyncKey = entity.SyncKey,
            PacienteId = entity.PacienteId,
            MedicoId = entity.MedicoId,
            Fecha = entity.Fecha,
            Hora = entity.Hora,
            Estado = entity.Estado,
            UpdatedAt = entity.UltimaActualizacion
        };
    }

    // Regla explícita de transformación documento -> relacional
    public static Turno ToEntity(TurnoDocument doc)
    {
        return new Turno
        {
            SyncKey = doc.SyncKey,
            PacienteId = doc.PacienteId,
            MedicoId = doc.MedicoId,
            Fecha = doc.Fecha,
            Hora = doc.Hora,
            Estado = doc.Estado,
            UltimaActualizacion = doc.UpdatedAt,
            UpdatedAt = doc.UpdatedAt
        };
    }

    public static string BuildSyncKey(int pacienteId, int medicoId, DateTime fecha)
    {
        return $"{pacienteId}|{medicoId}|{fecha:yyyy-MM-dd}";    }
}