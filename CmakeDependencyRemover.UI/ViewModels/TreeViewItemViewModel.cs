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

        readonly TreeViewItemViewModel parent;
        readonly ObservableCollection<TreeViewItemViewModel> children;

        bool isExpanded;
        bool isSelected;


        protected TreeViewItemViewModel(TreeViewItemViewModel parent, bool lazyLoadChildren)
        {
            this.parent = parent;

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
                    this.OnPropertyChanged("IsExpanded");
                }

                if(isExpanded && parent != null)
                {
                    parent.IsExpanded = true;
                }
                if(this.HasDummyChild)
                {
                    this.Children.Remove(DummyChild);
                    this.LoadChildren();
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
                    this.OnPropertyChanged("IsSelected");
                }
            }
        }

        protected virtual void LoadChildren()
        {

        }
        
        public TreeViewItemViewModel Parent
        {
            get { return parent; }
        }

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
