using System;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using ModernHttpClient;
using Newtonsoft.Json;
using Xamarin.Forms;

namespace DataTemplateSelector
{
	public partial class MainPage : ContentPage
    {
		MainPageViewModel mainPageViewModel = new MainPageViewModel();
        public MainPage()
        {
            InitializeComponent();
            Title = "Elephant";
			BindingContext = mainPageViewModel;

			Textfield.Completed += OnMessageCompleted;

        }

		public async void OnMessageCompleted(object sender, EventArgs e)
		{
			mainPageViewModel.SendCommand.Execute(null);
			await TalkToTheBot("hi");
		}

        private void MyListView_OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            MessagesListView.SelectedItem = null;
        }

        private void MyListView_OnItemTapped(object sender, ItemTappedEventArgs e)
        {
            MessagesListView.SelectedItem = null;

        }



		private async Task<bool> TalkToTheBot(string message)
		{
			bool IsReplyReceived = false;

			HttpClient client = new HttpClient(new NativeMessageHandler());
			client.BaseAddress = new Uri("https://directline.botframework.com");
			client.DefaultRequestHeaders.Accept.Clear();
			client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
			client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("BotConnector", "[ENTER YOUR SECRET HERE]");
			HttpResponseMessage response = await client.GetAsync("/api/tokens/");
			if (response.IsSuccessStatusCode)
			{
				var token = response.Content.ReadAsAsync(typeof(string)).Result as string;
				client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("BotConnector", token);
				
				var conversation = new Conversation();

				response = await client.PostAsJsonAsync("/api/conversations", conversation);

				if (response.IsSuccessStatusCode)
				{
					var content = await response.Content.ReadAsStringAsync();
					Conversation ConversationInfo = JsonConvert.DeserializeObject<Conversation>(content);

					string conversationUrl = "/api/conversations/" + ConversationInfo.conversationId + "/messages/";
					Message msg = new Message() { text = message };

					response = await client.PostAsJsonAsync(conversationUrl, msg);

					//if (response.IsSuccessStatusCode)
					//{ 
						MessageSet BotMessage = response.Content.ReadAsAsync(typeof(MessageSet)).Result as MessageSet;

					if (BotMessage != null && BotMessage.messages != null)
					{
						Textfield.Text = BotMessage.messages.ToString() + ".";
					}
					else
					{ 
						Textfield.Text = "I have a lot to do right now! Come back later.";
					}

					mainPageViewModel.SendCommand.Execute(null);
					IsReplyReceived = true;

				}


			}
			return IsReplyReceived;
		}

			
    }

	public class Conversation
	{
		public string conversationId { get; set; }
		public string token { get; set; }
	}


	public class MessageSet
	{
		public Message[] messages { get; set; }
		public string watermark { get; set; }
		public string eTag { get; set; }
	}

	public class Message
	{
		public string id { get; set; }
		public string conversationId { get; set; }
		public DateTime created { get; set; }
		public string from { get; set; }
		public string text { get; set; }
		public string channelData { get; set; }
		public string[] images { get; set; }
		public Attachment[] attachments { get; set; }
		public string eTag { get; set; }
	}

	public class Attachment
	{
		public string url { get; set; }
		public string contentType { get; set; }
	}
}
