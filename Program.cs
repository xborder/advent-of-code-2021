using System;
using System.Linq;
using System.IO;
using System.Collections.Generic;
using System.Threading;

namespace advent_of_code {
    class Program {
        static void Main(string[] args) {
            //day1_1();
            //day1_2();
            //day2_1();
            //day2_2();
            //day3_1();
            //day3_2();
            //day4_1();
            //day4_2();
            //day5();
            //day6();
            //day7_1();
            //day7_2();
            //day8_1();
            //day8_2();
            //day9_1();
            //day9_2();
            //day10();
            day11();
        }

        static void day11() {
            var input = ReadInputString("i11");

            int[][] matrix = new int[input.Length][];

            for (var i = 0; i < input.Length; i++) {
                var split = input[i].ToArray();
                matrix[i] = new int[split.Length];
                for (var j = 0; j < split.Length; j++) {
                    matrix[i][j] = split[j] - '0';
                }
            }

            void printMatrix((int x, int y)? pos) {
                for (var i = 0; i < matrix.Length; i++) {
                    for (var j = 0; j < matrix[i].Length; j++) {
                        if (matrix[i][j] == 0 || pos?.x == i && pos?.y == j) {
                            Console.ForegroundColor = ConsoleColor.Black;
                            Console.BackgroundColor = ConsoleColor.White;
                        }
                        Console.Write(matrix[i][j]);
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.BackgroundColor = ConsoleColor.Black;
                    }
                    Console.WriteLine();
                }
            }

            int step = 1;
            int flashes = 0;
            while (true) {
                Queue<(int x, int y)> queue = new Queue<(int x, int y)>();
                var stepFlashes = 0;
                for (var i = 0; i < matrix.Length; i++) {
                    for (var j = 0; j < matrix[i].Length; j++) {
                        if (++matrix[i][j] == 10) { //shine little octo
                            flashes++;
                            stepFlashes++;
                            matrix[i][j] = 0;
                            queue.Enqueue((i, j));
                        }
                    }
                }
                
                while (queue.Count() > 0) {
                    var pos = queue.Dequeue();
                    for (int x = pos.x - 1; x < pos.x + 2 && x < matrix.Length; x++) {
                        for (int y = pos.y - 1; y < pos.y + 2 && y < matrix[0].Length; y++) {
                            if (x < 0 || y < 0 || matrix[x][y] == 0) {
                                continue;
                            }
                            if (++matrix[x][y] == 10) {
                                flashes++;
                                stepFlashes++;
                                matrix[x][y] = 0;
                                queue.Enqueue((x, y));
                            }
                        }
                    }
                }
                Console.WriteLine("-----------------");
                Console.WriteLine($"step {step}");
                printMatrix(null);
                Console.WriteLine();

                if (stepFlashes == matrix.Length * matrix[0].Length) {
                    break;
                }
                step++;
            }
            Console.WriteLine(flashes);
        }

        static void day10() {
            var input = ReadInputString("i10");
            int sum = 0;
            List<(string, long)> incomplete = new List<(string, long)>();
            foreach (var line in input) {
                var array = line.ToCharArray();
                Stack<char> openers = new Stack<char>();
                int i = 0;
                openers.Push(array[i]);
                i++;
                bool corrupted = false;
                do {
                    var c = array[i];
                    if (pairs.ContainsKey(c)) {
                        openers.Push(c);
                    } else {
                        var pop = openers.Pop();
                        if (pairs[pop] == c) {
                            i++;
                            continue;
                        } else {
                            Console.Write(string.Join(' ', array.Take(i)) + " ");
                            Console.ForegroundColor = ConsoleColor.Black;
                            Console.BackgroundColor = ConsoleColor.White;
                            Console.Write(c);
                            Console.ForegroundColor = ConsoleColor.White;
                            Console.BackgroundColor = ConsoleColor.Black;
                            Console.Write(" " + string.Join(' ', array.Skip(i + 1)));
                            Console.WriteLine();
                            switch (c) {
                                case ']': sum += 57; break;
                                case ')': sum += 3; break;
                                case '>': sum += 25137; break;
                                case '}': sum += 1197; break;
                            }
                            corrupted = true; break;
                        }
                    }
                    i++;
                } while (i < array.Length);

                if (openers.Count() > 0 && !corrupted) {
                    long autoScore = 0;
                    string remaining = "";
                    while (openers.Count() > 0) {
                        var c = openers.Pop();
                        switch (c) {
                            case '(': remaining += ')'; autoScore = autoScore * 5 + 1; break;
                            case '[': remaining += ']'; autoScore = autoScore * 5 + 2; break;
                            case '{': remaining += '}'; autoScore = autoScore * 5 + 3; break;
                            case '<': remaining += '>'; autoScore = autoScore * 5 + 4; break;
                        }
                    }
                    incomplete.Add((remaining, autoScore));
                }
            }
            var ordered = incomplete.OrderByDescending(x => x.Item2);
            foreach (var i in ordered) {
                Console.WriteLine($"{i.Item1} - {i.Item2}");
            }
            var middle = ordered.Count() / 2;
            Console.WriteLine($"middle: {middle} {ordered.ElementAt(middle)}");
            Console.WriteLine(sum);
        }

        static Dictionary<char, char> pairs = new Dictionary<char, char>() {
                { '[', ']' },
                { '(', ')' },
                { '<', '>' },
                { '{', '}' }
            };


        static void day9_2() {
            var input = ReadInputString("i9");

            int[][] matrix = new int[input.Length][];

            for (var i = 0; i < input.Length; i++) {
                var split = input[i].ToArray();
                matrix[i] = new int[split.Length];
                for (var j = 0; j < split.Length; j++) {
                    matrix[i][j] = split[j] - '0';
                }
            }

            List<(int x, int y)> basins = new List<(int x, int y)>();

            for (var i = 0; i < matrix.Length; i++) {
                for (var j = 0; j < matrix[i].Length; j++) {
                    var refV = matrix[i][j];
                    var isLower = true;

                    if (i < matrix.Length - 1) {
                        isLower = refV < matrix[i + 1][j];
                    }
                    if (i > 0) {
                        isLower &= refV < matrix[i - 1][j];
                    }
                    if (j > 0) {
                        isLower &= refV < matrix[i][j - 1];
                    }
                    if (j < matrix[i].Length - 1) {
                        isLower &= refV < matrix[i][j + 1];
                    }

                    if (isLower) {
                        basins.Add((i, j));
                    }
                }
            }
            List<int> counts = new List<int>();
            foreach (var basin in basins) {
                HashSet<(int x, int y)> visited = new HashSet<(int x, int y)>();
                Queue<(int x, int y)> queue = new Queue<(int x, int y)>();
                queue.Enqueue(basin);
                var count = 0;
                while (queue.Count() > 0) {
                    var val = queue.Dequeue();
                    Console.WriteLine("-------------------");
                    for (var i = 0; i < matrix.Length; i++) {
                        for (var j = 0; j < matrix[i].Length; j++) {
                            if (visited.Contains((i, j))) {
                                Console.Write(".");
                            } else {
                                Console.Write(matrix[i][j]);
                            }
                        }
                        Console.WriteLine();
                    }

                    if (val.x - 1 >= 0 && !visited.Contains((val.x - 1, val.y)) && matrix[val.x - 1][val.y] < 9) {
                        count++;
                        queue.Enqueue((val.x - 1, val.y));
                        visited.Add((val.x - 1, val.y));
                    }
                    if (val.x + 1 < matrix.Length && !visited.Contains((val.x + 1, val.y)) && matrix[val.x + 1][val.y] < 9) {
                        count++;
                        queue.Enqueue((val.x + 1, val.y));
                        visited.Add((val.x + 1, val.y));
                    }

                    if (val.y - 1 >= 0 && !visited.Contains((val.x, val.y - 1)) && matrix[val.x][val.y - 1] < 9) {
                        count++;
                        queue.Enqueue((val.x, val.y - 1));
                        visited.Add((val.x, val.y - 1));
                    }
                    if (val.y + 1 < matrix[val.x].Length && !visited.Contains((val.x, val.y + 1)) && matrix[val.x][val.y + 1] < 9) {
                        count++;
                        queue.Enqueue((val.x, val.y + 1));
                        visited.Add((val.x, val.y + 1));
                    }
                }
                counts.Add(count);
            }
            var mul = 1;
            foreach (var n in counts.OrderByDescending(c => c).Take(3)) {
                mul *= n;
            }
            Console.WriteLine(mul);
        }

        static void day9_1() {
            var input = ReadInputString("i9");

            int[][] matrix = new int[input.Length][];

            for (var i = 0; i < input.Length; i++) {
                var split = input[i].ToArray();
                matrix[i] = new int[split.Length];
                for (var j = 0; j < split.Length; j++) {
                    matrix[i][j] = split[j] - '0';
                }
            }

            var sum = 0;
            for (var i = 0; i < matrix.Length; i++) {
                for (var j = 0; j < matrix[i].Length; j++) {
                    var refV = matrix[i][j];
                    var isLower = true;

                    if (i < matrix.Length - 1) {
                        isLower = refV < matrix[i + 1][j];
                    }
                    if (i > 0) {
                        isLower &= refV < matrix[i - 1][j];
                    }
                    if (j > 0) {
                        isLower &= refV < matrix[i][j - 1];
                    }
                    if (j < matrix[i].Length - 1) {
                        isLower &= refV < matrix[i][j + 1];
                    }

                    if (isLower) {
                        Console.WriteLine($"{i},{j} -> {refV}");
                        sum += 1 + refV;
                    }
                }
            }
            Console.WriteLine(sum);
        }

        static class Solver {

            class Number {
                public int number = -1;
                public bool uses_top, uses_middle, uses_bottom, uses_top_left, uses_top_right, uses_bottom_left, uses_bottom_right;
                char top, middle, bottom, top_left, top_right, bottom_left, bottom_right;
                public char[] unknown_segments;

                public Number(char[] unknown_segments) {
                    this.unknown_segments = unknown_segments;
                }
            }

            public static int solve(string line) {
                //    var split = line.Split("|");
                //    var input = split[0].Split(" ").Where(s => s.Length != 0);
                //    var output = split[1].Split(" ").Where(s => s.Length != 0);
                //    
                //    var all = new List<string>();
                //    all.AddRange(input);
                //    all.AddRange(output);
                //
                //    var known_numbers = new List<Number>();
                //    var unknown_numbers = new List<Number>();
                //
                //    /*
                //             0
                //         _________
                //        |         |
                //     5  |         |  1
                //        |_________|
                //        |    6    |
                //     4  |         |  2
                //        |_________|
                //             3
                //     */
                //    var dial = new char[71][];
                //    void fillSegment(int pos, char[] segments) {
                //        if(dial[pos] == null) {
                //            dial[pos] = segments;
                //        }
                //        dial[pos].Union(segments);
                //    }
                //    foreach (var number in all) {
                //        var segments = number.ToArray();
                //        if (number.Length == 2) { // 1
                //            fillSegment(1, segments);
                //            fillSegment(2, segments);
                //        } else if (number.Length == 3) { // 7
                //            fillSegment(0, segments);
                //            fillSegment(1, segments);
                //            fillSegment(2, segments);
                //        } else if (number.Length == 4) { // 4
                //            fillSegment(1, segments);
                //            fillSegment(2, segments);
                //            fillSegment(6, segments);
                //            fillSegment(5, segments);
                //        } else if (number.Length == 7) { // 8
                //            fillSegment(0, segments);
                //            fillSegment(1, segments);
                //            fillSegment(2, segments);
                //            fillSegment(3, segments);
                //            fillSegment(4, segments);
                //            fillSegment(5, segments);
                //            fillSegment(6, segments);
                //        } else {
                //
                //        }
                //
                //
                //
                //        //foreach (var number in input) {
                //        //var segments = number.ToArray();
                //        //if (number.Length == 2) { // 1
                //        //    known_numbers.Add(new Number(segments) {
                //        //        uses_top_right = true,
                //        //        uses_bottom_right = true,
                //        //        number = 1
                //        //    });;
                //        //} else if (number.Length == 3) { // 7
                //        //    known_numbers.Add(new Number(segments) {
                //        //        uses_top = true,
                //        //        uses_top_right = true,
                //        //        uses_bottom_right = true,
                //        //        number = 7
                //        //    });
                //        //} else if (number.Length == 4) { // 4
                //        //    known_numbers.Add(new Number(segments) {
                //        //        uses_top_left = true,
                //        //        uses_top_right = true,
                //        //        uses_bottom_right = true,
                //        //        uses_middle = true,
                //        //        number = 4
                //        //    });
                //        //} else if (number.Length == 7) { // 8
                //        //    known_numbers.Add(new Number(segments) {
                //        //        uses_top_left = true,
                //        //        uses_top_right = true,
                //        //        uses_bottom_right = true,
                //        //        uses_bottom_left = true,
                //        //        uses_middle = true,
                //        //        uses_bottom = true,
                //        //        uses_top = true,
                //        //        number = 8
                //        //    });
                //        //} else {
                //        //
                //        //}
                //    }
                //    var i = 0;
                //    foreach (var d in dial) {
                //        Console.WriteLine(i++ + " " + string.Join("\t", d));
                //    }
                //    //var n1 = known_numbers.OrderBy(n => n.unknown_segments).Where(n => n.number == 1).First();
                //    //foreach(var x in n1.unknown_segments) {
                //    //    known_numbers.Where(n => !n.unknown_segments.Contains(x));
                //    //}

                return 0;
            }
        }
        static void day8_2() {
            var x = ReadInputString("i8");
            Solver.solve(x.First());

        }
        static void day8_1() {

            var x = ReadInputString("i8").Select(s => s.Split("|")[1].Split(" ")).SelectMany(c => c).Count(s => s.Length == 7 || (s.Length >= 2 && s.Length <= 4));
            Console.WriteLine(x);

        }
        static void day7_2() {
            var crabs = ReadInputString("i7").First().Split(',').Select(c => Int32.Parse(c)).OrderBy(c => c);
            var pos = 0;

            var max = Int32.MaxValue;
            var finalpos = pos;
            while (pos < crabs.Count()) {
                var artiSum = new int[crabs.Count()];
                var j = 0;
                foreach (var c in crabs) {
                    var diff = Math.Abs(c - pos);
                    var sum = ((1 + diff) * diff) / 2;
                    artiSum[j++] = sum;
                }
                if (artiSum.Sum() < max) {
                    max = artiSum.Sum();
                    finalpos = pos;
                }
                pos++;
            }
            Console.WriteLine(finalpos);

            Console.WriteLine(max);
        }

        static void day7_1() {
            var crabs = ReadInputString("i7").First().Split(',').Select(c => Int32.Parse(c)).OrderBy(c => c);
            var medianIndex = 0;
            decimal mid = (crabs.Count() - 1) / 2;
            if (crabs.Count() % 2 == 0) {
                medianIndex = (int)Math.Floor(mid);
            } else {
                medianIndex = (int)mid;
            }

            var sum = 0;
            foreach (var c in crabs) {
                sum += Math.Abs(c - crabs.ElementAt(medianIndex));
            }
            Console.Write(sum);
        }

        static void day6() {
            var init = ReadInputString("i6").First().Split(',').Select(c => Int32.Parse(c) - 1);
            List<int> lantern = new List<int>(init);

            var totaldays = 256;
            Dictionary<int, long> amount = new Dictionary<int, long>();

            var day = 1;

            long amountFish = init.Count();
            foreach (var timer in init) {
                var nextSpawn = day + timer + 1;

                while (nextSpawn <= totaldays) {
                    amountFish++;
                    if (!amount.ContainsKey(nextSpawn)) {
                        amount.Add(nextSpawn, 1);
                    } else {
                        amount[nextSpawn]++;
                    }
                    nextSpawn += 7;
                }
            }
            day++;
            Console.WriteLine($"Initial state: {string.Join(",", lantern)}");
            while (day <= totaldays) {
                if (!amount.ContainsKey(day)) {
                    day++;
                    continue;
                }
                var n = amount[day];
                var nextSpawn = day + 8 + 1;

                while (nextSpawn <= totaldays) {
                    amountFish += n;
                    if (!amount.ContainsKey(nextSpawn)) {
                        amount.Add(nextSpawn, n);
                    } else {
                        amount[nextSpawn] += n;
                    }
                    nextSpawn += 7;
                }

                day++;
            }
            Console.WriteLine($"Sum: {amountFish}");

        }
        static int[,] buildBoard(string[] input) {

            int[,] board = new int[1000, 1000];
            foreach (var line in input) {
                var split = line.Split(" -> ").Select(l => l.Split(",")).SelectMany(i => i).Select(i => Int32.Parse(i)).ToArray();
                var x0 = Math.Min(split[0], split[2]);
                var x1 = Math.Max(split[0], split[2]);
                var y0 = Math.Min(split[1], split[3]);
                var y1 = Math.Max(split[1], split[3]);

                if (Math.Min(x0, x1) == Math.Max(x0, x1)) {
                    for (int j = y0; j <= y1; j++) {
                        board[j, x0]++;
                    }
                } else if (Math.Min(y0, y1) == Math.Max(y0, y1)) {
                    for (int j = x0; j <= x1; j++) {
                        board[y0, j]++;
                    }
                } else {
                    x0 = split[0];
                    y0 = split[1];
                    x1 = split[2];
                    y1 = split[3];
                    var m = (y1 - y0) / (x1 - x0);
                    var xdir = x1 - x0;
                    var ydir = y1 - y0;

                    var y = y0;
                    for (int x = x0; x != (x1 + ((xdir > 0) ? +1 : -1));) {
                        board[y, x]++;
                        x = (xdir > 0) ? x + 1 : x - 1;
                        y = (ydir > 0) ? y + 1 : y - 1;
                    }

                }

                //Console.WriteLine("-------------");
                //Console.WriteLine(line);
                //Console.WriteLine();
                //var count = 0;
                //for (int i = 0; i < 10; i++) {
                //    for (int j = 0; j < 10; j++) {
                //        if (board[i, j] >= 2) {
                //            count++;
                //        }
                //        Console.Write(board[i, j] == 0 ? "." : "" + board[i, j]);
                //    }
                //    Console.WriteLine();
                //}
                //Console.WriteLine(count);
            }
            return board;
        }

        static void day5() {
            var lines = ReadInputString("i5");

            var board = buildBoard(lines);
            int count = 0;
            for (int i = 0; i < board.GetLength(0); i++) {
                for (int j = 0; j < board.GetLength(1); j++) {
                    if (board[i, j] >= 2) {
                        count++;
                    }
                }
            }
            Console.WriteLine(count);
        }

        class Board {
            public Dictionary<int, int> rows = new Dictionary<int, int>();
            public Dictionary<int, int> cols = new Dictionary<int, int>();
            public Dictionary<int, List<(int x, int y)>> pos = new Dictionary<int, List<(int x, int y)>>();
            public int id;
            public bool won;
            public bool containsVal(int val) {
                return pos.ContainsKey(val);
            }
        }


        static List<Board> BuildBoards(string[] lines) {
            var boards = new List<Board>();
            int id = 0;
            for (int i = 0; i < lines.Length;) {
                if (lines[i] == "") {
                    i++;
                    continue;
                }
                Dictionary<int, int> rows = new Dictionary<int, int>();
                Dictionary<int, int> cols = new Dictionary<int, int>();
                Dictionary<int, List<(int x, int y)>> pos = new Dictionary<int, List<(int x, int y)>>();
                var row = 0;
                do {
                    var col = 0;
                    var line = lines[i].Split(' ').Where(c => c != "").Select(c => Int32.Parse(c));
                    foreach (var v in line) {
                        if (!rows.ContainsKey(row)) {
                            rows.Add(row, v);
                        } else {
                            rows[row] += v;
                        }
                        if (!cols.ContainsKey(col)) {
                            cols.Add(col, v);
                        } else {
                            cols[col] += v;
                        }
                        if (!pos.ContainsKey(v)) {
                            pos.Add(v, new List<(int x, int y)> { (x: row, y: col) });
                        } else {
                            pos[v].Add((x: row, y: col));
                        }
                        col++;
                    }
                    i++;
                    row++;
                } while (i < lines.Length && lines[i] != "");
                boards.Add(new Board {
                    rows = rows,
                    cols = cols,
                    pos = pos,
                    id = id
                });
                id++;
            }

            return boards;
        }

        static void day4_1() {
            var lines = ReadInputString("i4");

            var drafts = lines.ElementAt(0).Split(',').Select(c => Int32.Parse(c));

            var boards = BuildBoards(lines.Skip(2).ToArray());

            foreach (var draft in drafts) {
                var found = boards.Where(b => b.containsVal(draft));
                foreach (var board in found) {
                    foreach (var pos in board.pos[draft]) {
                        board.rows[pos.x] -= draft;
                        board.cols[pos.y] -= draft;
                        if (board.rows[pos.x] == 0 || board.cols[pos.y] == 0) {
                            Console.WriteLine(board.rows.Values.Sum() * draft);
                            return;
                        }
                    }
                }
            }
        }
        static void day4_2() {
            var lines = ReadInputString("i4");

            var drafts = lines.ElementAt(0).Split(',').Select(c => Int32.Parse(c));

            var boards = BuildBoards(lines.Skip(2).ToArray());
            Board lastBoard = null;
            var lastDraft = 0;
            foreach (var draft in drafts) {
                var found = boards.Where(b => b.containsVal(draft) && !b.won);
                foreach (var board in found) {
                    foreach (var pos in board.pos[draft]) {
                        board.rows[pos.x] -= draft;
                        board.cols[pos.y] -= draft;
                        if (board.rows[pos.x] == 0 || board.cols[pos.y] == 0) {
                            board.won = true;
                            lastBoard = board;
                            lastDraft = draft;
                            break;
                        }
                    }
                }


                // if (found.Count() == 0) {
                //     Console.WriteLine(lastDraft);
                //     Console.WriteLine(lastBoard.rows.Values.Sum());
                //     Console.WriteLine(lastBoard.cols.Values.Sum());
                //     Console.WriteLine(lastBoard.rows.Values.Sum() * lastDraft);
                //     return;
                // }
            }
            Console.WriteLine(lastDraft);
            Console.WriteLine(lastBoard.rows.Values.Sum());
            Console.WriteLine(lastBoard.cols.Values.Sum());
            Console.WriteLine(lastBoard.rows.Values.Sum() * lastDraft);
            return;
        }

        static int getRating(IEnumerable<string> input, int col, bool isOxygenGenerator) {
            if (input.Count() == 1) {
                return Convert.ToInt32(input.First(), 2);
            }

            var amounts = getAmounts(input);

            char charToChoose;
            if (amounts[col].zero == amounts[col].one || amounts[col].zero < amounts[col].one) {
                charToChoose = isOxygenGenerator ? '1' : '0';
            } else {
                charToChoose = isOxygenGenerator ? '0' : '1';
            }

            //Console.WriteLine(string.Join(" ", input.Where(s => s.ElementAt(col) == charToChoose)));
            return getRating(input.Where(s => s.ElementAt(col) == charToChoose), col + 1, isOxygenGenerator);
        }

        static (int zero, int one)[] getAmounts(IEnumerable<string> input) {
            (int zero, int one)[] amounts = new (int one, int zero)[input.First().Length];

            for (int i = 0; i < input.Count(); i++) {
                for (int j = 0; j < input.ElementAt(i).Length; j++) {
                    if (input.ElementAt(i).ElementAt(j) == '0') {
                        amounts[j].zero++;
                    } else {
                        amounts[j].one++;
                    }
                }
            }

            return amounts;
        }

        static void day3_2() {
            var input = ReadInputString("i3");

            var oxygen_generator_rating = getRating(input, 0, isOxygenGenerator: true);

            var CO2_scrubber_rating = getRating(input, 0, isOxygenGenerator: false);
            Console.WriteLine(oxygen_generator_rating * CO2_scrubber_rating);
        }

        static void day3_1() {
            var input = ReadInputString("i3");

            var amounts = getAmounts(input);

            string gammaS = "", epsilonS = "";

            foreach (var tup in amounts) {
                bool moreOnes = tup.one > tup.zero;
                gammaS += (moreOnes) ? "1" : "0";
                epsilonS += (moreOnes) ? "0" : "1";
            }

            Console.WriteLine(Convert.ToInt32(gammaS, 2) * Convert.ToInt32(epsilonS, 2));
        }

        static void day2_1() {
            var input = ReadInputString("i2");

            var depth = 0;
            var forward = 0;

            foreach (var line in input) {
                var split = line.Split(" ");
                var direction = split[0];
                var value = Int32.Parse(split[1]);

                switch (direction) {
                    case "up": depth -= value; break;
                    case "down": depth += value; break;
                    case "forward": forward += value; break;
                    default: break;
                }
            }
            Console.WriteLine(depth * forward);

        }
        static void day2_2() {
            var input = ReadInputString("i2");

            var depth = 0;
            var forward = 0;
            var aim = 0;

            foreach (var line in input) {
                var split = line.Split(" ");
                var direction = split[0];
                var value = Int32.Parse(split[1]);

                switch (direction) {
                    case "up": aim -= value; break;
                    case "down": aim += value; break;
                    case "forward": forward += value; depth = (aim == 0) ? depth : depth + aim * value; break;
                    default: break;
                }
            }
            Console.WriteLine(depth * forward);

        }

        static int[] ReadInput(string filename) {
            var input = File.ReadAllLines(@$"input\{filename}.txt").Select(l => Int32.Parse(l));
            return input.ToArray();
        }
        static string[] ReadInputString(string filename) {
            var input = File.ReadAllLines(@$"input\{filename}.txt");
            return input;
        }

        static void day1_1() {
            var input = ReadInput(@"i1");
            var previous = input[0];
            var count = 0;

            foreach (var line in input[1..]) {
                var depth = line;
                count = (depth > previous) ? count + 1 : count;
                previous = depth;
            }

            Console.WriteLine($"day 1 #1 - {count}");
        }

        static void day1_2() {
            var input = ReadInput(@"i1");
            var previous = input[0..3].Sum();
            var count = 0;

            for (int i = 1; i <= input.Length - 3; i++) {
                var interval = input[i..(i + 3)].Sum();
                count = (interval > previous) ? count + 1 : count;
                previous = interval;
            }

            Console.WriteLine($"day 1 #2 - {count}");
        }
    }
}
