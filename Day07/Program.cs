namespace Day07;

public static class Day07 {
    public static void Main() {
        const string filepath = "../../../data/input.txt";
        var input = File.ReadAllText(filepath);
        var solverPart1 = new Part1();
        solverPart1.Calculation(input);
        var solverPart2 = new Part2();
        solverPart2.Calculation(input);
    }
}

internal class Part1 {
    public void Calculation(string input) {
        var preProcessedInput = PreProcessInput(input);
        preProcessedInput.Sort((a, b) => {
            if (a.Rank != b.Rank) {
                return b.Rank - a.Rank;
            } else {
                return CompareHandValues(a.HandValue, b.HandValue);
            }
        });
        var result = 0;
        for (var index = 0; index < preProcessedInput.Count; index++) {
            var entry = preProcessedInput[index];
            result += entry.Bit * (index + 1);
        }

        Console.WriteLine($"Part1: {result}");
    }

    private List<Hand> PreProcessInput(string input) {
        List<Hand> hands = [];
        var listOfHands = input.Split("\n", StringSplitOptions.TrimEntries);
        foreach (var entry in listOfHands) {
            var originalHandAndBit = entry.Split(" ", StringSplitOptions.TrimEntries);
            var hand = PreProcessHand(originalHandAndBit[0]);
            hands.Add(
                new Hand(int.Parse(originalHandAndBit[1]),
                    hand,
                    HandToRank(hand)));
        }

        return hands;
    }

    private List<int> PreProcessHand(string rawHand) {
        List<int> hand = [];
        foreach (var letter in rawHand) {
            hand.Add(CharToValue(letter));
        }

        return hand;
    }

    private static int CharToValue(char cardValue) {
        return cardValue switch {
            'A' => 14,
            'K' => 13,
            'Q' => 12,
            'J' => 11,
            'T' => 10,
            '9' => 9,
            '8' => 8,
            '7' => 7,
            '6' => 6,
            '5' => 5,
            '4' => 4,
            '3' => 3,
            '2' => 2,
            _ => throw (new ArgumentException("Invalid Argument (should be a card suit value)"))
        };
    }

    private HandRank HandToRank(List<int> hand) {
        var grouped =
            hand
                .GroupBy(x => x)
                .Select(g => new { Value = g.Key, Count = g.Count() }).ToList();
        if (grouped.Any(item => item.Count == 5)) {
            return HandRank.FiveOfAKind;
        } else if (grouped.Any(item => item.Count == 4)) {
            return HandRank.FourOfAKind;
        } else if (grouped.Any(item => item.Count == 3) &&
                   grouped.Any(item => item.Count == 2)) {
            return HandRank.FullHouse;
        } else if (grouped.Any(item => item.Count == 3)) {
            return HandRank.ThreeOfAKind;
        } else if (grouped.Count(x => x.Count == 2) == 2) {
            return HandRank.TwoPairs;
        } else if (grouped.Any(item => item.Count == 2)) {
            return HandRank.OnePair;
        } else {
            return HandRank.HighCard;
        }
    }

    private int CompareHandValues(List<int> a, List<int> b) {
        for (var i = 0; i < a.Count; i++) {
            if (a[i] != b[i])
                return a[i] - b[i];
        }

        return 0;
    }
}

internal class Part2 {
    public void Calculation(string input) {
        var preProcessedInput = PreProcessInput(input);
        preProcessedInput.Sort((a, b) => {
            if (a.Rank != b.Rank) {
                return b.Rank - a.Rank;
            } else {
                return CompareHandValues(a.HandValue, b.HandValue);
            }
        });
        var result = 0;
        for (var index = 0; index < preProcessedInput.Count; index++) {
            var entry = preProcessedInput[index];
            result += entry.Bit * (index + 1);
        }

        Console.WriteLine($"Part2: {result}");
    }

    private List<Hand> PreProcessInput(string input) {
        List<Hand> hands = [];
        var listOfHands = input.Split("\n", StringSplitOptions.TrimEntries);
        foreach (var entry in listOfHands) {
            var originalHandAndBit = entry.Split(" ", StringSplitOptions.TrimEntries);
            var hand = PreProcessHand(originalHandAndBit[0]);
            hands.Add(
                new Hand(int.Parse(originalHandAndBit[1]),
                    hand,
                    HandToRank(hand)));
        }

        return hands;
    }

    private List<int> PreProcessHand(string rawHand) {
        List<int> hand = [];
        foreach (var letter in rawHand) {
            hand.Add(CharToValue(letter));
        }

        return hand;
    }

    private static int CharToValue(char cardValue) {
        return cardValue switch {
            'A' => 14,
            'K' => 13,
            'Q' => 12,
            'T' => 10,
            '9' => 9,
            '8' => 8,
            '7' => 7,
            '6' => 6,
            '5' => 5,
            '4' => 4,
            '3' => 3,
            '2' => 2,
            'J' => 1,
            _ => throw (new ArgumentException("Invalid Argument (should be a card suit value)"))
        };
    }

    private HandRank HandToRank(List<int> hand) {
        var jacks = hand.Count(item => item == 1);
        var grouped =
            hand
                .GroupBy(x => x)
                .Select(g => new { Value = g.Key, Count = g.Count() }).ToList();
        grouped.RemoveAll(item => item.Value == 1);

        HandRank result;
        if (grouped.Any(item => item.Count == 5)) {
            result = HandRank.FiveOfAKind;
        } else if (grouped.Any(item => item.Count == 4)) {
            result = HandRank.FourOfAKind;
        } else if (grouped.Any(item => item.Count == 3) &&
                   grouped.Any(item => item.Count == 2)) {
            result = HandRank.FullHouse;
        } else if (grouped.Any(item => item.Count == 3)) {
            result = HandRank.ThreeOfAKind;
        } else if (grouped.Count(x => x.Count == 2) == 2) {
            result = HandRank.TwoPairs;
        } else if (grouped.Any(item => item.Count == 2)) {
            result = HandRank.OnePair;
        } else {
            result = HandRank.HighCard;
        }

        switch (result) {
            case HandRank.FiveOfAKind:
                break;
            case HandRank.FourOfAKind:
                if (jacks == 1) {
                    result = HandRank.FiveOfAKind;
                }
                break;
            case HandRank.FullHouse:
                break;
            case HandRank.ThreeOfAKind:
                result = jacks switch {
                    2 => HandRank.FiveOfAKind,
                    1 => HandRank.FourOfAKind,
                    _ => result
                };
                break;
            case HandRank.TwoPairs:
                if (jacks == 1) {
                    result = HandRank.FullHouse;
                }
                break;
            case HandRank.OnePair:
                result = jacks switch {
                    3 => HandRank.FiveOfAKind,
                    2 => HandRank.FourOfAKind,
                    1 => HandRank.ThreeOfAKind,
                    _ => result
                };
                break;
            case HandRank.HighCard:
                result = jacks switch {
                    5 => HandRank.FiveOfAKind,
                    4 => HandRank.FiveOfAKind,
                    3 => HandRank.FourOfAKind,
                    2 => HandRank.ThreeOfAKind,
                    1 => HandRank.OnePair,
                    _ => result
                };
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }

        return result;
    }

    private int CompareHandValues(List<int> a, List<int> b) {
        for (var i = 0; i < a.Count; i++) {
            if (a[i] != b[i])
                return a[i] - b[i];
        }

        return 0;
    }
}

internal record Hand(
    int Bit,
    List<int> HandValue,
    HandRank Rank
);

internal enum HandRank {
    FiveOfAKind,
    FourOfAKind,
    FullHouse,
    ThreeOfAKind,
    TwoPairs,
    OnePair,
    HighCard,
};