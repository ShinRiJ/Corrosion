using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;

namespace RustCheck.MVVM.Models
{
    internal class PlateExtractor
    {
        public List<Plate> Plates;

        public PlateExtractor()
        {
            Plates = new List<Plate>();
        }

        public void Clear()
        {
            for(Int32 i = 0; i < Plates.Count; i++)
                Plates[i].Clear();
            Plates.Clear();
        }

        public void FindAllPlates(Mat src)
        {
            using (Mat hsvImage = new Mat())
            {
                CvInvoke.CvtColor(src, hsvImage, ColorConversion.Bgr2Hsv);

                using (Image<Hsv, Byte> imageHsv = new Image<Hsv, Byte>(hsvImage))
                {

                    Hsv min = new Hsv(0, 20, 0);
                    Hsv max = new Hsv(179, 255, 170);

                    Image<Gray, Byte> Mask = imageHsv.InRange(min, max);
                    Mask = Mask.AbsDiff(new Gray(255));

                    Mat kernel = CvInvoke.GetStructuringElement(ElementShape.Rectangle, new Size(3, 3), new Point(-1, -1));

                    CvInvoke.Erode(Mask, Mask, kernel, new Point(-1, -1), 2, BorderType.Default, new MCvScalar(0));
                    CvInvoke.Dilate(Mask, Mask, kernel, new Point(-1, -1), 2, BorderType.Default, new MCvScalar(0));

                    VectorOfVectorOfPoint contours = new VectorOfVectorOfPoint();
                    Mat hierarchy = new Mat();

                    CvInvoke.FindContours(Mask, contours, hierarchy, RetrType.Tree, ChainApproxMethod.ChainApproxSimple);

                    var _hierarchy = (Int32[,,])hierarchy.GetData();

                    for (Int32 index = 0; index < contours.Size; index++)
                    {
                        if (CvInvoke.ContourArea(contours[index]) > 8000 && _hierarchy[0, index, 3] != -1)
                        {
                            Rectangle boundingBox = CvInvoke.BoundingRectangle(contours[index]);
                            double boundingBoxArea = boundingBox.Width * boundingBox.Height;

                            if (CvInvoke.ContourArea(contours[index]) / boundingBoxArea > 0.7)
                            {
                                VectorOfPoint buffer = new VectorOfPoint();
                                CvInvoke.ApproxPolyDP(contours[index], buffer, 0.05 * CvInvoke.ArcLength(contours[index], true), true);

                                if (buffer.Size == 4)
                                {
                                    Plates.Add(new Plate(buffer));
                                }
                                else
                                {
                                    RotatedRect minAreaRect = CvInvoke.MinAreaRect(contours[index]);
                                    PointF[] boxPoints = CvInvoke.BoxPoints(minAreaRect);
                                    Point[] _buf = new Point[boxPoints.Length];

                                    for (Int32 i = 0; i < boxPoints.Length; i++)
                                        _buf[i] = new Point((Int32)boxPoints[i].X, (Int32)boxPoints[i].Y);

                                    Plates.Add(new Plate(buffer));
                                }
                            }
                        }
                    }

                    Plates.Sort((pl1, pl2) => (pl1.Center.Y * 5 + pl1.Center.X).CompareTo(pl2.Center.Y * 5 + pl2.Center.X));

                    for (Int32 i = 0; i < Plates.Count; i++)
                        Plates[i].Num = i + 1;
                }
            }
        }

        public void PlatesImageUpdate(Mat OriginalForCropping, Size changedSize)
        {
            for (int i = 0; i < Plates.Count; i++)
                CropFromOrig(Plates[i], OriginalForCropping, changedSize);
        }

        public void CropFromOrig(Plate plst, Mat OriginalForCropping, Size changedSize)
        {
            Image<Gray, Byte> mask = new Image<Gray, Byte>(OriginalForCropping.Size);
            Image<Bgr, Byte> result = new Image<Bgr, Byte>(OriginalForCropping.Size);

            Double scaleX = (Double)OriginalForCropping.Width / changedSize.Width;
            Double scaleY = (Double)OriginalForCropping.Height / changedSize.Height;

            Point[] scaledPoints = new Point[plst.BorderContour.Size];

            for (Int32 i = 0; i < plst.BorderContour.Size; i++)
                scaledPoints[i] = new Point((Int32)(plst.BorderContour[i].X * scaleX), (Int32)(plst.BorderContour[i].Y * scaleY));

            Int32 minX = scaledPoints.Min(p => p.X);
            Int32 minY = scaledPoints.Min(p => p.Y);
            Int32 maxX = scaledPoints.Max(p => p.X);
            Int32 maxY = scaledPoints.Max(p => p.Y);

            SortPointsClockwise(scaledPoints);

            Int32 width = maxX - minX;
            Int32 height = maxY - minY;

            Double expansionFactor = 0.1f;

            Int32 expandX = (Int32)(width * expansionFactor);
            Int32 expandY = (Int32)(height * expansionFactor);

            Point[] expandedPoints = new Point[scaledPoints.Length];

            expandedPoints[0] = new Point(scaledPoints[0].X - expandX / 2, scaledPoints[0].Y - expandY / 2);
            expandedPoints[1] = new Point(scaledPoints[1].X + expandX / 2, scaledPoints[1].Y - expandY / 2);
            expandedPoints[2] = new Point(scaledPoints[2].X + expandX / 2, scaledPoints[2].Y + expandY / 2);
            expandedPoints[3] = new Point(scaledPoints[3].X - expandX / 2, scaledPoints[3].Y + expandY / 2);

            CvInvoke.DrawContours(mask, new VectorOfVectorOfPoint(new VectorOfPoint(expandedPoints)), -1, new MCvScalar(255, 255, 255), -1);

            OriginalForCropping.CopyTo(result, mask);

            double angle = Math.Atan2(expandedPoints[1].Y - expandedPoints[0].Y, expandedPoints[1].X - expandedPoints[0].X) * (180.0 / Math.PI);

            result = result.GetSubRect(new System.Drawing.Rectangle(minX - expandX / 2, minY - expandY / 2, width + expandX, height + expandY));

            Mat rotationMatrix = new Mat();
            CvInvoke.GetRotationMatrix2D(new PointF(result.Width / 2f, result.Height / 2f), angle, 1.0, rotationMatrix);
            CvInvoke.WarpAffine(result, result, rotationMatrix, result.Size);

            plst.ImageOriginal = result.Clone();
            result.Dispose();
        }

        private void SortPointsClockwise(Point[] points)
        {
            float centerX = 0;
            float centerY = 0;

            foreach (var point in points)
            {
                centerX += point.X;
                centerY += point.Y;
            }

            centerX /= points.Length;
            centerY /= points.Length;

            Array.Sort(points, (p1, p2) =>
            {
                double angle1 = Math.Atan2(p1.Y - centerY, p1.X - centerX);
                double angle2 = Math.Atan2(p2.Y - centerY, p2.X - centerX);
                return angle1.CompareTo(angle2);
            });
        }

    }
}
