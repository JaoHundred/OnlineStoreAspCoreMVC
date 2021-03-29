using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineST.Models.ViewModel
{
    public class MessageViewModel
    {
        public string Message { get; set; }

        public FormType FormType { get; set; }

        public MessageType MessageType { get; set; }
    }

    public enum MessageType
    {
        none,
        danger,
        success,
    }

    public enum FormType
    {
        CreateAccount,
        Login,
    }
}
