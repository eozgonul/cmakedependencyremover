using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.WindowsAPICodePack.Dialogs;

using System.IO;

namespace CmakeDependencyRemover.UI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            InitializeSubComponents();
        }

        private void InitializeSubComponents()
        {
            mi_LoadSolution.AddHandler(MenuItem.ClickEvent, new RoutedEventHandler(OnLoadSolutionClicked));
            mi_Exit.AddHandler(MenuItem.ClickEvent, new RoutedEventHandler(OnExitClicked));
        }

        private void OnLoadSolutionClicked(object sender, RoutedEventArgs routedEventArgs)
        {
            var dialog = new CommonOpenFileDialog { Title = "SelectSolutionDirectory",
                                                    IsFolderPicker = true,
                                                    Multiselect = false,
                                                    EnsurePathExists = true,
                                                    EnsureValidNames = true,
                                                    ShowPlacesList = true };

            if(dialog.ShowDialog() == CommonFileDialogResult.Ok)
            {
                LoadSolutionDirectory(dialog.FileName);
                SolutionDirectory = dialog.FileName;
            }
        }

        private void OnExitClicked(object sender, RoutedEventArgs routedEventArgs)
        {
            Close();
        }

        private void LoadSolutionDirectory(string selectedDirectory)
        {
            ClearSolutionFiles();
            FillTreeViewWithSolutionDirectoryData(selectedDirectory);
        }

        private void ClearSolutionFiles()
        {
            tv_SolutionFiles.Items.Clear();
        }

        private void FillTreeViewWithSolutionDirectoryData(string directoryPath)
        {
            var stack = new Stack<TreeViewItem>();
            var rootDirectory = new DirectoryInfo(directoryPath);
            var node = new TreeViewItem{ Header = rootDirectory.Name, Tag = rootDirectory };
            stack.Push(node);

            while (stack.Count > 0)
            {
                var currentNode = stack.Pop();
                var directoryInfo = (DirectoryInfo)currentNode.Tag;
                foreach (var directory in directoryInfo.GetDirectories())
                {
                    var childDirectoryNode = new TreeViewItem { Header = directory.Name, Tag = directory };
                    currentNode.Items.Add(childDirectoryNode);
                    stack.Push(childDirectoryNode);
                }

                var filesToDelete = GetFilesToDelete(directoryPath);

                foreach (var file in directoryInfo.GetFiles())
                {
                    var treeViewItem = new TreeViewItem { Header = file.Name };
                    var test = file.FullName;

                    if (filesToDelete.Contains(test))
                    {
                        treeViewItem.Foreground = Brushes.Red;
                    }

                    treeViewItem.AddHandler(TreeViewItem.MouseDoubleClickEvent, new MouseButtonEventHandler(OnTreeViewItemClicked));
                    currentNode.Items.Add(treeViewItem);
                }
            }

            tv_SolutionFiles.Items.Add(node);
        }

        private List<string> GetFilesToDelete(string directoryPath)
        {
            var filesToDelete = DirectoryManager.GetAllFilesWithName(directoryPath, "ALL_BUILD");
            filesToDelete.AddRange(DirectoryManager.GetAllFilesWithName(directoryPath, "CMakeCache"));
            filesToDelete.AddRange(DirectoryManager.GetAllFilesWithName(directoryPath, "cmake_install"));
            filesToDelete.AddRange(DirectoryManager.GetAllFilesWithName(directoryPath, "ZERO_CHECK"));

            return filesToDelete;
        }

        private void OnTreeViewItemClicked(object sender, MouseButtonEventArgs mouseButtonEventArgs)
        {
            var treeViewItem = (TreeViewItem)(sender);
            var parentItem = (TreeViewItem)(treeViewItem.Parent);
            
            if(parentItem != null && parentItem is TreeViewItem)
            {
                var fileFullPath = /*SolutionDirectory + @"\" + */parentItem.Tag.ToString() + @"\" + treeViewItem.Header;
                OpenFileInTab(fileFullPath, treeViewItem.Header.ToString());
            }
        }

        private void OpenFileInTab(string filePath, string fileName)
        {
            var fileContent = ReadFile(filePath);

            var textBox = CreateTextBoxFilledWithFileContent(fileContent);
            var tabItem = CreateTabItem(textBox, fileName);

            tc_FileContents.Items.Add(tabItem);
            tc_FileContents.SelectedItem = tabItem;            
        }

        private TextBox CreateTextBoxFilledWithFileContent(string textBoxContent)
        {
            var textBox = new TextBox() { Text = textBoxContent };
            textBox.HorizontalAlignment = HorizontalAlignment.Stretch;
            textBox.VerticalAlignment = VerticalAlignment.Stretch;

            return textBox;
        }

        private TabItem CreateTabItem(object content, string header)
        {
            var tabItem = new TabItem();
            tabItem.Content = content;
            tabItem.Header = header;

            return tabItem;
        }

        private string ReadFile(string filePath)
        {
            return File.ReadAllText(filePath);
        }

        string SolutionDirectory { get; set; }
    }
}
