using CommunityToolkit.Mvvm.ComponentModel;

namespace MYTASKS.Models
{
    // Model phải kế thừa ObservableObject theo yêu cầu
    public partial class TaskItem : ObservableObject
    {
        [ObservableProperty]
        private string title;

        [ObservableProperty]
        private bool isCompleted;
    }
}