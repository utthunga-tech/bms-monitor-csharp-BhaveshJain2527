using System;
using System.Diagnostics;


public class Checker
{
    static int Main()
    {
        BatteryResult batteryResult = new BatteryResult();

        ExpectTrue(batteryResult.Evaluate(25f, 70f, 0.7f));
        ExpectFalse(batteryResult.Evaluate(50f, 85f, 0.0f));

        static void ExpectTrue(bool expression)
        {
            if (!expression)
            {
                Console.WriteLine("Expected true, but got false");
                Environment.Exit(1);
            }
        }

        static void ExpectFalse(bool expression)
        {
            if (expression)
            {
                Console.WriteLine("Expected false, but got true");
                Environment.Exit(1);
            }
        }

        Console.WriteLine("All ok");
        return 0;
    }
}

public class BatteryHelper
{
    public BatteryHelper()
    {

    }
    public bool batteryIsOk(float temperature, float soc, float chargeRate)
    {
        Console.WriteLine(temperature.ToString());
        if (temperature < 0 || temperature > 45)
        {
            Console.WriteLine("Temperature is out of range!");
            return false;
        }
        else if (soc < 20 || soc > 80)
        {
            Console.WriteLine("State of Charge is out of range!");
            return false;
        }
        else if (chargeRate > 0.8)
        {
            Console.WriteLine("Charge Rate is out of range!");
            return false;
        }
        return true;
    }
}

public class BatteryResult
{
    BatteryHelper helper;
    public BatteryResult()
    {
        helper = new BatteryHelper();
    }

    public bool Evaluate(float temperature, float soc, float chargeRate)
    {
        return helper.batteryIsOk(temperature, soc, chargeRate);
    }
}
