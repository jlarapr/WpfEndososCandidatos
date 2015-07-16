using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace jolcode
{
    public class Registry
	{
		public Registry() { /* TODO: Add constructor logic here */ }
// public **********************************************************
		public static string read(string key,string valueName) 
		{
			return getData(key,valueName);
		}// end read
		public static bool write(string key,string valueName,string valueData) 
		{
			return setData(key,valueName,valueData);
		}// end write
// private ********************************************************** 
		private static string getData(string key,string valueName) 
		{
			try 
			{
				Microsoft.Win32.RegistryKey  subKey = Microsoft.Win32.Registry.LocalMachine.OpenSubKey(key); 

				return subKey.GetValue(valueName).ToString() ;
			}
			catch (Exception ex)
			{
				throw new Exception (ex.Message + "--getData--");
			}
		}// end getData
		private static bool setData(string key,string valueName,string valuesData) 
		{
			try 
			{
				Microsoft.Win32.RegistryKey  subKey = Microsoft.Win32.Registry.LocalMachine.CreateSubKey(key); 			 
				subKey.SetValue(valueName,valuesData); 
				return true;
			}
			catch (Exception ex) 
			{
				throw new Exception (ex.Message + "--setData--");
			}
		}// end setData
	} // End Class
}// End namespace


