-- Más datos de prueba para PostgreSQL

INSERT INTO paciente (nombre, cedula, telefono) VALUES
('Valeria Cruz', '0956789012', '555-0301'),
('Miguel Ortega', '0967890123', '555-0302'),
('Paula Núñez', '0978901234', '555-0303'),
('Javier Molina', '0989012345', '555-0304'),
('Lucía Herrera', '0990123456', '555-0305');

INSERT INTO medico (nombre, especialidad) VALUES
('Dr. Andrés Salazar', 'Ginecología'),
('Dra. Elena Pardo', 'Oftalmología'),
('Dr. Sergio Rivas', 'Otorrinolaringología'),
('Dra. Paula León', 'Endocrinología');

INSERT INTO turno (paciente_id, medico_id, fecha, hora, estado, ultima_actualizacion) VALUES
(1, 1, '2026-02-18', '08:00:00', 'pendiente', NOW()),
(2, 2, '2026-02-18', '09:30:00', 'confirmado', NOW()),
(3, 3, '2026-02-18', '11:00:00', 'pendiente', NOW()),
(4, 4, '2026-02-19', '10:15:00', 'pendiente', NOW()),
(5, 1, '2026-02-19', '12:45:00', 'confirmado', NOW()),
(2, 3, '2026-02-20', '14:00:00', 'pendiente', NOW()),
(3, 2, '2026-02-20', '15:30:00', 'pendiente', NOW()),
(4, 1, '2026-02-21', '09:00:00', 'confirmado', NOW());

-- Verificación rápida
SELECT COUNT(*) AS pacientes_total FROM paciente;
SELECT COUNT(*) AS medicos_total FROM medico;
SELECT COUNT(*) AS turnos_total FROM turno;
