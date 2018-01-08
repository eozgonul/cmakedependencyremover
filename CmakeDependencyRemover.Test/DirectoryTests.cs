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
        List<string> filesSolution;

        [SetUp]
        public void SetUp()
        {
            existingDirectory = Path.GetDirectoryName(Path.GetDirectoryName(TestContext.CurrentContext.TestDirectory)) + @"\resource\TestFiles";
            nonExistingDirectory = existingDirectory + @"\nonexisting\directory";

            emptyDirectory = existingDirectory + @"\emptydirectory";

            filesAllBuild = new List<string> { existingDirectory + @"\ALL_BUILD.vcxproj",
                                               existingDirectory + @"\ALL_BUILD.vcxproj.filters",
                                               existingDirectory + @"\boggle_test\ALL_BUILD.vcxproj",
                                               existingDirectory + @"\boggle_test\ALL_BUILD.vcxproj.filters"};

            filesZeroCheck = new List<string> { existingDirectory + @"\ZERO_CHECK.vcxproj",
                                                existingDirectory + @"\ZERO_CHECK.vcxproj.filters",
                                                existingDirectory + @"\boggle_test\ZERO_CHECK.vcxproj",
                                                existingDirectory + @"\boggle_test\ZERO_CHECK.vcxproj.filters"};

            filesSolution = new List<string> { existingDirectory + @"\Boggle.sln",
                                               existingDirectory + @"\boggle_test\Boggle.sln"};
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
        public void CheckIfDirectoryExists_DirectoryNull_ThrowArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => DirectoryManager.CheckIfDirectoryExists(null));
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

        [Test, Category("Directory")]
        public void CheckIfDirectoryEmpty_DirectoryNull_ThrowArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => DirectoryManager.CheckIfDirectoryEmpty(null));
        }

        [Test, Category("Files")]
        public void GetAllFilesWithName_DirectoryDoesNotExistFileNameNull_ThrowArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => DirectoryManager.GetAllFilesWithName(nonExistingDirectory, null));
        }

        [Test, Category("Files")]
        public void GetAllFilesWithName_DirectoryDoesNotExistFileNameEmpty_ReturnNull()
        {
            var result = DirectoryManager.GetAllFilesWithName(nonExistingDirectory, "");

            Assert.That(result, Is.Null);
        }

        [Test, Category("Files")]
        public void GetAllFilesWithName_DirectoryDoesNotExistFileNameSet_ReturnNull()
        {
            var result = DirectoryManager.GetAllFilesWithName(nonExistingDirectory, "someFileName");

            Assert.That(result, Is.Null);
        }

        [Test, Category("Files")]
        public void GetAllFilesWithName_DirectoryEmptyFileNameNull_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => DirectoryManager.GetAllFilesWithName(emptyDirectory, null));
        }

        [Test, Category("Files")]
        public void GetAllFilesWithName_DirectoryNonEmptyFileNameNull_ThrowArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => DirectoryManager.GetAllFilesWithName(existingDirectory, null));
        }

        [Test, Category("Files")]
        public void GetAllFilesWithName_DirectoryEmptyFileNameEmpty_ReturnNull()
        {
            var result = DirectoryManager.GetAllFilesWithName(emptyDirectory, "");

            Assert.That(result, Is.Null);
        }

        [Test, Category("Files")]
        public void GetAllFilesWithName_DirectoryNonEmptyFileNameEmpty_ReturnNull()
        {
            var result = DirectoryManager.GetAllFilesWithName(existingDirectory, "");

            Assert.That(result, Is.Null);
        }

        [Test, Category("Files")]
        public void GetAllFilesWithName_DirectoryEmptyFileNameSet_ReturnNull()
        {
            var result = DirectoryManager.GetAllFilesWithName(emptyDirectory, "ALL_BUILD");

            Assert.That(result, Is.Null);
        }

        [Test, Category("Files")]
        public void GetAllFilesWithName_DirectoryNonEmptyFileNameAllBuild_ReturnAllBuildFiles()
        {
            var result = DirectoryManager.GetAllFilesWithName(existingDirectory, "ALL_BUILD");

            Assert.That(filesAllBuild.SequenceEqual<string>(result), Is.True);
        }

        [Test, Category("Files")]
        public void GetAllFilesWithName_DirectoryNonEmptyFileNameZeroCheck_ReturnZeroCheckFiles()
        {
            var result = DirectoryManager.GetAllFilesWithName(existingDirectory, "ZERO_CHECK");

            Assert.That(filesZeroCheck.SequenceEqual<string>(result), Is.True);
        }

        [Test, Category("Files")]
        public void GetAllFilesWithName_DirectoryNonEmptyFileNameDoesNotExist_ReturnEmpty()
        {
            var result = DirectoryManager.GetAllFilesWithName(existingDirectory, "NonExistingFileName");

            Assert.That(result, Is.Empty);
        }

        [Category("Files")]
        [TestCase(null, "")]
        [TestCase("", null)]
        [TestCase("someDirectory", null)]
        [TestCase(null, "someFileName")]
        public void GetAllFilesWithName_ParametersNull_ThrowArgumentNullException(string directory, string fileName)
        {
            Assert.Throws<ArgumentNullException>(() => DirectoryManager.GetAllFilesWithName(directory, fileName));
        }

        [Test, Category("DeleteFiles")]
        public void DeleteAllFilesWithName_DirectoryDoesNotExistFileNameNull_ThrowArgumentNulLException()
        {
            Assert.Throws<ArgumentNullException>(() => DirectoryManager.DeleteAllFilesWithName(nonExistingDirectory, null));
        }

        [Test, Category("DeleteFiles")]
        public void DeleteAllFilesWithName_DirectoryDoesNotExistFileNameEmpty_ThrowArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => DirectoryManager.DeleteAllFilesWithName(nonExistingDirectory, null));
        }

        [Test, Category("DeleteFiles")]
        public void DeleteAllFilesWithName_DirectoryDoesNotExistFileNameSet_ReturnNull()
        {
            var result = DirectoryManager.DeleteAllFilesWithName(nonExistingDirectory, "ALL_BUILD");

            Assert.That(result, Is.Null);
        }

        [Test, Category("DeleteFiles")]
        public void DeleteAllFilesWitName_DirectoryEmptyFileNameNull_ThrowNullArgumentException()
        {
            Assert.Throws<ArgumentNullException>(() => DirectoryManager.DeleteAllFilesWithName(emptyDirectory, null));
        }

        [Test, Category("DeleteFiles")]
        public void DeleteAllFilesWithName_DirectoryEmptyFileNameEmpty_ReturnNull()
        {
            var result = DirectoryManager.DeleteAllFilesWithName(emptyDirectory, "");

            Assert.That(result, Is.Null);
        }

        [Test, Category("DeleteFiles")]
        public void DeleteAllFilesWithName_DirectoryEmptyFileNameSet_ReturnNull()
        {
            var result = DirectoryManager.DeleteAllFilesWithName(emptyDirectory, "ALL_BUILD");

            Assert.That(result, Is.Null);
        }

        [Test, Category("DeleteFiles")]
        public void DeleteAllFilesWithName_DirectoryNonEmptyFileNameNull_ThrowNullArgumentException()
        {
            Assert.Throws<ArgumentNullException>(() => DirectoryManager.DeleteAllFilesWithName(existingDirectory, null));
        }

        [Test, Category("DeleteFiles")]
        public void DeleteAllFilesWithName_DirectoryNonEmptyFileNameEmpty_ReturnNull()
        {
            var result = DirectoryManager.DeleteAllFilesWithName(existingDirectory, "");

            Assert.That(result, Is.Null);
        }

        [Test, Category("DeleteFiles")]
        public void DeleteAllFilesWithName_DirectoryNonEmptyFileNameDoesNotExist_ReturnNull()
        {
            var result = DirectoryManager.DeleteAllFilesWithName(existingDirectory, "someFileName");

            Assert.That(result, Is.Null);
        }

        [Test, Category("DeleteFiles")]
        public void DeleteAllFilesWithName_DirectoryNonEmptyFileNameAllBuild_ReturnListOfDeletedAllBuildFiles()
        {
            var result = DirectoryManager.DeleteAllFilesWithName(existingDirectory, "ALL_BUILD");

            Assert.True(filesAllBuild.SequenceEqual(result) &&
                        Directory.GetFiles(existingDirectory).Where(s => Path.GetFileName(s).Equals("ALL_BUILD.vcxproj")).Count() == 0 &&
                        Directory.GetFiles(existingDirectory).Where(s => Path.GetFileName(s).Equals("ALL_BUILD.vcxproj.filters")).Count() == 0);
        }

        [Test, Category("DeleteFiles")]
        public void DeleteAllFilesWithName_DirectoryNonEmptyFileNameZeroCheck_ReturnListOfDeletedZeroCheckFiles()
        {
            var result = DirectoryManager.DeleteAllFilesWithName(existingDirectory, "ZERO_CHECK");

            Assert.True(filesZeroCheck.SequenceEqual(result) &&
                        Directory.GetFiles(existingDirectory).Where(s => Path.GetFileName(s).Equals("ZERO_CHECK.vcxproj")).Count() == 0 &&
                        Directory.GetFiles(existingDirectory).Where(s => Path.GetFileName(s).Equals("ZERO_CHECK.vcxproj.filters")).Count() == 0);
        }

        [Test, Category("Files")]
        public void GetAllFilesWithExtension_DirectoryDoesNotExistExtensionNull_ThrowArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => DirectoryManager.GetAllFilesWithExtension(nonExistingDirectory, null));
        }

        [Test, Category("Files")]
        public void GetAllFilesWithExtension_DirectoryDoesNotExistExtensionEmpty_ReturnNull()
        {
            var result = DirectoryManager.GetAllFilesWithExtension(nonExistingDirectory, "");

            Assert.That(result, Is.Null);
        }

        [Test, Category("Files")]
        public void GetAllFilesWithExtension_DirectoryDoesNotExistExtensionSet_ReturnNull()
        {
            var result = DirectoryManager.GetAllFilesWithExtension(nonExistingDirectory, ".sln");

            Assert.That(result, Is.Null);
        }

        [Test, Category("Files")]
        public void GetAllFilesWithExtension_DirectoryExistsExtensionNull_ThrowArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => DirectoryManager.GetAllFilesWithExtension(existingDirectory, null));
        }

        [Test, Category("Files")]
        public void GetAllFilesWithExtension_DirectoryExistsExtensionEmpty_ThrowArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => DirectoryManager.GetAllFilesWithExtension(existingDirectory, null));
        }

        [Test, Category("Files")]
        public void GetAllFilesWithExtension_DirectoryExistsExtensionDoesNotExist_ReturnNull()
        {
            var result = DirectoryManager.GetAllFilesWithExtension(existingDirectory, ".nonExistingExtension");

            Assert.That(result, Is.Empty);
        }

        [Test, Category("Files")]
        public void GetAllFilesWithExtension_DirectoryExistsExtensionExists_Return()
        {
            var result = DirectoryManager.GetAllFilesWithExtension(existingDirectory, ".sln");

            Assert.That(filesSolution.SequenceEqual(result), Is.True);
        }
    }
}
