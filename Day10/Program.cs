namespace Day10;

public static class Day10 {
    public static void Main() {
        const string filepath = "../../../data/input.txt";
        var input = File.ReadAllText(filepath);
        var solver1 = new Part1();
        solver1.Calculation(input);
        var solver2 = new Part2();
        solver2.Calculation(input);
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
                Console.WriteLine($"Solution Part1: {path.SelectMany(x => x).ToList().Max()}");
                break;
            }
            Console.WriteLine("No solution found, check logic.");
        }
    }

    private List<string> PreProcessInput(string input) {
        return input.Split("\n", StringSplitOptions.TrimEntries).ToList();
    }

    private (bool, List<List<int>>) FindPath(List<string> lines, Point start) {
        var path = new List<List<int>>();
        for (var y = 0; y < lines.Count; y++) {
            var tempRow = new List<int>();
            for (var x = 0; x < lines[0].Length; x++) {
                tempRow.Add(-1);
            }

            path.Add(tempRow);
        }

        path[start.Row][start.Col] = 0;
        var startingSymbol = lines[start.Row][start.Col];

        var moves = InitializeMoves(start, startingSymbol);

        while (true) {
            if (moves.All(move => ValidateMove(move, lines))) {
                moves[0] = UpdateMove(moves[0], lines);
                moves[1] = UpdateMove(moves[1], lines);
                path = UpdatePath(path, moves);
                if (moves[0].CurrentPosition == moves[1].CurrentPosition) {
                    return (true, path);
                }
            } else {
                return (false, path);
            }
        }

    }

    private List<List<int>> UpdatePath(List<List<int>> path, List<Move> moves) {
        foreach (var move in moves) {
            path[move.CurrentPosition.Row][move.CurrentPosition.Col] = move.MoveNumber;
        }

        return path;
    }

    private Move UpdateMove(Move move, List<string> lines) {
        var nextPosition = CalculateNextPosition(move);
        var nextSymbol = lines[nextPosition.Row][nextPosition.Col];
        var nextDirection = nextSymbol switch {
            '|' => move.Direction,
            '-' => move.Direction,
            '7' => move.Direction == Direction.Right ? Direction.Bottom : Direction.Left,
            'F' => move.Direction == Direction.Left ? Direction.Bottom : Direction.Right,
            'J' => move.Direction == Direction.Right ? Direction.Top : Direction.Left,
            'L' => move.Direction == Direction.Left ? Direction.Top : Direction.Right,
            _ => throw new ArgumentOutOfRangeException()
        };
        return new Move(move.MoveNumber + 1, nextPosition, nextDirection);
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

        // Check if the next position is out of bounds
        if (nextPosition.Row < 0 ||
            nextPosition.Row >= lines.Count ||
            nextPosition.Col < 0 ||
            nextPosition.Col >= lines[0].Length) {
            return false;
        }

        var nextSymbol = lines[nextPosition.Row][nextPosition.Col];

        // Check if the tiles are compatible considering the move direction
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
            default:
                throw new ArgumentOutOfRangeException();
        }

        return false;
    }

    private static Point CalculateNextPosition(Move move) {
        var nextPosition = move.Direction switch {
            Direction.Left => move.CurrentPosition with { Col = move.CurrentPosition.Col - 1 },
            Direction.Right => move.CurrentPosition with { Col = move.CurrentPosition.Col + 1 },
            Direction.Top => move.CurrentPosition with { Row = move.CurrentPosition.Row - 1 },
            Direction.Bottom => move.CurrentPosition with { Row = move.CurrentPosition.Row + 1 },
            _ => throw new ArgumentOutOfRangeException()
        };
        return nextPosition;
    }
}

internal class Part2 {
    public void Calculation(string input) {
        var lines = PreProcessInput(input);
        var start = new Point(-1, -1);

        var path = GetPath(lines, start);
        var numberOfFields = FindEnclosedFields(path);

        Console.WriteLine($"The number of enclosed fields is: {numberOfFields}");
    }

    private List<List<char>> GetPath(List<string> lines, Point start) {
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
                return path;
            } else {
                Console.WriteLine("No solution found, check logic.");
            }
        }

        return new List<List<char>>();
    }

    private int FindEnclosedFields(List<List<char>> path) {
        var expandedPath = ExpandPath(path);
        var filledArea = FillArea(expandedPath);
        return CountEnclosedFields(filledArea);
    }

    private int CountEnclosedFields(char[,] filledArea) {
        var rows = filledArea.GetLength(0);
        var cols = filledArea.GetLength(1);
        var numberOfUnfilledElements = 0;

        for (var rowStart = 0; rowStart <= rows - 3; rowStart += 3) {
            for (var colStart = 0; colStart <= cols - 3; colStart += 3) {
                var allDots = true;
                for (var i = 0; i < 3 && allDots; i++) {
                    for (var j = 0; j < 3 && allDots; j++) {
                        if (filledArea[rowStart + i, colStart + j] != '.') {
                            allDots = false;
                        }
                    }
                }

                if (allDots) {
                    numberOfUnfilledElements++;
                }
            }
        }

        return numberOfUnfilledElements;
    }

    private char[,] FillArea(char[,] expandedPath) {
        expandedPath[0, 0] = 'o';
        List<Point> open = [new Point(0, 0)];

        List<Point> neighbours = [
            new Point(-1, 0), // Up
            new Point(0, 1), // Right
            new Point(1, 0), // Down
            new Point(0, -1) // Left
        ];

        while (open.Count != 0) {
            var currentPoint = open[0];
            open.RemoveAt(0);

            foreach (var neighbour in neighbours) {
                var nextPoint = new Point(
                    currentPoint.Row + neighbour.Row,
                    currentPoint.Col + neighbour.Col
                    );

                if (
                    nextPoint is { Row: >= 0, Col: >= 0 } &&
                    nextPoint.Row <= expandedPath.GetUpperBound(0) &&
                    nextPoint.Col <= expandedPath.GetUpperBound(1)
                    ) {
                    if (expandedPath[nextPoint.Row, nextPoint.Col] == '.') {
                        expandedPath[nextPoint.Row, nextPoint.Col] = 'o';
                        open.Add(nextPoint);
                    } else {
                        // If nextPoint == 'x' do nothing
                        // If nextPoint == 'o' do nothing
                    }
                }
            }
        }

        return expandedPath;
    }

    private char[,] ExpandPath(List<List<char>> path) {
        var expandedPath = new char[3 * path.Count, 3 * path[0].Count];
        for (var i = 0; i < path.Count; i++) {
            for (var j = 0; j < path[0].Count; j++) {
                var currentSymbol = path[i][j];
                var symbols = currentSymbol switch {
                    '-' => [
                        '.', '.', '.',
                        'x', 'x', 'x',
                        '.', '.', '.'
                    ],
                    '|' => [
                        '.', 'x', '.',
                        '.', 'x', '.',
                        '.', 'x', '.'
                    ],
                    '7' => [
                        '.', '.', '.',
                        'x', 'x', '.',
                        '.', 'x', '.'
                    ],
                    'F' => [
                        '.', '.', '.',
                        '.', 'x', 'x',
                        '.', 'x', '.'
                    ],
                    'L' => [
                        '.', 'x', '.',
                        '.', 'x', 'x',
                        '.', '.', '.'
                    ],
                    'J' => new List<char> {
                        '.', 'x', '.',
                        'x', 'x', '.',
                        '.', '.', '.'
                    },
                    '.' => new List<char> {
                        '.', '.', '.',
                        '.', '.', '.',
                        '.', '.', '.'
                    },
                    _ => throw new ArgumentException($"Wrong symbol: '{currentSymbol}'")
                };

                // Col-1 Row-1, Col-0 Row-1, Col+1 Row-1
                // Col-1 Row-0, Col-0 Row-0, Col+1 Row-0
                // Col-1 Row+1, Col-0 Row+1, Col+1 Row+1
                var centerPoint = new Point(Row: (i * 3) + 1, Col: (j * 3) + 1);
                var rowMod = new List<int> {
                    -1, -1, -1,
                     0,  0,  0,
                     1,  1,  1
                };
                var colMod = new List<int> {
                    -1, 0, 1,
                    -1, 0, 1,
                    -1, 0, 1
                };

                for (int k = 0; k < 9; k++) {
                    expandedPath[centerPoint.Row + rowMod[k], centerPoint.Col + colMod[k]] = symbols[k];
                }
            }
        }

        return expandedPath;
    }

    private List<string> PreProcessInput(string input) {
        return input.Split("\n", StringSplitOptions.TrimEntries).ToList();
    }

    private (bool, List<List<char>>) FindPath(List<string> lines, Point start) {
        var path = new List<List<char>>();
        for (var y = 0; y < lines.Count; y++) {
            var tempRow = new List<char>();
            for (var x = 0; x < lines[0].Length; x++) {
                tempRow.Add('.');
            }

            path.Add(tempRow);
        }

        var startingSymbol = lines[start.Row][start.Col];
        path[start.Row][start.Col] = startingSymbol;

        var moves = InitializeMoves(start, startingSymbol);

        while (true) {
            if (moves.All(move => ValidateMove(move, lines))) {
                moves[0] = UpdateMove(moves[0], lines);
                moves[1] = UpdateMove(moves[1], lines);
                path = UpdatePath(path, moves, lines);
                if (moves[0].CurrentPosition == moves[1].CurrentPosition) {
                    return (true, path);
                }
            } else {
                return (false, path);
            }
        }

    }

    private List<List<char>> UpdatePath(List<List<char>> path, List<Move> moves, List<string> lines) {
        foreach (var move in moves) {
            path[move.CurrentPosition.Row][move.CurrentPosition.Col] =
                lines[move.CurrentPosition.Row][move.CurrentPosition.Col];
        }

        return path;
    }

    private Move UpdateMove(Move move, List<string> lines) {
        var nextPosition = CalculateNextPosition(move);
        var nextSymbol = lines[nextPosition.Row][nextPosition.Col];
        var nextDirection = nextSymbol switch {
            '|' => move.Direction,
            '-' => move.Direction,
            '7' => move.Direction == Direction.Right ? Direction.Bottom : Direction.Left,
            'F' => move.Direction == Direction.Left ? Direction.Bottom : Direction.Right,
            'J' => move.Direction == Direction.Right ? Direction.Top : Direction.Left,
            'L' => move.Direction == Direction.Left ? Direction.Top : Direction.Right,
            _ => throw new ArgumentOutOfRangeException()
        };
        return new Move(move.MoveNumber + 1, nextPosition, nextDirection);
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

        // Check if the next position is out of bounds
        if (nextPosition.Row < 0 ||
            nextPosition.Row >= lines.Count ||
            nextPosition.Col < 0 ||
            nextPosition.Col >= lines[0].Length) {
            return false;
        }

        var nextSymbol = lines[nextPosition.Row][nextPosition.Col];

        // Check if the tiles are compatible considering the move direction
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
            default:
                throw new ArgumentOutOfRangeException();
        }

        return false;
    }

    private static Point CalculateNextPosition(Move move) {
        var nextPosition = move.Direction switch {
            Direction.Left => move.CurrentPosition with { Col = move.CurrentPosition.Col - 1 },
            Direction.Right => move.CurrentPosition with { Col = move.CurrentPosition.Col + 1 },
            Direction.Top => move.CurrentPosition with { Row = move.CurrentPosition.Row - 1 },
            Direction.Bottom => move.CurrentPosition with { Row = move.CurrentPosition.Row + 1 },
            _ => throw new ArgumentOutOfRangeException()
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