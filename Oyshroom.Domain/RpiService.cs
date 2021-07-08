using Oyshroom.Domain.Entity;
using Oyshroom.Domain.Model;
using System;
using System.Collections.Generic;
using System.Device.Gpio;
using System.Text;

namespace Oyshroom.Domain
{
    public class RpiService
    {
        const int FANFORWARD = 17;
        const int FANREVERSE = 18;
        const int MISTING = 27;
        const int LED = 22;
        const int SOUND = 23;


        private readonly GpioController _gpioController = new GpioController();
        private readonly ActuatorEvaluator _actuatorEvaluator;
        private EvaluatorResult _evaluatorResult;
        private List<int> _gpios = new List<int>();

        public RpiService(ActuatorEvaluator actuatorEvaluator)
        {
            _actuatorEvaluator = actuatorEvaluator;

            _gpios.Add(17);
            _gpios.Add(18);
            _gpios.Add(27);
            _gpios.Add(22);
            _gpios.Add(23);

        }

        public EvaluatorResult ActuatorStatus { get { return _evaluatorResult; } }

        public void Trigger(SensorEntity sensors)
        {

            _evaluatorResult = _actuatorEvaluator.Evaluate(sensors);

            if (_gpioController == null)
                return;

            //Misting Pin
            SetPinStatus(MISTING, _evaluatorResult.Misting);

            //FanForward Pin
            SetPinStatus(FANFORWARD, _evaluatorResult.Exhaust);

            //FanReverse Pin
            SetPinStatus(FANREVERSE, _evaluatorResult.Intake);

            //Led Pin
            SetPinStatus(LED, _evaluatorResult.LED);

            //Sound Pin
            SetPinStatus(SOUND, _evaluatorResult.Sound);

        }

        private void SetPinStatus(int pin, PinValue pinValue)
        {
            if (_gpioController.IsPinOpen(pin))
            {
                _gpioController.ClosePin(pin);          
            }

            _gpioController.OpenPin(pin, PinMode.Input);
            var currentValue = _gpioController.Read(pin);
            _gpioController.ClosePin(pin);

            if (currentValue != pinValue)
            {
                if (_gpioController.IsPinOpen(pin))
                {
                    _gpioController.ClosePin(pin);
                    _gpioController.OpenPin(pin, PinMode.Output);
                }
                else
                {
                    _gpioController.OpenPin(pin, PinMode.Output);
                }

                _gpioController.Write(pin, pinValue);

            }
        }

        public void TurnOff(int pin)
        {
            if (!_gpioController.IsPinOpen(pin))
            {
                _gpioController.OpenPin(pin, PinMode.Output);
            }

            _gpioController.Write(pin, PinValue.Low);
        }

        public void TurnOn(int pin)
        {
            if (!_gpioController.IsPinOpen(pin))
            {
                _gpioController.OpenPin(pin, PinMode.Output);
            }

            _gpioController.Write(pin, PinValue.High);

        }

        public Dictionary<string, string> GetPinStatus()
        {
            var pinStatus = new Dictionary<string, string>();

            foreach (var gpio in _gpios)
            {
                if (!_gpioController.IsPinOpen(gpio))
                {
                    _gpioController.OpenPin(gpio, PinMode.Input);
                }

                var status = _gpioController.Read(gpio);

             
                pinStatus.Add(gpio.ToString(), status.ToString());

                _gpioController.ClosePin(gpio);
            }

            return pinStatus;
        }

        public void TurnOffAllPins()
        {

            foreach (var gpio in _gpios)
            {
                if (!_gpioController.IsPinOpen(gpio))
                {
                    _gpioController.OpenPin(gpio, PinMode.Output);
                }
  
                _gpioController.Write(gpio, PinValue.Low);
                _gpioController.ClosePin(gpio);
            }

        }
    }
}
