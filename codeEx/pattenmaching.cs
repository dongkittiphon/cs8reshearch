using System;
using ConsumerVehicleRegistration;
using CommercialRegistration;
using LiveryRegistration;

namespace ExploreCsharpEight
{
class Patterns
    {
//Switch expressions
//different source of data Using pattern matching features to extend data types
        public decimal CalculateToll(object vehicle) =>
                    vehicle switch
                    {
                        //Car c => 2.00m,
                        // property pattern
                        Car { Passengers: 0 } => 2.00m + 0.50m,
                        Car { Passengers: 1 } => 2.0m,
                        Car { Passengers: 2 } => 2.0m - 0.50m,
                        Car _ => 2.00m - 1.0m,

                        //Taxi t => 3.50m,
                        //nested switches
                        Taxi t => t.Fares switch
                        {
                            0 => 3.50m + 1.00m,
                            1 => 3.50m,
                            2 => 3.50m - 0.50m,
                            _ => 3.50m - 1.00m
                        },

                        //Bus b => 5.00m,
                        // Positional patterns
                        Bus b when ((double)b.Riders / (double)b.Capacity) > 0.90 => 5.00m - 1.00m,
                        DeliveryTruck t => 10.00m,

                        { } => throw new ArgumentException(message: "Not a known vehicle type", paramName: nameof(vehicle)),
                        null => throw new ArgumentNullException(nameof(vehicle))
                    };

        //tuple pattern
        private enum TimeBand
        {
            MorningRush,
            Daytime,
            EveningRush,
            Overnight
        }
         private static TimeBand GetTimeBand(DateTime timeOfToll)
        {
            int hour = timeOfToll.Hour;
            if (hour < 6)
                return TimeBand.Overnight;
            else if (hour < 10)
                return TimeBand.MorningRush;
            else if (hour < 16)
                return TimeBand.Daytime;
            else if (hour < 20)
                return TimeBand.EveningRush;
            else
                return TimeBand.Overnight;
        }

        private static bool IsWeekDay(DateTime timeOfToll) =>
            timeOfToll.DayOfWeek switch
            {
                DayOfWeek.Saturday => false,
                DayOfWeek.Sunday => false,
                _ => true
            };
        public decimal PeakTimePremiumFull(DateTime timeOfToll, bool inbound) =>
            (IsWeekDay(timeOfToll), GetTimeBand(timeOfToll), inbound) switch
            {
                (true,  TimeBand.MorningRush, true)  => 2.00m,
                (true,  TimeBand.MorningRush, false) => 1.00m,
                (true,  TimeBand.Daytime,     true)  => 1.50m,
                (true,  TimeBand.Daytime,     false) => 1.50m,
                (true,  TimeBand.EveningRush, true)  => 1.00m,
                (true,  TimeBand.EveningRush, false) => 2.00m,
                (true,  TimeBand.Overnight,   true)  => 0.75m,
                (true,  TimeBand.Overnight,   false) => 0.75m,
                (false, TimeBand.MorningRush, true)  => 1.00m,
                (false, TimeBand.MorningRush, false) => 1.00m,
                (false, TimeBand.Daytime,     true)  => 1.00m,
                (false, TimeBand.Daytime,     false) => 1.00m,
                (false, TimeBand.EveningRush, true)  => 1.00m,
                (false, TimeBand.EveningRush, false) => 1.00m,
                (false, TimeBand.Overnight,   true)  => 1.00m,
                (false, TimeBand.Overnight,   false) => 1.00m,
            };

        public decimal PeakTimePremium(DateTime timeOfToll, bool inbound) =>
                (IsWeekDay(timeOfToll), GetTimeBand(timeOfToll), inbound) switch
                {
                    (true, TimeBand.Overnight, _) => 0.75m,
                    (true, TimeBand.Daytime, _) => 1.5m,
                    (true, TimeBand.MorningRush, true) => 2.0m,
                    (true, TimeBand.EveningRush, false) => 2.0m,
                    (_, _, _) => 1.0m,
                };

}
}