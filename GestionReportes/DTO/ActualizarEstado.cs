﻿namespace GestionReportes.DTO
{
    public class ActualizarEstado
    {
        public int ReporteId { get; set; }
        public int EstadoId { get; set; }
        public int FuncionarioId { get; set; }
        public string Observacion { get; set; }
    }
}
