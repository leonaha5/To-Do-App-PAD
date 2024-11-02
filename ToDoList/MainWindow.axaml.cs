using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Avalonia.Controls;
using Avalonia.Interactivity;

namespace ToDoList
{
    public partial class MainWindow : Window
    {
        // Kolekcja wszystkich zadań
        private ObservableCollection<ToDoTask> tasks = new ObservableCollection<ToDoTask>();

        public MainWindow()
        {
            InitializeComponent();
            MyComboBox.SelectedIndex = 0;
            MyListTasks.ItemsSource = tasks;
        }

        private void MySubmit_OnClick(object? sender, RoutedEventArgs e)
        {
            var description = MyTextBox.Text;
            if (!string.IsNullOrWhiteSpace(description))
            {
                var task = new ToDoTask(description);
                tasks.Add(task);
                MyTextBox.Text = string.Empty;
                RefreshTaskList();
            }
            else
            {
                var messageBox = new Window
                {
                    Title = "Błąd",
                    Content = new TextBlock { Text = "Wpisz opis zadania." }
                };
                messageBox.ShowDialog(this);
            }
        }

        private void MyDelete_OnClick(object? sender, RoutedEventArgs e)
        {
            if(MyListTasks.SelectedItem is ToDoTask selectedTask)
            {
                tasks.Remove(selectedTask);
                RefreshTaskList();
            }
        }

        private void MyCheckBox_Checked(object? sender, RoutedEventArgs e)
        {
            if (MyListTasks.SelectedItem is ToDoTask selectedTask)
            {
                selectedTask.IsCompleted = true;
                RefreshTaskList();
            }
        }

        private void MyCheckBox_Unchecked(object? sender, RoutedEventArgs e)
        {
            if (MyListTasks.SelectedItem is ToDoTask selectedTask)
            {
                selectedTask.IsCompleted = false;
                RefreshTaskList();
            }
        }

        private void MyComboBox_OnSelectionChanged(object? sender, SelectionChangedEventArgs e)
        {
            RefreshTaskList();
        }

        private void RefreshTaskList()
        {
            var filter = MyComboBox.SelectedIndex;
            IEnumerable<ToDoTask> filteredTasks = filter switch
            {
                1 => tasks.Where(task => !task.IsCompleted), 
                2 => tasks.Where(task => task.IsCompleted),   
                _ => tasks                                    
            };
            
            MyListTasks.ItemsSource = filteredTasks.ToList();
        }
    }
    public class ToDoTask
    {
        public string Description { get; set; }
        public bool IsCompleted { get; set; }

        public ToDoTask(string description)
        {
            Description = description;
            IsCompleted = false;
        }
    
        public override string ToString()
        {
            return $"{Description} {(IsCompleted ? "(Zrobione)" : "(Do zrobienia)")}";
        }
    }
}