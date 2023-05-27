using System.IO;
using MadEye.Views;
using System.ComponentModel;

namespace MadEye.GlobalClasses
{
    public class PathManager : INotifyPropertyChanged
    {
        private string selectedDate;
        private string homeDirectory;
        private string username;
        private string screenshots;
        private string webCamImages;
        private string database;
        private string other;
        private string temporaryFiles;
        private string fetchedDatabaseFiles;

        public event PropertyChangedEventHandler PropertyChanged;

        public string SelectedDate
        {
            get => selectedDate;
            set
            {
                if (selectedDate != value)
                {
                    selectedDate = value.Replace("\\", "-");
                    NotifyPropertyChanged(nameof(SelectedDate));
                    UpdateConnectedValues();
                }
            }
        }

        public string HomeDirectory
        {
            get => homeDirectory;
            private set
            {
                if (homeDirectory != value)
                {
                    homeDirectory = value;
                    NotifyPropertyChanged(nameof(HomeDirectory));
                    UpdateConnectedValues();
                }
            }
        }

        public string Username
        {
            get => username;
            private set
            {
                if (username != value)
                {
                    username = value;
                    NotifyPropertyChanged(nameof(Username));
                    UpdateConnectedValues();
                }
            }
        }

        public string Screenshots
        {
            get => screenshots;
            private set
            {
                if (screenshots != value)
                {
                    screenshots = value;
                    NotifyPropertyChanged(nameof(Screenshots));
                    UpdateConnectedValues();
                }
            }
        }

        public string ScreenshotsSelectedDateFolder => Path.Combine(Screenshots, SelectedDate);

        public string WebCamImages
        {
            get => webCamImages;
            private set
            {
                if (webCamImages != value)
                {
                    webCamImages = value;
                    NotifyPropertyChanged(nameof(WebCamImages));
                    UpdateConnectedValues();
                }
            }
        }

        public string WebCamSelectedDateFolder => Path.Combine(WebCamImages, SelectedDate);

        public string Database
        {
            get => database;
            private set
            {
                if (database != value)
                {
                    database = value;
                    NotifyPropertyChanged(nameof(Database));
                    UpdateConnectedValues();
                }
            }
        }

        public string Other
        {
            get => other;
            private set
            {
                if (other != value)
                {
                    other = value;
                    NotifyPropertyChanged(nameof(Other));
                    UpdateConnectedValues();
                }
            }
        }

        public string TemporaryFiles
        {
            get => temporaryFiles;
            private set
            {
                if (temporaryFiles != value)
                {
                    temporaryFiles = value;
                    NotifyPropertyChanged(nameof(TemporaryFiles));
                    UpdateConnectedValues();
                }
            }
        }

        public string FetchedDatabaseFiles
        {
            get => fetchedDatabaseFiles;
            private set
            {
                if (fetchedDatabaseFiles != value)
                {
                    fetchedDatabaseFiles = value;
                    NotifyPropertyChanged(nameof(FetchedDatabaseFiles));
                    UpdateConnectedValues();
                }
            }
        }

        private static PathManager? instance;

        private PathManager()
        {
            // Set Variable's Values Here
            selectedDate = ((ShellPage)App.MainWindow.Content).Selected_Date.Replace("\\", "-");
            HomeDirectory = @"D:\FYP\";
            UpdateUsername(((ShellPage)App.MainWindow.Content).SelectedUser); // Set initial Username value based on SelectedUser
            Screenshots = Path.Combine(Username, "Screenshots");
            WebCamImages = Path.Combine(Username, "WebCam Images");
            Database = Path.Combine(Username, "Database");
            Other = Path.Combine(Username, "Other");
            TemporaryFiles = Path.Combine(Other, "Temporary Files");
            FetchedDatabaseFiles = Path.Combine(TemporaryFiles, "Fetched Database Files");
        }

        public static PathManager GetInstance()
        {
            var newInstance = instance ?? new PathManager();
            instance = null; // Reset instance to null after it has been accessed
            return newInstance;
        }

        public void CheckFolder(string folder)
        {
            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }
        }

        public void UpdateUsername(string selectedUser)
        {
            string newUsername = Path.Combine(HomeDirectory, selectedUser);
            if (Username != newUsername)
            {
                Username = newUsername;
            }
        }

        private void NotifyPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void UpdateConnectedValues()
        {
            NotifyPropertyChanged(nameof(ScreenshotsSelectedDateFolder));
            NotifyPropertyChanged(nameof(WebCamSelectedDateFolder));
        }
    }
}
