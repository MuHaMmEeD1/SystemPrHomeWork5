using System.Collections.ObjectModel;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace SystemPrHomeWork5
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public ObservableCollection<Thread> ThreadsCreat { get; set; } = new();   
        public ObservableCollection<Thread> ThreadsWait { get; set; } = new();   
        public ObservableCollection<Thread> ThreadsFinis { get; set; } = new();
        public object? Key1 { get; set;} = new();
        public object? Key2 { get; set;} = new();
        public object? Key3 { get; set;} = new();


        public SemaphoreSlim semaphoreSlim = new SemaphoreSlim(3);

        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = this;


        }


        public void IsMethod(object thr)
        {

            lock (Key1)
            {
                var selectedThread = thr as Thread;

                int IND = 0;

                for (int i = 0; i < ThreadsCreat.Count; i++)
                {
                    if (ThreadsCreat[i].ManagedThreadId == selectedThread.ManagedThreadId) { IND = i; }
                }


                Dispatcher.Invoke(() => {
                    ThreadsWait.Add(ThreadsCreat[IND]);
                    ThreadsCreat.RemoveAt(IND);
                });
            }
           

            semaphoreSlim.Wait();


            Thread.Sleep(5000);
            lock (Key2)
            {
                Dispatcher.Invoke(() =>
                {
                    ThreadsFinis.Add(ThreadsWait[ThreadsWait.Count - 1]);
                    ThreadsWait.RemoveAt(ThreadsWait.Count - 1);
                });
            }

            Thread.Sleep(3000);

            lock (Key3)
            {

                Dispatcher.Invoke(() => { ThreadsFinis.RemoveAt(ThreadsFinis.Count-1); });
            }

            semaphoreSlim.Release();
        }



        private void Button_Click(object sender, RoutedEventArgs e) // creat
        {
            ThreadsCreat.Add(new Thread(IsMethod));

        }

        private void CreatListViwe_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {


            if (sender is ListView listView)
            {
                if (listView.SelectedItem != null)
                {
                    var selectedThread = listView.SelectedItem as Thread;

                    if (selectedThread != null)
                    {
                       



                        selectedThread.Start(selectedThread);
                    }
                }
            }
        }
    }
}