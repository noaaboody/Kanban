using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Frontend.Model;
using Frontend.ViewModel;

namespace Frontend.View
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class TaskView : Window
    {
        private TaskViewModel viewModel;
        private readonly UserModel userModel;
        public TaskView(BoardModel b, UserModel u)
        {
            InitializeComponent();
            userModel = u;
            this.viewModel = new TaskViewModel(b);
            this.DataContext = viewModel;
        }

        /// <summary>
        /// The button transfer the user to the previous view (boards view).
        /// </summary>
        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            BoardView board = new BoardView(userModel);
            board.Show();
            this.Close();
        }

    }
}
