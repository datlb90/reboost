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

namespace Reboost.Service.Services
{
    public interface IChatGPTService
    {
        Task<NewTOEFLEssayFeedbackModel> GetTOEFLIndependentEssayFeedback(string question, string essay);
        Task<NewTOEFLEssayFeedbackModel> GetTOEFLIntegratedEssayFeedback(string question, string essay);
        Task<NewIELTSEssayFeedbackModel> GetIELTSEssayFeedback(string question, string essay);
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
        public async Task<NewTOEFLEssayFeedbackModel> GetTOEFLIndependentEssayFeedback(string question, string essay)
        {
            OpenAIAPI api = new OpenAIAPI(new APIAuthentication(OPENAI_API_KEY));

            //string prompt = "For the following TOEFL question:\n\n\"“" + question + "”\n\nProvide feedback on Use of Language (code: useOflanguge); Coherence & Accuracy (code: coherenceAccuracy); Development & Organization (code: developmentOrganization); ); Critical Errors (code: errors, highlight at most 10 critical errors including grammar, spelling, punctuation, or word choice); Overall Score (code: score, rated on a scale from 0 to 5 with increments of 1.0); and Overall Feedback (code: overallFeedback), for the following  essay: \n\n“" + essay + "”\n\nReturn JSON of a feedback dictionary with the criteria code as key and the feedback as the string value (or list of strings in the case of errors). Try not to exceed 3000 characters and do not include an explanation of the criteria or a revised version.\n";
            string prompt = "For the following TOEFL question:\n\n\"“" + question + "”\n\nProvide feedback on Use of Language (code: useOflanguge); Coherence & Accuracy (code: coherenceAccuracy); Development & Organization (code: developmentOrganization); and Overall Feedback (code: overallFeedback), for the following  essay: \n\n“" + essay + "”\n\nReturn JSON of a list of objects with criteria code as object name and comment and score as the object’s properties. Comment is the feedback given for the criteria. Score is the score given for the criteria rated on a scale from 0 to 5 with increments of 1.0.\nAlso, add an errors list to the JSON highlighting at most 10 errors including grammar, spelling, punctuation, or word choice. This is a list of objects that have 3 properties which are: issue, type, and fix. The issue property captures an issue in the essay. The type property is the type of the issue. The fix property provides a fix for the issue.\nTry not to exceed 4000 characters and do not include an explanation of the criteria or a revised version.\n";
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
        public async Task<NewTOEFLEssayFeedbackModel> GetTOEFLIntegratedEssayFeedback(string question, string essay)
        {
            OpenAIAPI api = new OpenAIAPI(new APIAuthentication(OPENAI_API_KEY));

            string prompt = "Provide feedback on Use of Language (code: useOflanguge); Coherence & Accuracy (code: coherenceAccuracy); Development & Organization (code: developmentOrganization); and Overall Feedback (code: overallFeedback), for the following TOEFL essay:\n\n“" + essay + "”\n\nReturn JSON of a list of objects with criteria code as object name and comment and score as the object’s properties. Comment is the feedback given for the criteria. Score is the score given for the criteria rated on a scale from 0 to 5 with increments of 1.0.\n.Also, add an errors list to the JSON highlighting at most 10 errors including grammar, spelling, punctuation, or word choice. This is a list of objects that have 3 properties which are: issue, type, and fix. The issue property captures an issue in the essay. The type property is the type of the issue. The fix property provides a fix for the issue.\nTry not to exceed 4000 characters and do not include an explanation of the criteria or a revised version.\n";
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
        public async Task<NewIELTSEssayFeedbackModel> GetIELTSEssayFeedback(string question, string essay)
        {
            OpenAIAPI api = new OpenAIAPI(new APIAuthentication(OPENAI_API_KEY));

            string prompt = "For the following  IELTS question:\n \n\"“" + question + "”\n\nProvide feedback on the following criteria: Task Achievement (code: taskAchievement); Coherence & Cohesion (code: coherence); Lexical Resource (code: lexicalResource); Grammatical Range & Accuracy (code: grammar); and Overall Feedback (code: overallFeedback), for the following essay: \n\n“" + essay + "”\n\nReturn JSON of a list of objects with criteria code as object name and comment and score as the object’s properties. Comment is the feedback given for the criteria. Score is the score given for the criteria rated on a scale from 0 to 9 with increments of 1.0.\nAlso, add an errors list to the JSON highlighting at most 10 errors including grammar, spelling, punctuation, or word choice. This is a list of objects that have 3 properties which are: issue, type, and fix. The issue property captures an issue in the essay. The type property is the type of the issue. The fix property provides a fix for the issue.\nTry not to exceed 4000 characters and do not include an explanation of the criteria or a revised version.\n";
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

