﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace SulsApp.Models
{
    public class Submission
    {
        public Submission()
        {
            this.Id = Guid.NewGuid().ToString();
        }

        [Key]
        public string Id { get; set; }

        [Required, MinLength(30), MaxLength(800)]
        public string Code { get; set; }

        public int AchievedResult { get; set; }

        [Required]
        public DateTime CreatedOn { get; set; }

        public string ProblemId { get; set; }
        public virtual Problem Problem { get; set; }

        public string UserId { get; set; }
        public virtual User User { get; set; }
    }
}
