using Microsoft.CSharp;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace jolcode
{
  public  class ValidateItDynamicCode
    {
        public static Assembly ValidateThis(string pcFieldName, string pcCode,string dllName,ref string[] strErr)
        {
            CSharpCodeProvider codeProvider = new CSharpCodeProvider();
            ICodeCompiler compiler = codeProvider.CreateCompiler();
            CompilerParameters options = new CompilerParameters();
            options.ReferencedAssemblies.Add("System.dll");
           
            options.GenerateExecutable = false;
            options.GenerateInMemory = false;
            string source = "using System;\r\nusing System.IO;\r\nusing System.Text.RegularExpressions;\r\nusing System.Collections;\r\nnamespace jolcode {\r\n[Serializable]\r\npublic class MyClass {\r\npublic static string DynamicCode() {\r\n" + pcCode + " \r\n}}}";

            if (File.Exists(dllName))
                File.Delete(dllName);

            options.OutputAssembly = dllName; 
            CompilerResults compilerResults = compiler.CompileAssemblyFromSource(options, source);
            if (!compilerResults.Errors.HasErrors)
                return compilerResults.CompiledAssembly;

            strErr = new string[compilerResults.Errors.Count];

            for (int index = 0; index < compilerResults.Errors.Count; ++index)
            {
                strErr[index] = "FieldName: " + pcFieldName + "; " + compilerResults.Errors.Count.ToString() + " Errors:" + pcFieldName + " \r\nLine: " + compilerResults.Errors[index].Line.ToString() + " - " + compilerResults.Errors[index].ErrorText + " " + source;
            }
            return (Assembly)null;
        }
        public static void CopyStream(Stream source, Stream dest, int bufferSize)
        {
            byte[] buffer = new byte[bufferSize];
            int count;
            while ((count = source.Read(buffer, 0, buffer.Length)) > 0)
                dest.Write(buffer, 0, count);
        }
    }
}
