using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using System.Runtime.InteropServices;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Threading;
using System.IO;
using System.Reflection;
using System.Configuration;

namespace jolcode
{
    public class SqlExcuteCommand : IDisposable
    {
        private IntPtr nativeResource = Marshal.AllocHGlobal(100);
        private string _DBCnnStr;
        private string _DBCeeMasterCnnStr;
        private string _DBImagenesCnnStr;
        private string _DBRadicacionesCEECnnStr;

        public SqlExcuteCommand()
        {

        }

        public string DBCnnStr
        {
            get
            {
                return _DBCnnStr;
            }
            set
            {
                _DBCnnStr = value;
            }
        }
        public string DBCeeMasterCnnStr
        {
            get
            {
                return _DBCeeMasterCnnStr;
            }
            set
            {
                _DBCeeMasterCnnStr = value;
            }
        }
        public string DBImagenesCnnStr
        {
            get
            {
                return _DBImagenesCnnStr;
            }
            set
            {
                _DBImagenesCnnStr = value;
            }
        }
        public string DBRadicacionesCEECnnStr
        {
            get
            {
                return _DBRadicacionesCEECnnStr;
            }
            set
            {
                _DBRadicacionesCEECnnStr = value;
            }
        }
        public byte[] MyGetImgInByte(String NumElec, String Partido, String Lot, String Batch, String Formulario, String Image, String Errores, String Status)
        {
            byte[] myout = null;
            try
            {
                string query = String.Format(@"
                SELECT EndosoImage FROM [dbo].[LotsEndo] 
                where [NumElec]   = '{0}'  and
                      [Partido]   = '{1}'  and
                      [Lot]       = '{2}'  and
                      [Batch]     = '{3}'  and
                      [Formulario]= '{4}'  and
                      [Image]     = '{5}'  and
                      [Errores]   = '{6}'  and
                      [Status]    = '{7}'
                 ", NumElec,
                    Partido,
                    Lot,
                    Batch,
                    Formulario,
                    Image,
                    Errores,
                    Status);

                using (SqlConnection cnn = new SqlConnection()
                {
                    ConnectionString = DBCnnStr
                })
                {
                    using (SqlCommand cmd = new SqlCommand()
                    {
                        Connection = cnn,
                        CommandType = CommandType.Text,
                    })
                    {

                        if (cnn.State == ConnectionState.Closed)
                            cnn.Open();

                        cmd.CommandText = query;

                        object o = cmd.ExecuteScalar();

                        if (o != null)
                            myout = (byte[])o;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString() + "\r\nMyGetImgInByte Error");
            }
            return myout;
        }
        public DataTable MyGetDuplicadoPorNumeroElectoral(String numElectoral, int whatIsModo)
        {
            DataTable myTableReturn = new DataTable();
            try
            {
                numElectoral = FixNum(numElectoral);
                // String mySqlstr = String.Format(
                //     @"select * from
                //     (
                //     select 
                //      (SELECT NumElec        FROM [dbo].[LotsEndo] where [NumElec] = {0} and [Status] = 0)                                                       as GoodNumElec,
                //      (SELECT NumElec        FROM [dbo].[LotsEndo] where [NumElec] = {0} and [Status] = 1 and ([Errores] like '%14%' or [Errores]  like '%15%')) as BadNumElec,
                //      (SELECT Partido        FROM [dbo].[LotsEndo] where [NumElec] = {0} and [Status] = 0)                                                       as GoodPartido,
                //      (SELECT Partido        FROM [dbo].[LotsEndo] where [NumElec] = {0} and [Status] = 1 and ([Errores] like '%14%' or [Errores]  like '%15%')) as BadPartido,
                //      (SELECT Lot            FROM [dbo].[LotsEndo] where [NumElec] = {0} and [Status] = 0)                                                       as GoodLot,
                //      (SELECT Lot            FROM [dbo].[LotsEndo] where [NumElec] = {0} and [Status] = 1 and ([Errores] like '%14%' or [Errores]  like '%15%')) as BadLot,
                //      (SELECT Batch          FROM [dbo].[LotsEndo] where [NumElec] = {0} and [Status] = 0)                                                       as GoodBatch,
                //      (SELECT Batch          FROM [dbo].[LotsEndo] where [NumElec] = {0} and [Status] = 1 and ([Errores] like '%14%' or [Errores]  like '%15%')) as BadBatch,
                //      (SELECT Formulario     FROM [dbo].[LotsEndo] where [NumElec] = {0} and [Status] = 0)                                                       as GoodFormulario,
                //      (SELECT Formulario     FROM [dbo].[LotsEndo] where [NumElec] = {0} and [Status] = 1 and ([Errores] like '%14%' or [Errores]  like '%15%')) as BadFormulario,
                //      (SELECT EndosoImage    FROM [dbo].[LotsEndo] where [NumElec] = {0} and [Status] = 0)                                                       as GoodEndosoImage,
                //      (SELECT EndosoImage    FROM [dbo].[LotsEndo] where [NumElec] = {0} and [Status] = 1 and ([Errores] like '%14%' or [Errores]  like '%15%')) as BadEndosoImage,
                //      (SELECT [Image]        FROM [dbo].[LotsEndo] where [NumElec] = {0} and [Status] = 0)                                                       as GoodPathImage,
                //      (SELECT [Image]        FROM [dbo].[LotsEndo] where [NumElec] = {0} and [Status] = 1 and ([Errores] like '%14%' or [Errores]  like '%15%')) as BadPathImage,
                //      (SELECT [Errores]      FROM [dbo].[LotsEndo] where [NumElec] = {0} and [Status] = 0 )                                                      as GoodErrores,
                //      (SELECT [Errores]      FROM [dbo].[LotsEndo] where [NumElec] = {0} and [Status] = 1 and ([Errores] like '%14%' or [Errores]  like '%15%')) as BadErrores
                //     ) as T
                //      order by T.GoodNumElec
                //      ", numElectoral);

                String mySqlstr = String.Format(
                    @"
                        SELECT [NumElec],[Partido],[Lot],[Batch],[Formulario],[Image],[Errores],[Status],[Modo] FROM [dbo].[LotsEndo] where [NumElec] = {0} and [Status] = 1 and ([Errores] like '%14%' or [Errores]  like '%15%') and Modo = {1}
                        union
                        SELECT [NumElec],[Partido],[Lot],[Batch],[Formulario],[Image],[Errores],[Status],[Modo] FROM [dbo].[LotsEndo] where [NumElec] = {0} and [Status] = 0 and Modo = {1}
                        order by [Status]
                    ", numElectoral, whatIsModo);


                using (SqlConnection cnn = new SqlConnection()
                {
                    ConnectionString = DBCnnStr
                })
                {
                    using (SqlCommand cmd = new SqlCommand()
                    {
                        Connection = cnn,
                        CommandType = CommandType.Text,
                    })
                    {
                        if (cnn.State == ConnectionState.Closed)
                            cnn.Open();

                        cmd.CommandText = String.Format(@"SELECT count(*) FROM [dbo].[LotsEndo] 
                                                          where [NumElec] = {0} and ([Errores] like '%14%' or [Errores]  like '%15%');", numElectoral);

                        object total = cmd.ExecuteScalar();

                        if (((int)total) <= 0)
                            return myTableReturn;

                        cmd.CommandText = mySqlstr;
                        using (SqlDataAdapter da = new SqlDataAdapter()
                        {
                            SelectCommand = cmd
                        })
                        {
                            da.Fill(myTableReturn);
                        }

                    }
                }
            }
            catch (Exception ex)
            {

                throw new Exception(ex.ToString() + "\r\nMyGetDuplicadoPorNumeroElectoral Error");
            }

            return myTableReturn;
        }
        public DataTable MyGetModo(int modoId)
        {
            DataTable myTableReturn = new DataTable();

            try
            {
                string[] mySqlstr = { "SELECT * ",
                                      "FROM [dbo].[tblModo] ",
                                      "where Modo =",modoId.ToString(),"" };

                using (SqlConnection cnn = new SqlConnection()
                {
                    ConnectionString = DBCnnStr
                })
                {
                    using (SqlCommand cmd = new SqlCommand()
                    {
                        Connection = cnn,
                        CommandType = CommandType.Text,
                        CommandText = string.Concat(mySqlstr)
                    })
                    {
                        if (cnn.State == ConnectionState.Closed)
                            cnn.Open();

                        using (SqlDataAdapter da = new SqlDataAdapter()
                        {
                            SelectCommand = cmd
                        })
                        {
                            da.Fill(myTableReturn);
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString() + "\r\nMyGetModo Error");
            }
            return myTableReturn;

        }
        public DataTable MyGeRechazadasToInforme(string lot)
        {

            DataTable myTableReturn = new DataTable();

            try
            {
                string[] mySqlstr = { "SELECT * ",
                                      "FROM [dbo].[LotsEndo] ",
                                      "where [Status] = 1 and lot='",lot,"'" };

                using (SqlConnection cnn = new SqlConnection()
                {
                    ConnectionString = DBCnnStr
                })
                {
                    using (SqlCommand cmd = new SqlCommand()
                    {
                        Connection = cnn,
                        CommandType = CommandType.Text,
                        CommandText = string.Concat(mySqlstr)
                    })
                    {
                        if (cnn.State == ConnectionState.Closed)
                            cnn.Open();

                        using (SqlDataAdapter da = new SqlDataAdapter()
                        {
                            SelectCommand = cmd
                        })
                        {
                            da.Fill(myTableReturn);
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString() + "\r\nMyGeRechazadasToInforme Error");
            }
            return myTableReturn;
        }

        public DataTable MyGetLotFix()
        {
            /*
             'STATUS LOTE
                '0 - LISTO PARA PROCESAR
                '1 - SIENDO PROCESADA
                '2 - FINALIZADO
                '3 - CON ERRORES
                '4 - SIENDO REVISADA
            */
            DataTable myTableReturn = new DataTable();
            try
            {
                string[] mySqlstr = { "Select Lot  ",
                                      "from From Lots ",
                                      "Where Status = 3;" };

                using (SqlConnection cnn = new SqlConnection()
                {
                    ConnectionString = DBCnnStr
                })
                {
                    using (SqlCommand cmd = new SqlCommand()
                    {
                        Connection = cnn,
                        CommandType = CommandType.Text,
                        CommandText = string.Concat(mySqlstr)
                    })
                    {
                        if (cnn.State == ConnectionState.Closed)
                            cnn.Open();

                        using (SqlDataAdapter da = new SqlDataAdapter()
                        {
                            SelectCommand = cmd
                        })
                        {
                            da.Fill(myTableReturn);
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString() + "\r\nMyGetLot Error");
            }
            return myTableReturn;
        }

        public int MyGetCatntidadDigitalizada(string lot)
        {
            int returnInt = 0;
            try
            {
                string mySqlstr = "Select  count(*) as total from [dbo].[TF-Partidos] Where [BatchTrack]='" + lot + "'";

                using (SqlConnection cnn = new SqlConnection()
                {
                    ConnectionString = DBCnnStr
                })
                {
                    using (SqlCommand cmd = new SqlCommand()
                    {
                        Connection = cnn,
                        CommandType = CommandType.Text,
                        CommandText = string.Concat(mySqlstr)
                    })
                    {
                        if (cnn.State == ConnectionState.Closed)
                            cnn.Open();

                        returnInt = (int)cmd.ExecuteScalar();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString() + "\r\n MyGetCatntidadDigitalizada");
            }
            return returnInt;
        }

        public int MyGetCatntidadEntregada(string lot)
        {
            int returnInt = 0;
            try
            {
                // string mySqlstr = "Select  distinct [BatchNo],[BatchPgCnt] as total from [dbo].[TF-Partidos] Where [BatchTrack]='" + lot + "'";
                string mySqlstr = "Select [FiledEndorsements] as total from [dbo].[FilingEndorsements] with (nolock) Where [BatchNumber]='" + lot + "'";

                using (SqlConnection cnn = new SqlConnection()
                {
                    ConnectionString = DBRadicacionesCEECnnStr
                })
                {
                    using (SqlCommand cmd = new SqlCommand()
                    {
                        Connection = cnn,
                        CommandType = CommandType.Text,
                        CommandText = string.Concat(mySqlstr)
                    })
                    {
                        if (cnn.State == ConnectionState.Closed)
                            cnn.Open();

                        using (SqlDataAdapter da = new SqlDataAdapter()
                        {
                            SelectCommand = cmd
                        })
                        {
                            SqlDataReader dr = cmd.ExecuteReader();

                            while (dr.Read())
                            {
                                int i = 0;

                                int.TryParse(dr["total"].ToString(), out i);
                                returnInt += i;
                            }



                        }

                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString() + "\r\nMyGetCatntidadEntregada");
            }
            return returnInt;
        }
        public DataTable MyGetLotFromTF()
        {
            DataTable myTableReturn = new DataTable();
            try
            {

                //string mySqlstr = "Select distinct BatchTrack  from [dbo].[TF-Partidos] Where Imported = 0 order by BatchTrack";
                string[] mySqlstr = { "Select [Partido],BatchTrack, count(*) as Amount  ",
                                      "from [dbo].[TF-Partidos] ",
                                      "Where Imported = 0 ",
                                      "Group By Partido,BatchTrack ",
                                      "Order By Partido,BatchTrack;" };

                using (SqlConnection cnn = new SqlConnection()
                {
                    ConnectionString = DBCnnStr
                })
                {
                    using (SqlCommand cmd = new SqlCommand()
                    {
                        Connection = cnn,
                        CommandType = CommandType.Text,
                        CommandText = string.Concat(mySqlstr)
                    })
                    {
                        if (cnn.State == ConnectionState.Closed)
                            cnn.Open();

                        using (SqlDataAdapter da = new SqlDataAdapter()
                        {
                            SelectCommand = cmd
                        })
                        {
                            da.Fill(myTableReturn);
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString() + "\r\nMyGetLot Error");
            }
            return myTableReturn;
        }
        public DataTable MyGetSelectLotes(string lot, string cantidad)
        {
            DataTable myTableReturn = new DataTable();
            bool myBoolError = true;
            try
            {
                int myIntCantidad = 0;

                if (!int.TryParse(cantidad, out myIntCantidad))
                    throw new Exception("Error en la Cantidad");

                string[] mySqlstrCount =
                {
                    "Select count(*) as Total ",
                    "From [dbo].[lots] ",
                    "Where lot=@lot ",
                    "And Status = 0;"
                };


                string[] mySqlstrExiste =
              {
                    "Select count(*) ",
                    "From [dbo].[TF-Partidos] ",
                    "Where BatchTrack =@lot ",
                    "And Imported = 0; "
                };


                string[] mySqlstrAll =
                {
                    "Select Partido,BatchTrack, count(*) as Amount ",
                    "From [dbo].[TF-Partidos] ",
                    "Where BatchTrack =@lot ",
                    "And Imported = 0 ",
                    "Group By Partido,BatchTrack ",
                    "Order By Partido,BatchTrack;"
                };

                string[] mySqlstrCountVerificar =
              {
                    "Select count(*) as Total ",
                    "From [dbo].[TF-Partidos] ",
                    "Where BatchTrack=@lot ",
                    "And Imported = 0;"
                };

                string[] mySqlstrCountVerificarMasDeUnPartido =
                {
                    "select count(*) as total ",
                    "from ( SELECT  Partido,count(*) as total ",
                    "FROM [dbo].[TF-Partidos] ",
                    "where BatchTrack = @lot ",
                    "group by Partido ",
                    ") as A"
                };


                using (SqlConnection cnn = new SqlConnection()
                {
                    ConnectionString = DBCnnStr
                })
                {
                    using (SqlCommand cmd = new SqlCommand()
                    {
                        Connection = cnn,
                        CommandType = CommandType.Text,

                    })
                    {
                        if (cnn.State == ConnectionState.Closed)
                            cnn.Open();

                        using (SqlDataAdapter da = new SqlDataAdapter()
                        {
                            SelectCommand = cmd
                        })
                        {

                            cmd.Parameters.Add(new SqlParameter("@lot", SqlDbType.VarChar)).Value = lot;


                            cmd.CommandText = string.Concat(mySqlstrCount);
                            if ((int)cmd.ExecuteScalar() > 0)
                                throw new Exception("Este Lote ya fue entrado");

                            cmd.CommandText = string.Concat(mySqlstrExiste);
                            if ((int)cmd.ExecuteScalar() == 0)
                                throw new Exception("Lote no Existe");

                            cmd.CommandText = string.Concat(mySqlstrCountVerificar);
                            if ((int)cmd.ExecuteScalar() != myIntCantidad)
                                throw new Exception("La cantidad de Endosos no concuerda");

                            cmd.CommandText = string.Concat(mySqlstrCountVerificarMasDeUnPartido);
                            if ((int)cmd.ExecuteScalar() > 1)
                                throw new Exception("La cantidad de Partidos en este Lot es mayor de 1\r\nSolo es permitido un partido por lote. favor verificar y subsanar la situacion.");


                            myBoolError = false;

                            cmd.CommandText = string.Concat(mySqlstrAll);
                            da.Fill(myTableReturn);
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                if (myBoolError)
                    throw new Exception(ex.Message);
                else
                    throw new Exception(ex.ToString() + "\r\n MyGetSelectLotes Error");
            }
            return myTableReturn;
        }
        public DataTable MyGetTodosLotes()
        {
            DataTable myTableReturn = new DataTable();
            try
            {

                string[] mySqlstrAll =
                {
                    "Select Partido,BatchTrack, count(*) as Amount ",
                    "From [dbo].[TF-Partidos] ",
                    "Where BatchTrack Is Not Null ",
                    "And Imported = 0 ",
                    "Group By Partido,BatchTrack ",
                    "Order By Partido,BatchTrack;"
                };



                using (SqlConnection cnn = new SqlConnection()
                {
                    ConnectionString = DBCnnStr
                })
                {
                    using (SqlCommand cmd = new SqlCommand()
                    {
                        Connection = cnn,
                        CommandType = CommandType.Text,
                        CommandText = string.Concat(mySqlstrAll)
                    })
                    {
                        if (cnn.State == ConnectionState.Closed)
                            cnn.Open();

                        using (SqlDataAdapter da = new SqlDataAdapter()
                        {
                            SelectCommand = cmd
                        })
                        {

                            da.Fill(myTableReturn);
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString() + "\r\nMyGetAreas Error");
            }
            return myTableReturn;
        }

        public DataTable MyGetCitizenImg(string CitizenID)
        {
            DataTable myTableReturn = new DataTable();
            try
            {

                string[] mySqlstr = { "Select * From [usercid].[CitizenImages] Where CitizenID =", CitizenID, "order by [CaptureDate] desc" };
                //       string[] mySqlstr = { "SELECT A.*,B.SignatureImage,B.PhotoImage ",
                //"FROM [usercid].[Citizen] A join [usercid].[CitizenImages] B ",
                //"on a.CItizenId = b.CitizenID ",
                //"where a.CItizenId =@CitizenID" };

                using (SqlConnection cnn = new SqlConnection()
                {
                    ConnectionString = DBImagenesCnnStr
                })
                {
                    using (SqlCommand cmd = new SqlCommand()
                    {
                        Connection = cnn,
                        CommandType = CommandType.Text,
                        CommandText = string.Concat(mySqlstr)
                    })
                    {
                        if (cnn.State == ConnectionState.Closed)
                            cnn.Open();

                        using (SqlDataAdapter da = new SqlDataAdapter()
                        {
                            SelectCommand = cmd
                        })
                        {
                            //cmd.Parameters.Add(new SqlParameter("@CitizenID", SqlDbType.VarChar));
                            //cmd.Parameters["@CitizenID"].Value = CitizenID;

                            da.Fill(myTableReturn);
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString() + "\r\nMyGetAreas Error");
            }
            return myTableReturn;
        }

        public byte[] GetEndosoImage(string filePath)
        {
            FileStream stream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            BinaryReader reader = new BinaryReader(stream);

            byte[] img = reader.ReadBytes((int)stream.Length);

            reader.Close();
            stream.Close();

            return img;
        }

        public DataTable MyGetCitizen(string CitizenID)
        {
            DataTable myTableReturn = new DataTable();
            try
            {

                string[] mySqlstr = { "Select * From [usercid].[Citizen] Where CitizenID =", CitizenID };

                //       string[] mySqlstr = { "SELECT A.*,B.SignatureImage,B.PhotoImage ",
                //"FROM [usercid].[Citizen] A join [usercid].[CitizenImages] B ",
                //"on a.CItizenId = b.CitizenID ",
                //"where a.CItizenId =@CitizenID" };

                using (SqlConnection cnn = new SqlConnection()
                {
                    ConnectionString = DBCeeMasterCnnStr
                })
                {
                    using (SqlCommand cmd = new SqlCommand()
                    {
                        Connection = cnn,
                        CommandType = CommandType.Text,

                    })
                    {
                        cmd.CommandText = string.Concat(mySqlstr);

                        if (cnn.State == ConnectionState.Closed)
                            cnn.Open();

                        using (SqlDataAdapter da = new SqlDataAdapter()
                        {
                            SelectCommand = cmd
                        })
                        {
                            //cmd.Parameters.Add(new SqlParameter("@CitizenID", SqlDbType.VarChar));
                            //cmd.Parameters["@CitizenID"].Value = CitizenID;

                            da.Fill(myTableReturn);
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString() + "\r\nMyGetAreas Error");
            }
            return myTableReturn;
        }
        public DataTable MyGetLot(string StatusReydi, string Status, int whatIsModo) //Status= "0,1,2,3,4"
        {
            DataTable myTableReturn = new DataTable();
            try
            {
                //'STATUS LOTE para la tabla lots
                //'0 - LISTO PARA PROCESAR
                //'1 - SIENDO PROCESADA
                //'2 - FINALIZADO
                //'3 - CON ERRORES
                //'4 - SIENDO REVISADA

                //string mySqlstr = "Select * from lots Where Status In (" + Status + ") and StatusReydi in (" + StatusReydi + ") order by Lot";

                string mySqlstr = string.Format("Select * from lots Where Status In ({0}) and StatusReydi in ({1}) and Modo = {2} order by Lot", Status, StatusReydi, whatIsModo);

                using (SqlConnection cnn = new SqlConnection()
                {
                    ConnectionString = DBCnnStr
                })
                {
                    using (SqlCommand cmd = new SqlCommand()
                    {
                        Connection = cnn,
                        CommandType = CommandType.Text,
                        CommandText = mySqlstr
                    })
                    {
                        if (cnn.State == ConnectionState.Closed)
                            cnn.Open();

                        using (SqlDataAdapter da = new SqlDataAdapter()
                        {
                            SelectCommand = cmd
                        })
                        {
                            da.Fill(myTableReturn);
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString() + "\r\n MyGetLot Error");
            }
            return myTableReturn;
        }
        public DataTable MyGetDuplicados(string lot)
        {
            DataTable myTableReturn = new DataTable();

            try
            {
                string mySqlstr = "Select * from [LotsEndo] Where lot = '" + lot + "' order by [Batch],[Formulario]";

                using (SqlConnection cnn = new SqlConnection()
                {
                    ConnectionString = DBCnnStr
                })
                {
                    using (SqlCommand cmd = new SqlCommand()
                    {
                        Connection = cnn,
                        CommandType = CommandType.Text,
                        CommandText = mySqlstr
                    })
                    {
                        if (cnn.State == ConnectionState.Closed)
                            cnn.Open();

                        using (SqlDataAdapter da = new SqlDataAdapter()
                        {
                            SelectCommand = cmd
                        })
                        {
                            da.Fill(myTableReturn);
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString() + "\r\n MyGetDuplicados Error");
            }
            return myTableReturn;

        }


        public DataTable MyGetLotInReydi(string StatusReydi = "1")
        {
            DataTable myTableReturn = new DataTable();
            try
            {
                //'StatusReydi LOTE para la tabla lots
                //'0 - no en reydi
                //'1 - si en reydi

                string mySqlstr = "Select * from lots Where StatusReydi In (" + StatusReydi + ") order by Lot";

                using (SqlConnection cnn = new SqlConnection()
                {
                    ConnectionString = DBCnnStr
                })
                {
                    using (SqlCommand cmd = new SqlCommand()
                    {
                        Connection = cnn,
                        CommandType = CommandType.Text,
                        CommandText = mySqlstr
                    })
                    {
                        if (cnn.State == ConnectionState.Closed)
                            cnn.Open();

                        using (SqlDataAdapter da = new SqlDataAdapter()
                        {
                            SelectCommand = cmd
                        })
                        {
                            da.Fill(myTableReturn);
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString() + "\r\n MyGetLotInReydi Error");
            }
            return myTableReturn;
        }
        public DataTable MyGetInfoEstatus(String Partido, bool isPartido)
        {
            DataTable myTableReturn = new DataTable();

            try
            {
                string[] mySqlstr = {
                   "Select ",
                   "(SELECT [Partido] FROM [dbo].[Candidatos]  where [Partido] = @Partido) as Partido ,",
                   "(SELECT [EndoReq] FROM [dbo].[Candidatos]  where [Partido] = @Partido) as Endosos_Requeridos,",
                   "(select count(*)  FROM [dbo].[LotsEndo]    where [Partido] = @Partido and [Status] = 0) as Endosos_Aceptados,",
                   "(select count(*)  FROM [dbo].[LotsEndo]    where [Partido] = @Partido and [Status] != 0) as Endosos_Rechazados,",
                   "(select count(*)  FROM [dbo].[LotsEndo]    where [Partido] = @Partido) as Endosos_Procesados,",
                   "(select count(*)  from [dbo].[TF-Partidos] where [Partido] = @Partido) as Endosos_Entregados"
                };
                string[] mySqlstrNoPartido = {
                    "SELECT * from ",
                    "(",
                    "    (SELECT partido,'Endosos_Requeridos' as Titulo, sum([EndoReq]) as Total FROM [dbo].[Candidatos] group by [Partido])",
                    "     union",
                    "    (select partido,'Endosos_Aceptados' as Titulo, count(*) as Total FROM [dbo].[LotsEndo]  where [Status] = 0 group by [Partido] )",
                    "     union",
                    "    (select partido,'Endosos_Rechazados' as Titulo ,count(*)  as Total FROM [dbo].[LotsEndo] where [Status] != 0 group by [Partido])",
                    "     union ",
                    "    (select partido,'Endosos_Procesados' as Titulo, count(*) as Total  FROM [dbo].[LotsEndo]  group by [Partido])",
                    "     union ",
                    "    (SELECT partido,'Endosos_Entregados' as Titulo, count(*)  as Total FROM [dbo].[TF-Partidos]  group by [Partido])",
                    ") ",
                    " as TT order by partido"
                };

                using (SqlConnection cnn = new SqlConnection()
                {
                    ConnectionString = DBCnnStr
                })
                {
                    using (SqlCommand cmd = new SqlCommand()
                    {
                        Connection = cnn,
                        CommandType = CommandType.Text,
                    })
                    {
                        if (isPartido)
                        {
                            cmd.CommandText = String.Concat(mySqlstr);
                        }
                        else
                        {
                            cmd.CommandText = String.Concat(mySqlstrNoPartido);
                        }

                        if (isPartido)
                        {
                            SqlParameter sqlParameterPartido = new SqlParameter("@Partido", SqlDbType.VarChar);
                            cmd.Parameters.Add(sqlParameterPartido).Value = Partido;
                        }

                        if (cnn.State == ConnectionState.Closed)
                            cnn.Open();

                        using (SqlDataAdapter da = new SqlDataAdapter()
                        {
                            SelectCommand = cmd
                        })
                        {
                            da.Fill(myTableReturn);
                        }

                    }
                }
            }
            catch (Exception ex)
            {

                throw new Exception(ex.ToString() + "\r\n MyGetInfoEstatus Error");

            }


            return myTableReturn;
        }

        public DataTable MyGetLotVoid(String Partido, String FechaDeRecibo, bool isDuplicados, bool isFechaRecibo, bool isPartidos)
        {
            DataTable myTableReturn = new DataTable();

            try
            {
                //partido and fecha de recibo
                string[] mySqlstrPartidoFecha = {
                    "SELECT A.[Partido],A.[Lot],A.[Batch],",
                    "       A.[Formulario] ,A.[Rechazo]," ,
                    "       (select [Desc] from [dbo].[Criterios] where [Campo] = A.Rechazo) as Causal ," ,
                    "       A.[NumElec],",
                    "       B.[Nombre],",
                    "       B.[Paterno],",
                    "       B.[Materno],",
                    "       (SELECT [Desc] FROM [dbo].[tblStatusLotsVoid] where [STATUS] = A.[Status]) as [Status],",
                    "       A.[EndosoImage],b.Fecha_Endoso",
                    "  FROM [dbo].[LotsVoid] A join [dbo].[LotsEndo] B",
                    "  on A.NumElec = B.NumElec",
                    "  where B.Status in (1,2) and B.Fecha_Endoso = @Fecha_Endoso and a.Partido = @Partido",
                    "  order by  A.Rechazo,A.Lot,A.NumElec"
                };
                //fecha de recibo
                string[] mySqlstrFecha = {
                    "SELECT A.[Partido],A.[Lot],A.[Batch],",
                    "       A.[Formulario] ,A.[Rechazo]," ,
                    "       (select [Desc] from [dbo].[Criterios] where [Campo] = A.Rechazo) as Causal," ,
                    "       A.[NumElec],",
                    "       B.[Nombre],",
                    "       B.[Paterno],",
                    "       B.[Materno],",
                    "       (SELECT [Desc] FROM [dbo].[tblStatusLotsVoid] where [STATUS] = A.[Status]) as [Status],",
                    "       A.[EndosoImage],b.Fecha_Endoso",
                    "  FROM [dbo].[LotsVoid] A join [dbo].[LotsEndo] B",
                    "  on A.NumElec = B.NumElec",
                    "  where B.Status in (1,2) and B.Fecha_Endoso = @Fecha_Endoso ",
                    "  order by  A.Rechazo,A.Lot,A.NumElec"
                };
                //Duplicados and partido
                string[] mySqlstrDuplicadosPartido = {
                    "SELECT A.[Partido],A.[Lot],A.[Batch],",
                    "       A.[Formulario] ,A.[Rechazo]," ,
                    "       (select [Desc] from [dbo].[Criterios] where [Campo] = A.Rechazo) as Causal," ,
                    "       A.[NumElec],",
                    "       B.[Nombre],",
                    "       B.[Paterno],",
                    "       B.[Materno],",
                    "       (SELECT [Desc] FROM [dbo].[tblStatusLotsVoid] where [STATUS] = A.[Status]) as [Status],",
                    "       A.[EndosoImage],b.Fecha_Endoso",
                    "  FROM [dbo].[LotsVoid] A join [dbo].[LotsEndo] B",
                    "  on A.NumElec = B.NumElec",
                    "  where B.Status in (1,2) and a.Partido = @Partido",
                    "  and [Rechazo] in (14,15,21)",
                    "  order by  A.Rechazo,A.Lot,A.NumElec"
                };

                //Duplicados and Fecha
                string[] mySqlstrDuplicadosFecha = {
                    "SELECT A.[Partido],A.[Lot],A.[Batch],",
                    "       A.[Formulario] ,A.[Rechazo]," ,
                    "       (select [Desc] from [dbo].[Criterios] where [Campo] = A.Rechazo) as Causal," ,
                    "       A.[NumElec],",
                    "       B.[Nombre],",
                    "       B.[Paterno],",
                    "       B.[Materno],",
                    "       (SELECT [Desc] FROM [dbo].[tblStatusLotsVoid] where [STATUS] = A.[Status]) as [Status],",
                    "       A.[EndosoImage],b.Fecha_Endoso",
                    "  FROM [dbo].[LotsVoid] A join [dbo].[LotsEndo] B",
                    "  on A.NumElec = B.NumElec",
                    "  where B.Status in (1,2) and B.Fecha_Endoso = @Fecha_Endoso",
                    "  and [Rechazo] in (14,15,21)",
                    "  order by  A.Rechazo,A.Lot,A.NumElec"
                };
                //Duplicados and Fecha and partido
                string[] mySqlstrDuplicadosFechaPartido = {
                    "SELECT A.[Partido],A.[Lot],A.[Batch],",
                    "       A.[Formulario] ,A.[Rechazo]," ,
                    "       (select [Desc] from [dbo].[Criterios] where [Campo] = A.Rechazo) as Causal," ,
                    "       A.[NumElec],",
                    "       B.[Nombre],",
                    "       B.[Paterno],",
                    "       B.[Materno],",
                    "       (SELECT [Desc] FROM [dbo].[tblStatusLotsVoid] where [STATUS] = A.[Status]) as [Status],",
                    "       A.[EndosoImage],b.Fecha_Endoso",
                    "  FROM [dbo].[LotsVoid] A join [dbo].[LotsEndo] B",
                    "  on A.NumElec = B.NumElec",
                    "  where B.Status in (1,2) and B.Fecha_Endoso = @Fecha_Endoso and a.Partido = @Partido",
                    "  and [Rechazo] in (14,15,21)",
                    "  order by  A.Rechazo,A.Lot,A.NumElec"
                };
                //Duplicados 
                string[] mySqlstrDuplicados = {
                    "SELECT A.[Partido],A.[Lot],A.[Batch],",
                    "       A.[Formulario] ,A.[Rechazo]," ,
                    "       (select [Desc] from [dbo].[Criterios] where [Campo] = A.Rechazo) as Causal," ,
                    "       A.[NumElec],",
                    "       B.[Nombre],",
                    "       B.[Paterno],",
                    "       B.[Materno],",
                    "       (SELECT [Desc] FROM [dbo].[tblStatusLotsVoid] where [STATUS] = A.[Status]) as [Status],",
                    "       A.[EndosoImage],b.Fecha_Endoso",
                    "  FROM [dbo].[LotsVoid] A join [dbo].[LotsEndo] B",
                    "  on A.NumElec = B.NumElec",
                    "  where B.Status in (1,2)",
                    "  and [Rechazo] in (14,15,21)",
                    "  order by  A.Rechazo,A.Lot,A.NumElec"
                };

                string[] mySqlstrPartidos = {
                    "SELECT A.[Partido],A.[Lot],A.[Batch],",
                    "       A.[Formulario] ,A.[Rechazo]," ,
                    "       (select [Desc] from [dbo].[Criterios] where [Campo] = A.Rechazo) as Causal ," ,
                    "       A.[NumElec],",
                    "       B.[Nombre],",
                    "       B.[Paterno],",
                    "       B.[Materno],",
                    "       (SELECT [Desc] FROM [dbo].[tblStatusLotsVoid] where [STATUS] = A.[Status]) as [Status],",
                    "       A.[EndosoImage],b.Fecha_Endoso",
                    "  FROM [dbo].[LotsVoid] A join [dbo].[LotsEndo] B",
                    "  on A.NumElec = B.NumElec",
                    "  where B.Status in (1,2) and a.Partido = @Partido",
                    "  order by  A.Rechazo,A.Lot,A.NumElec"
                };

                string[] mySqlstrTodos = {
                    "SELECT A.[Partido],A.[Lot],A.[Batch],",
                    "       A.[Formulario] ,A.[Rechazo]," ,
                    "      (select [Desc] from [dbo].[Criterios] where [Campo] = A.Rechazo) as Causal," ,
                    "       A.[NumElec],",
                    "       B.[Nombre],",
                    "       B.[Paterno],",
                    "       B.[Materno],",
                    "       (SELECT [Desc] FROM [dbo].[tblStatusLotsVoid] where [STATUS] = A.[Status]) as [Status],",
                    "       A.[EndosoImage],b.Fecha_Endoso",
                    "  FROM [dbo].[LotsVoid] A join [dbo].[LotsEndo] B",
                    "  on A.NumElec = B.NumElec",
                    "  where B.Status in (1,2) ",
                    "  order by  A.Rechazo,A.Lot,A.NumElec"
                };

                using (SqlConnection cnn = new SqlConnection()
                {
                    ConnectionString = DBCnnStr
                })
                {
                    using (SqlCommand cmd = new SqlCommand()
                    {
                        Connection = cnn,
                        CommandType = CommandType.Text,
                    })
                    {
                        SqlParameter sqlParameterPartido = new SqlParameter("@Partido", SqlDbType.VarChar);
                        SqlParameter sqlParameterFecha = new SqlParameter("@Fecha_Endoso", SqlDbType.VarChar);

                        cmd.Parameters.Add(sqlParameterPartido).Value = Partido;
                        cmd.Parameters.Add(sqlParameterFecha).Value = FechaDeRecibo;

                        if ((isPartidos) && (isFechaRecibo) && (!isDuplicados))
                        {
                            cmd.CommandText = String.Concat(mySqlstrPartidoFecha);
                        }
                        if ((isPartidos) && (isFechaRecibo) && (isDuplicados))
                        {
                            cmd.CommandText = String.Concat(mySqlstrDuplicadosFechaPartido);
                        }

                        if ((!isPartidos) && (isFechaRecibo) && (!isDuplicados))
                        {
                            cmd.CommandText = String.Concat(mySqlstrFecha);
                            cmd.Parameters.Remove(sqlParameterPartido);
                        }

                        if ((isPartidos) && (!isFechaRecibo) && (isDuplicados))
                        {
                            cmd.CommandText = String.Concat(mySqlstrDuplicadosPartido);
                            cmd.Parameters.Remove(sqlParameterFecha);
                        }

                        if ((!isPartidos) && (!isFechaRecibo) && (isDuplicados))
                        {
                            cmd.CommandText = String.Concat(mySqlstrDuplicados);
                            cmd.Parameters.Remove(sqlParameterPartido);
                            cmd.Parameters.Remove(sqlParameterFecha);
                        }

                        if ((isPartidos) && (!isFechaRecibo) && (!isDuplicados))
                        {
                            cmd.CommandText = String.Concat(mySqlstrPartidos);
                            cmd.Parameters.Remove(sqlParameterFecha);
                        }

                        if ((!isPartidos) && (!isFechaRecibo) && (!isDuplicados))
                        {
                            cmd.CommandText = String.Concat(mySqlstrTodos);
                            cmd.Parameters.Remove(sqlParameterPartido);
                            cmd.Parameters.Remove(sqlParameterFecha);
                        }

                        if ((!isPartidos) && (isFechaRecibo) && (isDuplicados))
                        {
                            cmd.CommandText = String.Concat(mySqlstrDuplicadosFecha);
                            cmd.Parameters.Remove(sqlParameterPartido);
                        }


                        if (cmd.CommandText.Trim().Length == 0)
                        {
                            MessageBox.Show("Problemas con el query");
                            return myTableReturn;
                            //cmd.CommandText = String.Concat(mySqlstrTodos);
                            //cmd.Parameters.Remove(sqlParameterPartido);
                            //cmd.Parameters.Remove(sqlParameterFecha);
                        }

                        if (cnn.State == ConnectionState.Closed)
                            cnn.Open();

                        using (SqlDataAdapter da = new SqlDataAdapter()
                        {
                            SelectCommand = cmd
                        })
                        {
                            da.Fill(myTableReturn);
                        }

                    }
                }

            }
            catch (Exception ex)
            {

                throw new Exception(ex.ToString() + "\r\n MyGetLotVoid Error");
            }

            return myTableReturn;
        }

        public DataTable MyGetLotToFixVoid(string CurrLot, bool isAll = false)
        {
            DataTable myTableReturn = new DataTable();
            try
            {
                //'STATUS LOTE
                //'0 - LISTO PARA PROCESAR
                //'1 - SIENDO PROCESADA
                //'2 - FINALIZADO
                //'3 - CON ERRORES
                //'4 - SIENDO REVISADA

                /*
                  'VERIFICA SI HAY LOTES PARA CORRECCION
                */


                //string[] mySqlstr =
                //    {
                //    "SELECT Lots.Lot, LotsEndo.Formulario, Lots.Status as LotRechazado, LotsEndo.Status as FormlularioRechazado,LotsVoid.Status as Rechazo_Or_Warning, LotsVoid.Rechazo as TipoDeRechazo,",
                //    "LotsEndo.Numelec, LotsEndo.Precinto, LotsEndo.FechaNac, LotsEndo.Sexo, LotsEndo.Candidato,",
                //    "LotsEndo.Cargo, LotsEndo.Notario, Lots.Conditions, LotsEndo.Lot, LotsEndo.Batch, LotsEndo.image,LotsEndo.Firma_Peticionario,LotsEndo.Firma_Pet_Inv, LotsEndo.Firma_Notario, LotsEndo.Firma_Not_Inv, LotsEndo.Fecha_Endoso,LotsEndo.Firma_Fecha,LotsEndo.Formulario ",
                //    "FROM (LotsEndo INNER JOIN Lots ON LotsEndo.Lot = Lots.Lot) INNER JOIN LotsVoid ON (LotsEndo.Formulario = LotsVoid.Formulario) AND (LotsEndo.Lot = LotsVoid.Lot) ",
                //    "WHERE Lots.Status = 3 ",
                //    "and Lots.Lot = '" , CurrLot , "' ",
                //    "ORDER BY Lots.Lot, LotsEndo.Formulario; "
                //};

                //string[] mySqlstr =
                //{
                //    "SELECT a.Lot,b.*  FROM [dbo].[Lots] A ", 
                //    "join [dbo].[TF-Partidos] B on a.Lot = b.BatchTrack  where A.Status = 3 ",
                //    "and A.lot ='",CurrLot,"' ",
                //    "order by  [BatchNo],[BatchPgNo]"
                //};
                string[] mySqlstr;
                if (!isAll)
                    mySqlstr = new string[]
                    {
                   "SELECT a.Lot,b.* FROM [dbo].[Lots] A ",
                   "join [dbo].[LotsEndo] B ",
                   "on a.Lot = b.Lot where A.Status = 3 ",
                   "and A.lot ='", CurrLot,"' AND b.Status = 1 ",
                   "order by  [Batch], [Formulario]"
                    };
                else
                    mySqlstr = new string[]
                    {
                   "SELECT * ",
                   "from [dbo].[LotsEndo] ",
                   "where lot ='", CurrLot,"' ",
                   "order by  [Batch], [Formulario]"
                    };


                using (SqlConnection cnn = new SqlConnection()
                {
                    ConnectionString = DBCnnStr
                })
                {
                    using (SqlCommand cmd = new SqlCommand()
                    {
                        Connection = cnn,
                        CommandType = CommandType.Text,
                        CommandText = string.Concat(mySqlstr)
                    })
                    {
                        if (cnn.State == ConnectionState.Closed)
                            cnn.Open();

                        using (SqlDataAdapter da = new SqlDataAdapter()
                        {
                            SelectCommand = cmd
                        })
                        {
                            da.Fill(myTableReturn);
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString() + "\r\nMyGetLotToProcess Error");
            }
            return myTableReturn;
        }
        public DataTable MyGetLotToProcess(int modo)
        {
            DataTable myTableReturn = new DataTable();
            try
            {
                //'STATUS LOTE
                //'0 - LISTO PARA PROCESAR
                //'1 - SIENDO PROCESADA
                //'2 - FINALIZADO
                //'3 - CON ERRORES
                //'4 - SIENDO REVISADA

                string mySqlstr = string.Format("Select * from lots Where Status ={0} and Modo={1} order by Lot", 0, modo);

                using (SqlConnection cnn = new SqlConnection()
                {
                    ConnectionString = DBCnnStr
                })
                {
                    using (SqlCommand cmd = new SqlCommand()
                    {
                        Connection = cnn,
                        CommandType = CommandType.Text,
                        CommandText = mySqlstr
                    })
                    {
                        if (cnn.State == ConnectionState.Closed)
                            cnn.Open();

                        using (SqlDataAdapter da = new SqlDataAdapter()
                        {
                            SelectCommand = cmd
                        })
                        {
                            da.Fill(myTableReturn);
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString() + "\r\nMyGetLotToProcess Error");
            }
            return myTableReturn;
        }
        public DataTable MyGetAreas(bool allColumm)
        {
            DataTable myTableReturn = new DataTable();
            try
            {
                string mySqlstr = "Select Area + ' - ' + [Desc] from areas order by area";

                if (allColumm)
                    mySqlstr = "Select * from areas order by area";

                using (SqlConnection cnn = new SqlConnection()
                {
                    ConnectionString = DBCnnStr
                })
                {
                    using (SqlCommand cmd = new SqlCommand()
                    {
                        Connection = cnn,
                        CommandType = CommandType.Text,
                        CommandText = mySqlstr
                    })
                    {
                        if (cnn.State == ConnectionState.Closed)
                            cnn.Open();

                        using (SqlDataAdapter da = new SqlDataAdapter()
                        {
                            SelectCommand = cmd
                        })
                        {
                            da.Fill(myTableReturn);
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString() + "\r\nMyGetAreas Error");
            }
            return myTableReturn;
        }


        public DataTable MyGetUsers()
        {
            DataTable myTableReturn = new DataTable();
            try
            {
                string mySqlstr = "Select * from tblUsers order by username";

                using (SqlConnection cnn = new SqlConnection()
                {
                    ConnectionString = DBCnnStr
                })
                {

                    using (SqlCommand cmd = new SqlCommand()
                    {
                        Connection = cnn,
                        CommandType = CommandType.Text,
                        CommandText = mySqlstr
                    })
                    {
                        if (cnn.State == ConnectionState.Closed)
                            cnn.Open();

                        using (SqlDataAdapter da = new SqlDataAdapter()
                        {
                            SelectCommand = cmd
                        })
                        {
                            da.Fill(myTableReturn);
                        }

                    }//end using 2
                }//end using 1
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString() + "\r\nMyGetUsers Error");
            }

            return myTableReturn;
        }

        public DataTable MyGetPrecintos()
        {
            DataTable myTableReturn = new DataTable();
            try
            {
                //string mySqlstr = "Select Precinto + ' - ' + [Desc] from Precintos order by Precinto";
                string mySqlstr = "Select * from Precintos order by Precinto";

                using (SqlConnection cnn = new SqlConnection()
                {
                    ConnectionString = DBCnnStr
                })
                {
                    using (SqlCommand cmd = new SqlCommand()
                    {
                        Connection = cnn,
                        CommandType = CommandType.Text,
                        CommandText = mySqlstr
                    })
                    {
                        if (cnn.State == ConnectionState.Closed)
                            cnn.Open();

                        using (SqlDataAdapter da = new SqlDataAdapter()
                        {
                            SelectCommand = cmd
                        })
                        {
                            da.Fill(myTableReturn);
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString() + "\r\nMyGetAreas Error");
            }
            return myTableReturn;
        }
        public DataTable MyGetPrecintosValidos(string area)
        {
            DataTable myTableReturn = new DataTable();
            try
            {
                string mySqlstr = "Select [Desc],PRECINTOS from AREAS where Area='" + area + "'";

                using (SqlConnection cnn = new SqlConnection()
                {
                    ConnectionString = DBCnnStr
                })
                {
                    using (SqlCommand cmd = new SqlCommand()
                    {
                        Connection = cnn,
                        CommandType = CommandType.Text,
                        CommandText = mySqlstr
                    })
                    {
                        if (cnn.State == ConnectionState.Closed)
                            cnn.Open();

                        using (SqlDataAdapter da = new SqlDataAdapter()
                        {
                            SelectCommand = cmd
                        })
                        {
                            da.Fill(myTableReturn);
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString() + "\r\nMyGetAreas Error");
            }
            return myTableReturn;
        }
        public DataTable MyGetCriterios(int modo)
        {
            DataTable myTableReturn = new DataTable();
            try
            {
                //string mySqlstr = "Select Precinto + ' - ' + [Desc] from Precintos order by Precinto";
                string mySqlstr = "Select * from Criterios order by Campo";

                if (modo == 2)
                    mySqlstr = "Select * from CriteriosPartido order by Campo";

                using (SqlConnection cnn = new SqlConnection()
                {
                    ConnectionString = DBCnnStr
                })
                {
                    using (SqlCommand cmd = new SqlCommand()
                    {
                        Connection = cnn,
                        CommandType = CommandType.Text,
                        CommandText = mySqlstr
                    })
                    {
                        if (cnn.State == ConnectionState.Closed)
                            cnn.Open();

                        using (SqlDataAdapter da = new SqlDataAdapter()
                        {
                            SelectCommand = cmd
                        })
                        {
                            da.Fill(myTableReturn);
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString() + "\r\nMyGetCriterio Error");
            }
            return myTableReturn;
        }
        public DataTable MyGetPartidos(int whatIsModo)
        {
            DataTable myTableReturn = new DataTable();
            try
            {
                //string mySqlstr = "Select Area + ' - ' + [Desc] from areas order by area";
                string mySqlstr = string.Format("Select * from Partidos where Modo={0} order by partido", whatIsModo);

                using (SqlConnection cnn = new SqlConnection()
                {
                    ConnectionString = DBCnnStr
                })
                {
                    using (SqlCommand cmd = new SqlCommand()
                    {
                        Connection = cnn,
                        CommandType = CommandType.Text,
                        CommandText = mySqlstr
                    })
                    {
                        if (cnn.State == ConnectionState.Closed)
                            cnn.Open();

                        using (SqlDataAdapter da = new SqlDataAdapter()
                        {
                            SelectCommand = cmd
                        })
                        {
                            da.Fill(myTableReturn);
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString() + "\r\nMyGetPartidos Error");
            }
            return myTableReturn;
        }

        public object MyGetPartidoTF(string lot)
        {
            object returnStr = "";
            try
            {
                string mySqlstr = "Select  [Partido] as total from [dbo].[TF-Partidos] Where [BatchTrack]='" + lot + "'";

                using (SqlConnection cnn = new SqlConnection()
                {
                    ConnectionString = DBCnnStr
                })
                {
                    using (SqlCommand cmd = new SqlCommand()
                    {
                        Connection = cnn,
                        CommandType = CommandType.Text,
                        CommandText = string.Concat(mySqlstr)
                    })
                    {
                        if (cnn.State == ConnectionState.Closed)
                            cnn.Open();
                        returnStr = cmd.ExecuteScalar();

                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString() + "\r\n MyGetCatntidadDigitalizada");
            }
            return returnStr;
        }


        public DataTable MyGetPartidos(String IdPartido)
        {
            DataTable myTableReturn = new DataTable();
            try
            {
                //string mySqlstr = "Select Area + ' - ' + [Desc] from areas order by area";
                string mySqlstr = "Select * from Partidos where id=" + IdPartido + ";";

                using (SqlConnection cnn = new SqlConnection()
                {
                    ConnectionString = DBCnnStr
                })
                {
                    using (SqlCommand cmd = new SqlCommand()
                    {
                        Connection = cnn,
                        CommandType = CommandType.Text,
                        CommandText = mySqlstr
                    })
                    {
                        if (cnn.State == ConnectionState.Closed)
                            cnn.Open();

                        using (SqlDataAdapter da = new SqlDataAdapter()
                        {
                            SelectCommand = cmd
                        })
                        {
                            da.Fill(myTableReturn);
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString() + "\r\nMyGetPartidos Error");
            }
            return myTableReturn;
        }


        public DataTable MyGetNotarios(int modo)
        {
            DataTable myTableReturn = new DataTable();
            try
            {

                string mySqlstr = string.Format("Select * from notarios where Modo={0} order by Nombre", modo);
                if (modo == 2)
                    mySqlstr = string.Format("Select * from notariosPartidos where Modo={0} order by Nombre", modo);

                using (SqlConnection cnn = new SqlConnection()
                {
                    ConnectionString = DBCnnStr
                })
                {
                    using (SqlCommand cmd = new SqlCommand()
                    {
                        Connection = cnn,
                        CommandType = CommandType.Text,
                        CommandText = mySqlstr
                    })
                    {
                        if (cnn.State == ConnectionState.Closed)
                            cnn.Open();

                        using (SqlDataAdapter da = new SqlDataAdapter()
                        {
                            SelectCommand = cmd
                        })
                        {
                            da.Fill(myTableReturn);
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString() + "\r\nMyGetPartidos Error");
            }
            return myTableReturn;
        }
        public DataTable MyGetCandidatos(int modo)
        {
            DataTable myTableReturn = new DataTable();
            try
            {
                string mySqlstr = string.Format("Select * from Candidatos where modo={0} order by Nombre", modo);

                using (SqlConnection cnn = new SqlConnection()
                {
                    ConnectionString = DBCnnStr
                })
                {
                    using (SqlCommand cmd = new SqlCommand()
                    {
                        Connection = cnn,
                        CommandType = CommandType.Text,
                        CommandText = mySqlstr
                    })
                    {
                        if (cnn.State == ConnectionState.Closed)
                            cnn.Open();

                        using (SqlDataAdapter da = new SqlDataAdapter()
                        {
                            SelectCommand = cmd
                        })
                        {
                            da.Fill(myTableReturn);
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString() + "\r\nMyGetCandidatos Error");
            }
            return myTableReturn;
        }
        public string MyDecRechazoToInforme(string param, string Candidato, string NumElec)
        {
            string myReturn = string.Empty;

            string[] rechazos = param.Split('|');

            try
            {
                using (SqlConnection cnn = new SqlConnection()
                {
                    ConnectionString = DBCnnStr
                })
                {
                    using (SqlCommand cmd = new SqlCommand()
                    {
                        Connection = cnn,
                        CommandType = CommandType.Text,

                    })
                    {
                        if (cnn.State == ConnectionState.Closed)
                            cnn.Open();

                        foreach (string Campo in rechazos)
                        {
                            if (Campo.Trim().Length > 0)
                            {
                                string mySqlstr = "SELECT [Desc]  FROM [Criterios] where Campo = " + Campo;
                                cmd.CommandText = mySqlstr;

                                myReturn += Campo + "-" + cmd.ExecuteScalar().ToString().Trim();

                                if (Campo == "14")
                                {
                                    if (!string.IsNullOrEmpty(Candidato))
                                    {
                                        if (!string.IsNullOrEmpty(NumElec))
                                        {
                                            mySqlstr = "SELECT [Lot]  FROM [dbo].[LotsEndo] where [Candidato] = " + Candidato + " and NumElec = " + NumElec + " and Status = 0";
                                            cmd.CommandText = mySqlstr;
                                            myReturn += " (" + cmd.ExecuteScalar().ToString().Trim() + ")";
                                        }
                                    }
                                }

                                myReturn += "\n\r";

                            }
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString() + "\r\nMyDecRechazoToInforme Error");
            }

            return myReturn;
        }

        public string MyCEEStatusToInforme(string CItizenId)
        {
            object myReturn = null;



            try
            {
                using (SqlConnection cnn = new SqlConnection()
                {
                    ConnectionString = DBCeeMasterCnnStr
                })
                {
                    using (SqlCommand cmd = new SqlCommand()
                    {
                        Connection = cnn,
                        CommandType = CommandType.Text,

                    })
                    {
                        if (cnn.State == ConnectionState.Closed)
                            cnn.Open();

                        string mySqlstr = "SELECT Status FROM [usercid].[Citizen] where[CItizenId] = '" + CItizenId + "'";
                        cmd.CommandText = mySqlstr;
                        myReturn = cmd.ExecuteScalar();
                        if (myReturn == null)
                            myReturn = "???";


                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString() + "\r\n MyCandidatoNameToInforme Error");
            }

            return myReturn.ToString();
        }

        public bool MySendToReydi(ObservableCollection<LotsToReydi> listaDeLotes, string sysUser, string partido)
        {
            bool myReturn = false;
            string lot = string.Empty;
            string Num_Candidato = string.Empty;

            try
            {
                string tableReydi = string.Empty;

                switch (partido)
                {
                    case "PNP":
                        tableReydi = "EndososPNP";
                        break;
                    case "PPD":
                        tableReydi = "EndososPPD";
                        break;
                    case "SEC":
                        tableReydi = "EndososCEE";
                        break;
                    default:
                        throw new Exception("Error con el Partido");
                }

                using (SqlConnection cnn = new SqlConnection()
                {
                    ConnectionString = DBCnnStr
                })
                {
                    using (SqlCommand cmd = new SqlCommand()
                    {
                        Connection = cnn,
                        CommandType = CommandType.Text,

                    })
                    {
                        if (cnn.State == ConnectionState.Closed)
                            cnn.Open();


                        using (SqlConnection cnnReydi = new SqlConnection()
                        {
                            ConnectionString = DBRadicacionesCEECnnStr
                        })
                        {
                            if (cnnReydi.State == ConnectionState.Closed)
                                cnnReydi.Open();

                            using (SqlCommand cmdReydi = new SqlCommand()
                            {
                                Connection = cnnReydi,
                                CommandType = CommandType.Text

                            })
                            {
                                foreach (LotsToReydi item in listaDeLotes)
                                {
                                    lot = item.Lot;
                                    Num_Candidato = item.Num_Candidato;
                                    //insert reydi log
                                    string insertReydiLog = "INSERT INTO [dbo].[tblLog]";
                                    insertReydiLog += " ([EndorsementGroupCode],[SysDate],[SysUser],[ElectoralNumber]) VALUES('";
                                    insertReydiLog += item.Lot + "',getdate(),'" + sysUser + "'," + item.Num_Candidato + ");";
                                    cmdReydi.CommandText = insertReydiLog;
                                    cmdReydi.ExecuteNonQuery();

                                    //update
                                    string queryUpdateLot = "update [Lots] set [StatusReydi]=1,[FinDate]=getdate(),[FinUser]='" + sysUser + "' ";
                                    queryUpdateLot += "where lot='" + item.Lot.Trim() + "'";
                                    cmd.CommandText = queryUpdateLot;
                                    cmd.ExecuteNonQuery();

                                    // insert into reydi
                                    string insertReydi = "INSERT INTO " + tableReydi;
                                    insertReydi += "(EndorsementGroupCode,ValidatedEndorsements,RejectedEndorsements,EndorsementValidationDate) VALUES('";
                                    insertReydi += item.Lot + "'," + item.ValidatedEndorsements.ToString() + "," + item.RejectedEndorsements.ToString() + ",'" + item.EndorsementValidationDate + "');";
                                    cmdReydi.CommandText = insertReydi;
                                    cmdReydi.ExecuteNonQuery();
                                }
                            }
                        }

                    }
                }
                myReturn = true;
            }
            catch (SqlException sqlEX)
            {
                if (sqlEX.Message.Contains("FOREIGN KEY constraint"))
                {
                    throw new Exception("Numero de Radicacion " + lot + " es invalido en el Sistema de Reydi o el nuemero del candidato es incorrecto " + Num_Candidato + " \r\n MySendToReydi Error");
                }
                else
                {
                    MethodBase site = sqlEX.TargetSite;
                    throw new Exception(sqlEX.Message + "\r\n" + site.Name);

                }
            }
            catch (Exception ex)
            {
                MethodBase site = ex.TargetSite;
                throw new Exception(ex.Message + "\r\n" + site.Name);

            }
            return myReturn;
        }
        public object MyReydiEndososDate(string lot)
        {
            object myReturn = null;


            try
            {
                using (SqlConnection cnn = new SqlConnection()
                {
                    ConnectionString = DBRadicacionesCEECnnStr
                })
                {
                    using (SqlCommand cmd = new SqlCommand()
                    {
                        Connection = cnn,
                        CommandType = CommandType.Text,

                    })
                    {
                        if (cnn.State == ConnectionState.Closed)
                            cnn.Open();

                        string mySqlstr = "SELECT [BatchFilingDate] FROM [dbo].[FilingEndorsements] with (nolock) where [BatchNumber] = '" + lot + "'";
                        cmd.CommandText = mySqlstr;
                        myReturn = cmd.ExecuteScalar();
                        if (myReturn == null)
                            myReturn = "???";


                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + "\r\n MyCandidatoNameToInforme Error");
            }

            return myReturn;
        }

        public bool MyTFJuramentoDate(string lot, DateTime EndososDate)
        {
            bool myReturn = false;

            try
            {
                using (SqlConnection cnn = new SqlConnection()
                {
                    ConnectionString = DBCnnStr
                })
                {
                    using (SqlCommand cmd = new SqlCommand()
                    {
                        Connection = cnn,
                        CommandType = CommandType.Text,

                    })
                    {
                        if (cnn.State == ConnectionState.Closed)
                            cnn.Open();

                        DateTime? m_Firma_Fecha = null;

                        string mySqlstr = "SELECT [BatchTrack],[BatchNo],[BatchPgNo],[FechaFirm_Dia],[FechaFirm_Mes],[FechaFirm_Ano],[FechaEndo_Mes],[FechaEndo_Dia],[FechaEndo_Ano] FROM [dbo].[TF-Partidos] where [BatchTrack] = '" + lot + "'";
                        cmd.CommandText = mySqlstr;
                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        DataTable table = new DataTable();

                        da.Fill(table);

                        foreach (DataRow dr in table.Rows)
                        {
                            string tmpmFirma_Fecha = string.Concat(dr["FechaEndo_Mes"].ToString().Trim().PadLeft(2, '0'),
                                                                   dr["FechaEndo_Dia"].ToString().Trim().PadLeft(2, '0'),
                                                                   dr["FechaEndo_Ano"].ToString().Trim().PadLeft(2, '0'));

                            if (tmpmFirma_Fecha.Trim().Length > 0)
                            {
                                string BatchNo = dr["BatchNo"].ToString();
                                string BatchPgNo = dr["BatchPgNo"].ToString();
                                m_Firma_Fecha = DateTimeUtil.MyValidarFechaMMddyy(tmpmFirma_Fecha);

                                long totalDeDias = DateTimeUtil.DateDiff(DateInterval.Day, m_Firma_Fecha, EndososDate);

                                if (totalDeDias > 7)
                                {
                                    totalDeDias = DateTimeUtil.DiasDisfrute(m_Firma_Fecha, EndososDate);
                                    //                                    totalDeDias = totalDeDias - 2;
                                }

                                string update = "update [dbo].[TF-Partidos] set [Leer_Inv]=" + totalDeDias + " where BatchTrack='" + lot + "' and BatchNo='" + BatchNo + "' and BatchPgNo='" + BatchPgNo + "';";
                                cmd.CommandText = update;
                                cmd.ExecuteNonQuery();

                            }
                        }
                        long totalDeDiasDeEntregada = DateTimeUtil.DateDiff(DateInterval.Day, EndososDate, DateTime.Now);

                        if (totalDeDiasDeEntregada > 20)
                        {
                            MessageBox.Show("La radicacion " + lot + " tiene mas de 20 días.\r\n Fecha de Entrega en CEE:" + EndososDate + "\r\n Total de Días:" + totalDeDiasDeEntregada.ToString() + "\r\n Favor verificar...", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                        }
                        myReturn = true;

                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + "\r\n MyTFJuramentoDate Error");
            }

            return myReturn;
        }

        public string MyCEEPrecintoToInforme(string CItizenId)
        {
            object myReturn = null;

            //FirstGeoCode

            try
            {
                using (SqlConnection cnn = new SqlConnection()
                {
                    ConnectionString = DBCeeMasterCnnStr
                })
                {
                    using (SqlCommand cmd = new SqlCommand()
                    {
                        Connection = cnn,
                        CommandType = CommandType.Text,

                    })
                    {
                        if (cnn.State == ConnectionState.Closed)
                            cnn.Open();

                        string mySqlstr = "SELECT FirstGeoCode FROM [usercid].[Citizen] where[CItizenId] = '" + CItizenId + "'";
                        cmd.CommandText = mySqlstr;
                        myReturn = cmd.ExecuteScalar();
                        if (myReturn == null)
                            myReturn = "???";


                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString() + "\r\n MyCandidatoNameToInforme Error");
            }

            return myReturn.ToString();
        }

        public string MyCEENameToInforme(string CItizenId)
        {
            object myReturn = null;

            //FirstGeoCode

            try
            {
                using (SqlConnection cnn = new SqlConnection()
                {
                    ConnectionString = DBCeeMasterCnnStr
                })
                {
                    using (SqlCommand cmd = new SqlCommand()
                    {
                        Connection = cnn,
                        CommandType = CommandType.Text,

                    })
                    {
                        if (cnn.State == ConnectionState.Closed)
                            cnn.Open();

                        string mySqlstr = "SELECT ltrim(rtrim([FirstName])) + ' ' + ltrim(rtrim([LastName1])) + ' ' + ltrim(rtrim([LastName2]))  as Nombre FROM [usercid].[Citizen] where[CItizenId] = '" + CItizenId + "'";
                        cmd.CommandText = mySqlstr;
                        myReturn = cmd.ExecuteScalar();
                        if (myReturn == null)
                            myReturn = "???";


                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString() + "\r\n MyCandidatoNameToInforme Error");
            }

            return myReturn.ToString();
        }

        public string MyCandidatoNameToInforme(string NumCand)
        {
            object myReturn = null;

            try
            {
                using (SqlConnection cnn = new SqlConnection()
                {
                    ConnectionString = DBCnnStr
                })
                {
                    using (SqlCommand cmd = new SqlCommand()
                    {
                        Connection = cnn,
                        CommandType = CommandType.Text,

                    })
                    {
                        if (cnn.State == ConnectionState.Closed)
                            cnn.Open();


                        string mySqlstr = "SELECT [Nombre] FROM [Candidatos] where [NumCand] = '" + NumCand + "'";
                        cmd.CommandText = mySqlstr;
                        myReturn = cmd.ExecuteScalar();
                        if (myReturn == null)
                            myReturn = "???";


                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString() + "\r\n MyCandidatoNameToInforme Error");
            }

            return myReturn.ToString().Trim();
        }
        public string MyDecCargoToInforme(string Cargo)
        {
            object myReturn = null;



            try
            {
                if (string.IsNullOrEmpty(Cargo))
                    return "???";
                else
                {
                    int cargo = 0;

                    if (!int.TryParse(Cargo, out cargo))
                        return "???";

                    if (cargo <= 0)
                        return "???";
                }

                using (SqlConnection cnn = new SqlConnection()
                {
                    ConnectionString = DBCnnStr
                })
                {
                    using (SqlCommand cmd = new SqlCommand()
                    {
                        Connection = cnn,
                        CommandType = CommandType.Text,

                    })
                    {
                        if (cnn.State == ConnectionState.Closed)
                            cnn.Open();




                        string mySqlstr = "SELECT [Desc]  FROM [tblCargos] where [Cargo] = " + Cargo;
                        cmd.CommandText = mySqlstr;
                        myReturn = cmd.ExecuteScalar();

                        if (myReturn == null)
                            myReturn = "???";


                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString() + "\r\nMyDecCargoToInforme Error");
            }

            return myReturn.ToString();
        }



        public string MyTipoDeRechazo(string param, string formulario, string CurrLot, string batch, ref byte[] EndosoImage, ref string rechazoNum)
        {
            string myReturn = string.Empty;
            try
            {

                string[] tipoDeRechazo = { "Select * from LotsVoid ",
                                           "where lot='",CurrLot,"' and Formulario=",formulario,
                                            " and Batch='",batch,"'"};


                using (SqlConnection cnn = new SqlConnection()
                {
                    ConnectionString = DBCnnStr
                })
                {
                    using (SqlCommand cmd = new SqlCommand()
                    {
                        Connection = cnn,
                        CommandType = CommandType.Text,
                        CommandText = string.Concat(tipoDeRechazo)
                    })
                    {
                        if (cnn.State == ConnectionState.Closed)
                            cnn.Open();



                        cmd.CommandText = string.Concat(tipoDeRechazo);
                        SqlDataReader dr = cmd.ExecuteReader();

                        while (dr.Read())
                        {
                            myReturn += dr["Rechazo"].ToString().Trim() + "|";
                            EndosoImage = (byte[])dr["EndosoImage"];
                        }
                        dr.Close();
                        dr = null;
                        rechazoNum = myReturn;
                        string[] data = myReturn.Trim().Split('|');
                        myReturn = string.Empty;

                        foreach (string s in data)
                        {
                            if (s.Trim().Length > 0)
                            {
                                string mySqlstr = "SELECT [Desc]  FROM [Criterios] where Campo = " + s;
                                cmd.CommandText = mySqlstr;
                                myReturn += s + "-" + cmd.ExecuteScalar() + "\r\n";
                            }
                        }





                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString() + "\r\nMyTipoDeRechazo Error");
            }
            return myReturn;

        }

        public bool MyInitLot(string lotNum, Logclass logClass)
        {
            /*
            'STATUS LOTE para la tabla de TF
                '0 - SIN IMPORTAR
                '1 - SIENDO IMPORTADO
                '2 - IMPORTADO
        */
            bool myBoolReturn = false;

            try
            {

                string[] myDeleteLots =
                {
                    "delete from [dbo].[lots] ",
                    "WHERE lot=@lot;"
               };

                string[] myDeleteLotsEndo =
                {
                    "Delete from LotsEndo ",
                    "Where lot=@lot"
                };

                string[] myDeleteLotsVoid =
                {
                    "Delete from LotsVoid ",
                    "Where lot=@lot"
                };

                string[] myUpDateLotsTF_Partidos =
                {
                    "update [dbo].[TF-Partidos] ",
                    "Set Imported=0 ",
                    "WHERE BatchTrack=@lot;"
                };

                using (SqlConnection cnn = new SqlConnection()
                {
                    ConnectionString = DBCnnStr
                })
                {
                    using (SqlCommand cmd = new SqlCommand()
                    {
                        Connection = cnn,
                        CommandType = CommandType.Text,

                    })
                    {
                        if (cnn.State == ConnectionState.Closed)
                            cnn.Open();

                        cmd.Parameters.Add(new SqlParameter("@lot", SqlDbType.VarChar));
                        cmd.Parameters["@lot"].Value = lotNum;

                        cmd.CommandText = string.Concat(myDeleteLots);
                        cmd.ExecuteNonQuery();

                        cmd.CommandText = string.Concat(myDeleteLotsEndo);
                        cmd.ExecuteNonQuery();

                        cmd.CommandText = string.Concat(myDeleteLotsVoid);
                        cmd.ExecuteNonQuery();

                        cmd.CommandText = string.Concat(myUpDateLotsTF_Partidos);
                        cmd.ExecuteNonQuery();

                        logClass.MYEventLog.WriteEntry(string.Concat(myDeleteLots) + "\r\n" +
                                                       string.Concat(myDeleteLotsEndo) + "\r\n" +
                                                       string.Concat(myDeleteLotsVoid) + "\r\n" +
                                                       string.Concat(myUpDateLotsTF_Partidos), System.Diagnostics.EventLogEntryType.Information, 100);

                    }
                }

                myBoolReturn = true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
            return myBoolReturn;
        }
        public bool MyReverseLots(string lotNum, string SysUser)
        {
            bool myBoolReturn = false;
            SqlTransaction transaction = null;

            string[] sqlstr = { "Update Lots Set Status = 0,[RejectedEndorsements]=0,[ValidatedEndorsements]=0, Revdate = getdate(), Revuser = @SysUser",
                                  " Where Lot = @lot "};

            string sqlstrDeleteLotEndo = "Delete from LotsEndo Where lot=@lot";

            string sqlstrDeleteLotVoid = "Delete from LotsVoid Where lot=@lot";

            try
            {
                using (SqlConnection cnn = new SqlConnection()
                {
                    ConnectionString = DBCnnStr
                })
                {
                    using (SqlCommand cmd = new SqlCommand()
                    {
                        Connection = cnn,
                        CommandType = CommandType.Text,

                    })
                    {
                        if (cnn.State == ConnectionState.Closed)
                            cnn.Open();

                        cmd.Parameters.Add(new SqlParameter("@lot", SqlDbType.VarChar));
                        cmd.Parameters.Add(new SqlParameter("@SysUser", SqlDbType.VarChar));
                        cmd.Parameters["@lot"].Value = lotNum;
                        cmd.Parameters["@SysUser"].Value = SysUser;

                        // Start a local transaction.
                        transaction = cnn.BeginTransaction(IsolationLevel.ReadCommitted);
                        cmd.Transaction = transaction;

                        cmd.CommandText = string.Concat(sqlstrDeleteLotEndo);
                        cmd.ExecuteNonQuery();

                        cmd.CommandText = string.Concat(sqlstrDeleteLotVoid);
                        cmd.ExecuteNonQuery();

                        cmd.CommandText = string.Concat(sqlstr);
                        cmd.ExecuteNonQuery();

                        transaction.Commit();
                    }

                }
                myBoolReturn = true;

            }
            catch (Exception ex)
            {
                if (transaction != null)
                {
                    try
                    {
                        transaction.Rollback();
                    }
                    catch
                    {
                        if (transaction.Connection != null)
                        {
                            Console.WriteLine("An exception of type " + ex.GetType() +
                                " was encountered while attempting to roll back the transaction.");
                        }
                    }
                }
                throw new Exception(ex.ToString() + "\r\nMyReverseLots Error\r\nAn exception of type" + ex.GetType());

            }
            return myBoolReturn;
        }
        public bool MyDeletePartidos(string where)
        {
            bool myBoolReturn = false;

            try
            {
                string[] myDelete =
                      {
                            "Delete from [dbo].[Partidos] ",
                            " WHERE Partido=@Where "
                        };

                using (SqlConnection cnn = new SqlConnection()
                {
                    ConnectionString = DBCnnStr
                })
                {
                    using (SqlCommand cmd = new SqlCommand()
                    {
                        Connection = cnn,
                        CommandType = CommandType.Text,
                        CommandText = string.Concat(myDelete)
                    })
                    {
                        if (cnn.State == ConnectionState.Closed)
                            cnn.Open();

                        cmd.Parameters.Add(new SqlParameter("@Where", SqlDbType.VarChar));
                        cmd.Parameters["@Where"].Value = where;

                        int myReturn = cmd.ExecuteNonQuery();

                        if (myReturn <= 0)
                            myBoolReturn = false;
                        else
                            myBoolReturn = true;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
            return myBoolReturn;

        }
        public bool MyDeleteUsers(Guid UserID)
        {
            bool myBoolReturn = false;

            try
            {
                string[] myDelete =
                      {
                            "Delete from [dbo].[tblUsers] ",
                            " WHERE [UserId]=@Where "
                        };

                using (SqlConnection cnn = new SqlConnection()
                {
                    ConnectionString = DBCnnStr
                })
                {
                    using (SqlCommand cmd = new SqlCommand()
                    {
                        Connection = cnn,
                        CommandType = CommandType.Text,
                        CommandText = string.Concat(myDelete)
                    })
                    {
                        if (cnn.State == ConnectionState.Closed)
                            cnn.Open();

                        cmd.Parameters.Add(new SqlParameter("@Where", SqlDbType.UniqueIdentifier));
                        cmd.Parameters["@Where"].Value = UserID;

                        int myReturn = cmd.ExecuteNonQuery();

                        if (myReturn <= 0)
                            myBoolReturn = false;
                        else
                            myBoolReturn = true;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
            return myBoolReturn;
        }
        public bool MyDeleteCandidato(string where)
        {
            bool myBoolReturn = false;

            try
            {
                string[] myDelete =
                      {
                            "Delete from [dbo].[Candidatos] ",
                            " WHERE NumCand=@Where "
                        };

                using (SqlConnection cnn = new SqlConnection()
                {
                    ConnectionString = DBCnnStr
                })
                {
                    using (SqlCommand cmd = new SqlCommand()
                    {
                        Connection = cnn,
                        CommandType = CommandType.Text,
                        CommandText = string.Concat(myDelete)
                    })
                    {
                        if (cnn.State == ConnectionState.Closed)
                            cnn.Open();

                        cmd.Parameters.Add(new SqlParameter("@Where", SqlDbType.VarChar));
                        cmd.Parameters["@Where"].Value = where;

                        int myReturn = cmd.ExecuteNonQuery();

                        if (myReturn <= 0)
                            myBoolReturn = false;
                        else
                            myBoolReturn = true;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
            return myBoolReturn;

        }
        public bool MyDeleteArea(string where)
        {
            bool myBoolReturn = false;
            string[] myDelete =
                       {
                            "Delete from [dbo].[Areas] ",
                            " WHERE Area=@Where "
                        };
            try
            {
                using (SqlConnection cnn = new SqlConnection()
                {
                    ConnectionString = DBCnnStr
                })
                {
                    using (SqlCommand cmd = new SqlCommand()
                    {
                        Connection = cnn,
                        CommandType = CommandType.Text,
                        CommandText = string.Concat(myDelete)
                    })
                    {
                        if (cnn.State == ConnectionState.Closed)
                            cnn.Open();

                        cmd.Parameters.Add(new SqlParameter("@Where", SqlDbType.VarChar));
                        cmd.Parameters["@Where"].Value = where;

                        int myReturn = cmd.ExecuteNonQuery();

                        if (myReturn <= 0)
                            myBoolReturn = false;
                        else
                            myBoolReturn = true;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
            return myBoolReturn;
        }
        public bool MyDeleteNotario(string where, string where2, string NumCand,int modo)
        {
            bool myBoolReturn = false;
            string[] myDelete =
                       {
                            "Delete from [dbo].[Notarios] ",
                            " WHERE NumElec=@Where and partido=@where2 and NumCand=@NumCand"
                        };

            string[] myDeletePartidos =
           {
                            "Delete from [dbo].[NotariosPartidos] ",
                            " WHERE NumElec=@Where and partido=@where2 and NumCand=@NumCand"
                        };

            try
            {
                using (SqlConnection cnn = new SqlConnection()
                {
                    ConnectionString = DBCnnStr
                })
                {
                    using (SqlCommand cmd = new SqlCommand()
                    {
                        Connection = cnn,
                        CommandType = CommandType.Text,
                        CommandText = string.Concat(myDelete)
                    })
                    {
                        if (modo == 2)
                        cmd.CommandText = string.Concat(myDeletePartidos);

                        if (cnn.State == ConnectionState.Closed)
                            cnn.Open();

                        cmd.Parameters.Add(new SqlParameter("@Where", SqlDbType.VarChar));
                        cmd.Parameters.Add(new SqlParameter("@Where2", SqlDbType.VarChar));
                        cmd.Parameters.Add(new SqlParameter("@NumCand", SqlDbType.VarChar));
                        cmd.Parameters["@Where"].Value = where;
                        cmd.Parameters["@Where2"].Value = where2;
                        cmd.Parameters["@NumCand"].Value = NumCand.Trim();


                        int myReturn = cmd.ExecuteNonQuery();

                        if (myReturn <= 0)
                            myBoolReturn = false;
                        else
                            myBoolReturn = true;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
            return myBoolReturn;
        }


        public string[] MyGetTotalEndosReqPartido(string partido)
        {
            string[] sOut = { "", "", "", "", "", "" };

            string mySqlstr = "Select * from [dbo].[Candidatos]  where [partido]='" + partido + "';";

            try
            {
                using (SqlConnection cnn = new SqlConnection()
                {
                    ConnectionString = DBRadicacionesCEECnnStr
                })
                {
                    if (cnn.State == ConnectionState.Closed)
                        cnn.Open();

                    using (SqlCommand cmd = new SqlCommand()
                    {
                        Connection = cnn,
                        CommandType = CommandType.Text,
                        CommandText = mySqlstr

                    })
                    {
                        SqlDataReader dr;
                        dr = cmd.ExecuteReader();

                        while (dr.Read())
                        {
                            sOut[0] = dr[0].ToString().Trim();
                            sOut[1] = dr[1].ToString().Trim();
                            sOut[2] = dr[2].ToString().Trim();
                            sOut[3] = dr[3].ToString().Trim();
                            sOut[4] = dr[4].ToString().Trim();
                            sOut[5] = dr[5].ToString().Trim();
                        }
                        dr.Close();
                        dr = null;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString() + "\r\nMyGetTotalEndosReqPartido Error");
            }
            return sOut;
        }

        public string[] MyGetTotalReq(string numElec)
        {
            string[] sOut = { "", "", "", "", "" };

            int tmpElec = 0;

            int.TryParse(numElec, out tmpElec);

            string mySqlstr = "Select top 1 [RequiredEndorsements],[FirstName] , [MiddleName],[LastName] ,[MaternalName] from [dbo].[FilingEndorsements] with (nolock) where [ElectoralNumber]=" + tmpElec.ToString();

            try
            {
                using (SqlConnection cnn = new SqlConnection()
                {
                    ConnectionString = DBRadicacionesCEECnnStr
                })
                {
                    if (cnn.State == ConnectionState.Closed)
                        cnn.Open();

                    using (SqlCommand cmd = new SqlCommand()
                    {
                        Connection = cnn,
                        CommandType = CommandType.Text,
                        CommandText = mySqlstr

                    })
                    {
                        SqlDataReader dr;

                        dr = cmd.ExecuteReader();

                        while (dr.Read())
                        {
                            sOut[0] = dr[0].ToString().Trim();
                            sOut[1] = dr[1].ToString().Trim();
                            sOut[2] = dr[2].ToString().Trim();
                            sOut[3] = dr[3].ToString().Trim();
                            sOut[4] = dr[4].ToString().Trim();
                        }
                        dr.Close();
                        dr = null;
                    }
                }
            }
            catch (Exception ex)
            {

                throw new Exception(ex.ToString() + "\r\nMyGetTotalReq Error");

            }
            return sOut;
        }


        public DataTable MyGetBodyCertificacion()
        {
            DataTable myTableReturn = new DataTable();
            try
            {
                string mySqlstr = "Select * from tblSetBodyCertificacion";

                using (SqlConnection cnn = new SqlConnection()
                {
                    ConnectionString = DBCnnStr
                })
                {
                    using (SqlCommand cmd = new SqlCommand()
                    {
                        Connection = cnn,
                        CommandType = CommandType.Text,
                        CommandText = mySqlstr
                    })
                    {
                        if (cnn.State == ConnectionState.Closed)
                            cnn.Open();

                        using (SqlDataAdapter da = new SqlDataAdapter()
                        {
                            SelectCommand = cmd
                        })
                        {
                            da.Fill(myTableReturn);
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString() + "\r\nMyGetCandidatos Error");
            }
            return myTableReturn;
        }

        public bool MyUpdateUser(Guid UserId, String PasswordHash, String SecurityStamp)
        {
            bool myBoolReturn = false;
            try
            {
                string[] myUpdate =
                    {
                    "update [dbo].[tblUsers] ",
                    "set [SecurityStamp] = @SecurityStamp, ",
                    "[PasswordHash] = @PasswordHash ",
                    "where UserId=@UserId"
                };


                using (SqlConnection cnn = new SqlConnection()
                {
                    ConnectionString = DBCnnStr
                })
                {
                    if (cnn.State == ConnectionState.Closed)
                        cnn.Open();

                    using (SqlCommand cmd = new SqlCommand()
                    {
                        Connection = cnn,
                        CommandType = CommandType.Text,

                    })
                    {
                        cmd.CommandText = string.Concat(myUpdate);
                        cmd.Parameters.Add(new SqlParameter("@UserId", SqlDbType.UniqueIdentifier)).Value = UserId;
                        cmd.Parameters.Add(new SqlParameter("@SecurityStamp", SqlDbType.VarChar)).Value = SecurityStamp.Trim();
                        cmd.Parameters.Add(new SqlParameter("@PasswordHash", SqlDbType.VarChar)).Value = PasswordHash.Trim();

                        int myReturn = cmd.ExecuteNonQuery();

                        if (myReturn <= 0)
                            myBoolReturn = false;
                        else
                            myBoolReturn = true;

                    }
                }



            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString() + "\r\n" + "MyUpdateUser");
            }


            return myBoolReturn;
        }


        public bool MyUpdateUser(Guid UserId, String AreasDeAcceso)
        {
            bool myBoolReturn = false;
            try
            {
                string[] myUpdate =
                    {
                    "update [dbo].[tblUsers] ",
                    "set [AreasDeAcceso] = @AreasDeAcceso ",
                    "where UserId=@UserId"
                };


                using (SqlConnection cnn = new SqlConnection()
                {
                    ConnectionString = DBCnnStr
                })
                {
                    if (cnn.State == ConnectionState.Closed)
                        cnn.Open();

                    using (SqlCommand cmd = new SqlCommand()
                    {
                        Connection = cnn,
                        CommandType = CommandType.Text,

                    })
                    {
                        cmd.CommandText = string.Concat(myUpdate);
                        cmd.Parameters.Add(new SqlParameter("@UserId", SqlDbType.UniqueIdentifier)).Value = UserId;
                        cmd.Parameters.Add(new SqlParameter("@AreasDeAcceso", SqlDbType.VarChar)).Value = AreasDeAcceso.Trim();

                        int myReturn = cmd.ExecuteNonQuery();

                        if (myReturn <= 0)
                            myBoolReturn = false;
                        else
                            myBoolReturn = true;

                    }
                }



            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString() + "\r\n" + "MyUpdateUser");
            }


            return myBoolReturn;
        }


        public bool MyInsertUsers(Guid UserId, String UserName, String PasswordHash, String SecurityStamp, String Email, String AreasDeAcceso)
        {
            bool myBoolReturn = false;
            try
            {
                string[] myInsert =
                     {
                    "insert into  [dbo].[tblUsers]([UserId],[UserName],[PasswordHash],[SecurityStamp],[Email],[AreasDeAcceso]) ",
                    "Values(@UserId,@UserName,@PasswordHash,@SecurityStamp,@Email,@AreasDeAcceso)"};


                using (SqlConnection cnn = new SqlConnection()
                {
                    ConnectionString = DBCnnStr
                })
                {
                    if (cnn.State == ConnectionState.Closed)
                        cnn.Open();

                    using (SqlCommand cmd = new SqlCommand()
                    {
                        Connection = cnn,
                        CommandType = CommandType.Text,

                    })
                    {
                        cmd.CommandText = string.Concat(myInsert);
                        cmd.Parameters.Add(new SqlParameter("@UserId", SqlDbType.UniqueIdentifier)).Value = UserId;
                        cmd.Parameters.Add(new SqlParameter("@UserName", SqlDbType.VarChar)).Value = UserName.Trim();
                        cmd.Parameters.Add(new SqlParameter("@PasswordHash", SqlDbType.VarChar)).Value = PasswordHash.Trim();
                        cmd.Parameters.Add(new SqlParameter("@SecurityStamp", SqlDbType.VarChar)).Value = SecurityStamp.Trim();
                        cmd.Parameters.Add(new SqlParameter("@Email", SqlDbType.VarChar)).Value = Email.Trim();
                        cmd.Parameters.Add(new SqlParameter("@AreasDeAcceso", SqlDbType.VarChar)).Value = AreasDeAcceso.Trim();

                        int myReturn = cmd.ExecuteNonQuery();

                        if (myReturn <= 0)
                            myBoolReturn = false;
                        else
                            myBoolReturn = true;

                    }
                }



            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString() + "\r\n" + "MyInsertUsers");
            }
            return myBoolReturn;
        }

        public bool MyInsertBodyCertificacion(string HupDerecho, string Fecha, string InfoSecretario, string InfoComisionado,
            string P1Body, string P2Body, string InfoDirectorValidaciones, string DireccionPostal, string Telefono, string Logo,
            string Color, string PageHeader, byte Template, string Firma)
        {
            bool myBoolReturn = false;
            try
            {
                string[] myInsert =
                      {
                            "INSERT INTO [dbo].[tblSetBodyCertificacion]",
                           "([HupDerecho]",
                           ",[Fecha]",
                           ",[InfoSecretario]",
                           ",[InfoComisionado]",
                           ",[P1Body]",
                           ",[P2Body]",
                           ",[InfoDirectorValidaciones]",
                           ",[DireccionPostal]",
                           ",[Telefono]",
                           ",[Logo],[LogoPath],[Color],[PageHeader],Template,Firma,FirmaPath) ",
                            "Values(@HupDerecho,",
                                   "@Fecha,",
                                   "@InfoSecretario,",
                                   "@InfoComisionado, ",
                                   "@P1Body, ",
                                   "@P2Body, ",
                                   "@InfoDirectorValidaciones,",
                                   "@DireccionPostal, ",
                                   "@Telefono, ",
                                   "@Logo,@LogoPath,@Color,@PageHeader,@Template,@Firma,@FirmaPath) "
                        };


                using (SqlConnection cnn = new SqlConnection()
                {
                    ConnectionString = DBCnnStr
                })
                {
                    if (cnn.State == ConnectionState.Closed)
                        cnn.Open();

                    using (SqlCommand cmd = new SqlCommand()
                    {
                        Connection = cnn,
                        CommandType = CommandType.Text,

                    })
                    {
                        cmd.CommandText = "Delete from [dbo].[tblSetBodyCertificacion]";
                        cmd.ExecuteNonQuery();
                        cmd.CommandText = string.Concat(myInsert);

                        cmd.Parameters.Add(new SqlParameter("@HupDerecho", SqlDbType.VarChar)).Value = HupDerecho.Trim();
                        cmd.Parameters.Add(new SqlParameter("@Fecha", SqlDbType.VarChar)).Value = Fecha.Trim();
                        cmd.Parameters.Add(new SqlParameter("@InfoSecretario", SqlDbType.VarChar)).Value = InfoSecretario.Trim();
                        cmd.Parameters.Add(new SqlParameter("@InfoComisionado", SqlDbType.VarChar)).Value = InfoComisionado.Trim();
                        cmd.Parameters.Add(new SqlParameter("@P1Body", SqlDbType.VarChar)).Value = P1Body;
                        cmd.Parameters.Add(new SqlParameter("@P2Body", SqlDbType.VarChar)).Value = P2Body;
                        cmd.Parameters.Add(new SqlParameter("@InfoDirectorValidaciones", SqlDbType.VarChar)).Value = InfoDirectorValidaciones;
                        cmd.Parameters.Add(new SqlParameter("@DireccionPostal", SqlDbType.VarChar)).Value = DireccionPostal.Trim();
                        cmd.Parameters.Add(new SqlParameter("@Telefono", SqlDbType.VarChar)).Value = Telefono.Trim();
                        cmd.Parameters.Add(new SqlParameter("@Logo", SqlDbType.Image));
                        cmd.Parameters.Add(new SqlParameter("@LogoPath", SqlDbType.VarChar)).Value = Logo.Trim();
                        cmd.Parameters.Add(new SqlParameter("@Color", SqlDbType.VarChar)).Value = Color.Trim();
                        cmd.Parameters.Add(new SqlParameter("@PageHeader", SqlDbType.VarChar)).Value = PageHeader.Trim();
                        cmd.Parameters.Add(new SqlParameter("@Template", SqlDbType.VarChar)).Value = Template;

                        cmd.Parameters.Add(new SqlParameter("@FirmaPath", SqlDbType.VarChar)).Value = Firma.Trim();
                        cmd.Parameters.Add(new SqlParameter("@Firma", SqlDbType.Image));

                        //Logo
                        FileStream stream = new FileStream(Logo, FileMode.Open, FileAccess.Read);
                        BinaryReader reader = new BinaryReader(stream);
                        byte[] img = reader.ReadBytes((int)stream.Length);
                        reader.Close();
                        stream.Close();
                        cmd.Parameters["@Logo"].Value = img;

                        //Firma
                        stream = new FileStream(Firma, FileMode.Open, FileAccess.Read);
                        reader = new BinaryReader(stream);
                        img = reader.ReadBytes((int)stream.Length);
                        reader.Close();
                        stream.Close();
                        cmd.Parameters["@Firma"].Value = img;


                        int myReturn = cmd.ExecuteNonQuery();

                        if (myReturn <= 0)
                            myBoolReturn = false;
                        else
                            myBoolReturn = true;

                    }
                }

            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString() + "\r\n MyInsertBodyCertificacion Error");

            }
            return myBoolReturn;
        }

        public bool MyChangeArea(bool isInsert, string area, string desc, string precintos, string electivePositionID, string demarcationID, string where)
        {
            bool myBoolReturn = false;
            try
            {
                string[] myInsert =
                        {
                            "INSERT INTO [dbo].[Areas]([Area],[Desc],[Precintos],[ElectivePositionID],[DemarcationID]) ",
                            "VALUES(@Area,@Desc,@Precintos,@ElectivePositionID,@DemarcationID)"
                        };

                string[] myUpdate =
                        {
                            "UPDATE [dbo].[Areas]",
                            " SET [Area] =@Area",
                            ",[Desc] = @Desc",
                            ",[Precintos] = @Precintos",
                            ",[ElectivePositionID] = @ElectivePositionID",
                            ",[DemarcationID] = @DemarcationID",
                            " WHERE Area=@Where"
                        };

                using (SqlConnection cnn = new SqlConnection()
                {
                    ConnectionString = DBCnnStr
                })
                {
                    using (SqlCommand cmd = new SqlCommand()
                    {
                        Connection = cnn,
                        CommandType = CommandType.Text,
                        CommandText = isInsert == true ? string.Concat(myInsert) : string.Concat(myUpdate)
                    })
                    {
                        if (cnn.State == ConnectionState.Closed)
                            cnn.Open();

                        cmd.Parameters.Add(new SqlParameter("@Area", SqlDbType.VarChar));
                        cmd.Parameters.Add(new SqlParameter("@Desc", SqlDbType.VarChar));
                        cmd.Parameters.Add(new SqlParameter("@Precintos", SqlDbType.VarChar));
                        cmd.Parameters.Add(new SqlParameter("@ElectivePositionID", SqlDbType.Int));
                        cmd.Parameters.Add(new SqlParameter("@DemarcationID", SqlDbType.Int));
                        if (!isInsert)
                            cmd.Parameters.Add(new SqlParameter("@Where", SqlDbType.VarChar));

                        cmd.Parameters["@Area"].Value = area;
                        cmd.Parameters["@Desc"].Value = desc;
                        cmd.Parameters["@Precintos"].Value = precintos;
                        cmd.Parameters["@ElectivePositionID"].Value = electivePositionID;
                        cmd.Parameters["@DemarcationID"].Value = demarcationID;

                        if (!isInsert)
                            cmd.Parameters["@Where"].Value = where;

                        int myReturn = cmd.ExecuteNonQuery();

                        if (myReturn <= 0)
                            myBoolReturn = false;
                        else
                            myBoolReturn = true;

                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString() + "\r\nMyInsertArea Error");
            }
            return myBoolReturn;
        }
        public bool MyChangePartidos(bool isInsert, string partido, string desc, string endoReq, string area, int Modo, string where)
        {
            bool myBoolReturn = false;
            try
            {
                string[] myInsert =
                        {
                            "INSERT INTO [dbo].[Partidos] ([Partido],[Desc],[EndoReq],[Area],[Modo]) ",
                            "VALUES (@Partido,@Desc,@EndoReq,@Area,@Modo)"
                        };

                string[] myUpdate =
                    {
                            "UPDATE [dbo].[Partidos] ",
                            "SET [Partido] = @Partido, ",
                            "[Desc] = @Desc,",
                            "[EndoReq] = @EndoReq,",
                            "[Area] = @Area,",
                            "[Modo] = @Modo ",
                            "WHERE Partido=@Where"
                };

                using (SqlConnection cnn = new SqlConnection()
                {
                    ConnectionString = DBCnnStr
                })
                {
                    using (SqlCommand cmd = new SqlCommand()
                    {
                        Connection = cnn,
                        CommandType = CommandType.Text,
                        CommandText = isInsert == true ? string.Concat(myInsert) : string.Concat(myUpdate)
                    })
                    {
                        if (cnn.State == ConnectionState.Closed)
                            cnn.Open();

                        cmd.Parameters.Add(new SqlParameter("@Partido", SqlDbType.VarChar));
                        cmd.Parameters.Add(new SqlParameter("@Desc", SqlDbType.VarChar));
                        cmd.Parameters.Add(new SqlParameter("@EndoReq", SqlDbType.Int));
                        cmd.Parameters.Add(new SqlParameter("@Area", SqlDbType.VarChar));
                        cmd.Parameters.Add(new SqlParameter("@Modo", SqlDbType.Int));

                        if (!isInsert)
                            cmd.Parameters.Add(new SqlParameter("@Where", SqlDbType.VarChar));

                        cmd.Parameters["@Partido"].Value = partido;
                        cmd.Parameters["@Desc"].Value = desc;
                        cmd.Parameters["@EndoReq"].Value = endoReq;
                        cmd.Parameters["@Area"].Value = area.Substring(0, 4);
                        cmd.Parameters["@Modo"].Value = Modo;

                        if (!isInsert)
                            cmd.Parameters["@Where"].Value = where;

                        int myReturn = cmd.ExecuteNonQuery();

                        if (myReturn <= 0)
                            myBoolReturn = false;
                        else
                            myBoolReturn = true;

                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString() + "\r\nMyInsertArea Error");
            }
            return myBoolReturn;
        }
        public bool MyChangeNotario(bool isInsert, string NumElec, String Partido, string NumCand, string Nombre, string Apellido1, string Apellido2, string Status, DateTime Fecha, int modo, string where)
        {
            bool myBoolReturn = false;
            try
            {
                String Fecha_Dia = ((DateTime)Fecha).Day.ToString();
                String Fecha_Mes = ((DateTime)Fecha).Month.ToString();
                String Fecha_Ano = ((DateTime)Fecha).Year.ToString();

                string[] myInsert =
                        {
                            "INSERT INTO [dbo].[Notarios] ([NumElec],[Partido],[NumCand],[Nombre],[Apellido1],[Apellido2],[Status],[Fecha_Dia],[Fecha_Mes],[Fecha_Ano],[Modo]) ",
                            "VALUES (@NumElec,@Partido,@NumCand,@Nombre,@Apellido1,@Apellido2,@Status,@Fecha_Dia,@Fecha_Mes,@Fecha_Ano,@Modo)"
                        };

                string[] myInsertPartidos =
                        {
                            "INSERT INTO [dbo].[NotariosPartidos] ([NumElec],[Partido],[NumCand],[Nombre],[Apellido1],[Apellido2],[Status],[Fecha_Dia],[Fecha_Mes],[Fecha_Ano],[Modo]) ",
                            "VALUES (@NumElec,@Partido,@NumCand,@Nombre,@Apellido1,@Apellido2,@Status,@Fecha_Dia,@Fecha_Mes,@Fecha_Ano,@Modo)"
                        };


                string[] myUpdate =
                    {
                            "UPDATE [dbo].[Notarios] ",
                            "SET [NumElec] = @NumElec,",
                            "[Partido] = @Partido,",
                            "[NumCand] = @NumCand,",
                            "[Nombre]  = @Nombre,",
                            "[Apellido1] = @Apellido1,",
                            "[Apellido2] = @Apellido2,",
                            "[Status]    = @Status,",
                            "[Fecha_Dia] = @Fecha_Dia,",
                            "[Fecha_Mes] = @Fecha_Mes,",
                            "[Fecha_Ano] = @Fecha_Ano,",
                            "[Modo] = @Modo ",
                            "WHERE NumElec=@where"
                };

                string[] myUpdatePartidos =
                    {
                            "UPDATE [dbo].[NotariosPartidos] ",
                            "SET [NumElec] = @NumElec,",
                            "[Partido] = @Partido,",
                            "[NumCand] = @NumCand,",
                            "[Nombre]  = @Nombre,",
                            "[Apellido1] = @Apellido1,",
                            "[Apellido2] = @Apellido2,",
                            "[Status]    = @Status,",
                            "[Fecha_Dia] = @Fecha_Dia,",
                            "[Fecha_Mes] = @Fecha_Mes,",
                            "[Fecha_Ano] = @Fecha_Ano,",
                            "[Modo] = @Modo ",
                            "WHERE NumElec=@where"
                };

                using (SqlConnection cnn = new SqlConnection()
                {
                    ConnectionString = DBCnnStr
                })
                {
                    using (SqlCommand cmd = new SqlCommand()
                    {
                        Connection = cnn,
                        CommandType = CommandType.Text,

                    })
                    {

                        cmd.CommandText = isInsert == true ? string.Concat(myInsert) : string.Concat(myUpdate);

                        if (modo == 2)
                            cmd.CommandText = isInsert == true ? string.Concat(myInsertPartidos) : string.Concat(myUpdatePartidos);

                        if (cnn.State == ConnectionState.Closed)
                            cnn.Open();

                        cmd.Parameters.Add(new SqlParameter("@NumElec", SqlDbType.Int));
                        cmd.Parameters.Add(new SqlParameter("@Partido", SqlDbType.VarChar));
                        cmd.Parameters.Add(new SqlParameter("@NumCand", SqlDbType.VarChar));
                        cmd.Parameters.Add(new SqlParameter("@Nombre", SqlDbType.VarChar));
                        cmd.Parameters.Add(new SqlParameter("@Apellido1", SqlDbType.VarChar));
                        cmd.Parameters.Add(new SqlParameter("@Apellido2", SqlDbType.VarChar));
                        cmd.Parameters.Add(new SqlParameter("@Status", SqlDbType.VarChar));//[Status]
                        cmd.Parameters.Add(new SqlParameter("@Fecha_Dia", SqlDbType.Int));
                        cmd.Parameters.Add(new SqlParameter("@Fecha_Mes", SqlDbType.Int));
                        cmd.Parameters.Add(new SqlParameter("@Fecha_Ano", SqlDbType.Int));
                        cmd.Parameters.Add(new SqlParameter("@Modo", SqlDbType.Int));

                        if (!isInsert)
                            cmd.Parameters.Add(new SqlParameter("@Where", SqlDbType.VarChar));

                        cmd.Parameters["@NumElec"].Value = NumElec.Trim();
                        cmd.Parameters["@Partido"].Value = Partido.Trim();
                        cmd.Parameters["@NumCand"].Value = NumCand.Trim();
                        cmd.Parameters["@Nombre"].Value = Nombre.Trim();
                        cmd.Parameters["@Apellido1"].Value = Apellido1.Trim();
                        cmd.Parameters["@Apellido2"].Value = Apellido2.Trim();
                        cmd.Parameters["@Status"].Value = Status.Trim();
                        cmd.Parameters["@Fecha_Dia"].Value = Fecha_Dia.ToString();
                        cmd.Parameters["@Fecha_Mes"].Value = Fecha_Mes.ToString();
                        cmd.Parameters["@Fecha_Ano"].Value = Fecha_Ano.ToString();
                        cmd.Parameters["@Modo"].Value = modo;

                        if (!isInsert)
                            cmd.Parameters["@Where"].Value = where;

                        int myReturn = cmd.ExecuteNonQuery();

                        if (myReturn <= 0)
                            myBoolReturn = false;
                        else
                            myBoolReturn = true;

                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString() + "\r\n Notario Error");
            }
            return myBoolReturn;
        }
        public bool MyChangeCriterios(string Campo, bool? Editar, string Desc, bool? Warning, int modo)
        {
            bool myBoolReturn = false;
            try
            {

                string[] myUpdate =
                    {
                            "UPDATE [dbo].[Criterios] ",
                            "SET [Editar] = @Editar,",
                            "[Desc] = @Desc,",
                            "[Warning] = @Warning ",
                            "WHERE Campo=@Campo"
                    };


                string[] myUpdatePartido =
                    {
                    "UPDATE [dbo].[CriteriosPartido] ",
                            "SET [Editar] = @Editar,",
                            "[Desc] = @Desc,",
                            "[Warning] = @Warning ",
                            "WHERE Campo=@Campo"
                    };

                using (SqlConnection cnn = new SqlConnection()
                {
                    ConnectionString = DBCnnStr
                })
                {
                    using (SqlCommand cmd = new SqlCommand()
                    {
                        Connection = cnn,
                        CommandType = CommandType.Text,
                        CommandText = string.Concat(myUpdate)
                    })
                    {
                        if (cnn.State == ConnectionState.Closed)
                            cnn.Open();

                        if (modo == 2)
                            cmd.CommandText = string.Concat(myUpdatePartido);

                        cmd.Parameters.Add(new SqlParameter("@Campo", SqlDbType.Int));
                        cmd.Parameters.Add(new SqlParameter("@Editar", SqlDbType.VarChar));
                        cmd.Parameters.Add(new SqlParameter("@Desc", SqlDbType.VarChar));
                        cmd.Parameters.Add(new SqlParameter("@Warning", SqlDbType.VarChar));

                        cmd.Parameters["@Campo"].Value = Campo;
                        cmd.Parameters["@Editar"].Value = Editar == true ? "1" : "0";
                        cmd.Parameters["@Desc"].Value = Desc;
                        cmd.Parameters["@Warning"].Value = Warning == true ? "1" : "0";

                        int myReturn = cmd.ExecuteNonQuery();

                        if (myReturn <= 0)
                            myBoolReturn = false;
                        else
                            myBoolReturn = true;

                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString() + "\r\nMyInsertArea Error");
            }
            return myBoolReturn;
        }
        public bool MyChangeTF(DataTable tableLots, string usercode, object fechaRecibo,int modo)
        {
            bool myBoolReturn = false;
            SqlTransaction transaction = null;
            bool myBoolErrorNoHayLotes = true;
            try
            {

                if (tableLots == null)
                    throw new Exception("No Hay Lotes Para Importar");

                if (tableLots.Rows.Count <= 0)
                    throw new Exception("No Hay Lotes Para Importar");

                myBoolErrorNoHayLotes = false;

                string[] myInsertLot =
                        {
                            "Insert Into lots  ([Partido],[Lot],[Amount],[Usercode],[AuthDate],[Status],[VerDate],[VerUser],[FinUser] ,[FinDate],[RevDate],[RevUser],[conditions],[ImportDate],[Num_Candidato],[modo])  ",
                            "VALUES (",
                             "@Partido,@Lot,@Amount,@Usercode,@AuthDate,@Status,null,null,null,null,null,null,null,@ImportDate,@Num_Candidato,@modo",
                            ");"
                        };

                string FechaRecibo_Mes = string.Empty;
                string FechaRecibo_Dia = string.Empty;
                string FechaRecibo_Ano = string.Empty;

                try
                {
                    FechaRecibo_Mes = ((DateTime)fechaRecibo).Month.ToString();
                    FechaRecibo_Dia = ((DateTime)fechaRecibo).Day.ToString();
                    FechaRecibo_Ano = ((DateTime)fechaRecibo).Year.ToString();
                }
                catch
                {
                    FechaRecibo_Mes = string.Empty;
                    FechaRecibo_Dia = string.Empty;
                    FechaRecibo_Ano = string.Empty;
                }

                string[] myUpdate_Imported1 =
                    {
                            "Update [TF-Partidos] ",
                            "SET [Imported] = 1, ",
                            "FirmaNot_Inv = 0,",
                            "FirmaElec_Inv = 0,",
                            "FechaRecibo_Mes = @FechaRecibo_Mes,",
                            "FechaRecibo_Dia = @FechaRecibo_Dia,",
                            "FechaRecibo_Ano = @FechaRecibo_Ano,",
                            "Leer = '',",
                            "Alteracion=0 ",
                            "WHERE BatchTrack=@where ",
                            "And Imported = 0"
                    };

                string[] myUpdate_Imported2 =
                   {
                            "Update [TF-Partidos] ",
                            "SET [Imported] = 2 ",
                            "WHERE BatchTrack=@where ",
                            "And Imported = 1"
                    };

                using (SqlConnection cnn = new SqlConnection()
                {
                    ConnectionString = DBCnnStr
                })
                {
                    using (SqlCommand cmd = new SqlCommand()
                    {
                        Connection = cnn,
                        CommandType = CommandType.Text,

                    })
                    {
                        if (cnn.State == ConnectionState.Closed)
                            cnn.Open();


                        SqlParameter whereParam = new SqlParameter();
                        whereParam.ParameterName = "@Where";
                        whereParam.SqlDbType = SqlDbType.VarChar;

                        SqlParameter partidoParam = new SqlParameter();
                        partidoParam.ParameterName = "@Partido";
                        partidoParam.SqlDbType = SqlDbType.VarChar;

                        SqlParameter lotParam = new SqlParameter();
                        lotParam.ParameterName = "@Lot";
                        lotParam.SqlDbType = SqlDbType.VarChar;

                        SqlParameter Num_CandidatoParam = new SqlParameter();
                        Num_CandidatoParam.ParameterName = "@Num_Candidato";
                        Num_CandidatoParam.SqlDbType = SqlDbType.Int;

                        SqlParameter amountParam = new SqlParameter();
                        amountParam.ParameterName = "@Amount";
                        amountParam.SqlDbType = SqlDbType.Int;

                        SqlParameter usercodeParam = new SqlParameter();
                        usercodeParam.ParameterName = "@Usercode";
                        usercodeParam.SqlDbType = SqlDbType.VarChar;

                        SqlParameter authDateParam = new SqlParameter();
                        authDateParam.ParameterName = "@AuthDate";
                        authDateParam.SqlDbType = SqlDbType.DateTime;

                        SqlParameter statusParam = new SqlParameter();
                        statusParam.ParameterName = "@Status";
                        statusParam.SqlDbType = SqlDbType.VarChar;

                        SqlParameter modoParam = new SqlParameter();
                        modoParam.ParameterName = "@modo";
                        modoParam.SqlDbType = SqlDbType.Int;

                        /*
                        SqlParameter verDateParam = new SqlParameter();
                        verDateParam.ParameterName = "@VerDate";
                        verDateParam.SqlDbType = SqlDbType.DateTime;

                        SqlParameter verUserParam = new SqlParameter();
                        verUserParam.ParameterName = "@VerUser";
                        verUserParam.SqlDbType = SqlDbType.VarChar;

                        SqlParameter finUserParam = new SqlParameter();
                        finUserParam.ParameterName = "@FinUser";
                        finUserParam.SqlDbType = SqlDbType.VarChar;

                        SqlParameter finDateParam = new SqlParameter();
                        finDateParam.ParameterName = "@FinDate";
                        finDateParam.SqlDbType = SqlDbType.DateTime;

                        SqlParameter revDateParam = new SqlParameter();
                        revDateParam.ParameterName = "@RevDate";
                        revDateParam.SqlDbType = SqlDbType.DateTime;

                        SqlParameter revUserParam = new SqlParameter();
                        revUserParam.ParameterName = "@RevUser";
                        revUserParam.SqlDbType = SqlDbType.VarChar;

                        SqlParameter conditionsParam = new SqlParameter();
                        conditionsParam.ParameterName = "@conditions";
                        conditionsParam.SqlDbType = SqlDbType.VarChar;
                        */
                        SqlParameter importDateParam = new SqlParameter();
                        importDateParam.ParameterName = "@ImportDate";
                        importDateParam.SqlDbType = SqlDbType.VarChar;

                        SqlParameter FechaRecibo_MesParam = new SqlParameter();
                        FechaRecibo_MesParam.ParameterName = "@FechaRecibo_Mes";
                        FechaRecibo_MesParam.SqlDbType = SqlDbType.VarChar;

                        SqlParameter FechaRecibo_DiaParam = new SqlParameter();
                        FechaRecibo_DiaParam.ParameterName = "@FechaRecibo_Dia";
                        FechaRecibo_DiaParam.SqlDbType = SqlDbType.VarChar;

                        SqlParameter FechaRecibo_AnoParam = new SqlParameter();
                        FechaRecibo_AnoParam.ParameterName = "@FechaRecibo_Ano";
                        FechaRecibo_AnoParam.SqlDbType = SqlDbType.VarChar;

                        //SqlParameter totalDeDiasParam = new SqlParameter();
                        //totalDeDiasParam.ParameterName = "@totalDeDias";
                        //totalDeDiasParam.SqlDbType = SqlDbType.Int;

                        cmd.Parameters.Add(whereParam);

                        foreach (DataRow row in tableLots.Rows)
                        {
                            // Start a local transaction.
                            transaction = cnn.BeginTransaction(IsolationLevel.ReadCommitted);
                            cmd.Transaction = transaction;

                            string where = row["BatchTrack"].ToString();

                            whereParam.Value = where;

                            int myReturn = 0;

                            cmd.Parameters.Add(FechaRecibo_MesParam);
                            cmd.Parameters.Add(FechaRecibo_DiaParam);
                            cmd.Parameters.Add(FechaRecibo_AnoParam);
                            //   cmd.Parameters.Add(totalDeDiasParam);

                            FechaRecibo_MesParam.Value = FechaRecibo_Mes;
                            FechaRecibo_DiaParam.Value = FechaRecibo_Dia;
                            FechaRecibo_AnoParam.Value = FechaRecibo_Ano;
                            //    totalDeDiasParam.Value = totalDeDias;

                            cmd.CommandText = string.Concat(myUpdate_Imported1);
                            myReturn = cmd.ExecuteNonQuery();

                            if (cmd.Parameters.Contains(whereParam))
                                cmd.Parameters.Remove(whereParam);

                            cmd.Parameters.Remove(FechaRecibo_MesParam);
                            cmd.Parameters.Remove(FechaRecibo_DiaParam);
                            cmd.Parameters.Remove(FechaRecibo_AnoParam);
                            //   cmd.Parameters.Remove(totalDeDiasParam);

                            cmd.Parameters.Add(partidoParam);
                            cmd.Parameters.Add(lotParam);
                            cmd.Parameters.Add(Num_CandidatoParam);
                            cmd.Parameters.Add(amountParam);
                            cmd.Parameters.Add(usercodeParam);
                            cmd.Parameters.Add(authDateParam);
                            cmd.Parameters.Add(statusParam);
                            cmd.Parameters.Add(modoParam);
                            /* cmd.Parameters.Add(verDateParam);
                             cmd.Parameters.Add(verUserParam);
                             cmd.Parameters.Add(finUserParam);
                             cmd.Parameters.Add(finDateParam);
                             cmd.Parameters.Add(revDateParam);
                             cmd.Parameters.Add(revUserParam);
                             cmd.Parameters.Add(conditionsParam);*/
                            cmd.Parameters.Add(importDateParam);

                            partidoParam.Value = row["Partido"].ToString();
                            lotParam.Value = row["BatchTrack"].ToString();
                            Num_CandidatoParam.Value = "0";
                            amountParam.Value = int.Parse(row["Amount"].ToString());
                            usercodeParam.Value = usercode;
                            authDateParam.Value = DateTime.Now.ToString(); ;
                            statusParam.Value = "0";
                            modoParam.Value = modo.ToString();
                            //verDateParam.Value = VerDate;
                            //verUserParam.Value = VerUser;
                            //finUserParam.Value = FinUser;
                            //finDateParam.Value = FinDate;
                            //revDateParam.Value = RevDate;
                            //revUserParam.Value = RevUser;
                            //conditionsParam.Value = conditions;
                            importDateParam.Value = DateTime.Now.ToString();

                            cmd.CommandText = string.Concat(myInsertLot);
                            myReturn = cmd.ExecuteNonQuery();

                            cmd.Parameters.Remove(partidoParam);
                            cmd.Parameters.Remove(lotParam);
                            cmd.Parameters.Remove(Num_CandidatoParam);
                            cmd.Parameters.Remove(amountParam);
                            cmd.Parameters.Remove(usercodeParam);
                            cmd.Parameters.Remove(authDateParam);
                            cmd.Parameters.Remove(statusParam);
                            cmd.Parameters.Remove(modoParam);
                            //cmd.Parameters.Remove(verDateParam);
                            //cmd.Parameters.Remove(verUserParam);
                            //cmd.Parameters.Remove(finUserParam);
                            //cmd.Parameters.Remove(finDateParam);
                            //cmd.Parameters.Remove(revDateParam);
                            //cmd.Parameters.Remove(revUserParam);
                            //cmd.Parameters.Remove(conditionsParam);
                            cmd.Parameters.Remove(importDateParam);

                            cmd.Parameters.Add(whereParam);
                            whereParam.Value = where;

                            cmd.CommandText = string.Concat(myUpdate_Imported2);
                            myReturn = cmd.ExecuteNonQuery();

                            transaction.Commit();

                        }

                    }
                }
                myBoolReturn = true;
            }
            catch (Exception ex)
            {
                if (transaction != null)
                {
                    try
                    {
                        transaction.Rollback();
                    }
                    catch
                    {
                        if (transaction.Connection != null)
                        {
                            Console.WriteLine("An exception of type " + ex.GetType() +
                                " was encountered while attempting to roll back the transaction.");
                        }
                    }
                }
                if (myBoolErrorNoHayLotes)
                    throw new Exception(ex.Message);
                else
                    throw new Exception(ex.ToString() + "\r\nMyChangeTF Error\r\nAn exception of type" + ex.GetType());
            }
            return myBoolReturn;
        }
        public bool MyChangeCandidatos(bool isInsert, string partido, string numCand, string nombre, string area, string cargo, string endoReq, int modo, string where)
        {
            bool myBoolReturn = false;
            try
            {
                string[] myInsert =
                        {
                            "INSERT INTO [dbo].[Candidatos] ([Partido],[NumCand],[Nombre],[Area],[Cargo],[EndoReq],[Modo]) ",
                            "VALUES (@Partido,@NumCand,@Nombre,@Area,@Cargo,@EndoReq,@Modo)"
                        };

                string[] myUpdate =
                    {
                            "UPDATE [dbo].[Candidatos] ",
                            "SET [Partido] = @Partido, ",
                            "[NumCand] = @NumCand,",
                            "[Nombre] = @Nombre,",
                            "[Area] = @Area,",
                            "[Cargo] =@Cargo,",
                            "[EndoReq]=@EndoReq,",
                            "[Modo]=@Modo ",
                            "WHERE NumCand=@Where"
                };

                using (SqlConnection cnn = new SqlConnection()
                {
                    ConnectionString = DBCnnStr
                })
                {
                    using (SqlCommand cmd = new SqlCommand()
                    {
                        Connection = cnn,
                        CommandType = CommandType.Text,
                        CommandText = isInsert == true ? string.Concat(myInsert) : string.Concat(myUpdate)
                    })
                    {
                        if (cnn.State == ConnectionState.Closed)
                            cnn.Open();

                        cmd.Parameters.Add(new SqlParameter("@Partido", SqlDbType.VarChar));
                        cmd.Parameters.Add(new SqlParameter("@NumCand", SqlDbType.VarChar));
                        cmd.Parameters.Add(new SqlParameter("@Nombre", SqlDbType.VarChar));
                        cmd.Parameters.Add(new SqlParameter("@Area", SqlDbType.VarChar));
                        cmd.Parameters.Add(new SqlParameter("@Cargo", SqlDbType.Int));
                        cmd.Parameters.Add(new SqlParameter("@EndoReq", SqlDbType.Int));
                        cmd.Parameters.Add(new SqlParameter("@Modo", SqlDbType.Int));

                        if (!isInsert)
                            cmd.Parameters.Add(new SqlParameter("@Where", SqlDbType.VarChar));

                        cmd.Parameters["@Partido"].Value = partido;
                        cmd.Parameters["@NumCand"].Value = numCand;
                        cmd.Parameters["@Nombre"].Value = nombre;
                        cmd.Parameters["@Area"].Value = area;
                        cmd.Parameters["@Cargo"].Value = cargo;
                        cmd.Parameters["@EndoReq"].Value = endoReq;
                        cmd.Parameters["@Modo"].Value = modo;

                        if (!isInsert)
                            cmd.Parameters["@Where"].Value = where;

                        int myReturn = cmd.ExecuteNonQuery();

                        if (myReturn <= 0)
                            myBoolReturn = false;
                        else
                            myBoolReturn = true;

                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString() + "\r\nMyInsertArea Error");
            }
            return myBoolReturn;
        }
        public bool MyProcessLot(string numlot, string usercode, ObservableCollection<Criterios> CollCriterios, ObservableCollection<string> lblNReasons, ObservableCollection<int> ProgressBar_Value, ObservableCollection<int> ProgressBar_Maximum, ObservableCollection<string> Resultados, int modo)
        {
            /*
                'STATUS LOTE para la tabla lots
                    '0 - LISTO PARA PROCESAR
                    '1 - SIENDO PROCESADA
                    '2 - FINALIZADO
                    '3 - CON ERRORES
                    '4 - SIENDO REVISADA
           
                'STATUS LOTE para la tabla de TF
                    '0 - SIN IMPORTAR
                    '1 - SIENDO IMPORTADO
                    '2 - IMPORTADO
            */

            Resultados.Clear();
            Resultados.Add(numlot);//lblLote               0
            Resultados.Add("0");//lblTota                  1
            Resultados.Add("0");//lblProcesadas            2    
            Resultados.Add("0");//lblAprobadas             3    
            Resultados.Add("0");//lblRechazadas            4
            Resultados.Add("0");//lblWarnings              5


            ProgressBar_Maximum.Clear();
            ProgressBar_Value.Clear();
            ProgressBar_Maximum.Add(100);
            ProgressBar_Value.Add(0);

            bool myBoolReturn = false;
            bool myBoolErrorNoHayLotes = true;
            //SqlTransaction transaction = null;
            DataTable myDataToProcessTF = new DataTable();
            int LotRechazo = 2;
            //'Define memory variables to hold information from access DB
            //'and modify them if needed during the validation process
            String m_BatchTrack = string.Empty;
            String m_BatchNo = string.Empty;
            int m_BatchPgNo = 0;
            String m_N_ELEC = string.Empty;

            string m_Nombre = string.Empty;
            string m_Paterno = string.Empty;
            string m_Materno = string.Empty;

            String m_N_PRECINTO = string.Empty;
            DateTime? m_FECHA_N = null;
            String m_FECHA_N2 = string.Empty;
            String m_SEXO = string.Empty;
            String m_PARTIDO = string.Empty;
            int m_Cargo = 0;
            String m_N_CANDIDAT = string.Empty;
            String m_N_CANDIDAT_Good = string.Empty;

            String m_N_NOTARIO = string.Empty;
            DateTime? m_Fecha_Endo_Notario = null;
            String m_Suspense_File = string.Empty;
            String m_Batch = string.Empty;
            String m_Firma_Peticionario = string.Empty;
            String m_Firma_Notario = string.Empty;
            int m_Firma_Pet_Inv = 0;
            int m_Firma_Not_Inv = 0;
            string m_Padre = string.Empty;
            string m_Madre = string.Empty;
            int? m_Leer_Inv = 0;
            string m_Leer = string.Empty;
            int m_Alteracion = 0;
            DateTime? m_Firma_Fecha_Elector = null;
            DateTime? m_Fecha_Recibo_CEE = null;
            string m_EndosoImage = string.Empty;
            try
            {
                // Verifica que ele lote este importado
                string[] mySqlstrTF = { "Select * ",
                                        "from [dbo].[TF-Partidos] ",
                                        "Where Imported = 2 and BatchTrack=@lot ",
                                        "Order By Partido,BatchTrack, BatchNo, BatchPgNo;" };

                // 'VERIFICA QUE EL LOTE EXISTA EN TELEFORM
                string[] mySqlstrTFCount = { "Select count(*) ",
                                        "from [dbo].[TF-Partidos] ",
                                        "Where Imported = 2 and BatchTrack=@lot;" };

                // 'VERIFICA QUE EL LOTE EXISTA
                string[] mySqlstrLotsCount = { "Select count(*) ",
                                          "from [dbo].[Lots] ",
                                          "Where Status = 0 and Lot=@lot; " };

                /* BORRA LOS ERRORES ANTERIORES DEL LOTE */
                string[] mySqlStrDeleteLotsVoid ={"DELETE FROM LotsVoid ",
                                                  "WHERE Lot =@lot" };

                string[] mySqlStrDeleteLotsEndo ={"DELETE FROM LotsEndo ",
                                                  "WHERE Lot =@lot" };

                //'ACTUALIZA EL STATUS DEL LOTE - STATUS 1
                string[] mySqlStrUpdateLots = { "Update Lots",
                                                " Set Status = 1",
                                                " Where Lot=@lot",
                                                " And Status = 0" };

                //'STATUS LOTE
                //'0 - LISTO PARA PROCESAR
                //'1 - SIENDO PROCESADA
                //'2 - FINALIZADO
                //'3 - CON ERRORES
                //'4 - SIENDO REVISADA



                SqlConnection myCnnDBEndosos = new SqlConnection();
                SqlConnection myCnnDBEndososValidarDatos = new SqlConnection();


                SqlConnection myCnnDBCeeMaster = new SqlConnection();
                SqlConnection myCnnDBImg = new SqlConnection();
                SqlConnection myCnnRadicacionesCEE = new SqlConnection();

                SqlCommand myCmdDBEndosos = new SqlCommand();
                //SqlCommand myCmdDBCeeMaster = new SqlCommand();
                //SqlCommand myCmdDBImg = new SqlCommand();

                SqlDataAdapter myDataAdapter = new SqlDataAdapter();


                myCnnDBEndosos.ConnectionString = DBCnnStr;
                myCnnDBEndososValidarDatos.ConnectionString = DBCnnStr;

                myCnnRadicacionesCEE.ConnectionString = DBRadicacionesCEECnnStr;

                myCnnDBCeeMaster.ConnectionString = DBCeeMasterCnnStr;
                myCnnDBImg.ConnectionString = DBImagenesCnnStr;

                myCnnDBEndosos.Open();
                myCnnDBEndososValidarDatos.Open();

                myCnnDBCeeMaster.Open();
                myCnnDBImg.Open();
                myCnnRadicacionesCEE.Open();

                myCmdDBEndosos.Connection = myCnnDBEndosos;
                //myCmdDBCeeMaster.Connection = myCnnDBCeeMaster;
                //myCmdDBImg.Connection = myCnnDBImg;

                myCmdDBEndosos.CommandType = CommandType.Text;
                myCmdDBEndosos.CommandText = string.Concat(mySqlstrLotsCount);

                SqlParameter lotParam = new SqlParameter();
                lotParam.ParameterName = "@lot";
                lotParam.SqlDbType = SqlDbType.VarChar;

                myCmdDBEndosos.Parameters.Add(lotParam);
                lotParam.Value = numlot;

                if ((int)myCmdDBEndosos.ExecuteScalar() <= 0)
                    throw new Exception("No encuentro el Lote Seleccionado");

                myCmdDBEndosos.CommandText = string.Concat(mySqlstrTFCount);

                ProgressBar_Maximum[0] = (int)myCmdDBEndosos.ExecuteScalar();

                if (ProgressBar_Maximum[0] <= 0)
                    throw new Exception("No encuentro el Lote Seleccionado");


                myCmdDBEndosos.CommandText = string.Concat(mySqlstrTF);
                myDataAdapter.SelectCommand = myCmdDBEndosos;
                myDataAdapter.Fill(myDataToProcessTF);

                if (myDataToProcessTF == null)
                    throw new Exception("No encuentro el Lote Seleccionado");

                if (myDataToProcessTF.Rows.Count <= 0)
                    throw new Exception("No encuentro el Lote Seleccionado");

                myBoolErrorNoHayLotes = false;

                // Start a local transaction.
                //transaction = myCnnDBEndosos.BeginTransaction(IsolationLevel.ReadCommitted);

                //myCmdDBEndosos.Transaction = transaction;

                myCmdDBEndosos.CommandText = string.Concat(mySqlStrDeleteLotsVoid);
                myCmdDBEndosos.ExecuteNonQuery();

                myCmdDBEndosos.CommandText = string.Concat(mySqlStrDeleteLotsEndo);
                myCmdDBEndosos.ExecuteNonQuery();

                myCmdDBEndosos.CommandText = string.Concat(mySqlStrUpdateLots);
                myCmdDBEndosos.ExecuteNonQuery();

                //    transaction.Commit();

                int[] Rechazo = new int[23];
                int[] Warning = new int[23];
                bool[] isRechazo = new bool[23];
                bool[] isWarning = new bool[23];
                Resultados[1] = ProgressBar_Maximum[0].ToString();         //lblTota                  1
                string strRechazos = string.Empty;
                int ValidatedEndorsements = 0;
                int RejectedEndorsements = 0;

                foreach (DataRow row in myDataToProcessTF.Rows)//processing
                {
                    ProgressBar_Value[0]++;

                    Resultados[2] = ProgressBar_Value[0].ToString();//lblProcesadas            2    

                    //transaction = myCnnDBEndosos.BeginTransaction(IsolationLevel.ReadCommitted);
                    //myCmdDBEndosos.Transaction = transaction;

                    isRechazo = new bool[23];
                    isWarning = new bool[23];

                    //default
                    m_Firma_Peticionario = "0";
                    m_Firma_Notario = "0";
                    m_BatchTrack = string.Empty;
                    m_N_ELEC = string.Empty;

                    m_Nombre = string.Empty;
                    m_Materno = string.Empty;
                    m_Paterno = string.Empty;

                    m_N_NOTARIO = string.Empty;
                    m_N_CANDIDAT = string.Empty;
                    m_N_CANDIDAT_Good = "0";
                    m_Cargo = -1;
                    m_FECHA_N = null;
                    m_SEXO = string.Empty;
                    m_N_PRECINTO = string.Empty;
                    m_Firma_Pet_Inv = -1;
                    m_Firma_Not_Inv = -1;
                    m_BatchPgNo = -1;
                    m_PARTIDO = string.Empty;
                    m_BatchNo = string.Empty;
                    m_Batch = string.Empty;
                    m_Suspense_File = string.Empty;
                    m_Padre = string.Empty;
                    m_Madre = string.Empty;
                    m_Leer = string.Empty;
                    m_Leer_Inv = 0;
                    m_Alteracion = 0;
                    m_Firma_Fecha_Elector = null;
                    m_EndosoImage = string.Empty;
                    strRechazos = string.Empty;
                    //
                    // m_Alteracion  ==row["Alteracion"].ToString().Trim();

                    int.TryParse(row["Alteracion"].ToString().Trim(), out m_Alteracion);
                    m_Leer = row["leer"].ToString().Trim();

                    //Notario
                    string tmpFirmaFecha_Endo_Notario = String.Empty;
                    try
                    {
                        if (modo == 2)
                        {
                            if (row["FechaEndo_Ano"].ToString().Trim().Length == 4)
                                tmpFirmaFecha_Endo_Notario = string.Concat(row["FechaEndo_Mes"].ToString().Trim().PadLeft(2, '0'), row["FechaEndo_Dia"].ToString().Trim().PadLeft(2, '0'), row["FechaEndo_Ano"].ToString().Trim().PadLeft(4, '0'));
                            else
                                tmpFirmaFecha_Endo_Notario = string.Concat(row["FechaEndo_Mes"].ToString().Trim().PadLeft(2, '0'), row["FechaEndo_Dia"].ToString().Trim().PadLeft(2, '0'), row["FechaEndo_Ano"].ToString().Trim().PadLeft(2, '0'));
                        }else
                        {
                            if (row["FechaFirm_Mes"].ToString().Trim().Length == 4)
                                tmpFirmaFecha_Endo_Notario = string.Concat(row["FechaFirm_Mes"].ToString().Trim().PadLeft(2, '0'), row["FechaFirm_Dia"].ToString().Trim().PadLeft(2, '0'), row["FechaFirm_Ano"].ToString().Trim().PadLeft(4, '0'));
                            else
                                tmpFirmaFecha_Endo_Notario = string.Concat(row["FechaFirm_Mes"].ToString().Trim().PadLeft(2, '0'), row["FechaFirm_Dia"].ToString().Trim().PadLeft(2, '0'), row["FechaFirm_Ano"].ToString().Trim().PadLeft(2, '0'));

                        }

                    }
                    catch
                    {
                        tmpFirmaFecha_Endo_Notario = "01012019";
                    }
                    //Elector
                    string tmpFirma_Fecha_Elector = string.Empty;
                    try
                    {
                        tmpFirma_Fecha_Elector = string.Concat(row["FechaFirm_Mes"].ToString().Trim().PadLeft(2, '0'), row["FechaFirm_Dia"].ToString().Trim().PadLeft(2, '0'), row["FechaFirm_Ano"].ToString().Trim().PadLeft(2, '0'));
                    }
                    catch
                    {
                        tmpFirma_Fecha_Elector = "01012019";//dummy date
                    }
                    //Fecha_Recibo_CEE
                    string tmpFecha_Recibo_CEE = string.Empty;
                    try
                    {
                        tmpFecha_Recibo_CEE = string.Concat(row["FechaRecibo_Mes"].ToString().Trim().PadLeft(2, '0'), row["FechaRecibo_Dia"].ToString().Trim().PadLeft(2, '0'), row["FechaRecibo_Ano"].ToString().Trim().PadLeft(2, '0'));
                    }
                    catch
                    {
                        tmpFecha_Recibo_CEE = "01012019"; //dummy date
                    }


                    try { int.TryParse(row["BatchPgNo"].ToString().Trim(), out m_BatchPgNo); } catch { throw; };

                    try { int.TryParse(row["FirmaElec_Inv"].ToString().Trim(), out m_Firma_Pet_Inv); } catch { throw; };
                    try { int.TryParse(row["FirmaNot_Inv"].ToString().Trim(), out m_Firma_Not_Inv); } catch { throw; };

                    try { int.TryParse(row["Funcionario"].ToString().Trim(), out m_Cargo); } catch { throw; }

                    //  m_Firma_Pet_Inv = 0; // se inicializa en 0 para que el empleado pueda evaluar la firma con la master de cee
                    // m_Firma_Not_Inv = 0;// se inicializa en 0 para que el empleado pueda evaluar la firma con la master de cee

                    m_Nombre = row["Nombre"].ToString().Trim();
                    m_Materno = row["Materno"].ToString().Trim();
                    m_Paterno = row["Paterno"].ToString().Trim();

                    m_Padre = row["Padre"].ToString().Trim();
                    m_Madre = row["Madre"].ToString().Trim();

                    if (row["Leer_Inv"] != null)
                        m_Leer_Inv = (int)row["Leer_Inv"];


                    m_PARTIDO = row["Partido"].ToString().Trim();
                    m_BatchNo = row["BatchNo"].ToString().Trim();
                    m_Batch = m_BatchNo;// row["Suspense_File"].ToString().Trim().Split('\\')[3];

                    m_Suspense_File = row["Suspense_File"].ToString().Trim();

                    // m_EndosoImage = row["BatchDir"].ToString().Trim();

                    //if (!m_EndosoImage.EndsWith("\\"))
                    //    m_EndosoImage += "\\";

                    m_EndosoImage = m_Suspense_File.Trim();


                    m_Firma_Peticionario = row["FirmaElector"].ToString().Trim();
                    m_Firma_Notario = row["FirmaNotario"].ToString().Trim();
                    m_BatchTrack = row["BatchTrack"].ToString().Trim();

                    try
                    {
                        m_N_ELEC = row["NumElec"].ToString().Trim();
                    }
                    catch
                    {
                        m_N_ELEC = "0";
                    }

                    m_N_NOTARIO = row["Notario_Funcionario"].ToString().Trim().PadLeft(7, '0');
                    m_N_CANDIDAT = row["Num_Candidato"].ToString().PadLeft(3, '0');
                    m_N_PRECINTO = row["Precinto"].ToString().PadLeft(3, '0');

                    m_SEXO = row["SEXO"].ToString().Trim();

                    string tmpFECHA_N = string.Empty;

                    try
                    {
                        tmpFECHA_N = string.Concat(row["FechaNac_Mes"].ToString().Trim().PadLeft(2, '0'), row["FechaNac_Dia"].ToString().Trim().PadLeft(2, '0'), row["FechaNac_Ano"].ToString().Trim().PadLeft(4, '0'));
                    }
                    catch
                    {
                        tmpFECHA_N = "01012019"; // dummy date
                    }

                    if (tmpFECHA_N.Trim().Length > 0)
                    {
                        m_FECHA_N = DateTimeUtil.MyValidarFecha(tmpFECHA_N);
                    }

                    if (tmpFirma_Fecha_Elector.Trim().Length > 0)
                    {
                        m_Firma_Fecha_Elector = DateTimeUtil.MyValidarFechaMMddyy(tmpFirma_Fecha_Elector);
                    }

                    if (tmpFecha_Recibo_CEE.Trim().Length > 0)
                    {
                        m_Fecha_Recibo_CEE = DateTimeUtil.MyValidarFecha(tmpFecha_Recibo_CEE);
                    }

                    if (tmpFirmaFecha_Endo_Notario.Length <= 0)
                    {
                        m_Fecha_Endo_Notario = null;
                    }
                    else
                    {
                        m_Fecha_Endo_Notario = DateTimeUtil.MyValidarFecha(tmpFirmaFecha_Endo_Notario);
                        if (m_Fecha_Endo_Notario == null)
                        {
                            m_Fecha_Endo_Notario = DateTimeUtil.MyValidarFechaMMddyy(tmpFirmaFecha_Endo_Notario);
                        }
                    }

                    float m_Firma_Peticionario_tmp_f = 0.0f;
                    int m_Firma_Peticionario_tmp_i = 0;

                    if (float.TryParse(m_Firma_Peticionario, out m_Firma_Peticionario_tmp_f))
                    {
                        m_Firma_Peticionario_tmp_i = (int)m_Firma_Peticionario_tmp_f;
                    }


                    if (((CollCriterios[0].Editar == true) || (CollCriterios[0].Warning == true)) && (m_Firma_Peticionario_tmp_i == 0))//'1-ELECTOR NO FIRMO EL ENDOSO
                    {
                        if (CollCriterios[0].Editar == true)
                        {
                            Rechazo[0]++;
                            isRechazo[0] = true;
                            strRechazos = "1|";
                        }
                        if (CollCriterios[0].Warning == true)
                        {
                            Warning[0]++;
                            isWarning[0] = true;
                        }
                    }

                    float m_Firma_Notario_f = 0.0f;
                    int m_Firma_Notario_i = 0;

                    if (float.TryParse(m_Firma_Notario, out m_Firma_Notario_f))
                    {
                        m_Firma_Notario_i = (int)m_Firma_Notario_f;
                    }

                    if (((CollCriterios[1].Editar == true) || (CollCriterios[1].Warning == true)) && m_Firma_Notario_i == 0)//2-'NOTARIO NO FIRMO EL ENDOSO
                    {
                        if (CollCriterios[1].Editar == true)
                        {
                            Rechazo[1]++;
                            strRechazos += "2|";
                            isRechazo[1] = true;
                        }

                        if (CollCriterios[1].Warning == true)
                        {
                            Warning[1]++;
                            isWarning[1] = true;
                        }
                    }

                    if (CollCriterios[2].Editar == true || CollCriterios[2].Warning == true)//'3-FECHA DEL ENDOSO FUERA DE TIEMPO (esta es la de los 7 dias)
                    {
                        if (m_Fecha_Endo_Notario != null)
                        {
                            //   if (DateTimeUtil.DateDiff(DateInterval.Day, m_Firma_Fecha, m_Fecha_Endo) > 7)
                            if (m_Leer_Inv > 7 || m_Leer_Inv <= -1)
                            {
                                if (CollCriterios[2].Editar == true)
                                {
                                    Rechazo[2]++;
                                    strRechazos += "3|";
                                    isRechazo[2] = true;
                                }

                                if (CollCriterios[2].Warning == true)
                                {
                                    Warning[2]++;
                                    isWarning[2] = true;
                                }
                            }
                        }
                        else
                        {
                            if (CollCriterios[2].Editar == true)
                            {
                                Rechazo[2]++;
                                strRechazos += "3|";
                                isRechazo[2] = true;
                            }
                            if (CollCriterios[2].Warning == true)
                            {
                                Warning[2]++;
                                isWarning[2] = true;
                            }
                        }
                    }

                    if (CollCriterios[3].Editar == true || CollCriterios[3].Warning == true)//'4-NOTARIO NO EXISTE EN NUESTROS ARCHIVOS
                    {

                        string tblNotarioi = "[Notarios]";

                        if (modo == 2)
                            tblNotarioi = "[NotariosPartidos]";

                        string[] sqlstr = { "SELECT count(*) ",
                                                "From ",tblNotarioi,
                                                " Where ([NumElec] = ", FixNum( m_N_NOTARIO) , ") and ",
                                                " ([Partido]='", m_PARTIDO , "')"};
                                               
                        object total = null;

                        if (MyValidarDatos(string.Concat(sqlstr), out total, myCnnDBEndososValidarDatos) != null)
                        {
                            if ((int)total == 0)
                            {
                                if (CollCriterios[3].Editar == true)
                                {
                                    Rechazo[3]++;
                                    strRechazos += "4|";
                                    isRechazo[3] = true;
                                }
                                if (CollCriterios[3].Warning == true)
                                {
                                    Warning[3]++;
                                    isWarning[3] = true;
                                }
                            }
                        }
                        else
                        {
                            if (CollCriterios[3].Editar == true)
                            {
                                Rechazo[3]++;
                                strRechazos += "4|";
                                isRechazo[3] = true;
                            }

                            if (CollCriterios[3].Warning == true)
                            {
                                Warning[3]++;
                                isWarning[3] = true;
                            }
                        }
                    }

                    if (CollCriterios[4].Editar == true || CollCriterios[4].Warning == true)// '5-ELECTOR IGUAL AL NOTARIO
                    {
                        if (m_N_ELEC == m_N_NOTARIO)
                        {
                            if (CollCriterios[4].Editar == true)
                            {
                                Rechazo[4]++;
                                strRechazos += "5|";
                                isRechazo[4] = true;
                            }
                            if (CollCriterios[4].Warning == true)
                            {
                                Warning[4]++;
                                isWarning[4] = true;
                            }
                        }
                    }

                    if (CollCriterios[5].Editar == true || CollCriterios[5].Warning == true)// '6-ELECTOR NO EXISTE
                    {
                        string[] sqlstr = { "SELECT count(*) ",
                                                "From [usercid].[Citizen] ",
                                                " Where [CitizenID] = '",  FixNum(m_N_ELEC) , "'"};
                        object total = null;
                        if (MyValidarDatos(string.Concat(sqlstr), out total, myCnnDBCeeMaster) != null)
                        {
                            if ((int)total == 0)
                            {
                                if (CollCriterios[5].Editar == true)
                                {
                                    Rechazo[5]++;
                                    strRechazos += "6|";
                                    isRechazo[5] = true;
                                }
                                if (CollCriterios[5].Warning == true)
                                {
                                    Warning[5]++;
                                    isWarning[5] = true;
                                }
                            }
                        }
                        else
                        {
                            if (CollCriterios[5].Editar == true)
                            {
                                Rechazo[5]++;
                                strRechazos += "6|";
                                isRechazo[5] = true;
                            }
                            if (CollCriterios[5].Warning == true)
                            {
                                Warning[5]++;
                                isWarning[5] = true;
                            }
                        }

                    }

                    if (CollCriterios[6].Editar == true || CollCriterios[6].Warning == true)// '7-ASPIRANTE NO EXISTE
                    {
                        string[] sqlstr = { "SELECT count(*) ",
                                                "From [Candidatos]  ",
                                                " Where [NumCand] = '", FixNum( m_N_CANDIDAT ), "'"};
                        object total = null;

                        if (MyValidarDatos(string.Concat(sqlstr), out total, myCnnDBEndososValidarDatos) != null)
                        {
                            if ((int)total == 0)
                            {
                                string[] sqlstrPartido = {
                                    "SELECT count(*) ",
                                                "From [Candidatos]  ",
                                                " Where [Partido] = '", m_PARTIDO, "'"};

                                if (MyValidarDatos(string.Concat(sqlstrPartido), out total, myCnnDBEndososValidarDatos) != null)
                                {
                                    if ((int)total == 0)
                                    {
                                        if (CollCriterios[6].Editar == true)
                                        {
                                            Rechazo[6]++;
                                            strRechazos += "7|";
                                            isRechazo[6] = true;
                                        }
                                        if (CollCriterios[6].Warning == true)
                                        {
                                            Warning[6]++;
                                            isWarning[6] = true;
                                        }
                                    }
                                    else
                                    {
                                        m_N_CANDIDAT_Good = m_N_CANDIDAT;
                                    }
                                }
                            }
                            else
                            {
                                m_N_CANDIDAT_Good = m_N_CANDIDAT;
                            }
                        }
                        else
                        {
                            if (CollCriterios[6].Editar == true)
                            {
                                Rechazo[6]++;
                                strRechazos += "7|";
                                isRechazo[6] = true;
                            }
                            if (CollCriterios[6].Warning == true)
                            {
                                Warning[6]++;
                                isWarning[6] = true;
                            }
                        }

                    }

                    if (CollCriterios[7].Editar == true || CollCriterios[7].Warning == true)//'8-NOTARIO INACTIVO
                    {
                        string[] sqlstr = { "SELECT status ",
                                                "From [usercid].[Citizen] ",
                                                " Where [CitizenID] = '",FixNum(m_N_NOTARIO) , "'"};
                        object status = null;

                        if (MyValidarDatos(string.Concat(sqlstr), out status, myCnnDBCeeMaster) == null)
                        {//No Exite
                            if (CollCriterios[7].Editar == true)
                            {
                                Rechazo[7]++;
                                strRechazos += "8|";
                                isRechazo[7] = true;
                            }
                            if (CollCriterios[7].Warning == true)
                            {
                                Warning[7]++;
                                isWarning[7] = true;
                            }
                        }
                        else
                        {
                            if (status != null)
                            {
                                if (!MyStatus(status.ToString()))
                                {
                                    if (CollCriterios[7].Editar == true)
                                    {
                                        Rechazo[7]++;
                                        strRechazos += "8|";
                                        isRechazo[7] = true;
                                    }
                                    if (CollCriterios[7].Warning == true)
                                    {
                                        Warning[7]++;
                                        isWarning[7] = true;
                                    }
                                }
                            }
                            else
                            {//Estatus es Null
                                if (CollCriterios[7].Editar == true)
                                {
                                    Rechazo[7]++;
                                    strRechazos += "8|";
                                    isRechazo[7] = true;
                                }
                                if (CollCriterios[7].Warning == true)
                                {
                                    Warning[7]++;
                                    isWarning[7] = true;
                                }
                            }
                        }
                    }

                    if (CollCriterios[8].Editar == true || CollCriterios[8].Warning == true) //9-FECHA NACIMIENTO NO CONCUERDA
                    {
                        string[] sqlstr = { "SELECT [DateOfBirth] ",
                                                "From [usercid].[Citizen] ",
                                                " Where [CitizenID] = '", FixNum( m_N_ELEC ), "'"};
                        object DateOfBirth = null;

                        if (MyValidarDatos(string.Concat(sqlstr), out DateOfBirth, myCnnDBCeeMaster) == null)
                        {
                            if (CollCriterios[8].Editar == true)
                            {
                                Rechazo[8]++;
                                strRechazos += "9|";

                                isRechazo[8] = true;
                            }
                            if (CollCriterios[8].Warning == true)
                            {
                                Warning[8]++;
                                isWarning[8] = true;
                            }
                        }
                        else
                        {
                            if (DateTimeUtil.DateDiff(DateInterval.Day, (DateTime)DateOfBirth, m_FECHA_N) > 0)
                            {
                                if (CollCriterios[8].Editar == true)
                                {
                                    Rechazo[8]++;
                                    strRechazos += "9|";
                                    isRechazo[8] = true;
                                }
                                if (CollCriterios[8].Warning == true)
                                {
                                    Warning[8]++;
                                    isWarning[8] = true;
                                }
                            }
                        }
                    }

                    if (CollCriterios[9].Editar == true || CollCriterios[9].Warning == true)//'10-TIPO DE SEXO NO CONUERDA
                    {
                        string[] sqlstr = { "SELECT [Gender] ",
                                                "From [usercid].[Citizen] ",
                                                " Where [CitizenID] = '", FixNum( m_N_ELEC) , "'"};
                        object Gender = null;

                        if (MyValidarDatos(string.Concat(sqlstr), out Gender, myCnnDBCeeMaster) == null)
                        {
                            if (CollCriterios[9].Editar == true)
                            {
                                Rechazo[9]++;
                                strRechazos += "10|";

                                isRechazo[9] = true;
                            }
                            if (CollCriterios[9].Warning == true)
                            {
                                Warning[9]++;
                                isWarning[9] = true;
                            }
                        }
                        else
                        {
                            if (Gender.ToString() != m_SEXO)
                            {
                                if (CollCriterios[9].Editar == true)
                                {
                                    Rechazo[9]++;
                                    strRechazos += "10|";
                                    isRechazo[9] = true;
                                }
                                if (CollCriterios[9].Warning == true)
                                {
                                    Warning[9]++;
                                    isWarning[9] = true;
                                }
                            }
                        }
                    }

                    if (CollCriterios[10].Editar == true || CollCriterios[10].Warning == true)// '11-STATUS ELECTO EXCLUIDO
                    {
                        string[] sqlstr = { "SELECT status ",
                                                "From [usercid].[Citizen] ",
                                                " Where [CitizenID] = '",  FixNum(m_N_ELEC) , "'"};
                        object status = null;

                        if (MyValidarDatos(string.Concat(sqlstr), out status, myCnnDBCeeMaster) == null)
                        {// No Exite
                            //if (CollCriterios[10].Editar == true)
                            //{
                            //    Rechazo[10]++;
                            //    strRechazos += "11|";
                            //    isRechazo[10] = true;
                            //}
                            //if (CollCriterios[10].Warning == true)
                            //{
                            //    Warning[10]++;
                            //    isWarning[10] = true;
                            //}
                        }
                        else
                        {
                            if (status != null)
                            {
                                if (!MyStatus(status.ToString()))
                                {
                                    if (CollCriterios[10].Editar == true)
                                    {
                                        Rechazo[10]++;
                                        strRechazos += "11|";
                                        isRechazo[10] = true;
                                    }
                                    if (CollCriterios[10].Warning == true)
                                    {
                                        Warning[10]++;
                                        isWarning[10] = true;
                                    }
                                }
                            }
                            else
                            {//Estatus es Null
                                if (CollCriterios[10].Editar == true)
                                {
                                    Rechazo[10]++;
                                    strRechazos += "11|";
                                    isRechazo[10] = true;
                                }
                                if (CollCriterios[10].Warning == true)
                                {
                                    Warning[10]++;
                                    isWarning[10] = true;
                                }
                            }
                        }

                    }


                    object FirstGeoCode = null;

                    string[] sqlstrPrecinto = { "SELECT FirstGeoCode ",
                                                "From [usercid].[Citizen] ",
                                                " Where [CitizenID] = '", FixNum( m_N_ELEC) , "'"};

                    if (MyValidarDatos(string.Concat(sqlstrPrecinto), out FirstGeoCode, myCnnDBCeeMaster) == null)
                    {
                        FirstGeoCode = "000";
                    }

                    if (CollCriterios[11].Editar == true || CollCriterios[11].Warning == true)// 12-'PRECINTO NO CONCUERDA
                    {

                        if (FirstGeoCode != null)
                        {
                            string precintoMasterCee = FirstGeoCode.ToString().Trim().PadLeft(3, '0');


                            if (precintoMasterCee != m_N_PRECINTO)
                            {
                                if (CollCriterios[11].Editar == true)
                                {
                                    Rechazo[11]++;
                                    strRechazos += "12|";
                                    isRechazo[11] = true;
                                }
                                if (CollCriterios[11].Warning == true)
                                {
                                    Warning[11]++;
                                    isWarning[11] = true;
                                }
                            }
                        }
                        else
                        {
                            if (CollCriterios[11].Editar == true)
                            {
                                Rechazo[11]++;
                                strRechazos += "12|";
                                isRechazo[11] = true;
                            }
                            if (CollCriterios[11].Warning == true)
                            {
                                Warning[11]++;
                                isWarning[11] = true;
                            }
                        }
                    }

                    // 'SOLO PARA SENADOR DISTRITO, REPRESENTANTE DISTRITO, ALCALDE, ASMBLEISTA MUNICIPAL

                    if (m_Cargo == 3 || m_Cargo == 5 || m_Cargo == 7 || m_Cargo == 8)
                    {

                        if (CollCriterios[12].Editar == true || CollCriterios[12].Warning == true)//13-'PRECINTO ELECTOR DISTINTO AL DEL CANDIDATO
                        {

                            string precintoMasterCee = FirstGeoCode.ToString().Trim().PadLeft(3, '0');

                            object CodigoArea = null;
                            string[] sqlstr2 = { "SELECT [Precintos] ",
                                                "From [dbo].[Areas] ",
                                                "where Area in (select Area from [dbo].[Candidatos] where [NumCand]=" + FixNum( m_N_CANDIDAT ),")"};

                            if (MyValidarDatos(string.Concat(sqlstr2), out CodigoArea, myCnnDBEndososValidarDatos) == null)
                            {//No Exite

                            }
                            if (CodigoArea != null)
                            {
                                if (!CodigoArea.ToString().Contains(precintoMasterCee))
                                {
                                    if (CollCriterios[12].Editar == true)
                                    {
                                        Rechazo[12]++;
                                        strRechazos += "13|";
                                        isRechazo[12] = true;
                                    }
                                    if (CollCriterios[12].Warning == true)
                                    {
                                        Warning[12]++;
                                        isWarning[12] = true;
                                    }
                                }
                            }
                        }
                    }//End if (m_Cargo == 3 || m_Cargo == 5 || m_Cargo == 7 || m_Cargo == 8) 

                    if (CollCriterios[13].Editar == true || CollCriterios[13].Warning == true)// 14-'MULTIPLES ENDOSOS PARA EL MISMO CANDIDATO
                    {
                        string[] sqlstr = { "SELECT count(*) ",
                                                "From [LotsEndo]  ",
                                                "Where [Cargo] = '",  m_Cargo.ToString() , "' ",
                                                "and ",
                                                "Candidato='",m_N_CANDIDAT,"' And Status = 0 ",
                                                "and NumElec='", FixNum( m_N_ELEC ), "'" };
                        object total = null;
                        if (MyValidarDatos(string.Concat(sqlstr), out total, myCnnDBEndososValidarDatos) != null)//>0
                        {
                            if ((int)total > 0)
                            {
                                if (CollCriterios[13].Editar == true)
                                {
                                    Rechazo[13]++;
                                    strRechazos += "14|";
                                    isRechazo[13] = true;
                                }
                                if (CollCriterios[13].Warning == true)
                                {
                                    Warning[13]++;
                                    isWarning[13] = true;
                                }
                            }
                        }
                    }//end if 14-

                    if (CollCriterios[14].Editar == true || CollCriterios[14].Warning == true)//15- 'MULTIPLES ENDOSOS PARA EL MISMO CARGO
                    {

                        string[] sqlstr = { "SELECT count(*) ",
                                                "From [LotsEndo]  ",
                                                "Where [NumElec] = '", FixNum(m_N_ELEC) , "' ",
                                                "and ",
                                                "Cargo='",m_Cargo.ToString(),"' And Status = 0"};
                        object total = null;

                        if (MyValidarDatos(string.Concat(sqlstr), out total, myCnnDBEndososValidarDatos) != null)
                        {
                            switch (m_Cargo)
                            {
                                case 0: //Partido
                                    {
                                        string strTotalPermitido = ConfigurationManager.AppSettings["Partido"].ToString();
                                        int.TryParse(strTotalPermitido, out int totalPermitido);

                                        if ((int)total >= totalPermitido) //if ((int)total >= 1)
                                        {
                                            if (CollCriterios[14].Editar == true)
                                            {
                                                Rechazo[14]++;
                                                strRechazos += "15|";
                                                isRechazo[14] = true;
                                            }
                                            if (CollCriterios[14].Warning == true)
                                            {
                                                Warning[14]++;
                                                isWarning[14] = true;
                                            }
                                        }
                                        break;
                                    }
                                case 1:// 'Gobernador
                                    {
                                        string strTotalPermitido = ConfigurationManager.AppSettings["Gobernador"].ToString();
                                        int.TryParse(strTotalPermitido, out int totalPermitido);
                                        if ((int)total >= totalPermitido) //if ((int)total >= 1)
                                        {
                                            if (CollCriterios[14].Editar == true)
                                            {
                                                Rechazo[14]++;
                                                strRechazos += "15|";
                                                isRechazo[14] = true;
                                            }
                                            if (CollCriterios[14].Warning == true)
                                            {
                                                Warning[14]++;
                                                isWarning[14] = true;
                                            }
                                        }
                                    }
                                    break;

                                case 2://'Comisionado Residente
                                    {
                                        string strTotalPermitido = ConfigurationManager.AppSettings["ComisionadoResidente"].ToString();
                                        int.TryParse(strTotalPermitido, out int totalPermitido);
                                        if ((int)total >= totalPermitido) //if ((int)total >= 1)
                                        {
                                            if (CollCriterios[14].Editar == true)
                                            {
                                                Rechazo[14]++;
                                                strRechazos += "15|";
                                                isRechazo[14] = true;
                                            }
                                            if (CollCriterios[14].Warning == true)
                                            {
                                                Warning[14]++;
                                                isWarning[14] = true;
                                            }
                                        }
                                    }
                                    break;

                                case 3: //'Senador Distrito
                                    {
                                        string strTotalPermitido = ConfigurationManager.AppSettings["SenadorDistrito"].ToString();
                                        int.TryParse(strTotalPermitido, out int totalPermitido);
                                        if ((int)total >= totalPermitido) //if ((int)total >= 2)
                                        {
                                            if (CollCriterios[14].Editar == true)
                                            {
                                                Rechazo[14]++;
                                                strRechazos += "15|";
                                                isRechazo[14] = true;
                                            }
                                            if (CollCriterios[14].Warning == true)
                                            {
                                                Warning[14]++;
                                                isWarning[14] = true;
                                            }
                                        }
                                    }
                                    break;

                                case 4://'Senador Acumulación
                                    {
                                        string strTotalPermitido = ConfigurationManager.AppSettings["SenadorAcumulacionCEE"].ToString();
                                        int.TryParse(strTotalPermitido, out int totalPermitido);


                                        if (m_PARTIDO == "PNP")
                                        {
                                            totalPermitido = 6;
                                            strTotalPermitido = ConfigurationManager.AppSettings["SenadorAcumulacionPNP"].ToString();
                                            int.TryParse(strTotalPermitido, out totalPermitido);
                                        }

                                        if (m_PARTIDO == "PPD")
                                        {
                                            totalPermitido = 6;
                                            strTotalPermitido = ConfigurationManager.AppSettings["SenadorAcumulacionPPD"].ToString();
                                            int.TryParse(strTotalPermitido, out totalPermitido);
                                        }

                                        if ((int)total >= totalPermitido)
                                        {
                                            if (CollCriterios[14].Editar == true)
                                            {
                                                Rechazo[14]++;
                                                strRechazos += "15|";
                                                isRechazo[14] = true;
                                            }
                                            if (CollCriterios[14].Warning == true)
                                            {
                                                Warning[14]++;
                                                isWarning[14] = true;
                                            }
                                        }
                                    }
                                    break;

                                case 5: //'Representante Distrito
                                    {
                                        string strTotalPermitido = ConfigurationManager.AppSettings["RepresentanteDistrito"].ToString();
                                        int.TryParse(strTotalPermitido, out int totalPermitido);

                                        if ((int)total >= totalPermitido) //if ((int)total >= 1)
                                        {
                                            if (CollCriterios[14].Editar == true)
                                            {
                                                Rechazo[14]++;
                                                strRechazos += "15|";
                                                isRechazo[14] = true;
                                            }
                                            if (CollCriterios[14].Warning == true)
                                            {
                                                Warning[14]++;
                                                isWarning[14] = true;
                                            }
                                        }
                                    }
                                    break;

                                case 6: //'Representante Acumulación
                                    {
                                        string strTotalPermitido = ConfigurationManager.AppSettings["RepresentanteAcumulacionCEE"].ToString();
                                        int.TryParse(strTotalPermitido, out int totalPermitido);

                                        if (m_PARTIDO == "PNP")
                                        {
                                            totalPermitido = 6;
                                            strTotalPermitido = ConfigurationManager.AppSettings["RepresentanteAcumulacionPNP"].ToString();
                                            int.TryParse(strTotalPermitido, out totalPermitido);
                                        }

                                        if (m_PARTIDO == "PPD")
                                        {
                                            totalPermitido = 6;
                                            strTotalPermitido = ConfigurationManager.AppSettings["RepresentanteAcumulacionPPD"].ToString();
                                            int.TryParse(strTotalPermitido, out totalPermitido);
                                        }

                                        if ((int)total >= totalPermitido)
                                        {
                                            if (CollCriterios[14].Editar == true)
                                            {
                                                Rechazo[14]++;
                                                strRechazos += "15|";
                                                isRechazo[14] = true;
                                            }
                                            if (CollCriterios[14].Warning == true)
                                            {
                                                Warning[14]++;
                                                isWarning[14] = true;
                                            }
                                        }
                                    }
                                    break;

                                case 7: //'Alcalde
                                    {
                                        string strTotalPermitido = ConfigurationManager.AppSettings["Alcalde"].ToString();
                                        int.TryParse(strTotalPermitido, out int totalPermitido);

                                        if ((int)total >= totalPermitido) //if ((int)total >= 1)
                                        {
                                            if (CollCriterios[14].Editar == true)
                                            {
                                                Rechazo[14]++;
                                                strRechazos += "15|";
                                                isRechazo[14] = true;
                                            }
                                            if (CollCriterios[14].Warning == true)
                                            {
                                                Warning[14]++;
                                                isWarning[14] = true;
                                            }
                                        }
                                    }
                                    break;

                                case 8: //'Legislador Municipales
                                    {//Falta tabala 
                                        object totalPermitidos = null;
                                        string precintoMasterCee = FirstGeoCode.ToString().Trim().PadLeft(3, '0');

                                        string[] sql = {
                                            "Select [Postulados] from [dbo].[tblLegisladoresMunicipalesPostulados] ",
                                            "where [Precinto] like '%",precintoMasterCee,"%'"
                                        };

                                        if (MyValidarDatos(string.Concat(sqlstr), out totalPermitidos, myCnnDBEndososValidarDatos) != null)
                                        {
                                            if ((int)total >= (int)totalPermitidos)
                                            {
                                                if (CollCriterios[14].Editar == true)
                                                {
                                                    Rechazo[14]++;
                                                    strRechazos += "15|";
                                                    isRechazo[14] = true;
                                                }

                                                if (CollCriterios[14].Warning == true)
                                                {
                                                    Warning[14]++;
                                                    isWarning[14] = true;
                                                }
                                            }
                                        }
                                        else
                                        {
                                            if (CollCriterios[14].Editar == true)
                                            {
                                                Rechazo[14]++;
                                                strRechazos += "15|";
                                                isRechazo[14] = true;
                                            }

                                            if (CollCriterios[14].Warning == true)
                                            {
                                                Warning[14]++;
                                                isWarning[14] = true;
                                            }
                                        }
                                    }
                                    break;
                            }
                        }
                    }

                    if (CollCriterios[15].Editar == true || CollCriterios[15].Warning == true)//16-'FIRMA DEL ELECTOR NO ES IGUAL A LA DEL ARCHIVO MAESTRO
                    {
                        if (m_Firma_Pet_Inv == 1)
                        {
                            if (CollCriterios[15].Editar == true)
                            {
                                Rechazo[15]++;
                                strRechazos += "16|";
                                isRechazo[15] = true;
                            }
                            if (CollCriterios[15].Warning == true)
                            {
                                Warning[15]++;
                                isWarning[15] = true;
                            }
                        }
                    }

                    if (CollCriterios[16].Editar == true || CollCriterios[16].Warning == true)//17- 'FIRMA DEL NOTARIO NO ES IGUAL A LA DEL ARCHIVO MAESTRO
                    {
                        if (m_Firma_Not_Inv == 1)
                        {
                            if (CollCriterios[16].Editar == true)
                            {
                                Rechazo[16]++;
                                strRechazos += "17|";
                                isRechazo[16] = true;
                            }
                            if (CollCriterios[16].Warning == true)
                            {
                                Warning[16]++;
                                isWarning[16] = true;
                            }
                        }
                    }

                    if (m_Alteracion == 1) //18- Otro Rechazo
                    {
                        Rechazo[17]++;
                        strRechazos += "18|";
                        isRechazo[17] = true;
                    }

                    if (CollCriterios[18].Editar == true || CollCriterios[18].Warning == true) //19- No concuerda el cargo
                    {
                        string[] sqlstr = { "select [Cargo] from [dbo].[Candidatos] where [NumCand]=" + FixNum(m_N_CANDIDAT), ";" };

                        //string[] sqlstr = { "SELECT [ElectivePositionID] ",
                        //                        "From [dbo].[Areas] ",
                        //                        "where Area in (select Area from [dbo].[FilingEndorsements] where [ElectoralNumber]=" + FixNum( m_N_CANDIDAT ),")"};

                        object Cargo = null;

                        if (MyValidarDatos(string.Concat(sqlstr), out Cargo, myCnnDBEndosos) == null)
                        {
                            Rechazo[18]++;
                            strRechazos += "19|";
                            isRechazo[18] = true;
                        }
                        else
                        {
                            if (m_Cargo != (int)Cargo)
                            {
                                Rechazo[18]++;
                                strRechazos += "19|";
                                isRechazo[18] = true;
                            }
                        }
                    }

                    if (CollCriterios[19].Editar == true || CollCriterios[19].Warning == true)//'20- Elector no concuerda
                    {
                        string[] sqlstr = { "SELECT [FirstName] ",
                                                "From [usercid].[Citizen] ",
                                                " Where [CitizenID] = '", FixNum(m_N_ELEC) , "'"};
                        object FirstName = null;

                        if (MyValidarDatos(string.Concat(sqlstr), out FirstName, myCnnDBCeeMaster) != null)
                        {
                            string[] m_Nombre_t = { " " };
                            if (m_Nombre.Trim().Length > 0)
                            {
                                if (m_Nombre.Contains(" "))
                                    m_Nombre_t = m_Nombre.Split(' ');
                                else
                                    m_Nombre_t[0] = m_Nombre.Trim();

                            }

                            //if (FirstName.ToString().Trim().ToUpper() != m_Nombre.Trim().ToUpper())
                            if (!FirstName.ToString().Trim().ToUpper().Contains(m_Nombre_t[0].Trim().ToUpper()))
                            {
                                string[] FirstName_t = { " " };

                                if (FirstName != null)
                                    if (FirstName.ToString().Trim().Length > 0)
                                        if (FirstName.ToString().Contains(" "))
                                            FirstName_t = FirstName.ToString().Split(' ');
                                        else
                                            FirstName_t[0] = FirstName.ToString().Trim();

                                if (!m_Nombre.ToString().Trim().ToUpper().Contains(FirstName_t[0].ToString().Trim().ToUpper()))
                                {
                                    if (CollCriterios[19].Editar == true)
                                    {
                                        Rechazo[19]++;
                                        strRechazos += "20|";
                                        isRechazo[19] = true;
                                    }
                                    if (CollCriterios[19].Warning == true)
                                    {
                                        Warning[19]++;
                                        isWarning[19] = true;
                                    }
                                }
                            }
                        }
                    }

                    if (CollCriterios[20].Editar == true || CollCriterios[20].Warning == true)//'21- Endosaron a otros partidos
                    {
                        string[] sqlstr = { "SELECT [NumElec] ",
                                                "From [tblEndosaronOtroPartido2019] ",
                                                " Where [NumElec] = '", FixNum(m_N_ELEC) , "'"};
                        object total = null;
                        if (MyValidarDatos(string.Concat(sqlstr), out total, myCnnDBEndosos) != null)
                        {
                            if ((int)total > 0)
                            {
                                if (CollCriterios[20].Editar == true)
                                {
                                    Rechazo[20]++;
                                    strRechazos += "21|";
                                    isRechazo[20] = true;
                                }
                                if (CollCriterios[20].Warning == true)
                                {
                                    Rechazo[20]++;
                                    isWarning[20] = true;
                                }
                            }
                        }
                    }

                    if (CollCriterios[21].Editar == true || CollCriterios[21].Warning == true)//'22- hay discrepancia entre la fecha de la petición y la fecha de juramento
                    {
                        if (!m_Firma_Fecha_Elector.Equals(m_Fecha_Endo_Notario))
                        {
                            if (CollCriterios[21].Editar == true)
                            {
                                Rechazo[21]++;
                                strRechazos += "22|";
                                isRechazo[21] = true;
                            }
                            if (CollCriterios[21].Warning == true)
                            {
                                Rechazo[21]++;
                                isWarning[21] = true;
                            }
                        }

                    }


                    if (isRechazo[3] == false) // Nota: si NOTARIO NO EXISTE EN NUESTROS ARCHIVOS no verifico la fecha de informado a la CEE
                    {
                        //'23- La petición ha sido juramentada ante un notario ad hoc cuyo nombre no le ha sido informado a la Comisión.
                        if (CollCriterios[22].Editar == true || CollCriterios[22].Warning == true)
                        {
                            string tblNotario = "[Notarios]";

                            if (modo == 2)
                                tblNotario = "[NotariosPartidos]";

                            string[] sqlstr = {
                                "SELECT [Fecha_Dia],[Fecha_Mes],[Fecha_Ano] ",
                                "From ",tblNotario,
                                " Where ([NumElec] = ", FixNum(m_N_NOTARIO) , ") and ",
                                " ([Partido]='", m_PARTIDO , "')"
                            };

                            int result = 0;
                            if (m_Fecha_Endo_Notario == null)
                            {
                                result = 1; // para rechazar por fecha null
                            }
                            else
                            {
                                DateTime? dt = MyValidarNotarioInformadoCEE(string.Concat(sqlstr), myCnnDBEndososValidarDatos);
                                result = DateTime.Compare(dt.Value, m_Fecha_Endo_Notario.Value);
                            }

                            // result < 0  is earlier than (es mas tarde)
                            // result == 0 is the same time as
                            // result >0  is later than

                            if (result > 0)
                            {
                                if (CollCriterios[22].Editar == true)
                                {
                                    Rechazo[22]++;
                                    strRechazos += "23|";
                                    isRechazo[22] = true;
                                }
                                if (CollCriterios[22].Warning == true)
                                {
                                    Rechazo[22]++;
                                    isWarning[22] = true;
                                }
                            }
                        }
                    }


                    bool isrechazo = false;
                    bool iswarning = false;

                    byte[] img = null;

                    if (File.Exists(m_EndosoImage))
                        img = GetEndosoImage(m_EndosoImage);
                    else
                        throw new Exception("Problema con la imagen\r\n" + m_EndosoImage);


                    //'ACTUALIZA LOS COUNTERS DE LA PANTALLA
                    for (int X = 0; X < CollCriterios.Count; X++)
                    {
                        if (isRechazo[X])
                        {
                            lblNReasons[X] = Rechazo[X].ToString();
                            //'ESCRIBE LOS ERRORES A LA TABLA LOTSVOID
                            WriteVoid(m_BatchTrack, m_Batch, m_BatchPgNo, row["NumElec"].ToString().Trim(), X + 1, m_PARTIDO, 0, img, myCmdDBEndosos, modo);
                            isrechazo = true;
                        }
                        else if (isWarning[X])
                        {
                            lblNReasons[X] = Warning[X].ToString();
                            //'ESCRIBE LOS ERRORES A LA TABLA LOTSVOID (Warning)
                            WriteVoid(m_BatchTrack, m_Batch, m_BatchPgNo, row["NumElec"].ToString().Trim(), X + 1, m_PARTIDO, 2, img, myCmdDBEndosos, modo);
                            iswarning = true;
                        }
                    }

                    if (!isrechazo)
                    {
                        ValidatedEndorsements++;
                        int Aprobadas = int.Parse(Resultados[3]);
                        Aprobadas++;
                        Resultados[3] = Aprobadas.ToString();//lblAprobadas             3
                    }
                    else
                    {
                        RejectedEndorsements++;
                        LotRechazo = 3;
                        int Rechazadas = int.Parse(Resultados[4]);
                        Rechazadas++;
                        Resultados[4] = Rechazadas.ToString();//lblRechazadas            4
                    }

                    if (iswarning)
                    {
                        int Warnings = int.Parse(Resultados[5]);
                        Warnings++;
                        Resultados[5] = Warnings.ToString();//lblWarnings               5
                    }

                    //    m_Nombre = m_Nombre.Replace("'", "''");
                    SqlParameter txtNombreParam = new SqlParameter();
                    txtNombreParam.ParameterName = "@txtNombre";
                    txtNombreParam.SqlDbType = SqlDbType.VarChar;
                    txtNombreParam.Value = m_Nombre;
                    //  'ESCRIBE LOS ENDOSOS A LA TABLA LOTSENDO
                    string[] mySqlStrLOTSENDO =
                    {
                        "Insert Into LotsEndo",
                        "([Partido],[Lot],[Batch],[Formulario],[Candidato],[Cargo],[NumElec],[Nombre],[Paterno],[Materno],[Padre],[Madre],[FechaNac],[Leer_Inv],[Alteracion],[Notario]",
                           ",[Firma_Peticionario],[Firma_Pet_Inv],[Firma_Notario],[Firma_Not_Inv],[Fecha_Endoso],[Image],[Status],[Firma_Fecha],[SEXO],[PRECINTO],[EndosoImage],[Errores],[LeerMSG],[Fecha_Recibo],[Modo] ) ",

                        "Values('" , m_PARTIDO , "'",                               //Partido
                        ",'" , m_BatchTrack , "'",                                  //Lot
                        ",'" , m_Batch , "'",                                       //Batch
                        "," , m_BatchPgNo.ToString(),                               //Formulario
                        ",'" , m_N_CANDIDAT , "'",                                  //Candidato
                        "," , m_Cargo.ToString(),                                   //Cargo
                        ",'" , row["NumElec"].ToString().Trim() , "'",              //NumElec
                        ", @txtNombre",                                             //Nombre
                        ",'" , m_Paterno , "'",                                     //Paterno
                        ",'" , m_Materno , "'",                                     //Materno
                        ",'" ,m_Padre , "'",                                        //Padre
                        ",'" ,m_Madre , "'",                                        //Madre
                        ",'" , MyFechaToSql( m_FECHA_N ),  "'",                     //FechaNac
                        ",'" , m_Leer_Inv.ToString() , "'",                         //Leer_Inv
                        ",'" , m_Alteracion.ToString() , "'",                       //Alteracion
                        ",'" , m_N_NOTARIO , "'",                                   //Notario
                        ",'" , m_Firma_Peticionario , "'",                          //Firma_Peticionario
                        "," , m_Firma_Pet_Inv.ToString(),                           //Firma_Pet_Inv
                        ",'" , m_Firma_Notario , "'",                               //Firma_Notario
                        "," , m_Firma_Not_Inv.ToString() ,                          //Firma_Not_Inv
                        ",'" , MyFechaToSql(m_Fecha_Endo_Notario) , "'",            //Fecha_Endoso Notario
                        ",'" , m_Suspense_File , "'",                               //Image
                        "," , (string)iif(isrechazo,  "1", iswarning ?"2":"0") ,    //Status
                        ",'" , MyFechaToSql( m_Firma_Fecha_Elector ), "'",          //Firma_Fecha Recibo
                        ",'" , m_SEXO , "'",                                        //SEXO
                        ",'" , m_N_PRECINTO , "'",                                  //PRECINTO
                        ", @EndosoImage",
                        ",'",strRechazos , "'",
                        ",'",m_Leer , "'",                                           // Otro Rechazo
                        ",'",MyFechaToSql(m_Fecha_Recibo_CEE),"',",modo.ToString(),
                        ")"

                    };
                    //'STATUS ENDOSO
                    //'0 = Sin Errores
                    //'1 = Con Errores
                    //'2 = Con Warnings
                    SqlParameter param = new SqlParameter();
                    param.ParameterName = "@EndosoImage";
                    param.SqlDbType = SqlDbType.Image;
                    param.Size = img.Length;
                    param.Value = img;
                    myCmdDBEndosos.Parameters.Add(param);
                    myCmdDBEndosos.Parameters.Add(txtNombreParam);
                    myCmdDBEndosos.CommandText = string.Concat(mySqlStrLOTSENDO);
                    myCmdDBEndosos.ExecuteNonQuery();

                    myCmdDBEndosos.Parameters.Remove(param);
                    myCmdDBEndosos.Parameters.Remove(txtNombreParam);


                    //   transaction.Commit();
                    DoEvents();
                }//end loop
                //[]
                //'ACTUALIZA EL STATUS DEL LOTE
                string mySqlqlstrUpdate = "Update Lots";
                mySqlqlstrUpdate = mySqlqlstrUpdate + " Set Status = " + LotRechazo.ToString() + ", verdate = getdate(), veruser = '" + usercode + "',";
                mySqlqlstrUpdate = mySqlqlstrUpdate + "ValidatedEndorsements=" + ValidatedEndorsements.ToString() + ",RejectedEndorsements=" + RejectedEndorsements.ToString();
                mySqlqlstrUpdate = mySqlqlstrUpdate + ",StatusReydi=0,Num_Candidato=" + m_N_CANDIDAT_Good;
                mySqlqlstrUpdate = mySqlqlstrUpdate + " Where Lot='" + m_BatchTrack + "'";
                mySqlqlstrUpdate = mySqlqlstrUpdate + " And Status = 1";

                myCmdDBEndosos.CommandText = string.Concat(mySqlqlstrUpdate);
                myCmdDBEndosos.ExecuteNonQuery();

                //'STATUS LOTE
                //'0 - LISTO PARA PROCESAR
                //'1 - SIENDO PROCESADA
                //'2 - FINALIZADO
                //'3 - CON ERRORES PARA REVISAR
                //'4 - SIENDO REVISADA

                myBoolReturn = true;
            }
            catch (Exception ex)
            {
                MyReverseLots(numlot, usercode);
                //if (transaction != null)
                //{
                //    try
                //    {
                //        transaction.Rollback();
                //    }
                //    catch
                //    {
                //        if (transaction.Connection != null)
                //        {
                //            Console.WriteLine("An exception of type " + ex.GetType() +
                //                " was encountered while attempting to roll back the transaction.");
                //        }
                //    }
                //}
                if (myBoolErrorNoHayLotes)
                    throw new Exception(ex.Message);
                else
                    throw new Exception(ex.ToString() + "\r\nMyProcessLot Error");
            }
            return myBoolReturn;
        }

        private DateTime? MyValidarNotarioInformadoCEE(string sql, SqlConnection cnn)
        {
            DateTime? myReturn = null;

            using (SqlCommand cmd = new SqlCommand(sql, cnn)
            {
                CommandType = CommandType.Text
            })
            {
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable table = new DataTable();
                da.Fill(table);
                foreach (DataRow dr in table.Rows)
                {
                    string tmp_Fecha = string.Concat(dr["Fecha_Mes"].ToString().Trim().PadLeft(2, '0'),
                                                                  dr["Fecha_Dia"].ToString().Trim().PadLeft(2, '0'),
                                                                  dr["Fecha_Ano"].ToString().Trim().PadLeft(4, '0'));
                    if (tmp_Fecha.Trim().Length > 0)
                    {
                        myReturn = DateTimeUtil.MyValidarFechaMMddyy(tmp_Fecha);
                    }

                }
            }
            return myReturn;
        }

        private object MyValidarDatos(string sql, SqlConnection cnn)
        {
            object myIntReturn = 0;


            using (SqlCommand cmd = new SqlCommand(sql, cnn)
            {
                CommandType = CommandType.Text
            })
            {
                try
                {
                    myIntReturn = (int)cmd.ExecuteScalar();
                }
                catch
                {
                    myIntReturn = (string)cmd.ExecuteScalar();

                }
            }

            return myIntReturn;
        }

        private object MyValidarDatos(string sql, out object returnValue, SqlConnection cnn)
        {

            returnValue = null;

            using (SqlCommand cmd = new SqlCommand(sql, cnn)
            {
                CommandType = CommandType.Text,

            })
            {
                try
                {
                    returnValue = cmd.ExecuteScalar();
                }
                catch
                {
                    throw new Exception("Error " + sql);
                }
            }

            return returnValue;
        }

        private string Left(string param, int length)
        {
            string result = param.Substring(0, length);
            return result;
        }

        private bool MyStatus(string param)
        {
            switch (param.Trim())
            {
                case "A":
                case "I":
                    return true;
                case "E":
                    return false;
                default:
                    return true;


            }
        }

        private string MyFechaToSql(DateTime? param)
        {
            if (param == null)
                return string.Empty;

            DateTime d = (DateTime)param;


            return d.ToString("MM/dd/yyyy");



        }

        private string FixNum(string param)
        {
            int FixNum = 0;

            if (int.TryParse(param, out FixNum))
                return param;
            else
                return "0";
        }

        public void DoEvents()
        {
            Application.Current.Dispatcher.Invoke(DispatcherPriority.Background,
            new Action(delegate { }));
        }

        private void WriteVoid(string Lot, string BatchNo, int Formulario, string NumElec, int Rechazo, string m_PARTIDO, int Status, byte[] EndosoImage, SqlCommand dbCmd, int modo)
        {
            // 'ESCRIBE EL ENDOSO RECHAZADO
            // 'STATUS DEL RECHAZO
            //'0 - SIN CORREGIR
            //'1 - CORREJIDO
            //'2 - WARNING



            string sqlstr = "Insert Into LotsVoid Values";
            sqlstr = sqlstr + "('" + m_PARTIDO + "'";
            sqlstr = sqlstr + ",'" + Lot + "'";
            sqlstr = sqlstr + ",'" + BatchNo + "'";
            sqlstr = sqlstr + "," + Formulario;
            sqlstr = sqlstr + ",'" + Rechazo + "'";
            sqlstr = sqlstr + ",'" + NumElec + "'";
            sqlstr = sqlstr + ", " + Status;
            sqlstr = sqlstr + ", @EndosoImage";
            sqlstr = sqlstr + ", " + modo.ToString() + ")";


            SqlParameter param = new SqlParameter();
            param.ParameterName = "@EndosoImage";
            param.SqlDbType = SqlDbType.Image;
            param.Size = EndosoImage.Length;
            param.Value = EndosoImage;
            dbCmd.Parameters.Add(param);

            //dbCmd.Parameters.Add("@EndosoImage", SqlDbType.Image, img.Length).Value = img;
            dbCmd.CommandText = sqlstr;
            dbCmd.ExecuteNonQuery();

            dbCmd.Parameters.Remove(param);

        }

        public bool MyUpdateTFTable(string txtNombre, string txtNumElec, string txtPrecinto, string txtSexo, string txtFechaNac, string txtCargo, string txtNotario,
                                     string txtCandidato, string txtFirma, string txtNotarioFirma, string chkFirmaInv, string chkFirmaNotInv, string chkAlteracion, string txtOtraRazonDeRechazo,
                                     string txtFechaFirmaElector,
                                     string txtFechaFirmaEndososNotario,
                                     string txtFechaReciboCEE,
                                     int? totalDeDias, string Lot, string Batch, string Formulario, string CurrElect, string SysUser, SqlCommand cmd)
        {
            bool myBoolReturn = false;

            try
            {
                string FechaNac_Mes = null;
                string FechaNac_Dia = null;
                string FechaNac_Ano = null;

                if (!string.IsNullOrEmpty(txtFechaNac))
                {
                    FechaNac_Mes = txtFechaNac.Substring(0, 2);
                    FechaNac_Dia = txtFechaNac.Substring(2, 2);
                    FechaNac_Ano = txtFechaNac.Substring(4, 4);
                }
                string FechaFirm_Mes_Elector = null;
                string FechaFirm_Dia_Elector = null;
                string FechaFirm_Ano_Elector = null;

                if (!string.IsNullOrEmpty(txtFechaFirmaElector))
                {
                    string[] fecha = txtFechaFirmaElector.Split('/');
                    FechaFirm_Mes_Elector = fecha[0];
                    FechaFirm_Dia_Elector = fecha[1];
                    FechaFirm_Ano_Elector = fecha[2];//
                }

                string FechaFirmaEndo_Mes_Notario = string.Empty;
                string FechaFirmaEndo_Dia_Notario = string.Empty;
                string FechaFirmaEndo_Ano_Notario = string.Empty;

                if (!string.IsNullOrEmpty(txtFechaFirmaEndososNotario))
                {
                    string[] fechaendosos = txtFechaFirmaEndososNotario.Split('/');
                    FechaFirmaEndo_Mes_Notario = fechaendosos[0];
                    FechaFirmaEndo_Dia_Notario = fechaendosos[1];
                    FechaFirmaEndo_Ano_Notario = fechaendosos[2];
                }

                string FechaRecibo_Mes_CEE = string.Empty;
                string FechaRecibo_Dia_CEE = string.Empty;
                string FechaRecibo_Ano_CEE = string.Empty;

                if (!string.IsNullOrEmpty(txtFechaReciboCEE))
                {
                    string[] fecha = txtFechaReciboCEE.Split('/');
                    FechaRecibo_Mes_CEE = fecha[0];
                    FechaRecibo_Dia_CEE = fecha[1];
                    FechaRecibo_Ano_CEE = fecha[2];

                }

                //   txtNombre = txtNombre.Replace("'", "''");

                SqlParameter txtNombreParam = new SqlParameter();
                txtNombreParam.ParameterName = "@txtNombre";
                txtNombreParam.SqlDbType = SqlDbType.VarChar;
                txtNombreParam.Value = txtNombre;

                string[] updatequery =
                    {
                    "Update [TF-Partidos] ",
                    "Set NumElec = '", txtNumElec , "'",
                    ", Precinto = '" ,txtPrecinto , "'",
                    ", SEXO = '" , txtSexo , "'",
                    ", FechaNac_Mes = '" ,FechaNac_Mes, "'",
                    ", FechaNac_Dia = '" ,FechaNac_Dia, "'",
                    ", FechaNac_Ano = '" ,FechaNac_Ano, "'",
                    ", Funcionario = '" , txtCargo , "'",
                    ", Notario_Funcionario = '" , txtNotario, "'",
                    ", Num_Candidato = '" , txtCandidato , "'",
                     ", FirmaElector = '" , txtFirma , "'",
                     ", FirmaNotario = '" ,txtNotarioFirma,"'",
                     ", FirmaElec_Inv = " , chkFirmaInv,
                     ", FirmaNot_Inv = " , chkFirmaNotInv,
                     ", Alteracion = " , chkAlteracion,
                     ", Leer = '" , txtOtraRazonDeRechazo,"'",
                     ", Leer_Inv =",totalDeDias.ToString(),
                     ", FechaFirm_Mes  = '" ,FechaFirm_Mes_Elector  , "'",
                     ", FechaFirm_Dia = '" ,FechaFirm_Dia_Elector , "'",
                     ", FechaFirm_Ano = '" ,FechaFirm_Ano_Elector  , "'",
                     ",FechaEndo_Mes ='",FechaFirmaEndo_Mes_Notario,"'",
                     ",FechaEndo_Dia='",FechaFirmaEndo_Dia_Notario,"'",
                     ", FechaEndo_Ano='",FechaFirmaEndo_Ano_Notario,"'",
                     ",FechaRecibo_Mes='", FechaRecibo_Mes_CEE,"'",
                     ",FechaRecibo_Dia='", FechaRecibo_Dia_CEE,"'",
                     ",FechaRecibo_Ano='", FechaRecibo_Ano_CEE,"'",
                     ", Nombre=@txtNombre",
                     " Where NumElec ='" , CurrElect , "'",
                    " And BATCHTRACK = '" , Lot, "'",
                    " And BatchPgNo=",Formulario,
                    " And BatchNo='",Batch,"'"
                    };

                //    'STATUS DEL RECHAZO
                //    '0 - SIN CORREGIR
                //    '1 - CORREJIDO

                //'ACTUALIZA EL STATUS DEL ERROR ACABADO DE CORREGIR
                string[] sqlstr = { "Update LotsVoid ",
                         " Set STATUS = 1",
                         " Where Lot = '" , Lot, "'",
                         " And Batch = '" , Batch, "'",
                         " And Formulario = " , Formulario };

                cmd.CommandText = string.Concat(sqlstr);
                cmd.ExecuteNonQuery();

                cmd.Parameters.Add(txtNombreParam);
                cmd.CommandText = string.Concat(updatequery);
                cmd.ExecuteNonQuery();
                cmd.Parameters.Remove(txtNombreParam);

                myBoolReturn = true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString() + "\r\nMyUpdateTFTable ");
            }
            return myBoolReturn;
        }

        private object iif(bool expression, object truePart, object falsePart)
        {

            return expression ? truePart : falsePart;
        }


        #region Dispose
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~SqlExcuteCommand()
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
