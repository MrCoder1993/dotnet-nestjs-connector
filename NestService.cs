using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NestjsConnector
{
    public class NestService
    {
        public event EventHandler<ServiceEventArgs> callback;
        //public delegate ServiceEventArgs callback(ServiceEventArgs e);

        public string Name { get; set; }
        public string Host { get; set; }
        public int Port { get; set; }
        public void onResponse(string sender,ServiceEventArgs e)
        {

            if (this.callback!= null) 
            this.callback.Invoke(sender, e);
        }
    }
}
