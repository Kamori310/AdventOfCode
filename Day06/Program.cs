namespace Day06;

public static class Day06 {
    public static void Main() {
        const string filepath = "../../../data/input.txt";
        var input = ReadInput(filepath);
        var solution1 = SolutionPart1.Calculation(input);
        Console.WriteLine($"Solution 1: {solution1}");
        var solution2 = SolutionPart2.Calculation(input);
        Console.WriteLine($"Solution 2: {solution2}");
    }

    private static List<List<int>> ReadInput(string filepath) {
        var rawInput = File.ReadAllText(filepath);
        var lines = rawInput.Split("\n");
        List<int> numbers = [];
        numbers.AddRange(
            from line in lines
            from lineNumbers in line.Split(":", StringSplitOptions.TrimEntries)[1..]
            from number in lineNumbers.Split(" ")
            where number != ""
            select int.Parse(number)
            );

        return [
            numbers[..(numbers.Count / 2)],
            numbers[(numbers.Count / 2)..]
        ];
    }
}

internal static class SolutionPart1 {
    public static int Calculation(List<List<int>> calcInput) {
        List<int> raceList = [];
        for (var race = 0; race < calcInput[0].Count; race++) {
            var raceTime = calcInput[0][race];
            var targetDistance = calcInput[1][race];
            var racesWon = 0;

            for (var timeProbe = 0; timeProbe <= raceTime; timeProbe++) {
                var distance = (raceTime - timeProbe) * timeProbe;
                if (distance > targetDistance)
                    racesWon++;
            }

            if (racesWon != 0)
                raceList.Add(racesWon);
        }

        return raceList.Count == 0 ? 0 : raceList.Aggregate(1, (acc, value) => acc * value);
    }
}

internal static class SolutionPart2 {
    public static long Calculation(List<List<int>> input) {
        var numberOfRaces = 0L;
        var (raceTime, raceDistance) = PreProcessInput(input);
        for (var timeProbe = 0; timeProbe <= raceTime; timeProbe++) {
            var distance = (raceTime - timeProbe) * timeProbe;
            if (distance > raceDistance)
                numberOfRaces++;
        }

        return numberOfRaces;
    }

    private static (long, long) PreProcessInput(List<List<int>> input) {
        var timeString = "";
        var distanceString = "";

        for (var i = 0; i < input[0].Count; i++) {
            timeString += input[0][i].ToString();
            distanceString += input[1][i].ToString();
        }

        return (long.Parse(timeString), long.Parse(distanceString));
    }
}