namespace Day03;

public static class Day03 {
    public static int Main() {
        const string filepath = "../../../data/input.txt";
        var input = ReadInput(filepath);
        var solution1 = new SolutionPart1(input);
        Console.WriteLine($"Solution 1: {solution1.Calculation()}");
        var solution2 = new SolutionPart2(input);
        Console.WriteLine($"Solution 2: {solution2.Calculation()}");
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
    private readonly int _height = input.Length;
    private readonly int _width = input[0].Length;

    public int Calculation() {
        var solution = 0;
        for (var i = 0; i < _height; i++) {
            var number = "";
            var inNumber = false;
            var start = -1;
            for (var j = 0; j < _width; j++)
                switch (inNumber) {
                    case false when char.IsDigit(input[i][j]):
                        inNumber = true;
                        start = j;
                        number += input[i][j];
                        break;
                    case true when char.IsDigit(input[i][j]):
                        number += input[i][j];
                        break;
                    case true when !char.IsDigit(input[i][j]): {
                        inNumber = false;
                        var end = j - 1;
                        solution += CheckNumber(number, i, start, end);
                        number = "";
                        break;
                    }
                }

            if (inNumber)
                solution += CheckNumber(number, i, start, _width);
        }

        return solution;
    }

    private int CheckNumber(string number, int row, int startColumn, int endColumn) {
        for (var i = int.Max(0, row - 1); i < int.Min(_height, row + 2); i++)
        for (var j = int.Max(0, startColumn - 1); j < int.Min(_width, endColumn + 2); j++)
            if ("#$%&*+-/=@".Contains(input[i][j]))
                return int.Parse(number);

        return 0;
    }
}

internal class SolutionPart2(string[] input) {
    private readonly int _height = input.Length;

    private readonly Dictionary<(int, int), List<int>> _possibleGears = new();
    private readonly int _width = input[0].Length;

    public int Calculation() {
        for (var i = 0; i < _height; i++) {
            var number = "";
            var inNumber = false;
            var start = -1;
            for (var j = 0; j < _width; j++)
                switch (inNumber) {
                    case false when char.IsDigit(input[i][j]):
                        inNumber = true;
                        start = j;
                        number += input[i][j];
                        break;
                    case true when char.IsDigit(input[i][j]):
                        number += input[i][j];
                        break;
                    case true when !char.IsDigit(input[i][j]): {
                        inNumber = false;
                        CheckNumber(number, i, start, j - 1);
                        number = "";
                        break;
                    }
                }

            if (inNumber) CheckNumber(number, i, start, _width);
        }

        return _possibleGears
            .Where(kvp => kvp.Value.Count == 2)
            .Select(kvp => kvp.Value.Aggregate(1, (x, y) => x * y)).Sum();
    }

    private void CheckNumber(string number, int row, int startColumn, int endColumn) {
        for (var i = int.Max(0, row - 1); i < int.Min(_height, row + 2); i++)
        for (var j = int.Max(0, startColumn - 1); j < int.Min(_width, endColumn + 2); j++)
            if ("*".Contains(input[i][j])) {
                if (_possibleGears.ContainsKey((i, j)))
                    _possibleGears[(i, j)].Add(int.Parse(number));
                else
                    _possibleGears.Add((i, j), [int.Parse(number)]);
            }
    }
}