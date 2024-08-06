using HahnCargoTruckLoader.Logic;


Console.WriteLine("Hello, Truck-Loader!");
var truck = Initialize.LoadTruck();
Console.WriteLine("Today's Truck is a " + truck.TruckType);
var crates = Initialize.GetCrates();
Console.WriteLine("You need to load " + crates.Count + " crates into it. Have FUN!");

var loadingPlan = new LoadingPlan(truck, crates);
var loadingInstructions = loadingPlan.GetLoadingInstructions();

Console.WriteLine("Checking Loading Plan...");
LoadingSimulator sim = new LoadingSimulator(truck, crates);
var result = sim.RunSimulation(loadingInstructions);

Console.WriteLine(result ? "The plan does work!" : "The plan does NOT work!");
Console.WriteLine("Hit any key to end the sim");
Console.ReadKey();

