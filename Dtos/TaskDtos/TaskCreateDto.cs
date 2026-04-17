using System.ComponentModel.DataAnnotations;

namespace MichelPage_TechnicalTest_Back.Dtos.TaskDtos
{
    public class TaskCreateDto
    {

        public string Titulo { get; set; }

        public int UserId { get; set; } 
        public string Informacion { get; set; }

        public int UserIdCrea { get; set; }


    }
}
