using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EurofighterInformationCenter
{
    public class EurofighterControl
    {
        private const int NeutralValue = 90;

        // Querruder (Ailerons)
        private int AileronRightValue(int joystickXValue, int joystickYValue)
        {
            return joystickYValue;
        }

        private int AileronLeftValue(int joystickXValue, int joystickYValue)
        {
            return 180 - joystickYValue;
        }

        private int AileronRightOUTValue(int joystickXValue, int joystickYValue)
        {
            if (joystickXValue > 80 && joystickXValue < 100)
            {
                return joystickYValue;
            }
            else
            {
                return joystickXValue;
            }
        }

        private int AileronLefttOUTValue(int joystickXValue, int joystickYValue)
        {
            if (joystickXValue > 80 && joystickXValue < 100)
            {
                return 180 - joystickYValue;
            }
            else
            {
                return joystickXValue;
            }
        }


        // Canards
        private int CanardRightValue(int joystickYValue, int joystickXValue)
        {
            int adjustment = joystickXValue < NeutralValue ? (NeutralValue - joystickXValue) / 2 : 0;
            return joystickYValue + adjustment;
        }

        private int CanardLeftValue(int joystickYValue, int joystickXValue)
        {
            int adjustment = joystickXValue > NeutralValue ? (joystickXValue - NeutralValue) / 2 : 0;
            return 180 - joystickYValue + adjustment;
        }

        // Seitenruder (Rudder)
        private int RudderValue(int rudderTriggerValue)
        {
            return rudderTriggerValue;
        }

        private int ThrottleValue(int throttleValue)
        {
            // Begrenze throttleValue auf den Bereich von 0 bis 180
            throttleValue = Math.Max(0, Math.Min(throttleValue, 180));

            // Bereich für Nachbrenner (Triebwerk öffnen)
            if (throttleValue >= 0 && throttleValue <= 57)
            {
                return ScaleValue(throttleValue, 0, 57, 100, 65);
            }
            else
            {
                // Bereich für Schubregelung (normaler Betrieb)
                return ScaleValue(throttleValue, 58, 180, 65, 110);
            }
        }

        static int ScaleValue(int value, int oldMin, int oldMax, int newMin, int newMax)
        {
            // Lineare Transformation
            return (int)((value - oldMin) * ((double)(newMax - newMin) / (oldMax - oldMin)) + newMin);
        }


        public (
            int aileronRightIN, int aileronLeftIN,
            int aileronRightOUT, int aileronLeftOUT,
            int rudder, int engineThrottle,
            int canardRight, int canardLeft
        )
        InterpretJoystickValues(int joystickXValue, int joystickYValue, int throttleValue, int rudderTrigger)
        {
            return (
                AileronRightValue(joystickXValue, joystickYValue),
                AileronLeftValue(joystickXValue, joystickYValue),
                AileronRightOUTValue(joystickXValue, joystickYValue),
                AileronLefttOUTValue(joystickXValue, joystickYValue),
                RudderValue(rudderTrigger),
                ThrottleValue(throttleValue),
                CanardRightValue(joystickYValue, joystickXValue),
                CanardLeftValue(joystickYValue, joystickXValue)
            );
        }
    }
}
