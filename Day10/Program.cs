namespace Day10;

public static class Day10 {
    public static void Main() {
        const string filepath = "../../../data/smallInput.txt";
        var input = File.ReadAllText(filepath);
        var solver1 = new Part1();
        solver1.Calculation(input);
    }
}

internal class Part1 {
    public void Calculation(string input) {
        var lines = PreProcessInput(input);
        var start = new Point(-1, -1);
        for (var i = 0; i < lines.Count; i++) {
            if (lines[i].Contains('S')) {
                start = new Point(i, lines[i].IndexOf('S'));
                break;
            }
        }

        List<char> pipes = ['|', '-', '7', 'F', 'L', 'J'];
        foreach (var pipe in pipes) {
            var tempLines = lines.Select(item => item.Replace('S', pipe)).ToList();
            var (success, path) = FindPath(tempLines, start);
            if (success) {
                Console.WriteLine($"Solution Part1: {path.Max()}");
                break;
            }
        }
    }

    private List<string> PreProcessInput(string input) {
        return input.Split("\n", StringSplitOptions.TrimEntries).ToList();
    }

    private (bool, List<List<int>>) FindPath(List<string> lines, Point start) {
        var path = new List<List<int>>();
        for (var y = 0; y < lines.Count; y++) {
            for (var x = 0; x < lines[0].Length; x++) {
                path[y][x] = -1;
            }
        }

        path[start.Row][start.Col] = 0;
        char currentSymbole;
        Direction movingDirection;
        
        var nextPositions = lines[start.Row][start.Col] switch {
            '|' => []
        }
        while (nextPositions.All(position => position != start)) {

        }

    }

    private bool ValidateMove(char previousSymbol, char currentSymbol, Direction direction) {
        switch (direction) {
            case Direction.Left:
                if (previousSymbol is '7' or 'J' or '-' && currentSymbol is 'F' or 'L' or '-') {
                    return true;
                }
                break;
            case Direction.Top:
                if (previousSymbol is '|' or 'J' or 'L' && currentSymbol is 'F' or '|' or '7') {
                    return true;
                }
                break;
            case Direction.Right:
                if (previousSymbol is 'F' or 'L' or '-' && currentSymbol is '7' or 'J' or '-') {
                    return true;
                }
                break;
            case Direction.Bottom:
                if (previousSymbol is 'F' or '|' or '7' && currentSymbol is '|' or 'J' or 'L') {
                    return true;
                }
                break;
        }

        return false;
    }
}

public record Point(int Row, int Col);

public enum Direction {
    Left,
    Top,
    Right,
    Bottom
}