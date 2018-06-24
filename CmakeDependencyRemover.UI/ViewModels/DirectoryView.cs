using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Collections.ObjectModel;
using System.IO;
using System.Windows.Input;
using Prism.Commands;
using System.Windows;

namespace CmakeDependencyRemover.UI.ViewModels
{
    class DirectoryView
    {
        readonly ReadOnlyCollection<DirectoryViewModel> directoryViewModels;
        
        public DirectoryView(DirectoryInfo[] directoryInfos)
        {
            directoryViewModels = new ReadOnlyCollection<DirectoryViewModel>(
                (from directory in directoryInfos
                 select new DirectoryViewModel(directory))
                 .ToList());

            TreeNodeDoubleClickEvent = new DelegateCommand<TreeViewItemViewModel>(MouseDoubleClick);
        }

        public ReadOnlyCollection<DirectoryViewModel> Directories
        {
            get { return directoryViewModels; }
        }

        public DelegateCommand<TreeViewItemViewModel> TreeNodeDoubleClickEvent { get; set; }

        private void MouseDoubleClick(TreeViewItemViewModel viewModel)
        {
            if(viewModel is DirectoryViewModel)
            {
                return;
            }
            else if(viewModel is FileViewModel)
            {
                var test = ((FileViewModel)viewModel).FileName;
            }
        }
    }
}
