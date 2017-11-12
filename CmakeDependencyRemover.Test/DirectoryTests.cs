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
        string existingSolutionDirectory;
        string nonExistingSolutionDirectory;
        string emptyDirectory;

        List<string> listAvailableExtensions;
        List<string> listNonAvailableExtensions;
        List<string> listEmptyExtensions;
        List<string> listNullExtensions;

        List<string> listFilesWithExtensions;
        List<string> listConfigurations;

        CmakeDependencyRemover.IDependencyManager DependencyManager;


        [SetUp]
        public void SetupSolutionPath()
        {
            DependencyManager = new CmakeDependencyRemover.DependencyManager();

            existingSolutionDirectory = Path.GetDirectoryName(Path.GetDirectoryName(TestContext.CurrentContext.TestDirectory)) + "\\resource\\FileExtensionTest";
            nonExistingSolutionDirectory = existingSolutionDirectory + "\\nonexisting\\directory";
            emptyDirectory = existingSolutionDirectory + "\\emptydirectory";

            listAvailableExtensions = new List<string> { ".sln", ".vcxproj", ".filters"};
            listNonAvailableExtensions = new List<string> {"dumymextension1", "dummyExtension2"};
            listEmptyExtensions = new List<string>();
            listNullExtensions = null;

            listFilesWithExtensions = new List<string> { "C:\\Users\\eozgonul\\Documents\\Visual Studio 2017\\Projects\\CmakeDependencyRemover\\CmakeDependencyRemover.Test\\resource\\FileExtensionTest\\ALL_BUILD.vcxproj",
                                                         "C:\\Users\\eozgonul\\Documents\\Visual Studio 2017\\Projects\\CmakeDependencyRemover\\CmakeDependencyRemover.Test\\resource\\FileExtensionTest\\ALL_BUILD.vcxproj.filters",
                                                         "C:\\Users\\eozgonul\\Documents\\Visual Studio 2017\\Projects\\CmakeDependencyRemover\\CmakeDependencyRemover.Test\\resource\\FileExtensionTest\\Boggle.sln",
                                                         "C:\\Users\\eozgonul\\Documents\\Visual Studio 2017\\Projects\\CmakeDependencyRemover\\CmakeDependencyRemover.Test\\resource\\FileExtensionTest\\ZERO_CHECK.vcxproj",
                                                         "C:\\Users\\eozgonul\\Documents\\Visual Studio 2017\\Projects\\CmakeDependencyRemover\\CmakeDependencyRemover.Test\\resource\\FileExtensionTest\\ZERO_CHECK.vcxproj.filters",
                                                         "C:\\Users\\eozgonul\\Documents\\Visual Studio 2017\\Projects\\CmakeDependencyRemover\\CmakeDependencyRemover.Test\\resource\\FileExtensionTest\\SomeInnerDirectory\\ALL_BUILD.vcxproj",
                                                         "C:\\Users\\eozgonul\\Documents\\Visual Studio 2017\\Projects\\CmakeDependencyRemover\\CmakeDependencyRemover.Test\\resource\\FileExtensionTest\\SomeInnerDirectory\\ALL_BUILD.vcxproj.filters",
                                                         "C:\\Users\\eozgonul\\Documents\\Visual Studio 2017\\Projects\\CmakeDependencyRemover\\CmakeDependencyRemover.Test\\resource\\FileExtensionTest\\SomeInnerDirectory\\Boggle.sln",
                                                         "C:\\Users\\eozgonul\\Documents\\Visual Studio 2017\\Projects\\CmakeDependencyRemover\\CmakeDependencyRemover.Test\\resource\\FileExtensionTest\\SomeInnerDirectory\\ZERO_CHECK.vcxproj",
                                                         "C:\\Users\\eozgonul\\Documents\\Visual Studio 2017\\Projects\\CmakeDependencyRemover\\CmakeDependencyRemover.Test\\resource\\FileExtensionTest\\SomeInnerDirectory\\ZERO_CHECK.vcxproj.filters"};

            listConfigurations = new List<string> { "Debug|x64", "Release|x64" };
        }


        [Test, Category("Directory")]
        public void IsDirectoryValid_DirectoryValid_True()
        {
            TestContext.WriteLine(existingSolutionDirectory);

            Assert.True(DependencyManager.CheckIfSolutionDirectoryExists(existingSolutionDirectory));
        }

        [Test, Category("Directory")]
        public void IsDirectoryValid_DirectoryInvalid_False()
        {
            Assert.False(DependencyManager.CheckIfSolutionDirectoryExists(nonExistingSolutionDirectory));
        }

        [Test, Category("Directory")]
        public void IsDirectoryEmpty_DirectoryNotEmpty_False()
        {
            Assert.False(DependencyManager.CheckIsSolutionDirectoryEmpty(existingSolutionDirectory));
        }

        [Test, Category("Directory")]
        public void IsDirectoryEmpty_DirectoryEmpty_True()
        {
            Assert.True(DependencyManager.CheckIsSolutionDirectoryEmpty(emptyDirectory));
        }

        [Test, Category("ExtensionCheck")]
        public void IfFilesWithGivenExtensionAvailable_DirectoryDoesNotExist_Null()
        {
            Assert.Null(DependencyManager.GetAllFilesWithGivenExtensions(nonExistingSolutionDirectory, listAvailableExtensions));
        }

        [Test, Category("ExtensionCheck")]
        public void IfFilesWithGivenExtensionAvailable_DirectoryEmpty_Null()
        {
            Assert.Null(DependencyManager.GetAllFilesWithGivenExtensions(emptyDirectory, listAvailableExtensions));
        }

        [Test, Category("ExtensionCheck")]
        public void IfFilesWithGivenExtensionAvailable_ExtensionListEmpty_Null()
        {
            Assert.Null(DependencyManager.GetAllFilesWithGivenExtensions(existingSolutionDirectory, listEmptyExtensions));
        }

        [Test, Category("ExtensionCheck")]
        public void IfFilesWithGivenExtensionAvailable_ExtensionListNull_Null()
        {
            Assert.Null(DependencyManager.GetAllFilesWithGivenExtensions(existingSolutionDirectory, listNullExtensions));
        }


        [Test, Category("ExtensionCheck")]
        public void IfFilesWithGivenExtensionAvailable_FilesWithExtensionsNotPresent_Null()
        {
            Assert.IsEmpty(DependencyManager.GetAllFilesWithGivenExtensions(existingSolutionDirectory, listNonAvailableExtensions));
        }

        [Test, Category("ExtensionCheck")]
        public void IfFilesWithGivenExtensionAvailable_FilesWithExtensionsPresent_Return10Items()
        {
            Assert.AreEqual(10, DependencyManager.GetAllFilesWithGivenExtensions(existingSolutionDirectory, listAvailableExtensions).Count());
        }

        [Test, Category("ExtensionCheck")]
        public void IfFilesWithGivenExtensionAvailable_FilesWithExtensionsPresent_CorrectFilesRetrieved()
        {
            Assert.True(listFilesWithExtensions.SequenceEqual<string>(DependencyManager.GetAllFilesWithGivenExtensions(existingSolutionDirectory, listAvailableExtensions)));
        }

        [Test, Category("ProjectFile")]
        public void DeleteAllBuildFiles_AllBuildFilePresent_True()
        {
            DependencyManager.DeleteAllBuildProjectFiles(existingSolutionDirectory);

            Assert.True(Directory.GetFiles(existingSolutionDirectory).Where(s => Path.GetFileName(s).Equals("ALL_BUILD.vcxproj")).Count() == 0 &&
                        Directory.GetFiles(existingSolutionDirectory).Where(s => Path.GetFileName(s).Equals("ALL_BUILD.vcxproj.filters")).Count() == 0);
        }

        [Test, Category("ProjectFile")]
        public void DeleteZeroCheckProjectFiles_ZeroCheckProjectFilesPresent_True()
        {
            DependencyManager.DeleteZeroCheckProjectFiles(existingSolutionDirectory);

            Assert.True(Directory.GetFiles(existingSolutionDirectory).Where(s => Path.GetFileName(s).Equals("ZERO_CHECK.vcxproj")).Count() == 0 &&
                        Directory.GetFiles(existingSolutionDirectory).Where(s => Path.GetFileName(s).Equals("ZERO_CHECK.vcxproj.filters")).Count() == 0);
        }

        [Test, Category("ProjectFile")]
        public void DeleteZeroCheckProjectFiles_ZeroCheckProjectFilesNotPresent_()
        {
            DependencyManager.DeleteZeroCheckProjectFiles(existingSolutionDirectory);

            Assert.True(Directory.GetFiles(existingSolutionDirectory).Where(s => Path.GetFileName(s).Equals("ZERO_CHECK.vcxproj")).Count() == 0 &&
                        Directory.GetFiles(existingSolutionDirectory).Where(s => Path.GetFileName(s).Equals("ZERO_CHECK.vcxproj.filters")).Count() == 0);
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
