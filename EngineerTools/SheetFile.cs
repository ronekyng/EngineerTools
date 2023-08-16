using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Excel=Microsoft.Office.Interop.Excel;
using System.Runtime.InteropServices;
using System.IO;

namespace EngineerTools
{
    class SheetFile : DesignFile
    {
        public struct  HeadStruct
        {
            public string Name;
            public string Code;
            public string ProjName;
            public int Pages;
            public bool IsInited;
        }
        [DllImport("user32.dll", EntryPoint = "GetWindowThreadProcessId", CharSet = CharSet.Auto)]
        public static extern int GetWindowThreadProcessId(IntPtr hwnd, out int ID);

        private NormalFile _Type;
        private bool _Opened=false;
        private HeadStruct _Head=new HeadStruct ();
        private Excel.Application xlApp;
        private Excel.Workbook xlsWorkBook = null;
        private Excel.Worksheet xlsSheetIndex=null;

        public void Close()
        {
            try
            {
                xlsWorkBook.Close(false);
                xlApp.Quit();
            }
            catch { }
            KillSpecailExcel(xlApp);
        }
        public SheetFile(FileInfo fileInfo)
        {

            Info = fileInfo;
        }

        /// <summary>
        /// NormalFile类型作为工作表的类型（Type）
        /// </summary>
        public NormalFile Type
        {
            get
            {
                if(_Type==null)
                {
                    if(_Opened==false)
                    {
                        Open();
                    }
                    if (xlsWorkBook != null)
                    {
                        xlsSheetIndex = FindWorkSheet(xlsWorkBook, "首页|索引Index");
                        if (xlsSheetIndex != null)
                        {
                            string FullersionText = xlsSheetIndex.PageSetup.RightFooter ;
                            FullersionText = FullersionText.Substring(FullersionText.IndexOf("HQSF"));
                            FullersionText = FullersionText.Trim();
                            string shortVersionText;
                            string[] versionTextArray;
                            if (FullersionText.Contains("/"))
                            {
                                versionTextArray = FullersionText.Split('/');
                                shortVersionText = versionTextArray[0];
                            }
                            else
                            {
                                shortVersionText = FullersionText.Substring(0, FullersionText.Count() - 5);
                            }
                            //NormalFile findNormalFile = global::EngineerTools.Properties.Settings.Default.NormalFileList.Find(c => c.Version == shortVersionText);
                            //if (findNormalFile != null)
                            //{
                            //    _Type = findNormalFile;
                            //    return _Type;
                            //}
                            
                        }
                    }
                }
                return _Type;
            }
            set 
            {
                _Type = value;
            }

        }
        public new void  Open()
        {
            xlApp = new Excel.Application();           
            xlApp.DisplayAlerts = false;
            xlApp.Visible = false;      //设置是否显示
            xlApp.ScreenUpdating = false;  //禁止刷新屏幕
            try
            {
                xlsWorkBook = xlApp.Workbooks.Open(
                    Info.FullName,
                    ReadOnly: false,
                    Editable: true,
                    IgnoreReadOnlyRecommended: true,
                    UpdateLinks: false,
                    Notify: false,
                    CorruptLoad: Excel.XlCorruptLoad.xlNormalLoad);
                    _Opened = true;
            }
            catch
            {
                try
                {
                    xlsWorkBook = xlApp.Workbooks.Open(
                        Info.FullName,
                        ReadOnly: false,
                        Editable: true,
                        IgnoreReadOnlyRecommended: true,
                        UpdateLinks: false,
                        Notify: false,
                        CorruptLoad: Excel.XlCorruptLoad.xlExtractData);
                        _Opened = true;
                }
                catch
                {

                }
            }
        }

        public HeadStruct Head
        {
            get
            {
                if(_Head.IsInited)
                {
                    return _Head;
                }
                else
                {  
                    try
                    {
                        Excel.Range nameRange = xlsSheetIndex.Cells.Find(Type.Name);
                        if (nameRange != null)
                        {
                            _Head.Name = (string)nameRange.Value;
                        }
                        else
                        {
                            _Head.Name = "Error";          
                        }
                        Excel.Range codeRange = xlsSheetIndex.get_Range("R3:U3").Find("/D");
                        if (codeRange != null)
                        {
                            _Head.Code = (string)codeRange.Value;
                        }
                        else
                        {
                            _Head.Code = "Error";
                        }
                        _Head.IsInited = true;
                    }
                    catch
                    {
                    }
                    return _Head;
                }
                
            }
            set
            {

            }
        }
        public string GetDocSheetName()
        {
            string name="";

            //Excel.Range myNameRange = xlsWorkSheet.get_Range("F2:M5").Find("数据表");
            Excel.Range myNameRange = xlsSheetIndex.UsedRange.Find("数据表");
            if (myNameRange != null)
            {

                string bookName = (string)myNameRange.Value;
                string bookCode;
                Excel.Range myCodeRange = xlsSheetIndex.get_Range("R3:U3").Find("/D");
                if (myCodeRange != null)
                {
                    bookCode = (string)myCodeRange.Value;
                }
                else
                {
                    bookCode = "文表号为空";
                }
            }
            return name;
        }
        public string GetDocSheetCode()
        {
            string code = "";
            return code;
        }
        
        private Excel.Worksheet FindWorkSheet(Excel.Workbook workBook, string names)
        {
            Excel.Worksheet findSheet = new Excel.Worksheet();
            foreach (Excel.Worksheet sheet in workBook.Sheets)
            {
                string[] nameA = names.Split('|');
                foreach(string name in nameA)
                {
                    if (sheet.Name.Contains(name) || name.Contains(sheet.Name))
                    {
                        findSheet = sheet;
                        return findSheet;
                    }
                    else
                    {
                        findSheet = null;
                    }
                }

            }
            return findSheet;
        }
        private static void KillSpecailExcel(Microsoft.Office.Interop.Excel.Application m_objExcel)
        {
            try
            {
                if (m_objExcel != null)
                {
                    int lpdwProcessId;
                    GetWindowThreadProcessId(new IntPtr(m_objExcel.Hwnd), out lpdwProcessId);
                    System.Diagnostics.Process.GetProcessById(lpdwProcessId).Kill();
                }
            }
            catch (Exception ex)
            {
            }
        }

        public object codeRange { get; set; }

        /*
        /// <summary>
        /// 通过查找excel表格的首页，确定文表号，并为类变量xlsSheetIndex赋值
        /// </summary>
        /// <returns>返excel文表的类型，以公司标准文件号为准</returns>
        
        public string GetSheetType()
        {
            string type="Null";
            if (xlsWorkBook != null)
            {
                //通过工作表的名称可以判断出数据表、管道表、等
                xlsSheetIndex = FindWorkSheet(xlsWorkBook, "索引Index|数据表索引Index");//数据表首页有两种情况
                if (xlsSheetIndex != null)
                {
                    type = "E";//数据表E，具体数据表类型未定
                    return type;
                }
                xlsSheetIndex = FindWorkSheet(xlsWorkBook, "管道表首页");//管道表首页
                if (xlsSheetIndex != null)
                {
                    type = "P1";
                    return type;
                }
                xlsSheetIndex = FindWorkSheet(xlsWorkBook, "材料表首页");//材料表首页
                if (xlsSheetIndex != null)
                {
                    type = "M1";
                    return type;
                }
                xlsSheetIndex = FindWorkSheet(xlsWorkBook, "设材表首页");//设材表首页
                if (xlsSheetIndex != null)
                {
                    type = "M2";
                    return type;
                }
                xlsSheetIndex = FindWorkSheet(xlsWorkBook, "目录首页");//目录首页
                if (xlsSheetIndex != null)
                {
                    type = "L1";
                    return type;
                }
                xlsSheetIndex = FindWorkSheet(xlsWorkBook, "管段材料表首页");//管段材料表首页
                if (xlsSheetIndex != null)
                {
                    type = "M3";
                    return type;
                }
                xlsSheetIndex = FindWorkSheet(xlsWorkBook, "支吊架索引表首页");//支吊架索引表首页
                if (xlsSheetIndex != null)
                {
                    type = "P4";
                    return type;
                }
                xlsSheetIndex = FindWorkSheet(xlsWorkBook, "管段图索引表首页");//管段图索引表首页
                if (xlsSheetIndex != null)
                {
                    type = "P5";
                    return type;
                }
                xlsSheetIndex = FindWorkSheet(xlsWorkBook, "支吊架汇总表首页");//支吊架汇总表首页
                if (xlsSheetIndex != null)
                {
                    type = "P6";
                    return type;
                }
                xlsSheetIndex = FindWorkSheet(xlsWorkBook, "伴热管道索引表首页");//伴热管道索引表首页
                if (xlsSheetIndex != null)
                {
                    type = "P7";
                    return type;
                }
                xlsSheetIndex = FindWorkSheet(xlsWorkBook, "界区条件表首页");//界区条件表首页
                if (xlsSheetIndex != null)
                {
                    type = "PT";
                    return type;
                }
                //如果不是以上表格，则通过表头来判断
                if(xlsSheetIndex==null)
                {
                    foreach(Excel.Worksheet sh in xlsWorkBook.Worksheets)
                    {

                        Excel.Range FindRange = sh.Cells.Find("文表号");
                        if(FindRange!=null)
                        {
                            string docCode=FindRange.get_Offset(1, 0).Value;
                            docCode.Trim();
                            string[] docCodeArray = docCode.Split('/');
                            type = docCodeArray[1].Trim();
                            return type;
                        }
                    }
                }
            }
            return type;
        }
        */
    }
}
