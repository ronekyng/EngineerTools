using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EngineerTools
{
    public class Role
    {
        public Person Worker=new Person();
        public int RoleWork =0;
        public enum roles
        { 
            制图=1,
            设计=2,
            校核=3,
            审核=4,
            审定=5,
            单元负责人=6,
            项目负责人=7,
            项目经理=8
        };


    }
}
