using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Models
{
public class Discussion
{
    public int Id { get; set; }

    public int ?IdSender { get; set; } 

    public int ?IdReceiver { get; set; } 

    public string ?Sender_name { get; set; }

    public string ?Receiver_name { get; set; } 

    public DateOnly Date { get; set; }
}
}