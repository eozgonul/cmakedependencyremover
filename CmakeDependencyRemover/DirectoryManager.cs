using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CmakeDependencyRemover
{
    class DirectoryManager
    {
        DirectoryManager(string solutionDirectory)
        {
            SolutionDirectory = solutionDirectory;
        }

        public string SolutionDirectory { get; private set; }
    }
}
