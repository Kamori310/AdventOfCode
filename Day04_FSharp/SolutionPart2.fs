module SolutionPart2

(*
1 Access lines by index and work each item recursive (could be pour performing
  because each line is processes multiple times)
2 Change data structure add counter to each line and increase it depending on the
  outcome of each line
*)
let printArray input =
    Array.map (fun x -> printfn $"{x}") input |> ignore
    input

let trimString (input: string): string =
    input.Trim()

let processLine (line: string): int =
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