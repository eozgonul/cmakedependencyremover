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
        static public bool CheckIfDirectoryExists(string directory)
        {
            return Directory.Exists(directory);
        }

        static public bool CheckIfDirectoryEmpty(string directory)
        {
            if (CheckIfDirectoryExists(directory))
            {
                return !Directory.EnumerateFileSystemEntries(directory).Any();
            }

            return true;
        }

        static public List<string> GetAllFilesWithName(string directory, string fileName)
        {  
            if(!CheckIfDirectoryEmpty(directory))
            {
                if(string.IsNullOrEmpty(fileName))
                {
                    return null;
                }

                return Directory.GetFiles(directory, fileName + ".*", SearchOption.AllDirectories).ToList();
            }

            return null;
        }

        static public List<string> GetAllFilesWithExtension(string directory, string fileExtension)
        {
            if(!CheckIfDirectoryEmpty(directory))
            {
                if(string.IsNullOrEmpty(fileExtension))
                {
                    return null;
                }

                return Directory.GetFiles(directory, "*.*", SearchOption.AllDirectories).Where(s => fileExtension.Contains(Path.GetExtension(s))).ToList<string>();
            }

            return null;
        }

        static public List<string> DeleteAllFilesWithName(string directory, string fileName)
        {
            var allFilesWithGivenName = GetAllFilesWithName(directory, fileName);

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
