let filename = "../../../data/input.txt"
let input = System.IO.File.ReadAllText filename
let solution1 = SolutionPart1.solution input
printfn $"Solution 1: {solution1}"