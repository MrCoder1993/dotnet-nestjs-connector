﻿using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NestjsConnector
{
    public class ServiceEventArgs : EventArgs
    {
        public JObject data { get; set; }
    }
}
