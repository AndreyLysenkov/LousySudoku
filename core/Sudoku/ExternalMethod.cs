using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LousySudoku
{

    /// <summary>
    /// Describe location of external method
    /// </summary>
    public class ExternalMethod
    {

        /// <summary>
        /// Does describe external method
        /// </summary>
        public bool IsSet
        {
            get;
            private set;
        }

        /// <summary>
        /// Path to file with method (*.dll, *.exe using .NET)
        /// </summary>
        public string AssemblyPath
        {
            get;
            private set;
        }

        /// <summary>
        /// 
        /// </summary>
        public string MethodName
        {
            get;
            private set;
        }

        /// <summary>
        /// Set empty external method
        /// </summary>
        public ExternalMethod()
        {
            this.IsSet = false;
            this.AssemblyPath = "";
            this.MethodName = "";
        }

        /// <summary>
        /// Set external method
        /// </summary>
        /// <param name="AssemblyPath">*.dll or *.exe file with method (using .NET)</param>
        /// <param name="MethodName">name of method in file</param>
        public ExternalMethod(string AssemblyPath, string MethodName)
        {
            this.IsSet = true;
            this.AssemblyPath = AssemblyPath;
            this.MethodName = MethodName;
        }

    }

}
