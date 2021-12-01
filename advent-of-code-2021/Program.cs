using System;
using System.Linq;

namespace advent_of_code {
    class Program {
        static void Main(string[] args) {
            day1_1();
            day1_2();
        }

        static void day1_1() {
            var input = System.IO.File.ReadAllLines(@"input1_1.txt").Select(l => Int32.Parse(l)).ToArray();
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
            var input = System.IO.File.ReadAllLines(@"input1_2.txt").Select(l => Int32.Parse(l)).ToArray();
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
