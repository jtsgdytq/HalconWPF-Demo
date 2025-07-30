using HalconDotNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _0722detetion.Models
{
    public class BottlesParamModel
    {
        // HObject 变量
        public HObject ho_Image, ho_Regions, ho_DarkRegion, ho_RegionOpening;
        public HObject ho_RegionClosing, ho_RegionFillUp, ho_RegionBorder;
        public HObject ho_RegionDilation, ho_ImageReduced, ho_Edges;
        public HObject ho_ContoursSplit, ho_UnionContours, ho_LongestContour;
        public HObject ho_Circle, ho_RegionErosion, ho_RegionDifference;
        public HObject ho_ImagePolar, ho_ImageScaleMax, ho_ImageMean;
        public HObject ho_Regions1, ho_Connection, ho_SelectedRegions;
        public HObject ho_RegionClosing1, ho_RegionUnion, ho_XYTransRegion;
        public HObject ho_ImageRotate, ho_RegionMirror;

        // HTuple 变量
        public HTuple hv_SmoothX, hv_ThresholdOffset, hv_MinDefectSize;
        public HTuple hv_PolarResolution, hv_RingSize, hv_StoreEmptyRegion;
        public HTuple hv_WindowHandle1, hv_WindowHandle;
        public HTuple hv_Index, hv_Length, hv_Row, hv_Column;
        public HTuple hv_Radius, hv_StartPhi, hv_EndPhi, hv_PointOrder;
        public HTuple hv_Number;
        //均值滤波窗口宽度

        public BottlesParamModel()
        {
            // HObject 初始化
            ho_Image = ho_Regions = ho_DarkRegion = ho_RegionOpening = null;
            ho_RegionClosing = ho_RegionFillUp = ho_RegionBorder = null;
            ho_RegionDilation = ho_ImageReduced = ho_Edges = null;
            ho_ContoursSplit = ho_UnionContours = ho_LongestContour = null;
            ho_Circle = ho_RegionErosion = ho_RegionDifference = null;
            ho_ImagePolar = ho_ImageScaleMax = ho_ImageMean = null;
            ho_Regions1 = ho_Connection = ho_SelectedRegions = null;
            ho_RegionClosing1 = ho_RegionUnion = ho_XYTransRegion = null;
            ho_ImageRotate = ho_RegionMirror = null;

            // HTuple 初始化并赋值
            hv_SmoothX = 501;
            hv_ThresholdOffset = 55;
            hv_MinDefectSize = 9;
            hv_PolarResolution = 640;
            hv_RingSize = 70;

            hv_StoreEmptyRegion = hv_WindowHandle1 = hv_WindowHandle = new HTuple();
            hv_Index = hv_Length = hv_Row = hv_Column = new HTuple();
            hv_Radius = hv_StartPhi = hv_EndPhi = hv_PointOrder = new HTuple();
            hv_Number = new HTuple();
        }
    }
}
