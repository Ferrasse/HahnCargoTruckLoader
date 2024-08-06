## The setting: 

You are working for a transport company. The company wants to know how to stack all boxes from an order into a truck.
A **truck** has the measurements of its cargo bay (width, height, length). Also there is a list of all the crates that need to go onto the truck.
All crates are **rectangular** and offer next to an ID their measurements.
There are some assumptions for the loading process:
* A crate can only be turned before it is moved into the truck.
* A crate can be fixed at its destination - you don't neet to worrie about floating or tipping over.
* A crate can't stay in or pass space, that is already blocked by another loaded crate.
 
Your job is to write code that will output a **LoadingPlan** for **all** crates of a given list and a truck.

## The challenge: 

We want you to write an algorithm, that finishes the given programm. 
In the end, the **GetLoadingInstructions** method of the **LoadingPlan** class needs to return valid loading instructions.

The given data (truck, list of crates) are only an example and can be switched out. Your algorithem should still be working.
 
Send in you solution in your own git repo. 

Finally, record a video explaining your solution (in English).
 
Additional remarks:
- We know that there are a lot of packages available. But choose them carefully, since we want to hire you, not the package author.

## The technical background: 

* You should use the given code without changing it (e.g. Model classes, given method headers).
* Your solution goes into the **GetLoadingInstructions** method of the **LoadingPlan** class.
* You can add new classes and methods if you need them.
* You can expect that there is a solution to put all crates on the truck.
* We estimate, that this task should be done in max. 4h.
* If you think, you found a bug: please let us know (best open up an issue here on git). 
