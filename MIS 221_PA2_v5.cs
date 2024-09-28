
using System;
using System.Collections.Generic; //allows use of List and Dictionary classes
using System.IO; //allows use of Logs
class ReturnToMainMenuException : Exception{}

class Program{ //start program
    static void Main(){
        while(true){
            try{
                MainMenu();
                string userInput = GetMenuInput(); //loop priming
                while(userInput != "3"){ //condition control
                    RouteUser(userInput);
                    MainMenu();
                    userInput = GetMenuInput(); //update read
                }
                RouteUser(userInput);
            }
            catch(ReturnToMainMenuException){ //catch to return to main menu after sub menu
                continue;
            }
        }
    }

    static void MainMenu(){
        Console.Clear();
        Console.WriteLine("Please select a menu choice\n1. Convert Units of Measure\n2. Rock Classification\n3. Exit");
    }

    static string GetMenuInput(){
        return(Console.ReadLine());
    }

    static void RouteUser(string userInput){
        switch(userInput){
            case "1":
                UnitConvertMenu();
                break;
            case "2":
                RockClassification();
                break;
            case "3":
                Exit();
                break;
            default:
                Console.WriteLine("Invalid menu selection.");
                Pause();
                break;
        }
    }

    //PART 1: UNIT CONVERSION

    static void UnitConvertMenu(){
        string unitType = "";
        while(unitType != "4"){
            Console.Clear();
            Console.WriteLine("Please select a unit type\n1. Length\n2. Mass\n3. Temperature\n4. Back");
            unitType = GetUnitMenuInput();
            RouteUnit(unitType);
        }
    }

    static string GetUnitMenuInput(){
        return(Console.ReadLine());
    }

    static void RouteUnit(string unitType){
        switch(unitType){
            case "1":
                Length();
                break;
            case "2":
                Mass();
                break;
            case "3":
                Temperature();
                break;
            case "4":
                Main();
                break;
            default:
                Console.WriteLine("Invalid menu selection.");
                Pause();
                break;
        }
    }

    static void Length(){
    List<string> validUnits = new List<string> { "mm", "cm", "m", "km", "in", "ft", "yd", "mi" };

    while (true){ //prime loop
        Console.Clear();
        string fromUnit = GetUnit("Enter the unit you want to convert from", validUnits, false, true, false);
        Console.Clear();
        string toUnit = GetUnit("Enter the unit you want to convert to", validUnits, false, true, false);

        Console.Clear();
        Console.WriteLine("Enter the value you want to convert:");
        double unitValue;
        while (!double.TryParse(Console.ReadLine(), out unitValue)){
            Console.WriteLine("Invalid input. Please enter a numeric value.");
        }

        double convertedUnitValue = ConvertUnit(fromUnit, toUnit, unitValue, "length");
        if (convertedUnitValue != double.MinValue){
            Console.WriteLine($"You converted {unitValue:F4}{fromUnit} to {convertedUnitValue:F4}{toUnit}");
            break; //exit loop
        }
        else{
            Console.WriteLine("Invalid unit conversion"); //error handle in case of task failure
            Pause();
        }
    }
    Pause();
    throw new ReturnToMainMenuException(); //return to main menu after task completion
}

    static void Mass(){
        List<string> validUnits = new List<string>{"g", "kg", "oz", "lb"};

        while(true){ //prime loop
            Console.Clear();
            string fromUnit = GetUnit("Enter the unit you want to convert from", validUnits, false, false, true);
            Console.Clear();
            string toUnit = GetUnit("Enter the unit you want to convert to", validUnits, false, false, true);
            
            Console.Clear();
            Console.WriteLine("Enter the value you want to convert:");
            double unitValue;
            while(!double.TryParse(Console.ReadLine(), out unitValue)){
                Console.WriteLine("Invalid input. Please enter a numeric value.");
            }

            double convertedUnitValue = ConvertUnit(fromUnit, toUnit, unitValue, "mass");
            if(convertedUnitValue != double.MinValue){
                Console.WriteLine($"You converted {unitValue:F4}{fromUnit} to {convertedUnitValue:F4}{toUnit}");
                break; //exit loop
            }
            else{
                Console.WriteLine("Invalid unit conversion."); //error handle in case of task failure
                Pause();
            }
        }
        Pause();
        throw new ReturnToMainMenuException(); //return to main menu after task completion
    }

    static void Temperature(){
        List<string> validUnits = new List<string>{"C", "F", "K"};

        while(true){ //prime loop
            Console.Clear();
            string fromUnit = GetUnit("Enter the unit you want to convert from:", validUnits, true, false, false);
            Console.Clear();
            string toUnit = GetUnit("Enter the unit you want to convert to:", validUnits, true, false, false);
            
            Console.Clear();
            Console.WriteLine("Enter the value you want to convert:");
            double unitValue;
            while(!double.TryParse(Console.ReadLine(), out unitValue)){
                Console.WriteLine("Invalid input. Please enter a numeric value.");
            }

            double convertedUnitValue = ConvertUnit(fromUnit, toUnit, unitValue, "temperature");
            if(convertedUnitValue != double.MinValue){
                string fromUnitSymbol = fromUnit == "K" ? "" : "°";
                string toUnitSymbol = toUnit == "K" ? "" : "°";
                Console.WriteLine($"You converted {unitValue:F4}{fromUnitSymbol}{fromUnit} to {convertedUnitValue:F4}{toUnitSymbol}{toUnit}");
                break; //exit loop
            }
            else{
                Console.WriteLine("Invalid unit conversion."); //error handle in case of task failure
                Pause();
            }
        }
        Pause();
        throw new ReturnToMainMenuException(); //return to main menu after task completion
    }

    static string GetUnit(string prompt, List<string> validUnits, bool toUpperCase, bool isLength, bool isMass){ //parameters for unit validation
        while (true){
            Console.WriteLine(prompt);
            if(isLength){
                Console.WriteLine("mm\tin\ncm\tft\nm\tyd\nkm\tmi"); //length units
            }
            else if(isMass){
                Console.WriteLine("g\toz\nkg\tlb"); //mass units
            }
            else{
                Console.WriteLine("F\tC\tK"); //temp units
            }
            string unit = Console.ReadLine();
            unit = toUpperCase ? unit.ToUpper() : unit.ToLower(); //convert input to upper or lower case based on toUpperCase parameter
            if (validUnits.Contains(unit)){ //check input for valid unit in lists
                return unit;
            }
            else{
                Console.WriteLine("Invalid unit. Please enter a valid unit.");
                Pause();
                Console.Clear();
            }
        }
    }

    static double ConvertUnit(string fromUnit, string toUnit, double value, string unitType){
        Dictionary<string, double> lengthUnits = new Dictionary<string, double>{ //stores conversion factors for lenght units
            { "mm", 0.001 },
            { "cm", 0.01 },
            { "m", 1 },
            { "km", 1000 },
            { "in", 0.0254 },
            { "ft", 0.3048 },
            { "yd", 0.9144 },
            { "mi", 1609.34 }
        };

        Dictionary<string, double> massUnits = new Dictionary<string, double>{ //stores conversion factors for mass units
            {"g", 0.001},
            {"kg", 1},
            {"oz", 0.0283495},
            {"lb", 0.453592}
        };

        var temperatureConversions = new Dictionary<(string, string), Func<double, double>>{ //stores conversion factors for temp units
            {("C", "F"), c => (c * 9 / 5) + 32},
            {("F", "C"), f => (f - 32) * 5 / 9},
            {("C", "K"), c => c + 273.15},
            {("K", "C"), k => k - 273.15},
            {("F", "K"), f => (f - 32) * 5 / 9 + 273.15},
            {("K", "F"), k => (k - 273.15) * 9 / 5 + 32}
        };

        if(fromUnit == toUnit){
            return value; //no conversion needed if units are the same
        }

        switch(unitType.ToLower()){ //switch to determine which dictionary to use based on initial unitType selection
            case "length":
                if(lengthUnits.ContainsKey(fromUnit) && lengthUnits.ContainsKey(toUnit)){
                    double valueInMeters = value * lengthUnits[fromUnit]; //convert fromUnit to meters
                    return valueInMeters / lengthUnits[toUnit]; //convert meters to toUnit
                }
                break;
            case "mass":
                if(massUnits.ContainsKey(fromUnit) && massUnits.ContainsKey(toUnit)){
                    double valueInKg = value * massUnits[fromUnit]; //convert fromUnit to kilograms
                    return valueInKg / massUnits[toUnit]; //convert kilograms to toUnit
                }
                break;
            case "temperature":
                if(temperatureConversions.TryGetValue((fromUnit, toUnit), out var conversionFunction)){
                    return conversionFunction(value); //apply conversion function for complex temp calculations
                }
                break;
        }
        return double.MinValue; //returns special value to indicate an error

    }

    //PART 2: ROCK CLASSIFICATION

    static void RockClassification(){
        int logCount = InitializedLogCount() + 1; //initialize and increment log count
        double totalPoints = 0;
        double adjustmentTotal = 0;

        var(identicalRockPoints, sampleSize) = GetIdenticalRockPoints(); //unpack tuple from method into variables
        totalPoints += identicalRockPoints;
        totalPoints += GetTransportPoints();
        totalPoints += GetTemperaturePoints();
        totalPoints = ApplyWeightBonus(totalPoints, sampleSize);

        Console.Clear();
        Console.WriteLine($"Total points for the rock classification: {totalPoints:F2}");
        Log(logCount, $"Initial classification: {totalPoints:F2} points");

        bool isSatisfied = false;
        while(!isSatisfied){
            Console.WriteLine("\nAre you satisfied with this classification? (yes/no):");
            string satisfied = Console.ReadLine().ToLower();
            isSatisfied = satisfied == "yes";
            while(!isSatisfied && satisfied != "no"){
                Console.WriteLine("Invalid input. Please enter 'yes' or 'no'.");
                satisfied = Console.ReadLine().ToLower();
                isSatisfied = satisfied == "yes";
            }

            if(!isSatisfied){
                Console.Clear();
                Console.WriteLine($"Current Total Points: {totalPoints:F2}");
                Console.WriteLine("\nEnter the point value to adjust (positive to increase, negative to decrease): ");
                double adjustment;
                while(!double.TryParse(Console.ReadLine(), out adjustment)){
                    Console.WriteLine("Invalid input. Please enter a numeric value.");
                }
                if(Math.Abs(adjustment) > totalPoints){ //absolute value of adjustment points
                    Console.WriteLine("Error: Adjustment cannot be greater than the original point value.");
                }
                else{
                totalPoints += adjustment;
                adjustmentTotal += adjustment;
                Console.Clear(); //clear screen before displaying updated points
                Console.WriteLine($"Updated Total Points: {totalPoints:F2}");
                Console.WriteLine($"Total Adjustment: {(adjustmentTotal >= 0? "+" : "")}{adjustmentTotal:F2}");
                Log(logCount, $"Adjustment made: {adjustment:F2} points, New total: {totalPoints:F2} points");
                }
            }
        }
        Console.Clear();
        Console.WriteLine($"Final Total Points: {totalPoints:F2}");
        Console.WriteLine($"Total Adjustment Made: {(adjustmentTotal >= 0 ? "+" : "")}{adjustmentTotal:F2}");
        Log(logCount, $"Final classification: {totalPoints:F2} points, Total adjustments: {adjustmentTotal:F2} points");
        IncrementLogCount(logCount); //save the incremented log count
        Pause();
    }

    static (double points, int sampleSize) GetIdenticalRockPoints(){
        Console.Clear();
        Console.WriteLine("Enter the total number of rock samples:");
        int totalSamples;
        while(!int.TryParse(Console.ReadLine(), out totalSamples) || totalSamples < 0){
            Console.WriteLine("Invalid input. Please enter a non-negative integer.");
        }

        Console.WriteLine("Enter the number of identical rock samples found:");
        int identicalSamples;
        while(!int.TryParse(Console.ReadLine(), out identicalSamples) || identicalSamples < 0 || identicalSamples > totalSamples){
            Console.WriteLine("Invalid input. Please enter a non-negative integer that is less than or equal to the total number of samples.");
        }
        double points = identicalSamples * 4.5;
        return (points, totalSamples);
    }

    static double GetTransportPoints(){
        Console.WriteLine("\nDo you need the rock samples transported? (yes/no):");
        string transport;
        while(true){
            transport = Console.ReadLine().ToLower();
            if(transport == "yes" || transport == "no"){
                break;
            }
            Console.WriteLine("Invalid input. Please enter 'yes' or 'no'.");
        }
        if(transport == "yes"){
            return 7.3;
        }
        return 0;
    }

    static double GetTemperaturePoints(){
        Console.WriteLine("\nEnter the surface temperature of the rock samples (in °C):");
        double temperature;
        while(!double.TryParse(Console.ReadLine(), out temperature)){
            Console.WriteLine("Invalid input. Please enter a numeric value.");
        }
        if(temperature <= 0){
            return 9.2;
        }
        return 0;
    }

    static double ApplyWeightBonus(double totalPoints, int sampleSize, int threshold = 5){
        double totalWeight = 0;
        if(sampleSize <= threshold){ //adjustable threshold for preference and needs
            //prompt for individual sample weights
            for(int i = 0; i < sampleSize; i++){
                Console.WriteLine($"\nEnter the weight of rock sample {i + 1} (in kg):");
                double weight;
                while(!double.TryParse(Console.ReadLine(), out weight) || weight < 0){
                    Console.WriteLine("Invalid input. Please enter a non-negative number.");
                }
                totalWeight += weight;
            }
        }
        else{
            //prompt for total sample weight
            Console.WriteLine("\nEnter the total weight of all rock samples (in kg):");
            while(!double.TryParse(Console.ReadLine(), out totalWeight) || totalWeight < 0){
                Console.WriteLine("Invalid input. Please enter a non-negative number.");
            }
        }
        if(totalWeight > 25){
            totalPoints *= 1.17; //applies 17% increase
        }
        return totalPoints; //returns the updated total points
    }

    //Logging methods to review Sample Log results
    static int InitializedLogCount(){
        if(!File.Exists("log_count.txt")){
            File.WriteAllText("log_count.txt", "0");
        }
        return int.Parse(File.ReadAllText("log_count.txt"));
    }

    static void IncrementLogCount(int count){
        File.WriteAllText("log_count.txt", count.ToString());
    }

    static void Log(int logCount, string message){
        using(StreamWriter writer = new StreamWriter("log.txt", true)){
            writer.WriteLine($"Sample Log {logCount}: {DateTime.Now}: {message}");
        }
    }

    //Additional methods for handling
    static void Exit(){
        Console.WriteLine();
        Console.WriteLine("Closing the program.");
        Console.WriteLine();
        Environment.Exit(0); //terminates the program
    }

    static void Pause(){
        Console.WriteLine();
        Console.WriteLine("Press any key to continue...");
        Console.ReadKey();
    }
} // end program