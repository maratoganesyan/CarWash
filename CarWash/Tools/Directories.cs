using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarWash.Tools
{
    internal class Directories
    {
        private static readonly string ParentPath;
        private static readonly string Templates = @"Templates\";
        static Directories()
        {
            ParentPath = Directory.GetCurrentDirectory() + @"\Resources\";
        }
        public static string Check() => ParentPath + Templates + @"Check.docx";
        public static string WorkReport() => ParentPath + Templates + @"WorkReport.docx";
        public static string EmployeeReport() => ParentPath + Templates + @"EmployeeReport.docx";

        public static string ConnectionString() => ParentPath + @"ConnectionString.txt";

    }
}
