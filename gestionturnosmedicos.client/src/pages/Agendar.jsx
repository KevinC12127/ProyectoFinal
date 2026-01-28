import { useState, useEffect } from 'react';
import { toast } from 'react-toastify';

export default function Agendar() {
    const [form, setForm] = useState({
        nombrePaciente: '',
        cedula: '',
        especialidad: '',
        fecha: '',
        hora: ''
    });

    const [loading, setLoading] = useState(false);
    const [citaConfirmada, setCitaConfirmada] = useState(null);

    const especialidades = [
        'Cardiología',
        'Pediatría',
        'Dermatología',
        'Ortopedia',
        'Ginecología'
    ];

    useEffect(() => {
        const citaGuardada = localStorage.getItem('citaAgendada');
        if (citaGuardada) setCitaConfirmada(JSON.parse(citaGuardada));
    }, []);

    const handleChange = (e) => {
        setForm({ ...form, [e.target.name]: e.target.value });
    };

    const handleSubmit = async (e) => {
        e.preventDefault();
        setLoading(true);

        const cita = {
            ...form,
            fechaRegistro: new Date().toISOString()
        };

        localStorage.setItem('citaAgendada', JSON.stringify(cita));
        setCitaConfirmada(cita);
        toast.success('✅ Cita médica agendada correctamente');

        setForm({
            nombrePaciente: '',
            cedula: '',
            especialidad: '',
            fecha: '',
            hora: ''
        });
        setLoading(false);
    };

    return (
        <div className="container" style={{ padding: '30px', maxWidth: '600px', margin: '0 auto' }}>
            <h2 style={{ textAlign: 'center', marginBottom: '20px' }}>Agendar Cita Médica</h2>

            <form onSubmit={handleSubmit} style={{ display: 'flex', flexDirection: 'column', gap: '12px' }}>
                <input name="nombrePaciente" placeholder="Nombre completo" value={form.nombrePaciente} onChange={handleChange} required />
                <input name="cedula" placeholder="Cédula" value={form.cedula} onChange={handleChange} required />
                <select name="especialidad" value={form.especialidad} onChange={handleChange} required>
                    <option value="">Seleccione una especialidad</option>
                    {especialidades.map((esp, i) => (
                        <option key={i} value={esp}>{esp}</option>
                    ))}
                </select>
                <input name="fecha" type="date" value={form.fecha} onChange={handleChange} required />
                <input name="hora" type="time" value={form.hora} onChange={handleChange} required />
                <button type="submit" disabled={loading} style={{ padding: '10px' }}>
                    {loading ? 'Agendando...' : 'Agendar'}
                </button>
            </form>

            {citaConfirmada && (
                <div style={{ marginTop: '40px', backgroundColor: '#f5f5f5', padding: '20px', borderRadius: '8px' }}>
                    <h3>📋 Cita médica confirmada</h3>
                    <p><strong>Paciente:</strong> {citaConfirmada.nombrePaciente}</p>
                    <p><strong>Cédula:</strong> {citaConfirmada.cedula}</p>
                    <p><strong>Especialidad:</strong> {citaConfirmada.especialidad}</p>
                    <p><strong>Fecha:</strong> {citaConfirmada.fecha}</p>
                    <p><strong>Hora:</strong> {citaConfirmada.hora}</p>
                    <p><strong>Fecha de registro:</strong> {new Date(citaConfirmada.fechaRegistro).toLocaleString()}</p>
                </div>
            )}
        </div>
    );
}


//hola