using System;
using System.Diagnostics;


public class Checker
{
    static int Main()
    {
        Console.WriteLine("Please select the Language");
        Console.WriteLine("Enter 1 for English");
        Console.WriteLine("Enter 2 for German");

        int language = int.Parse(Console.ReadLine());
        if (language == 1 || language == 2)
        {
            if (language == 1)
            {
                MessageHelper.SetLanguage(true);
            }
            else
            {
                MessageHelper.SetLanguage(false);
            }

            Console.WriteLine(language == 1 ? "English" : "German");
            //Warning tolerance will consider as Percentage
            int warningTolerance = 5;

            BatteryHelper.CheckEarlyWarningForBattery(25f, 70f, 0.7f, warningTolerance);

            ExpectTrue(BatteryHelper.batteryIsOk(25f, 70f, 0.7f));
            ExpectFalse(BatteryHelper.batteryIsOk(50f, 85f, 0.0f));

            static void ExpectTrue(bool expression)
            {
                if (!expression)
                {
                    BatteryHelper.ShowMessage(MessageHelper.GetExpectedTrue());
                    Environment.Exit(1);
                }
            }

            static void ExpectFalse(bool expression)
            {
                if (expression)
                {
                    BatteryHelper.ShowMessage(MessageHelper.GetExpectedFalse());
                    Environment.Exit(1);
                }
            }

            BatteryHelper.ShowMessage(MessageHelper.GetAllOk());
            return 0;
        }
        else
        {
            Console.WriteLine("Invalid Option - Please restart the application");
            return 0;
        }
    }
}

public static class BatteryHelper
{
    public static bool batteryIsOk(float temperature, float soc, float chargeRate)
    {
        if (TemperatureHelper.IsTempOutOfRange(temperature))
        {
            ShowMessage(MessageHelper.GetTempOutOfRange());
            return false;
        }
        else if (SOCHelper.IsSocOutOfRange(soc))
        {
            ShowMessage(MessageHelper.GetSocOutOfRange());
            return false;
        }
        else if (ChargeRateHelper.IsChargeRateOutOfRange(chargeRate))
        {
            ShowMessage(MessageHelper.GetChargeRateOutOfRange());
            return false;
        }
        return true;
    }

    public static void CheckEarlyWarningForBattery(float temperature, float soc, float chargeRate, int warningTolerance)
    {
        if(!TemperatureHelper.Check_LOW_Temp_WARNING(temperature, warningTolerance))
        {
            ShowMessage(MessageHelper.GetCheck_LOW_Temp_WARNING());
        }

        if (!TemperatureHelper.Check_HIGH_Temp_WARNING(temperature, warningTolerance))
        {
            ShowMessage(MessageHelper.GetCheck_HIGH_Temp_WARNING());
        }

        if (!SOCHelper.Check_LOW_SOC_WARNING(soc, warningTolerance))
        {
            ShowMessage(MessageHelper.GetCheck_LOW_SOC_WARNING());
        }

        if (!SOCHelper.Check_HIGH_SOC_WARNING(soc, warningTolerance))
        {
            ShowMessage(MessageHelper.GetCheck_HIGH_SOC_WARNING());
        }

        if (!ChargeRateHelper.Check_LOW_CR_WARNING(chargeRate, warningTolerance))
        {
            ShowMessage(MessageHelper.GetCheck_LOW_CR_WARNING());
        }

        if (!ChargeRateHelper.Check_HIGH_CR_WARNING(chargeRate, warningTolerance))
        {
            ShowMessage(MessageHelper.GetCheck_HIGH_CR_WARNING());
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

    public static bool Check_HIGH_Temp_WARNING(float temp, int warningTolerance)
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

public static class MessageHelper
{
    static bool IsEnglish;

    public static void SetLanguage(bool isEng)
    {
        IsEnglish = isEng;
    }

    public static string GetTempOutOfRange()
    {
        return IsEnglish ? "Temperature is out of range!" : "Temperatur ist außerhalb des zulässigen Bereichs!";
    }
    public static string GetSocOutOfRange()
    {
        return IsEnglish ? "State of Charge is out of range!" : "Ladezustand ist außer Reichweite!";
    }

    public static string GetChargeRateOutOfRange()
    {
        return IsEnglish ? "Charge Rate is out of range!" : "Laderate ist außerhalb des zulässigen Bereichs!";
    }

    public static string GetCheck_LOW_Temp_WARNING()
    {
        return IsEnglish ? "Warning: Temperature approaching discharge" : "Warnung: Temperatur nähert sich der Entladung";
    }
    public static string GetCheck_HIGH_Temp_WARNING()
    {
        return IsEnglish ? "Warning: Temperature approaching charge-peak" : "Warnung: Temperatur nähert sich der Ladespitze";
    }

    public static string GetCheck_LOW_SOC_WARNING()
    {
        return IsEnglish ? "Warning: SOC approaching discharge" : "Warnung: SOC nähert sich der Entladung";
    }

    public static string GetCheck_HIGH_SOC_WARNING()
    {
        return IsEnglish ? "Warning: SOC approaching charge-peak" : "Warnung: SOC nähert sich der Ladespitze";
    }

    public static string GetCheck_LOW_CR_WARNING()
    {
        return IsEnglish ? "Warning: Charge rate approaching discharge" : "Warnung: Laderate nähert sich der Entladung";
    }

    public static string GetCheck_HIGH_CR_WARNING()
    {
        return IsEnglish ? "Warning: Charge rate approaching charge-peak" : "Warnung: Laderate nähert sich der Ladespitze";
    }

    public static string GetAllOk()
    {
        return IsEnglish ? "All Ok" : "Alles ok";
    }

    public static string GetExpectedTrue()
    {
        return IsEnglish ? "Expected true, but got false" : "Erwartet wahr, wurde aber falsch";
    }

    public static string GetExpectedFalse()
    {
        return IsEnglish ? "Expected false, but got true" : "Falsch erwartet, aber wahr geworden";
    }

}
