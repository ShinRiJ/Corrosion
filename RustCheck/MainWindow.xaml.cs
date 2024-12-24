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
using System.Windows.Navigation;
using System.Windows.Shapes;

using Emgu.CV;
using RustCheck.MVVM.ViewModels;

namespace RustCheck
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = new ApplicationModelView();

            ApplicationModelView.TabItemAction += OnTabAdded;
        }

        private void OnTabAdded(Boolean action, TabItem? newTab = null)
        {
            if (action)
                MainTab.Items.Add(newTab);
            else
            {
                if (newTab is not null)
                    MainTab.Items.Remove(newTab);
                else
                    while (MainTab.Items.Count > 1)
                        MainTab.Items.RemoveAt(1);
            }
        }

        [System.Runtime.InteropServices.DllImport("gdi32.dll")]
        public static extern bool DeleteObject(IntPtr hObject);
    }
}
