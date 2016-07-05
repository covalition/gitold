using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gitold.Application
{
    // http://chrisparnin.github.io/articles/2013/09/parse-git-log-output-in-c/
    public class GitFileStatus
    {
        public string Status { get; set; }
        public string File { get; set; }
    }
}
