public static class Day09 {
    public static void Main() {
        const string filepath = "../../../data/input.txt";
        var input = File.ReadAllText(filepath);
        var solver1 = new Part1();
        solver1.Calcalation(input);
        var solver2 = new Part2();
        solver2.Calcalation(input);
    }
}

internal class Part1 {
    public void Calcalation(string input) {
        var numbers = PreProcessInput(input);
        foreach (var item in numbers) {
            var currentList = item[0];
            var helperLists = new List<List<int>>();
            while (currentList.Any(number => number != 0)) {
                var tempList = new List<int>();
                for (var i = 0; i < currentList.Count - 1; i++) {
                    tempList.Add(currentList[i + 1] - currentList[i]);
                }
                helperLists.Add(tempList);
                currentList = tempList;
            }

            foreach (var helperList in helperLists) {
                item.Add(helperList);
            }
        }

        foreach (var item in numbers) {
            for (var i = item.Count - 1; i > 0; i--) {
                item[i - 1].Add(item[i].Last() + item[i - 1].Last());
            }
        }

        var result = 0;
        foreach (var item in numbers) {
            result += item[0].Last();
        }
        Console.WriteLine($"Solution Part1: {result}");
    }

    private List<List<List<int>>> PreProcessInput(string input) {
        return input
            .Split("\n", StringSplitOptions.TrimEntries)
            .Select(line =>
                        (List<List<int>>) [line.Split(" ").Select(int.Parse).ToList()])
            .ToList();
    }
}

internal class Part2 {
    public void Calcalation(string input) {
        var numbers = PreProcessInput(input);
        foreach (var item in numbers) {
            var currentList = item[0];
            var helperLists = new List<List<int>>();
            while (currentList.Any(number => number != 0)) {
                var tempList = new List<int>();
                for (var i = 0; i < currentList.Count - 1; i++) {
                    tempList.Add(currentList[i + 1] - currentList[i]);
                }
                helperLists.Add(tempList);
                currentList = tempList;
            }

            foreach (var helperList in helperLists) {
                item.Add(helperList);
            }
        }

        foreach (var item in numbers) {
            for (var i = item.Count - 1; i > 0; i--) {
                item[i - 1].Insert(0, item[i - 1].First() - item[i].First());
            }
        }

        var result = 0;
        foreach (var item in numbers) {
            result += item[0].First();
        }
        Console.WriteLine($"Solution Part2: {result}");
    }

    private List<List<List<int>>> PreProcessInput(string input) {
        return input
            .Split("\n", StringSplitOptions.TrimEntries)
            .Select(line =>
                        (List<List<int>>) [line.Split(" ").Select(int.Parse).ToList()])
            .ToList();
    }
}
