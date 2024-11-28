namespace GestionReportes.Models
{
    public class Documento
    {
        public int id { get; set; }
        public string nombreArchivo { get; set; } // Nombre del archivo en el servidor
        public string ruta { get; set; } // Ruta completa del archivo
        public DateTime fechaSubida { get; set; } // Fecha de subida del archivo
    }
}
