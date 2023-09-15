using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ManagementTaskAPI.Models
{
    public class Task
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        [StringLength(255)]
        public string Title { get; set; }

        public string Description { get; set; }

        public DateTime DueDate { get; set; }

        public bool IsCompleted { get; set; }

        [ForeignKey("TaskListId")]
        public Guid TaskListId { get; set; }

        public TaskList TaskList { get; set; }

        [ForeignKey("UserId")]
        public Guid UserId { get; set; }

        public User User { get; set; }
    }
}
