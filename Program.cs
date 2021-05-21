using System;
using Newtonsoft.Json;
using WebSocketSharp;

namespace ConsoleApp
{
    class Program
    {
        const string uri = "ws://localhost";
        const string port = "80";
        static void Main(string[] args)
        {
            Console.WriteLine(DateTime.Now + "| Client started.");

            //Create instance of Data class
            Data data = new Data();
            //Convert instance into JSON string
            string jsonData = SerializeToJSON(data);

            Console.WriteLine(jsonData);

            ClientWebSocketFunctionality(jsonData);

            Console.ReadLine();
        }

        private static string SerializeToJSON(Data _data)
        {
            //Serializes instance into JSON string
            string json = JsonConvert.SerializeObject(_data, Formatting.Indented);
            return json;
        }

        private static void ClientWebSocketFunctionality(string _json)
        {
            using (WebSocket webSocket = new WebSocket(uri + ":" + port + "/Echo"))
            {
                try
                {
                    webSocket.Connect();
                    webSocket.Send(_json);
                    Console.WriteLine(DateTime.Now + " | Sent data to server.");
                }
                catch (Exception e)
                {
                    Console.WriteLine(DateTime.Now + " | [ERROR] " + e.Message);
                }
            }

        }

        class Data
        {
            public string computerName;
            public TimeZone computerTimeZone;
            public string computerOS;
            public string computerDotNETver;

            public Data()
            {
                computerName = Environment.MachineName;
                computerTimeZone = TimeZone.CurrentTimeZone;
                computerOS = Environment.OSVersion.ToString();
                computerDotNETver = Environment.Version.ToString();

                Console.WriteLine(DateTime.Now + " | Data collected.");
            }
        }
    }
}

