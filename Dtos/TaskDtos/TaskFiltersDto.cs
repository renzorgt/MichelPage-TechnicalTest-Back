namespace MichelPage_TechnicalTest_Back.Dtos.TaskDtos
{
    public class TaskFiltersDto
    {
        
        public int? UserId { get; set; }
        public string? Status { get; set; }
        public string? Prioridad { get; set; }
        public DateTime FechaEstimada { get; set; }
    }
}
