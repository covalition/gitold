using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gitoza
{
    public static class DomainFacade
    {
        internal static int[,] GetCommitCounts(string path) {
            if (string.IsNullOrEmpty(path))
                throw new Exception("The path is not set.");
            int[,] res = new int[7, 24];
            for (int i = 0; i < 24; i++)
                for (int j = 0; j < 7; j++) {
                    res[j, i] = i + j;
                }
            return res;
        }
    }
}
