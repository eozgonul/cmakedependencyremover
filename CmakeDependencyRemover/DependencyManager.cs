using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

using System.IO;

namespace CmakeDependencyRemover
{
    public class DependencyManager
    {
        public void DeleteProjectFiles(string directory)
        {
            DirectoryManager.DeleteAllFilesWithName(directory, "ALL_BUILD");
            DirectoryManager.DeleteAllFilesWithName(directory, "ZERO_CHECK");
        }

        public bool DetectAndRemoveAllBuildAndZeroCheckProjectsFromTheSolution(string solutionPath)
        {
            /*
            var listSolutionFiles = DirectoryManager.GetFilesWithGivenExtensions(solutionPath, new List<string> { ".sln" });

            if(listSolutionFiles == null)
            {
                return false;
            }

            bool result = false;

            foreach(string solutionFile in listSolutionFiles)
            {
                string fileContent;

                // \bProject\b\(\"\{([A-Z|0-9]+-*){5}\}\"\)\s=\s\"(\bALL_BUILD\b|\bZERO_CHECK\b)\".*?\bEndProject\b
                
                string regularExpression = "\\bProject\\b\\(\"\\{([A-Z|0-9]+-*){5}\\}\"\\)\\s=\\s\"(\\bALL_BUILD\\b|\\bZERO_CHECK\\b)\".*?\\bEndProject\\b";
                Regex regex = new Regex(regularExpression, RegexOptions.Singleline);

                fileContent = File.ReadAllText(solutionFile);
                var matches = regex.Matches(fileContent);

                foreach(Match match in matches)
                {
                    fileContent = fileContent.Replace(match.Value, "");
                }

                result = result || matches.Count != 0;

                File.WriteAllText(solutionFile, fileContent);
            }

            return result;
            */

            return false;
        }

        public string DetectProjectUID(string fileContent, string projectName)
        {
            Regex regex = new Regex("(?<=\\\"" + projectName + "\\.vcxproj\\\"\\,\\s\\\"\\{)([A-Z|0-9]{8}-([A-Z|0-9]{4}-){3}[A-Z|0-9]{12})");

            var match = regex.Match(fileContent);

            return match.Value;
        }

        public List<string> DetectSolutionConfigurations(string fileContent)
        {
            //(?<=GlobalSection\(SolutionConfigurationPlatforms\)\s\=\spreSolution\s)(\s*[A-z0-9]+\|[A-z0-9]+\s\=\s[A-z0-9]+\|[A-z0-9]+)+(?=\s*EndGlobalSection)

            var regexUnseparatedConfigList = new Regex("(?<=GlobalSection\\(SolutionConfigurationPlatforms\\)\\s\\=\\spreSolution\\s)(\\s*[A-z0-9]+\\|[A-z0-9]+\\s\\=\\s[A-z0-9]+\\|[A-z0-9]+)+(?=\\s*EndGlobalSection)"/*, RegexOptions.Singleline*/);


            var unseparatedConfig = regexUnseparatedConfigList.Match(fileContent);

            if(unseparatedConfig.Success)
            {
                Regex regexConfigs = new Regex("([A-z0-9]+\\|[A-z0-9]+)(?=\\s*\\=\\s[A-z0-9]+\\|[A-z0-9]+)", RegexOptions.Singleline);
                var matchesConfigs = regexConfigs.Matches(unseparatedConfig.Value);

                if(matchesConfigs.Count != 0)
                {
                    var listConfigs = new List<string>();

                    foreach(Match match in matchesConfigs)
                    {
                        listConfigs.Add(match.Value);
                    }

                    return listConfigs;
                }
            }

            return null;
        }

        public bool RemoveProjectUIDFromGlobalSettings(string fileContent, string uid)
        {
            return false;
        }
    }
}
