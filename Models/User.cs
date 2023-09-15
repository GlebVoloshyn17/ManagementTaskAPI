using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ManagementTaskAPI.Models
{
    public class User
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        [StringLength(255)]
        public string Username { get; set; }

        public ICollection<TaskList> TaskLists { get; set; }

        public ICollection<Task> Tasks { get; set; }
    }
}
