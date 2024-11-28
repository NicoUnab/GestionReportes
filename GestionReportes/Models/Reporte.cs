namespace GestionReportes.Models
{
    public class Reporte
    {
        public int Id { get; set; }
        public string Descripcion { get; set; }
        public string Ubicacion { get; set; }
        public DateTime FechaCreacion { get; set; }
        public string Imagen { get; set; }

        // Relaciones
        public int idVecino { get; set; }
        public Vecino Vecino { get; set; }
        public int idEstado { get; set; }
        public EstadoReporte Estado { get; set; }
        public int idTipo { get; set; }
        public TipoReporte Tipo { get; set; }
    }
}
