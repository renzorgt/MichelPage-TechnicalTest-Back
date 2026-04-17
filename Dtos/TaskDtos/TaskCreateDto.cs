using System.ComponentModel.DataAnnotations;

namespace MichelPage_TechnicalTest_Back.Dtos.TaskDtos
{
    public class TaskCreateDto
    {
        [Required]
        [StringLength(200)]
        public string Titulo { get; set; }

        [Required]
        public int UserId { get; set; } 
        public string Informacion { get; set; }
    }
}
