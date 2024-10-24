using System.Windows;

namespace AchievementUnlockerV2
{
    /// <summary>
    /// Interaction logic for WaitForm.xaml
    /// </summary>
    public partial class WaitForm : Window
    {
        //public static string Caption { get; set; }
        //public static string Description { get; set; }

        public WaitForm()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //CaptionBox.Text = Caption;
            //DescriptionBox.Text = Description;
        }
    }
}
