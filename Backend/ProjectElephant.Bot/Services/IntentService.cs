using ProjectElephant.Bot.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProjectElephant.Bot.Services
{
    public class IntentService
    {
        public HelpIntent GetIntent(string message)
        {
            if (message.ToLower().Contains("hi") || message.ToLower().Contains("hello"))
                return HelpIntent.Hello;
            else
                return HelpIntent.None;                
        }
    }
}