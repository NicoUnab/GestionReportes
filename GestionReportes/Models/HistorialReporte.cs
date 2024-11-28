namespace GestionReportes.Models
{
    public class HistorialReporte
    {
        public int Id { get; set; }
        public int idReporte { get; set; }
        public int idFuncionario { get; set; }
        public DateTime fecha { get; set; }
        public string observacion { get; set; }

        // Relaciones
        public Reporte Reporte { get; set; }
        public FuncionarioMunicipal Funcionario { get; set; }
    }
}
