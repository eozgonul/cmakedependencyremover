using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Collections.ObjectModel;
using System.ComponentModel;

namespace CmakeDependencyRemover.UI.ViewModels
{
    class TreeViewItemViewModel : INotifyPropertyChanged
    {
        static readonly TreeViewItemViewModel DummyChild = new TreeViewItemViewModel();
        bool isExpanded;
        bool isSelected;

        protected TreeViewItemViewModel(TreeViewItemViewModel parent, bool lazyLoadChildren)
        {
            Parent = parent;

            Children = new ObservableCollection<TreeViewItemViewModel>();

            if(lazyLoadChildren)
            {
                Children.Add(DummyChild);
            }
        }

        private TreeViewItemViewModel()
        {
        }

        public ObservableCollection<TreeViewItemViewModel> Children { get; }

        public bool HasDummyChild => Children.Count == 1 && Children[0] == DummyChild;

        public bool IsExpanded
        {
            get => isExpanded;
            set
            {
                if (value != isExpanded)
                {
                    isExpanded = value;
                    OnPropertyChanged("IsExpanded");
                }

                if (isExpanded && Parent != null)
                {
                    Parent.IsExpanded = true;
                }
                if (HasDummyChild)
                {
                    Children.Remove(DummyChild);
                    LoadChildren();
                }
            }
        }

        public bool IsSelected
        {
            get => isSelected;
            set
            {
                if (value != isSelected)
                {
                    isSelected = value;
                    OnPropertyChanged("IsSelected");
                }
            }
        }

        protected virtual void LoadChildren() {}
        
        public TreeViewItemViewModel Parent { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            if(PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
