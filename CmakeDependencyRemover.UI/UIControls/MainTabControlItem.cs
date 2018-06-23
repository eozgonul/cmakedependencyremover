using System.Windows;
using System.Windows.Controls;
using System.IO;

namespace CmakeDependencyRemover.UI.UIControls
{
    public class MainTabControlItem : TabItem
    {
        static MainTabControlItem()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(MainTabControlItem), new FrameworkPropertyMetadata(typeof(MainTabControlItem)));
        }

        public FileInfo FileInformation { get; set; }
    }
}
