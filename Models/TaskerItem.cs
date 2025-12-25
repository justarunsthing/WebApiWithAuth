using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace WebApiWithAuth.Models
{
    public class TaskerItem
    {
        public int Id { get; set; }
        public string? UserId { get; set; }

        [Required (ErrorMessage = "Every task must have a name")]
        public string? Name { get; set; }
        public bool IsComplete { get; set; } = false;

        // Foreign key relationship
        public virtual IdentityUser? User { get; set; }
    }
}