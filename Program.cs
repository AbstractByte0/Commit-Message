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
        public static string pass;
        public static string lastD = "null";
        public static string input;
        public static int times;
        static void Main(string[] args) {
            if (!File.Exists("PATH")) {
                Console.WriteLine("type the path to git, windows example : C:\\Program Files\\Git\\bin\\git.exe");
                Console.WriteLine("you will need to download git from https://git-scm.com/downloads");
                Console.WriteLine(": ");
                path = Console.ReadLine();
                
                Console.WriteLine("type the path to whare it should clone the repo to (put a \\ or / at the end of the path)");
                Console.WriteLine(": ");
                cloneDIR = Console.ReadLine();

                File.Create("cloneDIR");
                File.WriteAllText("cloneDIR",cloneDIR);
                File.Create("PATH");
                File.WriteAllText("PATH", path);
            } else { path = File.ReadLines("PATH").ToString(); cloneDIR = File.ReadLines("cloneDIR").ToString(); }
            Console.WriteLine("type your GH email: ");
            email = Console.ReadLine();
            Console.WriteLine("type your GH username: ");
            username = Console.ReadLine();
            Console.WriteLine("type your GH password: ");
            pass = Console.ReadLine();
            Console.WriteLine("type the repo git file, example: Commit-Message.git");
            Console.WriteLine(": ");
            repo = Console.ReadLine();
            commitGH("setup");
            Console.WriteLine("spell out messages on your contributions page by editing the readme.md file on a repo you select");
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
                Process process3 = new Process();
                ProcessStartInfo startInfo3 = new ProcessStartInfo();
                startInfo3.WindowStyle = ProcessWindowStyle.Hidden;
                startInfo3.FileName = path;
                startInfo3.Arguments = "git clone https://github.com/"+username+"/"+repo+" "+cloneDIR;
                process3.StartInfo = startInfo3;
                process3.Start();
                process3.WaitForExit();
            } if (Int32.Parse(commit_times) >= 1) {
                int looptimes = 0;
                while (looptimes != Int32.Parse(commit_times)) {
                    looptimes += 1;
                    var rand = new Random();
                    File.WriteAllText(cloneDIR+"README.md",rand.Next().ToString());
                    Process process4 = new Process();
                    ProcessStartInfo startInfo4 = new ProcessStartInfo();
                    startInfo4.WindowStyle = ProcessWindowStyle.Hidden;
                    startInfo4.FileName = path;
                    startInfo4.Arguments = "git add README.md";
                    process4.StartInfo = startInfo4;
                    process4.Start();
                    process4.WaitForExit();
                    Process process5 = new Process();
                    ProcessStartInfo startInfo5 = new ProcessStartInfo();
                    startInfo5.WindowStyle = ProcessWindowStyle.Hidden;
                    startInfo5.FileName = path;
                    startInfo5.Arguments = "git commit -m \"random\"";
                    process5.StartInfo = startInfo5;
                    process5.Start();
                    process5.WaitForExit();
                    //git commit -m "random"
                }
                Console.WriteLine("Committed once");
            } if (commit_times == "2") {
                Console.WriteLine("Committed twice");
            }
        }
    }
}
