using Microsoft.Bot.Connector;
using Microsoft.ServiceBus.Messaging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace ProjectElephant.Bot.Services
{
    public class EventHubService
    {
        private const string connectionString = "Endpoint=sb://clientinput-ns.servicebus.windows.net/;SharedAccessKeyName=Send;SharedAccessKey=g+jb40Z8eoOOKdKgfw106zJt5aza4fWYaIghEpbk5Zs=;EntityPath=clientinput";

        /// <summary>
        /// Sends the user activity to the Azure Event hub
        /// </summary>
        /// <param name="activity"></param>
        public void SendMessage(Activity activity, bool luisFound, string intentName)
        {
            // Create connection to Azure Event Hub
            var eventHubClient = EventHubClient.CreateFromConnectionString(connectionString);

            // Transform user data to CSV format.
            var data = $"Text,Date,UserName,UserId,Found,Intent\n{activity.Text},{DateTime.Now},{activity.From.Name},{activity.From.Id},{luisFound},{intentName}";

            try
            {
                // Send data ingest to Event Hub
                eventHubClient.Send(new EventData(Encoding.UTF8.GetBytes(data)));
            }
            catch (Exception exception)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("{0} > Exception: {1}", DateTime.Now, exception.Message);
                Console.ResetColor();
            }
        }
    }
}

