using Microsoft.CSharp;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace jolcode
{
  public  class ValidateItDynamicCode: IDisposable
    {
        private IntPtr nativeResource = Marshal.AllocHGlobal(100);

        //public Assembly ValidateThis(string pcFieldName, string pcCode,ref string[] strErr)
        public bool ValidateThis(string pcFieldName, string pcCode,string MyClass,ref string[] strErr)
        {
            CSharpCodeProvider codeProvider = new CSharpCodeProvider();
            ICodeCompiler compiler = codeProvider.CreateCompiler();
            CompilerParameters options = new CompilerParameters();
            options.ReferencedAssemblies.Add("System.dll");
           
            options.GenerateExecutable = false;
            options.GenerateInMemory = false;
            string source = "using System;\r\nusing System.IO;\r\nusing System.Text.RegularExpressions;\r\nusing System.Collections;\r\nnamespace jolcode {\r\n[Serializable]\r\npublic class " + MyClass + " {\r\npublic static string DynamicCode() {\r\n" + pcCode + " \r\n}}}";

            if (File.Exists(pcFieldName))
                File.Delete(pcFieldName);

            options.OutputAssembly = pcFieldName;

            CompilerResults compilerResults = compiler.CompileAssemblyFromSource(options, source);

            if (!compilerResults.Errors.HasErrors)
            {
                options = null;
                //return compilerResults.CompiledAssembly;
                return true;
            }
            strErr = new string[compilerResults.Errors.Count];

            for (int index = 0; index < compilerResults.Errors.Count; ++index)
            {
                strErr[index] = "FieldName: " + pcFieldName + "; " + compilerResults.Errors.Count.ToString() + " Errors:" + pcFieldName + " \r\nLine: " + compilerResults.Errors[index].Line.ToString() + " - " + compilerResults.Errors[index].ErrorText + " " + source;
            }
            //return (Assembly)null;
            return false;
        }
        public void CopyStream(Stream source, Stream dest, int bufferSize)
        {
            byte[] buffer = new byte[bufferSize];
            int count;
            while ((count = source.Read(buffer, 0, buffer.Length)) > 0)
                dest.Write(buffer, 0, count);
        }

        /*Dispose*/
        #region Dispose

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~ValidateItDynamicCode()
        {
            // Finalizer calls Dispose(false)
            Dispose(false);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                // free managed resources AnotherResource 
                //if (managedResource != null)
                //{
                //    managedResource.Dispose();
                //    managedResource = null;
                //}
            }
            // free native resources if there are any.
            if (nativeResource != IntPtr.Zero)
            {
                Marshal.FreeHGlobal(nativeResource);
                nativeResource = IntPtr.Zero;
            }
        }

        #endregion

    }
}
