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
        readonly ObservableCollection<TreeViewItemViewModel> children;

        bool isExpanded;
        bool isSelected;


        protected TreeViewItemViewModel(TreeViewItemViewModel parent, bool lazyLoadChildren)
        {
            Parent = parent;

            children = new ObservableCollection<TreeViewItemViewModel>();

            if(lazyLoadChildren)
            {
                children.Add(DummyChild);
            }
        }

        private TreeViewItemViewModel()
        {
        }

        public ObservableCollection<TreeViewItemViewModel> Children
        {
            get { return children; }
        }

        public bool HasDummyChild
        {
            get { return Children.Count == 1 && Children[0] == DummyChild; }
        }

        public bool IsExpanded
        {
            get { return isExpanded; }
            set
            {
                if(value != isExpanded)
                {
                    isExpanded = value;
                    OnPropertyChanged("IsExpanded");
                }

                if(isExpanded && Parent != null)
                {
                    Parent.IsExpanded = true;
                }
                if(HasDummyChild)
                {
                    Children.Remove(DummyChild);
                    LoadChildren();
                }
            }
        }

        public bool IsSelected
        {
            get { return isSelected; }
            set
            {
                if(value != isSelected)
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
            if(this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
