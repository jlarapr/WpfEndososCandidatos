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
            } set
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
                int myIntCanridad = 0;

                if (!int.TryParse(cantidad, out myIntCanridad))
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
                            if ((int)cmd.ExecuteScalar() != myIntCanridad)
                                throw new Exception("La cantidad de Endosos no concuerda");



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
                    throw new Exception(ex.ToString() + "\r\nMyGetSelectLotes Error");
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
        public DataTable MyGetCitizen(string CitizenID)
        {
            DataTable myTableReturn = new DataTable();
            try
            {

                //string mySqlstr = "Select * From [usercid].[Citizen] Where CitizenID =@CitizenID ";
                string[] mySqlstr = { "SELECT A.*,B.SignatureImage,B.PhotoImage ",
                                       "FROM [usercid].[Citizen] A join [usercid].[CitizenImages] B ",
                                       "on a.CItizenId = b.CitizenID ",
                                        "where a.CItizenId =@CitizenID" };

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
                            cmd.Parameters.Add(new SqlParameter("@CitizenID", SqlDbType.VarChar));
                            cmd.Parameters["@CitizenID"].Value = CitizenID;

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
        public DataTable MyGetLot()
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

                string mySqlstr = "Select * from lots Where Status In (0,1,2,3,4) order by Lot";

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
                throw new Exception(ex.ToString() + "\r\nMyGetLot Error");
            }
            return myTableReturn;
        }

        public DataTable MyGetLotToProcess()
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

                string mySqlstr = "Select * from lots Where Status =0 order by Lot";

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
        public DataTable MyGetCriterios()
        {
            DataTable myTableReturn = new DataTable();
            try
            {
                //string mySqlstr = "Select Precinto + ' - ' + [Desc] from Precintos order by Precinto";
                string mySqlstr = "Select * from Criterios order by Campo";

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
        public DataTable MyGetPartidos()
        {
            DataTable myTableReturn = new DataTable();
            try
            {
                //string mySqlstr = "Select Area + ' - ' + [Desc] from areas order by area";
                string mySqlstr = "Select * from Partidos order by partido";

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
        public DataTable MyGetNotarios()
        {
            DataTable myTableReturn = new DataTable();
            try
            {
                string mySqlstr = "Select * from notarios order by NumElec";

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
        public DataTable MyGetCandidatos()
        {
            DataTable myTableReturn = new DataTable();
            try
            {
                string mySqlstr = "Select * from Candidatos order by NumCand";

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
        public bool MyDeleteNotario(string where, string where2)
        {
            bool myBoolReturn = false;
            string[] myDelete =
                       {
                            "Delete from [dbo].[Notarios] ",
                            " WHERE NumElec=@Where and partido=@where2"
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
                        cmd.Parameters.Add(new SqlParameter("@Where2", SqlDbType.VarChar));
                        cmd.Parameters["@Where"].Value = where;
                        cmd.Parameters["@Where2"].Value = where2;

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
        public bool MyChangePartidos(bool isInsert, string partido, string desc, string endoReq, string area, string where)
        {
            bool myBoolReturn = false;
            try
            {
                string[] myInsert =
                        {
                            "INSERT INTO [dbo].[Partidos] ([Partido],[Desc],[EndoReq],[Area]) ",
                            "VALUES (@Partido,@Desc,@EndoReq,@Area)"
                        };

                string[] myUpdate =
                    {
                            "UPDATE [dbo].[Partidos] ",
                            "SET [Partido] = @Partido, ",
                            "[Desc] = @Desc,",
                            "[EndoReq] = @EndoReq,",
                            "[Area] = @Area ",
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

                        if (!isInsert)
                            cmd.Parameters.Add(new SqlParameter("@Where", SqlDbType.VarChar));

                        cmd.Parameters["@Partido"].Value = partido;
                        cmd.Parameters["@Desc"].Value = desc;
                        cmd.Parameters["@EndoReq"].Value = endoReq;
                        cmd.Parameters["@Area"].Value = area.Substring(0, 4);

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
        public bool MyChangeNotario(bool isInsert, string NumElec, string Partido, string Nombre, string Apellido1, string Apellido2, string where)
        {
            bool myBoolReturn = false;
            try
            {


                string[] myInsert =
                        {
                            "INSERT INTO [dbo].[Notarios] ([NumElec],[Partido],[Nombre],[Apellido1],[Apellido2]) ",
                            "VALUES (@NumElec,@Partido,@Nombre,@Apellido1,@Apellido2)"
                        };

                string[] myUpdate =
                    {
                            "UPDATE [dbo].[Notarios] ",
                            "SET [NumElec] = @NumElec,",
                            "[Partido] = @Partido,",
                            "[Nombre] = @Nombre,",
                            "[Apellido1] = @Apellido1,",
                            "[Apellido2] = @Apellido2 ",
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
                        CommandText = isInsert == true ? string.Concat(myInsert) : string.Concat(myUpdate)
                    })
                    {
                        if (cnn.State == ConnectionState.Closed)
                            cnn.Open();

                        cmd.Parameters.Add(new SqlParameter("@NumElec", SqlDbType.Int));
                        cmd.Parameters.Add(new SqlParameter("@Partido", SqlDbType.VarChar));
                        cmd.Parameters.Add(new SqlParameter("@Nombre", SqlDbType.VarChar));
                        cmd.Parameters.Add(new SqlParameter("@Apellido1", SqlDbType.VarChar));
                        cmd.Parameters.Add(new SqlParameter("@Apellido2", SqlDbType.VarChar));

                        if (!isInsert)
                            cmd.Parameters.Add(new SqlParameter("@Where", SqlDbType.VarChar));

                        cmd.Parameters["@NumElec"].Value = NumElec;
                        cmd.Parameters["@Partido"].Value = Partido;
                        cmd.Parameters["@Nombre"].Value = Nombre;
                        cmd.Parameters["@Apellido1"].Value = Apellido1;
                        cmd.Parameters["@Apellido2"].Value = Apellido2;

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
        public bool MyChangeCriterios(string Campo, bool? Editar, string Desc, bool? Warning)
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
        public bool MyChangeTF(DataTable tableLots, string usercode)
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

                string[] myInsert =
                        {
                            "Insert Into lots  ([Partido],[Lot],[Amount],[Usercode],[AuthDate],[Status],[VerDate],[VerUser],[FinUser] ,[FinDate],[RevDate],[RevUser],[conditions],[ImportDate])  ",
                            "VALUES (",
                             "@Partido,@Lot,@Amount,@Usercode,@AuthDate,@Status,null,null,null,null,null,null,null,@ImportDate",
                            ");"
                        };

                string[] myUpdate_Imported1 =
                    {
                            "Update [TF-Partidos] ",
                            "SET [Imported] = 1 ",
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

                        cmd.Parameters.Add(whereParam);

                        foreach (DataRow row in tableLots.Rows)
                        {
                            // Start a local transaction.
                            transaction = cnn.BeginTransaction(IsolationLevel.ReadCommitted);
                            cmd.Transaction = transaction;

                            string where = row["BatchTrack"].ToString();

                            whereParam.Value = where;

                            int myReturn = 0;

                            cmd.CommandText = string.Concat(myUpdate_Imported1);
                            myReturn = cmd.ExecuteNonQuery();

                            if (cmd.Parameters.Contains(whereParam))
                                cmd.Parameters.Remove(whereParam);


                            cmd.Parameters.Add(partidoParam);
                            cmd.Parameters.Add(lotParam);
                            cmd.Parameters.Add(amountParam);
                            cmd.Parameters.Add(usercodeParam);
                            cmd.Parameters.Add(authDateParam);
                            cmd.Parameters.Add(statusParam);
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
                            amountParam.Value = int.Parse(row["Amount"].ToString());
                            usercodeParam.Value = usercode;
                            authDateParam.Value = DateTime.Now.ToString(); ;
                            statusParam.Value = "0";
                            //verDateParam.Value = VerDate;
                            //verUserParam.Value = VerUser;
                            //finUserParam.Value = FinUser;
                            //finDateParam.Value = FinDate;
                            //revDateParam.Value = RevDate;
                            //revUserParam.Value = RevUser;
                            //conditionsParam.Value = conditions;
                            importDateParam.Value = DateTime.Now.ToString();

                            cmd.CommandText = string.Concat(myInsert);
                            myReturn = cmd.ExecuteNonQuery();

                            cmd.Parameters.Remove(partidoParam);
                            cmd.Parameters.Remove(lotParam);
                            cmd.Parameters.Remove(amountParam);
                            cmd.Parameters.Remove(usercodeParam);
                            cmd.Parameters.Remove(authDateParam);
                            cmd.Parameters.Remove(statusParam);
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

        public bool MyChangeCandidatos(bool isInsert, string partido, string numCand, string nombre, string area, string cargo, string endoReq, string where)
        {
            bool myBoolReturn = false;
            try
            {
                string[] myInsert =
                        {
                            "INSERT INTO [dbo].[Candidatos] ([Partido],[NumCand],[Nombre],[Area],[Cargo],[EndoReq]) ",
                            "VALUES (@Partido,@NumCand,@Nombre,@Area,@Cargo,@EndoReq)"
                        };

                string[] myUpdate =
                    {
                            "UPDATE [dbo].[Candidatos] ",
                            "SET [Partido] = @Partido, ",
                            "[NumCand] = @NumCand,",
                            "[Nombre] = @Nombre,",
                            "[Area] = @Area,",
                            "[Cargo] =@Cargo,",
                            "[EndoReq]=@EndoReq ",
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

                        if (!isInsert)
                            cmd.Parameters.Add(new SqlParameter("@Where", SqlDbType.VarChar));

                        cmd.Parameters["@Partido"].Value = partido;
                        cmd.Parameters["@NumCand"].Value = numCand;
                        cmd.Parameters["@Nombre"].Value = nombre;
                        cmd.Parameters["@Area"].Value = area;
                        cmd.Parameters["@Cargo"].Value = cargo;
                        cmd.Parameters["@EndoReq"].Value = endoReq;

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

        public bool MyProcessLot(string numlot, string usercode, ObservableCollection<Criterios> CollCriterios, ObservableCollection<string> lblNReasons, ObservableCollection<int> ProgressBar_Value, ObservableCollection<int> ProgressBar_Maximum, ObservableCollection<string> Resultados)
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
            SqlTransaction transaction = null;
            DataTable myDataToProcessTF = new DataTable();

            //'Define memory variables to hold information from access DB
            //'and modify them if needed during the validation process
            String m_BatchTrack = string.Empty;
            String m_BatchNo = string.Empty;
            int m_BatchPgNo = 0;
            String m_N_ELEC = string.Empty;
            String m_N_PRECINTO = string.Empty;
            DateTime? m_FECHA_N = null;
            String m_FECHA_N2 = string.Empty;
            String m_SEXO = string.Empty;
            String m_PARTIDO = string.Empty;
            int m_Cargo = 0;
            String m_N_CANDIDAT = string.Empty;
            String m_N_NOTARIO = string.Empty;
            DateTime? m_Fecha_Endo = null;
            String m_Suspense_File = string.Empty;
            String m_Batch = string.Empty;
            String m_Firma_Peticionario = string.Empty;
            String m_Firma_Notario = string.Empty;
            int m_Firma_Pet_Inv = 0;
            int m_Firma_Not_Inv = 0;
            string m_Padre = string.Empty;
            string m_Madre = string.Empty;
            string m_Leer_Inv = string.Empty;
            string m_Alteracion = string.Empty;
            DateTime? m_Firma_Fecha = null;

            try
            {
                string[] mySqlstrTF = { "Select * ",
                                        "from [dbo].[TF-Partidos] ",
                                        "Where Imported = 2 and BatchTrack=@lot ",
                                        "Order By Partido,BatchTrack, BatchNo, BatchPgNo;" };

                string[] mySqlstrTFCount = { "Select count(*) ",
                                        "from [dbo].[TF-Partidos] ",
                                        "Where Imported = 2 and BatchTrack=@lot;" };


                string[] mySqlstrLotsCount = { "Select count(*) ",
                                          "from [dbo].[Lots] ",
                                          "Where Status = 0 and Lot=@lot; " };

                /* BORRA LOS ERRORES ANTERIORES DEL LOTE */
                string[] mySqlStrDeleteLotsVoid ={"DELETE FROM LotsVoid ",
                                                  "WHERE Lot =@lot" };

                string[] mySqlStrDeleteLotsEndo ={"DELETE FROM LotsEndo ",
                                                  "WHERE Lot =@lot" };


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
                transaction = myCnnDBEndosos.BeginTransaction(IsolationLevel.ReadCommitted);

                myCmdDBEndosos.Transaction = transaction;

                myCmdDBEndosos.CommandText = string.Concat(mySqlStrDeleteLotsVoid);
                myCmdDBEndosos.ExecuteNonQuery();

                myCmdDBEndosos.CommandText = string.Concat(mySqlStrDeleteLotsEndo);
                myCmdDBEndosos.ExecuteNonQuery();
                transaction.Commit();

                int[] Rechazo = new int[20];
                int[] Warning = new int[20];
                bool isRechazo = false;
                Resultados[1] = ProgressBar_Maximum[0].ToString();         //lblTota                  1



                foreach (DataRow row in myDataToProcessTF.Rows)//processing
                {
                    ProgressBar_Value[0]++;

                    Resultados[2] = ProgressBar_Value[0].ToString();//lblProcesadas            2    

                    Rechazo[19] = 0;

                    transaction = myCnnDBEndosos.BeginTransaction(IsolationLevel.ReadCommitted);
                    myCmdDBEndosos.Transaction = transaction;


                    //default
                    isRechazo = false;
                    m_Firma_Peticionario = "0";
                    m_Firma_Notario = "0";
                    m_BatchTrack = string.Empty;
                    m_N_ELEC = string.Empty;
                    m_N_NOTARIO = string.Empty;
                    m_N_CANDIDAT = string.Empty;
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
                    m_Leer_Inv = string.Empty;
                    m_Alteracion = string.Empty;
                    m_Firma_Fecha = null;
                    //



                    string tmpFecha_Endo = string.Concat(row["FechaEndo_Mes"].ToString().Trim().PadLeft(2, '0'), row["FechaEndo_Dia"].ToString().Trim().PadLeft(2, '0'), row["FechaEndo_Ano"].ToString().Trim().PadLeft(4, '0'));
                    string tmpmFirma_Fecha = string.Concat(row["FechaFirm_Mes"].ToString().Trim().PadLeft(2, '0'), row["FechaFirm_Dia"].ToString().Trim().PadLeft(2, '0'), row["FechaFirm_Ano"].ToString().Trim().PadLeft(4, '0'));

                    try { int.TryParse(row["BatchPgNo"].ToString().Trim(), out m_BatchPgNo); } catch { };
                    try { int.TryParse(row["FirmaElec_Inv"].ToString().Trim(), out m_Firma_Pet_Inv); } catch { };
                    try { int.TryParse(row["FirmaNot_Inv"].ToString().Trim(), out m_Firma_Not_Inv); } catch { };
                    try { int.TryParse(row["Cargo"].ToString().Trim(), out m_Cargo); } catch { }

                    m_Padre = row["Padre"].ToString().Trim();
                    m_Madre = row["Madre"].ToString().Trim();
                    m_Leer_Inv = row["Leer_Inv"].ToString().Trim();
                    m_Alteracion = row["Alteracion"].ToString().Trim();

                    m_PARTIDO = row["Partido"].ToString().Trim();
                    m_BatchNo = row["BatchNo"].ToString().Trim();
                    m_Batch = row["Suspense_File"].ToString().Trim().Split('\\')[3];
                    m_Suspense_File = row["Suspense_File"].ToString().Trim().Split('\\')[4];

                    m_Firma_Peticionario = row["FirmaElector"].ToString().Trim();
                    m_Firma_Notario = row["FirmaNotario"].ToString().Trim();
                    m_BatchTrack = row["BatchTrack"].ToString().Trim();
                    m_N_ELEC = row["NumElec"].ToString().Trim().PadLeft(7, '0');

                    m_N_NOTARIO = row["Notario"].ToString().Trim().PadLeft(7, '0');
                    m_N_CANDIDAT = row["N_CANDIDAT"].ToString().PadLeft(3, '0');
                    m_N_PRECINTO = row["Precinto"].ToString().PadLeft(3, '0');

                    m_SEXO = row["SEXO"].ToString().Trim();

                    string tmpFECHA_N = string.Concat(row["FechaNac_Mes"].ToString().Trim().PadLeft(2, '0'), row["FechaNac_Dia"].ToString().Trim().PadLeft(2, '0'), row["FechaNac_Ano"].ToString().Trim().PadLeft(4, '0'));

                    if (tmpFECHA_N.Trim().Length > 0)
                    {
                        m_FECHA_N = DateTimeUtil.MyValidarFecha(tmpFECHA_N);
                    }

                    if (tmpmFirma_Fecha.Trim().Length > 0)
                    {
                        m_Firma_Fecha = DateTimeUtil.MyValidarFecha(tmpmFirma_Fecha);
                    }

                    if (tmpFecha_Endo.Length < 8)
                        m_Fecha_Endo = null;
                    else
                    {
                        m_Fecha_Endo = DateTimeUtil.MyValidarFecha(tmpFecha_Endo);
                    }

                    if ( ((CollCriterios[0].Editar == true) || (CollCriterios[0].Warning == true))  && (m_Firma_Peticionario == "0"))//'1-ELECTOR NO FIRMO EL ENDOSO
                    {
                        if (CollCriterios[0].Editar == true)
                        {
                            Rechazo[0]++; isRechazo = true;
                        }
                        if (CollCriterios[0].Warning == true)
                            Warning[0]++;
                    }

                    if ( ((CollCriterios[1].Editar == true) || (CollCriterios[1].Warning ==true)) && m_Firma_Notario == "0")//2-'NOTARIO NO FIRMO EL ENDOSO
                    {
                        if (CollCriterios[1].Editar == true)
                        {
                            Rechazo[1]++; isRechazo = true;
                        }

                            if (CollCriterios[1].Warning == true)
                            Warning[1]++;
                    }

                    if (CollCriterios[2].Editar == true || CollCriterios[2].Warning == true)//'3-FECHA DEL ENDOSO FUERA DE TIEMPO
                    {
                        string[] sqlstr = { "SELECT Entregado ",
                                                "From [LotsReceive] ",
                                                " Where [Lot] = '",  m_BatchTrack , "'"};

                        object fechaEntregaEndosos = null;

                        if (MyValidarDatos(string.Concat(sqlstr), out fechaEntregaEndosos, myCnnDBEndososValidarDatos) == null)
                        {
                            if (CollCriterios[2].Editar == true)
                            {
                                Rechazo[2]++; isRechazo = true;
                            }
                            if (CollCriterios[2].Warning == true)
                                Warning[2]++;
                        }
                        else
                        {
                            if (m_Fecha_Endo != null)
                            {
                                if (DateTimeUtil.DateDiff(DateInterval.Day, (DateTime)fechaEntregaEndosos, m_Fecha_Endo) > 7)
                                {
                                    if (CollCriterios[2].Editar == true)
                                    {
                                        Rechazo[2]++;
                                        isRechazo = true;
                                    }

                                    if (CollCriterios[2].Warning == true)
                                        Warning[2]++;
                                }
                            }
                            else
                            {
                                if (CollCriterios[2].Editar == true)
                                    Rechazo[2]++;

                                if (CollCriterios[2].Warning == true)
                                    Warning[2]++;
                            }

                        }

                    }

                    if (CollCriterios[3].Editar == true || CollCriterios[3].Warning == true)//'4-NOTARIO NO EXISTE EN NUESTROS ARCHIVOS
                    {
                        string[] sqlstr = { "SELECT count(*) ",
                                                "From [Notarios] ",
                                                " Where [VoterIDNotario] = '", FixNum( m_N_NOTARIO) , "'"};
                        object total = null;

                        if (MyValidarDatos(string.Concat(sqlstr), out total, myCnnDBEndososValidarDatos) != null)
                        {
                            if ((int)total == 0)
                            {
                                if (CollCriterios[3].Editar == true)
                                    Rechazo[3]++;

                                if (CollCriterios[3].Warning == true)
                                    Warning[3]++;

                            }
                        }
                        else
                        {
                            if (CollCriterios[3].Editar == true)
                                Rechazo[3]++;

                            if (CollCriterios[3].Warning == true)
                                Warning[3]++;
                        }
                    }

                    if (CollCriterios[4].Editar == true || CollCriterios[4].Warning==true)// '5-ELECTOR IGUAL AL NOTARIO
                    {
                        if (m_N_ELEC == m_N_NOTARIO)
                        {
                            if (CollCriterios[4].Editar == true)
                                Rechazo[4]++;

                            if (CollCriterios[4].Warning == true)
                                Warning[4]++;
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
                                    Rechazo[5]++;

                                if (CollCriterios[5].Warning == true)
                                    Warning[5]++;
                            }
                        }
                        else
                        {
                            if (CollCriterios[5].Editar == true)
                                Rechazo[5]++;

                            if (CollCriterios[5].Warning == true)
                                Warning[5]++;
                        }

                    }

                    if (CollCriterios[6].Editar == true || CollCriterios[6].Warning == true)// '7-ASPIRANTE NO EXISTE
                    {
                        string[] sqlstr = { "SELECT count(*) ",
                                                "From [Aspirantes]  ",
                                                " Where [NumCandidato] = '", FixNum( m_N_CANDIDAT ), "'"};
                        object total = null;

                        if (MyValidarDatos(string.Concat(sqlstr), out total, myCnnRadicacionesCEE) != null)
                        {
                            if ((int)total == 0)
                            {
                                if (CollCriterios[6].Editar == true)
                                    Rechazo[6]++;

                                if (CollCriterios[6].Warning == true)
                                    Warning[6]++;
                            }
                        }
                        else
                        {
                            if (CollCriterios[6].Editar == true)
                                Rechazo[6]++;

                            if (CollCriterios[6].Warning == true)
                                Warning[6]++;
                        }

                    }

                    if (CollCriterios[7].Editar == true || CollCriterios[7].Warning==true)//'8-NOTARIO INACTIVO
                    {
                        string[] sqlstr = { "SELECT status ",
                                                "From [usercid].[Citizen] ",
                                                " Where [CitizenID] = '",FixNum(m_N_NOTARIO) , "'"};
                        object status = null;

                        if (MyValidarDatos(string.Concat(sqlstr), out status, myCnnDBCeeMaster) == null)
                        {
                            if (CollCriterios[7].Editar == true)
                                Rechazo[7]++;

                            if (CollCriterios[7].Warning == true)
                                Warning[7]++;
                        }
                        else
                        {
                            if (status != null)
                            {
                                if (!MyStatus(status.ToString()))
                                {
                                    if (CollCriterios[7].Editar == true)
                                        Rechazo[7]++;

                                    if (CollCriterios[7].Warning == true)
                                        Warning[7]++;
                                }
                            }
                            else
                            {
                                if (CollCriterios[7].Editar == true)
                                    Rechazo[7]++;

                                if (CollCriterios[7].Warning == true)
                                    Warning[7]++;
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
                                Rechazo[8]++;

                            if (CollCriterios[8].Warning == true)
                                Warning[8]++;
                        }
                        else
                        {
                            if (DateTimeUtil.DateDiff(DateInterval.Day, (DateTime)DateOfBirth, m_FECHA_N) > 0)
                            {
                                if (CollCriterios[8].Editar == true)
                                    Rechazo[8]++;

                                if (CollCriterios[8].Warning == true)
                                    Warning[8]++;
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
                                Rechazo[9]++;

                            if (CollCriterios[9].Warning == true)
                                Warning[9]++;
                        }
                        else
                        {
                            if (Gender.ToString() != m_SEXO)
                            {
                                if (CollCriterios[9].Editar == true)
                                    Rechazo[9]++;

                                if (CollCriterios[9].Warning == true)
                                    Warning[9]++;
                            }
                        }
                    }

                    if (CollCriterios[10].Editar == true || CollCriterios[10].Warning==true)// '11-STATUS ELECTO EXCLUIDO
                    {
                        string[] sqlstr = { "SELECT status ",
                                                "From [usercid].[Citizen] ",
                                                " Where [CitizenID] = '",  FixNum(m_N_ELEC) , "'"};
                        object status = null;

                        if (MyValidarDatos(string.Concat(sqlstr), out status, myCnnDBCeeMaster) == null)
                        {
                            if (CollCriterios[10].Editar == true)
                                Rechazo[10]++;

                            if (CollCriterios[10].Warning == true)
                                Warning[10]++;
                        }
                        else
                        {
                            if (status != null)
                            {
                                if (!MyStatus(status.ToString()))
                                {
                                    if (CollCriterios[10].Editar == true)
                                        Rechazo[10]++;

                                    if (CollCriterios[10].Warning == true)
                                        Warning[10]++;
                                }
                            }
                            else
                            {
                                if (CollCriterios[10].Editar == true)
                                    Rechazo[10]++;

                                if (CollCriterios[10].Warning == true)
                                    Warning[10]++;
                            }
                        }

                    }

                    // 'SOLO PARA SENADOR DISTRITO, REPRESENTANTE DISTRITO, ALCALDE, ASMBLEISTA MUNICIPAL
                    if (m_Cargo == 3 || m_Cargo == 5 || m_Cargo == 7 || m_Cargo == 8)
                    {
                        if (CollCriterios[11].Editar == true || CollCriterios[11].Warning == true)// 12-'PRECINTO NO CONCUERDA
                        {
                            string[] sqlstr = { "SELECT FirstGeoCode ",
                                                "From [usercid].[Citizen] ",
                                                " Where [CitizenID] = '", FixNum( m_N_ELEC) , "'"};
                            object FirstGeoCode = null;

                            if (MyValidarDatos(string.Concat(sqlstr), out FirstGeoCode, myCnnDBCeeMaster) == null)
                            {
                                if (CollCriterios[11].Editar == true)
                                        Rechazo[11]++;

                                if (CollCriterios[11].Warning == true)
                                    Warning[11]++;
                            }
                            else
                            {
                                if (FirstGeoCode != null)
                                {
                                    string precinto = FirstGeoCode.ToString().Trim().PadLeft(3, '0');
                                    if (precinto != m_N_PRECINTO)
                                    {
                                        if (CollCriterios[11].Editar == true)
                                            Rechazo[11]++;

                                        if (CollCriterios[11].Warning == true)
                                            Warning[11]++;
                                    }
                                }
                                else
                                {
                                    if (CollCriterios[11].Editar == true)
                                        Rechazo[11]++;

                                    if (CollCriterios[11].Warning == true)
                                        Warning[11]++;
                                }
                            }
                        }

                        if (CollCriterios[12].Editar == true || CollCriterios[12].Warning ==true)//13-'PRECINTO ELECTOR DISTINTO AL DEL CANDIDATO
                        {
                            string[] sqlstr = { "SELECT [NumPrecinto] ",
                                                "From Aspirantes ",
                                                " Where [NumCandidato] = '", FixNum(m_N_CANDIDAT) , "'"};
                            object NumPrecinto = null;

                            if (MyValidarDatos(string.Concat(sqlstr), out NumPrecinto, myCnnRadicacionesCEE) == null)
                            {
                                if (CollCriterios[12].Editar == true)
                                    Rechazo[12]++;

                                if (CollCriterios[12].Warning == true)
                                    Warning[12]++;

                            }
                            else
                            {
                                if (NumPrecinto != null)
                                {
                                    string precinto = NumPrecinto.ToString().Trim().PadLeft(3, '0');
                                    if (precinto != m_N_PRECINTO)
                                    {
                                        if (CollCriterios[12].Editar == true)
                                            Rechazo[12]++;

                                        if (CollCriterios[12].Warning == true)
                                            Warning[12]++;
                                    }
                                }
                                else
                                {
                                    if (CollCriterios[12].Editar == true)
                                        Rechazo[12]++;

                                    if (CollCriterios[12].Warning == true)
                                        Warning[12]++;
                                }
                            }
                        }
                    }

                    if (CollCriterios[13].Editar == true || CollCriterios[13].Warning == true)// 14-'MULTIPLES ENDOSOS PARA EL MISMO CANDIDATO
                    {
                        string[] sqlstr = { "SELECT count(*) ",
                                                "From [LotsEndo]  ",
                                                "Where [Cargo] = '",  m_Cargo.ToString() , "' ",
                                                "and ",
                                                "Candidato='",m_N_CANDIDAT,"' And Status = 0"};
                        object total = null;
                        if (MyValidarDatos(string.Concat(sqlstr), out total, myCnnDBEndososValidarDatos) != null)//>0
                        {
                            if ((int)total > 0)
                            {
                                if (CollCriterios[13].Editar == true)
                                    Rechazo[13]++;

                                if (CollCriterios[13].Warning == true)
                                    Warning[13]++;
                            }
                        }
                    }

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
                                case 1:// 'Gobernador
                                    {
                                        if ((int)total > 1)
                                        {
                                            if (CollCriterios[14].Editar == true)
                                                Rechazo[14]++;
                                            if (CollCriterios[14].Warning == true)
                                                Warning[14]++;
                                        }
                                    }
                                    break;

                                case 2://'Comisionado Residente
                                    {
                                        if ((int)total > 1)
                                        {
                                            if (CollCriterios[14].Editar == true)
                                                Rechazo[14]++;
                                            if (CollCriterios[14].Warning == true)
                                                Warning[14]++;
                                        }
                                    }
                                    break;

                                case 3: //'Senador Distrito
                                    {
                                        if ((int)total > 2)
                                        {
                                            if (CollCriterios[14].Editar == true)
                                                Rechazo[14]++;
                                            if (CollCriterios[14].Warning == true)
                                                Warning[14]++;
                                        }
                                    }
                                    break;

                                case 4://'Senador Acumulación
                                    {
                                        if ((int)total > 6)
                                        {
                                            if (CollCriterios[14].Editar == true)
                                                Rechazo[14]++;
                                            if (CollCriterios[14].Warning == true)
                                                Warning[14]++;
                                        }
                                    }
                                    break;

                                case 5: //'Representante Distrito
                                    {
                                        if ((int)total > 1)
                                        {
                                            if (CollCriterios[14].Editar == true)
                                                Rechazo[14]++;
                                            if (CollCriterios[14].Warning == true)
                                                Warning[14]++;
                                        }
                                    }
                                    break;

                                case 6: //'Representante Acumulación
                                    {
                                        if ((int)total > 6)
                                        {
                                            if (CollCriterios[14].Editar == true)
                                                Rechazo[14]++;
                                            if (CollCriterios[14].Warning == true)
                                                Warning[14]++;
                                        }
                                    }
                                    break;

                                case 7: //'Alcalde
                                    {
                                        if ((int)total > 1)
                                        {
                                            if (CollCriterios[14].Editar == true)
                                                Rechazo[14]++;
                                            if (CollCriterios[14].Warning == true)
                                                Warning[14]++;
                                        }
                                    }
                                    break;

                                case 8: //'Asambleista
                                    {
                                        if ((int)total > 3)
                                        {
                                            if (CollCriterios[14].Editar == true)
                                                Rechazo[14]++;
                                            if (CollCriterios[14].Warning == true)
                                                Warning[14]++;
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
                                Rechazo[15]++;

                            if (CollCriterios[15].Warning == true)
                                Warning[15]++;
                        }
                    }

                    if (CollCriterios[16].Editar == true || CollCriterios[16].Warning == true)// 'FIRMA DEL NOTARIO NO ES IGUAL A LA DEL ARCHIVO MAESTRO
                    {
                        if (m_Firma_Not_Inv == 1)
                        {
                            if (CollCriterios[16].Editar == true)
                                Rechazo[16]++;
                            if (CollCriterios[16].Warning == true)
                                Warning[16]++;
                        }

                    }
                     

                    //'ACTUALIZA LOS COUNTERS DE LA PANTALLA
                    for (int X = 0; X < CollCriterios.Count; X++) 
                    {
                        bool writeWarning = true;

                        if (Rechazo[X] > 0)
                        {
                          
                            lblNReasons[X] = Rechazo[X].ToString();
                            Rechazo[19]++;

                            //'ESCRIBE LOS ERRORES A LA TABLA LOTSVOID
                            WriteVoid(m_BatchTrack, m_Batch, m_BatchPgNo, m_N_ELEC, X, m_PARTIDO, 0, myCmdDBEndosos);
                            writeWarning = false;
                        }
                        if (Warning[X] > 0 && writeWarning)
                        {
                            lblNReasons[X] = Warning[X].ToString();
                            Warning[19]++;
                            //'ESCRIBE LOS ERRORES A LA TABLA LOTSVOID (Warning)
                            WriteVoid(m_BatchTrack, m_Batch, m_BatchPgNo, m_N_ELEC, X, m_PARTIDO, 2, myCmdDBEndosos);
                        }
                    }

                   

                    if (Rechazo[19] > 0 )
                    {
                        int Rechazadas = int.Parse(Resultados[4]);
                        Rechazadas++;
                        Resultados[4] = Rechazadas.ToString();//lblRechazadas            4
                    }
                    else
                    {
                        int Aprobadas = int.Parse(Resultados[3]);
                        Aprobadas++;
                        Resultados[3] = Aprobadas.ToString();//lblAprobadas             3  
                    }

                    if (Warning[19] >0)
                    {
                        int Warnings = int.Parse(Resultados[5]);
                        Warnings++;
                        Resultados[5]   = Warnings.ToString();//lblWarnings              5
                    }

                    //  'ESCRIBE LOS ENDOSOS A LA TABLA LOTSENDO

                    string[] mySqlStrLOTSENDO =
                    {
                        "Insert Into LotsEndo",
                        "([Partido],[Lot],[Batch],[Formulario],[Candidato],[Cargo],[NumElec],[Padre],[Madre],[FechaNac],[Leer_Inv],[Alteracion],[Notario]",
                           ",[Firma_Peticionario],[Firma_Pet_Inv],[Firma_Notario],[Firma_Not_Inv],[Fecha_Endoso],[Image],[Status],[Firma_Fecha],[SEXO],[PRECINTO] ) ",

                        "Values('" , m_PARTIDO , "'",           //Partido
                        ",'" , m_BatchTrack , "'",              //Lot
                        ",'" , m_Batch , "'",                   //Batch
                        "," , m_BatchPgNo.ToString(),           //Formulario
                        ",'" , m_N_CANDIDAT , "'",              //Candidato
                        "," , m_Cargo.ToString(),               //Cargo
                        ",'" , m_N_ELEC , "'",                  //NumElec
                        ",'" ,m_Padre , "'",                    //Padre
                        ",'" ,m_Madre , "'",                    //Madre
                        ",'" , MyFechaToSql( m_FECHA_N ),  "'",      //FechaNac
                        ",'" , m_Leer_Inv , "'",                //Leer_Inv
                        ",'" , m_Alteracion , "'",              //Alteracion
                        ",'" , m_N_NOTARIO , "'",               //Notario
                        ",'" , m_Firma_Peticionario , "'",      //Firma_Peticionario
                        "," , m_Firma_Pet_Inv.ToString(),       //Firma_Pet_Inv
                        ",'" , m_Firma_Notario , "'",           //Firma_Notario
                        "," , m_Firma_Not_Inv.ToString() ,      //Firma_Not_Inv
                        ",'" , MyFechaToSql(m_Fecha_Endo) , "'",   //Fecha_Endoso
                        ",'" , m_Suspense_File , "'",           //Image
                        "," , Rechazo[19] == 0? "0": "1" ,      //Status
                        ",'" , MyFechaToSql( m_Firma_Fecha ), "'",  //Firma_Fecha
                        ",'" , m_SEXO , "'",                    //SEXO
                        ",'" , m_N_PRECINTO , "'",              //PRECINTO
                       ")"

                    };

                    myCmdDBEndosos.CommandText = string.Concat(mySqlStrLOTSENDO);
                    myCmdDBEndosos.ExecuteNonQuery();

                  

                    transaction.Commit();
                    DoEvents();
                }


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
                    throw new Exception(ex.ToString() + "\r\nMyProcessLot Error");
            }
            return myBoolReturn;
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
                } catch
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
                try {
                    returnValue = cmd.ExecuteScalar();
                } catch
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
                    return true;
                case "I":
                case "E":
                    return false;
                default:
                    return false;


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

        private void WriteVoid(string Lot, string BatchNo, int Formulario, string NumElec, int Rechazo, string m_PARTIDO, int Status,SqlCommand dbCmd)
        {
            // 'ESCRIBE EL ENDOSO RECHAZADO
            // 'STATUS DEL RECHAZO
            //'0 - SIN CORREJIR
            //'1 - CORREJIDO
            //'2 - WARNING

            string sqlstr = "Insert Into LotsVoid Values";
            sqlstr = sqlstr + "('" + m_PARTIDO + "'";
            sqlstr = sqlstr + ",'" + Lot + "'";
            sqlstr = sqlstr + ",'" + BatchNo + "'";
            sqlstr = sqlstr + "," + Formulario;
            sqlstr = sqlstr + ",'" + Rechazo + "'";
            sqlstr = sqlstr + ",'" + NumElec + "'";
            sqlstr = sqlstr + ", " + Status + ")";
            dbCmd.CommandText = sqlstr;
            dbCmd.ExecuteNonQuery();

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
