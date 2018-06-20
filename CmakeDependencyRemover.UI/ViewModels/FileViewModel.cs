using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;

namespace CmakeDependencyRemover.UI.ViewModels
{
    class FileViewModel : TreeViewItemViewModel
    {
        readonly FileInfo _fileInfo;

        public FileViewModel(FileInfo fileInfo) : base(null, true)
        {
            _fileInfo = fileInfo;
        }

        public string FileName
        {
            get { return _fileInfo.Name; }
        }

        protected override void LoadChildren()
        {
            base.LoadChildren();
        }
    }
}
