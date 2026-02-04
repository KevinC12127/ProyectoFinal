#!/usr/bin/env dotnet script

#r "nuget: Npgsql, 8.0.0"

using Npgsql;

var connectionString = "Host=localhost;Port=5432;Database=agenda_turnos_medicos;Username=postgres;Password=1234";

try
{
    using var conn = new NpgsqlConnection(connectionString);
    conn.Open();
    Console.WriteLine("✓ Connected to PostgreSQL");

    // Insert test data
    using var cmd = conn.CreateCommand();
    cmd.CommandText = "INSERT INTO turno (paciente_id, medico_id, fecha, hora, estado, ultima_actualizacion) VALUES (@p, @m, @f, @h, @e, NOW()) RETURNING id";
    cmd.Parameters.AddWithValue("@p", 1);
    cmd.Parameters.AddWithValue("@m", 1);
    cmd.Parameters.AddWithValue("@f", new DateTime(2026, 2, 5));
    cmd.Parameters.AddWithValue("@h", "10:00:00");
    cmd.Parameters.AddWithValue("@e", "pendiente");

    var id = (int?)cmd.ExecuteScalar();
    Console.WriteLine($"✓ Inserted turno with ID: {id}");

    // Read all turnos
    cmd.CommandText = "SELECT id, paciente_id, medico_id, fecha, hora, estado, ultima_actualizacion FROM turno ORDER BY id DESC LIMIT 5";
    cmd.Parameters.Clear();
    
    using var reader = cmd.ExecuteReader();
    Console.WriteLine("\nRecent turnos:");
    while (reader.Read())
    {
        Console.WriteLine($"  ID: {reader[0]}, Paciente: {reader[1]}, Medico: {reader[2]}, Fecha: {reader[3]}, Hora: {reader[4]}, Estado: {reader[5]}");
    }
}
catch (Exception ex)
{
    Console.WriteLine($"✗ Error: {ex.Message}");
}
