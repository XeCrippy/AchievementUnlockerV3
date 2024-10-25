using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace AchievementUnlockerV2
{
    /// <summary>
    /// Interaction logic for XMessageBox.xaml
    /// </summary>
    public partial class XMessageBox : Window
    {
        public XMessageBox(string message, string caption)
        {
            InitializeComponent();
            CaptionBox.Text = caption;
            MessageBlock.Text = message;
        }

        private void CloseBtn_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }
    }
}
