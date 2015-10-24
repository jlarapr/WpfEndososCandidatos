using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace jolcode
{  
    public class ExcuteScript
    {
        private string _ScriptFile;
        private SqlConnection _DBConnection;

        public string ScriptFile
        {
            set
            {
                if (File.Exists(value))
                {
                    try {
                        _ScriptFile = File.ReadAllText(value);
                    }catch
                    {
                        throw new Exception("Error en el ScriptFile ->" + value);
                    }
                }
                else
                    throw new FileNotFoundException("Error en el ScriptFile ->" + value);
            }

        }
        public SqlConnection DBConnection
        {
            set
            {
                _DBConnection = value;
            }
        }
        public bool MyExcuteScript()
        {
            bool myBoolOut = false;
            try
            {
                // split script on GO command
                IEnumerable<string> commandStrings = Regex.Split(_ScriptFile, @"^\s*GO\s*$",
                         RegexOptions.Multiline | RegexOptions.IgnoreCase);
                if (_DBConnection == null) 
                    throw new Exception("Error in SqlConnection is Null...");

                if (_DBConnection.State == System.Data.ConnectionState.Closed)
                _DBConnection.Open();

                foreach (string commandString in commandStrings)
                {
                    if (commandString.Trim() != "")
                    {
                        using (var command = new SqlCommand(commandString, _DBConnection))
                        {
                            command.ExecuteNonQuery();
                        }
                    }
                }
                _DBConnection.Close();
                
            }
            catch (Exception)
            {

                throw;
            }
            return myBoolOut;
        }
    }
}
