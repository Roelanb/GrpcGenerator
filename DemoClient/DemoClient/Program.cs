using Google.Protobuf.WellKnownTypes;
using Grpc.Net.Client;

namespace DemoClient
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Task.Run(Call_UspGetManagerEmployees_Test);
            Task.Run(() => ServiceCallDemo.Call_PersonPerson_GetData_Test("https://localhost:7019"));

            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }

        public async static void Call_UspGetManagerEmployees_Test()
        {

            // The port number must match the port of the gRPC server.
            using var channel = GrpcChannel.ForAddress("https://localhost:7019");
            var client = new DemoService.Services.Person.PersonClient(channel);
            var reply = await client.UspGetManagerEmployeesAsync(
                              new DemoService.Services.UspGetManagerEmployees_Request
                              {
                                  BusinessEntityID = 5
                              });
            Console.WriteLine("Greeting: " + reply.ResultText);

            foreach (var record in reply.Records)
            {
                Console.WriteLine($"{record.LastName}");
            }
          
        }

    }
}