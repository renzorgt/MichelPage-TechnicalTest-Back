using System.ComponentModel.DataAnnotations;

namespace MichelPage_TechnicalTest_Back.Dtos.TaskDtos
{
    public class TaskUpdateStatusDto
    {
 
        public int Id { get; set; } 

        public string Status { get; set; }
    }
}
