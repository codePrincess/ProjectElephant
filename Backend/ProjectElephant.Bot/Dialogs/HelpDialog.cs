using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Luis;
using Microsoft.Bot.Builder.Luis.Models;
using Microsoft.Bot.Connector;
using ProjectElephant.Bot.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace ProjectElephant.Bot.Dialogs
{
    [LuisModel("0e3c461d-e8df-48c0-844c-e2c75011a7f8", "91cabc5bf691417cace267201a53ad36")]
    [Serializable]
    public class HelpDialog : LuisDialog<object>
    {
        private Activity activity;
        private ElephantBrainService elephantBrainService;
        private EventHubService eventHubService;

        public HelpDialog(Activity activity)
        {
            this.activity = activity;

            elephantBrainService = new ElephantBrainService();
            eventHubService = new EventHubService();
        }

        [LuisIntent("")]
        public async Task None(IDialogContext context, LuisResult result)
        {
            // Register activity in Azure Event Hub
            eventHubService.SendMessage(activity, false, "");

            await context.PostAsync("Sorry, I did not understand this :(");
            context.Wait(MessageReceived);
        }

        [LuisIntent("GreetingsInformation")]
        public async Task GreetingsInformation(IDialogContext context, LuisResult result)
        {
            await context.PostAsync("Hi my friend. I am the Onboarding Elephant. How can I help you?");
            context.Wait(MessageReceived);
        }

        [LuisIntent("TravelInformation")]
        [LuisIntent("VacationInformation")]
        [LuisIntent("BookInformation")]
        [LuisIntent("NetworkInformation")]
        [LuisIntent("BestCoffeeInformation")]
        [LuisIntent("BenefitInformation")]
        [LuisIntent("GuestInformation")]
        [LuisIntent("PasswordInformation")]
        [LuisIntent("ExpenseInformation")]
        [LuisIntent("WorkplaceInformation")]
        [LuisIntent("ResponsibilityInformation")]
        [LuisIntent("AmexInformation")]
        [LuisIntent("CantinaInformation")]
        public async Task Information(IDialogContext context, LuisResult result)
        {
            // Find most likely intent
            var intent = result.Intents.OrderByDescending(x => x.Score).First();
            //await context.PostAsync($"DEV: Got this ({intent.Intent})!");
            context.Wait(MessageReceived);

            // Register activity in Azure Event Hub
            eventHubService.SendMessage(activity, true, intent.Intent);

            // Get answers for intnet from server
            var answers = await elephantBrainService.GetAnswersAsync(intent.Intent.Replace("Information", ""));

            // Show answers to the user
            if (answers.Any())
            {
                foreach (var answer in answers)
                {
                    await context.PostAsync(answer.Answer);
                }
            }
            else
            {
                await context.PostAsync($"Sorry, we don't have any answers for you :( Please call Helpdesk (+49 176 26789086) to get further help!");                
            }

            context.Wait(MessageReceived);
        }
    }
}