using System.ComponentModel.DataAnnotations;

namespace MichelPage_TechnicalTest_Back.Dtos.TaskDtos
{
    public class TaskUpdateDto
    {
 
        public int Id { get; set; } 

  
        public string Titulo { get; set; }

     
        public int UserId { get; set; }

     
        public string Status { get; set; }

        public string Informacion { get; set; }


        public int UserIdMod { get; set; }
    }
}
