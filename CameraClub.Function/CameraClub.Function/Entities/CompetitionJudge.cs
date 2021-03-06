﻿using System.ComponentModel.DataAnnotations.Schema;

namespace CameraClub.Function.Entities
{
    [Table("CompetitionJudge", Schema = "competition")]
    public class CompetitionJudge
    {
        public int CompetitionId { get; set; }

        public int JudgeId { get; set; }

        public Competition Competition { get; set; }

        public Judge Judge { get; set; }
    }
}