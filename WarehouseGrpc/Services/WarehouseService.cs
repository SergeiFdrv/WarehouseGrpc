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

        public override Task<Gate> GetGate(ID request, ServerCallContext context)
        {
            var gate = new Gate
            {
                GateNumber = request.Id,
                GateOpen = true,
                CarPresent = true,
                CarBeginTime = Timestamp.FromDateTime(DateTime.UtcNow.AddHours(-1)),
                CarNumber = "о000оо00",
                OperationType = 1,
                AlarmPalletCount = 0,
                PalletCount = 5
            };
            return Task.FromResult(gate);
        }

        public override Task<Duration> GetCraneErrorDuration(Crane request, ServerCallContext context)
        {
            return Task.FromResult(Duration.FromTimeSpan(DateTime.UtcNow - request.ErrorTime.ToDateTime()));
        }
    }
}
