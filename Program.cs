using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading;

namespace github_commit_message {
    class Program {
        public static string lastD = "null";
        public static string input;
        public static int times;
        static void Main(string[] args) {
            Console.WriteLine("spell out messages on your contributions page");
            Console.WriteLine("type what you want it to say (only accepts letters) : ");
            input = Console.ReadLine();
            input = Regex.Replace(input," ", "/");
            Console.Clear();
            Console.WriteLine(input);
            foreach (var letter in input) {
                string letterstr = letter.ToString();
                if (File.Exists("words.fnt")) {
                    foreach (var words in File.ReadLines("words.fnt")) {
                        string[] num = words.Split(":");
                        if (letterstr == num[0]) {
                            Console.WriteLine(letterstr+":"+num[1]);
                            string[] coordtr = num[1].Split(",");
                            Console.WriteLine("len : "+coordtr.Length);
                            while (times < coordtr.Length) {
                                bool commit = false;
                                while (commit == false) {
                                    lastD = DateTime.Now.ToString("dd");
                                    Console.WriteLine("day: "+lastD);
                                    Console.WriteLine(coordtr[times]);
                                    //make it wait a hour
                                    Thread.Sleep(3600000);
                                    if (lastD != DateTime.Now.ToString("dd")) {
                                        commitGH(coordtr[times]);
                                        lastD = DateTime.Now.ToString("dd");
                                        bool loop = true;
                                        while (loop) {
                                            //wait a day
                                            if (lastD != DateTime.Now.ToString("dd")) {
                                                loop = false;
                                            } else {
                                                Thread.Sleep(3600000); 
                                            }
                                        }
                                        //reset variables so it will work on next char
                                        commit = true;
                                        times += 1;
                                    }
                                }
                            }
                            //reset variables so it works next letter
                            times = 0;
                        }
                    }
                } else {Console.WriteLine("font file not found (words.fnt)");}
            }
        }
        public static void commitGH(string commit_times) {
            if (commit_times == "1") {
                
                Console.WriteLine("Committed once");
            } if (commit_times == "2") {
                
                Console.WriteLine("Committed twice");
            }
        }
    }
}