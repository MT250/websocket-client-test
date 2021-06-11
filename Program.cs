using System;
using System.Collections;
using System.Threading;
using Newtonsoft.Json;
using WebSocketSharp;

namespace ConsoleApp
{
    class Program
    {
        const string uri = "ws://localhost";
        const string port = "80";

        static WebSocket webSocket;
        private static bool isConnectionOpen;


        static void Main(string[] args)
        {
            Console.WriteLine(DateTime.Now + " | Client started.");

            CreateWebSocket();

            Console.ReadLine();
        }

        private static void CreateWebSocket()
        {
            if (!isConnectionOpen)
            {
                webSocket = new WebSocket(uri + ":" + port + "/Echo");
                try
                {
                    webSocket.ConnectAsync();
                    isConnectionOpen = true;
                    webSocket.OnMessage += Websocket_OnMessage;

                    //Thread.Sleep(3000);
                    CollectDataAndSendToServer();

                    Console.WriteLine(DateTime.Now + " | Press any key to close WebSocket connection.");
                    while (!Console.KeyAvailable)
                    { }

                    CloseWebSocket();
                }
                catch (Exception e)
                {
                    Console.WriteLine(DateTime.Now + " | [ERROR] " + e.Message);
                }
            }
        }

        private static void CloseWebSocket()
        {
            webSocket.CloseAsync();
            isConnectionOpen = false;
            Console.WriteLine(DateTime.Now + " | Connection closed.");
        }

        private static string SerializeToJSON(Data _data)
        {
            //Serializes instance into JSON string
            string json = JsonConvert.SerializeObject(_data, Formatting.Indented);
            return json;
        }

        private static void ClientWebSocketFunctionality(string _json) //TODO: Dead
        {
            if (!isConnectionOpen) {
                using (WebSocket webSocket = new WebSocket(uri + ":" + port + "/Echo"))
                {
                    try
                    {
                        webSocket.Connect();
                        isConnectionOpen = true;
                        webSocket.OnMessage += Websocket_OnMessage;

                        webSocket.Send(_json);


                        Console.WriteLine(DateTime.Now + " | Press any key to close connection");
                        while (!Console.KeyAvailable)
                        {
                        }

                        webSocket.Close();
                        isConnectionOpen = false;
                        Console.WriteLine(DateTime.Now + " | Connection closed");
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(DateTime.Now + " | [ERROR] " + e.Message);
                    }
                }
            }

        }

        private static void Websocket_OnMessage(object sender, MessageEventArgs e)
        {
            if (e.Data == "GetData") //TODO: Replace string with something else
            {
                CollectDataAndSendToServer();
            }
            else
                Console.WriteLine(e.Data);
        }

        private static void CollectDataAndSendToServer()
        {
            //Create instance of Data class
            Data data = new Data();
            //Convert instance into JSON string
            string jsonData = SerializeToJSON(data);

            if (isConnectionOpen)
            {
                webSocket.SendAsync(jsonData, null);
                Console.WriteLine(DateTime.Now + " | Send data to server.");
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

