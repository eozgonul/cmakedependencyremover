using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Collections.ObjectModel;
using System.IO;
using System.Windows.Input;
using Prism.Commands;
using System.ComponentModel;

namespace CmakeDependencyRemover.UI.ViewModels
{
    class DirectoryView : INotifyPropertyChanged
    {
        private int selectedIndex;
        public DelegateCommand<TreeViewItemViewModel> TreeNodeDoubleClickEvent { get; set; }

        public ReadOnlyCollection<DirectoryViewModel> Directories { get; set; }
        public ObservableCollection<FileViewModel> OpenFiles { get; set; }
        public FileViewModel CurrentItem { get; set; }
        public int SelectedIndex
        {
            get
            {
                return selectedIndex;
            }
            set
            {
                selectedIndex = value;
                this.OnPropertyChanged("SelectedIndex");
            }
        }
                
        public DirectoryView(DirectoryInfo[] directoryInfos)
        {
            Directories = new ReadOnlyCollection<DirectoryViewModel>(
                (from directory in directoryInfos
                 select new DirectoryViewModel(directory))
                 .ToList());

            OpenFiles = new ObservableCollection<FileViewModel>();
            TreeNodeDoubleClickEvent = new DelegateCommand<TreeViewItemViewModel>(MouseDoubleClick);
        }

        private void MouseDoubleClick(TreeViewItemViewModel viewModel)
        {
            if(viewModel is DirectoryViewModel)
            {
                return;
            }
            else if(viewModel is FileViewModel)
            {
                var item = (FileViewModel)viewModel;
                OpenFiles.Add(item);
                SelectedIndex = OpenFiles.IndexOf(item);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            if(this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
