using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gitoza
{
    public class GitFileStatus
    {
        public string Status { get; set; }
        public string File { get; set; }
    }

    public class GitCommit
    {
        public GitCommit() {
            Headers = new Dictionary<string, string>();
            Files = new List<GitFileStatus>();
            Message = "";
        }

        public Dictionary<string, string> Headers { get; set; }
        public string Sha { get; set; }
        public string Message { get; set; }
        public List<GitFileStatus> Files { get; set; }

        public void Print() {
            Console.WriteLine("commit " + Sha);
            foreach (var key in Headers.Keys) {
                Console.WriteLine(key + ":" + Headers[key]);
            }
            Console.WriteLine();
            Console.WriteLine(Message);
            Console.WriteLine();
            foreach (var file in Files) {
                Console.WriteLine(file.Status + "\t" + file.File);
            }
        }
    }

    public static class DomainFacade
    {
        public static string ListShaWithFiles(string path) {
            var output = RunProcess(string.Format(" --git-dir={0}/.git --work-tree={1} log --name-status", path.Replace("\\", "/"), path.Replace("\\", "/")));
            return output;
        }

        private static string RunProcess(string command) {
            // Start the child process.
            Process p = new Process();
            // Redirect the output stream of the child process.
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.RedirectStandardOutput = true;
            p.StartInfo.FileName = Properties.Settings.Default.GitExecutable;
            p.StartInfo.Arguments = command;
            p.Start();
            // Read the output stream first and then wait.
            string output = p.StandardOutput.ReadToEnd();
            p.WaitForExit();
            return output;
        }

        private static void test(string[] args) {
            string path = @"C:\DEV\github\Codegrams";
            if (args.Length > 0)
                path = args[0];
            string output = ListShaWithFiles(path);

            ParseGitLog parser = new ParseGitLog();
            List<GitCommit> commits = parser.Parse(output);

            Console.WriteLine(commits.Count);
            foreach (var commit in commits) {
                commit.Print();
            }
        }

        // http://chrisparnin.github.io/articles/2013/09/parse-git-log-output-in-c/
        internal static List<int> GetCommitCounts(string path) {
            if (string.IsNullOrEmpty(path))
                throw new Exception("The path is not set.");
            List<int> res = new List<int>();
            for (int j = 0; j < 7; j++)
                for (int i = 0; i < 24; i++)
                    res.Add(i + j);
            return res;
        }
    }
}
