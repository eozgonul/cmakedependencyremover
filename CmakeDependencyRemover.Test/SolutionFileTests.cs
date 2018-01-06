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
	class SolutionFileTests
	{
		string UIDInfoAllBuild;
		string projectInfoAllBuild;
		string solutionFileContent;
		List<string> solutionConfigurations;

		[SetUp]
		public void SetUp()
		{
			UIDInfoAllBuild = "Project(\"{8BC9CEB8-8B4A-11D0-8D11-00A0C91BC942}\") = \"ALL_BUILD\", \"ALL_BUILD.vcxproj\", \"{8BAE2EF6-B67E-3634-9B27-B99536808058}\"\n" +
							  "     ProjectSection(ProjectDependencies) = postProject\n" +
							  "         {07CB081B-DCB7-31FC-8BF9-0CD05B752220} = {07CB081B-DCB7-31FC-8BF9-0CD05B752220}\n" +
							  "         {B4AEEABC-FA41-3892-8644-533F537A0ED7} = {B4AEEABC-FA41-3892-8644-533F537A0ED7}\n" +
							  "         {B28D5454-5C92-348F-BB2D-BB0B9FAE4192} = {B28D5454-5C92-348F-BB2D-BB0B9FAE4192}\n" +
							  "     EndProjectSection\n" +
							  "EndProject";

			projectInfoAllBuild = "Microsoft Visual Studio Solution File, Format Version 12.00\n" +
								  "# Visual Studio 15\n" +
								  "Project(\"{8BC9CEB8-8B4A-11D0-8D11-00A0C91BC942}\") = \"ALL_BUILD\", \"ALL_BUILD.vcxproj\", \"{8BAE2EF6-B67E-3634-9B27-B99536808058}\"\n" +
								  "     ProjectSection(ProjectDependencies) = postProject\n" +
								  "         {07CB081B-DCB7-31FC-8BF9-0CD05B752220} = {07CB081B-DCB7-31FC-8BF9-0CD05B752220}\n" +
								  "         {B4AEEABC-FA41-3892-8644-533F537A0ED7} = {B4AEEABC-FA41-3892-8644-533F537A0ED7}\n" +
								  "         {B28D5454-5C92-348F-BB2D-BB0B9FAE4192} = {B28D5454-5C92-348F-BB2D-BB0B9FAE4192}\n" +
								  "     EndProjectSection\n" +
								  "EndProject\n" +
								  "Project(\"{8BC9CEB8-8B4A-11D0-8D11-00A0C91BC942}\") = \"BoggleLibrary\", \"boggle_library\\BoggleLibrary.vcxproj\", \"{07CB081B-DCB7-31FC-8BF9-0CD05B752220}\"\n" +
								  "     ProjectSection(ProjectDependencies) = postProject\n" +
								  "         {B28D5454-5C92-348F-BB2D-BB0B9FAE4192} = {B28D5454-5C92-348F-BB2D-BB0B9FAE4192}\n" +
								  "     EndProjectSection\n" +
								  "EndProject\n" +
								  "Project(\"{8BC9CEB8-8B4A-11D0-8D11-00A0C91BC942}\") = \"BoggleTest\", \"boggle_test\\BoggleTest.vcxproj\", \"{B4AEEABC-FA41-3892-8644-533F537A0ED7}\"\n" +
								  "     ProjectSection(ProjectDependencies) = postProject\n" + 
								  "         {07CB081B-DCB7-31FC-8BF9-0CD05B752220} = {07CB081B-DCB7-31FC-8BF9-0CD05B752220}\n" +
								  "         {B28D5454-5C92-348F-BB2D-BB0B9FAE4192} = {B28D5454-5C92-348F-BB2D-BB0B9FAE4192}\n" +
								  "     EndProjectSection\n" +
								  "EndProject\n" +
								  "Project(\"{8BC9CEB8-8B4A-11D0-8D11-00A0C91BC942}\") = \"ZERO_CHECK\", \"ZERO_CHECK.vcxproj\", \"{B28D5454-5C92-348F-BB2D-BB0B9FAE4192}\"\n" +
								  "     ProjectSection(ProjectDependencies) = postProject\n" +
								  "     EndProjectSection\n" +
								  "EndProject\n";


			solutionFileContent =   "Microsoft Visual Studio Solution File, Format Version 12.00\n" +
									"# Visual Studio 15\n" +
									"Project(\"{8BC9CEB8-8B4A-11D0-8D11-00A0C91BC942}\") = \"ALL_BUILD\", \"ALL_BUILD.vcxproj\", \"{8BAE2EF6-B67E-3634-9B27-B99536808058}\"\n" +
									"   ProjectSection(ProjectDependencies) = postProject\n" +
									"		{07CB081B-DCB7-31FC-8BF9-0CD05B752220} = {07CB081B-DCB7-31FC-8BF9-0CD05B752220}\n" +
									"		{B4AEEABC-FA41-3892-8644-533F537A0ED7} = {B4AEEABC-FA41-3892-8644-533F537A0ED7}\n" +
									"		{B28D5454-5C92-348F-BB2D-BB0B9FAE4192} = {B28D5454-5C92-348F-BB2D-BB0B9FAE4192}\n" +
									"	EndProjectSection\n" +
									"EndProject\n" +
									"Project(\"{8BC9CEB8-8B4A-11D0-8D11-00A0C91BC942}\") = \"BoggleLibrary\", \"boggle_library\\BoggleLibrary.vcxproj\", \"{07CB081B-DCB7-31FC-8BF9-0CD05B752220}\"\n" +
									"	ProjectSection(ProjectDependencies) = postProject\n" +
									"		{B28D5454-5C92-348F-BB2D-BB0B9FAE4192} = {B28D5454-5C92-348F-BB2D-BB0B9FAE4192}\n" +
									"	EndProjectSection\n" +
									"EndProject\n" +
									"Project(\"{8BC9CEB8-8B4A-11D0-8D11-00A0C91BC942}\") = \"BoggleTest\", \"boggle_test\\BoggleTest.vcxproj\", \"{B4AEEABC-FA41-3892-8644-533F537A0ED7}\"\n" +
									"	ProjectSection(ProjectDependencies) = postProject\n" +
									"		{07CB081B-DCB7-31FC-8BF9-0CD05B752220} = {07CB081B-DCB7-31FC-8BF9-0CD05B752220}\n" +
									"		{B28D5454-5C92-348F-BB2D-BB0B9FAE4192} = {B28D5454-5C92-348F-BB2D-BB0B9FAE4192}\n" +
									"	EndProjectSection\n" +
									"EndProject\n" +
									"Project(\"{8BC9CEB8-8B4A-11D0-8D11-00A0C91BC942}\") = \"ZERO_CHECK\", \"ZERO_CHECK.vcxproj\", \"{B28D5454-5C92-348F-BB2D-BB0B9FAE4192}\"\n" +
									"	ProjectSection(ProjectDependencies) = postProject\n" +
									"	EndProjectSection\n" +
									"EndProject\n" +
									"Global\n" +
									"   GlobalSection(SolutionConfigurationPlatforms) = preSolution\n" +
									"       Debug|x64 = Debug|x64\n" +
									"       Release|x64 = Release|x64\n" +
									"   EndGlobalSection\n" +
									"   GlobalSection(ProjectConfigurationPlatforms) = postSolution\n" +
									"       {8BAE2EF6-B67E-3634-9B27-B99536808058}.Debug|x64.ActiveCfg = Debug|x64\n" +
									"       {8BAE2EF6-B67E-3634-9B27-B99536808058}.Release|x64.ActiveCfg = Release|x64\n" +
									"       {07CB081B-DCB7-31FC-8BF9-0CD05B752220}.Debug|x64.ActiveCfg = Debug|x64\n" +
									"       {07CB081B-DCB7-31FC-8BF9-0CD05B752220}.Debug|x64.Build.0 = Debug|x64\n" +
									"       {07CB081B-DCB7-31FC-8BF9-0CD05B752220}.Release|x64.ActiveCfg = Release|x64\n" +
									"       {07CB081B-DCB7-31FC-8BF9-0CD05B752220}.Release|x64.Build.0 = Release|x64\n" +
									"       {B4AEEABC-FA41-3892-8644-533F537A0ED7}.Debug|x64.ActiveCfg = Debug|x64\n" +
									"       {B4AEEABC-FA41-3892-8644-533F537A0ED7}.Debug|x64.Build.0 = Debug|x64\n" +
									"       {B4AEEABC-FA41-3892-8644-533F537A0ED7}.Release|x64.ActiveCfg = Release|x64\n" +
									"       {B4AEEABC-FA41-3892-8644-533F537A0ED7}.Release|x64.Build.0 = Release|x64\n" +
									"       {B28D5454-5C92-348F-BB2D-BB0B9FAE4192}.Debug|x64.ActiveCfg = Debug|x64\n" +
									"       {B28D5454-5C92-348F-BB2D-BB0B9FAE4192}.Debug|x64.Build.0 = Debug|x64\n" +
									"       {B28D5454-5C92-348F-BB2D-BB0B9FAE4192}.Release|x64.ActiveCfg = Release|x64\n" +
									"       {B28D5454-5C92-348F-BB2D-BB0B9FAE4192}.Release|x64.Build.0 = Release|x64\n" +
									"	EndGlobalSection\n" +
									"	GlobalSection(ExtensibilityGlobals) = postSolution\n" +
									"	EndGlobalSection\n" +
									"	GlobalSection(ExtensibilityAddIns) = postSolution\n" +
									"	EndGlobalSection\n" +
									"EndGlobal\n";

			solutionConfigurations = new List<string> { "Debug|x64", "Release|x64" };
		}

		[Category("UID")]
		[TestCase(null, null)]
		[TestCase("", null)]
		[TestCase(null, "")]
		[TestCase("someStringWhichContainsNoUID", null)]
		[TestCase(null, "someProjectName")]
		public void GetProjectUID_ParametersNull_ThrowArgumentNullException(string fileContent, string projectName)
		{
			Assert.Throws<ArgumentNullException>(() => SolutionFileManager.GetProjectUID(fileContent, projectName));
		}

		[Category("UID")]
		[TestCase("", "")]
		[TestCase("someStringWhichContainsNoUID", "")]
		[TestCase("", "someProjectName")]
		public void GetProjectUID_ParametersEmpty_ReturnNull(string fileContent, string projectName)
		{
			var result = SolutionFileManager.GetProjectUID("someStringWhichContainsNoUID", "someProjectName");
			Assert.That(result, Is.Null);
		}

		[Test, Category("UID")]
		public void GetProjectUID_ParametersDoesNotExist_ReturnNull()
		{
			var result = SolutionFileManager.GetProjectUID("someStringWhichContainsNoUID", "someProjectName");
			Assert.That(result, Is.Null);
		}

		[Test, Category("UID")]
		public void GetProjectUID_FileContentValidProjectNameValid_ReturnValidUID()
		{
			var result = SolutionFileManager.GetProjectUID(UIDInfoAllBuild, "ALL_BUILD");
			Assert.AreEqual(result, "8BAE2EF6-B67E-3634-9B27-B99536808058");
		}

		[Test, Category("UID")]
		public void GetProjectUID_FileContentValidProjectNameInvalid_ReturnNull()
		{
			var result = SolutionFileManager.GetProjectUID(UIDInfoAllBuild, "ALL_BUIL");
			Assert.That(result, Is.Null);
		}

		[Category("ProjectInfo")]
		[TestCase(null, null)]
		[TestCase("", null)]
		[TestCase(null, "")]
		[TestCase("someStringWhichContainsNoProjectInfo", null)]
		[TestCase(null, "someProjectName")]
		public void GetProjectInfo_ParametersNull_ThrowArgumentNullException(string fileContent, string projectName)
		{
			Assert.Throws<ArgumentNullException>(() => SolutionFileManager.GetProjectInfo(fileContent, projectName));
		}

		[Category("ProjectInfo")]
		[TestCase("", "", null)]
		[TestCase("someStringWhichContainsNoProjectInfo", "", null)]
		[TestCase("", "someProjectName", null)]
		[TestCase("someStringWhichContainsNoProjectInfo", "someProjectName", null)]
		public void GetProjectInfo_ParametersEmptyOrDoesNotExist_ReturnNull(string fileContent, string projectName, string expected)
		{
			var result = SolutionFileManager.GetProjectInfo(fileContent, projectName);
			Assert.AreEqual(result, expected);
		}

		[Test, Category("ProjectInfo")]
		public void GetProjectInfo_FileContentValidProjectNameValid_ReturnValidProjectInfo()
		{
			var result = SolutionFileManager.GetProjectInfo(projectInfoAllBuild, "ALL_BUILD");
			Assert.AreEqual(result, UIDInfoAllBuild);
		}

		[Test, Category("ProjectInfo")]
		public void GetProjectInfo_FileContentValidProjectNameDoesNotExist_ReturnNull()
		{
			var result = SolutionFileManager.GetProjectInfo(projectInfoAllBuild, "ALL_BUIL");
			Assert.That(result, Is.Null);
		}

		[Category("RemoveProjectInfo")]
		[TestCase(null, null)]
		[TestCase(null, "")]
		[TestCase(null, "someProjectName")]
		[TestCase("", null)]
		[TestCase("someFileContentWithNoValidProjectInfo", null)]
		public void RemoveProjectInfoFromSolutionFile_ParametersNull_ThrowArgumentNullException(string fileContent, string fileName)
		{
			Assert.Throws<ArgumentNullException>(() => SolutionFileManager.RemoveProjectInfoFromSolutionFile(fileContent, fileName));
		}

		[Test, Category("RemoveProjectInfo")]
		public void RemoveProjectInfoFromSolutionFile_FileContentEmptyProjectNameInvalid_ReturnFalse()
		{
			var result = SolutionFileManager.RemoveProjectInfoFromSolutionFile("", "someProjectName");
			Assert.That(result, Is.False);
		}

		[Test, Category("RemoveProjectInfo")]
		public void RemoveProjectInfoFromSolutionFile_FileContentEmptyProjectNameValid_ReturnFalse()
		{
			var result = SolutionFileManager.RemoveProjectInfoFromSolutionFile("", "ALL_BUILD");
			Assert.That(result, Is.False);
		}

		[Test, Category("RemoveProjectInfo")]
		public void RemoveProjectInfoFromSolutionFile_FileContentValidProjectNameNull_ThrowArgumentNullException()
		{
			Assert.Throws<ArgumentNullException>(() => SolutionFileManager.RemoveProjectInfoFromSolutionFile(solutionFileContent, null));
		}

		[Test, Category("RemoveProjectInfo")]
		public void RemoveProjectInfoFromSolutionFile_FileContentValidProjectNameEmpty_ThrowArgumentNullException()
		{
			var result = SolutionFileManager.RemoveProjectInfoFromSolutionFile(solutionFileContent, "");
			Assert.That(result, Is.False);
		}

		[Test, Category("RemoveProjectInfo")]
		public void RemoveProjectInfoFromSolutionFile_FileContentValidProjectNameInvalid_ThrowArgumentNullException()
		{
			var result = SolutionFileManager.RemoveProjectInfoFromSolutionFile(solutionFileContent, "someProjectName");
			Assert.That(result, Is.False);
		}

		[Test, Category("RemoveProjectInfo")]
		public void RemoveProjectInfoFromSolutionFile_FileContentValidProjectNameValid_ReturnTrue()
		{
			var result = SolutionFileManager.RemoveProjectInfoFromSolutionFile(solutionFileContent, "ALL_BUILD");
			Assert.That(result, Is.True);
		}

		[Test, Category("GetSolutionConfigurations")]
		public void GetSolutionConfigurations_ParameterNull_ThrowArgumentNullException()
		{
			Assert.Throws<ArgumentNullException>(() => SolutionFileManager.GetSolutionConfigurations(null));
		}

		[Category("GetSolutionConfigurations")]
		[TestCase("")]
		[TestCase("someFileContentWithNoSolutionInformation")]
		public void GetSolutionConfigurations_ParametersEmptyOrInvalid_ReturnNull(string fileContent)
		{
			var result = SolutionFileManager.GetSolutionConfigurations(fileContent);
			Assert.That(result, Is.Null);
		}

		[Test, Category("GetSolutionConfigurations")]
		public void GetSolutionConfigurations_FileContentValid_ReturnDebugRelease()
		{
			var result = SolutionFileManager.GetSolutionConfigurations(solutionFileContent);
			Assert.That(solutionConfigurations.SequenceEqual(result));
		}

		[Category("RemoveProjectUIDFromConfigurationPlatforms")]
		[TestCase(null, null)]
		[TestCase(null, "")]
		[TestCase("", null)]
		public void RemoveProjectUIDFromProjectConfigurationPlatforms_ParametersNull_ThrowArgumentNullException(string fileContent, string uid)
		{
			Assert.Throws<ArgumentNullException>(() => SolutionFileManager.RemoveProjectInfoFromSolutionFile(fileContent, uid));
		}

		[Test, Category("RemoveProjectUIDFromConfigurationPlatforms")]
		public void RemoveProjectUIDFromProjectConfigurationPlatforms_FileContentEmptyUIDEmpty_ReturnNull()
		{
			var result = SolutionFileManager.RemoveProjectUIDFromProjectConfigurationPlatforms("", "");
			Assert.That(result, Is.Null);
		}

		[Test, Category("RemoveProjectUIDFromConfigurationPlatforms")]
		public void RemoveProjectUIDFromProjectConfigurationPlatforms_FileContentValidUIDEmpty_ReturnNull()
		{
			var result = SolutionFileManager.RemoveProjectUIDFromProjectConfigurationPlatforms(solutionFileContent, "");
			Assert.AreEqual(result, solutionFileContent);
		}

		[Test, Category("RemoveProjectUIDFromConfigurationPlatforms")]
		public void RemoveProjectUIDFromProjectConfigurationPlatforms_FileContentValidUIDInvalid_ReturnNull()
		{
			var result = SolutionFileManager.RemoveProjectUIDFromProjectConfigurationPlatforms(solutionFileContent, "someInvalidUID");
			Assert.AreEqual(result, solutionFileContent);
		}

		[Test, Category("RemoveProjectUIDFromConfigurationPlatforms")]
		public void RemoveProjectUIDFromProjectConfigurationPlatforms_FileContentValidUIDValid_UIDRemovedFromFileContent()
		{
			var result = SolutionFileManager.RemoveProjectUIDFromProjectConfigurationPlatforms(solutionFileContent, "8BAE2EF6-B67E-3634-9B27-B99536808058");
			Assert.That(result.Contains(UIDInfoAllBuild), Is.False);
		}

		[Category("ChangeSolutionDirectoryToMacro")]
		[TestCase(null, null)]
		[TestCase(null, "")]
		[TestCase("", null)]
		public void ChangeHardCodedProjectDirectoryToMacro_ParametersNull_ThrowArgumentNullException(string fileContent, string solutionDirectory)
		{
			Assert.Throws<ArgumentNullException>(() => ProjectFileManager.ChangeHardCodedProjectDirectoryToMacro(fileContent, solutionDirectory));
		}
	}
}
