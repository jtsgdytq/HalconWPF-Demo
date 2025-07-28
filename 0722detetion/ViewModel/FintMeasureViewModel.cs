using _0722detetion.Models;
using HalconDotNet;
using System.Collections.ObjectModel;

namespace _0722detetion.ViewModel;

public class FintMeasureViewModel:BindableBase
{

    public FintMeasureViewModel()
    {
        StartCommand = new DelegateCommand(Run);
    }

    private ObservableCollection<FitMeasureInfo> infos;

    public ObservableCollection<FitMeasureInfo> Infos
    {
        get { return infos; }
        set { infos = value;  RaisePropertyChanged(); }
    }


    public HWindow hWindow { get; set; }

    public DelegateCommand StartCommand { get; set; }


    private void Run()
    {
        Task.Run(() => {
            Measurre();
        } );
    }
    /// <summary>
    /// Analyzes an image to detect and measure geometric features such as circles and lines.
    /// </summary>
    /// <remarks>This method processes an image to identify contours, classify them as either circular or
    /// linear, and then calculates their respective measurements. Detected circles are displayed with their radii,
    /// while detected lines are displayed with their lengths. The results are visualized on the provided display window
    /// and stored for further use.</remarks>
    private void Measurre()
    {
        HObject ho_Image, ho_Edges, ho_ContoursSplit;
        HObject ho_SortedContours, ho_ObjectSelected = null, ho_ContEllipse = null;
        HObject ho_Line = null;
        HTuple hv_Width = new HTuple(), hv_Height = new HTuple();
        HTuple hv_WindowID = new HTuple(), hv_Number = new HTuple();
        HTuple hv_i = new HTuple(), hv_Attrib = new HTuple(), hv_Row = new HTuple();
        HTuple hv_Column = new HTuple(), hv_Radius = new HTuple();
        HTuple hv_StartPhi = new HTuple(), hv_EndPhi = new HTuple();
        HTuple hv_PointOrder = new HTuple(), hv_RowBegin = new HTuple();
        HTuple hv_ColBegin = new HTuple(), hv_RowEnd = new HTuple();
        HTuple hv_ColEnd = new HTuple(), hv_Nr = new HTuple();
        HTuple hv_Nc = new HTuple(), hv_Dist = new HTuple(), hv_Length = new HTuple();


      
        HOperatorSet.ReadImage(out ho_Image, "metal-parts/metal-parts-01");
        HOperatorSet.DispObj(ho_Image, hWindow);

        HOperatorSet.GetImageSize(ho_Image, out hv_Width, out hv_Height);


        HOperatorSet.EdgesSubPix(ho_Image, out ho_Edges, "lanser2", 0.5, 40, 90);

        
        HOperatorSet.SegmentContoursXld(ho_Edges, out ho_ContoursSplit, "lines_circles",
            6, 4, 4);
        
        HOperatorSet.SortContoursXld(ho_ContoursSplit, out ho_SortedContours, "upper_left",
            "true", "row");

        hv_Number.Dispose();
        HOperatorSet.CountObj(ho_SortedContours, out hv_Number);

        HTuple end_val22 = hv_Number;
        HTuple step_val22 = 1;
        for (hv_i = 1; hv_i.Continue(end_val22, step_val22); hv_i = hv_i.TupleAdd(step_val22))
        {

            HOperatorSet.SelectObj(ho_SortedContours, out ho_ObjectSelected, hv_i);

         
            HOperatorSet.GetContourGlobalAttribXld(ho_ObjectSelected, "cont_approx",
                out hv_Attrib);
            if ((int)(new HTuple(hv_Attrib.TupleEqual(1))) != 0)
            {

               

                HOperatorSet.FitCircleContourXld(ho_ObjectSelected, 
                    "atukey", 
                    -1,
                    2,
                    0,
                    5,
                    2, 
                    out hv_Row, 
                    out hv_Column,
                    out hv_Radius,
                    out hv_StartPhi,
                    out hv_EndPhi,
                    out hv_PointOrder);
                using (HDevDisposeHelper dh = new HDevDisposeHelper())
                {
                   
                    HOperatorSet.GenEllipseContourXld(out ho_ContEllipse,
                        hv_Row, hv_Column,
                        0, 
                        hv_Radius, 
                        hv_Radius,
                        0, 
                        (new HTuple(360)).TupleRad(), 
                        "positive",
                        1.0);
                }
               
                    HOperatorSet.SetColor(hWindow, "white");
                
                
                    HOperatorSet.DispObj(ho_ContEllipse, hWindow);
                
                HOperatorSet.SetTposition(hWindow, hv_Row, hv_Column);
                using (HDevDisposeHelper dh = new HDevDisposeHelper())
                {
                    HOperatorSet.WriteString(hWindow, "R" + hv_i);
                }

                AddInfo("R" + hv_i, hv_Radius.D);

            }
            else
            {

                
                HOperatorSet.FitLineContourXld(ho_ObjectSelected, "tukey", -1, 0, 5, 2,
                    out hv_RowBegin, 
                    out hv_ColBegin, 
                    out hv_RowEnd, 
                    out hv_ColEnd, 
                    out hv_Nr,
                    out hv_Nc, 
                    out hv_Dist);
                using (HDevDisposeHelper dh = new HDevDisposeHelper())
                {
                  
                    HOperatorSet.GenContourPolygonXld(out ho_Line, hv_RowBegin.TupleConcat(
                        hv_RowEnd), hv_ColBegin.TupleConcat(hv_ColEnd));
                }
               
                    HOperatorSet.SetColor(hWindow, "yellow");
               
                    HOperatorSet.DispObj(ho_Line,hWindow);
                
               
                HOperatorSet.DistancePp(hv_RowBegin, hv_ColBegin, hv_RowEnd, hv_ColEnd,
                    out hv_Length);

                using (HDevDisposeHelper dh = new HDevDisposeHelper())
                {
                    HOperatorSet.SetTposition(hWindow, ((hv_RowBegin + hv_RowEnd) / 2) - (hv_Nr * 10),
                        (hv_ColBegin + hv_ColEnd) / 2);
                }
                using (HDevDisposeHelper dh = new HDevDisposeHelper())
                {
                    HOperatorSet.WriteString(hWindow, "L" + hv_i);
                }
                AddInfo("L" + hv_i, hv_Length.D);

            }

        }


    }

    /// <summary>
    ///  添加测量信息
    /// </summary>
    /// <param name="name"></param>
    /// <param name="value"></param>

    private void AddInfo(string name, double value)
    {
        if (Infos == null)
        {
            Infos = new ObservableCollection<FitMeasureInfo>();
        }

        value = Math.Round(value, 2);
        App.Current.Dispatcher.Invoke(() =>
        {
            Infos.Add(new FitMeasureInfo() { Name = name, Value = value });
        });
    }
}