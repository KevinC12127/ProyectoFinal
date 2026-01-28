using System;
using System.ComponentModel.DataAnnotations;

public class SyncLog
{
    public int Id { get; set; }

    [Required]
    public string Direction { get; set; } = string.Empty; // sql-to-mongo | mongo-to-sql | bidirectional

    public DateTime SyncedAt { get; set; } = DateTime.UtcNow;

    public string ConflictPolicy { get; set; } = "LastWriteWins";

    public int ItemsCreated { get; set; }

    public int ItemsUpdated { get; set; }

    public int ConflictsResolved { get; set; }
}
