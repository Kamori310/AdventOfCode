module SolutionPart1

let processLine (line: string) =
    let splitLine = line.Split '|'
    let winningNumbers =
        let gameSplit =
            splitLine[0].Split ':'
        gameSplit[1].Trim().Split ' '
        |> Array.filter (fun x -> match x with | "" -> false | _ -> true)
        |> Array.map int
    let myNumbers =
        splitLine[1].Trim().Split ' '
        |> Array.filter (fun x -> match x with | "" -> false | _ -> true)
        |> Array.map int

    myNumbers
    |> Array.filter (fun x -> winningNumbers |> Array.contains x)
    |> Array.length
    |> (fun length ->
        match length with
        | 0 -> 0
        | a -> pown 2 (a - 1))

let solution (input: string): int =
    input.Split '\n'
    |> Array.map processLine
    |> Array.sum