using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Gitoza
{
    public static class DomainFacade
    {
        private static string listShaWithFiles(string path) {
            var output = RunProcess(string.Format(" --git-dir=\"{0}/.git\" --work-tree=\"{1}\" log --name-status", path.Replace("\\", "/"), path.Replace("\\", "/")));
            //var output = RunProcess("dupa");
            return output;
        }

        private static string RunProcess(string command) {
            // Start the child process.
            Process p = new Process();
            // Redirect the output stream of the child process.
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.RedirectStandardOutput = true;
            p.StartInfo.RedirectStandardError = true;
            p.StartInfo.FileName = Properties.Settings.Default.GitExecutable;
            p.StartInfo.Arguments = command;
            p.ErrorDataReceived += p_ErrorDataReceived;
            p.Start();
            p.BeginErrorReadLine();
            // Read the output stream first and then wait.
            string output = p.StandardOutput.ReadToEnd();
            // string error = p.StandardError.ReadToEnd();
            p.WaitForExit();
            //if (errorMsg != null) {
            //    MessageBox.Show(errorMsg);
            //    errorMsg = null;
            //}
            return output;
        }

        static string errorMsg;

        static void p_ErrorDataReceived(object sender, DataReceivedEventArgs e) {
            if(e.Data != null)
                MessageBox.Show(e.Data);
        }

        private static void test(string[] args) {
            string path = @"C:\DEV\github\Codegrams";
            if (args.Length > 0)
                path = args[0];
            string output = listShaWithFiles(path);

            ParseGitLog parser = new ParseGitLog();
            List<GitCommit> commits = parser.Parse(output);

            Console.WriteLine(commits.Count);
            foreach (var commit in commits) {
                commit.Print();
            }
        }

        // http://chrisparnin.github.io/articles/2013/09/parse-git-log-output-in-c/
        internal static List<int> GetCommitCounts(string repoPath) {
            if (string.IsNullOrEmpty(repoPath))
                throw new Exception("The path is not set.");

            string output = listShaWithFiles(repoPath);
            ParseGitLog parser = new ParseGitLog();
            List<GitCommit> commits = parser.Parse(output);

            List<int> res = new List<int>();
            for (int j = 0; j < 7; j++)
                for (int i = 0; i < 24; i++)
                    res.Add(i + j);
            return res;
        }
    }
}
