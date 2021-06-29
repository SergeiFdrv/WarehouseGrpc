using Grpc.Net.Client;
using System;
using System.Threading.Tasks;

namespace WarehouseGrpc.TestClient
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var client = new Warehouse.WarehouseClient(GrpcChannel);
            ID id = new ID
            {
                Id = 0
            };
            var crane = await client.GetCraneAsync(id);
            Console.WriteLine(client.GetCraneErrorDuration(crane).ToTimeSpan());
            var transporter = await client.GetTransporterAsync(id);
            Console.WriteLine(transporter.WorkOut);
            var gate = await client.GetGateAsync(id);
            Console.WriteLine(gate.CarNumber);

            var gates = client.GetGates(id);
            Gate cGate;
            while (gates.ResponseStream.MoveNext(System.Threading.CancellationToken.None).Result)
            {
                cGate = gates.ResponseStream.Current;
                Console.WriteLine(cGate.CarNumber);
            };
        }

        private static readonly GrpcChannel GrpcChannel = GrpcChannel.ForAddress("https://localhost:5555");
    }
}
