using Microsoft.Extensions.Configuration;
using Reboost.DataAccess;
using Reboost.DataAccess.Entities;
using Reboost.DataAccess.Models;
using Reboost.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Reboost.Shared.Extensions;
using Stripe;
using System.Net.Http;
using OpenAI_API.Completions;
using OpenAI_API;
using OpenAI_API.Chat;
using OpenAI_API.Models;
using Newtonsoft.Json;
using System.Text.RegularExpressions;

namespace Reboost.Service.Services
{
    public interface IChatGPTService
    {
        Task GetDifficultyLevelForAllQuestions();
        Task<NewTOEFLEssayFeedbackModel> GetTOEFLIndependentEssayFeedback(string question, string essay, string feedbackLanguage);
        Task<NewTOEFLEssayFeedbackModel> GetTOEFLIntegratedEssayFeedback(string question, string essay, string feedbackLanguage);
        Task<NewIELTSEssayFeedbackModel> GetIELTSEssayFeedback(string question, string essay, string feedbackLanguage);
    }

    public class ChatGPTService : BaseService, IChatGPTService
    {
        private string OPENAI_API_KEY = "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";
        IConfiguration configuration;
        public ChatGPTService(IUnitOfWork unitOfWork,
            IConfiguration _configuration) : base(unitOfWork)
        {
           
            configuration = _configuration;
        }

        public static string StripHTML(string input)
        {
            return Regex.Replace(input, "<.*?>", String.Empty);
        }

        public async Task GetDifficultyLevelForAllQuestions()
        {
            OpenAIAPI api = new OpenAIAPI(new APIAuthentication(OPENAI_API_KEY));
            string task1Criteria = "You will be given a IELTS academic writing task 1 topic, your job is to classify the difficulty level of the topic into easy, medium, and hard based on the following criteria:\n\n•\tEasy: Topics are common, widely recognized, and relatable to the general public. Specialized knowledge is not required, and test takers can generate ideas just by critical thinking about the topic. \n•\tMedium: Topics are moderately familiar, where having some background knowledge is beneficial but not essential. Data or processes may include some unfamiliar elements but are generally accessible.\n•\tHard: Topics may be quite specialized or technical. Often includes data, or processes that are not commonly encountered in everyday contexts.\n\nJust response with: ‘Easy’, ‘Medium’, or ‘Hard’.\n";
            string task2Criteria = "You will be given a IELTS academic writing task 2 topic, your job is to classify the difficulty level of the topic into easy, medium, and hard based on the following criteria:\n\n•\tEasy: Topics are common, widely recognized, and relatable to the general public. Specialized knowledge is not required, and test takers can readily generate ideas and examples from their personal experiences or common knowledge.\n•\tMedium: Topics are moderately familiar, where having some background knowledge is beneficial but not essential. More critical thinking is needed to develop coherent arguments and counterarguments.\n•\tHard: Topics are specialized or abstract, demanding in-depth knowledge or a deep understanding. These subjects often involve complex concepts, technical details, or nuanced perspectives that go beyond general knowledge.\n\nJust response with: ‘Easy’, ‘Medium’, or ‘Hard’.\n";
            string independentCriteria = "You will be given a TOEFL independent writing topic, your job is to classify the difficulty level of the topic into easy, medium, and hard based on the following criteria:\n\nFamiliarity:\n•\tEasy: Topics are very common and frequently encountered in everyday life. These subjects require minimal specialized knowledge and are broadly relatable.\n•\tMedium: Topics are moderately familiar, where having some background knowledge is beneficial but not essential.\n•\tHard: Topics are specialized or abstract, demanding in-depth knowledge or a deep understanding. These subjects often involve complex concepts, technical details, or nuanced perspectives that go beyond general knowledge.\n\nSimplicity:\n•\tEasy: Questions are straightforward, typically asking for a personal preference or opinion. Easier to organize thoughts and structure the essay into introduction, body paragraphs, and conclusion.\n•\tMedium: Questions may involve a bit more complexity, such as comparing two viewpoints or analyzing a trend. Requires more developed skills in structuring arguments and supporting them with reasons and examples.\n•\tHard: Questions are more complex and might involve abstract thinking or sophisticated arguments. High level of planning, organization, and critical thinking is needed to construct a coherent and persuasive essay.\n\nLanguage Demand:\n•\tEasy: Basic vocabulary and simple grammatical structures are often sufficient. The focus is on clear and coherent expression of personal opinions and experiences.\n•\tMedium: Requires a mix of basic and advanced vocabulary and varied sentence structures. The ability to articulate and support slightly complex ideas is important.\n•\tHard: High proficiency in language use is required, with a wide range of vocabulary and complex sentence structures. The ability to express and justify complex ideas and arguments clearly and effectively is crucial.\n\nResource Availability:\n•\tEasy: There are abundant study materials, sample essays, and practice prompts available. It's relatively easy to find model essays and guidelines for these common topics.\n•\tMedium: A fair amount of practice materials and examples are available but might require more analytical skills. Candidates may need to seek specific examples or more detailed study guides.\n•\tHard: There are fewer ready-made resources and sample essays at this level. Extensive reading and research might be needed to effectively tackle these topics.\n\nJust response with: ‘Easy’, ‘Medium’, or ‘Hard’.\n";
            string integratedCriteria = "You will be given a TOEFL integrated writing topic, your job is to classify the difficulty level of the topic into easy, medium, and hard based on the following criteria:\n\nFamiliarity:\n•\tEasy: Topics are very common and frequently encountered in everyday life. These subjects require minimal specialized knowledge and are broadly relatable. Information presented in both the reading and the listening is straightforward and clear.\n•\tMedium: Topics are moderately familiar, where having some background knowledge is beneficial but not essential. Information may have more depth compared to the easy level, requiring attentive reading, and listening.\n•\tHard: Topics are specialized or abstract, demanding in-depth knowledge or a deep understanding. Both the reading and listening passages contain dense and intricate information.\n\nSimplicity:\n•\tEasy: The relationship between the reading and listening materials is relatively clear and direct. Easier to identify the main points and how they either contrast with or support each other.\n•\tMedium: The connections between the reading and listening materials might be less direct, with more subtle points of comparison or contrast. Requires more critical thinking to identify and articulate the key relationships.\n•\tHard: Relationships between the reading and listening materials are complex, often involving nuanced arguments or detailed analysis. High level of skill in organizing, integrating, and presenting information coherently and concisely is needed.\n\nLanguage Demand:\n•\tEasy: Requires basic vocabulary and simple sentence structures for summarizing and comparing information. Focus is more on accuracy in conveying information than on using complex language.\n•\tMedium: Requires a balance of basic and more advanced vocabulary, along with varied sentence structures. Ability to express relationships between more complex ideas and details is important.\n•\tHard: High proficiency in language use is required, with a wide range of vocabulary and complex sentence structures. The ability to synthesize and analyze intricate details and viewpoints is crucial.\n\nResource Availability:\n•\tEasy: Plenty of practice materials and sample responses are available, including texts and lectures on familiar topics. Easy to find resources that demonstrate how to integrate and summarize key points.\n•\tMedium: A fair amount of practice materials are available but may require more analytical skills to dissect. Might need to seek specific examples or more detailed guides on notetaking and synthesis.\n•\tHard: Fewer ready-made resources and model responses are available at this higher level of complexity. Requires extensive research and practice with advanced materials to develop proficiency.\n\nJust response with: ‘Easy’, ‘Medium’, or ‘Hard’.\n";
            string criteria = task2Criteria;

            var questions = await _unitOfWork.Questions.GetAllActiveQuestions();

            foreach(Questions question in questions)
            {
                QuestionParts questionContent = await _unitOfWork.QuestionParts.GetQuestionContentById(question.Id);
                if(questionContent != null)
                {
                    string content = ""; // StripHTML(questionContent.Content);
                    Tasks questionTask = await _unitOfWork.Questions.GetTaksById(question.TaskId);
                    string task = questionTask.Name;

                    if (task == "Independent Writing")
                    {
                        criteria = independentCriteria;
                        content = "Classify the difficulty level of the following TOEFL independent writing topic: " + StripHTML(questionContent.Content) + "";
                    }
                    else if (task == "Integrated Writing")
                    {
                        criteria = integratedCriteria;
                        string reading = "";
                        QuestionParts readingPart = await _unitOfWork.QuestionParts.GetReadingByQuestionId(question.Id);
                        if (readingPart != null)
                            reading = StripHTML(readingPart.Content);

                        string transcript = "";
                        QuestionParts transcriptPart = await _unitOfWork.QuestionParts.GetTranscriptByQuestionId(question.Id);
                        if (transcriptPart != null)
                            transcript = StripHTML(transcriptPart.Content);

                        content = "Classify the difficulty level of the following TOEFL integrated writing topic: \nQuesiton: " + StripHTML(questionContent.Content) + "\nReading: " + reading + "\nListening Transcript: " + transcript + "";

                    }
                    else if (task == "Academic Writing Task 1")
                    {
                        criteria = task1Criteria;
                        content = "Classify the difficulty level of the following IELTS academic writing task 1 topic: " + StripHTML(questionContent.Content) + "";
                    }
                    else
                    {
                        criteria = task2Criteria;
                        content = "Classify the difficulty level of the following IELTS academic writing task 2 topic: " + StripHTML(questionContent.Content) + "";
                    }
                    string difficulty = "Medium";
                    ChatMessage prerequisite = new ChatMessage(ChatMessageRole.User, criteria);
                    ChatMessage request = new ChatMessage(ChatMessageRole.User, content);
                    List<ChatMessage> messages = new List<ChatMessage>();
                    messages.Add(prerequisite);
                    messages.Add(request);

                    var result = await api.Chat.CreateChatCompletionAsync(new ChatRequest()
                    {
                        Model = Model.GPT4_Turbo,
                        Temperature = 0.1,
                        MaxTokens = 2000,
                        ResponseFormat = ChatRequest.ResponseFormats.Text,
                        Messages = messages

                    });;

                    difficulty = result.Choices[0].Message.TextContent;
                    question.Difficulty = difficulty;
                    question.LastActivityDate = DateTime.UtcNow;
                    await _unitOfWork.Questions.Update(question);
                }
                
            }
           
        }

        public async Task<NewTOEFLEssayFeedbackModel> GetTOEFLIndependentEssayFeedback(string question, string essay, string feedbackLanguage)
        {
            OpenAIAPI api = new OpenAIAPI(new APIAuthentication(OPENAI_API_KEY));

            //string prompt = "Cho chủ đề viết TOEFL Independent sau:\n\n\"“" + question + "”\n\nCung cấp phản hồi v"ề
            string prompt = "For the following TOEFL question:\n\n\"“"+ question + "”\n\nProvide feedback on Use of Language (code: useOflanguge); Coherence & Accuracy (code: coherenceAccuracy); Development & Organization (code: developmentOrganization); and Overall Feedback (code: overallFeedback), for the following  essay: \n\n“" + essay + "”\n\nReturn JSON of a list of objects with criteria code as object name and comment and score as the object’s properties. Comment is the feedback given for the criteria. Score is the score given for the criteria rated on a scale from 0 to 5 with increments of 1.0.\nAlso, add an errors list to the JSON highlighting at most 10 errors including grammar, spelling, punctuation, or word choice. This is a list of objects that have 3 properties which are: issue, type, and fix. The issue property captures an issue in the essay. The type property is the type of the issue. The fix property provides a fix for the issue.\nTry not to exceed 4000 characters and do not include an explanation of the criteria or a revised version. The feedback must be written in Vietnamese.\n";
            if(feedbackLanguage != "vn")
                prompt = "For the following TOEFL question:\n\n\"“" + question + "”\n\nProvide feedback on Use of Language (code: useOflanguge); Coherence & Accuracy (code: coherenceAccuracy); Development & Organization (code: developmentOrganization); and Overall Feedback (code: overallFeedback), for the following  essay: \n\n“" + essay + "”\n\nReturn JSON of a list of objects with criteria code as object name and comment and score as the object’s properties. Comment is the feedback given for the criteria. Score is the score given for the criteria rated on a scale from 0 to 5 with increments of 1.0.\nAlso, add an errors list to the JSON highlighting at most 10 errors including grammar, spelling, punctuation, or word choice. This is a list of objects that have 3 properties which are: issue, type, and fix. The issue property captures an issue in the essay. The type property is the type of the issue. The fix property provides a fix for the issue.\nTry not to exceed 4000 characters and do not include an explanation of the criteria or a revised version.\n";
            var result = await api.Chat.CreateChatCompletionAsync(new ChatRequest()
            {
                Model = Model.GPT4_Turbo,
                Temperature = 0.1,
                MaxTokens = 1000,
                ResponseFormat = ChatRequest.ResponseFormats.JsonObject,
                Messages = new ChatMessage[] {
                    new ChatMessage(ChatMessageRole.System, "You are a helpful assistant designed to output JSON."),
                    new ChatMessage(ChatMessageRole.User, prompt)
                }
            });
            Console.WriteLine(result);
            Console.WriteLine(result.Choices[0].Message.TextContent);
            //return result.Choices[0].Message.TextContent;
            NewTOEFLEssayFeedbackModel feedback = JsonConvert.DeserializeObject<NewTOEFLEssayFeedbackModel>(result.Choices[0].Message.TextContent);
            return feedback;
        }
        public async Task<NewTOEFLEssayFeedbackModel> GetTOEFLIntegratedEssayFeedback(string question, string essay, string feedbackLanguage)
        {
            OpenAIAPI api = new OpenAIAPI(new APIAuthentication(OPENAI_API_KEY));
            string prompt = "Provide feedback on Use of Language (code: useOflanguge); Coherence & Accuracy (code: coherenceAccuracy); Development & Organization (code: developmentOrganization); and Overall Feedback (code: overallFeedback), for the following TOEFL essay:\n\n“" + essay + "”\n\nReturn JSON of a list of objects with criteria code as object name and comment and score as the object’s properties. Comment is the feedback given for the criteria. Score is the score given for the criteria rated on a scale from 0 to 5 with increments of 1.0.\n.Also, add an errors list to the JSON highlighting at most 10 errors including grammar, spelling, punctuation, or word choice. This is a list of objects that have 3 properties which are: issue, type, and fix. The issue property captures an issue in the essay. The type property is the type of the issue. The fix property provides a fix for the issue.\nTry not to exceed 4000 characters and do not include an explanation of the criteria or a revised version. The feedback must be written in Vietnamese.\n";
            if (feedbackLanguage != "vn")
                prompt = "Provide feedback on Use of Language (code: useOflanguge); Coherence & Accuracy (code: coherenceAccuracy); Development & Organization (code: developmentOrganization); and Overall Feedback (code: overallFeedback), for the following TOEFL essay:\n\n“" + essay + "”\n\nReturn JSON of a list of objects with criteria code as object name and comment and score as the object’s properties. Comment is the feedback given for the criteria. Score is the score given for the criteria rated on a scale from 0 to 5 with increments of 1.0.\n.Also, add an errors list to the JSON highlighting at most 10 errors including grammar, spelling, punctuation, or word choice. This is a list of objects that have 3 properties which are: issue, type, and fix. The issue property captures an issue in the essay. The type property is the type of the issue. The fix property provides a fix for the issue.\nTry not to exceed 4000 characters and do not include an explanation of the criteria or a revised version.\n";
            var result = await api.Chat.CreateChatCompletionAsync(new ChatRequest()
            {
                Model = Model.GPT4_Turbo,
                Temperature = 0.1,
                MaxTokens = 1000,
                ResponseFormat = ChatRequest.ResponseFormats.JsonObject,
                Messages = new ChatMessage[] {
                    new ChatMessage(ChatMessageRole.System, "You are a helpful assistant designed to output JSON."),
                    new ChatMessage(ChatMessageRole.User, prompt)
                }
            });
            //Console.WriteLine(result);
            //return result.Choices[0].Message.TextContent;
            NewTOEFLEssayFeedbackModel feedback = JsonConvert.DeserializeObject<NewTOEFLEssayFeedbackModel>(result.Choices[0].Message.TextContent);
            return feedback;
        }
        public async Task<NewIELTSEssayFeedbackModel> GetIELTSEssayFeedback(string question, string essay, string feedbackLanguage)
        {
            OpenAIAPI api = new OpenAIAPI(new APIAuthentication(OPENAI_API_KEY));
            
            string prompt = "For the following  IELTS question:\n \n\"“" + question + "”\n\nProvide feedback on the following criteria: Task Achievement (code: taskAchievement); Coherence & Cohesion (code: coherence); Lexical Resource (code: lexicalResource); Grammatical Range & Accuracy (code: grammar); and Overall Feedback (code: overallFeedback), for the following essay: \n\n“" + essay + "”\n\nReturn JSON of a list of objects with criteria code as object name and comment and score as the object’s properties. Comment is the feedback given for the criteria. Score is the score given for the criteria rated on a scale from 0 to 9 with increments of 1.0.\nAlso, add an errors list to the JSON highlighting at most 10 errors including grammar, spelling, punctuation, or word choice. This is a list of objects that have 3 properties which are: issue, type, and fix. The issue property captures an issue in the essay. The type property is the type of the issue. The fix property provides a fix for the issue.\nTry not to exceed 4000 characters and do not include an explanation of the criteria or a revised version. The feedback must be written in Vietnamese.\n";
            if (feedbackLanguage != "vn")
                 prompt = "For the following  IELTS question:\n \n\"“" + question + "”\n\nProvide feedback on the following criteria: Task Achievement (code: taskAchievement); Coherence & Cohesion (code: coherence); Lexical Resource (code: lexicalResource); Grammatical Range & Accuracy (code: grammar); and Overall Feedback (code: overallFeedback), for the following essay: \n\n“" + essay + "”\n\nReturn JSON of a list of objects with criteria code as object name and comment and score as the object’s properties. Comment is the feedback given for the criteria. Score is the score given for the criteria rated on a scale from 0 to 9 with increments of 1.0.\nAlso, add an errors list to the JSON highlighting at most 10 errors including grammar, spelling, punctuation, or word choice. This is a list of objects that have 3 properties which are: issue, type, and fix. The issue property captures an issue in the essay. The type property is the type of the issue. The fix property provides a fix for the issue.\nTry not to exceed 4000 characters and do not include an explanation of the criteria or a revised version.\n";
            var result = await api.Chat.CreateChatCompletionAsync(new ChatRequest()
            {
                Model = Model.GPT4_Turbo,
                Temperature = 0.1,
                MaxTokens = 1000,
                ResponseFormat = ChatRequest.ResponseFormats.JsonObject,
                Messages = new ChatMessage[] {
                    new ChatMessage(ChatMessageRole.System, "You are a helpful assistant designed to output JSON."),
                    new ChatMessage(ChatMessageRole.User, prompt)
                }
            });

            NewIELTSEssayFeedbackModel feedback = JsonConvert.DeserializeObject<NewIELTSEssayFeedbackModel>(result.Choices[0].Message.TextContent);
            return feedback;
        }
    }
}

