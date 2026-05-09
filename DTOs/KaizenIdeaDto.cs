namespace Kaizen.DTOs
{
    public class KaizenIdeaDto
    {
        public int NoEmpleado { get; set; }
        public string Supervisor { get; set; }
        public string Area { get; set; }
        public int Turno { get; set; }
        public string AreaAplicacion { get; set; }
        public string Descripcion { get; set; }

        // Estas son las imágenes del boceto (Fabric.js)
        public string ImagenActualBase64 { get; set; }
        public string ImagenMejoradaBase64 { get; set; }
    }
}