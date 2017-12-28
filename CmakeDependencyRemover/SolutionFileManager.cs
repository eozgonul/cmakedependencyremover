using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

using System.IO;

namespace CmakeDependencyRemover
{
    public class SolutionFileManager
    {
        static public string GetProjectUID(string fileContent, string projectName)
        {
            if(fileContent == null || projectName == null)
            {
                throw new ArgumentNullException("GetProjectUID called with null reference(s)");
            }

            var regex = new Regex("(?<=\\\"" + projectName + "\\.vcxproj\\\"\\,\\s\\\"\\{)([A-Z|0-9]{8}-([A-Z|0-9]{4}-){3}[A-Z|0-9]{12})");
            var match = regex.Match(fileContent);

            return string.IsNullOrEmpty(match.Value) ? null : match.Value;
        }

        static public string GetProjectInfo(string fileContent, string projectName)
        {
            if(fileContent == null || projectName == null)
            {
                throw new ArgumentNullException("GetProjectInfo called with null reference(s)");
            }

            var regularExpression = "\\bProject\\b\\(\"\\{([A-Z|0-9]+-*){5}\\}\"\\)\\s=\\s\"(\\b" + projectName + "\\b)\".*?\\bEndProject\\b";
            var regex = new Regex(regularExpression, RegexOptions.Singleline);
            var match = regex.Match(fileContent);

            return string.IsNullOrEmpty(match.Value) ? null : match.Value;
        }

        static public bool RemoveProjectInfoFromSolutionFile(string fileContent, string projectName)
        {
            if(fileContent == null || projectName == null)
            {
                throw new ArgumentNullException("RemoveProjectInfoFromSolutionFile called with null reference(s)");
            }

            var projectInfo = GetProjectInfo(fileContent, projectName);

            if(string.IsNullOrEmpty(projectInfo))
            {
                return false;
            }

            fileContent = fileContent.Replace(projectInfo, "");

            return !fileContent.Contains(projectInfo);
        }

        public bool DetectAndRemoveAllBuildAndZeroCheckProjectsFromTheSolution(string solutionPath)
        {
            var listSolutionFiles = DirectoryManager.GetAllFilesWithExtension(solutionPath, ".sln");

            if(listSolutionFiles == null)
            {
                return false;
            }

            bool result = false;

            foreach(var solutionFile in listSolutionFiles)
            {
                // \bProject\b\(\"\{([A-Z|0-9]+-*){5}\}\"\)\s=\s\"(\bALL_BUILD\b|\bZERO_CHECK\b)\".*?\bEndProject\b
                
                string regularExpression = "\\bProject\\b\\(\"\\{([A-Z|0-9]+-*){5}\\}\"\\)\\s=\\s\"(\\bALL_BUILD\\b|\\bZERO_CHECK\\b)\".*?\\bEndProject\\b";
                Regex regex = new Regex(regularExpression, RegexOptions.Singleline);

                var fileContent = File.ReadAllText(solutionFile);
                var matches = regex.Matches(fileContent);

                foreach(Match match in matches)
                {
                    fileContent = fileContent.Replace(match.Value, "");
                }

                result = result || matches.Count != 0;

                File.WriteAllText(solutionFile, fileContent);
            }

            return result;
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
