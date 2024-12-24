using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Dnn;
using Emgu.CV.Reg;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using Microsoft.Win32;
using RustCheck.MVVM.Commands;
using RustCheck.MVVM.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Diagnostics;
using System.DirectoryServices;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using static RustCheck.MVVM.Models.BrushAndImages;

namespace RustCheck.MVVM.ViewModels
{
    internal class ApplicationModelView : BaseModelView
    {
        public static event Action<Boolean, TabItem> TabItemAction;

        public ObservableCollection<TabItem> Tabs { get; private set; }
        

        private PlateExtractor _plateExtractor;
        private BrushAndImages _imageStorage;

        private RelayCommand _closeCommand;
        private RelayCommand _loadClickCommand; 
        private RelayCommand _findPlastinCommand;

        public Double GammaSliderValueShow { get; set; }
        public Double GammaSliderValueSearch { get; set; }
        public Boolean ImageExist
        {
            get => _imageStorage.ImageExist;
        }
        public Int32 ImageWidth
        {
            get => _imageStorage.ImageShowWidth;
        }

        public Int32 ImageHeight
        {
            get => _imageStorage.ImageShowHeight;
        }

        public Visibility LogoMarker
        {
            get => _imageStorage.LogoMarker;
            set => _imageStorage.LogoMarker = value;
        }

        public ImageBrush ImageBrush {
            get => _imageStorage.ImageBrush;
        }

        public Mat ImageMat
        {
            get => _imageStorage.ImageMat;
            set
            {
                GammaSliderValueShow = 1;
                GammaSliderValueSearch = 1;

                Tabs?.Clear();

                _imageStorage.ImageMat = value;
                
                GC.Collect();
                GC.WaitForPendingFinalizers();

                if(ImageExist)
                    FindPlastinCommand.Execute(null);

                BindingUpdate();
            }
        }

        public ApplicationModelView()
        {
            GammaSliderValueShow = 1;
            GammaSliderValueSearch = 1;

            _imageStorage = new BrushAndImages();
            _plateExtractor = new PlateExtractor();
            Tabs = new ObservableCollection<TabItem>();

            Tabs.CollectionChanged += TabsCollectionChanged;
        }

        public void LoadImage(object sender, DragEventArgs e)
        {
            if(e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                String[] paths = (String[]) e.Data.GetData(DataFormats.FileDrop);
                LoadFromPath(paths[0]);
            }
        }

        public void MainImageGammaChange(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if(ImageExist)
            {
                _imageStorage.AdjustGamma(BrushAndImages.TargetImage.ResizedOriginal, GammaSliderValueShow);

                Mat matWithCnt = _imageStorage.ImageMatInWorkToShow.Clone();

                foreach (var item in _plateExtractor.Plates)
                {
                    CvInvoke.DrawContours(matWithCnt, new VectorOfVectorOfPoint(item.BorderContour), -1, new MCvScalar(0, 255, 0), 1);
                    CvInvoke.Circle(matWithCnt, item.Center, 15, new MCvScalar(0, 255, 0), -1);
                    CvInvoke.PutText(matWithCnt, item.Num.ToString(), item.CenterForNum, FontFace.HersheySimplex, 1, new MCvScalar(255, 255, 255), 2);
                }

                _imageStorage.ImageBrushUpdate(matWithCnt);
            }
        }

        public RelayCommand CloseCommand
        {
            get
            {
                return _closeCommand ?? (
                    _closeCommand = new RelayCommand(obj =>
                    {
                        _plateExtractor.Clear();
                        Tabs?.Clear();
                        ImageMat = null;
                    }));
            }
        }

        public RelayCommand LoadClickCommand
        {
            get
            {
                return _loadClickCommand ?? (
                    _loadClickCommand = new RelayCommand(obj =>
                    {
                        OpenFileDialog openFileDialog = new OpenFileDialog();
                        if (openFileDialog.ShowDialog() == true)
                        {
                            String result = openFileDialog.FileName;
                            LoadFromPath(result);
                        }
                    }));
            }
        }

        public RelayCommand FindPlastinCommand
        {
            get
            {
                return _findPlastinCommand ?? (
                    _findPlastinCommand = new RelayCommand(obj =>
                    {
                        if (ImageExist)
                        {
                            Tabs?.Clear();
                            _imageStorage.AdjustGamma(BrushAndImages.TargetImage.ResizedSearch, GammaSliderValueSearch);

                            _plateExtractor.Clear();
                            _plateExtractor.FindAllPlates(_imageStorage.ImageMatInWorkToSearch);

                            Mat matWithCnt = _imageStorage.ImageMatInWorkToShow.Clone();

                            foreach (var item in _plateExtractor.Plates)
                            {
                                CvInvoke.DrawContours(matWithCnt, new VectorOfVectorOfPoint(item.BorderContour), -1, new MCvScalar(0, 255, 0), 1);
                                CvInvoke.Circle(matWithCnt, item.Center, 15, new MCvScalar(0, 255, 0), -1);
                                CvInvoke.PutText(matWithCnt, item.Num.ToString(), item.CenterForNum, FontFace.HersheySimplex, 1, new MCvScalar(255, 255, 255), 2);
                            }

                            _imageStorage.ImageBrushUpdate(matWithCnt);
                        }
                    }));
            }
        }

        private void LoadFromPath(String path)
        {

            if (!(path.EndsWith("png", StringComparison.OrdinalIgnoreCase) || path.EndsWith("jpg", StringComparison.OrdinalIgnoreCase) || path.EndsWith("jpeg", StringComparison.OrdinalIgnoreCase)))
            {
                MessageBox.Show("Неверный формат файла!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            using (FileStream stream = new FileStream(path, FileMode.Open))
            {
                using (var bitmap = new Bitmap(stream))
                {
                    ImageMat = bitmap.ToMat();
                }
            }
        }

        public void CompileNewPlastinsForm(object sender, RoutedEventArgs e)
        {
            _imageStorage.AdjustGamma(TargetImage.Original, GammaSliderValueShow);
            _plateExtractor.PlatesImageUpdate(_imageStorage._imageMatOriginal, _imageStorage.ImageMat.Size);

            foreach (var plate in _plateExtractor.Plates)
            {
                if (plate.Showed is not true)
                {
                    plate.Showed = true;

                    TabItem newTab = new TabItem();
                    newTab.Header = $"Пластинка №: {plate.Num}";

                    PlastinForm newPlastinForm = new PlastinForm();

                    newPlastinForm.DataContext = new PlateModelView(plate);

                    newTab.Content = newPlastinForm;

                    Tabs.Add(newTab);
                }
            }
        }

        void TabsCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            switch(e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    if (e.NewItems?[0] is TabItem newTab)
                        TabItemAction(true, newTab);
                    break;
                case NotifyCollectionChangedAction.Remove:
                    if (e.OldItems?[0] is TabItem oldTab)
                        TabItemAction(false, oldTab);
                    break;
                case NotifyCollectionChangedAction.Reset:
                    TabItemAction(false, null);
                    break;
            }
        }

        private void BindingUpdate()
        {
            OnPropertyChanged(nameof(ImageBrush));
            OnPropertyChanged(nameof(ImageWidth));
            OnPropertyChanged(nameof(ImageHeight));
            OnPropertyChanged(nameof(LogoMarker));
            OnPropertyChanged(nameof(ImageExist));
            OnPropertyChanged(nameof(GammaSliderValueShow));
        }
    }
}
