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
    class ProjectFileTests
    {
        string projectFileContent;
        string existingDirectory;
        string zeroCheckProjectReferenceSection;
        List<string> zeroCheckProjectReferences;

        List<string> cMakeCustomBuildEventSections;
        string cMakeCustomBuildEventSection;

        [SetUp]
        public void SetUp()
        {
            existingDirectory = Path.GetDirectoryName(Path.GetDirectoryName(TestContext.CurrentContext.TestDirectory)) + @"\resource\TestFiles";
            projectFileContent = File.ReadAllText(existingDirectory + @"\boggle_test\BoggleTest.vcxproj");

            zeroCheckProjectReferenceSection = "<ProjectReference Include=\"C:\\Users\\eozgonul\\Documents\\Visual Studio 2017\\Projects\\CmakeDependencyRemover\\CmakeDependencyRemover.Test\\resource\\BoggleTest\\ZERO_CHECK.vcxproj\">\r\n      <Project>{B28D5454-5C92-348F-BB2D-BB0B9FAE4192}</Project>\r\n      <Name>ZERO_CHECK</Name>\r\n    </ProjectReference>";
            zeroCheckProjectReferences = new List<string> { zeroCheckProjectReferenceSection };

            cMakeCustomBuildEventSection = "\r\n  <ItemGroup>\r\n    <CustomBuild Include=\"C:\\Users\\eozgonul\\Documents\\Visual Studio 2017\\Projects\\CmakeDependencyRemover\\CmakeDependencyRemover.Test\\resource\\TestFiles\\boggle_test\\cmake\\CMakeLists.txt\">\r\n      <Message Condition=\"'$(Configuration)|$(Platform)'=='Debug|x64'\">Building Custom Rule C:/Users/eozgonul/Documents/Visual Studio 2017/Projects/CmakeDependencyRemover/CmakeDependencyRemover.Test/resource/TestFiles/boggle_test/cmake/CMakeLists.txt</Message>\r\n      <Command Condition=\"'$(Configuration)|$(Platform)'=='Debug|x64'\">setlocal\r\n\"C:\\Program Files\\CMake\\bin\\cmake.exe\" \"-HC:/Users/eozgonul/Documents/Visual Studio 2017/Projects/CmakeDependencyRemover/CmakeDependencyRemover.Test/resource/TestFiles/cmake\" \"-BC:/Users/eozgonul/Documents/Visual Studio 2017/Projects/CmakeDependencyRemover/CmakeDependencyRemover.Test/resource/TestFiles\" --check-stamp-file \"C:/Users/eozgonul/Documents/Visual Studio 2017/Projects/CmakeDependencyRemover/CmakeDependencyRemover.Test/resource/TestFiles/boggle_test/CMakeFiles/generate.stamp\"\r\nif %errorlevel% neq 0 goto :cmEnd\r\n:cmEnd\r\nendlocal &amp; call :cmErrorLevel %errorlevel% &amp; goto :cmDone\r\n:cmErrorLevel\r\nexit /b %1\r\n:cmDone\r\nif %errorlevel% neq 0 goto :VCEnd</Command>\r\n      <AdditionalInputs Condition=\"'$(Configuration)|$(Platform)'=='Debug|x64'\">C:/Users/eozgonul/Documents/Visual Studio 2017/Projects/CmakeDependencyRemover/CmakeDependencyRemover.Test/resource/TestFiles/boggle_test/cmake/CMakeLists.txt;C:\\Users\\eozgonul\\Documents\\Visual Studio 2017\\Projects\\CmakeDependencyRemover\\CmakeDependencyRemover.Test\\resource\\TestFiles\\boggle_test\\cmake\\CMakeLists.txt;C:\\Users\\eozgonul\\Documents\\Visual Studio 2017\\Projects\\CmakeDependencyRemover\\CmakeDependencyRemover.Test\\resource\\TestFiles\\boggle_test\\cmake\\CMakeLists.txt;%(AdditionalInputs)</AdditionalInputs>\r\n      <Outputs Condition=\"'$(Configuration)|$(Platform)'=='Debug|x64'\">C:\\Users\\eozgonul\\Documents\\Visual Studio 2017\\Projects\\CmakeDependencyRemover\\CmakeDependencyRemover.Test\\resource\\TestFiles\\boggle_test\\CMakeFiles\\generate.stamp</Outputs>\r\n      <LinkObjects Condition=\"'$(Configuration)|$(Platform)'=='Debug|x64'\">false</LinkObjects>\r\n      <Message Condition=\"'$(Configuration)|$(Platform)'=='Release|x64'\">Building Custom Rule C:/Users/eozgonul/Documents/Visual Studio 2017/Projects/CmakeDependencyRemover/CmakeDependencyRemover.Test/resource/TestFiles/boggle_test/cmake/CMakeLists.txt</Message>\r\n      <Command Condition=\"'$(Configuration)|$(Platform)'=='Release|x64'\">setlocal\r\n\"C:\\Program Files\\CMake\\bin\\cmake.exe\" \"-HC:/Users/eozgonul/Documents/Visual Studio 2017/Projects/CmakeDependencyRemover/CmakeDependencyRemover.Test/resource/TestFiles/cmake\" \"-BC:/Users/eozgonul/Documents/Visual Studio 2017/Projects/CmakeDependencyRemover/CmakeDependencyRemover.Test/resource/TestFiles\" --check-stamp-file \"C:/Users/eozgonul/Documents/Visual Studio 2017/Projects/CmakeDependencyRemover/CmakeDependencyRemover.Test/resource/TestFiles/boggle_test/CMakeFiles/generate.stamp\"\r\nif %errorlevel% neq 0 goto :cmEnd\r\n:cmEnd\r\nendlocal &amp; call :cmErrorLevel %errorlevel% &amp; goto :cmDone\r\n:cmErrorLevel\r\nexit /b %1\r\n:cmDone\r\nif %errorlevel% neq 0 goto :VCEnd</Command>\r\n      <AdditionalInputs Condition=\"'$(Configuration)|$(Platform)'=='Release|x64'\">C:/Users/eozgonul/Documents/Visual Studio 2017/Projects/CmakeDependencyRemover/CmakeDependencyRemover.Test/resource/TestFiles/boggle_test/cmake/CMakeLists.txt;C:\\Users\\eozgonul\\Documents\\Visual Studio 2017\\Projects\\CmakeDependencyRemover\\CmakeDependencyRemover.Test\\resource\\TestFiles\\boggle_test\\cmake\\CMakeLists.txt;C:\\Users\\eozgonul\\Documents\\Visual Studio 2017\\Projects\\CmakeDependencyRemover\\CmakeDependencyRemover.Test\\resource\\TestFiles\\boggle_test\\cmake\\CMakeLists.txt;%(AdditionalInputs)</AdditionalInputs>\r\n      <Outputs Condition=\"'$(Configuration)|$(Platform)'=='Release|x64'\">C:\\Users\\eozgonul\\Documents\\Visual Studio 2017\\Projects\\CmakeDependencyRemover\\CmakeDependencyRemover.Test\\resource\\TestFiles\\boggle_test\\CMakeFiles\\generate.stamp</Outputs>\r\n      <LinkObjects Condition=\"'$(Configuration)|$(Platform)'=='Release|x64'\">false</LinkObjects>\r\n    </CustomBuild>\r\n  </ItemGroup>";
            cMakeCustomBuildEventSections = new List<string> { cMakeCustomBuildEventSection };
        }

        [Category("GetProjectReferenceSection")]
        [TestCase(null, null)]
        [TestCase(null, "")]
        [TestCase("", null)]
        public void GetProjectReferenceSection_ParametersNull_ThrowArgumentNullException(string fileContent, string projectName)
        {
            Assert.Throws<ArgumentNullException>(() => ProjectFileManager.GetProjectReferenceSection(fileContent, projectName));
        }

        [Category("GetProjectReferenceSection")]
        [TestCase("", "")]
        [TestCase("someInvalidFileContent", "")]
        [TestCase("", "someInvalidProjectName")]
        [TestCase("someInvalidFileContent", "someInvalidProjectName")]
        public void GetProjectReferenceSection_ParametersEmptyOrInvalid_ReturnNull(string fileContent, string projectName)
        {
            var result = ProjectFileManager.GetProjectReferenceSection(fileContent, projectName);
            Assert.That(result, Is.Null);
        }

        [Test, Category("GetProjectReferenceSection")]
        public void GetProjectReferenceSection_FileContentValidProjectNameInvalid_ReturnNull()
        {
            var result = ProjectFileManager.GetProjectReferenceSection(projectFileContent, "someInvalidProjectName");
            Assert.That(result, Is.Null);
        }

        [Test, Category("GetProjectReferenceSection")]
        public void GetProjectReferenceSection_FileContentValidProjectNameValid_ReturnZeroCheckSection()
        {
            var result = ProjectFileManager.GetProjectReferenceSection(projectFileContent, "ZERO_CHECK");
            Assert.That(zeroCheckProjectReferences.SequenceEqual(result), Is.True);
        }

        [Category("RemoveProjectReference")]
        [TestCase(null, null)]
        [TestCase(null, "")]
        [TestCase("", null)]
        public void RemoveProjectReference_ParametersNull_ThrowArgumentNullException(string fileContent, string projectName)
        {
            Assert.Throws<ArgumentNullException>(() => ProjectFileManager.RemoveProjectReference(fileContent, projectName));
        }

        [Category("RemoveProjectReference")]
        [TestCase("", "")]
        [TestCase("someInvalidFileContent", "")]
        [TestCase("", "someProjectName")]
        [TestCase("someInvalidFileContent", "someProjectName")]
        public void RemoveProjectReference_FileContentEmptyProjectNameEmpty_ReturnNull(string fileContent, string projectName)
        {
            var result = ProjectFileManager.RemoveProjectReference(fileContent, projectName);
            Assert.That(result, Is.Null);
        }

        [Test, Category("RemoveProjectReference")]
        public void RemoveProjectReference_FileContentValidProjectNameInvalid_ReturnNull()
        {
            var result = ProjectFileManager.RemoveProjectReference(projectFileContent, "ZERO_CHEC");
            Assert.That(result, Is.Null);
        }

        [Test, Category("RemoveProjectReference")]
        public void RemoveProjectReference_FileContentValidProjectNameValid_ProjectReferenceRemoved()
        {
            var result = ProjectFileManager.RemoveProjectReference(projectFileContent, "ZERO_CHECK");
            Assert.That(result.Contains(zeroCheckProjectReferenceSection), Is.False);
        }

        [Test, Category("GetItemGroupSections")]
        public void GetCMakeCustomBuildEventSections_FileContentNull_ThrowArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => ProjectFileManager.GetCMakeCustomBuildEventSections(null));
        }

        [Category("GetItemGroupSections")]
        [TestCase("")]
        [TestCase("invalidFileContent")]
        public void GetCMakeCustomBuildEventSections_FileContentInvalid_ReturnNull(string fileContent)
        {
            var result = ProjectFileManager.GetCMakeCustomBuildEventSections(fileContent);
            Assert.That(result, Is.Null);
        }

        [Test, Category("GetItemGroupSections")]
        public void GetCMakeCustomBuildEventSections_FileContentValid_ReturnValidItemGroups()
        {
            var result = ProjectFileManager.GetCMakeCustomBuildEventSections(projectFileContent);
            Assert.That(result.SequenceEqual(cMakeCustomBuildEventSections));
        }

        [Test, Category("RemoveCustomCMakeBuildEvents")]
        public void RemoveCMakeCustomBuildEvents_ParameterNullOrEmpty_ThrowArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => ProjectFileManager.RemoveCMakeCustomBuildEvents(null));
        }

        [Category("RemoveCustomCMakeBuildEvents")]
        [TestCase("")]
        [TestCase("invalidFileContent")]
        public void RemoveCMakeCustomBuildEvents_FileContentEmpty_ReturnNull(string fileContent)
        {
            var result = ProjectFileManager.RemoveCMakeCustomBuildEvents(fileContent);
            Assert.That(result, Is.Null);
        }

        [Test, Category("RemoveCustomCMakeBuildEvents")]
        public void RemoveCMakeBuildEvents_FileContentValid_ReturnCustomBuildEventRemoved()
        {
            var result = ProjectFileManager.RemoveCMakeCustomBuildEvents(projectFileContent);
            Assert.That(result.Contains(cMakeCustomBuildEventSection), Is.False);
        }

        [Category("ChangeHardCodedProjectDirectoryToMacro")]
        [TestCase(null, null)]
        [TestCase(null, "")]
        [TestCase("", null)]
        public void ChangeHardCodedProjectDirectoryToMacro_ParametersNullOrEmpty_ThrowArgumentNullException(string fileContent, string solutionDirectory)
        {
            Assert.Throws<ArgumentNullException>(() => ProjectFileManager.ChangeHardCodedProjectDirectoryToMacro(fileContent, solutionDirectory));
        }

        [Category("ChangeHardCodedProjectDirectoryToMacro")]
        [TestCase("", "")]
        [TestCase("", @"C:\some\invalid\directory")]
        [TestCase("someInvalidFileContent", "")]
        [TestCase("someInvalidFileContent", @"C:\some\invalid\directory")]
        public void ChangeHardCodedProjectDirectoryToMacro_ParametersEmptyOrInvalid_ReturnNull(string fileContent, string projectDirectory)
        {
            var result = ProjectFileManager.ChangeHardCodedProjectDirectoryToMacro(fileContent, projectDirectory);
            Assert.That(result, Is.Null);
        }

        [Test, Category("ChangeHardCodedProjectDirectoryToMacro")]
        public void ChangeHardCodedProjectDirectoryToMacro_FileContentValidProjectDirectoryInvalid_ReturnNull()
        {
            var result = ProjectFileManager.ChangeHardCodedProjectDirectoryToMacro(projectFileContent, @"C:\some\invalid\directory");
            Assert.That(result, Is.Null);
        }

        [Test, Category("ChangeHardCodedProjectDirectoryToMacro")]
        public void ChangeHardCodedProjectDirectoryToMacro_FileContentValidProjectDirectoryValid()
        {
            var result = ProjectFileManager.ChangeHardCodedProjectDirectoryToMacro(projectFileContent, existingDirectory + @"\boggle_test");
            Assert.That(result.Contains(existingDirectory + @"\boggle_test"), Is.False);
        }
    }
}
