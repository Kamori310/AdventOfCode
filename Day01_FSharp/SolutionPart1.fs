module SolutionPart1

let rec lineToDigits (line: string): string =
  if line.Length = 0 then line
  else
    match line.[0] with
    | '1' -> "1" + lineToDigits line.[1..]
    | '2' -> "2" + lineToDigits line.[1..]
    | '3' -> "3" + lineToDigits line.[1..]
    | '4' -> "4" + lineToDigits line.[1..]
    | '5' -> "5" + lineToDigits line.[1..]
    | '6' -> "6" + lineToDigits line.[1..]
    | '7' -> "7" + lineToDigits line.[1..]
    | '8' -> "8" + lineToDigits line.[1..]
    | '9' -> "9" + lineToDigits line.[1..]
    | _ -> lineToDigits line.[1..]

let findNumberOfLine (line: string): int =
  let digits = lineToDigits line
  System.String.Concat ([digits.[0]] @ [digits.[digits.Length - 1]])
  |> int

let solution (input: string): int =
  // Split string into lines
  let lines =
    input.Split '\n'
  // Find number of Line
  Array.map findNumberOfLine lines
  // Sum
  |> Array.sum