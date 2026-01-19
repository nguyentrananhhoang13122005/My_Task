using MYTASKS.ViewModels;

namespace MYTASKS
{
    public partial class MainPage : ContentPage
    {
        public MainPage(TasksViewModel vm)
        {
            InitializeComponent();
            BindingContext = vm;
        }
    }
}