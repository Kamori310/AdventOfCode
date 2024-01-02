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
        var currentSymbol = lines[start.Row][start.Col];

        var moves = InitializeMoves(start, currentSymbol);

        while (true) {
            if (moves.All(move => ValidateMove(move, lines))) {
                // Next position
                // Next symbol
                // Update path
                // Next Moves
                // Check if end is reached
            } else {
                return (false, path);
            }
        }

    }

    private static List<Move> InitializeMoves(Point start, char currentSymbol) {
        List<Move> moves =
            currentSymbol switch {
                '|' => [
                    new Move(0, start, Direction.Top),
                    new Move(0, start, Direction.Bottom)
                ],
                '-' => [
                    new Move(0, start, Direction.Left),
                    new Move(0, start, Direction.Right)
                ],
                '7' => [
                    new Move(0, start, Direction.Left),
                    new Move(0, start, Direction.Bottom)
                ],
                'F' => [
                    new Move(0, start, Direction.Right),
                    new Move(0, start, Direction.Bottom)
                ],
                'L' => [
                    new Move(0, start, Direction.Top),
                    new Move(0, start, Direction.Right)
                ],
                'J' => [
                    new Move(0, start, Direction.Top),
                    new Move(0, start, Direction.Left)
                ],
                _ => throw new ArgumentOutOfRangeException()
            };
        return moves;
    }

    private bool ValidateMove(Move move, List<string> lines) {
        var currentSymbol = lines[move.CurrentPosition.Row][move.CurrentPosition.Col];
        var nextPosition = CalculateNextPosition(move);

        if (nextPosition.Row < 0 ||
            nextPosition.Row >= lines.Count ||
            nextPosition.Col < 0 ||
            nextPosition.Col >= lines[0].Length) {
            return false;
        }

        var nextSymbol = lines[nextPosition.Row][nextPosition.Col];

        switch (move.Direction) {
            case Direction.Left:
                if (currentSymbol is '7' or 'J' or '-' && nextSymbol is 'F' or 'L' or '-') {
                    return true;
                }

                break;
            case Direction.Top:
                if (currentSymbol is '|' or 'J' or 'L' && nextSymbol is 'F' or '|' or '7') {
                    return true;
                }

                break;
            case Direction.Right:
                if (currentSymbol is 'F' or 'L' or '-' && nextSymbol is '7' or 'J' or '-') {
                    return true;
                }

                break;
            case Direction.Bottom:
                if (currentSymbol is 'F' or '|' or '7' && nextSymbol is '|' or 'J' or 'L') {
                    return true;
                }

                break;
        }

        return false;
    }

    private static Point CalculateNextPosition(Move move) {
        var nextPosition = move.Direction switch {
            Direction.Left => move.CurrentPosition with { Col = move.CurrentPosition.Col - 1 },
            Direction.Right => move.CurrentPosition with { Col = move.CurrentPosition.Col + 1 },
            Direction.Top => move.CurrentPosition with { Row = move.CurrentPosition.Row + 1 },
            Direction.Bottom => move.CurrentPosition with { Row = move.CurrentPosition.Row - 1 },
        };
        return nextPosition;
    }
}

public record Point(int Row, int Col);

internal record Move(
    int MoveNumber,
    Point CurrentPosition,
    Direction Direction
);

public enum Direction {
    Left,
    Top,
    Right,
    Bottom
}