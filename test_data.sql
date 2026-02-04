-- Ejecuta este script en pgAdmin (Query Tool) para insertar datos de prueba

-- Insertar pacientes
INSERT INTO paciente (nombre, cedula, telefono) VALUES
('Juan Pérez', '1234567890', '555-0101'),
('María García', '0987654321', '555-0102'),
('Carlos López', '1122334455', '555-0103');

-- Insertar médicos
INSERT INTO medico (nombre, especialidad) VALUES
('Dr. Roberto Sánchez', 'Cardiología'),
('Dra. Ana Martínez', 'Pediatría'),
('Dr. Luis Fernández', 'Medicina General');

-- Insertar turnos
INSERT INTO turno (paciente_id, medico_id, fecha, hora, estado, ultima_actualizacion) VALUES
(1, 1, '2026-02-10', '09:00:00', 'pendiente', NOW()),
(2, 2, '2026-02-10', '10:30:00', 'confirmado', NOW()),
(3, 3, '2026-02-11', '14:00:00', 'pendiente', NOW());

-- Verificar que se insertaron correctamente
SELECT 'PACIENTES:' as tabla;
SELECT * FROM paciente;

SELECT 'MEDICOS:' as tabla;
SELECT * FROM medico;

SELECT 'TURNOS:' as tabla;
SELECT * FROM turno;
