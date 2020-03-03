using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace GradedExercise
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private ObservableCollection<User> data;

        public MainWindow()
        {
            InitializeComponent();

            data = new ObservableCollection<User>();

            Grid.DataContext = data;
        }

        private void mItemOpenFile_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog file = new OpenFileDialog();
            file.DefaultExt = ".txt";
            file.Filter = "Text Document (.txt)|*.txt";
            DateTime timeStamp = DateTime.Now;
            if (file.ShowDialog() == true)
            {
                using (StreamReader reader = new StreamReader(file.OpenFile()))
                {
                    string fileContent = reader.ReadToEnd();
                    try
                    {
                        foreach (var item in fileContent.Split('\n'))
                        {
                            int id = int.Parse(item.Split(';')[0]);
                            string name = item.Split(';')[1];
                            int age = int.Parse(item.Split(';')[2]);
                            int score = int.Parse(item.Split(';')[3]);

                            data.Add(new User(id, name, age, score));
                        }
                        statusBarLabel.Content = $"Added at {timeStamp} - Contains {data.Count} users"; 
                    } 
                    catch (Exception)
                    {
                        MessageBoxButton button = MessageBoxButton.OK;
                        MessageBoxImage icon = MessageBoxImage.Error;
                        string caption = "Error!";
                        string text = "Unreadable file content!";
                        MessageBox.Show(text, caption, button, icon);
                        statusBarLabel.Content = "Failed to load file!";
                    }
                }
            }
        }

        private void mItemExit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }

    class User : INotifyPropertyChanged
    {
        private int id;
        private string name;
        private int age;
        private int score;

        public User(int id, string name, int age, int score)
        {
            this.id = id;
            this.name = name;
            this.age = age;
            this.score = score;
        }

        public string Name
        {
            set
            {
                name = value;
                NotifyPropertyChanged("Name");
            }
            get { return name; }
        }

        public int Id
        {
            set
            {
                id = value;
                NotifyPropertyChanged("Weigth");
            }
            get { return id; }
        }

        public int Age
        {
            set
            {
                age = value;
                NotifyPropertyChanged("Age");
            }
            get { return age; }
        }

        public int Score
        {
            set
            {
                score = value;
                NotifyPropertyChanged("Score");
            }
            get { return score; }
        }

        public override string ToString()
        {
            return Id + ": " + Name;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
