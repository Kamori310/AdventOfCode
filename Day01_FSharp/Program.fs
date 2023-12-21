let filePath = "../../../data/input.txt"
let input = System.IO.File.ReadAllText(filePath)
let solutionPart1 = SolutionPart1.solution input
let solutionPart2 = SolutionPart2.solution input

printfn $"Solution 1: {solutionPart1}"
printfn $"Solution 2: {solutionPart2}"
