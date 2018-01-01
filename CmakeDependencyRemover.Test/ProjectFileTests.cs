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

        [SetUp]
        public void SetUp()
        {
            existingDirectory = Path.GetDirectoryName(Path.GetDirectoryName(TestContext.CurrentContext.TestDirectory)) + @"\resource\TestFiles";
            projectFileContent = File.ReadAllText(existingDirectory + @"\SomeInnerDirectory\BoggleTest.vcxproj");

            zeroCheckProjectReferenceSection = "<ProjectReference Include=\"C:\\Users\\eozgonul\\Documents\\Visual Studio 2017\\Projects\\CmakeDependencyRemover\\CmakeDependencyRemover.Test\\resource\\BoggleTest\\ZERO_CHECK.vcxproj\">\r\n      <Project>{B28D5454-5C92-348F-BB2D-BB0B9FAE4192}</Project>\r\n      <Name>ZERO_CHECK</Name>\r\n    </ProjectReference>";

            zeroCheckProjectReferences = new List<string> { zeroCheckProjectReferenceSection };
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
        public void RemoveProjectReference_ProjectNameNull_ThrowArgumentNullException(string fileContent, string projectName)
        {
            Assert.Throws<ArgumentNullException>(() => ProjectFileManager.RemoveProjectReference(fileContent, projectName));
        }
    }
}
