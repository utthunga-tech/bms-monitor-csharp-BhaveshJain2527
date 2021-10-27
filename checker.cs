using System;
using System.Diagnostics;


public class Checker
{
    static int Main()
    {
        //Warning tolerance will consider as Percentage
        int warningTolerance = 5;

        BatteryHelper.CheckEarlyWarningForBattery(25f, 70f, 0.7f, warningTolerance);

        ExpectTrue(BatteryHelper.batteryIsOk(25f, 70f, 0.7f));
        ExpectFalse(BatteryHelper.batteryIsOk(50f, 85f, 0.0f));

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
        if (TemperatureHelper.IsTempOutOfRange(temperature))
        {
            ShowMessage("Temperature is out of range!");
            return false;
        }
        else if (SOCHelper.IsSocOutOfRange(soc))
        {
            ShowMessage("State of Charge is out of range!");
            return false;
        }
        else if (ChargeRateHelper.IsChargeRateOutOfRange(chargeRate))
        {
            ShowMessage("Charge Rate is out of range!");
            return false;
        }
        return true;
    }

    public static void CheckEarlyWarningForBattery(float temperature, float soc, float chargeRate, int warningTolerance)
    {
        if(!TemperatureHelper.Check_LOW_Temp_WARNING(temperature, warningTolerance))
        {
            ShowMessage("Warning: Temperature approaching discharge");
        }

        if (!TemperatureHelper.Check_HIGH_SOC_WARNING(temperature, warningTolerance))
        {
            ShowMessage("Warning: Temperature approaching charge-peak");
        }

        if (!SOCHelper.Check_LOW_SOC_WARNING(soc, warningTolerance))
        {
            ShowMessage("Warning: SOC approaching discharge");
        }

        if (!SOCHelper.Check_HIGH_SOC_WARNING(soc, warningTolerance))
        {
            ShowMessage("Warning: SOC approaching charge-peak");
        }

        if (!ChargeRateHelper.Check_LOW_CR_WARNING(chargeRate, warningTolerance))
        {
            ShowMessage("Warning: Charge rate approaching discharge");
        }

        if (!ChargeRateHelper.Check_HIGH_CR_WARNING(chargeRate, warningTolerance))
        {
            ShowMessage("Warning: Charge rate approaching charge-peak");
        }
    }

    public static double EvaluateToleranceValue(float value, int warningTolerance)
    {
        return (value * (warningTolerance / 100));
    }

    public static void ShowMessage(string message)
    {
        Console.WriteLine(message);
    }

}

public static class SOCHelper
{
    public static bool IsSocOutOfRange(float soc)
    {
        return (soc < 20 || soc > 80) ? true : false;
    }

    public static bool Check_LOW_SOC_WARNING(float soc, int warningTolerance)
    {
        double calculatedSOC = soc + BatteryHelper.EvaluateToleranceValue(soc, warningTolerance);

        return (soc > 20 && soc <= calculatedSOC) ? true : false;
    }

    public static bool Check_HIGH_SOC_WARNING(float soc, int warningTolerance)
    {
        double calculatedSOC = soc - BatteryHelper.EvaluateToleranceValue(soc,warningTolerance);

        return (soc >= calculatedSOC && soc <= 80) ? true : false;
    }
}

public static class TemperatureHelper
{
    public static bool IsTempOutOfRange(float temperature)
    {
        return (temperature < 0 || temperature > 45) ? true : false;
    }

    public static bool Check_LOW_Temp_WARNING(float temp, int warningTolerance)
    {
        double calculatedTemp = temp + BatteryHelper.EvaluateToleranceValue(temp, warningTolerance);

        return (temp > 20 && temp <= calculatedTemp) ? true : false;
    }

    public static bool Check_HIGH_SOC_WARNING(float temp, int warningTolerance)
    {
        double calculatedTemp = temp - BatteryHelper.EvaluateToleranceValue(temp, warningTolerance);

        return (temp >= calculatedTemp && temp <= 80) ? true : false;
    }
}

public static class ChargeRateHelper
{
    public static bool IsChargeRateOutOfRange(float chargeRate)
    {
        return (chargeRate > 0.8) ? true : false;
    }

    public static bool Check_LOW_CR_WARNING(float rate, int warningTolerance)
    {
        double calculatedRate = rate + BatteryHelper.EvaluateToleranceValue(rate, warningTolerance);

        return (rate > 20 && rate <= calculatedRate) ? true : false;
    }

    public static bool Check_HIGH_CR_WARNING(float rate, int warningTolerance)
    {
        double calculatedRate = rate - BatteryHelper.EvaluateToleranceValue(rate, warningTolerance);

        return (rate >= calculatedRate || rate <= 80) ? true : false;
    }

}
