using System;
using System.Linq;
using System.IO;

namespace advent_of_code {
    class Program {
        static void Main(string[] args) {
            day1_1();
            day1_2();
            day2_1();
            day2_2();
        }
        
        static void day2_1() {
            var input = ReadInputString("input2_1");

            var depth = 0;
            var forward = 0;

            foreach(var line in input) {
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
            var input = ReadInputString("input2_2");

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
            var input = ReadInput(@"input1_1");
            var previous = input[0];
            var count = 0;

            foreach(var line in input[1..]) {
                var depth = line;
                count = (depth > previous) ? count + 1 : count;
                previous = depth;
            }

            Console.WriteLine($"day 1 #1 - {count}");
        }

        static void day1_2() {
            var input = ReadInput(@"input1_2");
            var previous = input[0..3].Sum();
            var count = 0;

            for (int i = 1; i <= input.Length-3; i ++) {
                var interval = input[i..(i + 3)].Sum();
                count = (interval > previous) ? count + 1 : count;
                previous = interval;
            }

            Console.WriteLine($"day 1 #2 - {count}");
        }
    }
}
