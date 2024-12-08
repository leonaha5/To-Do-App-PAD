using System.Collections.ObjectModel;
using System.Linq;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Layout;
using Avalonia.Media;

namespace ToDoList;

public partial class MainWindow : Window
{
    private readonly ObservableCollection<Task> _tasks = [];

    public MainWindow()
    {
        InitializeComponent();
        FilterComboBox.SelectedIndex = 0;
        TasksListBox.ItemsSource = _tasks;
    }

    private void AddTask(object? sender, RoutedEventArgs e)
    {
        var description = TaskTextBox.Text;
        if (string.IsNullOrWhiteSpace(description))
        {
            new Window
            {
                Width = 150,
                Height = 100,
                Title = "Error",
                Background = new SolidColorBrush(Colors.Red),
                Content = new TextBlock
                {
                    Text = "Task description is required",
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center
                }
            }.ShowDialog(this);
            return;
        }

        _tasks.Add(new Task(description));
        TaskTextBox.Text = string.Empty;
        RefreshTaskList();
    }

    private void DeleteTask(object? sender, RoutedEventArgs e)
    {
        if (TasksListBox.SelectedItem is not Task selectedTask) return;
        _tasks.Remove(selectedTask);
        RefreshTaskList();
    }

    private void MarkTaskCompleted(object? sender, RoutedEventArgs e)
    {
        if (TasksListBox.SelectedItem is not Task selectedTask) return;
        selectedTask.IsCompleted = !selectedTask.IsCompleted;
        RefreshTaskList();
    }

    private void FilterChanged(object? sender, SelectionChangedEventArgs e)
    {
        RefreshTaskList();
    }

    private void RefreshTaskList()
    {
        TasksListBox.ItemsSource = (FilterComboBox.SelectedIndex switch
        {
            1 => _tasks.Where(task => !task.IsCompleted),
            2 => _tasks.Where(task => task.IsCompleted),
            _ => _tasks
        }).ToList();
    }
}

public class Task(string description)
{
    private string Description { get; } = description;
    public bool IsCompleted { get; set; }

    public override string ToString()
    {
        return $"{Description}{(IsCompleted ? ": Completed" : ": Not completed")}";
    }
}