using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace NestjsConnector
{
    public static class Services
    {
        private static List<NestService> services = new List<NestService>();
        private static Dictionary<string, TcpClient> clients = new Dictionary<string, TcpClient>();
        public delegate void Callback(string sender, ServiceEventArgs args);
        public static bool addService(NestService item)
        {
            bool Result = false;

            try
            {
                if (!services.Any(x => x.Name.ToUpper() == item.Name.ToUpper()))
                {
                    item.Name = item.Name.ToUpper();
                    services.Add(item);
                    Result = true;
                    TcpClient client = new TcpClient();
                    client.Connect(item.Host, item.Port);
                    clients.Add(item.Name, client);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return Result;
        }
        public static bool addService(int port, string host = "localhost", Callback callback = null)
        {
            bool Result = false;
            NestService item = new NestService()
            {
                Host = host,
                Port = port,
                Name = port.ToString(),
            };

            if (callback != null) 
            item.callback += ((sender,args) => callback(sender.ToString(), args));
            try
            {
                if (!services.Any(x => x.Name.ToUpper() == item.Name.ToUpper()))
                {
                    item.Name = item.Name.ToUpper();
                    services.Add(item);
                    Result = true;

                    TcpClient client = new TcpClient();
                    client.Connect(host, port);
                    clients.Add(item.Name, client);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return Result;
        }


        public static bool removeService(string name)
        {
            bool Result = false;
            try
            {
                services = services.Where(x => x.Name.ToUpper() != name.ToUpper()).ToList();
                TcpClient client = clients[name];
                if (client != null && client.Connected)
                    client.Close();

                clients.Remove(name);

            }
            catch (Exception)
            {

                throw;
            }
            return Result;
        }
        public static async Task<string> Send(string name, string pattern, object data = null, Callback callback = null)
        {
            NestService item = services.FirstOrDefault(x => x.Name.ToUpper() != name.ToUpper());
            if (item == null)
            {
                throw new Exception("Service not register");
            }


            string Id = Guid.NewGuid().ToString();


            Task.Run(() =>
            {
                SendProccess(item, Id, pattern, data, callback);
            });

            return Id;
        }

        public static async Task<string> Send(int port, string pattern, object data = null, Callback callback = null)

        {
            NestService item = services.FirstOrDefault(x => x.Port == port);
            if (item == null)
            {
                throw new Exception("Service not register");
            }


            string Id = Guid.NewGuid().ToString();

            Task.Run(() =>
            {
                SendProccess(item, Id, pattern, data, callback);
            });

            return Id;
        }
        private static async Task SendProccess(NestService item, string id, string pattern, object data =null, Callback callback = null)
        { 
            TcpClient client = clients[item.Name];
            if (client == null || !client.Connected)
            {
                try
                {
                    client = new TcpClient();
                    client.Connect(item.Host, item.Port);
                    if (client == null)
                    {
                        clients.Add(item.Name, client);
                    }

                }
                catch (Exception)
                {
                    throw new Exception("your nestjs service not running");
                }

            }

            // ساخت آبجکت JSON
            var requestObj = new
            {
                pattern, // الگوی @MessagePattern("test")
                data, // محتوای درخواست
                id
            };
            // تبدیل آبجکت به JSON
            string jsonRequest = JsonConvert.SerializeObject(requestObj);
            byte[] dataByteArray = Encoding.UTF8.GetBytes($"{jsonRequest.Length}#{jsonRequest}");
            NetworkStream stream = client.GetStream();
            stream.Write(dataByteArray, 0, dataByteArray.Length);

            byte[] buffer = new byte[1024];
            int bytesRead = stream.Read(buffer, 0, buffer.Length);
            string response = Encoding.UTF8.GetString(buffer, 0, bytesRead);

 
            stream.Close();
            //client.Close();

            JObject result = JsonConvert.DeserializeObject<JObject>(response.Split("#")[1]); 
           
            if (callback != null)
                callback.Invoke(pattern, new ServiceEventArgs() { data = result });
            item.onResponse(pattern, new ServiceEventArgs() { data = result });
            //callback(pattern, );

        }
    }
}
