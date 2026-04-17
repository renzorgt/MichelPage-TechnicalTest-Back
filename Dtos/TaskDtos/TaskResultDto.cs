namespace MichelPage_TechnicalTest_Back.Dtos.TaskDtos
{
    public class TaskResultDto
    {
        public int Id { get; set; }
        public string Titulo { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string Status { get; set; }
        public string Informacion { get; set; }
        public string Prioridad { get; set; }
        public DateTime FechaEstimada { get; set; }
        public string Descripcion { get; set; }
        public DateTime FechaCreacion { get; set; }
        public DateTime FechaModificacion { get; set; }
    }
}
