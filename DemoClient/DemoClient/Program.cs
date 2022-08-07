﻿using Google.Protobuf.WellKnownTypes;
using Grpc.Net.Client;

namespace DemoClient
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Task.Run(CallTest);

            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }

        public async static void CallTest()
        {

            // The port number must match the port of the gRPC server.
            using var channel = GrpcChannel.ForAddress("https://localhost:7019");
            var client = new TestDatabase.TestDatabaseClient(channel);
            var reply = await client.Csp_rpc_procedure1Async(
                              new Csp_rpc_procedure1_Request { 
                                  Startdate = Timestamp.FromDateTimeOffset(DateTime.Now),
                              Search1="test search"});
            Console.WriteLine("Greeting: " + reply.ResultText);

            foreach (var record in reply.Records)
            {
                Console.WriteLine($"{record.Id}");
            }
          
        }
    }
}