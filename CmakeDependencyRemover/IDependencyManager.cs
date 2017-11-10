using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CmakeDependencyRemover
{
    public interface IDependencyManager
    {
        bool CheckIfSolutionDirectoryExists(string solutionDirectory);
        bool CheckIsSolutionDirectoryEmpty(string solutionDirectory);

        List<string> GetAllFilesWithGivenExtensions(string solutionDirectory, List<string> listExtensions);

        void DeleteCmakeSolutionFiles(string solutionDirectory);

        void DeleteAllBuildProjectFiles(string solutionDirectory);
        void DeleteZeroCheckProjectFiles(string solutionDirectory);

        bool DetectAndRemoveAllBuildAndZeroCheckProjectsFromTheSolution(string solutionPath);

        string DetectProjectUID(string fileContent, string projectName);
        List<string> DetectSolutionConfigurations(string fileContent);
        bool RemoveProjectUIDFromGlobalSettings(string fileContent, string uid);
    }
}
