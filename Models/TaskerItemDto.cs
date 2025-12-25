using System.ComponentModel.DataAnnotations;

namespace WebApiWithAuth.Models
{
    public class TaskerItemDto
    {
        public int Id { get; set; }

        [Required (ErrorMessage = "Every task must have a name")]
        public string? Name { get; set; }
        public bool IsComplete { get; set; }
    }
}