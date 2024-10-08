﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnlineSurveyTool.Server.DAL.Models
{
    [Table("Surveys")]
    public class Survey : EntityBaseStringId
    {
        [Required]
        [ForeignKey("Owner")]
        public int OwnerId { get; set; }

        [Required(AllowEmptyStrings = false)]
        [StringLength(100, MinimumLength = 1)]
        public string Name { get; set; }

        public DateTime? OpeningDate { get; set; }

        public DateTime? ClosingDate { get; set; }

        [Required]
        public bool IsOpen { get; set; }

        [Required]
        public bool IsArchived { get; set; }

        public virtual User Owner { get; set; }

        public ICollection<Question> Questions { get; set; }
        public virtual ICollection<SurveyResult> Results { get; set; }

    }
}
