using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HahnCargoTruckLoader.Model
{
  public class Crate
  {
    public int CrateID { get; set; }
    public int Width { get; set; }
    public int Height { get; set; }
    public int Length { get; set; }

    public void Turn(LoadingInstruction instruction)
    {
      if (instruction.TurnHorizontal)
      {
        (Width, Length) = (Length, Width);
      }

      if (instruction.TurnVertical)
      {
        (Width, Height) = (Height, Width);
      }
    }

  }
}
