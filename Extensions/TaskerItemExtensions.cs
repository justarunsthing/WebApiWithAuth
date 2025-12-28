using WebApiWithAuth.Models;

namespace WebApiWithAuth.Extensions
{
    public static class TaskerItemExtensions
    {
        public static TaskerItemDto ToDTO(this TaskerItem item ) => new()
        {
            Id = item.Id,
            Name = item.Name,
            IsComplete = item.IsComplete
        };
    }
}