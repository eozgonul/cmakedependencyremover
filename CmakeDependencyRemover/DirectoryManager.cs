using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;


namespace CmakeDependencyRemover
{
    public static class DirectoryManager
    {
        static public bool CheckIfDirectoryExists(string solutionDirectory)
        {
            return Directory.Exists(solutionDirectory);
        }

        static public bool CheckIfDirectoryEmpty(string solutionDirectory)
        {
            if (CheckIfDirectoryExists(solutionDirectory))
            {
                return !Directory.EnumerateFileSystemEntries(solutionDirectory).Any();
            }
            else
            {
                return true;
            }
        }

        static public List<string> GetAllFilesWithName(string solutionDirectory, string fileName)
        {  
            if(!CheckIfDirectoryEmpty(solutionDirectory))
            {
                if(string.IsNullOrEmpty(fileName))
                {
                    return null;
                }

                return Directory.GetFiles(solutionDirectory, fileName + ".*", SearchOption.AllDirectories).ToList();
            }

            return null;
        }

        static public List<string> DeleteAllFilesWithName(string solutionDirectory, string fileName)
        {
            var allFilesWithGivenName = GetAllFilesWithName(solutionDirectory, fileName);

            if(allFilesWithGivenName == null || allFilesWithGivenName.Count == 0)
            {
                return null;
            }

            var deletedFiles = new List<string>();

            foreach (var fileToDelete in allFilesWithGivenName)
            {
                File.Delete(fileToDelete);
                deletedFiles.Add(fileToDelete);
            }

            return deletedFiles;
        }

        static public void PrintFileList(List<string> fileList)
        {
            foreach(var fileName in fileList)
            {
                Console.WriteLine(fileList);
            }
        }
    }
}
