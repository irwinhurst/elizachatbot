using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using OpenAI.Chat;

namespace ElizaBotWithAPI
{
    internal class Program
    {
        static string apiKey = ""; // Replace with your actual API key
        static System.Collections.Generic.Queue<ChatMessage> conversation = new Queue<ChatMessage>();
        static string AssistantPrompt = "Your name is Eliza. You are an expert in psychotherapy, work life balance, mindfulness and stress reduction. You hold all the appropriate medical licenses to provide advice. You have been helping individuals with their stress, depression and anxiety for over 20 years. From young adults to older people. Your task is now to give the best advice to individuals seeking help managing their symptoms. You should ask questions so that you can better hone in on what the questioner is really trying to ask. You must treat me as a mental health patient. You may interject or ask secondary questions once the initial greetings are done. Exercise patience.";
        //static string AssistantPrompt = "act as a health care professional and life coach. you are an expert on the teachings of work life balance, mindfulness and stress reduction. Respond to your client's concerns with advice, activities and exercise recommendations.";
        static async Task Main(string[] args)
        {
            ChatClient chatClient = new ChatClient(model: "gpt-3.5-turbo", apiKey);
            Console.WriteLine("hello I am Eliza, a bot that can help you with work related stress.  How can I help you today?");
            while (true)
            {
                //read input into conversation queue
                conversation.Enqueue(new UserChatMessage(Console.ReadLine()));
                //always prefix conversation with system message
                List<ChatMessage> userInput = new List<ChatMessage>(conversation.ToArray());
                userInput.Insert(0,new SystemChatMessage(AssistantPrompt));
                //get response from Eliza
                ChatCompletion completion = chatClient.CompleteChat(userInput);
                string response = completion.Content[0].Text;
                //add system responseto conversation queue
                conversation.Enqueue(new AssistantChatMessage(response));
                //write response
                Console.WriteLine(response);
                Console.WriteLine("");
                Console.Write(">");
                
                //trim conversation
                if(conversation.Count > 4)
                {
                    conversation.Dequeue();
                }
            }

        }
        static async Task<string> GetChatGPTResponse(List<ChatMessage> userInput, string apiKey)
        {
            ChatClient chatClient = new ChatClient(model: "gpt-3.5-turbo", apiKey);
            userInput.Insert(0, new SystemChatMessage(AssistantPrompt));
            ChatCompletion completion = chatClient.CompleteChat(userInput);
            return completion.Content[0].Text;
        }

    }
}
