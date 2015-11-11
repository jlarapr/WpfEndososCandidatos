using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using System.Runtime.InteropServices;

namespace jolcode
{
    public class SqlExcuteCommand  : IDisposable
    {
        private IntPtr nativeResource = Marshal.AllocHGlobal(100);
        private string _DBCnnStr;

        public SqlExcuteCommand()
        {

        }

        public string DBCnnStr
        {
            get
            {
                return _DBCnnStr;
            }set
            {
                _DBCnnStr = value;
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
        public DataTable MyGetSelectLotes(string lot,string cantidad)
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

                            cmd.Parameters.Add(new SqlParameter("@lot",SqlDbType.VarChar)).Value = lot;
                            

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
                    throw new Exception(ex.ToString() + "\r\nMyGetAreas Error");
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

                int myCitizenID = 0;

                if (int.TryParse(CitizenID,out myCitizenID))
                {
                    CitizenID = string.Format("{0:0000000}", myCitizenID);
                }



                string mySqlstr = "Select * From TblCitizen Where CitizenID =@CitizenID ";

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


        public bool MyInitLot(string lotNum,Logclass logClass)
        {
            /*
            'STATUS LOTE
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
                                                       string.Concat(myUpDateLotsTF_Partidos),System.Diagnostics.EventLogEntryType.Information,100);

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
        public DataTable MyGetLot()
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

        public DataTable MyGetAreas (bool allColumm)
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
                string mySqlstr = "Select [Desc],PRECINTOS from AREAS where Area='" + area +  "'";

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

        public bool MyDeleteNotario(string where,string where2)
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

        public bool MyChangeArea(bool isInsert,string area,string desc,string precintos,string electivePositionID,string demarcationID,string where)
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
                        CommandText = isInsert == true?string.Concat(myInsert):string.Concat(myUpdate)
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

        public bool MyChangePartidos(bool isInsert, string partido, string desc, string endoReq, string area,   string where)
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
                        cmd.Parameters["@Area"].Value = area.Substring(0,4);

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


        public bool MyChangeCriterios( string Campo, bool? Editar, string Desc, bool? Warning)
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
                        cmd.Parameters["@Editar"].Value = Editar == true?"1":"0";
                        cmd.Parameters["@Desc"].Value = Desc;
                        cmd.Parameters["@Warning"].Value = Warning == true?"1":"0";

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

        public bool MyChangeTF(DataTable tableLots,string usercode)
        {
            bool myBoolReturn = false;
            SqlTransaction transaction = null;
            bool myBoolErrorNoHayLotes = true;
            try
            {

                if (tableLots == null )
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
                    throw new Exception(ex.Message );
                else
                    throw new Exception(ex.ToString() + "\r\nMyChangeTF Error\r\nAn exception of type" + ex.GetType());
            }
            return myBoolReturn;
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
