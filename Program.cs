using System;
using System.Linq;
using System.IO;

namespace advent_of_code {
    class Program {
        static void Main(string[] args) {
            day1_1();
            day1_2();
        }
        
        static int[] ReadInput(string filename) {
            var input = File.ReadAllLines(@$"input\{filename}.txt").Select(l => Int32.Parse(l));
            return input.ToArray();
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
