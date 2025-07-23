using HalconDotNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace _0722detetion.ViewModel
{
    public class ScarachDetectionViewModel : BindableBase
    {
        public readonly string operationstart = "Operation Start";
        public readonly string operationend = "Operation End";
        /// <summary>
        /// 由View加载时设置,得到halcon窗口句柄
        /// </summary>
        public HWindow hImage { get; set; }
        public HWindow hResult { get; set; }

        private CancellationTokenSource cancellationTokenSource;
        private Task halcontask;


        public ScarachDetectionViewModel()
        {
            Operation = operationstart;
            RunCommnd = new DelegateCommand(Run);
            LoadImageFiles();//加载图片文件
        }




        private string operation;

        public string Operation
        {
            get { return operation; }
            set { operation = value; RaisePropertyChanged(); }
        }


        public DelegateCommand RunCommnd { get; set; }

        private async void Run()
        {
            if (Operation == operationstart)
            {
                cancellationTokenSource = new CancellationTokenSource();
                halcontask = Task.Run(() => Detection(cancellationTokenSource.Token));
                Operation = operationend;

            }
            else
            {
                Operation = operationstart;
                cancellationTokenSource.Cancel();
                if (halcontask != null && !halcontask.IsCompleted)
                {
                    try
                    {
                        halcontask.Wait();
                    }
                    catch (AggregateException ex)
                    {

                        Console.WriteLine("Task was cancelled: " + ex.Message);
                    }
                }
            }
        }
        private int Index = 0;
        private string path = @"C:\Users\Seeney\Desktop\15中缺陷检测\halcon15种缺陷案例(样板图片+源码)\源码及素材\1\lcd";
        private string[] imagefiles;
        /// <summary>
        /// 根据路径加载图片文件
        /// </summary>
        private void LoadImageFiles()
        {
            string[] extensions = { "*.jpg", "*.jpeg", "*.png", "*.bmp", "*.tiff" };
            imagefiles = extensions.SelectMany(ext => System.IO.Directory.GetFiles(path, ext)).ToArray();
        }

        /// <summary>
        /// 图片检测
        /// </summary>
        /// <param name="cancellationToken"></param>
        private void Detection(CancellationToken cancellationToken)
        {
            try
            {
                while (Index < imagefiles.Length && !cancellationToken.IsCancellationRequested)
                {
                    string imagePath = imagefiles[Index];
                    if (!System.IO.File.Exists(imagePath))
                    {
                        Console.WriteLine($"File not found: {imagePath}");
                        Index++;
                        continue;
                    }
                    HOperatorSet.ReadImage(out HObject image, imagePath);
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        HOperatorSet.DispImage(image, hImage);
                    });
                    HOperatorSet.SetColor(hResult, "red");
                    HObject processedImage = ProcessImage(image);

                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        HOperatorSet.DispObj(image, hImage);
                        HOperatorSet.DispObj(processedImage, hResult);
                    });

                    Index++;
                    // 释放资源
                    image.Dispose();
                    processedImage.Dispose();
                 
                    Thread.Sleep(500); // 控制处理速度

                    cancellationToken.ThrowIfCancellationRequested();
                }
            }
            catch (OperationCanceledException)
            {

            }
        }

        /// <summary>
        /// 处理输入图片,并返回处理后的图片
        /// </summary>
        /// <param name="image"></param>
        /// <returns></returns>
        private HObject ProcessImage(HObject image)
        {
            HObject  ho_R = null, ho_G = null, ho_B = null;
            HObject ho_ImageFFT = null, ho_ImageGauss = null, ho_ImageConvol = null;
            HObject ho_ImageFFT1 = null, ho_ImageSub = null, ho_ImageZoomed = null;
            HObject ho_Domain = null, ho_RegionErosion = null, ho_ImageReduced = null;
            HObject ho_Lines = null, ho_Defects = null;

          

            HTuple hv_Path = new HTuple(), hv_Width = new HTuple();
            HTuple hv_Height = new HTuple(), hv_WindowHandle = new HTuple();
            HTuple hv_ScaleFactor = new HTuple(), hv_Sigma = new HTuple();
            HTuple hv_Low = new HTuple(), hv_High = new HTuple(), hv_f = new HTuple();
            HTuple hv_HomMat2DIdentity = new HTuple(), hv_HomMat2DScale = new HTuple();
          
            
            HOperatorSet.GenEmptyObj(out ho_R);
            HOperatorSet.GenEmptyObj(out ho_G);
            HOperatorSet.GenEmptyObj(out ho_B);
            HOperatorSet.GenEmptyObj(out ho_ImageFFT);
            HOperatorSet.GenEmptyObj(out ho_ImageGauss);
            HOperatorSet.GenEmptyObj(out ho_ImageConvol);
            HOperatorSet.GenEmptyObj(out ho_ImageFFT1);
            HOperatorSet.GenEmptyObj(out ho_ImageSub);
            HOperatorSet.GenEmptyObj(out ho_ImageZoomed);
            HOperatorSet.GenEmptyObj(out ho_Domain);
            HOperatorSet.GenEmptyObj(out ho_RegionErosion);
            HOperatorSet.GenEmptyObj(out ho_ImageReduced);
            HOperatorSet.GenEmptyObj(out ho_Lines);
            HOperatorSet.GenEmptyObj(out ho_Defects);






            //获取图片的宽度和高度
            HOperatorSet.GetImageSize(image, out  hv_Width, out hv_Height);
            hv_ScaleFactor = 0.4;
            //获取lines_gauss算子Sigma, Low, High三个参数值
            hv_Sigma.Dispose(); hv_Low.Dispose(); hv_High.Dispose();
            calculate_lines_gauss_parameters(17, (new HTuple(25)).TupleConcat(3), out hv_Sigma,out hv_Low, out hv_High);
            
            try
            {
                // 初始化所有HObject
                HOperatorSet.GenEmptyObj(out ho_R);
                HOperatorSet.GenEmptyObj(out ho_G);
                HOperatorSet.GenEmptyObj(out ho_B);
                HOperatorSet.GenEmptyObj(out ho_ImageFFT);
                HOperatorSet.GenEmptyObj(out ho_ImageGauss);
                HOperatorSet.GenEmptyObj(out ho_ImageConvol);
                HOperatorSet.GenEmptyObj(out ho_ImageFFT1);
                HOperatorSet.GenEmptyObj(out ho_ImageSub);
                HOperatorSet.GenEmptyObj(out ho_ImageZoomed);
                HOperatorSet.GenEmptyObj(out ho_Domain);
                HOperatorSet.GenEmptyObj(out ho_RegionErosion);
                HOperatorSet.GenEmptyObj(out ho_ImageReduced);
                HOperatorSet.GenEmptyObj(out ho_Lines);
                HOperatorSet.GenEmptyObj(out ho_Defects);

                // 检查输入图像是否有效
                if (!image.IsInitialized())
                {
                    throw new Exception("Input image is not initialized");
                }
                HOperatorSet.Decompose3(image, out ho_R, out ho_G, out ho_B);
                //*将图像转化为频域图像
                
                HOperatorSet.RftGeneric(ho_B, out ho_ImageFFT, "to_freq", "none", "complex",
                    hv_Width);
                //生成一个高斯滤波核
                
                HOperatorSet.GenGaussFilter(out ho_ImageGauss, 100, 100, 0, "n", "rft", hv_Width,
                    hv_Height);
                //将频域图像核高斯滤波核进行卷积运算
            
                HOperatorSet.ConvolFft(ho_ImageFFT, ho_ImageGauss, out ho_ImageConvol);
                //将卷积后的图像转换为空间域图像
               
                HOperatorSet.RftGeneric(ho_ImageConvol, out ho_ImageFFT1, "from_freq", "none",
                    "byte", hv_Width);
                //用缺陷图像减去背景图像(时域图像)
              
                HOperatorSet.SubImage(ho_B, ho_ImageFFT1, out ho_ImageSub, 2, 100);
                //对上述图像进行抽点，变焦
               
                HOperatorSet.ZoomImageFactor(ho_ImageSub, out ho_ImageZoomed, hv_ScaleFactor,
                    hv_ScaleFactor, "constant");
                //获取变焦后的图像的ROI
            
                HOperatorSet.GetDomain(ho_ImageZoomed, out ho_Domain);
                //图像ROI进行腐蚀操作
                
                HOperatorSet.ErosionRectangle1(ho_Domain, out ho_RegionErosion, 7, 7);
                //获取变焦图像中ROI区域内的图像
               
                HOperatorSet.ReduceDomain(ho_ImageZoomed, ho_RegionErosion, out ho_ImageReduced
                    );
                //探测线和获取线宽度
                ho_Lines.Dispose();
                HOperatorSet.LinesGauss(ho_ImageReduced, out ho_Lines, hv_Sigma, hv_Low,
                    hv_High, "dark", "true", "gaussian", "true");
                //生成一个2D的齐次变换矩阵
               
                HOperatorSet.HomMat2dIdentity(out hv_HomMat2DIdentity);
                //添加一个缩放因子到齐次变换矩阵
                using (HDevDisposeHelper dh = new HDevDisposeHelper())
                {
                    
                    HOperatorSet.HomMat2dScaleLocal(hv_HomMat2DIdentity, 1 / hv_ScaleFactor, 1 / hv_ScaleFactor,
                        out hv_HomMat2DScale);
                }
                //仿射变换
                
                HOperatorSet.AffineTransContourXld(ho_Lines, out ho_Defects, hv_HomMat2DScale);
                var result = ho_Defects;
                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
                throw;
               
            }
            finally
            {
                // 确保所有HObject都被释放
                // 释放所有中间对象，但保留返回对象
                ho_R?.Dispose();
                ho_G?.Dispose();
                ho_B?.Dispose();
                ho_ImageFFT?.Dispose();
                ho_ImageGauss?.Dispose();
                ho_ImageConvol?.Dispose();
                ho_ImageFFT1?.Dispose();
                ho_ImageSub?.Dispose();
                ho_ImageZoomed?.Dispose();
                ho_Domain?.Dispose();
                ho_RegionErosion?.Dispose();
                ho_ImageReduced?.Dispose();
                ho_Lines?.Dispose();

                // 释放HTuple
                hv_Width?.Dispose();
                hv_Height?.Dispose();
                hv_ScaleFactor?.Dispose();
                hv_Sigma?.Dispose();
                hv_Low?.Dispose();
                hv_High?.Dispose();
                hv_HomMat2DIdentity?.Dispose();
                hv_HomMat2DScale?.Dispose();
            }


            
        }



        public void calculate_lines_gauss_parameters(HTuple hv_MaxLineWidth, HTuple hv_Contrast,out HTuple hv_Sigma, out HTuple hv_Low, out HTuple hv_High)
        {



            // Local iconic variables 

            // Local control variables 

            HTuple hv_ContrastHigh = new HTuple(), hv_ContrastLow = new HTuple();
            HTuple hv_HalfWidth = new HTuple(), hv_Help = new HTuple();
            HTuple hv_MaxLineWidth_COPY_INP_TMP = new HTuple(hv_MaxLineWidth);

            // Initialize local and output iconic variables 
            hv_Sigma = new HTuple();
            hv_Low = new HTuple();
            hv_High = new HTuple();
            try
            {
                //Check control parameters
                if ((int)(new HTuple((new HTuple(hv_MaxLineWidth_COPY_INP_TMP.TupleLength()
                    )).TupleNotEqual(1))) != 0)
                {
                    throw new HalconException("Wrong number of values of control parameter: 1");
                }
                if ((int)(((hv_MaxLineWidth_COPY_INP_TMP.TupleIsNumber())).TupleNot()) != 0)
                {
                    throw new HalconException("Wrong type of control parameter: 1");
                }
                if ((int)(new HTuple(hv_MaxLineWidth_COPY_INP_TMP.TupleLessEqual(0))) != 0)
                {
                    throw new HalconException("Wrong value of control parameter: 1");
                }
                if ((int)((new HTuple((new HTuple(hv_Contrast.TupleLength())).TupleNotEqual(
                    1))).TupleAnd(new HTuple((new HTuple(hv_Contrast.TupleLength())).TupleNotEqual(
                    2)))) != 0)
                {
                    throw new HalconException("Wrong number of values of control parameter: 2");
                }
                if ((int)(new HTuple(((((hv_Contrast.TupleIsNumber())).TupleMin())).TupleEqual(
                    0))) != 0)
                {
                    throw new HalconException("Wrong type of control parameter: 2");
                }
                //Set and check ContrastHigh
                hv_ContrastHigh.Dispose();
                using (HDevDisposeHelper dh = new HDevDisposeHelper())
                {
                    hv_ContrastHigh = hv_Contrast.TupleSelect(
                        0);
                }
                if ((int)(new HTuple(hv_ContrastHigh.TupleLess(0))) != 0)
                {
                    throw new HalconException("Wrong value of control parameter: 2");
                }
                //Set or derive ContrastLow
                if ((int)(new HTuple((new HTuple(hv_Contrast.TupleLength())).TupleEqual(2))) != 0)
                {
                    hv_ContrastLow.Dispose();
                    using (HDevDisposeHelper dh = new HDevDisposeHelper())
                    {
                        hv_ContrastLow = hv_Contrast.TupleSelect(
                            1);
                    }
                }
                else
                {
                    hv_ContrastLow.Dispose();
                    using (HDevDisposeHelper dh = new HDevDisposeHelper())
                    {
                        hv_ContrastLow = hv_ContrastHigh / 3.0;
                    }
                }
                //Check ContrastLow
                if ((int)(new HTuple(hv_ContrastLow.TupleLess(0))) != 0)
                {
                    throw new HalconException("Wrong value of control parameter: 2");
                }
                if ((int)(new HTuple(hv_ContrastLow.TupleGreater(hv_ContrastHigh))) != 0)
                {
                    throw new HalconException("Wrong value of control parameter: 2");
                }
                //
                //Calculate the parameters Sigma, Low, and High for lines_gauss
                if ((int)(new HTuple(hv_MaxLineWidth_COPY_INP_TMP.TupleLess((new HTuple(3.0)).TupleSqrt()
                    ))) != 0)
                {
                    //Note that LineWidthMax < sqrt(3.0) would result in a Sigma < 0.5,
                    //which does not make any sense, because the corresponding smoothing
                    //filter mask would be of size 1x1.
                    //To avoid this, LineWidthMax is restricted to values greater or equal
                    //to sqrt(3.0) and the contrast values are adapted to reflect the fact
                    //that lines that are thinner than sqrt(3.0) pixels have a lower contrast
                    //in the smoothed image (compared to lines that are sqrt(3.0) pixels wide).
                    using (HDevDisposeHelper dh = new HDevDisposeHelper())
                    {
                        {
                            HTuple
                              ExpTmpLocalVar_ContrastLow = (hv_ContrastLow * hv_MaxLineWidth_COPY_INP_TMP) / ((new HTuple(3.0)).TupleSqrt()
                                );
                            hv_ContrastLow.Dispose();
                            hv_ContrastLow = ExpTmpLocalVar_ContrastLow;
                        }
                    }
                    using (HDevDisposeHelper dh = new HDevDisposeHelper())
                    {
                        {
                            HTuple
                              ExpTmpLocalVar_ContrastHigh = (hv_ContrastHigh * hv_MaxLineWidth_COPY_INP_TMP) / ((new HTuple(3.0)).TupleSqrt()
                                );
                            hv_ContrastHigh.Dispose();
                            hv_ContrastHigh = ExpTmpLocalVar_ContrastHigh;
                        }
                    }
                    hv_MaxLineWidth_COPY_INP_TMP.Dispose();
                    using (HDevDisposeHelper dh = new HDevDisposeHelper())
                    {
                        hv_MaxLineWidth_COPY_INP_TMP = (new HTuple(3.0)).TupleSqrt()
                            ;
                    }
                }
                //Convert LineWidthMax and the given contrast values into the input parameters
                //Sigma, Low, and High required by lines_gauss
                hv_HalfWidth.Dispose();
                using (HDevDisposeHelper dh = new HDevDisposeHelper())
                {
                    hv_HalfWidth = hv_MaxLineWidth_COPY_INP_TMP / 2.0;
                }
                hv_Sigma.Dispose();
                using (HDevDisposeHelper dh = new HDevDisposeHelper())
                {
                    hv_Sigma = hv_HalfWidth / ((new HTuple(3.0)).TupleSqrt()
                        );
                }
                hv_Help.Dispose();
                using (HDevDisposeHelper dh = new HDevDisposeHelper())
                {
                    hv_Help = ((-2.0 * hv_HalfWidth) / (((new HTuple(6.283185307178)).TupleSqrt()
                        ) * (hv_Sigma.TuplePow(3.0)))) * (((-0.5 * (((hv_HalfWidth / hv_Sigma)).TuplePow(
                        2.0)))).TupleExp());
                }
                hv_High.Dispose();
                using (HDevDisposeHelper dh = new HDevDisposeHelper())
                {
                    hv_High = ((hv_ContrastHigh * hv_Help)).TupleFabs()
                        ;
                }
                hv_Low.Dispose();
                using (HDevDisposeHelper dh = new HDevDisposeHelper())
                {
                    hv_Low = ((hv_ContrastLow * hv_Help)).TupleFabs()
                        ;
                }

                hv_MaxLineWidth_COPY_INP_TMP.Dispose();
                hv_ContrastHigh.Dispose();
                hv_ContrastLow.Dispose();
                hv_HalfWidth.Dispose();
                hv_Help.Dispose();

                return;
            }
            catch (HalconException HDevExpDefaultException)
            {

                hv_MaxLineWidth_COPY_INP_TMP.Dispose();
                hv_ContrastHigh.Dispose();
                hv_ContrastLow.Dispose();
                hv_HalfWidth.Dispose();
                hv_Help.Dispose();

                throw HDevExpDefaultException;
            }
        }

    }
}
