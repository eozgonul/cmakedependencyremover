using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NUnit.Framework;
using System.IO;

namespace CmakeDependencyRemover.Test
{
    [TestFixture]
    class DirectoryTests
    {
        string existingDirectory;
        string nonExistingDirectory;

        string emptyDirectory;
        List<string> filesAllBuild;
        List<string> filesZeroCheck;

        [SetUp]
        public void SetDirectories()
        {
            existingDirectory = Path.GetDirectoryName(Path.GetDirectoryName(TestContext.CurrentContext.TestDirectory)) + @"\resource\FileExtensionTest";
            nonExistingDirectory = existingDirectory + @"\nonexisting\directory";

            emptyDirectory = existingDirectory + @"\emptydirectory";

            filesAllBuild = new List<string> { existingDirectory + @"\ALL_BUILD.vcxproj",
                                               existingDirectory + @"\ALL_BUILD.vcxproj.filters",
                                               existingDirectory + @"\SomeInnerDirectory\ALL_BUILD.vcxproj",
                                               existingDirectory + @"\SomeInnerDirectory\ALL_BUILD.vcxproj.filters", };

            filesZeroCheck = new List<string> { existingDirectory + @"\ZERO_CHECK.vcxproj",
                                                existingDirectory + @"\ZERO_CHECK.vcxproj.filters",
                                                existingDirectory + @"\SomeInnerDirectory\ZERO_CHECK.vcxproj",
                                                existingDirectory + @"\SomeInnerDirectory\ZERO_CHECK.vcxproj.filters"};                                        
        }

        [Test, Category("Directory")]
        public void CheckIfDirectoryExists_DirectoryExists_ReturnTrue()
        {
            var result = DirectoryManager.CheckIfDirectoryExists(existingDirectory);

            Assert.That(result, Is.True);
        }

        [Test, Category("Directory")]
        public void CheckIfDirectoryExists_DirectoryDoesNotExist_ReturnFalse()
        {
            var result = DirectoryManager.CheckIfDirectoryExists(nonExistingDirectory);

            Assert.That(result, Is.False);
        }

        [Test, Category("Directory")]
        public void CheckIfDirectoryEmpty_DirectoryNotEmpty_ReturnTrue()
        {
            var result = DirectoryManager.CheckIfDirectoryEmpty(existingDirectory);

            Assert.That(result, Is.False);
        }

        [Test, Category("Directory")]
        public void CheckIfDirectoryEmpty_DirectoryDoesNotExist_ReturnTrue()
        {
            var result = DirectoryManager.CheckIfDirectoryEmpty(nonExistingDirectory);

            Assert.That(result, Is.True);
        }

        [Test, Category("Directory")]
        public void CheckIfDirectoryEmpty_DirectoryEmpty_ReturnTrue()
        {
            var result = DirectoryManager.CheckIfDirectoryEmpty(emptyDirectory);

            Assert.That(result, Is.True);
        }

        [Test, Category("Files")]
        public void GetAllFilesWithGivenName_DirectoryDoesNotExistFileNameNull_ReturnNull()
        {
            var result = DirectoryManager.GetAllFilesWithName(nonExistingDirectory, null);

            Assert.That(result, Is.Null);
        }

        [Test, Category("Files")]
        public void GetAllFilesWithGivenName_DirectoryDoesNotExistFileNameEmpty_ReturnNull()
        {
            var result = DirectoryManager.GetAllFilesWithName(nonExistingDirectory, "");

            Assert.That(result, Is.Null);
        }

        [Test, Category("Files")]
        public void GetAllFilesWithGivenName_DirectoryDoesNotExistFileNameSet_ReturnNull()
        {
            var result = DirectoryManager.GetAllFilesWithName(nonExistingDirectory, "someFileName");

            Assert.That(result, Is.Null);
        }

        [Test, Category("Files")]
        public void GetAllFilesWithGivenName_DirectoryEmptyFileNameNull_ReturnNull()
        {
            var result = DirectoryManager.GetAllFilesWithName(emptyDirectory, null);

            Assert.That(result, Is.Null);
        }

        [Test, Category("Files")]
        public void GetAllFilesWithGivenName_DirectoryNonEmptyFileNameNull_ReturnNull()
        {
            var result = DirectoryManager.GetAllFilesWithName(existingDirectory, null);

            Assert.That(result, Is.Null);
        }

        [Test, Category("Files")]
        public void GetAllFilesWithGivenName_DirectoryEmptyFileNameEmpty_ReturnNull()
        {
            var result = DirectoryManager.GetAllFilesWithName(emptyDirectory, "");

            Assert.That(result, Is.Null);
        }

        [Test, Category("Files")]
        public void GetAllFilesWithGivenName_DirectoryNonEmptyFileNameEmpty_ReturnNull()
        {
            var result = DirectoryManager.GetAllFilesWithName(existingDirectory, "");

            Assert.That(result, Is.Null);
        }

        [Test, Category("Files")]
        public void GetAllFilesWithGivenName_DirectoryEmptyFileNameSet_ReturnNull()
        {
            var result = DirectoryManager.GetAllFilesWithName(emptyDirectory, "ALL_BUILD");

            Assert.That(result, Is.Null);
        }

        [Test, Category("Files")]
        public void GetAllFilesWithGivenName_DirectoryNonEmptyFileNameAllBuild_ReturnAllBuildFiles()
        {
            var result = DirectoryManager.GetAllFilesWithName(existingDirectory, "ALL_BUILD");

            Assert.That(filesAllBuild.SequenceEqual<string>(result), Is.True);
        }

        [Test, Category("Files")]
        public void GetAllFilesWithGivenName_DirectoryNonEmptyFileNameZeroCheck_ReturnZeroCheckFiles()
        {
            var result = DirectoryManager.GetAllFilesWithName(existingDirectory, "ZERO_CHECK");

            Assert.That(filesZeroCheck.SequenceEqual<string>(result), Is.True);
        }

        [Test, Category("Files")]
        public void GetAllFilesWithGivenName_DirectoryNonEmptyFileNameDoesNotExist_ReturnEmpty()
        {
            var result = DirectoryManager.GetAllFilesWithName(existingDirectory, "NonExistingFileName");

            Assert.That(result, Is.Empty);
        }

        [Test, Category("DeleteFiles")]
        public void DeleteAllFilesWithGivenName_DirectoryDoesNotExistFileNameNull_ReturnNull()
        {
            var result = DirectoryManager.DeleteAllFilesWithName(nonExistingDirectory, null);

            Assert.That(result, Is.Null);
        }

        [Test, Category("DeleteFiles")]
        public void DeleteAllFilesWithGivenName_DirectoryDoesNotExistFileNameEmpty_ReturnNull()
        {
            var result = DirectoryManager.DeleteAllFilesWithName(nonExistingDirectory, null);

            Assert.That(result, Is.Null);
        }

        [Test, Category("DeleteFiles")]
        public void DeleteAllFilesWithGivenName_DirectoryDoesNotExistFileNameSet_ReturnNull()
        {
            var result = DirectoryManager.DeleteAllFilesWithName(nonExistingDirectory, "ALL_BUILD");

            Assert.That(result, Is.Null);
        }

        [Test, Category("DeleteFiles")]
        public void DeleteAllFilesWithGivenName_DirectoryEmptyFileNameNull_ReturnNull()
        {
            var result = DirectoryManager.DeleteAllFilesWithName(emptyDirectory, null);

            Assert.That(result, Is.Null);
        }

        [Test, Category("DeleteFiles")]
        public void DeleteAllFilesWithGivenName_DirectoryEmptyFileNameEmpty_ReturnNull()
        {
            var result = DirectoryManager.DeleteAllFilesWithName(emptyDirectory, "");

            Assert.That(result, Is.Null);
        }

        [Test, Category("DeleteFiles")]
        public void DeleteAllFilesWithGivenName_DirectoryEmptyFileNameSet_ReturnNull()
        {
            var result = DirectoryManager.DeleteAllFilesWithName(emptyDirectory, "ALL_BUILD");

            Assert.That(result, Is.Null);
        }

        [Test, Category("DeleteFiles")]
        public void DeleteAllFilesWithGivenName_DirectoryNonEmptyFileNameNull_ReturnNull()
        {
            var result = DirectoryManager.DeleteAllFilesWithName(existingDirectory, null);

            Assert.That(result, Is.Null);
        }

        [Test, Category("DeleteFiles")]
        public void DeleteAllFilesWithGivenName_DirectoryNonEmptyFileNameEmpty_ReturnNull()
        {
            var result = DirectoryManager.DeleteAllFilesWithName(existingDirectory, "");

            Assert.That(result, Is.Null);
        }

        [Test, Category("DeleteFiles")]
        public void DeleteAllFilesWithGivenName_DirectoryNonEmptyFileNameDoesNotExist_ReturnNull()
        {
            var result = DirectoryManager.DeleteAllFilesWithName(existingDirectory, "someFileName");

            Assert.That(result, Is.Null);
        }

        [Test, Category("DeleteFiles")]
        public void DeleteAllFilesWithGivenName_DirectoryNonEmptyFileNameAllBuild_ReturnListOfDeletedAllBuildFiles()
        {
            var result = DirectoryManager.DeleteAllFilesWithName(existingDirectory, "ALL_BUILD");

            Assert.True(filesAllBuild.SequenceEqual(result) &&
                        Directory.GetFiles(existingDirectory).Where(s => Path.GetFileName(s).Equals("ALL_BUILD.vcxproj")).Count() == 0 &&
                        Directory.GetFiles(existingDirectory).Where(s => Path.GetFileName(s).Equals("ALL_BUILD.vcxproj.filters")).Count() == 0);
        }

        [Test, Category("DeleteFiles")]
        public void DeleteAllFilesWithGivenName_DirectoryNonEmptyFileNameZeroCheck_ReturnListOfDeletedZeroCheckFiles()
        {
            var result = DirectoryManager.DeleteAllFilesWithName(existingDirectory, "ZERO_CHECK");

            Assert.True(filesZeroCheck.SequenceEqual(result) &&
                        Directory.GetFiles(existingDirectory).Where(s => Path.GetFileName(s).Equals("ZERO_CHECK.vcxproj")).Count() == 0 &&
                        Directory.GetFiles(existingDirectory).Where(s => Path.GetFileName(s).Equals("ZERO_CHECK.vcxproj.filters")).Count() == 0);
        }
    }
}

        /*

        [SetUp]
        public void SetupSolutionPath()
        {
            listConfigurations = new List<string> { "Debug|x64", "Release|x64" };
        }

        [Test, Category("ProjectFile")]
        public void DetectProjectUIDFromSolutionFile_AllBuildUIDChecked_True()
        {
            var fileContent = File.ReadAllText(existingSolutionDirectory + "\\Boggle.sln");

            var result = DependencyManager.DetectProjectUID(fileContent, "ALL_BUILD");

            Assert.AreEqual("8BAE2EF6-B67E-3634-9B27-B99536808058", result);
        }

        [Test, Category("ProjectFile")]
        public void DetectProjectUIDFromSolutionFile_ZeroCheckUIDChecked_True()
        {
            var fileContent = File.ReadAllText(existingSolutionDirectory + "\\Boggle.sln");

            var result = DependencyManager.DetectProjectUID(fileContent, "ZERO_CHECK");

            Assert.AreEqual("B28D5454-5C92-348F-BB2D-BB0B9FAE4192", result);
        }


        [Test, Category("ProjectFile")]
        public void DetectSolutionConfigurations_Debugx64Releasex64_True()
        {
            var fileContent = File.ReadAllText(existingSolutionDirectory + "\\Boggle.sln");

            var solutionConfigurations = DependencyManager.DetectSolutionConfigurations(fileContent);

            Assert.True(listConfigurations.SequenceEqual<string>(solutionConfigurations));
        }

        [Test, Category("ProjectFile")]
        public void RemoveProjectUIDFromSolutionFileGlobalSettings()
        {
            var fileContent = File.ReadAllText(existingSolutionDirectory + "\\Boggle.sln");

            var allBuildUID = DependencyManager.DetectProjectUID(fileContent, "ALL_BUILD");
            var result = DependencyManager.RemoveProjectUIDFromGlobalSettings(fileContent, allBuildUID);

            Assert.True(result);
        }
    }
}
*/
