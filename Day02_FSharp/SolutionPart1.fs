module SolutionPart1

let rec lineToDigits (line: string) : string =
    if line.Length = 0 then
        line
    else
        match line.Chars 0 with
        | '0' -> "0" + lineToDigits (line.Substring 1)
        | '1' -> "1" + lineToDigits (line.Substring 1)
        | '2' -> "2" + lineToDigits (line.Substring 1)
        | '3' -> "3" + lineToDigits (line.Substring 1)
        | '4' -> "4" + lineToDigits (line.Substring 1)
        | '5' -> "5" + lineToDigits (line.Substring 1)
        | '6' -> "6" + lineToDigits (line.Substring 1)
        | '7' -> "7" + lineToDigits (line.Substring 1)
        | '8' -> "8" + lineToDigits (line.Substring 1)
        | '9' -> "9" + lineToDigits (line.Substring 1)
        | _ -> lineToDigits (line.Substring 1)

let determineColorCount (color: char) (matchStr: string): int =
    matchStr.Split ','
    |> Array.filter (fun x -> x.Contains color)
    |> (fun x ->
        if x.Length <> 0 then
            lineToDigits x[0]
            |> int
        else
            0)

let processMatch (input: string) =
    let MAX_RED = 12
    let MAX_GREEN = 13
    let MAX_BLUE = 14
    let red = determineColorCount 'd' input
    let green = determineColorCount 'g' input
    let blue = determineColorCount 'b' input
    // printfn $"red: {red}, green: {green}, blue: {blue}"
    not (red > MAX_RED) && not (green > MAX_GREEN) && not (blue > MAX_BLUE)

let processLine (line: string) =
    let splitLine = line.Split ':'
    let gameNumber = (splitLine[0].Split ' ')[1] |> int
    let matches = splitLine[1].Split ';'
    Array.map processMatch matches |> ignore
    if Array.forall processMatch matches then
        gameNumber
    else
        0

let solution (input: string) =
    input.Split '\n'
    |> Array.map processLine
    |> Array.sum