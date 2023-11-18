using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Models
{
    public class Message
    {
        public int Id { get; set; }
        public string ?Content { get; set; }

        public int IdSender  { get; set; }

        public int IdReceiver { get; set; }

        public int IdDiscussion { get; set; }

    }
}