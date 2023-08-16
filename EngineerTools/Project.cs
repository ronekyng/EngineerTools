using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.OleDb;
using System.Windows.Forms;


namespace EngineerTools
{
    public class Project
    {
        private struct DataFormat
        {
            public string  ID;
            public string No;
            public string Name;
            public string Constructure;
            public string Address;
            public string StageCode;
            public string _PMID;
            public string ShortName;

            public string MyProjectPerson;
            public string MyProjectProject;
            public string MyProjectDir;
            public string MyProjectStartTime;
            public string MyProjectEndTime;
            public string MyProjectId;
            public DataFormat(bool tag)
            {
                ID= "ID";
                No="项目号";
                Name= "项目名称";
                Constructure= "建设单位";
                Address="建设地";
                StageCode= "设计阶段";
                _PMID = "项目经理";
                ShortName="简称";

                MyProjectPerson = "人员";
                MyProjectProject = "项目";
                MyProjectDir = "项目根目录";
                MyProjectStartTime = "开始时间";
                MyProjectEndTime = "最后时间";
                MyProjectId = "ID";
            }
        }        
        public struct WorkStruct
        {
            public string Dir;
            public DateTime StartTime;
            public DateTime EndTime;
            //public string StartTime;
            //public string EndTime;
            public long ID;
        }
        public long ID;
        public  String Name;
        public  String Constructure;
        public  String Address;
        public List<Unit> Units = new List<Unit>();
        public string ShortName;
        public bool Working = false;
        public ProjectStage Stage=new ProjectStage();
        public WorkStruct Work;

        private String _Code;
        private String _No;
        private int _PMID;
        private string _FilesPath;
        
        private const string DBTableName = "项目表";
        private const string MyProjectDBTableName = "个人项目表";
        private const string PersonDBTableName = "Person";
        private static DataFormat DF = new DataFormat(true);

        public String No
        {
            get
            {     
                return _No;
            }
            set
            {
                _No = value;
            }

        }

        public String Code
        {
            get
            {
                _Code = _No + "-" + Stage.Code;
                return _Code;
            }
            set
            {
                _Code = _No + "-" + Stage.Code;
            }

        }
        public string FilesPath
        {
            get
            {
                _FilesPath = global::EngineerTools.Properties.Settings.Default.ProjectRootFolder + @"\" + Work.Dir;
                return _FilesPath;
            }
        }
        public Project()
        {

        }
        public Project(string code)
        {

        }
        public Project(string no, string stageCode)
        {
            /*
            DataFormat DF = new DataFormat(true);
            OleDbConnection conn = MainForm.getAccessConn(global::EngineerTools.Properties.Settings.Default.MdbFileName);
            conn.Open();
            String strSQL = "SELECT * FROM " + DBTableName + " WHERE " + DF.No + " ='" + no + "' AND "+DF.StageCode+" ='"+stageCode+"'";
            OleDbCommand myCommand = new OleDbCommand(strSQL, conn);
            OleDbDataReader reader = myCommand.ExecuteReader();
            if (reader.Read())
            {
                //Project pro = InitialFromDataReader(reader);
                //_ID = pro._ID;
                //Name = pro.Name;
                //No = pro.No;
                //Constructure = pro.Constructure;
                //Stage = pro.Stage;
                //Address = pro.Address;
                ID = Convert.ToInt64( reader[DF.ID]);
                Name = reader[DF.Name].ToString();
                No = reader[DF.No].ToString();
                Constructure = reader[DF.Constructure].ToString();
                Stage.Code = reader[DF.StageCode].ToString();
                Address = reader[DF.Address].ToString();  
            }
            reader.Close();
            conn.Close();
            */
        }

        public Project(long ID)
        {
            //OleDbConnection conn = MainForm.getAccessConn(Setting.MdbFile);
            //conn.Open();
            //String strSQL = "SELECT * FROM " + DBTableName + " WHERE " + DF.ID + " =" + ID + "";
            //OleDbCommand myCommand = new OleDbCommand(strSQL, conn);
            //OleDbDataReader reader = myCommand.ExecuteReader();
            //if (reader.Read())
            //{
            //    InitialFromDataReader(reader);
            //    Units = Unit.GetProjectUnitsList(this);
            //}
            //reader.Close();
            //conn.Close();
        }

        //public List<Unit> Units()
        //{
        //    List<Unit> UnitsList = new List<Unit>();


        //    return UnitsList;
        //}
        public static List<Project> GetAllProjectList()
        {
            List<Project> list = new List<Project>();

            OleDbConnection conn = MainForm.getAccessConn(SettingFormController.MdbFile);
            try
            {
                conn.Open();
                String strSQL = "SELECT * FROM " + DBTableName;
                OleDbCommand myCommand = new OleDbCommand(strSQL, conn);
                OleDbDataReader reader = myCommand.ExecuteReader();
                while (reader.Read())
                {
                    Project project = GetFromDataReader(reader);
                    list.Add(project);
                }
                reader.Close();
                conn.Close();
            }
            catch (Exception ex)
            {

            }

            return list;

        }
        public static List<Project> GetOnesProjectsList( List<Project> AllProjects,Person One)
        {
            
            List<Project> OnesProjects = new List<Project>();
            try
            {
                OleDbConnection conn = MainForm.getAccessConn(SettingFormController.MdbFile);
                string strSQL = "SELECT *  FROM 个人项目表 "                  
                    + " WHERE (( 人员)=" + One.ID + ");";
                conn.Open();
                OleDbCommand myCommand = new OleDbCommand(strSQL, conn);
                OleDbDataReader reader;
                reader = myCommand.ExecuteReader();
                while (reader.Read())
                {
                    long projID = Convert.ToInt64(reader[DF.MyProjectProject]);
                    Project proj = AllProjects.Find(c => c.ID.Equals(projID));
                    if(proj!=null)
                    {
                        proj.Work.Dir = reader[DF.MyProjectDir].ToString();
                        proj.Work.ID = Convert.ToInt64(reader[DF.MyProjectId]);
                        if (reader[DF.MyProjectStartTime]!= System.DBNull.Value)
                            proj.Work.StartTime =Convert.ToDateTime( reader[DF.MyProjectStartTime]);
                        if (reader[DF.MyProjectEndTime] != System.DBNull.Value)
                            proj.Work.EndTime = Convert.ToDateTime( reader[DF.MyProjectEndTime]);
                    }
                    OnesProjects.Add(proj);
                }
                reader.Close();
                conn.Close();
            }
            catch (Exception e)
            {
                //MessageBox.Show("读取项目数据失败！");
            }
            return OnesProjects;
        }

        public List<Unit> GetOnesUnits(Person One)
        {
            List<Unit> OnesUnits=Unit.GetOnesUnits(this,ref Units,One);
            return OnesUnits;
        }
        /// <summary>
        /// 静态方法，根据oledbDataReader，返回类的实例。
        /// </summary>
        /// <param name="reader">查询数据库生成的oledbDataReader</param>
        /// <returns>Project</returns>
        private static Project GetFromDataReader(OleDbDataReader reader)
        {
            Project pro = new Project();
            pro.ID = Convert.ToInt64(reader[DF.ID]);
            pro.Name = reader[DF.Name].ToString();
            pro.No = reader[DF.No].ToString();
            pro.Constructure = reader[DF.Constructure].ToString();
            pro.Stage.Code = reader[DF.StageCode].ToString();
            pro.Address = reader[DF.Address].ToString();
            pro.Units = Unit.GetProjectUnitsList(pro);
            return pro;
        }

        /// <summary>
        /// 根据oledbDataReader初始化对象的各成员。
        /// </summary>
        /// <param name="reader"></param>
        private  void InitialFromDataReader(OleDbDataReader reader)
        {
            ID = Convert.ToInt64(reader[DF.ID]);
            Name = reader[DF.Name].ToString();
            No = reader[DF.No].ToString();
            Constructure = reader[DF.Constructure].ToString();
            Stage.Code = reader[DF.StageCode].ToString();
            Address = reader[DF.Address].ToString();
            Units = Unit.GetProjectUnitsList(this);
        }

    }
}
