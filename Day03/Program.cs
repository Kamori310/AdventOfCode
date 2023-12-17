// See https://aka.ms/new-console-template for more information

using System.Collections.Frozen;

namespace Day03;

public static class Day03 {
    public static int Main() {
        const string filepath = "../../../data/input.txt";
        var input = ReadInput(filepath);
        var solution = new SolutionPart1(input);
        Console.WriteLine($"Solution 1: {solution.Calculation()}");
        solution = new Solution_Part2();
        Console.WriteLine($"Solution 2: {solution.Calculation()}");
        return 0;
    }

    private static string[] ReadInput(string filepath) {
        var rawInput = File.ReadAllText(filepath);
        // Used in debugger to get all the symbols
        // var stringToSet = new SortedSet<char>();
        // foreach (var character in rawInput)
        // {
        //     if (!stringToSet.Contains(character)) {
        //         stringToSet.Add(character);
        //     }
        // }

        return rawInput.Split("\n", StringSplitOptions.TrimEntries);
    }
}

internal class SolutionPart1(string[] input) {
    private readonly int _width = input[0].Length;
    private readonly int _height = input.Length;

    public int Calculation() {
        var solution = 0;
        for (var i = 0; i < _height; i++) {
            var number = "";
            var inNumber = false;
            var start = -1;
            var end = -1;
            for (var j = 0; j < _width; j++)
                if (!inNumber && char.IsDigit(input[i][j])) {
                    inNumber = true;
                    start = j;
                    number += input[i][j];
                } else if (inNumber && char.IsDigit(input[i][j])) {
                    number += input[i][j];
                } else if (inNumber && !char.IsDigit(input[i][j])) {
                    inNumber = false;
                    end = j - 1;
                    solution += CheckNumber(number, i, start, end);
                    number = "";
                }

            if (inNumber)
                solution += CheckNumber(number, i, start, _width);
        }

        return solution;
    }

    private int CheckNumber(string number, int row, int startColumn, int endColumn)
    {
        for (var i = int.Max(0, row - 1); i < int.Min(_height, row + 2); i++) {
            for (var j = int.Max(0, startColumn - 1); j < int.Min(_width, endColumn + 2); j++) {
                if ("#$%&*+-/=@".Contains(input[i][j])) {
                    return int.Parse(number);
                }
            }
        }

        return 0;
    }
}

class SolutionPart2(string[] input){
    private readonly int _width = input[0].Length;
    private readonly int _height = input.Length;

    public int Calculation() {
        var solution = 0;

        return solution;
    }

}
