using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.OleDb;
using System.Windows.Forms;

namespace EngineerTools
{
    public class Unit
    {
        private struct DataFormat
        {
            public string ID;
            public string Code;
            public string Name;
            public string Working;
            public string Dir;
            public string NormalUnitCode;
            public string ProjectID;

            public string MyWork_ID;
            public string MyWork_Person;
            public string MyWork_Unit;
            public string MyWork_Dir;
            public string MyWork_StartTime;
            public string MyWork_EndTime;
            public string MyWork_State;

            public DataFormat(bool tag)
            {
                ID = "ID";
                Code = "单元号";
                Name = "单元名称";
                Working = "状态";
                Dir = "目录";
                NormalUnitCode = "标准编号";
                ProjectID = "项目阶段ID";

                MyWork_ID="个人单元表.ID";
                MyWork_Person="人员";
                MyWork_Unit="单元表.ID";
                MyWork_Dir="单元目录";
                MyWork_StartTime="开始时间";
                MyWork_EndTime="最后时间";
                MyWork_State="状态";
            }
        }
        public struct WorkStruct
        {
            public string Dir;
            public DateTime StartTime;
            public DateTime EndTime;
            public string State;
        }
        public long ID;
        public String Name;
        public String Code;
        public List<Role> RoleList = new List<Role>();
        public string Working ;
        public List<Partition> Partitions = new List<Partition>();
        public WorkStruct Work=new WorkStruct();
        public Project Owner=new Project();
        public string NormalUnitCode;
        private long ProjectID;
        private string _FilesPath;
        
        private static string DBTableName = "单元表";
        private static string MyWorkDBTableName = "_个人项目单元表";
        private static DataFormat DF = new DataFormat(true);
        public string FilesPath
        {
            get
            {
                _FilesPath = Owner.FilesPath + @"\" +Work.Dir;
                return _FilesPath;
            }
        }
        public  Unit()
        {

        }
        public Unit(Project project)
        {
            Owner = project;
        }
        public  Unit(long ID)
        {
            OleDbConnection conn = MainForm.getAccessConn(global::EngineerTools.Properties.Settings.Default.MdbFileName);
            conn.Open();
            String strSQL = "SELECT * FROM " + DBTableName + " WHERE " + DF.ID + " = " + ID ;
            OleDbCommand myCommand = new OleDbCommand(strSQL, conn);
            OleDbDataReader reader = myCommand.ExecuteReader();
            while (reader.Read())
            {
                InitFromDataReader(reader);
            }
        }
        public static List<Unit> GetProjectUnitsList(Project project)
        {
            List<Unit> list = new List<Unit>();

            OleDbConnection conn = MainForm.getAccessConn(global::EngineerTools.Properties.Settings.Default.MdbFileName);
            conn.Open();
            String strSQL = "SELECT * FROM " + DBTableName + " WHERE " + DF.ProjectID + " = " + project.ID;
            OleDbCommand myCommand = new OleDbCommand(strSQL, conn);
            OleDbDataReader reader = myCommand.ExecuteReader();
            while (reader.Read())
            {
                Unit unit = new Unit(project);
                unit.InitFromDataReader(reader);
                list.Add( unit);
            }
            reader.Close();
            conn.Close();
            return list;
        }

        public static List<Unit> GetOnesUnits( Project project ,ref List<Unit> ProjectUnits, Person One)
        {
            //MessageBox.Show( ProjectUnits[0].Owner.Code);
            List<Unit> OnesUnits = new List<Unit>();
            OleDbConnection conn = MainForm.getAccessConn(global::EngineerTools.Properties.Settings.Default.MdbFileName);
            conn.Open();
            String strSQL = "SELECT * FROM " + MyWorkDBTableName + " WHERE " + DF.ProjectID + " = " + project.ID + " AND " + DF.MyWork_Person + "=" + One.ID;
            OleDbCommand myCommand = new OleDbCommand(strSQL, conn);
            OleDbDataReader reader = myCommand.ExecuteReader();
            while (reader.Read())
            {
                long unitID = Convert.ToInt64(reader[DF.MyWork_Unit]);
                Unit unit = ProjectUnits.Find(c => c.ID.Equals(unitID));
                if(unit!=null)
                {
                    unit.Work.Dir = reader[DF.MyWork_Dir].ToString();
                    if (reader[DF.MyWork_StartTime] != System.DBNull.Value)
                        unit.Work.StartTime = Convert.ToDateTime(reader[DF.MyWork_StartTime]);
                    if (reader[DF.MyWork_EndTime] != System.DBNull.Value)
                        unit.Work.EndTime = Convert.ToDateTime(reader[DF.MyWork_EndTime]);
                    if (reader[DF.MyWork_State] != System.DBNull.Value)
                        unit.Work.State = reader[DF.MyWork_State].ToString();
                    OnesUnits.Add(unit);
                }
            }
            reader.Close();
            conn.Close();
            return OnesUnits;
        }

        private static Unit GetFromDataReader(OleDbDataReader reader,Project owner)
        {
            Unit u = new Unit(owner);
            u.ID = Convert.ToInt64(reader[DF.ID]);
            u.Name = reader[DF.Name].ToString();
            u.Code = reader[DF.Code].ToString();
            u.NormalUnitCode = reader[DF.NormalUnitCode].ToString();
            u.Working = reader[DF.Working].ToString();
            u.ProjectID =Convert.ToInt64( reader[DF.ProjectID]);
            return u;
        }
        private  void InitFromDataReader(OleDbDataReader reader)
        {
            ID = Convert.ToInt64(reader[DF.ID]);
            Name = reader[DF.Name].ToString();
            Code = reader[DF.Code].ToString();
            NormalUnitCode = reader[DF.NormalUnitCode].ToString();
            Working = reader[DF.Working].ToString();
            ProjectID = Convert.ToInt64(reader[DF.ProjectID]);
        }
    }
}
