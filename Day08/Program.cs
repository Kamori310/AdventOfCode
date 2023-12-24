using System.Text.RegularExpressions;

namespace Day08;

public static class Day08 {
    public static void Main() {
        const string filepath1 = "../../../data/input.txt";
        var input1 = File.ReadAllText(filepath1);
        var solver1 = new Part1();
        solver1.Calculation(input1);

        const string filepath2 = "../../../data/input.txt";
        var input2 = File.ReadAllText(filepath2);
        var solver2 = new Part2();
        solver2.Calculation(input2);
    }
}

internal class Part1 {
    public void Calculation(string input) {
        var (instructions, map) = ProcessInput(input);
        var counter = 0;
        var key = "AAA";
        while (true) {
            if (key == "ZZZ") {
                Console.WriteLine($"Solution Part1: {counter} Steps were made.");
                return;
            } else {
                key = map[key][instructions[counter % instructions.Count]];
                counter++;
            }
        }
    }

    private (List<int>, Dictionary<string, List<string>>) ProcessInput(string input) {
        var splitInput = input.Split("\r\n\r\n", StringSplitOptions.TrimEntries);
        List<int> instructions = [];
        instructions.AddRange(splitInput[0].Select(letter => letter == 'L' ? 0 : 1));

        const string pattern = @"\w{3}";
        var regex = new Regex(pattern);
        var map = new Dictionary<string, List<string>>();
        foreach (var line in splitInput[1].Split("\n", StringSplitOptions.TrimEntries)) {
            var matches = regex.Matches(line);
            map.Add(matches[0].Value, [matches[1].Value, matches[2].Value]);
        }

        return (instructions, map);
    }
}

internal class Part2 {
    public void Calculation(string input) {
        var (instructions, map) = ProcessInput(input);
        var keys = map.Keys.Where(item => item.EndsWith("A")).ToList();
        var steps = keys.Select(key => GetSteps(instructions, map, key));
        var step = steps.Aggregate(Lcm);
        Console.WriteLine($"Solution Part2: {step}");
    }

    private (List<int>, Dictionary<string, List<string>>) ProcessInput(string input) {
        var splitInput = input.Split("\r\n\r\n", StringSplitOptions.TrimEntries);
        List<int> instructions = [];
        instructions.AddRange(splitInput[0].Select(letter => letter == 'L' ? 0 : 1));

        const string pattern = @"\w{3}";
        var regex = new Regex(pattern);
        var map = new Dictionary<string, List<string>>();
        foreach (var line in splitInput[1].Split("\n", StringSplitOptions.TrimEntries)) {
            var matches = regex.Matches(line);
            map.Add(matches[0].Value, [matches[1].Value, matches[2].Value]);
        }

        return (instructions, map);
    }

    private long GetSteps(List<int> instructions, Dictionary<string, List<string>> map, string startKey) {
        var steps = 0;
        var key = startKey;
        while (!key.EndsWith('Z')) {
            key = map[key][instructions[steps % instructions.Count]];
            steps++;
        }

        return steps;
    }

    private static long Gcd(long a, long b)
    {
        while (b != 0)
        {
            long temp = b;
            b = a % b;
            a = temp;
        }
        return a;
    }

    private static long Lcm(long a, long b)
    {
        return (a / Gcd(a, b)) * b;
    }

}