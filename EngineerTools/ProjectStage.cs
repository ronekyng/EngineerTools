using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EngineerTools
{
    public class ProjectStage
    {
        Dictionary<string, string> StageDic = new Dictionary<string, string> 
        { 
                     
            {"B", "基础设计"},
            {"D", "详细设计"},
            {"F", "可研"},
            {"G", "总体设计"},
        };

        private string _Code;
        private string _Name;

        //public ProjectStage(string str)
        //{
        //    Code = str;
        //    Name=StageDic[Code];
        //}
        public String Code
        {
            get
            {
                return _Code;
            }
            set
            {
                _Code = value;
                Name = StageDic[Code];
            }
        }

        public String Name
        {
            get
            {
                return _Name;
            }
            set
            {
                 _Name=value ;
            }
        }
    }
}
