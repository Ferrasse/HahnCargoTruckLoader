using HahnCargoTruckLoader.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HahnCargoTruckLoader.Logic
{
    public class LoadingPlan
    {
        private readonly Truck _truck;
        private readonly List<Crate> _crates;
        private readonly Dictionary<int, LoadingInstruction> _instructions;
        private int _loadingStepNumber;
        private bool[,,] _cargoSpace;

        public LoadingPlan(Truck truck, List<Crate> crates)
        {
            _truck = truck;
            _crates = crates;
            _instructions = new Dictionary<int, LoadingInstruction>();
            _loadingStepNumber = 1;
            _cargoSpace = new bool[_truck.Width, _truck.Height, _truck.Length];
        }

        public Dictionary<int, LoadingInstruction> GetLoadingInstructions()
        {
            var solution = InitializeRandomSolution();
            solution = SimulatedAnnealing(solution);

            foreach (var instruction in solution)
            {
                _instructions[instruction.LoadingStepNumber] = instruction;
            }

            DisplayInstructions();

            return _instructions;
        }

        private void DisplayInstructions()
        {
            Console.WriteLine("Valid Loading Instructions:");
            foreach (var instruction in _instructions.Values.OrderBy(i => i.LoadingStepNumber))
            {
                Console.WriteLine($"Step {instruction.LoadingStepNumber}: Crate {instruction.CrateId} at ({instruction.TopLeftX}, {instruction.TopLeftY}, {instruction.TopLeftZ}) with TurnHorizontal={instruction.TurnHorizontal}, TurnVertical={instruction.TurnVertical}");
            }
        }

        private List<LoadingInstruction> InitializeRandomSolution()
        {
            var random = new Random();
            var solution = new List<LoadingInstruction>();

            foreach (var crate in _crates)
            {
                bool placed = false;
                while (!placed)
                {
                    int x = random.Next(0, _truck.Width - crate.Width + 1);
                    int y = random.Next(0, _truck.Height - crate.Height + 1);
                    int z = random.Next(0, _truck.Length - crate.Length + 1);

                    bool turnHorizontal = random.Next(2) == 1;
                    bool turnVertical = random.Next(2) == 1;

                    var instruction = new LoadingInstruction
                    {
                        LoadingStepNumber = _loadingStepNumber,
                        CrateId = crate.CrateID,
                        TopLeftX = x,
                        TopLeftY = y,
                        TopLeftZ = z,
                        TurnHorizontal = turnHorizontal,
                        TurnVertical = turnVertical
                    };

                    if (CanPlaceCrate(instruction, solution))
                    {
                        solution.Add(instruction);
                        PlaceCrate(instruction);
                        _loadingStepNumber++;
                        placed = true;
                    }
                }
            }

            return solution;
        }

        private List<LoadingInstruction> SimulatedAnnealing(List<LoadingInstruction> initialSolution)
        {
            double temperature = 1000.0;
            double coolingRate = 0.003;
            var currentSolution = new List<LoadingInstruction>(initialSolution);
            var bestSolution = new List<LoadingInstruction>(currentSolution);
            var random = new Random();

            while (temperature > 1)
            {
                var newSolution = GenerateNeighbor(currentSolution, random);
                double currentEnergy = CalculateEnergy(currentSolution);
                double newEnergy = CalculateEnergy(newSolution);

                if (AcceptanceProbability(currentEnergy, newEnergy, temperature) > random.NextDouble())
                {
                    currentSolution = new List<LoadingInstruction>(newSolution);
                }

                if (newEnergy < CalculateEnergy(bestSolution))
                {
                    bestSolution = new List<LoadingInstruction>(newSolution);
                }

                temperature *= 1 - coolingRate;
                Console.WriteLine($"Temperature: {temperature:F2}, Current Energy: {currentEnergy}, Best Energy: {CalculateEnergy(bestSolution)}");
            }

            return bestSolution;
        }

        private List<LoadingInstruction> GenerateNeighbor(List<LoadingInstruction> currentSolution, Random random)
        {
            var newSolution = new List<LoadingInstruction>(currentSolution);

            int index = random.Next(newSolution.Count);
            var crate = _crates.First(c => c.CrateID == newSolution[index].CrateId);

            bool placed = false;

            while (!placed)
            {
                int x = random.Next(0, _truck.Width - crate.Width + 1);
                int y = random.Next(0, _truck.Height - crate.Height + 1);
                int z = random.Next(0, _truck.Length - crate.Length + 1);

                bool turnHorizontal = random.Next(2) == 1;
                bool turnVertical = random.Next(2) == 1;

                var instruction = new LoadingInstruction
                {
                    LoadingStepNumber = newSolution[index].LoadingStepNumber,
                    CrateId = crate.CrateID,
                    TopLeftX = x,
                    TopLeftY = y,
                    TopLeftZ = z,
                    TurnHorizontal = turnHorizontal,
                    TurnVertical = turnVertical
                };

                newSolution[index] = instruction;

                if (CanPlaceCrate(instruction, newSolution))
                {
                    placed = true;
                }
            }

            return newSolution;
        }

        private double CalculateEnergy(List<LoadingInstruction> solution)
        {
            int overlaps = 0;
            bool[,,] cargoSpace = new bool[_truck.Width, _truck.Height, _truck.Length];

            foreach (var instruction in solution)
            {
                var crate = _crates.First(c => c.CrateID == instruction.CrateId);
                crate.Turn(instruction);

                for (int i = 0; i < crate.Width; i++)
                {
                    for (int j = 0; j < crate.Height; j++)
                    {
                        for (int k = 0; k < crate.Length; k++)
                        {
                            if (instruction.TopLeftX + i >= _truck.Width ||
                                instruction.TopLeftY + j >= _truck.Height ||
                                instruction.TopLeftZ + k >= _truck.Length ||
                                cargoSpace[instruction.TopLeftX + i, instruction.TopLeftY + j, instruction.TopLeftZ + k])
                            {
                                overlaps++;
                            }
                            else
                            {
                                cargoSpace[instruction.TopLeftX + i, instruction.TopLeftY + j, instruction.TopLeftZ + k] = true;
                            }
                        }
                    }
                }
            }

            return overlaps;
        }

        private double AcceptanceProbability(double currentEnergy, double newEnergy, double temperature)
        {
            if (newEnergy < currentEnergy)
            {
                return 1.0;
            }
            return Math.Exp((currentEnergy - newEnergy) / temperature);
        }

        private bool CanPlaceCrate(LoadingInstruction instruction, List<LoadingInstruction> solution)
        {
            var crate = _crates.First(c => c.CrateID == instruction.CrateId);
            crate.Turn(instruction);

            for (int i = 0; i < crate.Width; i++)
            {
                for (int j = 0; j < crate.Height; j++)
                {
                    for (int k = 0; k < crate.Length; k++)
                    {
                        if (instruction.TopLeftX + i >= _truck.Width ||
                            instruction.TopLeftY + j >= _truck.Height ||
                            instruction.TopLeftZ + k >= _truck.Length ||
                            _cargoSpace[instruction.TopLeftX + i, instruction.TopLeftY + j, instruction.TopLeftZ + k])
                        {
                            return false;
                        }
                    }
                }
            }

            return true;
        }

        private void PlaceCrate(LoadingInstruction instruction)
        {
            var crate = _crates.First(c => c.CrateID == instruction.CrateId);
            crate.Turn(instruction);

            for (int i = 0; i < crate.Width; i++)
            {
                for (int j = 0; j < crate.Height; j++)
                {
                    for (int k = 0; k < crate.Length; k++)
                    {
                        _cargoSpace[instruction.TopLeftX + i, instruction.TopLeftY + j, instruction.TopLeftZ + k] = true;
                    }
                }
            }
        }
    }
}


