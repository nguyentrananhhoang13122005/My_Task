using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MYTASKS.Models;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace MYTASKS.ViewModels
{
    public partial class TasksViewModel : ObservableObject
    {
        public TasksViewModel()
        {
            // 1. Khởi tạo danh sách với dữ liệu mẫu (để giống hình Lab)
            Tasks = new ObservableCollection<TaskItem>
            {
                new TaskItem { Title = "Set up MVVM project structure", IsCompleted = false },
                new TaskItem { Title = "Bind entry text to property", IsCompleted = false },
                new TaskItem { Title = "Install CommunityToolkit.Mvvm", IsCompleted = true }, // Cái này đã xong để test Converter
                new TaskItem { Title = "Implement RelayCommand for Add", IsCompleted = false }
            };

            // 2. Quan trọng: Gắn sự kiện lắng nghe cho các item mẫu này
            // Nếu không có bước này, bấm checkbox của item cũ sẽ không cập nhật số lượng
            foreach (var task in Tasks)
            {
                task.PropertyChanged += Task_PropertyChanged;
            }

            // 3. Cập nhật số lượng ban đầu (Total/Completed)
            UpdateCounts();

            // 4. Lắng nghe khi danh sách thay đổi (Thêm/Xóa)
            Tasks.CollectionChanged += (s, e) =>
            {
                // Nếu có item mới được thêm vào, cũng phải lắng nghe sự kiện change của nó
                if (e.NewItems != null)
                {
                    foreach (TaskItem item in e.NewItems)
                        item.PropertyChanged += Task_PropertyChanged;
                }
                UpdateCounts();
            };
        }

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(AddTaskCommand))]
        private string newTaskTitle = string.Empty;

        [ObservableProperty]
        private ObservableCollection<TaskItem> tasks;

        [ObservableProperty]
        private TaskItem selectedTask;

        [ObservableProperty]
        private int totalCount;

        [ObservableProperty]
        private int completedCount;

        [RelayCommand(CanExecute = nameof(CanAddTask))]
        private void AddTask()
        {
            var task = new TaskItem { Title = NewTaskTitle, IsCompleted = false };

            
            Tasks.Add(task);

            NewTaskTitle = string.Empty;
        }

        private bool CanAddTask() => !string.IsNullOrWhiteSpace(NewTaskTitle);

        
        private void Task_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(TaskItem.IsCompleted))
            {
                UpdateCounts();
            }
        }

        private void UpdateCounts()
        {
            TotalCount = Tasks.Count;
            CompletedCount = Tasks.Count(t => t.IsCompleted);
        }
    }
}