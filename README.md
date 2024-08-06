Project Overview

This project implements an algorithm to determine a valid loading plan for stacking crates into a truck's cargo bay. The crates have specific dimensions, and the goal is to fit all the crates in the truck without any overlap, adhering to the truck's dimensions. The solution utilizes a combination of random initialization and simulated annealing to optimize crate placement.

Algorithm Explanation

Step 1: Initialization
Truck and Crates Initialization: The truck's dimensions and the list of crates are initialized.
Loading Instructions and Cargo Space: An initial setup of loading instructions and a 3D array to track the cargo space is created.
Step 2: Generate Initial Solution
Random Placement: Crates are placed randomly in the truck. For each crate:
Random positions and orientations are tried until a valid position without overlap is found.
The crate's position and orientation are recorded as a loading instruction.
Step 3: Optimize Solution Using Simulated Annealing
Initial Solution: The randomly generated solution is taken as the starting point.
Simulated Annealing Process:
Temperature Control: Begins with a high temperature, gradually cooling down.
Neighbor Generation: Slightly modified versions of the current solution are generated by changing the position/orientation of a random crate.
Energy Calculation: The number of overlaps or invalid placements in the solution is measured.
Acceptance Probability: Determines whether to accept the new solution based on its energy and the current temperature.
Solution Update: If the new solution is better or probabilistically acceptable, it replaces the current solution.
Convergence: The process continues until the temperature is sufficiently low.
Step 4: Final Solution
Best Solution: The best-found solution is taken as the final loading plan.
Instruction Output: The final loading instructions are displayed and returned.

Explanation Video
For a detailed explanation of the algorithm and its implementation, please watch the video [here](https://drive.google.com/file/d/1g0wDd7X7YWQGDALd7AXU6FJ6VIndTC0D/view?usp=drivesdk).
