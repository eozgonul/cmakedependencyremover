﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;

namespace CmakeDependencyRemover.UI.ViewModels
{
    class FileViewModel : TreeViewItemViewModel
    {
        readonly FileInfo fileInfo;

        public FileViewModel(FileInfo fileInfo) : base(null, false)
        {
            this.fileInfo = fileInfo;
        }

        public string FileName
        {
            get { return fileInfo.Name; }
        }

        protected override void LoadChildren() => base.LoadChildren();

        public string Content
        {
            get
            {
                return File.ReadAllText(fileInfo.FullName);
            }
        }
    }
}
