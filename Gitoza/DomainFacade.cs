using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gitoza
{
    public static class DomainFacade
    {
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
