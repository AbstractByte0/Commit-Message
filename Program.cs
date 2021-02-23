using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading;

namespace github_commit_message {
    class Program {
        public static string cloneDIR;
        public static string repo;
        public static string path;
        public static string username;
        public static string email;
        public static string lastD = "null";
        public static string input;
        public static int times;
        static void Main(string[] args) {
            if (!File.Exists("PATH")) {
                Console.WriteLine("--One time setup--");
                Console.WriteLine("");
                Console.WriteLine("type the path to git, windows example : C:\\Program Files\\Git\\bin\\git.exe");
                Console.WriteLine("you will need to download git from https://git-scm.com/downloads");
                Console.WriteLine(": ");
                path = Console.ReadLine();
                
                Console.WriteLine("type the path to whare it should clone the repo to (put a \\ or / at the end of the path)");
                Console.WriteLine(": ");
                cloneDIR = Console.ReadLine();
                
                File.WriteAllText("cloneDIR",cloneDIR);
                File.WriteAllText("PATH", path);
                
                Console.WriteLine("");
                Console.WriteLine("--One time setup done--");
            } else { path = File.ReadAllLines("PATH")[0]; cloneDIR = File.ReadAllLines("cloneDIR")[0]; }
            Console.WriteLine("type your GH email: ");
            email = Console.ReadLine();
            Console.WriteLine("type your GH username: ");
            username = Console.ReadLine();
            Console.WriteLine("type the repo git file, example: Commit-Message.git");
            Console.WriteLine(": ");
            repo = Console.ReadLine();
            commitGH("setup");
            Console.WriteLine("spell out messages on your contributions page by editing the readme.md file on a repo you select");
            Console.WriteLine("type what you want it to say (only accepts letters) : ");
            input = Console.ReadLine().ToLower();
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
                                            } else { Thread.Sleep(3600000); }
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
            if (commit_times == "setup") {
                Process process = new Process();
                ProcessStartInfo startInfo = new ProcessStartInfo();
                startInfo.WindowStyle = ProcessWindowStyle.Hidden;
                startInfo.FileName = path;
                startInfo.Arguments = "config --global user.email \""+email+"\"";
                process.StartInfo = startInfo;
                process.Start();
                process.WaitForExit();
                Process process2 = new Process();
                ProcessStartInfo startInfo2 = new ProcessStartInfo();
                startInfo2.WindowStyle = ProcessWindowStyle.Hidden;
                startInfo2.FileName = path;
                startInfo2.Arguments = "config --global user.name \""+username+"\"";
                process2.StartInfo = startInfo2;
                process2.Start();
                process2.WaitForExit();
            } else {
                if (Int32.Parse(commit_times) >= 1) {
                    int looptimes = 0;
                    while (looptimes != Int32.Parse(commit_times)) {
                        Process process3 = new Process();
                        ProcessStartInfo startInfo3 = new ProcessStartInfo();
                        startInfo3.WindowStyle = ProcessWindowStyle.Hidden;
                        startInfo3.FileName = path;
                        startInfo3.Arguments = "clone https://github.com/"+username+"/"+repo+" "+cloneDIR;
                        process3.StartInfo = startInfo3;
                        process3.Start();
                        process3.WaitForExit();
                        //write to readme.md
                        var rand = new Random();
                        File.WriteAllText(cloneDIR+"README.md",rand.Next().ToString());
                        looptimes += 1;
                        //
                        Process process5 = new Process();
                        ProcessStartInfo startInfo5 = new ProcessStartInfo();
                        startInfo5.WindowStyle = ProcessWindowStyle.Hidden;
                        startInfo5.FileName = path;
                        startInfo5.Arguments = "-C "+cloneDIR+" add README.md";
                        process5.StartInfo = startInfo5;
                        process5.Start();
                        process5.WaitForExit();
                        Process process6 = new Process();
                        ProcessStartInfo startInfo6 = new ProcessStartInfo();
                        startInfo6.WindowStyle = ProcessWindowStyle.Hidden;
                        startInfo6.FileName = path;
                        startInfo6.Arguments = "-C "+cloneDIR+" commit -m \"random\"";
                        process6.StartInfo = startInfo6;
                        process6.Start();
                        process6.WaitForExit();
                        Process process7 = new Process();
                        ProcessStartInfo startInfo7 = new ProcessStartInfo();
                        startInfo7.WindowStyle = ProcessWindowStyle.Hidden;
                        startInfo7.FileName = path;
                        startInfo7.Arguments = "-C "+cloneDIR+" init";
                        process7.StartInfo = startInfo7;
                        process7.Start();
                        process7.WaitForExit();
                        Process process8 = new Process();
                        ProcessStartInfo startInfo8 = new ProcessStartInfo();
                        startInfo8.WindowStyle = ProcessWindowStyle.Hidden;
                        startInfo8.FileName = path;
                        startInfo8.Arguments = "-C "+cloneDIR+" push origin master";
                        process8.StartInfo = startInfo8;
                        process8.Start();
                        process8.WaitForExit();
                    } Console.WriteLine("Committed");
                }
            }
        }
    }
}
