using System.ComponentModel.DataAnnotations;

namespace MichelPage_TechnicalTest_Back.Dtos.TaskDtos
{
    public class TaskCreateDto
    {
        [Required(ErrorMessage = "El Titulo es  obligatorio")]
        public string Titulo { get; set; }
        [Required(ErrorMessage = "El UserId es obligatorio")]
        [Range(1, int.MaxValue, ErrorMessage = "El UserId debe ser mayor a 0")]
        public int UserId { get; set; }

        [Required(ErrorMessage = "La Información es obligatoria")]
        public string Informacion { get; set; }

        [Required(ErrorMessage = "El UserIdCrea es obligatorio")]
        [Range(1, int.MaxValue, ErrorMessage = "El UserIdCrea debe ser mayor a 0")]
        public int UserIdCrea { get; set; }


    }
}
