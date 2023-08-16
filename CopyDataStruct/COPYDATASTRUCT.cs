using System;
using System.Runtime.InteropServices;

namespace CopyDataStruct
{
        public struct CPOYDATASTRUCT
        {
            public IntPtr dwData;//用户定义数据
            public int cbData;//数据大小
            [MarshalAs(UnmanagedType.LPStr)]
            public string lpData;//指向数据的指针
        }
}
