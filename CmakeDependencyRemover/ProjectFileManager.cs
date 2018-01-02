using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;
using System.Text.RegularExpressions;

namespace CmakeDependencyRemover
{
    public class ProjectFileManager
    {
        static public List<string> GetProjectReferenceSection(string fileContent, string projectName)
        {
            if(fileContent == null || projectName == null)
            {
                throw new ArgumentNullException("GetProjectReferences called with null reference(s)");
            }

            //\<ProjectReference Include.*?\<Name\>ZERO_CHECK<\/Name\>\s*\<\/ProjectReference\>
            var regexString = "\\<ProjectReference Include.*?\\<Name\\>" + projectName + "<\\/Name\\>\\s*\\<\\/ProjectReference\\>";
            var regexProjectReference = new Regex(regexString, RegexOptions.Singleline);

            var matches = regexProjectReference.Matches(fileContent);

            if(matches.Count == 0)
            {
                return null;
            }
            else
            {
                return matches.Cast<Match>()
                              .Select(m => m.Value)
                              .ToList();
            }
        }

        static public string RemoveProjectReference(string fileContent, string projectName)
        {
            if(fileContent == null || projectName == null)
            {
                throw new ArgumentNullException("RemoveProjectReference called with null reference(s)");
            }

            var projectReferences = GetProjectReferenceSection(fileContent, projectName);

            if(projectReferences == null || !projectReferences.Any())
            {
                return null;
            }

            foreach(var projectReference in projectReferences)
            {
                fileContent = fileContent.Replace(projectReference, "");
            }

            return fileContent;
        }
    }
}
