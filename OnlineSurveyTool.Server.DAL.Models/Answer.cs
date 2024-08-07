﻿using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnlineSurveyTool.Server.DAL.Models
{
    public class Answer : EntityBaseIntegerId
    {
        public QuestionType Type { get; set; }

        [ForeignKey(nameof(Question))]
        public string QuestionId { get; set; }

        [ForeignKey(nameof(SurveyResultId))]
        public int SurveyResultId { get; set; }

        [ForeignKey(nameof(SingleChoiceOption))]
        public string? SingleChoiceOptionId { get; set; }

        public virtual SurveyResult? SurveyResult { get; set; }
        public virtual ChoiceOption? SingleChoiceOption { get; set; }
        public IEnumerable<AnswerOption> AnswerOptions { get; set; }
        public virtual Question Question { get; set; }


        [NotMapped]
        public IEnumerable<ChoiceOption> ChoiceOptions
        {
            get => AnswerOptions?.Select(ao => ao.ChoiceOption);
        }
    }
}
