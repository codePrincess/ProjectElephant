using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace DataTemplateSelector
{
    public class MainPageViewModel : BaseViewModel
    {

        private ObservableCollection<MessageViewModel> messagesList;

        public ObservableCollection<MessageViewModel> Messages
        {
            get { return messagesList; }
            set { messagesList = value; RaisePropertyChanged(); }
        }

        private string outgoingText;

        public string OutGoingText
        {
            get { return outgoingText; }
            set { outgoingText = value; RaisePropertyChanged(); }
        }

        public ICommand SendCommand { get; set; }


        public MainPageViewModel()
        {
			// Initialize with default values

			Messages = new ObservableCollection<MessageViewModel>();
            OutGoingText = null;
            SendCommand = new Command(() =>
            {
				var incoming = OutGoingText.EndsWith(".");
              Messages.Add(new MessageViewModel {Text =  OutGoingText, IsIncoming = incoming, MessagDateTime = DateTime.Now});
                OutGoingText = null;
            });
        }
       // public List<MessageViewModel> Messages { get; set; } = new List<MessageViewModel>();

    }

    
}
