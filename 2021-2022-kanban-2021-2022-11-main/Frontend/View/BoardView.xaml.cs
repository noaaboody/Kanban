using Frontend.Model;
using Frontend.ViewModel;
using System.Windows;

namespace Frontend.View
{
    /// <summary>
    /// Interaction logic for BoardView.xaml
    /// </summary>
    public partial class BoardView : Window
    {
        private BoardViewModel viewModel;
        private readonly UserModel userModel;
        public BoardView(UserModel u)
        {
            InitializeComponent();
            this.viewModel = new BoardViewModel(u);
            this.DataContext = viewModel;
            userModel = u;
        }

        /// <summary>
        /// The view board button transfer the user to the Task view according to the selected board.
        /// </summary>
        private void ViewBoard_Button(object sender, RoutedEventArgs e)
        {
            BoardModel b = viewModel.SelectedBoard;
            if (b != null)
            {
                TaskView taskView = new TaskView(b,userModel);
                taskView.Show();
                this.Close();
            }
        }

        /// <summary>
        /// The button logged out the user to the previous view (main view).
        /// </summary>
        private void Button_Click(object sender, RoutedEventArgs e)
        {           
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        }
    }
}
