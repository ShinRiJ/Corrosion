using Emgu.CV;
using RustCheck.MVVM.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Interop;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using System.Windows;
using System.Windows.Controls;
using Emgu.CV.Structure;
using RustCheck.MVVM.Commands;

namespace RustCheck.MVVM.ViewModels
{
    internal class PlateModelView : BaseModelView
    {
        private Boolean _groupCheck;
        private Boolean _groupBlock;

        private Int32 _imageWidth;
        private Int32 _imageHeight;

        private Double _leftSliderValue;
        private Double _rightSliderValue;
        private Double _topSliderValue;
        private Double _bottomSliderValue;

        private RelayCommand _calcclickCommand;
        private String _outputText;

        public String OutputText
        {
            get => _outputText;
            set
            {
                _outputText = value;
                OnPropertyChanged(nameof(OutputText));
            }
        }

        public Double LeftSliderValue
        {
            get => _leftSliderValue;
            set
            {
                _leftSliderValue = value;
                OnPropertyChanged(nameof(LeftSliderValue));
            }
        }
        public Double RightSliderValue
        {
            get => _rightSliderValue;
            set
            {
                _rightSliderValue = value;
                OnPropertyChanged(nameof(RightSliderValue));
            }
        }
        public Double TopSliderValue
        {
            get => _topSliderValue;
            set
            {
                _topSliderValue = value;
                OnPropertyChanged(nameof(TopSliderValue));
            }
        }
        public Double BottomSliderValue
        {
            get => _bottomSliderValue;
            set
            {
                _bottomSliderValue = value;
                OnPropertyChanged(nameof(BottomSliderValue));
            }
        }

        public Int32 ImageWidth
        {
            get => _imageWidth;
            set
            {
                _imageWidth = value;
                OnPropertyChanged(nameof(ImageWidth));
            }
        }

        public Int32 ImageHeight
        {
            get => _imageHeight;
            set
            {
                _imageHeight = value;
                OnPropertyChanged(nameof(ImageHeight));
            }
        }

        public Boolean GroupCheck
        {
            get => _groupCheck;
            set
            {
                _groupCheck = value;
                OnPropertyChanged(nameof(GroupCheck));
            }
        }

        public Plate ModelPlate { get; set; }
        public ImageBrush ImageBrush
        {
            get;
            init;
        }

        public PlateModelView(Plate inputPlate)
        {
            _groupBlock = true;

            ModelPlate = inputPlate;

            ImageWidth = 640;
            ImageHeight = 640;

            ImageBrush = new ImageBrush();

            LeftSliderValue = RightSliderValue = TopSliderValue = BottomSliderValue = 0;
            GroupCheck = true;

            ImageBrushUpdate();
        }

        private void ImageBrushUpdate()
        {
            Image<Bgr, Byte> result = new Image<Bgr, byte>(ModelPlate.ImageToShow.Size);
            ModelPlate.ImageToShow.CopyTo(result);

            if (ModelPlate.InterContour is not null)
                CvInvoke.DrawContours(result, ModelPlate.InterContour, -1, new MCvScalar(0, 255, 0), 1);

            IntPtr hBitmap = result.ToBitmap().GetHbitmap();

            try
            {
                ImageBrush.ImageSource = Imaging.CreateBitmapSourceFromHBitmap(
                                                hBitmap,
                                                IntPtr.Zero,
                                                Int32Rect.Empty,
                                                BitmapSizeOptions.FromEmptyOptions()
                                                );
            }
            finally
            {
                RustCheck.MainWindow.DeleteObject(hBitmap);
            }

            OnPropertyChanged(nameof(ImageBrush));
        }

        public RelayCommand CalcclickCommand
        {
            get
            {
                return _calcclickCommand ?? (
                    _calcclickCommand = new RelayCommand(obj =>
                    {
                        ModelPlate.AreaCalc();
                        OutputText = $"Поражение: {ModelPlate.Corrosion:f2}%";
                        ImageBrushUpdate();
                    }));
            }
        }

        public void Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (GroupCheck && _groupBlock)
            {
                _groupBlock = false;
                if (sender is Slider slider)
                {
                    LeftSliderValue = RightSliderValue = TopSliderValue = BottomSliderValue = slider.Value;
                    ModelPlate.CropOriginal(LeftSliderValue, TopSliderValue, RightSliderValue, BottomSliderValue);
                    _groupBlock = true;
                }
            }
            else if (GroupCheck && !_groupBlock)
                return;
            else if (!GroupCheck)
            {
                ModelPlate.CropOriginal(LeftSliderValue, TopSliderValue, RightSliderValue, BottomSliderValue);
            }

            ImageBrushUpdate();
        }
    }
}
