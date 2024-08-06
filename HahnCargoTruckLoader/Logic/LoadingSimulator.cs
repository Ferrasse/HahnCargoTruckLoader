using HahnCargoTruckLoader.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HahnCargoTruckLoader.Logic
{
  public class LoadingSimulator
  {
    private readonly bool[,,] cargoSpace;
    private readonly List<Crate> crates;

    public LoadingSimulator(Truck truck, List<Crate> cratesToLoad)
    {
      cargoSpace = new bool[truck.Width, truck.Height, truck.Length];

      for (int w = 0; w < cargoSpace.GetLength(0); w++)
      {
        for (int h = 0; h < cargoSpace.GetLength(1); h++)
        {
          for (int l = 0; l < cargoSpace.GetLength(2); l++)
          {
            cargoSpace[w, h, l] = false;
          }
        }
      }

      crates = cratesToLoad;
    }

    public bool RunSimulation(Dictionary<int, LoadingInstruction> instructions)
    {
      foreach (var instruction in instructions)
      {
        if(!LoadCrate(instruction.Value)) {
          return false;
        }
        crates.Remove(crates.Where(c => c.CrateID == instruction.Value.CrateId).First());
      }

      if (crates.Count > 0) return false;

      return true;
    }

        private bool LoadCrate(LoadingInstruction instruction)
        {
            var crate = crates.FirstOrDefault(c => c.CrateID == instruction.CrateId);
            if (crate == null) return false;

            crate.Turn(instruction);

            if (!WillCrateFitInCargoWithAndHight(crate, instruction)) return false;

            var startPosLenght = cargoSpace.GetLength(2) - 1;
            for (int l = startPosLenght; l >= 0; l--)
            {
                var blocked = false;
                for (int w = instruction.TopLeftX; w < crate.Width; w++)
                {
                    for (int h = instruction.TopLeftY; h < crate.Height; h++)
                    {
                        if (cargoSpace[w, h, l] == true)
                        {
                            blocked = true;
                            break;
                        }
                    }
                    if (blocked) break;
                }
                if (blocked) break;

                if (startPosLenght > 0) startPosLenght--;
            }

            if (!WillCrateFitInCargoLenght(crate, startPosLenght)) return false;

            for (int l = startPosLenght; l < crate.Length; l++)
            {
                for (int w = instruction.TopLeftX; w < crate.Width; w++)
                {
                    for (int h = instruction.TopLeftY; h < crate.Height; h++)
                    {
                        cargoSpace[w, h, l] = true;
                    }
                }
            }

            return true;
        }



        private bool WillCrateFitInCargoWithAndHight(Crate crate, LoadingInstruction instruction)
    {
      if (instruction.TopLeftX + crate.Width > cargoSpace.GetLength(0) ||
          instruction.TopLeftY + crate.Height > cargoSpace.GetLength(1)) return false;

      return true;
    }

    private bool WillCrateFitInCargoLenght(Crate crate, int startPosLenght)
    {
      if (startPosLenght + crate.Length > cargoSpace.GetLength(2)) return false;

      return true;
    }


  }
}
