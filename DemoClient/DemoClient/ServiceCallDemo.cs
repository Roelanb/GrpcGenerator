using Grpc.Net.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoClient
{
    public static class ServiceCallDemo
    {
        public async static Task Call_PersonPerson_GetData_Test(string rpcEndpoint)
        {

            // The port number must match the port of the gRPC server.
            using var channel = GrpcChannel.ForAddress("https://localhost:7019");
            var client = new DemoService.Services.Person.PersonClient(channel);
            var reply = await client.PersonPerson_GetDataAsync(
                              new DemoService.Services.PersonPerson_GetData_Query
                              {
                                  WhereClause = ""
                              });

            Console.WriteLine("Greeting: " + reply.ResultText);

            foreach (var record in reply.Records)
            {
                Console.WriteLine($"{record.LastName}");
            }

        }
    }
}
