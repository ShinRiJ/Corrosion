using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using Emgu.CV;
using Emgu.CV.Ccm;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using System.Drawing;
using Emgu.CV.Util;
using System.Threading;
using Microsoft.Win32;
using System.IO;
using System.Data;
using static System.Net.WebRequestMethods;
using System.Windows.Ink;

namespace Corrosion_damage
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        CorrosionCalculator MainCalc = null;
        public MainWindow()
        {
            InitializeComponent();

            Dropper.Drop += Dropper_Drop;
            Dropper.PreviewMouseDown += OpenFile_Click;
            Dropper.MouseWheel += Dropper_MouseWheel;

            SizeChanged += MainWindow_SizeChanged;
            GammaSlider.ValueChanged += GammaSlider_ValueChanged;
            ImageProcessing.MouseWheel += ImageProcessing_MouseWheel;
            Calculate.Click += Calculate_Click;
        }

        private void Dropper_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            if(ImageProcessing.IsEnabled)
            {
                if (e.Delta > 0)
                    GammaSlider.Value = Math.Min(GammaSlider.Maximum, GammaSlider.Value + 0.05);
                else
                    GammaSlider.Value = Math.Max(GammaSlider.Minimum, GammaSlider.Value - 0.05);
            }
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            if(MainCalc is not null)
            {
                MainCalc.Dispose();
                MainCalc = null;

                Dropper.Fill = new SolidColorBrush(Colors.WhiteSmoke);

                Logo.Visibility = Visibility.Visible;
                ImageProcessing.IsEnabled = false;


                if (MainTab.Items.Count > 1)
                {
                    Monitor.Enter(MainTab);
                    Int32 _fixed = MainTab.Items.Count;
                    for (Int32 i = _fixed - 1; i > 0; i--)
                        MainTab.Items.Remove(MainTab.Items[i]);
                    Monitor.Exit(MainTab);
                }
            }
        }

        private void SaveAll_Click(object sender, RoutedEventArgs e)
        {
            if(MainCalc is null || MainCalc?.Plastins?.Count == 0)
            { 
                MessageBox.Show("Нет пластинок для сохранения!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            foreach (var item in MainCalc?.Plastins)
            {
                if (item.Corrosion == 0)
                {
                    MessageBox.Show("Все пластинки должны быть расчитаны!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
            }

            foreach (TabItem item in MainTab.Items)
            {
                if (item.Content is PlastinForm form)
                {
                    if(form.InputName.Text == "Введите название" || form.InputName.Text == "")
                    {
                        MessageBox.Show("Все пластинки должны иметь название!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }
                }
            }

            string path;
            var dialog = new Microsoft.Win32.SaveFileDialog();
            dialog.Title = "Место сохранения"; // instead of default "Save As"
            dialog.Filter = "Директории|*.Папка"; // Prevents displaying files
            dialog.FileName = "Выбор"; // Filename will then be "select.this.directory"
            if (dialog.ShowDialog() == true)
            {
                path = dialog.FileName;
                // Remove fake filename from resulting path
                path = path.Replace("\\Выбор.Папка", "");
                path = path.Replace(".Папка", "");
                // If user has changed the filename, create the new directory
                if (!System.IO.Directory.Exists(path))
                {
                    System.IO.Directory.CreateDirectory(path);
                }
            }


        }

        private void OpenFile_Click(object sender, MouseButtonEventArgs e)
        {
            String result = OpenFile();

            if (result is not "")
            {
                NewImageImport(result);

                Dropper.Width = MainCalc.Processed.Width;
                Dropper.MinHeight = MainCalc.Processed.Height;

                ShowArea.MinWidth = Dropper.Width + Dropper.Margin.Left * 2;
                ShowArea.MinHeight = Dropper.MinHeight + Dropper.Margin.Top * 2;

                Dropper.Width = Dropper.ActualWidth * (MainCalc.OriginalRatio / (Dropper.ActualWidth / Dropper.ActualHeight));

                Logo.Visibility = Visibility.Collapsed;
                ImageProcessing.IsEnabled = true;
                GammaSlider.Value = 1;

                Image<Bgr, Byte> inWork = MainCalc.Original.Clone();

                MainCalc.AdjustGamma(inWork, GammaSlider.Value);
                MainCalc.FindPlastins(inWork.Convert<Hsv, Byte>(), MainCalc.Original);

                Dropper.Fill = new ImageBrush(Convert(MainCalc.Processed.ToBitmap(), PixelFormats.Bgr24));

                inWork.Dispose();
            }
        }

        private void OpenFile_Click(object sender, RoutedEventArgs e)
        {
            String result = OpenFile();

            if (result is not "")
            {
                NewImageImport(result);

                Dropper.Width = MainCalc.Processed.Width;
                Dropper.MinHeight = MainCalc.Processed.Height;

                ShowArea.MinWidth = Dropper.Width + Dropper.Margin.Left * 2;
                ShowArea.MinHeight = Dropper.MinHeight + Dropper.Margin.Top * 2;

                Dropper.Width = Dropper.ActualWidth * (MainCalc.OriginalRatio / (Dropper.ActualWidth / Dropper.ActualHeight));

                Logo.Visibility = Visibility.Collapsed;
                ImageProcessing.IsEnabled = true;
                GammaSlider.Value = 1;

                Image<Bgr, Byte> inWork = MainCalc.Original.Clone();

                MainCalc.AdjustGamma(inWork, GammaSlider.Value);
                MainCalc.FindPlastins(inWork.Convert<Hsv, Byte>(), MainCalc.Original);

                Dropper.Fill = new ImageBrush(Convert(MainCalc.Processed.ToBitmap(), PixelFormats.Bgr24));

                inWork.Dispose();
            }
        }

        private void Calculate_Click(object sender, RoutedEventArgs e)
        {
            foreach (var plastin in MainCalc.Plastins)
            {
                if(plastin.Showed is not true)
                {
                    plastin.Showed = true;

                    MainCalc.CropFromOrig(plastin);

                    TabItem newTab = new TabItem();
                    newTab.Header = $"Пластинка №: {plastin.Num}";
                    PlastinForm newPlastinForm = new PlastinForm();

                    newPlastinForm.Top.ValueChanged += (sender, e) => Slider_ValueChanged(sender, e, newPlastinForm, plastin);
                    newPlastinForm.Top.MouseWheel+= Slider_MouseWheel;

                    newPlastinForm.Bottom.ValueChanged += (sender, e) => Slider_ValueChanged(sender, e, newPlastinForm, plastin);
                    newPlastinForm.Bottom.MouseWheel += Slider_MouseWheel;

                    newPlastinForm.Left.ValueChanged += (sender, e) => Slider_ValueChanged(sender, e, newPlastinForm, plastin);
                    newPlastinForm.Left.MouseWheel += Slider_MouseWheel;

                    newPlastinForm.Right.ValueChanged += (sender, e) => Slider_ValueChanged(sender, e, newPlastinForm, plastin);
                    newPlastinForm.Right.MouseWheel += Slider_MouseWheel;

                    newPlastinForm.AreaCalc.Click += (sender, e) => Corrosion_Click(sender, e, newPlastinForm, plastin);

                    newPlastinForm.SaveCalc.Click += (sender, e) => SaveCalc_Click(sender, e, plastin, newPlastinForm.InputName.Text);

                    newPlastinForm.InputName.PreviewMouseDown += (sender, e) => InputName_PreviewMouseDown(sender, e, newPlastinForm.InputName);

                    newPlastinForm.Dropper.MouseWheel += (sender, e) => Dropper_MouseWheel1(sender, e, newPlastinForm, plastin);

                    newPlastinForm.Dropper.Width = MainCalc.Processed.Width;
                    newPlastinForm.Dropper.MinHeight = MainCalc.Processed.Height;

                    newPlastinForm.ShowArea.MinWidth = newPlastinForm.Dropper.Width + newPlastinForm.Dropper.Margin.Left * 2;
                    newPlastinForm.ShowArea.MinHeight = newPlastinForm.Dropper.MinHeight + newPlastinForm.Dropper.Margin.Top * 2;

                    newPlastinForm.Dropper.Width = newPlastinForm.Dropper.ActualWidth * (MainCalc.OriginalRatio / (newPlastinForm.Dropper.ActualWidth / newPlastinForm.Dropper.ActualHeight));
                    newPlastinForm.Dropper.Fill = new ImageBrush(Convert(plastin.resImg.ToBitmap(), PixelFormats.Bgr24));

                    newTab.Content = newPlastinForm;
                    MainTab.Items.Add(newTab);
                }
            }
        }

        private void Dropper_MouseWheel1(object sender, MouseWheelEventArgs e, PlastinForm plastinForm, Plastin plastin)
        {
            if(plastinForm.GroupBorder.IsEnabled)
            {
                if (e.Delta > 0)
                {
                    plastinForm.Left.Value = Math.Min(plastinForm.Left.Maximum, plastinForm.Left.Value + 0.25);
                    plastinForm.Right.Value = Math.Min(plastinForm.Right.Maximum, plastinForm.Right.Value + 0.25);
                    plastinForm.Top.Value = Math.Min(plastinForm.Top.Maximum, plastinForm.Top.Value + 0.25);
                    plastinForm.Bottom.Value = Math.Min(plastinForm.Bottom.Maximum, plastinForm.Bottom.Value + 0.25);
                } else
                {
                    plastinForm.Left.Value = Math.Min(plastinForm.Left.Maximum, plastinForm.Left.Value - 0.25);
                    plastinForm.Right.Value = Math.Min(plastinForm.Right.Maximum, plastinForm.Right.Value - 0.25);
                    plastinForm.Top.Value = Math.Min(plastinForm.Top.Maximum, plastinForm.Top.Value - 0.25);
                    plastinForm.Bottom.Value = Math.Min(plastinForm.Bottom.Maximum, plastinForm.Bottom.Value - 0.25);
                }
            }
        }

        private void InputName_PreviewMouseDown(object sender, MouseButtonEventArgs e, TextBox text)
        {
            if(text.Text == "Введите название")
                text.Text = string.Empty;
        }

        private void SaveCalc_Click(object sender, RoutedEventArgs e, Plastin plastin, String name)
        {
            if(sender is Button save)
            {
                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.InitialDirectory = AppContext.BaseDirectory;
                if(name != "Введите название" && name != String.Empty)
                    saveFileDialog.FileName = $"{name} {plastin.Corrosion :f2}%";
                saveFileDialog.Filter = "Portable network graphics (*.png)|*.png";

                if (saveFileDialog.ShowDialog() == true)
                    plastin.resImgWithCnt?.Save(saveFileDialog.FileName);
            }
        }

        private void Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e, PlastinForm plastinForm, Plastin plastin)
        {

            if(sender is Slider thisSlider)
            {
                plastinForm.SaveCalc.Visibility = Visibility.Collapsed;

                System.Drawing.Rectangle roi = new System.Drawing.Rectangle(
                    (Int32)(plastinForm.Left.Value * plastin.preImg.Width / 100),
                    (Int32)(plastinForm.Top.Value * plastin.preImg.Height / 100),
                    plastin.preImg.Width - ((Int32)(plastinForm.Left.Value * plastin.preImg.Width / 100) + (Int32)(plastinForm.Right.Value * plastin.preImg.Width / 100)),
                    plastin.preImg.Height - ((Int32)(plastinForm.Top.Value * plastin.preImg.Height / 100) + (Int32)(plastinForm.Bottom.Value * plastin.preImg.Height / 100))
                );
                plastin.resImg = plastin.preImg.Copy(roi);
                plastinForm.Dropper.Fill = new ImageBrush(Convert(plastin.resImg.ToBitmap(), PixelFormats.Bgr24));
            

                if((bool) plastinForm.GroupBorder.IsChecked)
                {
                    if(thisSlider != plastinForm.Left)
                        plastinForm.Left.Value = thisSlider.Value;
                    if (thisSlider != plastinForm.Right)
                        plastinForm.Right.Value = thisSlider.Value;
                    if (thisSlider != plastinForm.Top)
                        plastinForm.Top.Value = thisSlider.Value;
                    if (thisSlider != plastinForm.Bottom)
                        plastinForm.Bottom.Value = thisSlider.Value;
                }
            }
        }

        private void Corrosion_Click(object sender, RoutedEventArgs e, PlastinForm plastinForm, Plastin plastin)
        {
            if(sender is Button)
            {
                plastin.AreaCalc();
                plastinForm.Dropper.Fill = new ImageBrush(Convert(plastin.resImgWithCnt.ToBitmap(), PixelFormats.Bgr24));
                plastinForm.OutputText.Text = $"Поражение: {plastin.Corrosion:f2}%";
                plastinForm.SaveCalc.Visibility = Visibility.Visible;
            }
        }

        private void Slider_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            if(sender is Slider slider)
                if (e.Delta > 0)
                    slider.Value = Math.Min(slider.Maximum, slider.Value + 0.25);
                else
                    slider.Value = Math.Max(slider.Minimum, slider.Value - 0.25);
        }

        private void ImageProcessing_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (e.Delta > 0)
                GammaSlider.Value = Math.Min(GammaSlider.Maximum, GammaSlider.Value + 0.05);
            else
                GammaSlider.Value = Math.Max(GammaSlider.Minimum, GammaSlider.Value - 0.05);
        }

        //https://stackoverflow.com/questions/1922040/how-to-resize-an-image-c-sharp
        private void GammaSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (MainTab.Items.Count > 1)
            {
                Monitor.Enter(MainTab);
                Int32 _fixed = MainTab.Items.Count;
                for (Int32 i = _fixed - 1; i > 0; i--)
                    MainTab.Items.Remove(MainTab.Items[i]);
                Monitor.Exit(MainTab);
            }

            Image<Bgr, Byte> inWork = MainCalc.Original.Clone();

            MainCalc.AdjustGamma(inWork, GammaSlider.Value);
            MainCalc.FindPlastins(inWork.Convert<Hsv, Byte>(), MainCalc.Original);

            Dropper.Fill = new ImageBrush(Convert(MainCalc.Processed.ToBitmap(), PixelFormats.Bgr24));

            inWork.Dispose();
        }

        private void MainWindow_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (MainCalc is not null)
            {
                Dropper.Width = Dropper.ActualWidth * (MainCalc.OriginalRatio/(Dropper.ActualWidth / Dropper.ActualHeight));
                foreach (TabItem items in MainTab.Items)
                {
                    if (items.Content is PlastinForm form)
                    {
                        form.Dropper.Width = form.Dropper.ActualWidth * (MainCalc.OriginalRatio / (form.Dropper.ActualWidth / form.Dropper.ActualHeight));
                    }
                }
            }
        }

        private void Dropper_Drop(object sender, DragEventArgs e)
        {
            String result = OpenFile(e);

            if(result is not "")
            {
                NewImageImport(result);

                Dropper.Width = MainCalc.Processed.Width;
                Dropper.MinHeight = MainCalc.Processed.Height;

                ShowArea.MinWidth = Dropper.Width + Dropper.Margin.Left * 2;
                ShowArea.MinHeight = Dropper.MinHeight + Dropper.Margin.Top * 2;

                Dropper.Width = Dropper.ActualWidth * (MainCalc.OriginalRatio / (Dropper.ActualWidth / Dropper.ActualHeight));

                Logo.Visibility = Visibility.Collapsed;
                ImageProcessing.IsEnabled = true;
                GammaSlider.Value = 1;

                Image<Bgr, Byte> inWork = MainCalc.Original.Clone();

                MainCalc.AdjustGamma(inWork, GammaSlider.Value);
                MainCalc.FindPlastins(inWork.Convert<Hsv, Byte>(), MainCalc.Original);

                Dropper.Fill = new ImageBrush(Convert(MainCalc.Processed.ToBitmap(), PixelFormats.Bgr24));

                inWork.Dispose();
            }
        }

        private void NewImageImport(String path)
        {
            if (MainTab.Items.Count > 1)
            {
                Monitor.Enter(MainTab);
                Int32 _fixed = MainTab.Items.Count;
                for (Int32 i = _fixed - 1; i > 0; i--)
                    MainTab.Items.Remove(MainTab.Items[i]);
                Monitor.Exit(MainTab);
            }

            if (MainCalc is null)
                MainCalc = new CorrosionCalculator(path);
            else if (MainCalc.Update(path))
            {

            }
            else
                return;
        }

        public static BitmapSource Convert(System.Drawing.Bitmap bitmap, PixelFormat format)
        {
            var bitmapData = bitmap.LockBits(
                new System.Drawing.Rectangle(0, 0, bitmap.Width, bitmap.Height),
                System.Drawing.Imaging.ImageLockMode.ReadOnly, bitmap.PixelFormat);

            var bitmapSource = BitmapSource.Create(
                bitmapData.Width, bitmapData.Height,
                bitmap.HorizontalResolution, bitmap.VerticalResolution,
                format, null,
                bitmapData.Scan0, bitmapData.Stride * bitmapData.Height, bitmapData.Stride);

            bitmap.UnlockBits(bitmapData);

            return bitmapSource;
        }

        private static String OpenFile()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
            {
                String result = openFileDialog.FileName;

                if (!(result.EndsWith("png", StringComparison.OrdinalIgnoreCase) || result.EndsWith("jpg", StringComparison.OrdinalIgnoreCase) || result.EndsWith("jpeg", StringComparison.OrdinalIgnoreCase)))
                {
                    MessageBox.Show("Неверный формат файла!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return "";
                }
                else return result;
            }
            return "";
        }

        private static String OpenFile(DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                String[] dataPaths = (String[])e.Data.GetData(DataFormats.FileDrop);

                if (dataPaths.Length > 1 || !(dataPaths[0].EndsWith("png", StringComparison.OrdinalIgnoreCase) || dataPaths[0].EndsWith("jpg", StringComparison.OrdinalIgnoreCase) || dataPaths[0].EndsWith("jpeg", StringComparison.OrdinalIgnoreCase)))
                {
                    MessageBox.Show("Неверный формат файла!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return "";
                }
                return dataPaths[0];
            }
            else
                return "";
        }
    }
}
