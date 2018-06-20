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
        readonly DirectoryInfo _directoryInfo;
        
        public DirectoryViewModel(DirectoryInfo directoryInfo) : base(null, true)
        {
            _directoryInfo = directoryInfo;
        }

        public string DirectoryName
        {
            get { return _directoryInfo.Name; }
        }

        protected override void LoadChildren()
        {
            foreach(DirectoryInfo directoryInfo in _directoryInfo.GetDirectories())
            {
                base.Children.Add(new DirectoryViewModel(directoryInfo));
            }
        }

    }
}
