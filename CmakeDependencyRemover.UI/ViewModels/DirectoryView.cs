using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Collections.ObjectModel;
using System.IO;

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
        }

        public ReadOnlyCollection<DirectoryViewModel> Directories
        {
            get { return directoryViewModels; }
        }
    }
}
