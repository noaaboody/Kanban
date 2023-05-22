using Frontend.Model;
using Frontend.ViewModel;
using System.Windows;
using Frontend.View;

namespace Frontend
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private MainViewModel viewModel;

        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = new MainViewModel();
            this.viewModel = (MainViewModel)DataContext;

        }

        /// <summary>
        /// The login button logged in the user and tranfer to the his boards view.
        /// </summary>
        private void Login_Click(object sender, RoutedEventArgs e)
        {
            UserModel u = viewModel.Login();
            if (u != null)
            {
                BoardView boardView = new BoardView(u);
                boardView.Show();
                this.Close();
            }

        }

        /// <summary>
        /// The register button register the user and tranfer to the his boards view.
        /// </summary>
        private void Register_Click(object sender, RoutedEventArgs e)
        {
            UserModel u = viewModel.Register();
            if (u != null)
            {
                BoardView boardView = new BoardView(u);
                boardView.Show();
                this.Close();
            }

        }

    }
}
