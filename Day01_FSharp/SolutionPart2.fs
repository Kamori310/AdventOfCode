module SolutionPart2

let replaceNumberWords (line: string): string =
  let numberWords =
    [("one", "1");
    ("two", "2");
    ("three", "3");
    ("four", "4");
    ("five", "5");
    ("six", "6");
    ("seven", "7");
    ("eight", "8");
    ("nine", "9")]

  let rec replaceNumberWords' (numberWords: (string * string) list) (line: string): string =
    let mutable minPos = -1
    let mutable minOldValue = ""
    let mutable replacement = ""
    for (oldValue, newValue) in numberWords do
      let pos = line.IndexOf(oldValue)
      if pos >= 0 && (minPos = -1 || pos < minPos) then
        minPos <- pos
        minOldValue <- oldValue
        replacement <- newValue

    if minPos >= 0 then
      let before = line.Substring(0, minPos)
      let after = line.Substring(minPos + minOldValue.Length - 1)
      let newLine = before + replacement + after
      replaceNumberWords' numberWords newLine
    else
      line

  replaceNumberWords' numberWords line

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
  input.Split '\n'
  |> Array.map replaceNumberWords
  |> Array.map findNumberOfLine
  |> Array.sum