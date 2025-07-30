using _0722detetion.Models;
using HalconDotNet;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;

namespace _0722detetion.ViewModel
{
    public class BottleDefectsViewModel : BindableBase
    {
        public HSmartWindowControlWPF hsmart_1 { get; set; }
        public HSmartWindowControlWPF hsmart_2 { get; set; }
        public BottlesParamModel bottles;

        private bool isAutoStart;
        public string[] imageFiles = null;

        public bool IsAutoStart
        {
            get { return isAutoStart; }
            set { isAutoStart = value;  RaisePropertyChanged(); }
        }



        public BottleDefectsViewModel(BottlesParamModel _bottlesParam)
        {
            bottles = _bottlesParam;
            AutoStartCommand = new DelegateCommand(AutoStart);
            IsAutoStart= true; // 默认开启自动启动
            ReadImageFile(); // 读取图片文件
        }


        public DelegateCommand AutoStartCommand { get; set; }

        public void AutoStart()
        {
                
            if(IsAutoStart==true)
            {
               Task.Run(() =>
                {
                    string imagePath = @"C:\halcon\HALCON-22.05-Progress\examples\images\bottles\bottle_mouth_03";
                    HOperatorSet.ReadImage(out HObject hObject, imagePath);

                    HObject resultimage;
                    resultimage = DealImage(hObject);
                    //HOperatorSet.GetImageSize(resultimage, out HTuple hv_Width, out HTuple hv_Height);
                    //hsmart_1.HalconWindow.SetPart(0, 0, (int)hv_Height - 1, (int)hv_Width - 1);
                    Application.Current.Dispatcher.Invoke(() => {

                        HOperatorSet.DispObj(hObject,hsmart_1.HalconWindow);
                        hsmart_2.HalconWindow.SetColor("red");
                        HOperatorSet.DispObj(hObject,hsmart_2.HalconWindow);
                        HOperatorSet.DispObj(resultimage,hsmart_2.HalconWindow);

                       });
                });
            }
        }
        /// <summary>
        /// 读取图片文件
        /// </summary>
        public void ReadImageFile()
        {
            string filePath = @"C:\Users\Seeney\Desktop\Bottles\";
            string[] extensions = { "*.jpg", "*.jpeg", "*.png", "*.bmp", "*.tiff" };
            imageFiles = extensions.SelectMany(ext => System.IO.Directory.GetFiles(filePath, ext)).ToArray();
        } 
      

        private HObject DealImage(HObject image)
        {
            
            
          
            hsmart_1.HalconWindow.ClearWindow();
           

            HOperatorSet.AutoThreshold(image, out bottles.ho_Regions, 2);
            //通过自动阈值，选择最黑的区域
            
            HOperatorSet.SelectObj(bottles.ho_Regions, out bottles.ho_DarkRegion, 1);
         
            HOperatorSet.OpeningCircle(bottles.ho_DarkRegion, out bottles.ho_RegionOpening, 3.5);

            HOperatorSet.ClosingCircle(bottles.ho_RegionOpening, out bottles.ho_RegionClosing, 25.5);
            //填充区域
        
            HOperatorSet.FillUp(bottles.ho_RegionClosing, out bottles.ho_RegionFillUp);
            //找到边界
     
            HOperatorSet.Boundary(bottles.ho_RegionFillUp, out bottles.ho_RegionBorder, "outer");
            //膨胀边界
         
            HOperatorSet.DilationCircle(bottles.ho_RegionBorder, out bottles.ho_RegionDilation, 3.5);
            //取图像的边缘，其实取的是Image 和RegionDilation的相同的部分
     ;
            HOperatorSet.ReduceDomain(image, bottles.ho_RegionDilation, out bottles.ho_ImageReduced
                );
            //
            //通过为提取的边缘拟合一个圆来查找瓶中心
            //canny边缘检测
       
            HOperatorSet.EdgesSubPix(bottles.ho_ImageReduced, out bottles.ho_Edges, "canny", 0.5, 20,
                40);
            //打散成圆弧和直线
        
            HOperatorSet.SegmentContoursXld(bottles.ho_Edges, out bottles.ho_ContoursSplit, "lines_circles",
                5, 4, 2);
            //找出一个共同圆的部分
        
            HOperatorSet.UnionCocircularContoursXld(bottles.ho_ContoursSplit, out bottles.ho_UnionContours,
                0.9, 0.5, 0.5, 200, 50, 50, "true", 1);
            //计算轮廓或者多边形的长度
        
            HOperatorSet.LengthXld(bottles.ho_UnionContours, out bottles.hv_Length);

            HOperatorSet.SelectObj(bottles.ho_UnionContours, out bottles.ho_LongestContour, ((new HTuple(bottles.hv_Length.TupleSortIndex()
            )).TupleSelect((new HTuple(bottles.hv_Length.TupleLength())) - 1)) + 1);

            HOperatorSet.FitCircleContourXld(bottles.ho_LongestContour, "ahuber", -1, 0, 0, 3,
            2, out bottles.hv_Row, out bottles.hv_Column, out bottles.hv_Radius, out bottles.hv_StartPhi, out bottles.hv_EndPhi,
            out bottles.hv_PointOrder);
            //
            //Part 2:圆形带变换成长方形 Transform the ring-shaped bottle neck region to a rectangle
            //创建一个圆
            
            HOperatorSet.GenCircle(out bottles.ho_Circle, bottles.hv_Row, bottles.hv_Column, bottles.hv_Radius);
            //膨胀圆
           
            HOperatorSet.DilationCircle(bottles.ho_Circle, out bottles.ho_RegionDilation, 5);
            //腐蚀圆
             
                HOperatorSet.ErosionCircle(bottles.ho_Circle, out bottles.ho_RegionErosion, bottles.hv_RingSize - 5);
            
            //做减法
            HOperatorSet.Difference(bottles.ho_RegionDilation, bottles.ho_RegionErosion, out bottles.ho_RegionDifference
                );
            //把带状区域的图像找出来
            
            HOperatorSet.ReduceDomain(image, bottles.ho_RegionDifference, out bottles.ho_ImageReduced
                );
            //极坐标到笛卡尔坐标转换
            // 极坐标到笛卡尔坐标转换
            double angleEnd = (360.0 * Math.PI) / 180.0;

            // 确保 RadiusStart 参数的有效性
            HTuple radiusStart = bottles.hv_Radius - bottles.hv_RingSize;
            HTuple radiusEnd = bottles.hv_Radius;

            // 检查并修正参数
            if (radiusStart < 0)
                radiusStart = 0;
            if (radiusStart >= radiusEnd)
                radiusStart = radiusEnd - 1;

            // 执行极坐标变换
            HOperatorSet.PolarTransImageExt(
                bottles.ho_ImageReduced,
                out bottles.ho_ImagePolar,
                bottles.hv_Row,
                bottles.hv_Column,
                0,                    // AngleStart
                angleEnd,            // AngleEnd  
                radiusStart,         // RadiusStart (修正后的值)
                radiusEnd,           // RadiusEnd
                bottles.hv_PolarResolution,
                bottles.hv_RingSize,
                "nearest_neighbor"
            );
            //
            //Part 3: 通过动态阈值打到缺陷位置

            //scale_image_max应该是指正则化，找到图像最亮和最暗，然后把图像像素拉成0-255

            HOperatorSet.ScaleImageMax(bottles.ho_ImagePolar, out bottles.ho_ImageScaleMax);
            //使用平均平滑图像，后面两个参数应该是平均窗口的大小，但是为什么长度设置为501高为3呢，可能是因为垂直方向上差异大，想要找出垂直方向差异
           
            HOperatorSet.MeanImage(bottles.ho_ImageScaleMax, out bottles.ho_ImageMean, bottles.hv_SmoothX, 3);
            //使用本地的图像动态阈值，然后分割imageSacleMax,Offset越大，找到的区域越小，因为容差大了，大于这个容差的就少了
            
            HOperatorSet.DynThreshold(bottles.ho_ImageScaleMax, bottles.ho_ImageMean, out bottles.ho_Regions1,
                bottles.hv_ThresholdOffset, "not_equal");
            //计算连通区域
           
            HOperatorSet.Connection(bottles.ho_Regions1, out bottles.ho_Connection);
            //选择高度在9~9999之间的连通区域
            
            HOperatorSet.SelectShape(bottles.ho_Connection, out bottles.ho_SelectedRegions, "height",
                "and", bottles.hv_MinDefectSize, 99999);
            //ignore noise regions
            
            HOperatorSet.ClosingRectangle1(bottles.ho_SelectedRegions, out bottles.ho_RegionClosing1,
                10, 20);
            
            HOperatorSet.Union1(bottles.ho_RegionClosing1, out bottles.ho_RegionUnion);
            //re-transform defect regions for visualization
            //把带状反变换为极坐标图像

            // 计算并验证半径参数
            HTuple radiusStart1 = bottles.hv_Radius - bottles.hv_RingSize;
            HTuple radiusEnd1 = bottles.hv_Radius;

            // 确保 RadiusStart 的有效性
            if (radiusStart1 < 0)
                radiusStart1 = 0;
            if (radiusStart1 >= radiusEnd1)
                radiusStart1 = radiusEnd1 - 1;

            // 执行极坐标区域逆变换
            HOperatorSet.PolarTransRegionInv(
                bottles.ho_RegionUnion,
                out bottles.ho_XYTransRegion,
                bottles.hv_Row,
                bottles.hv_Column,
                0,                                    // AngleStart
                (new HTuple(360)).TupleRad(),        // AngleEnd
                radiusStart1,                         // RadiusStart (修正后)
                radiusEnd1,                           // RadiusEnd
                bottles.hv_PolarResolution,
                bottles.hv_RingSize,
                1280,                                // Width
                1024,                                // Height
                "nearest_neighbor"
            );
            HObject result = bottles.ho_XYTransRegion;

            return result;
        }

    }
}
