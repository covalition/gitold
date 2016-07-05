using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gitold.ViewModels
{
    public class RepoExceptionMessage
    {
        public Exception Exception { get; private set; }

        public RepoExceptionMessage(Exception ex) {
            Exception = ex;
        }
    }
}
