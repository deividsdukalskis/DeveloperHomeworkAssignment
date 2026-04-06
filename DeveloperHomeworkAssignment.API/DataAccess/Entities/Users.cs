namespace DeveloperHomeworkAssignment.API.DataAccess.Entities
{
    using System.ComponentModel.DataAnnotations;

    public class Users
    {
        [Key]
        public Guid Id { get; set; }
        public Profiles Profile { get; set; } = null!;
        public string Username { get; set; } = null!;
        public string Email { get; set; } = null!;
    }
}