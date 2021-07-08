using Oyshroom.Data;
using Oyshroom.Data.Model;
using Oyshroom.Domain.Model;
using System;
using System.Collections.Generic;
using System.Device.Gpio;
using System.Text;

namespace Oyshroom.Domain.Entity
{
    public class EvaluatorResult
    {
        public PinValue Intake { get; set; }

        public PinValue Exhaust { get; set; }

        public PinValue LED { get; set; }

        public PinValue Misting { get; set; }

        public PinValue Sound { get; set; }

    }

    public class ActuatorEvaluator
    {
        private readonly EvaluatorResult _evaluatorResult;

        public ActuatorEvaluator()
        {
            _evaluatorResult = new EvaluatorResult();
        }


        public EvaluatorResult Evaluate(SensorEntity sensors)
        {
            _evaluatorResult.Misting = sensors.Misting == 1 ? PinValue.High : PinValue.Low;
            _evaluatorResult.Intake = sensors.Intake == 1 ? PinValue.High : PinValue.Low;
            _evaluatorResult.Exhaust = sensors.Exhaust == 1? PinValue.High : PinValue.Low;
            _evaluatorResult.LED = sensors.LED == 1 ? PinValue.High : PinValue.Low;
            _evaluatorResult.Sound = sensors.Sound == 1 ? PinValue.High : PinValue.Low;

            return _evaluatorResult;

        }

    }
}
