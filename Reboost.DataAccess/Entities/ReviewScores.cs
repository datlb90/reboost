using System;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;

namespace Reboost.DataAccess.Entities
{
    public class ReviewScores : BaseEntity
    {
        public int ReviewId { get; set; }
        // Overall score
        public decimal? OverallBandScore { get; set; }

        // 4 Cretieria score
        public decimal? TaskAchievementScore { get; set; }
        public decimal? TaskResponseScore { get; set; }
        public decimal? CoherenceScore { get; set; }
        public decimal? LexicalResourceScore { get; set; }
        public decimal? GrammarScore { get; set; }

        // Task Achievement scores
        public decimal? FulfillRequirements { get; set; }
        public decimal? HighlightKeyFeatures { get; set; }
        public decimal? CompareAndContrast { get; set; }
        public decimal? DataSelection { get; set; }

        // Task Response scores
        public decimal? AddressingAllParts { get; set; }
        public decimal? ClarityOfPosition { get; set; }
        public decimal? DevelopmentOfIdeas { get; set; }
        public decimal? JustificationOfOpinion { get; set; }
        //public decimal coherenceinArgument { get; set; }
        public bool AppropriateWordCount { get; set; }

        // Coherence & Cohesion scores
        public decimal? LogicalOrganization { get; set; }
        public decimal? Paragraphing { get; set; }
        public decimal? CohesiveDevices { get; set; }
        public decimal? Referencing { get; set; }

        // Lexical Resource scores
        public decimal? RangeOfVocabulary { get; set; }
        public decimal? AccuracyOfWordChoice { get; set; }
        public decimal? SpellingAndFormation { get; set; }
        public decimal? RegisterAndStyle { get; set; }

        // Grammatical Range and Accuracy scores
        public decimal? GrammarRange { get; set; }
        public decimal? SentenceComplexity { get; set; }
        public decimal? GrammarAccuracy { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
    }
}

