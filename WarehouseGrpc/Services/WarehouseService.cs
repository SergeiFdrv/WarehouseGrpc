using Grpc.Core;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Google.Protobuf.WellKnownTypes;

namespace WarehouseGrpc
{
    public class WarehouseService : Warehouse.WarehouseBase
    {
        private readonly ILogger<WarehouseService> _logger;
        public WarehouseService(ILogger<WarehouseService> logger)
        {
            _logger = logger;
        }

        private readonly Random Random = new Random();

        public override Task<Crane> GetCrane(ID request, ServerCallContext context)
        {
            var crane = new Crane
            {
                CraneNumber = request.Id,
                CraneState = 0,
                ErrorTime = Timestamp.FromDateTime(DateTime.UtcNow.AddDays(-1)),
                QueueLength = 0
            };
            return Task.FromResult(crane);
        }

        public override Task GetCranes(ID request, IServerStreamWriter<Crane> responseStream, ServerCallContext context)
        {
            for (int i = 1; i <= 10; i++)
            {
                Crane crane = new Crane
                {
                    CraneNumber = i,
                    CraneState = Random.Next(4) + 1
                };
                if (crane.CraneState < 3)
                {
                    crane.QueueLength = Random.Next(10);
                }
                else
                {
                    crane.ErrorTime = Timestamp.FromDateTime(
                        DateTime.UtcNow.AddMinutes(-Random.Next(240)));
                }
                responseStream.WriteAsync(crane);
            }
            return Task.CompletedTask;
        }

        public override Task<Transporter> GetTransporter(ID request, ServerCallContext context)
        {
            var transporter = new Transporter
            {
                TransporterNumber = request.Id,
                ErrorIn = false,
                ErrorOut = false,
                WorkIn = false,
                WorkOut = true
            };
            return Task.FromResult(transporter);
        }

        public override Task GetTransporters(ID request, IServerStreamWriter<Transporter> responseStream, ServerCallContext context)
        {
            for (int i = 1; i <= 2; i++)
            {
                Transporter transporter = new Transporter
                {
                    TransporterNumber = i,
                    WorkIn = Random.Next() % 2 == 0,
                    WorkOut = Random.Next() % 2 == 0,
                    ErrorIn = Random.Next() % 2 == 0,
                    ErrorOut = Random.Next() % 2 == 0
                };
                responseStream.WriteAsync(transporter);
            }
            return Task.CompletedTask;
        }

        public override Task<Gate> GetGate(ID request, ServerCallContext context)
        {
            var gate = new Gate
            {
                GateNumber = request.Id,
                CarPresent = Random.Next() % 2 == 0
            };
            if (gate.CarPresent)
            {
                gate.GateOpen = Random.Next() % 2 == 0;
                gate.CarBeginTime = Timestamp.FromDateTime(DateTime.UtcNow.AddMinutes(Random.Next()));
                gate.CarNumber = "о000оо00";
                gate.OperationType = Random.Next() % 2 == 0 ? 1 : 2;
                gate.PalletCount = Random.Next(41);
                gate.AlarmPalletCount = Random.Next(10);
                if (gate.AlarmPalletCount < 5 || gate.AlarmPalletCount > gate.PalletCount)
                {
                    gate.AlarmPalletCount = 0;
                }
            }
            return Task.FromResult(gate);
        }

        public override Task GetGates(ID request, IServerStreamWriter<Gate> responseStream, ServerCallContext context)
        {
            for (int i = 0; i < 15; i++)
            {
                Gate gate = new Gate();
                gate.GateNumber = i;
                gate.CarPresent = Random.Next() % 2 == 0;
                if (gate.CarPresent)
                {
                    gate.CarNumber = "a123bc178";
                    gate.CarBeginTime = Timestamp.FromDateTime(
                        DateTime.UtcNow.AddMinutes(-Random.Next(240)));
                    gate.GateOpen = Random.Next() % 2 == 0;
                    gate.OperationType = Random.Next() % 2 == 0 ? 1 : 2;
                    gate.PalletCount = Random.Next(41);
                    gate.AlarmPalletCount = Random.Next(10);
                    if (gate.AlarmPalletCount < 5 ||
                        gate.AlarmPalletCount > gate.PalletCount)
                    {
                        gate.AlarmPalletCount = 0;
                    }
                }
                responseStream.WriteAsync(gate);
            }
            return Task.CompletedTask;
        }

        public override Task<Duration> GetCraneErrorDuration(Crane request, ServerCallContext context)
        {
            return Task.FromResult(Duration.FromTimeSpan(DateTime.UtcNow - request.ErrorTime.ToDateTime()));
        }
    }
}
