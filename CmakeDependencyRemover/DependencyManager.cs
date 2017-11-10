using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

using System.IO;

namespace CmakeDependencyRemover
{
    public class DependencyManager : IDependencyManager
    {
        public bool CheckIfSolutionDirectoryExists(string solutionDirectory)
        {
            return Directory.Exists(solutionDirectory);
        }

        public bool CheckIsSolutionDirectoryEmpty(string solutionDirectory)
        {
            if(CheckIfSolutionDirectoryExists(solutionDirectory))
            {
                return !Directory.EnumerateFileSystemEntries(solutionDirectory).Any();
            }
            else
            {
                return true;
            }
        }

        public List<string> GetAllFilesWithGivenExtensions(string solutionDirectory, List<string> listExtensions)
        {
            if(listExtensions == null || listExtensions.Count() == 0)
            {
                return null;
            }
            else if(CheckIsSolutionDirectoryEmpty(solutionDirectory))
            {
                return null;
            }

            return Directory.GetFiles(solutionDirectory, "*.*", SearchOption.AllDirectories).Where(s => listExtensions.Contains(Path.GetExtension(s))).ToList<string>();
        }

        private void DeleteAllFilesWithGivenName(string solutionDirectory, string fileName)
        {
            var listAllFilesWithGivenExtension = GetAllFilesWithGivenExtensions(solutionDirectory, new List<string> { Path.GetExtension(fileName) });

            var listAllFilesWithGivenName = listAllFilesWithGivenExtension.Where(s => Path.GetFileName(s).Equals(fileName));

            foreach(string fileNameToDelete in listAllFilesWithGivenName)
            {
                File.Delete(fileNameToDelete);
            }
        }

        public void DeleteAllBuildProjectFiles(string solutionDirectory)
        {
            DeleteAllFilesWithGivenName(solutionDirectory, "ALL_BUILD.vcxproj");
            DeleteAllFilesWithGivenName(solutionDirectory, "ALL_BUILD.vcxproj.filters");

        }

        public void DeleteZeroCheckProjectFiles(string solutionDirectory)
        {
            DeleteAllFilesWithGivenName(solutionDirectory, "ZERO_CHECK.vcxproj");
            DeleteAllFilesWithGivenName(solutionDirectory, "ZERO_CHECK.vcxproj.filters");
        }

        public void DeleteCmakeSolutionFiles(string solutionDirectory)
        {
            DeleteAllBuildProjectFiles(solutionDirectory);
            DeleteZeroCheckProjectFiles(solutionDirectory);
        }

        public bool DetectAndRemoveAllBuildAndZeroCheckProjectsFromTheSolution(string solutionPath)
        {
            var listSolutionFiles = GetAllFilesWithGivenExtensions(solutionPath, new List<string> { ".sln" });

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
        }

        public string DetectProjectUID(string fileContent, string projectName)
        {
            //Regex regex = new Regex("[A-Z|0-9]{8}-([A-Z|0-9]{4}-){3}[A-Z|0-9]{12}", RegexOptions.Singleline);

            Regex regex = new Regex("(?<=\\\"" + projectName + "\\.vcxproj\\\"\\,\\s\\\"\\{)([A-Z|0-9]{8}-([A-Z|0-9]{4}-){3}[A-Z|0-9]{12})");

            var match = regex.Match(fileContent);

            return match.Value;
        }

        public List<string> DetectSolutionConfigurations(string fileContent)
        {
            //(?<=(GlobalSection\(SolutionConfigurationPlatforms\)\s\=\spreSolution))((\s\s\s[A-z0-9]+\|[A-z0-9]+)(?:\s\=\s[A-z0-9]+\|[A-z0-9]+))+(?=(\s\sEndGlobalSection))

            Regex regexUnseparatedConfigList = new Regex("(?<=(GlobalSection\\(SolutionConfigurationPlatforms\\)\\s\\=\\spreSolution))((\\s\\s\\s[A-z0-9]+\\|[A-z0-9]+)(\\s\\=\\s[A-z0-9]+\\|[A-z0-9]+))+(?=(\\s\\sEndGlobalSection))", RegexOptions.Singleline);

            var unseparatedConfig = regexUnseparatedConfigList.Match(fileContent);

            if(unseparatedConfig.Success)
            {
                Regex regexConfigs = new Regex(@"(?<=(=\s))[A-z0-9]+\|[A-z0-9]+", RegexOptions.Singleline);
                var matchesConfigs = regexConfigs.Matches(unseparatedConfig.Value);

                if(matchesConfigs.Count != 0)
                {
                    List<string> listConfigs = new List<string>();

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
            Regex regex = new Regex("", RegexOptions.Singleline);

            var matches = regex.Matches(fileContent);

            if(matches.Count == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}
