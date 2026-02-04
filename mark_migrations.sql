-- Marcar migraciones como aplicadas en la BD
INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion") 
VALUES ('20260204034022_InitialCreate', '9.0.6')
ON CONFLICT DO NOTHING;

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion") 
VALUES ('20260204035145_AddSyncLogsTable', '9.0.6')
ON CONFLICT DO NOTHING;
