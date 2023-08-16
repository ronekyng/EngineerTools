using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using Cad = Autodesk.AutoCAD.Interop;
using CadCommon = Autodesk.AutoCAD.Interop.Common;
using Microsoft.VisualBasic;
using System.Text.RegularExpressions;
using DatabaseServices = Autodesk.AutoCAD.DatabaseServices;
//using Editor = Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.Runtime;
using Autodesk.AutoCAD.Geometry;

using System.Windows.Forms;

namespace EngineerTools
{
    class DWGInfo
    {
        public string Constructure;
        public string ProjectName;
        public string UnitName;
        public string Area;
        public string Name;
        public string ProjectNo;
        public string No;
        /// <summary>
        /// 图框缩放的比例
        /// </summary>
        public double FrameScale;
        /// <summary>
        /// 角章中注明的比例（实际蓝图比例）
        /// </summary>
        public string DwgScale;
        /// <summary>
        /// 图纸尺寸1.0A1
        /// </summary>
        public string Size;
        /// <summary>
        /// true为横版，false为竖版
        /// </summary>
        public bool Direction;
        public static Cad.AcadApplication CadApp = null;
        /// <summary>
        /// 根据图框块获取图纸信息
        /// </summary>
        /// <param name="FrameBlockRef">图框块</param>
        public DWGInfo(CadCommon.AcadBlockReference FrameBlockRef)
        {
            Size = FrameBlockRef.Name;
            string[] FrameSizeCode = Size.Split('A');
            FrameScale =  FrameBlockRef.XScaleFactor;
            Cad.AcadApplication AcadApp=FrameBlockRef.Application;
            Cad.AcadDocument AcadDoc=FrameBlockRef.Document;
            //MessageBox.Show(FrameBlockRef.Name);
            //获取边界，根据MinPoint MaxPoint去框选图角章的框，但是GetBoundingBox获取的是世界坐标，可能在UCS中有问题
            object MinPoint,MaxPoint;
            FrameBlockRef.GetBoundingBox(out MinPoint,out MaxPoint);

            double[] MinPointArray,MaxPointArray;
            MinPointArray= new double[12];
            MaxPointArray=new double[12];
            MinPointArray= (double[])MinPoint;
            MaxPointArray=(double[])MaxPoint;

             Double[] TitleSelectionEdgePoints=new double[12];
            TitleSelectionEdgePoints[0]=MinPointArray[0];
            TitleSelectionEdgePoints[1]=MinPointArray[1];
            TitleSelectionEdgePoints[2]=0;

            TitleSelectionEdgePoints[3]=MinPointArray[0];
            TitleSelectionEdgePoints[4]=MaxPointArray[1];
            TitleSelectionEdgePoints[5]=0;

            TitleSelectionEdgePoints[6]=MaxPointArray[0];
            TitleSelectionEdgePoints[7]=MaxPointArray[1];
            TitleSelectionEdgePoints[8]=0;

            TitleSelectionEdgePoints[9]=MaxPointArray[0];
            TitleSelectionEdgePoints[10]=MinPointArray[1];
            TitleSelectionEdgePoints[11]=0;


            Int16[] TitleFilterType = new Int16[1];
            object[] TitleFilterData = new object[1];
            TitleFilterType[0] = 0;
            TitleFilterData[0] = "*";

            try
            {
                AcadDoc.SelectionSets.Item("TitleSelectionSet").Delete();
            }
            catch
            {

            }
            Cad.AcadSelectionSet TitleSelectionSet;
            TitleSelectionSet = AcadDoc.SelectionSets.Add("TitleSelectionSet");
            TitleSelectionSet.SelectByPolygon(CadCommon.AcSelect.acSelectionSetCrossingPolygon, TitleSelectionEdgePoints, TitleFilterType, TitleFilterData);
            for (int k = 0; k < TitleSelectionSet.Count; k++)
            {
                CadCommon.AcadEntity entity = (CadCommon.AcadEntity)TitleSelectionSet.Item(k);
                if ("AcDbBlockReference" == entity.ObjectName)
                {
                    CadCommon.AcadBlockReference TitleBlockRef;
                    TitleBlockRef = (CadCommon.AcadBlockReference)entity;
                    if(TitleBlockRef.Name=="项目名称")
                    {
                        foreach (object Att in TitleBlockRef.GetAttributes())
                        {
                            CadCommon.AcadAttributeReference AttRef;
                            AttRef = (CadCommon.AcadAttributeReference)Att;
                            switch (AttRef.TagString)
                            {
                                case "公司名称":
                                    Constructure = AttRef.TextString;
                                    break;
                                case "项目名称":
                                    ProjectName = AttRef.TextString;
                                    break;
                                case "装置名称":
                                    UnitName = AttRef.TextString;
                                    break;
                                case "区域":
                                    Area = AttRef.TextString;
                                    break;
                                case "图名":
                                    Name = AttRef.TextString;
                                    break;
                            }
                        }
                    }
                    if (TitleBlockRef.Name == "项目号")
                    {
                        foreach (object Att in TitleBlockRef.GetAttributes())
                        {
                            CadCommon.AcadAttributeReference AttRef;
                            AttRef = (CadCommon.AcadAttributeReference)Att;
                            switch (AttRef.TagString)
                            {
                                case "PRO.NO.":
                                    ProjectNo = AttRef.TextString;
                                    break;
                                case "DWG.NO.":
                                    No = AttRef.TextString;
                                    break;
                                case "SCALE":
                                    DwgScale = AttRef.TextString.Trim();
                                    break;
                            }
                        }
                    }
                    if(TitleBlockRef.Name == "角章项目信息")
                    {
                        foreach (object Att in TitleBlockRef.GetAttributes())
                        {
                            CadCommon.AcadAttributeReference AttRef;
                            AttRef = (CadCommon.AcadAttributeReference)Att;
                            switch (AttRef.TagString)
                            {
                                case "公司名称":
                                    Constructure = AttRef.TextString;
                                    break;
                                case "项目名称":
                                    ProjectName = AttRef.TextString;
                                    break;
                                case "装置名称":
                                    UnitName = AttRef.TextString;
                                    break;
                                case "区域":
                                    Area = AttRef.TextString;
                                    break;
                                case "图名":
                                    Name = AttRef.TextString;
                                    break;
                                case "PRO.NO.":
                                    ProjectNo = AttRef.TextString;
                                    break;
                                case "DWG.NO.":
                                    No = AttRef.TextString;
                                    break;
                                case "SCALE":
                                    DwgScale = AttRef.TextString.Trim();
                                    break;
                            }
                        }
                    }

                }
            }
            TitleSelectionSet.Clear();
            AcadDoc.SelectionSets.Item("TitleSelectionSet").Delete();
        }
        /// <summary>
        /// 根据版本号单行文字，获取图框，并获取图纸信息
        /// </summary>
        /// <param name="VersionText"></param>
        public DWGInfo(CadCommon.AcadText VersionText)
        {
            //根据版本号判断图框号
            string[] SubVersionText = VersionText.TextString.Split('-');//HQDF-01-01／01-2013 或者HQDF-01-03-2013 按照“-”分段
            string[] VersionNumText = SubVersionText[2].Split('/');//版本号第3段，01／01或者03再分段，继续判断；

            switch (VersionNumText[0].Trim())
            {
                case "03":
                    Size = "A4";//扁角章
                    break;
                case "05":
                    Size = "A1";//压力容器角章（分横板、竖版）
                    break;
                case "06":
                    Size = "A1";//压力管道角章
                    break;
                case "07":
                    Size = "A2";//专利商双角章
                    break;
                case "08":
                    Size = "A1";//双角章
                    break;
                case "01":
                    switch (VersionNumText[1].Trim())
                    {
                        case "01":
                            Size = "A1";
                            break;
                        case "02":
                            Size = "A2";
                            break;
                        case "03":
                            Size = "A3";
                            break;
                        case "04":
                            Size = "A1";//竖版
                            break;
                        case "05":
                            Size = "A2";//竖版
                            break;
                        case "06":
                            Size = "A0";//
                            break;
                    }
                    break;
                case "02":
                    switch (VersionNumText[1].Trim())
                    {
                        case "01":
                            Size = "A2";//小角章
                            break;
                        case "02":
                            Size = "A3";//小角章
                            break;
                    }
                    break;
            }
            FrameScale = VersionText.Height / 3.5;
            Cad.AcadDocument AcadDoc = VersionText.Document;
            Cad.AcadApplication AcadApp = (Cad.AcadApplication)AcadDoc.Application;
            //MessageBox.Show(AcadDoc.FullName);
            //Microsoft.VisualBasic.Interaction.AppActivate(AcadApp.Caption);
            //如果边框的选择集存在，则删除
            try
            {
                AcadDoc.SelectionSets.Item("FrameSelectionSet").Delete();
            }
            catch
            {

            }
            Cad.AcadSelectionSet FrameSelectionSet;
            FrameSelectionSet = AcadDoc.SelectionSets.Add("FrameSelectionSet");//定义边框的选择集，用来选择边框（内框）

            double[] VersionTextInsertionPoint = (double[])VersionText.InsertionPoint;//版本号的坐标
            Double[] FrameSelectionEdgePoints = new double[12];//版本号插入点开始，向上4.5，向左0.1的矩形区域可以选到内框
            FrameSelectionEdgePoints[0] = VersionTextInsertionPoint[0];
            FrameSelectionEdgePoints[1] = VersionTextInsertionPoint[1];
            FrameSelectionEdgePoints[2] = VersionTextInsertionPoint[2];

            FrameSelectionEdgePoints[3] = VersionTextInsertionPoint[0];
            FrameSelectionEdgePoints[4] = VersionTextInsertionPoint[1] + 4.5 * FrameScale;
            FrameSelectionEdgePoints[5] = VersionTextInsertionPoint[2];

            FrameSelectionEdgePoints[6] = VersionTextInsertionPoint[0] - 0.5 * FrameScale;
            FrameSelectionEdgePoints[7] = VersionTextInsertionPoint[1] + 4.5 * FrameScale;
            FrameSelectionEdgePoints[8] = VersionTextInsertionPoint[2];

            FrameSelectionEdgePoints[9] = VersionTextInsertionPoint[0] - 0.5 * FrameScale;
            FrameSelectionEdgePoints[10] = VersionTextInsertionPoint[1];
            FrameSelectionEdgePoints[11] = VersionTextInsertionPoint[2];

            Int16[] FrameFilterType = new Int16[1];
            object[] FrameFilterData = new object[1];
            FrameFilterType[0] = 0;
            //FilterData[0] = "*";//过滤多段线
            FrameFilterData[0] = "LWPOLYLINE";//过滤多段线
            FrameSelectionSet.SelectByPolygon(CadCommon.AcSelect.acSelectionSetCrossingPolygon, FrameSelectionEdgePoints, FrameFilterType, FrameFilterData);//选择集添加，将图框边框添加进去
            //遍历选择集
            for (int i = 0; i < FrameSelectionSet.Count;i++ )       
            {
                CadCommon.AcadLWPolyline FramePolyLine =(CadCommon.AcadLWPolyline)FrameSelectionSet.Item(i);
                
                //判断多段线全局宽度是否1.5，否则不是边框
                if (FramePolyLine.ConstantWidth == 1.5 * FrameScale)
                {
                    //MessageBox.Show("找到了边界");
                    object leftBottomPoint, rightTopPoint;
                    FramePolyLine.GetBoundingBox(out leftBottomPoint,out rightTopPoint);
                    double[] leftBottomArray,rightTopArray;
                    leftBottomArray=(double[])leftBottomPoint;
                    rightTopArray=(double[])rightTopPoint;

                    double PLLength = rightTopArray[0] - leftBottomArray[0];
                    double PLHeight = rightTopArray[1] - leftBottomArray[1];

                    if(PLHeight>PLLength)
                    {
                        Direction = false;
                    }
                    else
                    {
                        Direction = true;
                    }

                    try
                    {
                        AcadDoc.SelectionSets.Item("TitleSelectionSet").Delete();
                    }
                    catch
                    {

                    }
                    Cad.AcadSelectionSet TitleSelectionSet;
                    TitleSelectionSet = AcadDoc.SelectionSets.Add("TitleSelectionSet");
                    Double[] TitleSelectionEdgePoints = new double[12];//版本号插入点开始，向上4.5，向左0.1的矩形区域可以选到内框
                    Double[] FrameLWPolylineCoords;
                    FrameLWPolylineCoords=(double[])FramePolyLine.Coordinates;

                    for (int j = 0; j < FrameLWPolylineCoords.Length / 2; j++)
                    {
                        TitleSelectionEdgePoints[3 * j] = FrameLWPolylineCoords[2 * j];
                        TitleSelectionEdgePoints[3 * j + 1] = FrameLWPolylineCoords[2 * j + 1];
                        TitleSelectionEdgePoints[3 * j + 2] = 0;
                    }
                    Int16[] TitleFilterType = new Int16[1];
                    object[] TitleFilterData = new object[1];
                    TitleFilterType[0] = 0;
                    TitleFilterData[0] = "*";//过滤多段线
                    TitleSelectionSet.SelectByPolygon(CadCommon.AcSelect.acSelectionSetCrossingPolygon, TitleSelectionEdgePoints, TitleFilterType, TitleFilterData);
                    for (int k = 0; k < TitleSelectionSet.Count; k++)
                    {
                        CadCommon.AcadEntity entity = (CadCommon.AcadEntity)TitleSelectionSet.Item(k);
                        if ("AcDbBlockReference" == entity.ObjectName)
                        {
                            CadCommon.AcadBlockReference blkRef;
                            blkRef = (CadCommon.AcadBlockReference)entity;
                            if(blkRef.Name=="项目名称")
                            {
                                foreach (object Att in blkRef.GetAttributes())
                                {
                                    CadCommon.AcadAttributeReference AttRef;
                                    AttRef = (CadCommon.AcadAttributeReference)Att;
                                    switch (AttRef.TagString)
                                    {
                                        case "公司名称":
                                            Constructure = AttRef.TextString;
                                            break;
                                        case "项目名称":
                                            ProjectName = AttRef.TextString;
                                            break;
                                        case "装置名称":
                                            UnitName = AttRef.TextString;
                                            break;
                                        case "区域":
                                            Area = AttRef.TextString;
                                            break;
                                        case "图名":
                                            Name = AttRef.TextString;
                                            break;
                                    }
                                }
                            }
                            if (blkRef.Name == "项目号")
                            {
                                foreach (object Att in blkRef.GetAttributes())
                                {
                                    CadCommon.AcadAttributeReference AttRef;
                                    AttRef = (CadCommon.AcadAttributeReference)Att;
                                    switch (AttRef.TagString)
                                    {
                                        case "PRO.NO.":
                                            ProjectNo = AttRef.TextString;
                                            break;
                                        case "DWG.NO.":
                                            No = AttRef.TextString;
                                            break;
                                        case "SCALE":
                                            DwgScale = AttRef.TextString.Trim();
                                            break;
                                    }
                                }
                            }
                            //MessageBox.Show(blkRef.Name);

                        }
                    }
                    TitleSelectionSet.Clear();
                    AcadDoc.SelectionSets.Item("TitleSelectionSet").Delete();
                }
            }
            FrameSelectionSet.Clear() ;
            AcadDoc.SelectionSets.Item("FrameSelectionSet").Delete();
            
        }
        /// <summary>
        /// 获取每个dwg文件中的图框信息
        /// </summary>
        /// <param name="dwgPath">dwg文件完整路径</param>
        /// <returns>dwg信息列表</returns>
        public static List<DWGInfo> DwgTitles(string dwgPath)
        {
            //经常出现不响应呼叫的问题用下列方法
            //distributed Transaction Coordinator  和 RemoteProcedure Call服务需启动
            //组件服务-》 DCOM-》 AutoCad Application ->标示交互式用户
            //可能还是不好用，最好是用sleep
            List<DWGInfo> DIList = new List<DWGInfo>();
            Cad.AcadDocument CadDoc = null;
            CadCommon.AcadModelSpace CadSpace=null;
            bool dwgExist = false;

            if (CadApp == null)
            {
                try
                {  
                    CadApp = (Cad.AcadApplication)System.Runtime.InteropServices.Marshal.GetActiveObject("AutoCAD.Application"); 
                }
                catch
                {
                    try
                    {
                        int AppInitTimes = 0;
                        while (AppInitTimes <= 30)
                        {
                            AppInitTimes++;
                            System.Threading.Thread.Sleep(100 * AppInitTimes);
                            CadApp = new Cad.AcadApplication();
                            break;
                        }
                    }
                    catch
                    {
                    }     
                }
            }
            //查找CadApp打开的文件是否包含dwgPath
            foreach (Cad.AcadDocument doc in CadApp.Documents)
            {
                //MessageBox.Show(doc.FullName);
                if (doc.FullName == dwgPath)
                {
                    dwgExist = true;
                    CadDoc = doc;
                }  
            }
            if(dwgExist != true)//dwgPath不存在就打开
            {
                    //通过循环尝试，解决cad软件启动时间及dwg文档打开时间问题
                int DocInitTimes = 0;
                while (DocInitTimes <= 30)
                {
                    try
                    {
                        if (!dwgExist)
                        {
                            DocInitTimes++;
                            System.Threading.Thread.Sleep(100 * DocInitTimes);
                            CadDoc = CadApp.Documents.Open(dwgPath);
                            break;
                        }
                    }
                    catch
                    {
                    }
                }
             }
            if (!dwgExist)
            {
                int SpaceInitTimes = 0;
                while (SpaceInitTimes <= 30)
                {
                    try
                    {
                        SpaceInitTimes++;
                        System.Threading.Thread.Sleep(100 * SpaceInitTimes);
                        CadSpace = (CadCommon.AcadModelSpace)CadDoc.ModelSpace;
                        break;
                    }
                
                    catch
                    {
                    }
                }
            }       
            Microsoft.VisualBasic.Interaction.AppActivate(CadApp.Caption);
            CadDoc.SendCommand("zoom\nall\n");
            double curViewHeight,curViewWidth;
            double[] curScreeSize, curViewCenter, curViewLeftBottom = new double[3], curViewRightTop = new double[3];
            curViewHeight = CadDoc.GetVariable("VIEWSIZE");
            curViewCenter = CadDoc.GetVariable("VIEWCTR");
            curScreeSize = CadDoc.GetVariable("SCREENSIZE");
            curViewWidth = curViewHeight * curScreeSize[0] / curScreeSize[1];
            curViewLeftBottom[0] = curViewCenter[0] - curViewWidth / 2;
            curViewLeftBottom[1] = curViewCenter[1] - curViewHeight / 2;
            curViewLeftBottom[2] = 0;
            curViewRightTop[0] = curViewCenter[0] + curViewWidth / 2;
            curViewRightTop[1] = curViewCenter[1] + curViewHeight / 2;
            curViewRightTop[2] = 0;

            //如果边框的选择集存在，则删除
            try
            {
                CadDoc.SelectionSets.Item("TEXTSelectionSet").Delete();
            }
            catch
            {

            }
            Cad.AcadSelectionSet TEXTSelectionSet;
            TEXTSelectionSet = CadDoc.SelectionSets.Add("TEXTSelectionSet");//定义选择集，选择所有文本或者块
            Int16[] FilterType = new Int16[1];
            object[] FilterData = new object[1];
            FilterType[0] = 0;
            FilterData[0] = "TEXT";//过滤文本
            TEXTSelectionSet.Select(CadCommon.AcSelect.acSelectionSetWindow,curViewLeftBottom,curViewRightTop, FilterType, FilterData);

            //MessageBox.Show("全选得到" + TEXTSelectionSet.Count.ToString() + "个结果");

            for (int i=0;i<TEXTSelectionSet.Count;i++)
            {
                //CadCommon.AcadEntity entity =(CadCommon.AcadEntity) SelectionSet.Item(i);
                CadCommon.AcadText acText = (CadCommon.AcadText)TEXTSelectionSet.Item(i);
                //找到包含HQDF-01的TEXT文本
                if(acText.TextString.Contains("HQDF-01"))
                {
                    //MessageBox.Show(acText.TextString);
                    DWGInfo di = new DWGInfo(acText);
                    DIList.Add(di);
                }
            }
            try
            {
                TEXTSelectionSet.Clear();
                CadDoc.SelectionSets.Item("TEXTSelectionSet").Delete();
            }
            catch
            {
            }
            //查找包含图框的块
            try
            {
                CadDoc.SelectionSets.Item("BlockSelectionSet").Delete();
            }
            catch
            {

            }
            Cad.AcadSelectionSet BlockSelectionSet;
            BlockSelectionSet = CadDoc.SelectionSets.Add("BlockSelectionSet");//定义选择集，选择所有文本或者块
            FilterType[0] = 2;
            FilterData[0] = "1*A*";//对图块名称进行过滤，选择名字为1*A*的所有图块；
            BlockSelectionSet.Select(CadCommon.AcSelect.acSelectionSetWindow, curViewLeftBottom, curViewRightTop, FilterType, FilterData);

            //MessageBox.Show("全选得到" + BlockSelectionSet.Count.ToString() + "个块");

            for (int i = 0; i < BlockSelectionSet.Count; i++)
            {
                CadCommon.AcadBlockReference acBlock = (CadCommon.AcadBlockReference)BlockSelectionSet.Item(i);
                Regex regFrameBlock = new Regex(@"^1.+\d{0,2}A((0)|(1)|(2)|(3)|(4))$");
                if (regFrameBlock.IsMatch(acBlock.Name))
                {
                    DWGInfo di = new DWGInfo(acBlock);
                    DIList.Add(di);
                }
            }
            try
            {
                BlockSelectionSet.Clear();
                CadDoc.SelectionSets.Item("BlockSelectionSet").Delete();
            }
            catch
            {
            }    
            string txt = "";

            foreach (DWGInfo di in DIList)
            {
                txt = txt + di.ProjectName + di.No + "\r\n";
            }
            //MessageBox.Show(txt);
            
            return DIList;

        }

        public void SelectByBound(Cad.AcadApplication AcadApp)
        {
            //Microsoft.VisualBasic.Interaction.AppActivate(AcadApp.Caption);
            //object returnObject, pickPoint;
            //string pickPrompt = "";
            //Cad.AcadSelectionSet mySelectionSet;
            //mySelectionSet = AcadDoc.SelectionSets.Add("NewSelectionSet06");
            //Double[] selectionEdgePoints = new double[6];
            //Int16[] FilterType = new Int16[1];
            //object[] FilterData = new object[1];
            //FilterType[0] = 0;
            //FilterData[0] = "*";
            //mySelectionSet.SelectByPolygon(CadCommon.AcSelect.acSelectionSetCrossingPolygon,selectionEdgePoints, FilterType, FilterData);
        }

        public double[] To3D(double[] points)
        {
            double[] p3D = new double[points.Count() / 2 * 3];

            for (int i = 0; i < points.Count()/2;i++ )
            {
                p3D[3 * i] = points[2 * i];
                p3D[3 * i + 1] = points[2 * i + 1];
                p3D[3 * i + 2] = 0;
            }
            return p3D;
        }
        public double[] To2D(double[] points)
        {

            double[] p2D = new double[points.Count() / 3 * 2];
            for (int i = 0; i < points.Count()/3;i++ )
            {
                p2D[2 * i] = points[3 * i];
                p2D[2 * i + 1] = points[3 * i + 1];
            }
            return p2D;
        }

    }
}
