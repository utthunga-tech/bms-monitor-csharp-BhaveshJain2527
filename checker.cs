using System;
using System.Diagnostics;


public class Checker
{
    static int Main()
    {
        ExpectTrue(BatteryResult.Evaluate(25f, 70f, 0.7f));
        ExpectFalse(BatteryResult.Evaluate(50f, 85f, 0.0f));

        static void ExpectTrue(bool expression)
        {
            if (!expression)
            {
                BatteryHelper.ShowMessage("Expected true, but got false");
                Environment.Exit(1);
            }
        }

        static void ExpectFalse(bool expression)
        {
            if (expression)
            {
                BatteryHelper.ShowMessage("Expected false, but got true");
                Environment.Exit(1);
            }
        }

        BatteryHelper.ShowMessage("All ok");
        return 0;
    }
}

public static class BatteryHelper
{
    public static bool batteryIsOk(float temperature, float soc, float chargeRate)
    {
        if (IsTempOutOfRange(temperature))
        {
            ShowMessage("Temperature is out of range!");
            return false;
        }
        else if (IsSocOutOfRange(soc))
        {
            ShowMessage("State of Charge is out of range!");
            return false;
        }
        else if (IsChargeRateOutOfRange(chargeRate))
        {
            ShowMessage("Charge Rate is out of range!");
            return false;
        }
        return true;
    }

    public static void ShowMessage(string message)
    {
        Console.WriteLine(message);
    }

    public static bool IsTempOutOfRange(float temperature)
    {
        return (temperature < 0 || temperature > 45) ? true : false;
    }

    public static bool IsSocOutOfRange(float soc)
    {
        return (soc < 20 || soc > 80) ? true : false;
    }

    public static bool IsChargeRateOutOfRange(float chargeRate)
    {
        return (chargeRate > 0.8) ? true : false;
    }
}

public static class BatteryResult
{
    public static bool Evaluate(float temperature, float soc, float chargeRate)
    {
        return BatteryHelper.batteryIsOk(temperature, soc, chargeRate);
    }
}
