using System.ComponentModel.DataAnnotations;

namespace MichelPage_TechnicalTest_Back.Dtos.TaskDtos
{
    public class TaskUpdateDto
    {
        [Required]
        public int Id { get; set; } 

        [Required]
        [StringLength(200)]
        public string Titulo { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required]
        [RegularExpression("^(Pending|InProgress|Done)$", ErrorMessage = "Estado no válido.")]
        public string Status { get; set; }

        public string Informacion { get; set; }
    }
}
