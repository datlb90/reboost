using System;
namespace Reboost.Shared
{
    public class ExperimentFeedbackModel
    {
        public string task { get; set; }
        public string topic { get; set; }
        public string chartName { get; set; }
        public string chartDescription { get; set; }
        public string essay { get; set; }
        public string criteriaName { get; set; }
        public string feedbackLanguage { get; set; }
        public string prompt { get; set; }
    }

    public class CriteriaFeedbackModel
    {
        public string userId { get; set; }
        public string task { get; set; }
        public string topic { get; set; }
        public string chartName { get; set; }
        public string chartDescription { get; set; }
        public string essay { get; set; }
        public string criteriaName { get; set; }
        public string feedbackLanguage { get; set; }
    }

    public class EssayFeedbackModel
    {
        // criteria feedback
        public string task { get; set; }
        public string topic { get; set; }
        public string chartName { get; set; }
        public string chartDescription { get; set; }
        public string essay { get; set; }
        public string criteriaName { get; set; }
        public string feedbackLanguage { get; set; }

        // criteria model
        public int criteriaId { get; set; }
        public int order { get; set; }
    }

    //public class EssayFeedback
    //{
    //    public string criteriaName { get; set; }
    //    public int criteriaId { get; set; }
    //    public decimal mark { get; set; }
    //    public string comment { get; set; }
    //    public int order { get; set; }
    //    // essay score
    //    public decimal taskAchievementScore { get; set; }
    //    public decimal taskResponseScore { get; set; }
    //    public decimal coherenceScore { get; set; }
    //    public decimal lexicalResourceScore { get; set; }
    //    public decimal grammarScore { get; set; }
    //    public decimal overallScore { get; set; }
    //}


    public class EssayFeedback
    {
        public string criteriaName { get; set; }
        public int criteriaId { get; set; }
        public decimal mark { get; set; }
        public string comment { get; set; }
        public int order { get; set; }
        // essay score
        public decimal taskAchievementScore { get; set; }
        public decimal taskResponseScore { get; set; }
        public decimal coherenceScore { get; set; }
        public decimal lexicalResourceScore { get; set; }
        public decimal grammarScore { get; set; }
        public decimal overallScore { get; set; }

        public decimal logicalStructureScore { get; set; }
        public decimal linkingWordScore { get; set; }

        public decimal variedVocabularyScore { get; set; }
        public decimal variedSentenceScore { get; set; }

        public decimal allPartsScore { get; set; }
    }

    public class EssayScore
    {
        // Overall score
        public decimal? overallBandScore { get; set; }

        // 4 Cretieria score
        public decimal? taskAchievementScore { get; set; }
        public decimal? taskResponseScore { get; set; }
        public decimal? coherenceScore { get; set; }
        public decimal? lexicalResourceScore { get; set; }
        public decimal? grammarScore { get; set; }

        // Task Achievement scores
        public decimal? fulfillRequirements { get; set; }
        public decimal? highlightKeyFeatures { get; set; }
        public decimal? compareAndContrast { get; set; }
        public decimal? dataSelection { get; set; }

        // Task Response scores
        public decimal? addressingAllParts { get; set; }
        public decimal? clarityOfPosition { get; set; }
        public decimal? developmentOfIdeas { get; set; }
        public decimal? justificationOfOpinion { get; set; }
        //public decimal coherenceinArgument { get; set; }
        public bool appropriateWordCount { get; set; }

        // Coherence & Cohesion scores
        public decimal? logicalOrganization { get; set; }
        public decimal? paragraphing { get; set; }
        public decimal? cohesiveDevices { get; set; }
        public decimal? referencing { get; set; }

        // Lexical Resource scores
        public decimal? rangeOfVocabulary { get; set; }
        public decimal? accuracyOfWordChoice { get; set; }
        public decimal? spellingAndFormation { get; set; }
        public decimal? registerAndStyle { get; set; }

        // Grammatical Range and Accuracy scores
        public decimal? grammarRange { get; set; }
        public decimal? sentenceComplexity { get; set; }
        public decimal? grammarAccuracy { get; set; }
    }


    public class ErrorFeedbackModel
    {
        public string essay { get; set; }
        public string feedbackLanguage { get; set; }
    }
}

