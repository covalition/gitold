using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Igorary.Utils.Extensions;
using LibGit2Sharp;

namespace Gitold.Application
{
    public static class DomainFacade {
        public static async Task<int[,]> GetCommitCounts(string[] repoPaths) {
            int[,] res = new int[7, 24];

            List<Task<int[,]>> tasks = new List<Task<int[,]>>();
            foreach (string repoPath in repoPaths) {
                tasks.Add(GetCommitCounts(repoPath));
            }

            foreach (Task<int[,]> t in tasks) {
                int[,] counts = await t;
                for (int i = 0; i < 7; i++)
                    for (int j = 0; j < 24; j++)
                        res[i, j] += counts[i, j];
            }

            return res;
        }

        private static Task<int[,]> GetCommitCounts(string repoPath) {
            return Task.Run(() =>
            {
                using (Repository repo = new Repository(repoPath)) {
                    int[,] res = new int[7, 24];
                    List<DateTime> dates = repo.Commits
                        .QueryBy(new CommitFilter() { FirstParentOnly = true/*, IncludeReachableFrom="master"*/})
                        .Select(c => c.Committer.When.LocalDateTime)
                        .ToList();
                    var counts = dates
                       .GroupBy(d => new { d.DayOfWeek, d.Hour })
                       .Select(g => new { g.Key.DayOfWeek, g.Key.Hour, Count = g.Count() });

                    foreach (var c in counts)
                        res[(int)c.DayOfWeek, c.Hour] = c.Count;

                    return res;
                }
            });
        }

        public static async Task<Details> GetRepoDetails(string[] repoPaths) {
            List<Task<Details>> tasks = new List<Task<Details>>();
            foreach (string repoPath in repoPaths) {
                tasks.Add(GetRepoDetails(repoPath));
            }

            Details details = new Details();

            foreach (Task<Details> t in tasks) {
                Details d = await t;
                details.DateFrom = details.DateFrom.Min(d.DateFrom);
                details.DateTo = details.DateFrom.Max(d.DateTo);
                details.Commiters.AddRange(d.Commiters.Except(details.Commiters));
            }

            return details;
        }

        private static Task<Details> GetRepoDetails(string repoPath) {
            return Task.Run(() =>
            {
                using (Repository repo = new Repository(repoPath)) {
                    List<Commit> commits = repo.Commits.ToList();
                    if (!commits.Any())
                        return null;
                    Details details = new Details();
                    details.Commiters.AddRange(commits.Select(c => c.Committer.Email).ToList());
                    details.DateFrom = commits.OrderBy(c => c.Committer.When).FirstOrDefault().Committer.When.LocalDateTime;
                    details.DateTo = commits.OrderByDescending(c => c.Committer.When).FirstOrDefault().Committer.When.LocalDateTime;
                    return details;
                }
            });
        }
    }
}
