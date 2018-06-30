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
using System.Threading;

using System.IO;

namespace CmakeDependencyRemover.UI
{
    public partial class MainWindow : Window
    {
        string SolutionDirectory { get; set; }
        
        public MainWindow()
        {
            InitializeComponent();

            InitializeSubComponents();

            //this.DataContext = new ViewModels.MainWindowView();
            //tc_FileContents.DataContext = new ViewModels.TabControlView();
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
            var directoryInfos = new DirectoryInfo[] { new DirectoryInfo(selectedDirectory) };
            var directoryView = new ViewModels.DirectoryView(directoryInfos);

            tv_SolutionFiles.DataContext = directoryView;
            tc_FileContents.DataContext = directoryView;
        }

        private void ClearSolutionFiles()
        {
            tv_SolutionFiles.Items.Clear();
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
            //var parentItem = (TreeViewItem)(treeViewItem.Parent);
            
            //if(parentItem != null && parentItem is TreeViewItem)
            //{
            //    var fileFullPath = /*SolutionDirectory + @"\" + */parentItem.Tag.ToString() + @"\" + treeViewItem.Header;
                //OpenFileInTab(fileFullPath, treeViewItem.FileInformation);
            //}

          
            var header = treeViewItem.Header.ToString();

        }

        private void OpenFileInTab(string filePath, FileInfo fileInformation)
        {
            //tc_FileContents.OpenFile(fileInformation);
        }
    }
}
