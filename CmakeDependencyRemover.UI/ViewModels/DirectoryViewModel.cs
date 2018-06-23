using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;

namespace CmakeDependencyRemover.UI.ViewModels
{
    class DirectoryViewModel : TreeViewItemViewModel
    {
        readonly DirectoryInfo directoryInfo;
        
        public DirectoryViewModel(DirectoryInfo directoryInfo) : base(null, true)
        {
            this.directoryInfo = directoryInfo;
        }

        public string DirectoryName
        {
            get { return directoryInfo.Name; }
        }

        protected override void LoadChildren()
        {
            foreach(DirectoryInfo directoryInfo in this.directoryInfo.GetDirectories())
            {
                base.Children.Add(new DirectoryViewModel(directoryInfo));
            }

            foreach(FileInfo fileInfo in directoryInfo.GetFiles())
            {
                base.Children.Add(new FileViewModel(fileInfo));
            }
        }

    }
}
