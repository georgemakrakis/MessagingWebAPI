﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MessagesWebApi.Models
{
    public class MessageItem
    {
        public int Id { get; set; }
        public string Message { get; set; }
        public bool IsSeen { get; set; }
    }
}
