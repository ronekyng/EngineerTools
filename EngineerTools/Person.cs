using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.OleDb;

namespace EngineerTools
{
    
    public class Person
    {
        private struct DataFormat
        {
            public string ID;
            public string Name;
            public string FullName;
            public string Department;
            public string RTXID;
            public string Sex;
            public string Specialty;
            public string Email;
            public DataFormat(bool tag)
            {
                ID = "ID";
                Name = "用户名";
                FullName = "姓名";
                Department = "部门";
                RTXID = "RTXID";
                Sex = "性别";
                Specialty = "专业";
                Email = "Email";
            }
        }     
        private String _FullName;    //中文名
        private String _Name;    //电脑用户名
        public String Email;       //email
        public String RtxUser;
        private String _Specialty;
        public String Department;
        private bool Sex;
        public long ID;
        public String Class;
        public int RTXID;
        private static string DBTableName="PersonTest";
        private static DataFormat DF = new DataFormat(true);
        
        public Person()
        {
                       
        }
        public Person(String name)
        {
            GetFromUserName(name);
        }
        
        public string FullName
        {
            get
            {
                return _FullName;
            }
            set
            {
                _FullName = value;
            }

        }

        public string Name
        {
            get
            {
                return _Name;
            }
            set
            {
                _Name = value;
            }

        }

        public string Specialty
        {
            get
            {
                return _Specialty;
            }
            set
            {
                _Specialty = value;
            }
        }

        public void GetFromUserName(string name)
        {

            OleDbConnection conn = MainForm.getAccessConn(global::EngineerTools.Properties.Settings.Default.MdbFileName);
            conn.Open();
            String strSQL = "SELECT * FROM Person WHERE 电脑名='"+name+"' ";
            OleDbCommand myCommand = new OleDbCommand(strSQL,conn);
            OleDbDataReader reader = myCommand.ExecuteReader();
            if(reader.Read())
            {
                ID = Convert.ToInt64( reader["ID"]);
                FullName = reader["姓名"].ToString();
                Sex = (bool)reader["性别"];
                Specialty = reader["专业"].ToString();
                Department = reader["部门"].ToString();
                Email = reader["Email"].ToString();
                Name = name;
            }
            reader.Close();
            conn.Close();
        }

        
        public static List<Person> GetAllPersonList()
        {
            List<Person> list = new List<Person>();

            OleDbConnection conn = MainForm.getAccessConn(global::EngineerTools.Properties.Settings.Default.MdbFileName);
            conn.Open();
            String strSQL = "SELECT * FROM "+DBTableName;
            OleDbCommand myCommand = new OleDbCommand(strSQL, conn);
            OleDbDataReader reader = myCommand.ExecuteReader();
            while (reader.Read())
            {
                Person person = new Person();
                person.Name = reader[DF.Name].ToString();
                person.FullName = reader[DF.FullName].ToString();
                person.Sex = (bool)reader[DF.Sex];
                person.Specialty = reader[DF.Specialty].ToString();
                person.Department = reader[DF.Department].ToString();
                person.Email = reader[DF.Email].ToString();
                person.RTXID = (int) reader[DF.RTXID];
                list.Add(person);
            }
            reader.Close();
            conn.Close();
            return list;

        }
         

    }
}
