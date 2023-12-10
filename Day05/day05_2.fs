module Day05_2

open System

type dataRange = { lo: int64; hi: int64 }

let printList input =
  List.map (fun item -> printfn $"{item}") |> ignore
  input

let getSeeds (input: string array): int64 list =
  let numberString =
    input[0].Split(":", StringSplitOptions.TrimEntries)[1]
  numberString.Split(" ", StringSplitOptions.TrimEntries)
  |> Array.map int64
  |> Array.toList

let getMap (input: string array) =
  printfn $"{input}"
  input
  |> Array.toList
  |> List.tail
  |> List.map (fun rawMap ->
    rawMap.Split "\n"
    |> Array.toList
    |> List.tail
    |> List.map (fun line ->
      line.Split(" ", StringSplitOptions.TrimEntries)
      |> Array.map int64
      |> Array.toList))

let processPart2 inp =
  let seeds = getSeeds inp
  let maps = getMap inp
  let seedRanges =
    seeds
    |> List.chunkBySize 2
    |> List.map (fun sq ->
      match sq with
      | [seedNum ; len] -> { lo = seedNum; hi = seedNum + len - 1L }
      | _ -> failwith "invalid schema")

  let processTable g =
    g
    |> List.map (fun line ->
      match line with
      | [dst; src; len ] ->
        ( { lo = src; hi = src + len - 1L },
          { lo = dst; hi = dst + len - 1L } )
      | _ -> failwith "invalid schema")

  (seedRanges, List.map processTable maps)

let inRange num lowerBound upperBound =
  num >= lowerBound && num <= upperBound

let rec applyMap map num =
  match map with
  | [] -> num
  | (src, dst) :: map' ->
    if inRange num src.lo src.hi then dst.lo + (num - src.lo)
    else applyMap map' num

let applyMapToRange map range =
  (* Transform the range range according to the mapping map *)
  { lo = applyMap map range.lo; hi = applyMap map range.hi }

let fragmentRange range1 range2 =
  (* Break up range1 if it partially intersects with range2
     range2 is the range of inputs that can be transformed by a mapping *)
  if inRange range1.lo range2.lo range2.hi && range1.hi > range2.hi then
    [ { lo = range1.lo; hi = range2.hi }; { lo = range2.hi + 1L; hi = range1.hi } ]
    (*
            x--------------x
          y----------y
          x----------xy----y
     *)
  else if inRange range1.hi range2.lo range2.hi && range1.lo < range2.lo then
    [ { lo = range1.lo; hi = range2.lo - 1L }; { lo = range2.lo; hi = range1.hi } ]
    (*
            x------x
               y----------y
            x-xy----------y
     *)
  else [ range1 ]
  (*
       x---------------x
         y--------y
       x---------------x
   *)
   (* Missing case
           x-----x
        y-----------y
        y-----------y
    *)

let rangesTroughMap ranges map =
  (* Take a list of ranges and put them through a one-layer mapping *)
  List.fold
    (fun rngs (src, _) ->
      rngs |> List.collect (fun rng -> fragmentRange rng src))
    ranges map
  |> List.map (applyMapToRange map)

[<EntryPoint>]
let main argv =
  let filePath = "../../../data/input.txt"
  let rawInput = System.IO.File.ReadAllText filePath
  let input = rawInput.Split("\n\r", StringSplitOptions.TrimEntries)
  let seedRanges, maps = processPart2 input
  printfn $"{maps}"
  List.fold rangesTroughMap seedRanges maps
  |> List.map (fun rng -> rng.lo)
  |> List.fold min Int64.MaxValue
  |> (fun value -> printfn $"{value}")
  0
