using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gitoza.BusinessLogic
{
    // based on: http://chrisparnin.github.io/articles/2013/09/parse-git-log-output-in-c/
    public class ParseGitLog
    {
        private bool startsWithHeader(string line) {
            if (line.Length > 0 && char.IsLetter(line[0])) {
                var seq = line.SkipWhile(ch => char.IsLetter(ch) && ch != ':');
                return seq.FirstOrDefault() == ':';
            }
            return false;
        }

        public async Task<List<GitCommit>> Parse(string output) {
            GitCommit commit = null;
            List<GitCommit> commits = new List<GitCommit>();
            bool processingMessage = false;
            using (StringReader strReader = new StringReader(output)) {
                do {
                    string line = await strReader.ReadLineAsync();

                    if (line.StartsWith("commit ")) {
                        if (commit != null)
                            commits.Add(commit);
                        commit = new GitCommit();
                        commit.Sha = line.Split(' ')[1];
                    }

                    if (startsWithHeader(line)) {
                        var header = line.Split(':')[0];
                        var val = string.Join(":", line.Split(':').Skip(1)).Trim();

                        // headers
                        commit.Headers.Add(header, val);
                    }

                    if (string.IsNullOrEmpty(line)) {
                        // commit message divider
                        processingMessage = !processingMessage;
                    }

                    if (line.Length > 0 && line[0] == '\t') {
                        // commit message.
                        commit.Message += line;
                    }

                    if (line.Length > 1 && Char.IsLetter(line[0]) && line[1] == '\t') {
                        var status = line.Split('\t')[0];
                        var file = line.Split('\t')[1];
                        commit.Files.Add(new GitFileStatus() { Status = status, File = file });
                    }
                }
                while (strReader.Peek() != -1);
            }
            if (commit != null)
                commits.Add(commit);

            return commits;
        }
    }
}
