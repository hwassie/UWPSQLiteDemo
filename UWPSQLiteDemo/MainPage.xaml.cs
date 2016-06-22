using SQLite.Net.Attributes;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;

using Windows.Foundation;
using Windows.Foundation.Collections;

using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;  

// The Blank Page item template is documented at  http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409 
 
namespace UWPSQLiteDemo
{

    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>

    public sealed partial class MainPage : Page
    {
        string path;
        SQLite.Net.SQLiteConnection conn;

        public MainPage()
        {
            this.InitializeComponent();
            path = Path.Combine(Windows.Storage.ApplicationData.Current.LocalFolder.Path,
               "db.sqlite");
            conn = new SQLite.Net.SQLiteConnection(new
               SQLite.Net.Platform.WinRT.SQLitePlatformWinRT(), path);
            conn.CreateTable<Customer>();
            this.InitializeComponent();
            this.ViewModel = new RecordingViewModel();
        }

        public RecordingViewModel ViewModel { get; set; }

        private void Retrieve_Click(object sender, RoutedEventArgs e)
        {
            var query = conn.Table<Customer>();
            string id = "";
            string name = "";
            string age = "";

            foreach (var message in query)
            {
                id = id + " " + message.Id;
                name = name + " " + message.Name;
                age = age + " " + message.Age;
            }

            textBlock2.Text = "ID: " + id + "\nName: " + name + "\nAge: " + age;
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {

            var s = conn.Insert(new Customer()
            {
                Name = textBox.Text,
                Age = textBox1.Text
            });

        }
    }

    public class Customer
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Age { get; set; }
    }

    public class RecordingViewModel
    {


        private Recording defaultRecording = new Recording();
        public Recording DefaultRecording { get { return this.defaultRecording; } }


        private System.Collections.ObjectModel.ObservableCollection<Recording> recordings = new System.Collections.ObjectModel.ObservableCollection<Recording>();
        public System.Collections.ObjectModel.ObservableCollection<Recording> Recordings { get { return this.recordings; } }

        public RecordingViewModel()
        {
            this.recordings.Add(new Recording()
            {
                ArtistName = "Johann Sebastian Bach",
                CompositionName = "Mass in B minor",
                ReleaseDateTime = new DateTime(1748, 7, 8)
            });
            this.recordings.Add(new Recording()
            {
                ArtistName = "Ludwig van Beethoven",
                CompositionName = "Third Symphony",
                ReleaseDateTime = new DateTime(1805, 2, 11)
            });
            this.recordings.Add(new Recording()
            {
                ArtistName = "George Frideric Handel",
                CompositionName = "Serse",
                ReleaseDateTime = new DateTime(1737, 12, 3)
            });
        }
    }


    public class Recording
    {
        public string ArtistName { get; set; }
        public string CompositionName { get; set; }
        public DateTime ReleaseDateTime { get; set; }

        public Recording()
        {
            this.ArtistName = "Wolfgang Amadeus Mozart";
            this.CompositionName = "Andante in C for Piano";
            this.ReleaseDateTime = new DateTime(1761, 1, 1);
        }

        public string OneLineSummary
        {
            get
            {
                return $"{this.CompositionName} by {this.ArtistName}, released: "
                    + this.ReleaseDateTime.ToString("d");
            }
        }
    }


}