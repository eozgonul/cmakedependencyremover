using System.Windows;
using System.Windows.Controls;
using System.IO;

namespace CmakeDependencyRemover.UI.UIControls
{
    public class MainTabControl : TabControl
    {
        static MainTabControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(MainTabControl), new FrameworkPropertyMetadata(typeof(MainTabControl)));
        }

        public void OpenFile(FileInfo fileInformation)
        {
            var item = CheckIfFileOpenInTab(fileInformation);

            if(item != null)
            {
                SelectedItem = item;
            }
            else
            {
                var fileContent = File.ReadAllText(fileInformation.FullName);

                var textBox = CreateTextBoxFilledWithFileContent(fileContent);
                var tabItem = CreateTabItem(textBox, fileInformation);

                Items.Add(tabItem);
                SelectedItem = tabItem;
            }
        }

        public MainTabControlItem CheckIfFileOpenInTab(FileInfo fileInformation)
        {
            foreach (MainTabControlItem item in Items)
            {
                if(item.FileInformation.FullName == fileInformation.FullName)
                {
                    return item;
                }
            }
            return null;
        }

        private TabItem CreateTabItem(object content, FileInfo fileInformation)
        {
            var tabItem = new UIControls.MainTabControlItem();
            tabItem.Content = content;
            tabItem.Header = fileInformation.Name;
            tabItem.FileInformation = fileInformation;

            return tabItem;
        }

        private TextBox CreateTextBoxFilledWithFileContent(string textBoxContent)
        {
            var textBox = new TextBox() { Text = textBoxContent };
            textBox.HorizontalAlignment = HorizontalAlignment.Stretch;
            textBox.VerticalAlignment = VerticalAlignment.Stretch;

            return textBox;
        }
    }
}
