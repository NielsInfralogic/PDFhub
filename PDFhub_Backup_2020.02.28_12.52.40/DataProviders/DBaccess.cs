using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;

namespace PDFhub.DataProviders
{
    public class DBaccess
    {
        readonly SqlConnection connection;

        public DBaccess()
        {
            connection = new SqlConnection(Utils.ReadConfigString("ConnectionString", ""));
        }

        public void CloseAll()
        {
            if (connection.State == ConnectionState.Open)
                connection.Close();
        }

        public bool ClearServiceErrorState(string serviceName, int instanceNumber, out string errmsg)
        {
            errmsg = "";

            string sql = $"UPDATE ServiceStates SET State=1,LastUpdateTime=GETDATE(),LastErrorMessage='' WHERE ServiceName='{serviceName}' AND InstanceNumber={instanceNumber}";
            SqlCommand command = new SqlCommand(sql, connection)
            {
                CommandType = CommandType.Text
            };

            try
            {
                if (connection.State == ConnectionState.Closed || connection.State == ConnectionState.Broken)
                    connection.Open();

                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                errmsg = ex.Message;
                return false;
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                    connection.Close();
                command.Dispose();
            }
            return true;

        }

        public bool GetServiceStates(ref List<Models.Service> services, out string errmsg)
        {
            errmsg = "";
            services.Clear();

            string sql = "SELECT ServiceName,InstanceNumber,ServiceType,State,LastUpdateTime,LastJob,LastErrorMessage FROM ServiceStates ORDER BY ServiceType ";

            SqlCommand command = new SqlCommand(sql, connection)
            {
                CommandType = CommandType.Text
            };

            SqlDataReader reader = null;

            try
            {
                if (connection.State == ConnectionState.Closed || connection.State == ConnectionState.Broken)
                    connection.Open();

                reader = command.ExecuteReader();

                while (reader.Read())
                {
                    int idx = 0;
                    Models.Service service = new Models.Service()
                    {
                        Name = reader.GetString(idx++),
                        InstanceNumber = reader.GetInt32(idx++),
                        Type = (Models.ServiceType)reader.GetInt32(idx++),
                        State = (Models.ServiceState)reader.GetInt32(idx++),
                        LastEventTime = Utils.Time2String(reader.GetDateTime(idx++)),
                        LastMessage = reader.GetString(idx++),
                        StateImageUrl = "Application"

                    };

                    service.ID = service.Name + "_" + service.InstanceNumber;

                    string lastErrorMessage = reader.GetString(idx++);
                    if (lastErrorMessage != "")
                        service.LastMessage = lastErrorMessage + " " + service.LastMessage;
                    if (service.State == Models.ServiceState.Running)
                        service.StateImageUrl = "Running";
                    else if (service.State == Models.ServiceState.Stopped)
                        service.StateImageUrl = "Stop";

                    service.ViewButton = service.Type != Models.ServiceType.Database && service.Type != Models.ServiceType.FileServer;

                    services.Add(service);
                }
            }
            catch (Exception ex)
            {
                errmsg = ex.Message;
                return false;
            }
            finally
            {
                if (reader != null)
                    reader.Close();
                if (connection.State == ConnectionState.Open)
                    connection.Close();
                command.Dispose();
            }

            return true;

        }

        public bool GetLastLogItems(ref List<Models.LogItem> items, int maxItems, bool errorsOnly, Models.LogItemSource sourceEvents, out string errmsg)
        {
            errmsg = "";
            items.Clear();
            string sql = $"SELECT TOP {maxItems} ServiceName + '_' + CAST(InstanceNumber as varchar(8)),EventTime,Message,ServiceLog.Event,FileName,Source, ISNULL(EventCodes.EventName,'') FROM ServiceLog WITH (NOLOCK) LEFT OUTER JOIN EventCodes  WITH (NOLOCK)  ON EventCodes.EventNumber=ServiceLog.Event ";

            string eventList = "";
            if (sourceEvents == Models.LogItemSource.Input)
                eventList = "6,10";
            else if (sourceEvents == Models.LogItemSource.Import)
                eventList = "996,990,992,999";
            else if (sourceEvents == Models.LogItemSource.Processing)
                eventList = "110,116,117,120,126,127,16";
            else if (sourceEvents == Models.LogItemSource.Export)
                eventList = "180,186";
            else if (sourceEvents == Models.LogItemSource.Maintenance)
                eventList = "610,611,612,613,614,615,616,617,618,619,620";

            if (errorsOnly)
            {
                eventList = "";
                foreach (int n in Constants.ErrorEvents)
                {
                    eventList += eventList != "" ? "," + n.ToString() : n.ToString();
                }
            }
               
            sql +=  "WHERE Event IN (" + eventList + ") ORDER BY EventTime DESC";

            SqlCommand command = new SqlCommand(sql, connection)
            {
                CommandType = CommandType.Text
            };

            SqlDataReader reader = null;

            try
            {
                if (connection.State == ConnectionState.Closed || connection.State == ConnectionState.Broken)
                    connection.Open();

                reader = command.ExecuteReader();

                while (reader.Read())
                {
                    int idx = 0;
                    Models.LogItem item = new Models.LogItem()
                    {
                        Type = Models.LogItemType.ErrorLogItem,
                        Service =  reader.GetString(idx++).Trim(),
                        Time =  reader.GetDateTime(idx++),                      
                        Message = reader.GetString(idx++).Trim(),
                        Status = reader.GetInt32(idx++),
                        FileName = reader.GetString(idx++).Trim(),
                        Source = reader.GetString(idx++).Trim(),
                        StatusName = reader.GetString(idx++).Trim()
                    };

                    if (errorsOnly && item.Source != "")
                        item.Message += " (" + item.Source + ")";

                    items.Add(item);
                }
            }
            catch (Exception ex)
            {
                errmsg = ex.Message;
                return false;
            }
            finally
            {
                if (reader != null)
                    reader.Close();
                if (connection.State == ConnectionState.Open)
                    connection.Close();
                command.Dispose();
            }

            return true;
        }

        private bool HasID(int id, string idFieldName, string tableName, ref bool hasID, out string errmsg)
        {
            errmsg = "";
            hasID = false;
            string sql = $"SELECT TOP 1 {idFieldName} FROM {tableName} WHERE {idFieldName}={id}";

            SqlCommand command = new SqlCommand(sql, connection)
            {
                CommandType = CommandType.Text
            };
            try
            {
                if (connection.State == ConnectionState.Closed || connection.State == ConnectionState.Broken)
                    connection.Open();

                if (command.ExecuteScalar() != null)
                    hasID = true;
            }
            catch (Exception ex)
            {
                errmsg = ex.Message;
                return false;
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                    connection.Close();
                command.Dispose();
            }

            return true;
        }

        private bool HasIDString(string idstring, string idFieldName, string tableName, ref bool hasID, out string errmsg)
        {
            errmsg = "";
            hasID = false;
            string sql = $"SELECT TOP 1 '{idFieldName}' FROM {tableName} WHERE {idFieldName}='{idstring}'";

            SqlCommand command = new SqlCommand(sql, connection)
            {
                CommandType = CommandType.Text
            };
            try
            {
                if (connection.State == ConnectionState.Closed || connection.State == ConnectionState.Broken)
                    connection.Open();

                if (command.ExecuteScalar() != null)
                    hasID = true;
            }
            catch (Exception ex)
            {
                errmsg = ex.Message;
                return false;
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                    connection.Close();
                command.Dispose();
            }

            return true;
        }

        private int GenerateNextID(string idFieldName, string tableName, out string errmsg)
        {
            errmsg = "";
            int ret = 0;
            string sql = $"SELECT ISNULL(MAX({idFieldName}),0)+1 FROM {tableName}";

            SqlCommand command = new SqlCommand(sql, connection)
            {
                CommandType = CommandType.Text
            };
            try
            {
                if (connection.State == ConnectionState.Closed || connection.State == ConnectionState.Broken)
                    connection.Open();

                ret = (int)command.ExecuteScalar();
            }
            catch (Exception ex)
            {
                errmsg = ex.Message;
                return 0;
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                    connection.Close();
                command.Dispose();
            }

            return ret;
        }

        #region Regular Expressions

        public bool DeleteExpressions(bool channelRegex,int inputID, out string errmsg)
        {
            errmsg = "";
            string sql;
            if (channelRegex)
                sql = $"DELETE FROM ChannelRegularExpressions WHERE ChannelID={inputID}";
            else
                sql = $"DELETE FROM RegularExpressions WHERE InputID={inputID}";

            SqlCommand command = new SqlCommand(sql, connection)
            {
                CommandType = CommandType.Text
            };

            try
            {
                if (connection.State == ConnectionState.Closed || connection.State == ConnectionState.Broken)
                    connection.Open();

                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                errmsg = ex.Message;
                return false;
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                    connection.Close();
                command.Dispose();
            }
            return true;
        }

        public bool InsertExpression(bool channelRegex, int inputID, Models.RegExpression regExp, out string errmsg)
        {
            errmsg = "";
            string sql;
            if (channelRegex)
                sql = $"INSERT INTO ChannelRegularExpressions (ChannelID,UseExpression,Rank,MatchExpression,FormatExpression,PartialMatch,Comment) VALUES ({inputID},1,{regExp.Rank},'{regExp.MatchExpression}','{regExp.FormatExpression}',0,'{regExp.Comment}')";
            else
                sql = $"INSERT INTO RegularExpressions (InputID,UseExpression,Rank,MatchExpression,FormatExpression,PartialMatch,Comment) VALUES ({inputID},1,{regExp.Rank},'{regExp.MatchExpression}','{regExp.FormatExpression}',0,'{regExp.Comment}')";

            SqlCommand command = new SqlCommand(sql, connection)
            {
                CommandType = CommandType.Text
            };

            try
            {
                if (connection.State == ConnectionState.Closed || connection.State == ConnectionState.Broken)
                    connection.Open();

                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                errmsg = ex.Message;
                return false;
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                    connection.Close();
                command.Dispose();
            }
            return true;
        }

        public bool GetRegularExpressions(bool channelRegex, int inputID, ref List<Models.RegExpression> regularExpressions, out string errmsg)
        {
            errmsg = "";
            regularExpressions.Clear();

            string sql;
            if (channelRegex)
                sql = $"SELECT Rank, MatchExpression,FormatExpression, Comment FROM ChannelRegularExpressions WHERE ChannelID={inputID} ORDER BY Rank";
            else
                sql = $"SELECT Rank, MatchExpression,FormatExpression, Comment FROM RegularExpressions WHERE InputID={inputID} ORDER BY Rank";

            SqlCommand command = new SqlCommand(sql, connection)
            {
                CommandType = CommandType.Text
            };

            SqlDataReader reader = null;

            try
            {
                if (connection.State == ConnectionState.Closed || connection.State == ConnectionState.Broken)
                    connection.Open();

                reader = command.ExecuteReader();

                while (reader.Read())
                {
                    int idx = 0;
                    regularExpressions.Add(new Models.RegExpression()
                    {
                        Rank = reader.GetInt32(idx++),
                        MatchExpression = reader.GetString(idx++).Trim(),
                        FormatExpression = reader.GetString(idx++).Trim(),
                        Comment = reader.GetString(idx++).Trim()
                    });
                }
            }
            catch (Exception ex)
            {
                errmsg = ex.Message;
                return false;
            }
            finally
            {
                if (reader != null)
                    reader.Close();
                if (connection.State == ConnectionState.Open)
                    connection.Close();
                command.Dispose();
            }
            return true;
        }

        #endregion

        #region Inputconfigurations

        public bool GetInputConfiguration(ref Models.InputConfiguration inputConfiguration, int inputID, out string errmsg)
        {
            List<Models.InputConfiguration> inputConfigurations = new List<Models.InputConfiguration>();
            if (GetInputConfigurations(ref inputConfigurations, inputID, out errmsg) == false)
                return false;

            if (inputConfigurations.Count > 0)
                inputConfiguration = inputConfigurations[0];

            return true;
        }

        public bool GetInputConfigurations(ref List<Models.InputConfiguration> inputConfigurations, out string errmsg)
        {
            return GetInputConfigurations(ref inputConfigurations, 0, out errmsg);
        }

        public bool GetInputConfigurations(ref List<Models.InputConfiguration> inputConfigurations, int inputID, out string errmsg)
        {
            errmsg = "";
            inputConfigurations.Clear();
            string sql = "SELECT InputID,InputName,Enabled,InputPath,SearchMask,StableTime,PollTime,UseRegExp,NamingMask,FTPdownload,FTPserver,FTPusername,FTPpassword,FTPfolder,FTPport,UseCurrentUser,UserName,PassWord,CallScript,ScriptName,Separators,UseRegExp,ISNULL(FTPPasv,1),ISNULL(FTPXCRC,1),ISNULL(FTPTLS,0),MakeCopy,CopyFolder,ConfigChangeTime,SendAckFile,AckFileFolder,AckFlagValue FROM [InputConfigurations] ";
            if (inputID > 0)
                sql += $"WHERE InputID={inputID} ";

            sql += " ORDER BY InputID";

            SqlCommand command = new SqlCommand(sql, connection)
            {
                CommandType = CommandType.Text
            };

            SqlDataReader reader = null;

            try
            {
                if (connection.State == ConnectionState.Closed || connection.State == ConnectionState.Broken)
                    connection.Open();

                reader = command.ExecuteReader();

                while (reader.Read())
                {
                    int idx = 0;
                    inputConfigurations.Add(new Models.InputConfiguration()
                    {
                        InputID = reader.GetInt32(idx++),
                        InputName = reader.GetString(idx++).Trim(),
                        Enabled = reader.GetInt32(idx++)>0,
                        InputPath = reader.GetString(idx++).Trim(),
                        SearchMask = reader.GetString(idx++).Trim(),
                        StableTime = reader.GetInt32(idx++),
                        PollTime = reader.GetInt32(idx++),
                        UseRegExp = reader.GetInt32(idx++) > 0,
                        NamingMask = reader.GetString(idx++).Trim(),
                        InputType = (Models.InputType)reader.GetInt32(idx++),  //FTPdownload field                   
                        FTPserver =  reader.GetString(idx++).Trim(),
                        FTPusername = reader.GetString(idx++).Trim(),
                        FTPpassword = reader.GetString(idx++).Trim(),
                        FTPfolder = reader.GetString(idx++).Trim(),
                        FTPport = reader.GetInt32(idx++),
                        UseSpecificUser = reader.GetInt32(idx++) == 0,
                        UserName = reader.GetString(idx++).Trim(),
                        Password = reader.GetString(idx++).Trim(),
                        CallScript = reader.GetInt32(idx++) > 0,
                        ScriptName = reader.GetString(idx++).Trim(),
                        Separators = reader.GetString(idx++).Trim(),
                        UseRegex = reader.GetInt32(idx++) >0,
                        FTPpasw = reader.GetInt32(idx++) > 0,
                        FTPxcrc = reader.GetInt32(idx++) > 0,
                        FTPtls = (Models.EncryptionType)reader.GetInt32(idx++),
                        MakeCopy = reader.GetInt32(idx++) > 0,
                        CopyFolder = reader.GetString(idx++).Trim(),
                        ConfigChangeTime = reader.GetDateTime(idx++),
                        SendAckFile = reader.GetInt32(idx++) > 0,
                        AckFileFolder = reader.GetString(idx++).Trim(),
                        AckFlagValue = reader.GetInt32(idx++)
                    });
                }
            }
            catch (Exception ex)
            {
                errmsg = ex.Message;
                return false;
            }
            finally
            {
                if (reader != null)
                    reader.Close();
                if (connection.State == ConnectionState.Open)
                    connection.Close();
                command.Dispose();
            }

            foreach(Models.InputConfiguration inputConfiguration in inputConfigurations)
            {
                if (GetRegularExpressions(false,inputConfiguration.InputID, ref inputConfiguration.RegularExpressions, out errmsg) == false)
                    return false;
            }

            return true;
        }

        public bool InsertUpdateInputConfiguration(Models.InputConfiguration inp, out string errmsg)
        {
         
            bool hasID = false;
            string sql;
            if (HasID(inp.InputID, "InputID", "InputConfigurations", ref hasID, out errmsg) == false)
                return false;

            if (hasID)
                sql = $"UPDATE InputConfigurations SET InputName='{inp.InputName}',Enabled={(inp.Enabled?1:0)},InputPath='{inp.InputPath}',SearchMask='{inp.SearchMask}',StableTime={inp.StableTime},PollTime={inp.PollTime},UseRegExp={(inp.UseRegExp ? 1 : 0)},NamingMask='{inp.NamingMask}',FTPdownload={(int)inp.InputType},FTPserver='{inp.FTPserver}',FTPusername='{inp.FTPusername}',FTPpassword='{inp.FTPpassword}',FTPfolder='{inp.FTPfolder}',FTPport={inp.FTPport}, UseCurrentUser={(inp.UseSpecificUser ? 0 : 1)},UserName='{inp.UserName}',Password='{inp.Password}',CallScript={(inp.CallScript ? 1 : 0)},ScriptName='{inp.ScriptName}',Separators='{inp.Separators}',FTPPasv={(inp.FTPpasw?1:0)},FTPXCRC={(inp.FTPxcrc?1:0)},FTPTLS={((int)inp.FTPtls)},MakeCopy={(inp.MakeCopy?1:0)},CopyFolder='{inp.CopyFolder}',ConfigChangeTime=GETDATE(),SendAckFile={(inp.SendAckFile ? 1 : 0)},AckFileFolder='{inp.AckFileFolder}',AckFlagValue={inp.AckFlagValue} WHERE InputID={inp.InputID}";
            else
            {
                inp.InputID = GenerateNextID("InputID", "InputConfigurations", out errmsg);
                if (inp.InputID <= 0)
                    return false;
                sql = $"INSERT INTO InputConfigurations (InputID,InputName,Enabled,LocationID,InputPath,SearchMask,StableTime,PollTime,UseRegExp,NamingMask,FTPdownload,FTPserver,FTPusername,FTPpassword,FTPfolder,FTPport,UseCurrentUser,UserName,Password,CallScript,ScriptName,Separators,FTPpasv,FTPXCRC,FTPTLS,MakeCopy,CopyFolder,ConfigChangeTime,SendAckFile,AckFileFolder,AckFlagValue) VALUES ({inp.InputID},'{inp.InputName}',{(inp.Enabled?1:0)},1,'{inp.InputPath}','{inp.SearchMask}',{inp.StableTime},{inp.PollTime},{(inp.UseRegExp ? 1 : 0)},'{inp.NamingMask}',{(int)inp.InputType},'{inp.FTPserver}','{inp.FTPusername}','{inp.FTPpassword}','{inp.FTPfolder}',{inp.FTPport},{(inp.UseSpecificUser ? 0 : 1)},'{inp.UserName}','{inp.Password}',{(inp.CallScript ? 1 : 0)},'{inp.ScriptName}','{inp.Separators}',{(inp.FTPpasw?1:0)},{(inp.FTPxcrc?1:0)},{((int)inp.FTPtls)},{(inp.MakeCopy?1:0)},'{inp.CopyFolder}',GETDATE(),{(inp.SendAckFile ? 1 : 0)},'{inp.AckFileFolder}',{inp.AckFlagValue})";
            }
            SqlCommand command = new SqlCommand(sql, connection)
            {
                CommandType = CommandType.Text
            };
            try
            {
                if (connection.State == ConnectionState.Closed || connection.State == ConnectionState.Broken)
                    connection.Open();

                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                errmsg = ex.Message;
                return false;
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                    connection.Close();
                command.Dispose();
            }

            if (hasID)
            {
                DeleteExpressions(false, inp.InputID, out errmsg);
            }
            foreach(Models.RegExpression regExp in inp.RegularExpressions)
            {
                if (InsertExpression(false,inp.InputID, regExp, out errmsg) == false)
                    return false;
            }

            return true;
        }

        public bool DeleteInputConfiguration(int inputID, out string errmsg)
        {
       
            if (DeleteExpressions(false, inputID, out errmsg) == false)
            {
                Utils.WriteLog(false, "db.DeleteExpressions() - " + errmsg);
                return false;
            }

            string sql = $"DELETE FROM InputConfigurations WHERE InputID={inputID}";
            SqlCommand command = new SqlCommand(sql, connection)
            {
                CommandType = CommandType.Text
            };
            try
            {
                if (connection.State == ConnectionState.Closed || connection.State == ConnectionState.Broken)
                    connection.Open();

                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                errmsg = ex.Message;
                return false;
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                    connection.Close();
                command.Dispose();
            }


            return true;
        }

        #endregion

        #region Publishers    
        public bool GetPublisherList(ref List<Models.Publisher> publishers, out string errmsg)
        {
            errmsg = "";
            publishers.Clear();

            string sql = "SELECT DISTINCT PublisherID,PublisherName FROM PublisherNames ORDER BY PublisherName";
            SqlCommand command = new SqlCommand(sql, connection)
            {
                CommandType = CommandType.Text
            };

            SqlDataReader reader = null;

            try
            {
                if (connection.State == ConnectionState.Closed || connection.State == ConnectionState.Broken)
                    connection.Open();

                reader = command.ExecuteReader();

                while (reader.Read())
                {
                    int idx = 0;
                    publishers.Add(new Models.Publisher
                    {
                         PublisherID = reader.GetInt32(idx++),
                        PublisherName = reader.GetString(idx++).Trim()
                    });
                }
            }
            catch (Exception ex)
            {
                errmsg = ex.Message;
                return false;
            }
            finally
            {
                if (reader != null)
                    reader.Close();
                if (connection.State == ConnectionState.Open)
                    connection.Close();
                command.Dispose();
            }

            return true;
        }

        public bool InsertUpdatePublisher(Models.Publisher publisher, out string errmsg)
        {
          

            bool hasID = false;
            string sql;
            if (HasID(publisher.PublisherID, "PublisherID", "PublisherNames", ref hasID, out errmsg) == false)
                return false;

            if (hasID)
                sql = $"UPDATE PublisherNames SET PublisherName='{publisher.PublisherName}' WHERE PublisherID={publisher.PublisherID}";
            else
            {
                publisher.PublisherID = GenerateNextID("PublisherID", "PublisherNames", out errmsg);
                if (publisher.PublisherID <= 0)
                    return false;
                sql = $"INSERT INTO PublisherNames (PublisherID,PublisherName) VALUES ({publisher.PublisherID},'{publisher.PublisherName}')";
            }
            SqlCommand command = new SqlCommand(sql, connection)
            {
                CommandType = CommandType.Text
            };
            try
            {
                if (connection.State == ConnectionState.Closed || connection.State == ConnectionState.Broken)
                    connection.Open();

                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                errmsg = ex.Message;
                return false;
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                    connection.Close();
                command.Dispose();
            }          

            return true;
        }

        public bool DeletePublisher(int publisherID, out string errmsg)
        {
            errmsg = "";

            string sql = $"DELETE FROM PublisherNames WHERE PublisherID={publisherID}";
            SqlCommand command = new SqlCommand(sql, connection)
            {
                CommandType = CommandType.Text
            };
            try
            {
                if (connection.State == ConnectionState.Closed || connection.State == ConnectionState.Broken)
                    connection.Open();

                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                errmsg = ex.Message;
                return false;
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                    connection.Close();
                command.Dispose();
            }


            return true;
        }


        #endregion

        #region Channels

        public bool GetChannel(int channelID, ref Models.Channel channel, out string errmsg)
        {
            List<Models.Channel> channels = new List<Models.Channel>();
            if (GetChannelList(ref channels, channelID, out errmsg) == false)
                return false;
            if (channels.Count == 0)
            {
                errmsg = "Unable to load specific Channel";
                return false;
            }

            channel = channels[0];

            return true;
        }

        public int GetChannelType(int channelID, out string errmsg)
        {
            errmsg = "";
            int pdfType = 0;

            string sql = $"SELECT PdfType FROM ChannelNames WHERE ChannelID={channelID}";

            SqlCommand command = new SqlCommand(sql, connection)
            {
                CommandType = CommandType.Text
            };
            try
            {
                if (connection.State == ConnectionState.Closed || connection.State == ConnectionState.Broken)
                    connection.Open();

                pdfType = (int)command.ExecuteScalar();
            }
            catch (Exception ex)
            {
                errmsg = ex.Message;
                return 0;
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                    connection.Close();
                command.Dispose();
            }

            return pdfType;

        }

        public bool GetChannelList(ref List<Models.Channel> channels, out string errmsg)
        {
            return GetChannelList(ref channels, 0, out errmsg);
        }


        public bool GetChannelList(ref List<Models.Channel> channels, int specificChannelID, out string errmsg)
        {
            errmsg = "";
            channels.Clear();
            string sql = "SELECT DISTINCT ChannelID,Name,Enabled,OwnerInstance,UseReleaseTime,ReleaseTime,ReleaseTimeEnd,TransmitNameFormat ,TransmitNameDateFormat,TransmitNameUseAbbr,TransmitNameOptions,MiscInt,MiscString,ConfigFile,OutputType,FTPServer,FTPPort,FTPUserName,FTPPassword,FTPfolder,FTPEncryption,FTPPasv,FTPXCRC,FTPTimeout,FTPBlockSize,FTPUseTmpFolder,FTPPostCheck,EmailServer,EmailPort,EmailUserName,EmailPassword,EmailFrom,EmailTo,EmailCC,EmailUseSSL,EmailSubject,EmailBody,EmailHTML,EmailTimeout,OutputFolder,UseSpecificUser,UserName,Password,SubFolderNamingConvension,ChannelNameAlias,PDFType,MergedPDF,EditionsToGenerate,SendCommonPages,ConfigChangeTime,PDFProcessID,TriggerMode,TriggerEmail,DeleteOldOutputFilesDays,UsePackageNames,OnlySentSelectedPages,PageNumberStart,PageNumberEnd FROM ChannelNames ";
            if (specificChannelID > 0)
                sql += $"WHERE ChannelID={specificChannelID} ";

            sql += "ORDER BY ChannelID";

            SqlCommand command = new SqlCommand(sql, connection)
            {
                CommandType = CommandType.Text
            };

            SqlDataReader reader = null;

            try
            {
                if (connection.State == ConnectionState.Closed || connection.State == ConnectionState.Broken)
                    connection.Open();

                reader = command.ExecuteReader();

                while (reader.Read())
                {
                    int idx = 0;
                    channels.Add(new Models.Channel()
                    {
                        ChannelID = reader.GetInt32(idx++),
                        Name = reader.GetString(idx++).Trim(),
                        Enabled = reader.GetInt32(idx++) > 0,
                        OwnerInstance = reader.GetInt32(idx++),
                        //ChannelGroupID = reader.GetInt32(idx++),
                        //PublisherID = reader.GetInt32(idx++),
                        UseReleaseTime = reader.GetInt32(idx++)>0,
                        ReleaseTime = reader.GetInt32(idx++),
                        ReleaseTimeEnd = reader.GetInt32(idx++),
                        TransmitNameFormat = reader.GetString(idx++).Trim(),
                        TransmitNameDateFormat = reader.GetString(idx++).Trim(),
                        TransmitNameUseAbbr = reader.GetInt32(idx++),
                        TransmitNameOptions  = reader.GetInt32(idx++),
                        MiscInt = reader.GetInt32(idx++),
                        MiscString = reader.GetString(idx++).Trim(),
                        ConfigFile = reader.GetString(idx++).Trim(),
                        OutputType =  reader.GetInt32(idx++),
                        FTPServer = reader.GetString(idx++).Trim(),
                        FTPPort = reader.GetInt32(idx++),
                        FTPUserName = reader.GetString(idx++).Trim(),
                        FTPPassword = reader.GetString(idx++).Trim(),
                        FTPfolder = reader.GetString(idx++).Trim(),
                        FTPEncryption = reader.GetInt32(idx++), // (Models.EncryptionType)
                        FTPPasv = reader.GetInt32(idx++) > 0,
                        FTPXCRC = reader.GetInt32(idx++) > 0,
                        FTPTimeout = reader.GetInt32(idx++),
                        FTPBlockSize = reader.GetInt32(idx++),
                        FTPUseTmpFolder = reader.GetInt32(idx++) > 0,
                        FTPPostCheck = reader.GetInt32(idx++), // (Models.FTPPostCheckMode)
                        EmailServer = reader.GetString(idx++).Trim(),
                        EmailPort = reader.GetInt32(idx++),
                        EmailUserName = reader.GetString(idx++).Trim(),
                        EmailPassword = reader.GetString(idx++).Trim(),
                        EmailFrom = reader.GetString(idx++).Trim(),
                        EmailTo = reader.GetString(idx++).Trim(),
                        EmailCC = reader.GetString(idx++).Trim(),
                        EmailUseSSL = reader.GetInt32(idx++) > 0,
                        EmailSubject = reader.GetString(idx++).Trim(),
                        EmailBody = reader.GetString(idx++).Trim(),
                        EmailHTML = reader.GetInt32(idx++) > 0,
                        EmailTimeout = reader.GetInt32(idx++),
                        OutputFolder = reader.GetString(idx++).Trim(),
                        UseSpecificUser = reader.GetInt32(idx++) > 0,
                        UserName = reader.GetString(idx++).Trim(),
                        Password = reader.GetString(idx++).Trim(),
                        SubFolderNamingConvension = reader.GetString(idx++).Trim(),
                        ChannelNameAlias = reader.GetString(idx++).Trim(),
                        PDFType = reader.GetInt32(idx++),
                        MergedPDF = reader.GetInt32(idx++)>0,
                        EditionsToGenerate = reader.GetInt32(idx++),
                        SendCommonPages = reader.GetInt32(idx++)>0,
                        ConfigChangeTime = reader.GetDateTime(idx++),
                        PDFProcessID = reader.GetInt32(idx++),
                        TriggerMode = reader.GetInt32(idx++),
                        TriggerEmail = reader.GetString(idx++).Trim(),
                        DeleteOldOutputFilesDays = reader.GetInt32(idx++),
                        UsePackageNames = reader.GetInt32(idx++) > 0,
                        OnlySentSelectedPages = reader.GetInt32(idx++) > 0,
                        PageNumberStart = reader.GetInt32(idx++),
                        PageNumberEnd = reader.GetInt32(idx++)

                    });
                }
            }
            catch (Exception ex)
            {
                errmsg = ex.Message;
                return false;
            }
            finally
            {
                if (reader != null)
                    reader.Close();
                if (connection.State == ConnectionState.Open)
                    connection.Close();
                command.Dispose();
            }


            foreach (Models.Channel channel in channels)
            {
                List<Models.RegExpression> regexList = new List<Models.RegExpression>();
                if (GetRegularExpressions(true, channel.ChannelID, ref regexList, out errmsg))
                    channel.RegularExpressions = regexList;
            }

            if (specificChannelID <= 0)
                HttpContext.Current.Session["ChannelList"] = channels;

            return true;
        }

        public bool InsertUpdateChannel(Models.Channel channel, out string errmsg)
        {
          
            bool hasID = false;
            string sql;
            if (HasID(channel.ChannelID, "ChannelID", "ChannelNames", ref hasID, out errmsg) == false)
                return false;

            if (hasID)
                sql = $"UPDATE ChannelNames SET Name='{channel.Name}',Enabled={(channel.Enabled ? 1 : 0)},OwnerInstance={channel.OwnerInstance},UseReleaseTime={(channel.UseReleaseTime ? 1 : 0)},ReleaseTime={channel.ReleaseTime},ReleaseTimeEnd={channel.ReleaseTimeEnd},TransmitNameFormat='{channel.TransmitNameFormat}',TransmitNameDateFormat='{channel.TransmitNameDateFormat}',TransmitNameUseAbbr={channel.TransmitNameUseAbbr},TransmitNameOptions={channel.TransmitNameOptions},MiscInt={channel.MiscInt},MiscString='{channel.MiscString}',ConfigFile='{channel.ConfigFile}',OutputType={channel.OutputType},FTPServer='{channel.FTPServer}',FTPPort={channel.FTPPort},FTPUserName='{channel.FTPUserName}',FTPPassword='{channel.FTPPassword}',FTPfolder='{channel.FTPfolder}',FTPEncryption={(int)channel.FTPEncryption},FTPPasv={(channel.FTPPasv ? 1 : 0)},FTPXCRC={(channel.FTPXCRC ? 1 : 0)},FTPTimeout={channel.FTPTimeout},FTPBlockSize={channel.FTPBlockSize},FTPUseTmpFolder={(channel.FTPUseTmpFolder ? 1 : 0)},FTPPostCheck={channel.FTPPostCheck},EmailServer = '{channel.EmailServer}',EmailPort={channel.EmailPort},EmailUserName='{channel.EmailUserName}',EmailPassword='{channel.EmailPassword}',EmailFrom='{channel.EmailFrom}',EmailTo='{channel.EmailTo}',EmailCC='{channel.EmailCC}',EmailUseSSL={(channel.EmailUseSSL ? 1 : 0)},EmailSubject='{channel.EmailSubject}',EmailBody='{channel.EmailBody}',EmailHTML={(channel.EmailHTML ? 1 : 0)},EmailTimeout={channel.EmailTimeout},OutputFolder='{channel.OutputFolder}',UseSpecificUser={(channel.UseSpecificUser?1:0)},UserName='{channel.UserName}',Password='{channel.Password}',SubFolderNamingConvension='{channel.SubFolderNamingConvension}',ChannelNameAlias='{channel.ChannelNameAlias}',PDFType={channel.PDFType},MergedPDF={(channel.MergedPDF?1:0)},EditionsToGenerate={channel.EditionsToGenerate},SendCommonPages={(channel.SendCommonPages ? 1 : 0)},ConfigChangeTime=GETDATE(),PDFProcessID={channel.PDFProcessID},TriggerMode={channel.TriggerMode},TriggerEmail='{channel.TriggerEmail}',DeleteOldOutputFilesDays={channel.DeleteOldOutputFilesDays},UsePackageNames={(channel.UsePackageNames?1:0)},OnlySentSelectedPages={(channel.OnlySentSelectedPages?1:0)},PageNumberStart={channel.PageNumberStart},PageNumberEnd={channel.PageNumberEnd} WHERE ChannelID={channel.ChannelID}";
            else
            {
                channel.ChannelID = GenerateNextID("ChannelID", "ChannelNames", out errmsg);
                if (channel.ChannelID <= 0)
                    return false;
                sql = $"INSERT INTO ChannelNames (ChannelID,Name,Enabled,OwnerInstance,UseReleaseTime,ReleaseTime,ReleaseTimeEnd,TransmitNameFormat ,TransmitNameDateFormat,TransmitNameUseAbbr,TransmitNameOptions,MiscInt,MiscString,ConfigFile,OutputType,FTPServer,FTPPort,FTPUserName,FTPPassword,FTPfolder,FTPEncryption,FTPPasv,FTPXCRC,FTPTimeout,FTPBlockSize,FTPUseTmpFolder,FTPPostCheck,EmailServer,EmailPort,EmailUserName,EmailPassword,EmailFrom,EmailTo,EmailCC,EmailUseSSL,EmailSubject,EmailBody,EmailHTML,EmailTimeout,OutputFolder,UseSpecificUser,UserName,Password,SubFolderNamingConvension,ChannelNameAlias,PDFType,MergedPDF,EditionsToGenerate,SendCommonPages,ConfigChangeTime,PDFProcessID,TriggerMode,TriggerEmail,DeleteOldOutputFilesDays,UsePackageNames,OnlySentSelectedPages,PageNumberStart,PageNumberEnd) VALUES ( {channel.ChannelID},'{channel.Name}',{(channel.Enabled?1:0)},{channel.OwnerInstance},{(channel.UseReleaseTime?1:0)},{channel.ReleaseTime},{channel.ReleaseTimeEnd},'{channel.TransmitNameFormat}' ,'{channel.TransmitNameDateFormat}',{channel.TransmitNameUseAbbr},{channel.TransmitNameOptions},{channel.MiscInt},'{channel.MiscString}','{channel.ConfigFile}',{channel.OutputType},'{channel.FTPServer}',{channel.FTPPort},'{channel.FTPUserName}','{channel.FTPPassword}','{channel.FTPfolder}',{(int)channel.FTPEncryption},{(channel.FTPPasv?1:0)},{(channel.FTPXCRC?1:0)},{channel.FTPTimeout},{channel.FTPBlockSize},{(channel.FTPUseTmpFolder?1:0)},{channel.FTPPostCheck},'{channel.EmailServer}',{channel.EmailPort},'{channel.EmailUserName}','{channel.EmailPassword}','{channel.EmailFrom}','{channel.EmailTo}','{channel.EmailCC}',{(channel.EmailUseSSL?1:0)},'{channel.EmailSubject}','{channel.EmailBody}',{(channel.EmailHTML?1:0)},{channel.EmailTimeout},'{channel.OutputFolder}',{(channel.UseSpecificUser?1:0)},'{channel.UserName}','{channel.Password}','{channel.SubFolderNamingConvension}','{channel.ChannelNameAlias}',{channel.PDFType},{(channel.MergedPDF ? 1:0)},{channel.EditionsToGenerate},{(channel.SendCommonPages?1:0)},GETDATE(),{channel.PDFProcessID},{channel.TriggerMode},'{channel.TriggerEmail}',{channel.DeleteOldOutputFilesDays},{(channel.UsePackageNames ? 1:0)},{(channel.OnlySentSelectedPages ? 1 : 0)},{channel.PageNumberStart},{channel.PageNumberEnd})";
            }
            SqlCommand command = new SqlCommand(sql, connection)
            {
                CommandType = CommandType.Text
            };
            try
            {
                if (connection.State == ConnectionState.Closed || connection.State == ConnectionState.Broken)
                    connection.Open();

                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                errmsg = ex.Message;
                return false;
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                    connection.Close();
                command.Dispose();
            }

            if (hasID)
            {
                DeleteExpressions(true,channel.ChannelID, out errmsg);
            }
            foreach (Models.RegExpression regExp in channel.RegularExpressions)
            {
                if (InsertExpression(true, channel.ChannelID, regExp, out errmsg) == false)
                    return false;
            }

            return true;
        }

        public bool DeleteChannel(int channelID, out string errmsg)
        {
           
            if (DeleteExpressions(true, channelID, out errmsg) == false)
                return false;

            DeletePublicationChannelsWithChannelID(channelID, out errmsg);

            string sql = $"DELETE FROM ChannelNames WHERE ChannelID={channelID}";
            SqlCommand command = new SqlCommand(sql, connection)
            {
                CommandType = CommandType.Text
            };
            try
            {
                if (connection.State == ConnectionState.Closed || connection.State == ConnectionState.Broken)
                    connection.Open();

                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                errmsg = ex.Message;
                return false;
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                    connection.Close();
                command.Dispose();
            }


            return true;
        }

        #endregion

        #region PublicationChannels

        private bool GetPublicationChannels(int publicationID, ref List<Models.PublicationChannel> pubChannelList, out string errmsg)
        {
            errmsg = "";
            pubChannelList.Clear();

            string sql = $"SELECT ChannelID,ISNULL(PushTrigger,0),ISNULL(PubDateMoveDays,0),ISNULL(ReleaseDelay,0),ISNULL(SendPlan,0) FROM PublicationChannels WHERE PublicationID={publicationID} ORDER BY ChannelID";

            SqlCommand command = new SqlCommand(sql, connection)
            {
                CommandType = CommandType.Text
            };

            SqlDataReader reader = null;

            try
            {
                if (connection.State == ConnectionState.Closed || connection.State == ConnectionState.Broken)
                    connection.Open();

                reader = command.ExecuteReader();

                while (reader.Read())
                {
                    pubChannelList.Add(new Models.PublicationChannel()
                    {
                        ChannelID = reader.GetInt32(0),
                        Trigger = reader.GetInt32(1),
                        PubDateMoveDays = reader.GetInt32(2),
                        ReleaseDelay = reader.GetInt32(3),
                        SendPlan = reader.GetInt32(4)>0

                    });
                }
            }
            catch (Exception ex)
            {
                errmsg = ex.Message;
                return false;
            }
            finally
            {
                if (reader != null)
                    reader.Close();
                if (connection.State == ConnectionState.Open)
                    connection.Close();
                command.Dispose();
            }

            return true;
        }

        public bool InsertPublicationChannel(int publicationID, Models.PublicationChannel pubChannel, out string errmsg)
        {
            errmsg = "";
            string sql = $"INSERT INTO PublicationChannels (PublicationID,ChannelID,PushTrigger,PubDateMoveDays,ReleaseDelay,SendPlan) VALUES ({publicationID},{pubChannel.ChannelID},{pubChannel.Trigger},{pubChannel.PubDateMoveDays},{pubChannel.ReleaseDelay},{(pubChannel.SendPlan?1:0)})";

            SqlCommand command = new SqlCommand(sql, connection)
            {
                CommandType = CommandType.Text
            };

            try
            {
                if (connection.State == ConnectionState.Closed || connection.State == ConnectionState.Broken)
                    connection.Open();

                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                errmsg = ex.Message;
                return false;
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                    connection.Close();
                command.Dispose();
            }
            return true;
        }




        public bool DeletePublicationChannels(int publicationID, out string errmsg)
        {
            errmsg = "";
            string sql = $"DELETE FROM PublicationChannels WHERE PublicationID={publicationID}";

            SqlCommand command = new SqlCommand(sql, connection)
            {
                CommandType = CommandType.Text
            };

            try
            {
                if (connection.State == ConnectionState.Closed || connection.State == ConnectionState.Broken)
                    connection.Open();

                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                errmsg = ex.Message;
                return false;
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                    connection.Close();
                command.Dispose();
            }
            return true;
        }

        public bool DeletePublicationChannelsWithChannelID(int channelID, out string errmsg)
        {
            errmsg = "";
            string sql = $"DELETE FROM PublicationChannels WHERE ChannelID={channelID}";

            SqlCommand command = new SqlCommand(sql, connection)
            {
                CommandType = CommandType.Text
            };

            try
            {
                if (connection.State == ConnectionState.Closed || connection.State == ConnectionState.Broken)
                    connection.Open();

                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                errmsg = ex.Message;
                return false;
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                    connection.Close();
                command.Dispose();
            }
            return true;
        }


        #endregion

        #region ImportConfigurations

        public bool GetPPINames(int importID, ref List<Models.PPITranslations> ppiTranslations, out string errmsg)
        {
            errmsg = "";
            ppiTranslations.Clear();

            string sql = $"SELECT DISTINCT RuleID,PPIProduct,PPIEdition,Publication FROM PPINames WHERE ImportID={importID} ORDER BY RuleID";
            SqlCommand command = new SqlCommand(sql, connection)
            {
                CommandType = CommandType.Text
            };

            SqlDataReader reader = null;

            try
            {
                if (connection.State == ConnectionState.Closed || connection.State == ConnectionState.Broken)
                    connection.Open();

                reader = command.ExecuteReader();

                while (reader.Read())
                {
                    int fld = 0;
                    ppiTranslations.Add(new Models.PPITranslations()
                    {
                        RuleID = reader.GetInt32(fld++),
                        PPIProduct = reader.GetString(fld++).Trim(),
                        PPIEdition = reader.GetString(fld++).Trim(),
                        Publication = reader.GetString(fld++).Trim()
                    });
                }
            }
            catch (Exception ex)
            {
                errmsg = ex.Message;
                return false;
            }
            finally
            {
                if (reader != null)
                    reader.Close();
                if (connection.State == ConnectionState.Open)
                    connection.Close();
                command.Dispose();
            }

            return true;
        }

        public bool InsertUpdatePPITranslation(int importID, Models.PPITranslations ppiTranslation, out string errmsg)
        {
           

            bool hasID = false;
            string sql;
            if (HasID(ppiTranslation.RuleID, "RuleID", "PPINames", ref hasID, out errmsg) == false)
                return false;

            if (hasID)
                sql = $"UPDATE PPINames SET PPIProduct='{ppiTranslation.PPIProduct}',PPIEdition='{ppiTranslation.PPIEdition}',Publication='{ppiTranslation.Publication}',ConfigChangeTime=GETDATE() WHERE RuleID={ppiTranslation.RuleID}";
            else
            {
                ppiTranslation.RuleID = GenerateNextID("RuleID", "PPINames", out errmsg);
                if (ppiTranslation.RuleID <= 0)
                    return false;
                sql = $"INSERT INTO PPINames (RuleID,ImportID,PPIProduct,PPIEdition,Publication,ConfigChangeTime) VALUES ({ppiTranslation.RuleID},{importID},'{ppiTranslation.PPIProduct}','{ppiTranslation.PPIEdition}','{ppiTranslation.Publication}',GETDATE())";
            }
            SqlCommand command = new SqlCommand(sql, connection)
            {
                CommandType = CommandType.Text
            };

            try
            {
                if (connection.State == ConnectionState.Closed || connection.State == ConnectionState.Broken)
                    connection.Open();

                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                errmsg = ex.Message;
                return false;
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                    connection.Close();
                command.Dispose();
            }
            return true;
        }

        public bool GetImportConfiguration(int importID, ref Models.ImportConfiguration import, out string errmsg)
        {
            List<Models.ImportConfiguration> imports = new List<Models.ImportConfiguration>();
            if (GetImportConfigurations(importID, ref imports, out errmsg) == false)
                return false;
            if (imports.Count > 0)
                import = imports[0];

            return true;
        }
        public bool GetImportConfigurations(ref List<Models.ImportConfiguration> importList, out string errmsg)
        {
            return GetImportConfigurations(0, ref importList, out errmsg);
        }

        public bool GetImportConfigurations(int specificImportID, ref List<Models.ImportConfiguration> importList, out string errmsg)
        {
            errmsg = "";
            importList.Clear();

            string sql = $"SELECT DISTINCT ImportID,Name,Enabled,Type,InputFolder,DoneFolder,ErrorFolder,LogFolder,ConfigFile,ConfigFile2,OwnerInstance,CopyFolder,SendErrorEmail,EmailReceiver,ConfigChangeTime FROM ImportConfigurations ";

            if (specificImportID > 0)
            sql += $"WHERE ImportID={specificImportID}";

            sql += " ORDER BY ImportID";

            SqlCommand command = new SqlCommand(sql, connection)
            {
                CommandType = CommandType.Text
            };

            SqlDataReader reader = null;

            try
            {
                if (connection.State == ConnectionState.Closed || connection.State == ConnectionState.Broken)
                    connection.Open();

                reader = command.ExecuteReader();

                while (reader.Read())
                {
                    int fld = 0;
                    importList.Add(new Models.ImportConfiguration()
                    {
                        ImportID = reader.GetInt32(fld++), 
                        Name = reader.GetString(fld++).Trim(),
                        Enabled = reader.GetInt32(fld++)>0,
                        ImportType = reader.GetInt32(fld++), 
                        InputFolder = reader.GetString(fld++).Trim(),
                        DoneFolder = reader.GetString(fld++).Trim(),
                        ErrorFolder = reader.GetString(fld++).Trim(),
                        LogFolder = reader.GetString(fld++).Trim(),
                        ConfigFile = reader.GetString(fld++).Trim(),
                        ConfigFile2 = reader.GetString(fld++).Trim(),
                        OwnerInstance = reader.GetInt32(fld++),
                        CopyFolder = reader.GetString(fld++).Trim(),
                        SendErrorEmail = reader.GetInt32(fld++) > 0,
                        EmailReceiver = reader.GetString(fld++).Trim(),
                        ConfigChangeTime = reader.GetDateTime(fld++)
                    });
                }
            }
            catch (Exception ex)
            {
                errmsg = ex.Message;
                return false;
            }
            finally
            {
                if (reader != null)
                    reader.Close();
                if (connection.State == ConnectionState.Open)
                    connection.Close();
                command.Dispose();
            }


            foreach (Models.ImportConfiguration import in importList)
            {
                List<Models.PPITranslations> ppiTranslations = new List<Models.PPITranslations>();
                if (GetPPINames(import.ImportID, ref ppiTranslations, out errmsg))
                    import.PPITranslations = ppiTranslations;
            }

            return true;
        }

        public bool InsertUpdateImportConfiguration(Models.ImportConfiguration importConf, out string errmsg)
        {
        

            bool hasID = false;
            string sql;
            if (HasID(importConf.ImportID, "ImportID", "ImportConfigurations", ref hasID, out errmsg) == false)
                return false;

            if (hasID)
                sql = $"UPDATE ImportConfigurations SET Name='{importConf.Name}',Type={(int)importConf.ImportType},InputFolder='{importConf.InputFolder}',DoneFolder='{importConf.DoneFolder}',ErrorFolder='{importConf.ErrorFolder}',LogFolder='{importConf.LogFolder}',ConfigFile='{importConf.ConfigFile}',ConfigFile2='{importConf.ConfigFile2}',OwnerInstance={importConf.OwnerInstance},CopyFolder='{importConf.CopyFolder}',SendErrorEmail={(importConf.SendErrorEmail?1:0)},EmailReceiver='{importConf.EmailReceiver}',ConfigChangeTime=GETDATE() WHERE ImportID={importConf.ImportID}";
            else
            {
                importConf.ImportID = GenerateNextID("ImportID", "ImportConfigurations", out errmsg);
                if (importConf.ImportID <= 0)
                    return false;
                sql = $"INSERT INTO ImportConfigurations (ImportID,Name,Type,InputFolder,DoneFolder,ErrorFolder,LogFolder,ConfigFile,ConfigFile2,OwnerInstance,CopyFolder,SendErrorEmail,EmailReceiver,ConfigChangeTime) VALUES ({importConf.ImportID},'{importConf.Name}',{(int)importConf.ImportType},'{importConf.InputFolder},'{importConf.DoneFolder}','{importConf.ErrorFolder}','{importConf.LogFolder}','{importConf.ConfigFile}','{importConf.ConfigFile2},{importConf.OwnerInstance},'{importConf.CopyFolder}',{(importConf.SendErrorEmail?1:0)},'{importConf.EmailReceiver}',GETDATE())";
            }
            SqlCommand command = new SqlCommand(sql, connection)
            {
                CommandType = CommandType.Text
            };

            try
            {
                if (connection.State == ConnectionState.Closed || connection.State == ConnectionState.Broken)
                    connection.Open();

                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                errmsg = ex.Message;
                return false;
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                    connection.Close();
                command.Dispose();
            }

            foreach(Models.PPITranslations tx in importConf.PPITranslations)
                InsertUpdatePPITranslation(importConf.ImportID, tx , out errmsg);

            return true;
        }

        public bool DeleteImportConfiguration(int importID, out string errmsg)
        {
            if (DeletePPITranslations(importID, out errmsg) == false)
                return false;

            string sql = $"DELETE FROM ImportConfigurations WHERE ImportID={importID}";

            SqlCommand command = new SqlCommand(sql, connection)
            {
                CommandType = CommandType.Text
            };

            try
            {
                if (connection.State == ConnectionState.Closed || connection.State == ConnectionState.Broken)
                    connection.Open();

                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                errmsg = ex.Message;
                return false;
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                    connection.Close();
                command.Dispose();
            }
            return true;
        }


        public bool DeletePPITranslations(int importID, out string errmsg)
        {
            errmsg = "";
            string sql = $"DELETE FROM PPINames WHERE ImportID={importID}";

            SqlCommand command = new SqlCommand(sql, connection)
            {
                CommandType = CommandType.Text
            };

            try
            {
                if (connection.State == ConnectionState.Closed || connection.State == ConnectionState.Broken)
                    connection.Open();

                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                errmsg = ex.Message;
                return false;
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                    connection.Close();
                command.Dispose();
            }

            return true;
        }


        #endregion

        #region Publications

        public bool GetPublicationFromName(string name, string inputAlias, ref int id, out string errmsg)
        {
            id = 0;
            errmsg = "";
            string sql = $"SELECT TOP 1 PublicationID FROM PublicationNames WHERE Name='{name}' OR InputAlias='{inputAlias}'";
            SqlCommand command = new SqlCommand(sql, connection)
            {
                CommandType = CommandType.Text
            };

            SqlDataReader reader = null;

            try
            {
                if (connection.State == ConnectionState.Closed || connection.State == ConnectionState.Broken)
                    connection.Open();

                reader = command.ExecuteReader();

                if (reader.Read())
                {
                    id = reader.GetInt32(0);
                }
            }
            catch (Exception ex)
            {
                errmsg = ex.Message;
                return false;
            }
            finally
            {
                if (reader != null)
                    reader.Close();
                if (connection.State == ConnectionState.Open)
                    connection.Close();
                command.Dispose();
            }

            return true;
        }

        public bool GetPublication(int publicationID, ref Models.Publication publication, out string errmsg)
        {
            List<Models.Publication> publications = new List<Models.Publication>();
            if (GetPublications(ref publications, publicationID, 0, out errmsg) == false)
                return false;
            if (publications.Count == 1)
            {
                publication = publications[0];
                return true;
            }

            errmsg = "Unable to load specific PublicationID";
            return false;
        }

        public bool GetPublications(ref List<Models.Publication> publications, out string errmsg)
        {
            return GetPublications(ref publications, 0, 0, out errmsg);
        }

        public bool GetPublications(ref List<Models.Publication> publications, int specificPublicationID, int specificPublisherID, out string errmsg)
        {
            errmsg = "";
            publications.Clear();
            string sql = "SELECT DISTINCT PublicationID,[Name],PageFormatID,TrimToFormat,LatestHour,DefaultProofID,DefaultApprove,DefaultPriority,CustomerID,AutoPurgeKeepDays,EmailRecipient,EmailCC,EmailSubject,EmailBody,UploadFolder,Deadline,AnnumText,AllowUnplanned,ReleaseDays,ReleaseTime,PubdateMove,PubdateMoveDays,InputAlias,OutputALias,ExtendedAlias,PublisherID,ConfigChangeTime,ExtendedAlias2,NoReleaseTime,AutoPurgeKeepDaysArchive FROM PublicationNames";

            if (specificPublicationID > 0)
                sql += $" WHERE PublicationID={specificPublicationID} ";
            else if (specificPublisherID > 0)
                sql += $" WHERE PublisherID={specificPublisherID} ";

            sql += " ORDER BY [Name]";
            SqlCommand command = new SqlCommand(sql, connection)
            {
                CommandType = CommandType.Text
            };

            SqlDataReader reader = null;

            try
            {
                if (connection.State == ConnectionState.Closed || connection.State == ConnectionState.Broken)
                    connection.Open();

                reader = command.ExecuteReader();

                while (reader.Read())
                {
                    int idx = 0;
                    publications.Add(new Models.Publication()
                    {
                        PublicationID = reader.GetInt32(idx++),
                        Name = reader.GetString(idx++).Trim(),
                        PageFormatID = reader.GetInt32(idx++),
                        TrimToFormat = reader.GetInt32(idx++)>0,
                        LatestHour =  reader.GetDouble(idx++),
                        DefaultProofID = reader.GetInt32(idx++),
                        DefaultApprove = reader.GetInt32(idx++)>0,
                        DefaultPriority = reader.GetInt32(idx++),
                        CustomerID = reader.GetInt32(idx++),
                        AutoPurgeKeepDays = reader.GetInt32(idx++),
                        EmailRecipient = reader.GetString(idx++).Trim(),
                        EmailCC = reader.GetString(idx++).Trim(),
                        EmailSubject = reader.GetString(idx++).Trim(),
                        EmailBody = reader.GetString(idx++).Trim(),
                        UploadFolder = reader.GetString(idx++).Trim(),
                        Deadline = reader.GetDateTime(idx++),
                        AnnumText = reader.GetString(idx++).Trim(),
                        AllowUnplanned = reader.GetInt32(idx++) > 0,
                        ReleaseDays = reader.GetInt32(idx++),
                        ReleaseTime = reader.GetInt32(idx++),
                        PubdateMove = reader.GetInt32(idx++)>0,
                        PubdateMoveDays = reader.GetInt32(idx++),
                        InputAlias = reader.GetString(idx++).Trim(),
                        OutputAlias = reader.GetString(idx++).Trim(),
                        ExtendedAlias = reader.GetString(idx++).Trim(),
                        PublisherID = reader.GetInt32(idx++),
                        ConfigChangeTime = reader.GetDateTime(idx++),
                        ExtendedAlias2 = reader.GetString(idx++).Trim(),
                        NoReleaseTime = reader.GetInt32(idx++),
                        AutoPurgeKeepDaysArchive = reader.GetInt32(idx++)

                    });
                }
            }
            catch (Exception ex)
            {
                errmsg = ex.Message;
                return false;
            }
            finally
            {
                if (reader != null)
                    reader.Close();
                if (connection.State == ConnectionState.Open)
                    connection.Close();
                command.Dispose();
            }

            foreach(Models.Publication publication in publications)
            {
                List<Models.PublicationChannel> pubChannels = new List<Models.PublicationChannel>();
                if (GetPublicationChannels(publication.PublicationID, ref pubChannels, out errmsg) == false)
                    return false;
                publication.PublicationChannels = pubChannels;
             
            }

            return true;
        }

        public bool InsertUpdatePublication(Models.Publication publication, out string errmsg)
        {
          
            bool hasID = false;
            string sql;
            if (HasID(publication.PublicationID, "PublicationID", "PublicationNames", ref hasID, out errmsg) == false)
                return false;

            if (hasID)
                sql = $"UPDATE PublicationNames SET Name='{publication.Name}',PageFormatID={publication.PageFormatID},TrimToFormat={(publication.TrimToFormat?1:0)},LatestHour={publication.LatestHour},DefaultProofID={publication.DefaultProofID},DefaultApprove={(publication.DefaultApprove?1:0)},DefaultPriority={publication.DefaultPriority},CustomerID={publication.CustomerID},AutoPurgeKeepDays={publication.AutoPurgeKeepDays},EmailRecipient='{publication.EmailRecipient}',EmailCC='{publication.EmailCC}',EmailSubject='{publication.EmailSubject}',EmailBody='{publication.EmailBody}',UploadFolder='{publication.UploadFolder}',Deadline=@Deadline,AnnumText='{publication.AnnumText}',AllowUnplanned={(publication.AllowUnplanned?1:0)},ReleaseDays={publication.ReleaseDays},ReleaseTime={publication.ReleaseTime},PubdateMove={(publication.PubdateMove?1:0)},PubdateMoveDays={publication.PubdateMoveDays},InputAlias='{publication.InputAlias}',OutputAlias='{publication.OutputAlias}',ExtendedAlias='{publication.ExtendedAlias}',PublisherID={publication.PublisherID},ConfigChangeTime=GETDATE(),ExtendedAlias2='{publication.ExtendedAlias2}',NoReleaseTime={(publication.NoReleaseTime)},AutoPurgeKeepDaysArchive={publication.AutoPurgeKeepDaysArchive} WHERE PublicationID={publication.PublicationID}";
            else
            {
                publication.PublicationID = GenerateNextID("PublicationID", "PublicationNames", out errmsg);
                if (publication.PublicationID <= 0)
                    return false;
                sql = $"INSERT INTO PublicationNames (PublicationID,Name,PageFormatID,TrimToFormat,LatestHour,DefaultProofID,DefaultApprove,DefaultPriority,CustomerID,AutoPurgeKeepDays,EmailRecipient,EmailCC,EmailSubject,EmailBody,UploadFolder,Deadline,AnnumText,AllowUnplanned,ReleaseDays,ReleaseTime,PubdateMove,PubdateMoveDays,InputAlias,OutputAlias,ExtendedAlias,PublisherID,ConfigChangeTime,ExtendedAlias2,NoReleaseTime,AutoPurgeKeepDaysArchive) VALUES ({publication.PublicationID},'{publication.Name}',{publication.PageFormatID},{(publication.TrimToFormat?1:0)},{publication.LatestHour},{publication.DefaultProofID},{(publication.DefaultApprove?1:0)},{publication.DefaultPriority},{publication.CustomerID},{publication.AutoPurgeKeepDays},'{publication.EmailRecipient}','{publication.EmailCC}','{publication.EmailSubject}','{publication.EmailBody}','{publication.UploadFolder}',@Deadline, '{publication.AnnumText}',{(publication.AllowUnplanned?1:0)},{publication.ReleaseDays},{publication.ReleaseTime},{(publication.PubdateMove?1:0)},{publication.PubdateMoveDays},'{publication.InputAlias}','{publication.OutputAlias}','{publication.ExtendedAlias}',{publication.PublisherID},GETDATE(),'{publication.ExtendedAlias2}',{(publication.NoReleaseTime)},{publication.AutoPurgeKeepDaysArchive})";
            }

            SqlCommand command = new SqlCommand(sql, connection)
            {
                CommandType = CommandType.Text
            };
            if (publication.Deadline.Year < 1975)
                publication.Deadline = new DateTime(1975, 1, 1, 0, 0, 0);
            command.Parameters.Add(new SqlParameter("@Deadline", SqlDbType.DateTime) { Value = publication.Deadline });

            try
            {
                if (connection.State == ConnectionState.Closed || connection.State == ConnectionState.Broken)
                    connection.Open();

                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                errmsg = ex.Message;
                return false;
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                    connection.Close();
                command.Dispose();
            }

            if (hasID)
            {
                DeletePublicationChannels(publication.PublicationID,  out errmsg);
            }
            foreach (Models.PublicationChannel pubChannel in publication.PublicationChannels)
            {
                if (InsertPublicationChannel(publication.PublicationID, pubChannel, out errmsg) == false)
                    return false;
            }
            
            return true;
        }

        public bool DeletePublication(int publicationID, out string errmsg)
        {
     

            if (DeletePublicationChannels(publicationID, out errmsg) == false)
                return false;
            if (DeleteUserPublications(publicationID, out errmsg) == false)
                return false;

            string sql = $"DELETE FROM PublicationNames WHERE PublicationID={publicationID}";

            SqlCommand command = new SqlCommand(sql, connection)
            {
                CommandType = CommandType.Text
            };

            try
            {
                if (connection.State == ConnectionState.Closed || connection.State == ConnectionState.Broken)
                    connection.Open();

                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                errmsg = ex.Message;
                return false;
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                    connection.Close();
                command.Dispose();
            }
            return true;
        }

        public bool PublicationInPageTable(int publicationID, ref bool used, out string errmsg)
        {
            errmsg = "";

            used = false;
            string sql = $"SELECT PublicationID FROM PageTable WHERE PublicationID={publicationID}";

            SqlCommand command = new SqlCommand(sql, connection)
            {
                CommandType = CommandType.Text
            };

            SqlDataReader reader = null;

            try
            {
                if (connection.State == ConnectionState.Closed || connection.State == ConnectionState.Broken)
                    connection.Open();

                reader = command.ExecuteReader();

                if (reader.Read())
                {
                    used = true;
                }
            }
            catch (Exception ex)
            {
                errmsg = ex.Message;
                return false;
            }
            finally
            {
                if (reader != null)
                    reader.Close();
                if (connection.State == ConnectionState.Open)
                    connection.Close();
                command.Dispose();
            }

            return true;
        }

        #endregion

        #region Users

        public bool GetUser(string userName, ref Models.User user, out string errmsg)
        {
            List<Models.User> users = new List<Models.User>();
            if (GetUsers(userName, ref users, out errmsg) == false)
                return false;
            if (users.Count > 0)
                user = users[0];

            return true;
        }

        public bool GetUsers(ref List<Models.User> users, out string errmsg)
        {
            return GetUsers("", ref users, out  errmsg);
        }

        public bool GetUsers(string specificUserName, ref List<Models.User> users, out string errmsg)
        {
            errmsg = "";
            users.Clear();

            string sql = "SELECT DISTINCT Username,Password,UserGroupID,AccountEnabled,Email,FullName FROM UserNames";
            if (specificUserName != "")
                sql += $" WHERE UserName='{specificUserName}'";
            SqlCommand command = new SqlCommand(sql, connection)
            {
                CommandType = CommandType.Text
            };

            SqlDataReader reader = null;

            try
            {
                if (connection.State == ConnectionState.Closed || connection.State == ConnectionState.Broken)
                    connection.Open();

                reader = command.ExecuteReader();

                while (reader.Read())
                {
                    int idx = 0;
                    users.Add(new Models.User()
                    {
                        UserName = reader.GetString(idx++).Trim(),
                        Password = reader.GetString(idx++).Trim(),
                        UserGroupID = reader.GetInt32(idx++),
                        AccountEnabled = reader.GetInt32(idx++) > 0,
                        Email = reader.GetString(idx++).Trim(),
                        FullName = reader.GetString(idx++).Trim()
                    });
                }
            }
            catch (Exception ex)
            {
                errmsg = ex.Message;
                return false;
            }
            finally
            {
                if (reader != null)
                    reader.Close();
                if (connection.State == ConnectionState.Open)
                    connection.Close();
                command.Dispose();
            }

            foreach(Models.User user in users)
            {
                List<int> publicationIDList = new List<int>();
                if (GetUserPublications(user.UserName, ref publicationIDList, out errmsg) == false)
                    return false;
                user.publicationIDList = publicationIDList;
            }

            foreach (Models.User user in users)
            {
                List<int> publisherIDList = new List<int>();
                if (GetUserPublishers(user.UserName, ref publisherIDList, out errmsg) == false)
                    return false;
                user.publisherIDList = publisherIDList;
            }

            return true;
        }

        public bool GetUserPublications(string userName, ref List<int> publicationIdList, out string errmsg)
        {
            errmsg = "";
            publicationIdList.Clear();
            string sql = $"SELECT DISTINCT PublicationID FROM UserPublications WHERE UserName='{userName}'";
            SqlCommand command = new SqlCommand(sql, connection)
            {
                CommandType = CommandType.Text
            };

            SqlDataReader reader = null;

            try
            {
                if (connection.State == ConnectionState.Closed || connection.State == ConnectionState.Broken)
                    connection.Open();

                reader = command.ExecuteReader();

                while (reader.Read())
                {
                    publicationIdList.Add(reader.GetInt32(0));
                }
            }
            catch (Exception ex)
            {
                errmsg = ex.Message;
                return false;
            }
            finally
            {
                if (reader != null)
                    reader.Close();
                if (connection.State == ConnectionState.Open)
                    connection.Close();
                command.Dispose();
            }

            return true;
        }

        public bool GetUserPublishers(string userName, ref List<int> publisherList, out string errmsg)
        {
            errmsg = "";
            publisherList.Clear();
            string sql = $"SELECT DISTINCT PublisherID FROM UserPublishers WHERE UserName='{userName}'";
            SqlCommand command = new SqlCommand(sql, connection)
            {
                CommandType = CommandType.Text
            };

            SqlDataReader reader = null;

            try
            {
                if (connection.State == ConnectionState.Closed || connection.State == ConnectionState.Broken)
                    connection.Open();

                reader = command.ExecuteReader();

                while (reader.Read())
                {
                    publisherList.Add(reader.GetInt32(0));
                }
            }
            catch (Exception ex)
            {
                errmsg = ex.Message;
                return false;
            }
            finally
            {
                if (reader != null)
                    reader.Close();
                if (connection.State == ConnectionState.Open)
                    connection.Close();
                command.Dispose();
            }

            return true;
        }

        public bool InsertUpdateUser(Models.User user, out string errmsg)
        {
      
            bool hasID = false;
            string sql;
            if (HasIDString(user.UserName, "UserName", "UserNames", ref hasID, out errmsg) == false)
                return false;

            if (hasID)
                sql = $"UPDATE UserNames SET Password='{user.Password}', UserGroupID={user.UserGroupID},AccountEnabled={(user.AccountEnabled?1:0)},Email='{user.Email}',FullName='{user.FullName}' WHERE UserName='{user.UserName}'";
            else            
                sql = $"INSERT INTO UserNames (Username,Password,UserGroupID,AccountEnabled,Email,FullName) VALUES ('{user.UserName}','{user.Password}',{user.UserGroupID},{(user.AccountEnabled ? 1 : 0)},'{user.Email}','{user.FullName}')";

            SqlCommand command = new SqlCommand(sql, connection)
            {
                CommandType = CommandType.Text
            };

            try
            {
                if (connection.State == ConnectionState.Closed || connection.State == ConnectionState.Broken)
                    connection.Open();

                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                errmsg = ex.Message;
                return false;
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                    connection.Close();
                command.Dispose();
            }

            if (hasID)
                if (DeleteUserPublications(user.UserName, out errmsg) == false)
                    return false;

            foreach (int publicationID in user.publicationIDList)
            {
                if (InsertUserPublication(user.UserName, publicationID, out errmsg) == false)
                    return false;
            }


            if (hasID)
                if (DeleteUserPublishers(user.UserName, out errmsg) == false)
                    return false;

            foreach (int publisherID in user.publisherIDList)
            {
                if (InsertUserPublisher(user.UserName, publisherID, out errmsg) == false)
                    return false;
            }

            return true;
        }

        public bool InsertUserPublication(string userName, int publicationID, out string errmsg)
        {
            errmsg = "";
            string sql = $"INSERT INTO UserPublications (UserName,PublicationID) VALUES ('{userName}',{publicationID})";

            SqlCommand command = new SqlCommand(sql, connection)
            {
                CommandType = CommandType.Text
            };

            try
            {
                if (connection.State == ConnectionState.Closed || connection.State == ConnectionState.Broken)
                    connection.Open();

                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                errmsg = ex.Message;
                return false;
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                    connection.Close();
                command.Dispose();
            }
            return true;
        }

        public bool InsertUserPublisher(string userName, int publisherID, out string errmsg)
        {
            errmsg = "";
            string sql = $"INSERT INTO UserPublishers (UserName,PublisherID) VALUES ('{userName}',{publisherID})";

            SqlCommand command = new SqlCommand(sql, connection)
            {
                CommandType = CommandType.Text
            };

            try
            {
                if (connection.State == ConnectionState.Closed || connection.State == ConnectionState.Broken)
                    connection.Open();

                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                errmsg = ex.Message;
                return false;
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                    connection.Close();
                command.Dispose();
            }
            return true;
        }

        public bool DeleteUser(string userName, out string errmsg)
        {
          
            if (DeleteUserPublications(userName, out errmsg) == false)
                return false;

            string sql = $"DELETE FROM UserNames WHERE UserName='{userName}'";

            SqlCommand command = new SqlCommand(sql, connection)
            {
                CommandType = CommandType.Text
            };

            try
            {
                if (connection.State == ConnectionState.Closed || connection.State == ConnectionState.Broken)
                    connection.Open();

                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                errmsg = ex.Message;
                return false;
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                    connection.Close();
                command.Dispose();
            }
            return true;
        }

        public bool DeleteUserPublications(int publicationID, out string errmsg)
        {
            errmsg = "";
            string sql = $"DELETE FROM UserPublications WHERE PublicationID={publicationID}";

            SqlCommand command = new SqlCommand(sql, connection)
            {
                CommandType = CommandType.Text
            };

            try
            {
                if (connection.State == ConnectionState.Closed || connection.State == ConnectionState.Broken)
                    connection.Open();

                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                errmsg = ex.Message;
                return false;
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                    connection.Close();
                command.Dispose();
            }
            return true;
        }


        public bool DeleteUserPublications(string userName, out string errmsg)
        {
            errmsg = "";
            string sql = $"DELETE FROM UserPublications WHERE UserName='{userName}'";

            SqlCommand command = new SqlCommand(sql, connection)
            {
                CommandType = CommandType.Text
            };

            try
            {
                if (connection.State == ConnectionState.Closed || connection.State == ConnectionState.Broken)
                    connection.Open();

                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                errmsg = ex.Message;
                return false;
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                    connection.Close();
                command.Dispose();
            }
            return true;
        }


        public bool DeleteUserPublishers(string userName, out string errmsg)
        {
            errmsg = "";
            string sql = $"DELETE FROM UserPublishers WHERE UserName='{userName}'";

            SqlCommand command = new SqlCommand(sql, connection)
            {
                CommandType = CommandType.Text
            };

            try
            {
                if (connection.State == ConnectionState.Closed || connection.State == ConnectionState.Broken)
                    connection.Open();

                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                errmsg = ex.Message;
                return false;
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                    connection.Close();
                command.Dispose();
            }
            return true;
        }

        #endregion

        #region UserGroups

        public bool GetUserGroups(ref List<Models.UserGroup> userGroupList, out string errmsg)
        {
            errmsg = "";
            userGroupList.Clear();
            string sql = $"SELECT DISTINCT UserGroupID,UserGroupName,IsAdmin FROM UserGroupNames";
            SqlCommand command = new SqlCommand(sql, connection)
            {
                CommandType = CommandType.Text
            };

            SqlDataReader reader = null;

            try
            {
                if (connection.State == ConnectionState.Closed || connection.State == ConnectionState.Broken)
                    connection.Open();

                reader = command.ExecuteReader();

                while (reader.Read())
                {
                    userGroupList.Add(new Models.UserGroup()
                    {
                        UserGroupID = reader.GetInt32(0),
                        UserGroupName = reader.GetString(1).Trim(),
                        IsAdmin = reader.GetInt32(2) > 0
                    });
                }
            }
            catch (Exception ex)
            {
                errmsg = ex.Message;
                return false;
            }
            finally
            {
                if (reader != null)
                    reader.Close();
                if (connection.State == ConnectionState.Open)
                    connection.Close();
                command.Dispose();
            }

            return true;
        }

        #endregion

        #region PDFProcesses

        public bool GetPDFProcesses(ref List<Models.PDFProcess> processes, out string errmsg)
        {
            errmsg = "";
            processes.Clear();

            string sql = "SELECT DISTINCT ProcessID,ProcessName,ProcessType,ConvertProfile,ExternalProcess,ExternalInputFolder,ExternalOutputFolder,ExternalErrorFolder,ProcessTimeOut FROM PDFProcessConfigurations";

            SqlCommand command = new SqlCommand(sql, connection)
            {
                CommandType = CommandType.Text
            };

            SqlDataReader reader = null;

            try
            {
                if (connection.State == ConnectionState.Closed || connection.State == ConnectionState.Broken)
                    connection.Open();

                reader = command.ExecuteReader();

                while (reader.Read())
                {
                    int idx = 0;
                    Models.PDFProcess p = new Models.PDFProcess()
                    {
                        ProcessID = reader.GetInt32(idx++),
                        ProcessName = reader.GetString(idx++).Trim(),
                        ProcessType = (Models.PDFProcessType)reader.GetInt32(idx++),
                        ConvertProfile = reader.GetString(idx++).Trim(),
                        ExternalProcess = reader.GetInt32(idx++)>0,
                        ExternalInputFolder = reader.GetString(idx++).Trim(),
                        ExternalOutputFolder = reader.GetString(idx++).Trim(),
                        ExternalErrorFolder = reader.GetString(idx++).Trim(),
                        ProcessTimeOut = reader.GetInt32(idx++)
                    };
                    if (p.ProcessType == Models.PDFProcessType.ToCMYKPDF)
                        p.ProcessName += " (CMYK)";
                    processes.Add(p);
                }
            }
            catch (Exception ex)
            {
                errmsg = ex.Message;
                return false;
            }
            finally
            {
                if (reader != null)
                    reader.Close();
                if (connection.State == ConnectionState.Open)
                    connection.Close();
                command.Dispose();
            }

            return true;
        }

        public bool InsertUpdatePDFProcess(Models.PDFProcess process, out string errmsg)
        {
          
            bool hasID = false;
            string sql;
            if (HasID(process.ProcessID, "ProcessID", "PDFProcessConfigurations", ref hasID, out errmsg) == false)
                return false;

            if (hasID)
                sql = $"UPDATE PDFProcessConfigurations SET ProcessName='{process.ProcessName}',ProcessType={(int)process.ProcessType},ConvertProfile='{process.ConvertProfile}',ExternalProcess={(process.ExternalProcess?1:0)},ExternalInputFolder='{process.ExternalInputFolder}',ExternalOutputFolder='{process.ExternalOutputFolder}',ExternalErrorFolder='{process.ExternalErrorFolder}',ProcessTimeOut={process.ProcessTimeOut} WHERE ProcessID={process.ProcessID}";
            else
                sql = $"INSERT INTO PDFProcessConfigurations (ProcessID,ProcessName,ProcessType,ConvertProfile,ExternalProcess,ExternalInputFolder,ExternalOutputFolder,ExternalErrorFolder,ProcessTimeOut) VALUES ({process.ProcessID},'{process.ProcessName}',{(int)process.ProcessType},'{process.ConvertProfile}',{(process.ExternalProcess?1:0)},'{process.ExternalInputFolder}','{process.ExternalOutputFolder}','{process.ExternalErrorFolder}',{process.ProcessTimeOut})";

            SqlCommand command = new SqlCommand(sql, connection)
            {
                CommandType = CommandType.Text
            };

            try
            {
                if (connection.State == ConnectionState.Closed || connection.State == ConnectionState.Broken)
                    connection.Open();

                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                errmsg = ex.Message;
                return false;
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                    connection.Close();
                command.Dispose();
            }

            return true;

        }

        public bool DeletePDFProcess(int processID, out string errmsg)
        {
            errmsg = "";
            string sql = $"DELETE FROM PDFProcessConfigurations WHERE ProcessID={processID}";

            SqlCommand command = new SqlCommand(sql, connection)
            {
                CommandType = CommandType.Text
            };

            try
            {
                if (connection.State == ConnectionState.Closed || connection.State == ConnectionState.Broken)
                    connection.Open();

                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                errmsg = ex.Message;
                return false;
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                    connection.Close();
                command.Dispose();
            }
            return true;
        }


        #endregion

        #region Package

        public bool GetPackages(ref List<Models.Package> packages, out string errmsg)
        {
            packages.Clear();
            errmsg = "";
    
            string sql = $"SELECT DISTINCT PG.PackageID,PG.Name,PG.PublicationID,PG.SectionIndex,PG.Condition,PG.Comment,PUB.InputAlias FROM PackageNames PG INNER JOIN PublicationNames PUB ON PUB.PublicationID=PG.PublicationID ORDER BY PG.Name,PG.SectionIndex";
            SqlCommand command = new SqlCommand(sql, connection)
            {
                CommandType = CommandType.Text
            };

            SqlDataReader reader = null;

            try
            {
                if (connection.State == ConnectionState.Closed || connection.State == ConnectionState.Broken)
                    connection.Open();

                reader = command.ExecuteReader();

                while (reader.Read())
                {
                    packages.Add(new Models.Package()
                    {
                        PackageID = reader.GetInt32(0),
                        Name = reader.GetString(1).Trim(),                            
                        PublicationID = reader.GetInt32(2),
                        SectionIndex = reader.GetInt32(3),
                        Condition = reader.GetInt32(4),
                        Comment = reader.GetString(5).Trim(),
                        ProductAlias = reader.GetString(6).Trim()
                    });
                }
            }
            catch (Exception ex)
            {
                errmsg = ex.Message;
                return false;
            }
            finally
            {
                if (reader != null)
                    reader.Close();
                if (connection.State == ConnectionState.Open)
                    connection.Close();
                command.Dispose();
            }

            return true;
        }

        public bool GetPublicationShortList(ref List<Models.PublicationShort> publicationList, out string errmsg)
        {
            errmsg = "";

            string sql = "SELECT DISTINCT PublicationID,Name FROM PublicationNames WHERE Name<>'' ORDER BY Name";

            SqlCommand command = new SqlCommand(sql, connection)
            {
                CommandType = CommandType.Text
            };

            SqlDataReader reader = null;

            try
            {
                if (connection.State == ConnectionState.Closed || connection.State == ConnectionState.Broken)
                    connection.Open();

                reader = command.ExecuteReader();

                while (reader.Read())
                {
                    publicationList.Add(new Models.PublicationShort()
                    {
                        PublicationID = reader.GetInt32(0),
                        Name = reader.GetString(1)
                    });
                }
            }
            catch (Exception ex)
            {
                errmsg = ex.Message;
                return false;
            }
            finally
            {
                if (reader != null)
                    reader.Close();
                if (connection.State == ConnectionState.Open)
                    connection.Close();
                command.Dispose();
            }

            return true;


        }

        private int GetPublicationID(string name, out string errmsg)
        {
            errmsg = "";
            int publicationID = 0;

            string sql = $"SELECT PublicationID FROM PublicationNames WHERE Name='{name}' OR InputAlias='{name}'";

            SqlCommand command = new SqlCommand(sql, connection)
            {
                CommandType = CommandType.Text
            };

            SqlDataReader reader = null;

            try
            {
                if (connection.State == ConnectionState.Closed || connection.State == ConnectionState.Broken)
                    connection.Open();

                reader = command.ExecuteReader();

                if (reader.Read())
                {
                    publicationID = reader.GetInt32(0);
                }
            }
            catch (Exception ex)
            {
                errmsg = ex.Message;
                return -1;
            }
            finally
            {
                if (reader != null)
                    reader.Close();
                if (connection.State == ConnectionState.Open)
                    connection.Close();
                command.Dispose();
            }

            return publicationID;

        }

        public bool InsertUpdatePackage(Models.Package package, out string errmsg)
        {
       

            bool hasID = false;
            string sql;
            if (HasID(package.PackageID, "PackageID", "PackageNames", ref hasID, out errmsg) == false)
                return false;


            package.PublicationID = GetPublicationID(package.ProductAlias, out errmsg);
            if (package.PublicationID == -1)
                return false;

            if (hasID)
                sql = $"UPDATE PackageNames SET Name='{package.Name}',PublicationID={package.PublicationID},SectionIndex={package.SectionIndex},Condition={package.Condition}, Comment='{package.Comment}',ConfigChangeTime=GETDATE() WHERE PackageID={package.PackageID}";
            else
            {
                package.PackageID = GenerateNextID("PackageID", "PackageNames", out errmsg);
                if (package.PackageID <= 0)
                    return false;
                sql = $"INSERT INTO PackageNames (PackageID,Name,PublicationID,SectionIndex,Condition,Comment,ConfigChangeTime) VALUES ({package.PackageID},'{package.Name}',{package.PublicationID},{package.SectionIndex},{package.Condition},'{package.Comment}',GETDATE())";
            }

            SqlCommand command = new SqlCommand(sql, connection)
            {
                CommandType = CommandType.Text
            };

            try
            {
                if (connection.State == ConnectionState.Closed || connection.State == ConnectionState.Broken)
                    connection.Open();

                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                errmsg = ex.Message;
                return false;
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                    connection.Close();
                command.Dispose();
            }
            return true;
        }

        public bool DeletePackage(int packageID, out string errmsg)
        {
            errmsg = "";
            string sql = $"DELETE FROM PackageNames WHERE PackageID={packageID}";

            SqlCommand command = new SqlCommand(sql, connection)
            {
                CommandType = CommandType.Text
            };

            try
            {
                if (connection.State == ConnectionState.Closed || connection.State == ConnectionState.Broken)
                    connection.Open();

                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                errmsg = ex.Message;
                return false;
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                    connection.Close();
                command.Dispose();
            }
            return true;
        }


        #endregion

        public bool GetLogEvents(Models.SeriesType type, int hoursBack, ref List<DateTime> samples, out string errmsg)
        {
            samples.Clear();
            errmsg = "";

            string eventList = "10";
            if (type == Models.SeriesType.Processing)
                eventList = "20,110,120";
            else if (type == Models.SeriesType.Export)
                eventList = "180";

            string sql = $"SELECT EventTime FROM ServiceLog WHERE Event IN ({eventList}) AND DATEDIFF(minute,Eventtime,GETDATE()) <= {hoursBack * 60} ORDER BY EventTime";

            SqlCommand command = new SqlCommand(sql, connection)
            {
                CommandType = CommandType.Text
            };

            SqlDataReader reader = null;

            try
            {
                if (connection.State == ConnectionState.Closed || connection.State == ConnectionState.Broken)
                    connection.Open();

                reader = command.ExecuteReader();

                while (reader.Read())
                {
                    samples.Add(reader.GetDateTime(0));
                }
            }
            catch (Exception ex)
            {
                errmsg = ex.Message;
                return false;
            }
            finally
            {
                if (reader != null)
                    reader.Close();
                if (connection.State == ConnectionState.Open)
                    connection.Close();
                command.Dispose();
            }

            return true;
        }

        public bool GetPublicationsForExport(int channelID, ref List<string> publications, out string errmsg)
        {
            errmsg = "";
            publications.Clear();

            string sql = "SELECT DISTINCT PUB.Name FROM ChannelNames CN " +
                        "INNER JOIN PublicationChannels PC ON CN.ChannelID=PC.ChannelID " +
                        "INNER JOIN PublicationNames PUB ON PUB.PublicationID = PC.PublicationID " +
                        $"WHERE CN.ChannelID={channelID} " +
                        "ORDER BY PUB.Name ";

            SqlCommand command = new SqlCommand(sql, connection)
            {
                CommandType = CommandType.Text
            };

            SqlDataReader reader = null;

            try
            {
                if (connection.State == ConnectionState.Closed || connection.State == ConnectionState.Broken)
                    connection.Open();

                reader = command.ExecuteReader();

                while (reader.Read())
                {
                    publications.Add(reader.GetString(0));
                }
            }
            catch (Exception ex)
            {
                errmsg = ex.Message;
                return false;
            }
            finally
            {
                if (reader != null)
                    reader.Close();
                if (connection.State == ConnectionState.Open)
                    connection.Close();
                command.Dispose();
            }

            return true;
        }

        public bool GetPubDates(int publisherID, string publicationName, int channelID, ref List<DateTime> pubDates, out string errmsg)
        {
            errmsg = "";
            pubDates.Clear();

            string sql = "SELECT DISTINCT PubDate FROM PageTable P WITH (NOLOCK) " +
                        "INNER JOIN PublicationNames PUB ON PUB.PublicationID = P.PublicationID " +
                        "INNER JOIN ChannelStatus CS ON CS.MasterCopySeparationSet=P.MasterCopySeparationSet " +
                        $"WHERE P.Dirty = 0 AND DATEPART(year, P.PubDate) < 2100 AND PUB.PublisherID = {publisherID} ";
            if (publicationName != "" && publicationName != "All")
                sql += $"AND PUB.Name='{publicationName}' ";
            if (channelID > 0)
                sql += $"AND CS.CahnnelID={channelID} ";

            sql += "ORDER BY P.PubDate DESC";

            SqlCommand command = new SqlCommand(sql, connection)
            {
                CommandType = CommandType.Text
            };

            SqlDataReader reader = null;

            try
            {
                if (connection.State == ConnectionState.Closed || connection.State == ConnectionState.Broken)
                    connection.Open();

                reader = command.ExecuteReader();

                while (reader.Read())
                {
                    pubDates.Add(reader.GetDateTime(0));
                }
            }
            catch (Exception ex)
            {
                errmsg = ex.Message;
                return false;
            }
            finally
            {
                if (reader != null)
                    reader.Close();
                if (connection.State == ConnectionState.Open)
                    connection.Close();
                command.Dispose();
            }

            return true;
        }


        public bool GetPublicationsFromPublisher(int publisherID, ref List<string> publicationList, out string errmsg)
        {
            errmsg = "";
            publicationList.Clear();

            string sql = $"SELECT Name FROM PublicationNames WHERE PublisherID={publisherID}";

            SqlCommand command = new SqlCommand(sql, connection)
            {
                CommandType = CommandType.Text
            };

            SqlDataReader reader = null;

            try
            {
                if (connection.State == ConnectionState.Closed || connection.State == ConnectionState.Broken)
                    connection.Open();

                reader = command.ExecuteReader();

                while (reader.Read())
                {
                    publicationList.Add(reader.GetString(0));
                }
            }
            catch (Exception ex)
            {
                errmsg = ex.Message;
                return false;
            }
            finally
            {
                if (reader != null)
                    reader.Close();
                if (connection.State == ConnectionState.Open)
                    connection.Close();
                command.Dispose();
            }

            return true;
        }

        public bool GetReportData(DateTime pubDate, int publisherID, string publicationName, int channelID,ref List<Models.ReportItem> reportItems, out string errmsg)
        {
            errmsg = "";
            reportItems.Clear();

       /*
            string sql = "SELECT DISTINCT P.ProductionID, P.PubDate, PUB.[Name], CN.Name, COUNT(DISTINCT P.MasterCopySeparationSet), " +
                         "MAX(CS.ReleaseTime),MAX(P.InputTime),MAX(CS.ExportTime) ,CN.ChannelID,P.EditionID " +
                         "FROM PageTable P WITH (NOLOCK) " +
                         "INNER JOIN PublicationNames PUB ON PUB.PublicationID = P.PublicationID " +
                         "INNER JOIN ChannelStatus CS ON CS.MasterCopySeparationSet = P.MasterCopySeparationSet " +
                         "INNER JOIN ChannelNames CN ON CS.ChannelID = CN.ChannelID " +
                         $"WHERE P.PubDate = @PubDate AND PUB.PublisherID={publisherID}";

            if (publicationName != "" && publicationName != "All")
                sql += $" AND PUB.Name='{publicationName}' ";
            if (channelID != 0)
                sql += $" AND CS.ChannelID='{channelID}' ";

            sql += "GROUP BY P.ProductionID,P.PubDate,PUB.[Name],CN.Name,CN.ChannelID,P.EditionID ";
            sql += "ORDER BY P.PubDate,PUB.[Name],CN.Name,P.EditionID ";
            */
            SqlCommand command = new SqlCommand("spGetPDFHUBReportData", connection)
            {
                CommandType = CommandType.StoredProcedure,
                CommandTimeout = 600
                
            };

            command.Parameters.Clear();
            SqlParameter param = command.Parameters.Add("@PubDate", SqlDbType.DateTime);
            param.Value = pubDate;
            param = command.Parameters.Add("@PublisherID", SqlDbType.Int);
            param.Value = publisherID;
            param = command.Parameters.Add("@Publication", SqlDbType.VarChar,50);
            param.Value = publicationName;
            param = command.Parameters.Add("@ChannelID", SqlDbType.Int);
            param.Value = channelID;
            param = command.Parameters.Add("@EditionID", SqlDbType.Int);
            param.Value = 0;

            SqlDataReader reader = null;

            try
            {
                if (connection.State == ConnectionState.Closed || connection.State == ConnectionState.Broken)
                    connection.Open();

                reader = command.ExecuteReader();
                while (reader.Read())
                {
                    int f = 0;
                  
                    Models.ReportItem item = new Models.ReportItem()
                    {
                        ProductionID = reader.GetInt32(f++),
                        PubDate = reader.GetDateTime(f++),
                        Publication = reader.GetString(f++),
                        Channel = reader.GetString(f++),
                        Pages = reader.GetInt32(f++),
                        ReleaseTime = reader.GetDateTime(f++),
                        LastPageIn = reader.GetDateTime(f++),
                        LastPageSent = reader.GetDateTime(f++),
                        _ChannelID = reader.GetInt32(f++),
                        _EditionID = reader.GetInt32(f++)
                    };

                    // Only take first edition 
                    if (reportItems.FirstOrDefault(p => p.ProductionID == item.ProductionID && p.PubDate == item.PubDate && p._ChannelID == item._ChannelID) == null)
                        reportItems.Add(item);
                }
            }
            catch (Exception ex)
            {
                errmsg = ex.Message;
                return false;
            }
            finally
            {
                if (reader != null)
                    reader.Close();
                if (connection.State == ConnectionState.Open)
                    connection.Close();
                command.Dispose();

            }

            foreach (Models.ReportItem item in reportItems)
            {
                int pagesSent = 0;
                DateTime latestExportTime = DateTime.MinValue;
                if (GetPagesSentToChannel(item.ProductionID, item._ChannelID, item._EditionID, ref pagesSent, ref latestExportTime, out errmsg) == false)
                    return false;
                item.PagesSent = pagesSent;
                item.LastPageSent = latestExportTime;
            }

            return true;
        }

        public bool GetReportDataPageHistory(DateTime pubDate, int publisherID, string publicationName, int channelID, ref List<Models.ReportPageHistoryItem> reportItems, out string errmsg)
        {
            errmsg = "";
            reportItems.Clear();
            /*
            string sql = "SELECT DISTINCT P.ProductionID, P.PubDate, PUB.[Name], CN.Name,CN.ChannelID,P.EditionID, P.SectionID, P.PageIndex,P.PageName,SLI.Version, " +
                         "CS.ReleaseTime, " +
						 "ISNULL(SLI.EventTime, 0), " +
                         "MAX(ISNULL(SLO.EventTime, CS.ExportTime)) ,SLO.Version " +
                         "FROM PageTable P WITH (NOLOCK) " +
                         "INNER JOIN PublicationNames PUB WITH (NOLOCK) ON PUB.PublicationID = P.PublicationID " +
                         "INNER JOIN ChannelStatus CS WITH (NOLOCK) ON CS.MasterCopySeparationSet = P.MasterCopySeparationSet " +
                         "INNER JOIN ChannelNames CN WITH (NOLOCK) ON CS.ChannelID = CN.ChannelID " +
                         "LEFT OUTER JOIN ServiceLog SLI WITH (NOLOCK) ON SLI.Event = 10 AND SLI.Filename = p.FileName AND SLI.MiscInt = P.MasterCopySeparationSet " +
                         "LEFT OUTER JOIN ServiceLog SLO WITH (NOLOCK) ON SLO.Event = 180 AND SLO.Source = CN.Name AND SLO.MiscInt = P.MasterCopySeparationSet  AND SLO.ProductionID = P.ProductionID " +
                         $"WHERE P.PubDate = @PubDate AND PUB.PublisherID={publisherID} " +
                         "AND CS.ReleaseTime <=  CS.ExportTime AND CS.ReleaseTime <= SLO.EventTime " +
                         "AND SLO.EventTime > SLI.EventTime AND SLI.Version = SLO.Version ";
                        if (publicationName != "" && publicationName != "All")
                            sql += $" AND PUB.Name='{publicationName}' ";
                        if (channelID != 0)
                            sql += $" AND CS.ChannelID={channelID} ";

            sql += "GROUP BY P.ProductionID,P.PubDate,PUB.[Name],CN.Name,CN.ChannelID,P.EditionID,P.SectionID,CS.ReleaseTime,P.PageIndex,P.PageName,SLI.Version,SLO.Version, P.InputTime,CS.ExportTime,SLI.EventTime ";
            sql += "ORDER BY P.PubDate,PUB.[Name],CN.Name,P.EditionID,P.SectionID,P.PageIndex,SLI.Version,SLO.Version ";
            */
            SqlCommand command = new SqlCommand("spGetPDFHUBReportDataHistory", connection)
            {
                CommandType = CommandType.StoredProcedure,
                CommandTimeout = 600
            };

            command.Parameters.Clear();
            SqlParameter param = command.Parameters.Add("@PubDate", SqlDbType.DateTime);
            param.Value = pubDate;
            param = command.Parameters.Add("@PublisherID", SqlDbType.Int);
            param.Value = publisherID;
            param = command.Parameters.Add("@Publication", SqlDbType.VarChar, 50);
            param.Value = publicationName;
            param = command.Parameters.Add("@ChannelID", SqlDbType.Int);
            param.Value = channelID;
            param = command.Parameters.Add("@EditionID", SqlDbType.Int);
            param.Value = 0;

            SqlDataReader reader = null;

            try
            {
                if (connection.State == ConnectionState.Closed || connection.State == ConnectionState.Broken)
                    connection.Open();

                reader = command.ExecuteReader();
                while (reader.Read())
                {
                    int f = 0;

                    Models.ReportPageHistoryItem item = new Models.ReportPageHistoryItem()
                    {
                        ProductionID = reader.GetInt32(f++),
                        PubDate = reader.GetDateTime(f++),
                        Publication = reader.GetString(f++),
                        Channel = reader.GetString(f++),
                        _ChannelID = reader.GetInt32(f++),
                        _EditionID = reader.GetInt32(f++)
                    };
                    int sectionID = reader.GetInt32(f++);
                    int pageIndex = reader.GetInt32(f++);
                    string pageName = reader.GetString(f++);
                    item.PageName = string.Format("{0}-{1}", sectionID, pageName);
                    item.Version = reader.GetInt32(f++);
                    item.ReleaseTime = reader.GetDateTime(f++);
                    item.PageIn = reader.GetDateTime(f++);
                    item.PageSent = reader.GetDateTime(f++);

                    // Only take edition X_1 
                   /* if (item._EditionID == 111 || item._EditionID == 118 ||
                        item._EditionID == 119 || item._EditionID == 125 || 
                        item._EditionID == 128 || item._EditionID == 131 || 
                        item._EditionID == 134 || item._EditionID == 137 || 
                        item._EditionID == 140 || item._EditionID == 144)
                    */
                        reportItems.Add(item);

                    
                      
                }
            }
            catch (Exception ex)
            {
                errmsg = ex.Message;
                return false;
            }
            finally
            {
                if (reader != null)
                    reader.Close();
                if (connection.State == ConnectionState.Open)
                    connection.Close();

                command.Dispose();
            }

          
            return true;
        }

        public bool GetPagesSentToChannel(int productionID, int channelID, int editionID,  ref int pagesSent, ref DateTime latestExportTime, out string errmsg)
        {
            errmsg = "";
            pagesSent = 0;
            latestExportTime = DateTime.MinValue;

            string sql = "SELECT COUNT(DISTINCT CS.MasterCopySeparationSet),ISNULL(MAX(CS.ExportTime),'1900-01-01') FROM ChannelStatus CS " +
                         "INNER JOIN PageTable P WITH(NOLOCK) ON P.MasterCopySeparationSet = CS.MasterCopySeparationSet " +
                         $"WHERE P.ProductionID = {productionID} AND CS.ExportStatus >= 10 AND CS.ChannelID = {channelID} AND P.EditionID={editionID} AND P.Dirty=0";

            SqlCommand command = new SqlCommand(sql, connection)
            {
                CommandType = CommandType.Text
            };

            SqlDataReader reader = null;

            try
            {
                if (connection.State == ConnectionState.Closed || connection.State == ConnectionState.Broken)
                    connection.Open();

                reader = command.ExecuteReader();

                if (reader.Read())
                {
                    pagesSent = reader.GetInt32(0);
                    latestExportTime = reader.GetDateTime(1);
                }

            }
            catch (Exception ex)
            {
                errmsg = ex.Message;
                return false;
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                    connection.Close();
                command.Dispose();
            }

            return true;
        }



        #region Maint param functions

        public bool GetMaintParameters(ref int keepDaysProducts,ref  int keepDaysLogdata, ref bool useNewsPilotFeedback,
                                     ref string feedbackMessageFolder, ref string feedbackTemplateSuccess,
                                     ref string feedbackTemplateError, ref string feedbackFilename,
            out string errmsg)
        {
            errmsg = "";
            keepDaysProducts = 10;
            keepDaysLogdata = 10;
            string sql = $"SELECT TOP 1 DaysToKeepProducts,DaysToKeepLogdata,UseNewsPilotFeedback,FeedbackMessageFolder,FeedbackTemplateSuccess,FeedbackTemplateError,FeedbackFilename FROM MaintConfigurations";
            SqlCommand command = new SqlCommand(sql, connection)
            {
                CommandType = CommandType.Text
            };

            SqlDataReader reader = null;

            try
            {
                if (connection.State == ConnectionState.Closed || connection.State == ConnectionState.Broken)
                    connection.Open();

                reader = command.ExecuteReader();

                if (reader.Read())
                {
                    keepDaysProducts = reader.GetInt32(0);
                    keepDaysLogdata = reader.GetInt32(1);
                    useNewsPilotFeedback = reader.GetInt32(2) > 0;
                    feedbackMessageFolder = reader.GetString(3);
                    feedbackTemplateSuccess = reader.GetString(4);
                    feedbackTemplateError = reader.GetString(5);
                    feedbackFilename = reader.GetString(6);

                }
            }
            catch (Exception ex)
            {
                errmsg = ex.Message;
                return false;
            }
            finally
            {
                if (reader != null)
                    reader.Close();
                if (connection.State == ConnectionState.Open)
                    connection.Close();
                command.Dispose();
            }

            return true;
        }

        public bool UpdateMaintParameters(int keepDaysProducts, int keepDaysLogdata, bool userNewspilotFeedback,
                                            string feedbackMessageFolder, string feedbackTemplateSuccess,
                                            string feedbackTemplateError, string feedbackFilename, out string errmsg)
        {
            errmsg = "";
            string sql = $"UPDATE MaintConfigurations SET [DaysToKeepProducts]={keepDaysProducts},[DaysToKeepLogdata]={keepDaysLogdata}, UseNewsPilotFeedback={(userNewspilotFeedback?1:0)}, FeedbackMessageFolder='{feedbackMessageFolder}', FeedbackTemplateSuccess='{feedbackTemplateSuccess}', FeedbackTemplateError='{feedbackTemplateError}, FeedbackFilename='{feedbackFilename}'' ";
            SqlCommand command = new SqlCommand(sql, connection)
            {
                CommandType = CommandType.Text
            };
            try
            {
                if (connection.State == ConnectionState.Closed || connection.State == ConnectionState.Broken)
                    connection.Open();

                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                errmsg = ex.Message;
                return false;
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                    connection.Close();
                command.Dispose();
            }

            return true;
        }

        #endregion

        #region retry functions

        public bool RetryFile(Models.ServiceType type, string fileName, string queue, out string errmsg)
        {
            errmsg = "";
            string sql = $"INSERT INTO RetryQueue (Type, FileName, QueueName, EventTime) VALUES ({(int)type},'{fileName}','{queue}',GETDATE())";

            SqlCommand command = new SqlCommand(sql, connection)
            {
                CommandType = CommandType.Text
            };
            try
            {
                if (connection.State == ConnectionState.Closed || connection.State == ConnectionState.Broken)
                    connection.Open();

               command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                errmsg = ex.Message;
                return false;
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                    connection.Close();
                command.Dispose();
            }

            return true;

        }
        #endregion

        #region Get active productions

        public bool GetLastPageStat(ref DateTime lastPageIn, ref DateTime lastPageOut, ref int pageCount, out string errmsg)
        {
            errmsg = "";

            string sql = "SELECT MAX(P.InputTime), MAX(CS.ExportTime),COUNT(DISTINCT P.MasterCopySeparationSet) FROM PageTable P WITH (NOLOCK) " +
                         "INNER JOIN ChannelStatus CS WITh (NOLOCK) ON CS.MasterCopySeparationSet=p.MasterCopySeparationSet " +
                         "WHERE P.Dirty=0";
            SqlCommand command = new SqlCommand(sql, connection)
            {
                CommandType = CommandType.Text
            };

            SqlDataReader reader = null;

            try
            {
                if (connection.State == ConnectionState.Closed || connection.State == ConnectionState.Broken)
                    connection.Open();

                reader = command.ExecuteReader();

                if (reader.Read())
                {

                    lastPageIn = reader.GetDateTime(0);
                    lastPageOut = reader.GetDateTime(1);
                    pageCount = reader.GetInt32(2);
                }
            }
            catch (Exception ex)
            {
                errmsg = ex.Message;
                return false;
            }
            finally
            {
                if (reader != null)
                    reader.Close();
                if (connection.State == ConnectionState.Open)
                    connection.Close();
                command.Dispose();
            }

            return true;
        }

        public bool GetProductions(ref List<Models.Production> productions, DateTime fromDate, DateTime toDate, int publisherID, out string errmsg)
        {
            errmsg = "";
            productions.Clear();

            SqlCommand command = new SqlCommand("spGetActiveProductionsForReport", connection)
            {
                CommandType = CommandType.StoredProcedure
            };

            command.Parameters.Clear();
            SqlParameter param = command.Parameters.Add("@PubDateFrom", SqlDbType.DateTime);
            param.Value = fromDate;
            param = command.Parameters.Add("@PubDateTo", SqlDbType.DateTime);
            param.Value = toDate;
            param = command.Parameters.Add("@PublisherID", SqlDbType.Int);
            param.Value = publisherID;


            SqlDataReader reader = null;

            try
            {
                if (connection.State == ConnectionState.Closed || connection.State == ConnectionState.Broken)
                    connection.Open();

                reader = command.ExecuteReader();

                while (reader.Read())
                {
                    int f = 0;
                    int productionID = reader.GetInt32(f++);
                    DateTime pubDate = reader.GetDateTime(f++);
                    string publication = reader.GetString(f++);
                    string alias = reader.GetString(f++);
                    string edition = reader.GetString(f++);
                    string section = reader.GetString(f++);
                    int pages = reader.GetInt32(f++);
                    int pagesReceived = reader.GetInt32(f++);
                    string channel = reader.GetString(f++);
                    DateTime releaseTime = reader.GetDateTime(f++);
                    int released = reader.GetInt32(f++);
                    Models.Production production = productions.FirstOrDefault(p => p.ProductionID == productionID);
                    if (production == null)
                    {
                        production = new Models.Production()
                        {
                            ProductionID = productionID,
                            PubDate = pubDate,
                            Publication = publication,
                            Alias = alias,
                            ReleaseTime =releaseTime,
                            Released = released > 0//releaseTime < DateTime.Now
                        };
                        production.Editions.Add(new Models.ProductionEdition()
                        {
                            Edition = edition,
                            Pages = pages,
                            PagesReceived = pagesReceived
                        });
                        production.Sections.Add(new Models.ProductionSection()
                        {
                            Section = section,
                            Pages = pages,
                            PagesReceived = pagesReceived
                        });
                        production.Channels.Add(channel);
                        productions.Add(production);
                    }
                    else
                    {
                        bool found = false;
                        foreach (Models.ProductionEdition ed in production.Editions)
                        {
                            if (ed.Edition == edition)
                            {
                                found = true;
                                break;
                            }
                        }
                        if (found == false)
                        {
                            production.Editions.Add(new Models.ProductionEdition()
                            {
                                Edition = edition,
                                Pages = pages,
                                PagesReceived = pagesReceived
                            });
                        }


                        found = false;
                        foreach (Models.ProductionSection sec in production.Sections)
                        {
                            if (sec.Section == section)
                            {
                                found = true;
                                break;
                            }
                        }
                        if (found == false)
                        {
                            production.Sections.Add(new Models.ProductionSection()
                            {
                                Section = section,
                                Pages = pages,
                                PagesReceived = pagesReceived
                            });
                        }

                        if (production.Channels.Contains(channel) == false)
                            production.Channels.Add(channel);
                    }
                    
                }
            }
            catch (Exception ex)
            {
                errmsg = ex.Message;
                return false;
            }
            finally
            {
                if (reader != null)
                    reader.Close();
                if (connection.State == ConnectionState.Open)
                    connection.Close();
                command.Dispose();
            }


            return true;
        }

        public bool GetChannelsForProduction(int productionID, ref string prodName, ref List<int> channelIDForProduct, out string errmsg)
        {
            errmsg = "";
            prodName = "";
            channelIDForProduct.Clear();
            string sql = $"SELECT DISTINCT PUB.InputAlias,P.PubDate,CS.ChannelID FROM PageTable P WITH (NOLOCK) INNER JOIN PublicationNames PUB ON P.PublicationID=PUB.PublicationID INNER JOIN ChannelStatus CS WITH (NOLOCK) ON CS.MasterCopySeparationSet=P.MasterCopySeparationSet WHERE P.ProductionID={productionID} ORDER BY CS.ChannelID";
            SqlCommand command = new SqlCommand(sql, connection)
            {
                CommandType = CommandType.Text
            };

            SqlDataReader reader = null;

            try
            {
                if (connection.State == ConnectionState.Closed || connection.State == ConnectionState.Broken)
                    connection.Open();

                reader = command.ExecuteReader();

                while (reader.Read())
                {
                    string s  = reader.GetString(0);

                    DateTime dt = reader.GetDateTime(1);
                    prodName = string.Format("{0:0000}-{1:00}-{2:00} {3}", dt.Year, dt.Month, dt.Day, s);
                    channelIDForProduct.Add(reader.GetInt32(2));
                }
            }
 
            catch (Exception ex)
            {
                errmsg = ex.Message;
                return false;
            }
            finally
            {
                if (reader != null)
                    reader.Close();
                if (connection.State == ConnectionState.Open)
                    connection.Close();
                command.Dispose();
            }

            return true;
        }


        public bool GetChannelsForPage(int masterCopySeparationSet, ref List<int> channelIDForPage, out string errmsg)
        {
            errmsg = "";

            channelIDForPage.Clear();
            string sql = $"SELECT DISTINCT CS.ChannelID FROM PageTable P WITH (NOLOCK)  INNER JOIN ChannelStatus CS WITH (NOLOCK) ON CS.MasterCopySeparationSet=P.MasterCopySeparationSet WHERE P.MasterCopySeparationSet={masterCopySeparationSet} ORDER BY CS.ChannelID";
            SqlCommand command = new SqlCommand(sql, connection)
            {
                CommandType = CommandType.Text
            };

            SqlDataReader reader = null;

            try
            {
                if (connection.State == ConnectionState.Closed || connection.State == ConnectionState.Broken)
                    connection.Open();

                reader = command.ExecuteReader();

                while (reader.Read())
                {
                    channelIDForPage.Add(reader.GetInt32(0));
                }
            }

            catch (Exception ex)
            {
                errmsg = ex.Message;
                return false;
            }
            finally
            {
                if (reader != null)
                    reader.Close();
                if (connection.State == ConnectionState.Open)
                    connection.Close();
                command.Dispose();
            }

            return true;
        }

        public bool GetProductionName(int productionID, ref string prodName, out string errmsg)
        {
            errmsg = "";
            prodName = "";
         
            string sql = $"SELECT DISTINCT PUB.InputAlias,P.PubDate FROM PageTable P WITH (NOLOCK) INNER JOIN PublicationNames PUB ON P.PublicationID=PUB.PublicationID  WHERE P.ProductionID={productionID}";
            SqlCommand command = new SqlCommand(sql, connection)
            {
                CommandType = CommandType.Text
            };

            SqlDataReader reader = null;

            try
            {
                if (connection.State == ConnectionState.Closed || connection.State == ConnectionState.Broken)
                    connection.Open();

                reader = command.ExecuteReader();

                if (reader.Read())
                {
                    string s = reader.GetString(0);

                    DateTime dt = reader.GetDateTime(1);
                    prodName = string.Format("{0:0000}-{1:00}-{2:00} {3}", dt.Year, dt.Month, dt.Day, s);
                   
                }
            }

            catch (Exception ex)
            {
                errmsg = ex.Message;
                return false;
            }
            finally
            {
                if (reader != null)
                    reader.Close();
                if (connection.State == ConnectionState.Open)
                    connection.Close();
                command.Dispose();
            }

            return true;
        }

        public bool GetChannelDetailsForProduction(int productionID, ref List<Models.ChannelProgress> channelProgressList, out string errmsg)
        {
            channelProgressList.Clear();
            errmsg = "";

            string sql = "spGetChannelDetailsForProductionReport";

/*            string sql = "SELECT CS.ChannelID,CN.Name,COUNT(DISTINCT CS.MasterCopySeparationSet),ISNULL(COUNT(DISTINCT CS1.MasterCopySeparationSet),0),ISNULL(COUNT(DISTINCT CS2.MasterCopySeparationSet),0),CN.ChannelNameAlias,MIN(cs.ExportTime),MAX(cs.ExportTime),CN.MergedPDF " +
                        "FROM ChannelStatus CS WITH (NOLOCK) " +
                        "INNER JOIN ChannelNames CN WITH (NOLOCK) ON CN.ChannelID = CS.ChannelID " +
                        "INNER JOIN PageTable P WITH (NOLOCK) ON P.MasterCopySeparationSet = CS.MasterCopySeparationSet " +
                        "INNER JOIN EditionNames ED WITH (NOLOCK) ON P.EditionID=ED.EditionID " +
                        "LEFT JOIN ChannelStatus CS1 WITH (NOLOCK) ON CS1.ExportStatus = 10 AND CS1.MasterCopySeparationSet = CS.MasterCopySeparationSet AND CS1.ChannelID = CS.ChannelID " +
                         "LEFT JOIN ChannelStatus CS2 WITH (NOLOCK) ON CS2.ExportStatus = 6 AND CS2.MasterCopySeparationSet = CS.MasterCopySeparationSet AND CS2.ChannelID = CS.ChannelID " +
                        $"WHERE P.ProductionID = {productionID} AND P.UniquePage = 1 " +
                         "AND(CN.EditionsToGenerate = 0 OR " +
                         " (CN.EditionsToGenerate = 1 AND SUBSTRING(ED.Name, 3, 1) = '1') " +
                            "OR (CN.EditionsToGenerate = 2 AND(SUBSTRING(ED.Name, 3, 1) = '1' OR SUBSTRING(ED.Name, 3, 1) = '2')) " + 
                            "OR (CN.EditionsToGenerate = 3 AND(SUBSTRING(ED.Name, 3, 1) = '1' OR SUBSTRING(ED.Name, 3, 1) = '2' OR SUBSTRING(ED.Name, 3, 1) = '3')) " +
                            "OR (CN.EditionsToGenerate = 4 AND(SUBSTRING(ED.Name, 3, 1) = '1' OR SUBSTRING(ED.Name, 3, 1) = '2' OR SUBSTRING(ED.Name, 3, 1) = '3' OR SUBSTRING(ED.Name, 3, 1) = '4'))) " +
                        "GROUP BY CS.ChannelID,CN.Name,CN.ChannelNameAlias,CN.MergedPDF  " +
                        "ORDER BY CS.ChannelID,CN.Name,CN.ChannelNameAlias,CN.MergedPDF ";
                        */
            SqlCommand command = new SqlCommand(sql, connection)
            {
                //CommandType = CommandType.Text
                CommandType = CommandType.StoredProcedure, 
                CommandTimeout = 600
            };

            command.Parameters.AddWithValue("@ProductionID", productionID);
            
            SqlDataReader reader = null;

            try
            {
                if (connection.State == ConnectionState.Closed || connection.State == ConnectionState.Broken)
                    connection.Open();

                reader = command.ExecuteReader();

                while (reader.Read())
                {
                    Models.ChannelProgress p = new Models.ChannelProgress() { 
                   
                        ProductionID = productionID,
                        ChannelID = reader.GetInt32(0),
                        Name = reader.GetString(1),
                        Pages = reader.GetInt32(2),
                        PagesSent = reader.GetInt32(3),
                        PagesWithError = reader.GetInt32(4),
                        Alias = reader.GetString(5),
                        
                    };
                    DateTime dt = reader.GetDateTime(6);
                    p.FirstSent = dt.Year > 2000 ? Utils.Time2StringShort(dt) : "";
                    dt = reader.GetDateTime(7);
                    p.LastSent = dt.Year > 2000 ? Utils.Time2StringShort(dt) : "";
                    p.MergedPDF = reader.GetInt32(8);
                    channelProgressList.Add(p);

                }
            }

            catch (Exception ex)
            {
                errmsg = ex.Message;
                return false;
            }
            finally
            {
                if (reader != null)
                    reader.Close();
                if (connection.State == ConnectionState.Open)
                    connection.Close();
                command.Dispose();
            }

            foreach (Models.ChannelProgress p in channelProgressList)
            {
                string str = "";
                GetMissingExportPages(productionID, p.ChannelID, ref str, out errmsg);
                if (str != "")
                    p.PageList = str;
            }

            return true;
        }

        public bool GetMissingExportPages(int productionID, int channelID, ref string pagesMissing, out string errmsg)
        {
            pagesMissing = "";
            errmsg = "";
            string sql = "SELECT DISTINCT P.PageName,CS.ExportStatus,P.PageIndex FROM PageTable P WITH (NOLOCK) INNER JOIN ChannelStatus CS WITH (NOLOCK) ON P.MasterCopySeparationSet=CS.MasterCopySeparationSet " +
                $"WHERE P.ProductionID= {productionID} AND CS.ChannelID={channelID} ORDER BY P.PageIndex";

            SqlCommand command = new SqlCommand(sql, connection)
            {
                CommandType = CommandType.Text
            };

            SqlDataReader reader = null;

            try
            {
                if (connection.State == ConnectionState.Closed || connection.State == ConnectionState.Broken)
                    connection.Open();

                reader = command.ExecuteReader();

                while (reader.Read())
                {
                    if (pagesMissing != "")
                        pagesMissing += ",";
                   
                    string s = reader.GetString(0);
                    if (s.Length == 1)
                        s = "0" + s;
                    int status = reader.GetInt32(1);
                    if (status == 6 || status == 16)
                        pagesMissing += "!" + s;
                    else
                        pagesMissing += status < 10 ? "*" + s : s;


                }
            }

            catch (Exception ex)
            {
                errmsg = ex.Message;
                return false;
            }
            finally
            {
                if (reader != null)
                    reader.Close();
                if (connection.State == ConnectionState.Open)
                    connection.Close();
                command.Dispose();
            }

            return true;
        }

        public bool MasterCopySeparationSetPage(ref Models.Page page, out string errmsg)
        {
            errmsg = "";
            string sql = $"SELECT DISTINCT PageName, SectionID, Version, Status, ProofStatus FROM PageTable WITH (NOLOCK) WHERE MasterCopySeparationSet={page.MasterCopySeparationSet} AND Dirty=0";

            SqlCommand command = new SqlCommand(sql, connection)
            {
                CommandType = CommandType.Text
            };

            SqlDataReader reader = null;

            try
            {
                if (connection.State == ConnectionState.Closed || connection.State == ConnectionState.Broken)
                    connection.Open();

                reader = command.ExecuteReader();

                if (reader.Read())
                {
                    page.PageName = reader.GetString(0);
                    page.Section = reader.GetInt32(1).ToString(); // OK..!
                    page.Version = reader.GetInt32(2);
                    page.Status = reader.GetInt32(3);
                    page.ProofStatus = reader.GetInt32(4);
                }
            }

            catch (Exception ex)
            {
                errmsg = ex.Message;
                return false;
            }
            finally
            {
                if (reader != null)
                    reader.Close();
                if (connection.State == ConnectionState.Open)
                    connection.Close();
                command.Dispose();
            }

            return true;
        }


        public bool GetHiresPath(int masterCopySeparationSet, ref string hiresPath, out string errmsg)
        {
            errmsg = "";
            hiresPath = "";

            string sql = $"SELECT TOP 1 G.ServerFilePath, P.FileName FROM PageTable P WITH (NOLOCK) INNER JOIN GeneralPreferences G ON G.MainLocationID=1 WHERE P.MasterCopySeparationSet={masterCopySeparationSet} AND P.UniquePage=1 AND P.Dirty=0";

            SqlCommand command = new SqlCommand(sql, connection)
            {
                CommandType = CommandType.Text
            };

            SqlDataReader reader = null;

            try
            {
                if (connection.State == ConnectionState.Closed || connection.State == ConnectionState.Broken)
                    connection.Open();

                reader = command.ExecuteReader();

                if (reader.Read())
                {
                    string folder = reader.GetString(0);
                    string file = reader.GetString(1);
                    if (file != "")
                        hiresPath = folder + @"\" + Path.GetFileNameWithoutExtension(file) + "#" + masterCopySeparationSet.ToString() + ".pdf";
                }
            }

            catch (Exception ex)
            {
                errmsg = ex.Message;
                return false;
            }
            finally
            {
                if (reader != null)
                    reader.Close();
                if (connection.State == ConnectionState.Open)
                    connection.Close();
                command.Dispose();
            }

            return true;
        }

        public bool GetPagesForProduction(int productionID, ref List<Models.Page> pages, out string errmsg)
        {
            errmsg = "";
            pages.Clear();

            string sql = $"SELECT DISTINCT P.MasterCopySeparationSet, P.PageName, P.Status, P.ProofStatus, SEC.Name, P.EditionID, P.FileName, P.SectionID,P.PageIndex FROM PageTable P WITH (NOLOCK) INNER JOIN SectionNames SEC ON Sec.SectionID=P.SectionID WHERE ProductionID={productionID} AND P.UniquePage=1 AND P.Dirty=0 ORDER BY P.EditionID,P.SectionID,P.PageIndex";

            SqlCommand command = new SqlCommand(sql, connection)
            {
                CommandType = CommandType.Text
            };

            SqlDataReader reader = null;

            try
            {
                if (connection.State == ConnectionState.Closed || connection.State == ConnectionState.Broken)
                    connection.Open();

                reader = command.ExecuteReader();

                while (reader.Read())
                {
                    Models.Page page = new Models.Page()
                    {
                        MasterCopySeparationSet = reader.GetInt32(0),
                        PageName = reader.GetString(1),
                        Status = reader.GetInt32(2),
                        ProofStatus = reader.GetInt32(3),
                        Section = reader.GetString(4),
                        _EditionID = reader.GetInt32(5),
                        FileName = reader.GetString(6),
                    };

                    // Only first edition..!
                    if (pages.FirstOrDefault(p => p.PageName == page.PageName && p.Section == page.Section) == null)
                        pages.Add(page);
                }
            }

            catch (Exception ex)
            {
                errmsg = ex.Message;
                return false;
            }
            finally
            {
                if (reader != null)
                    reader.Close();
                if (connection.State == ConnectionState.Open)
                    connection.Close();
                command.Dispose();
            }

            return true;
        }

        public bool GetMasterCopySeparationSetsForProduction(int productionID, ref List<int> masterCopySeparationSetList, out string errmsg)
        {
            errmsg = "";
            masterCopySeparationSetList.Clear();

            string sql = $"SELECT DISTINCT MasterCopySeparationSet,EditionID,SectionID,PageIndex FROM PageTable WITH (NOLOCK) WHERE ProductionID={productionID} ORDER BY EditionID,SectionID,PageIndex";

            SqlCommand command = new SqlCommand(sql, connection)
            {
                CommandType = CommandType.Text
            };

            SqlDataReader reader = null;

            try
            {
                if (connection.State == ConnectionState.Closed || connection.State == ConnectionState.Broken)
                    connection.Open();

                reader = command.ExecuteReader();

                while (reader.Read())
                {
                    masterCopySeparationSetList.Add(reader.GetInt32(0));
                }
            }

            catch (Exception ex)
            {
                errmsg = ex.Message;
                return false;
            }
            finally
            {
                if (reader != null)
                    reader.Close();
                if (connection.State == ConnectionState.Open)
                    connection.Close();
                command.Dispose();
            }

            return true;

        }


        public bool GetMasterCopySeparationSetsForProductionNotSent(int productionID, int channelID, ref List<int> masterCopySeparationSetList, out string errmsg)
        {
            errmsg = "";
           // masterCopySeparationSetList.Clear();

            string sql = "SELECT DISTINCT P.MasterCopySeparationSet,P.EditionID,P.SectionID,P.PageIndex FROM PageTable P WITH (NOLOCK) " +
                         "INNER JOIN ChannelStatus CS WITH(NOLOCK) ON P.MasterCopySeparationSet = CS.MasterCopySeparationSet " +
                        $"WHERE P.ProductionID ={ productionID} AND CS.ChannelID={channelID} AND CS.ExportStatus < 10 " +
                        "ORDER BY EditionID,SectionID,PageIndex";

            SqlCommand command = new SqlCommand(sql, connection)
            {
                CommandType = CommandType.Text
            };

            SqlDataReader reader = null;

            try
            {
                if (connection.State == ConnectionState.Closed || connection.State == ConnectionState.Broken)
                    connection.Open();

                reader = command.ExecuteReader();

                while (reader.Read())
                {
                    masterCopySeparationSetList.Add(reader.GetInt32(0));
                }
            }

            catch (Exception ex)
            {
                errmsg = ex.Message;
                return false;
            }
            finally
            {
                if (reader != null)
                    reader.Close();
                if (connection.State == ConnectionState.Open)
                    connection.Close();
                command.Dispose();
            }

            return true;

        }

        public bool SetBackStatusForPage(int masterCopySeparationSet, int status, out string errmsg)
        {
            errmsg = "";
            string sql = $"UPDATE PageTable  Set Status={status} WHERE MasterCopySeparationSet={masterCopySeparationSet} AND Status>{status} ";

            SqlCommand command = new SqlCommand(sql, connection)
            {
                CommandType = CommandType.Text,
                CommandTimeout = 300
            };

            try
            {
                if (connection.State == ConnectionState.Closed || connection.State == ConnectionState.Broken)
                    connection.Open();

                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                errmsg = ex.Message;
                return false;
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                    connection.Close();
                command.Dispose();
            }

            return true;

        }

        public bool ResendToChannel(int masterCopySeparationSet, int channelID, out string errmsg)
        {
            errmsg = "";
            string sql = $"UPDATE ChannelStatus Set ExportStatus=0 WHERE MasterCopySeparationSet={masterCopySeparationSet} AND ChannelID={channelID}";

            SqlCommand command = new SqlCommand(sql, connection)
            {
                CommandType = CommandType.Text,
                CommandTimeout = 300                
            };

            try
            {
                if (connection.State == ConnectionState.Closed || connection.State == ConnectionState.Broken)
                    connection.Open();

                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                errmsg = ex.Message;
                return false;
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                    connection.Close();
                command.Dispose();
            }

            return SetBackStatusForPage(masterCopySeparationSet, 30, out errmsg);
        }

        public bool ReProcessLowresPage(int masterCopySeparationSet, out string errmsg)
        {
            errmsg = "";

            string sql = $"UPDATE PageTable SET StatusPdfLowres=0,CheckSumPdfLowres=0 WHERE MasterCopySeparationSet={masterCopySeparationSet}";

            SqlCommand command = new SqlCommand(sql, connection)
            {
                CommandType = CommandType.Text,
                CommandTimeout = 300
            };

            try
            {
                if (connection.State == ConnectionState.Closed || connection.State == ConnectionState.Broken)
                    connection.Open();

                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                errmsg = ex.Message;
                return false;
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                    connection.Close();
                command.Dispose();
            }

            return true;
        }


        public bool ReProcessPrintPage(int masterCopySeparationSet, out string errmsg)
        {
            errmsg = "";

            string sql = $"UPDATE PageTable SET StatusPdfPrint=0,CheckSumPdfPrint=0 WHERE MasterCopySeparationSet={masterCopySeparationSet}";

            SqlCommand command = new SqlCommand(sql, connection)
            {
                CommandType = CommandType.Text,
                CommandTimeout = 300
            };

            try
            {
                if (connection.State == ConnectionState.Closed || connection.State == ConnectionState.Broken)
                    connection.Open();

                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                errmsg = ex.Message;
                return false;
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                    connection.Close();
                command.Dispose();
            }

            return true;
        }

        // reprocessMissing = 1 : All
        // reprocessMissing = 2 : Misssing
        public bool ResendChannelsForProduction(int productionID, List<int> channelIDList, bool resendMissingOnly, int reprocessPages, out string errmsg)
        {
            errmsg = "";
            List<int> masterSetList = new List<int>();
                      
            if (resendMissingOnly || reprocessPages == 2)
            {
                foreach (int channelID in channelIDList)
                {
                    if (GetMasterCopySeparationSetsForProductionNotSent(productionID, channelID, ref masterSetList, out errmsg) == false)
                        return false;
                }
            }
            else
            {
                if (GetMasterCopySeparationSetsForProduction(productionID, ref masterSetList, out errmsg) == false)
                    return false;
            }

           

            foreach (int channelID in channelIDList)
            {
                foreach (int masterCopySeparationSet in masterSetList)
                {
                    if (reprocessPages > 0)
                    {
                        int pdfType = GetChannelType(channelID, out errmsg);
                        if (pdfType == 2)
                            ReProcessPrintPage(masterCopySeparationSet, out errmsg);
                        else
                            ReProcessLowresPage(masterCopySeparationSet, out errmsg);
                    }

                    if (ResendToChannel(masterCopySeparationSet, channelID, out errmsg) == false)
                        return false;
                }
            }

            // Check if we need to re-process for some reason..

            // This is for lowres!
            foreach (int masterCopySeparationSet in masterSetList)
            {
                int lowresStatus = 0;
                int crclowres = 0;
                int inputStatus = 0;    
                int inputAgeMin = 0;
                if (GetProcessStatusForPage(masterCopySeparationSet, ref inputStatus, ref lowresStatus, ref crclowres, ref inputAgeMin, out errmsg))
                {
                    if (inputStatus >= 10 && inputAgeMin > 10 && (lowresStatus < 0 || crclowres == 0))
                        ReProcessLowresPage(masterCopySeparationSet, out errmsg);
                }

            }

            return true;
        }

        public bool GetProcessStatusForPage(int masterCopySeparationSet, ref int inputStatus, ref int lowresStatus, ref int crclowres, ref int inputAgeMin, out string errmsg)
        {
            errmsg = "";
            inputStatus = 0;
            lowresStatus = 0;
            crclowres = 0;
            inputAgeMin = 0;

            string sql = $"SELECT TOP 1 Status,StatusPdfLowres,CheckSumPdfLowres,DATEDIFF(minute,InputTime,GETDATE())  FROM PageTable WITH (NOLOCK) WHERE MasterCopySeparationSet = {masterCopySeparationSet}";

            SqlCommand command = new SqlCommand(sql, connection)
            {
                CommandType = CommandType.Text
            };

            SqlDataReader reader = null;

            try
            {
                if (connection.State == ConnectionState.Closed || connection.State == ConnectionState.Broken)
                    connection.Open();

                reader = command.ExecuteReader();

                if (reader.Read())
                {
                    inputStatus = reader.GetInt32(0);
                    lowresStatus = reader.GetInt32(1);
                    crclowres = reader.GetInt32(2);
                    inputAgeMin = reader.GetInt32(3);
                }
            }

            catch (Exception ex)
            {
                errmsg = ex.Message;
                return false;
            }
            finally
            {
                if (reader != null)
                    reader.Close();
                if (connection.State == ConnectionState.Open)
                    connection.Close();
                command.Dispose();
            }

            return true;
        }

        public bool SetChannelsForProduction(int productionID, List<int> channelIDList, out string errmsg)
        {
            string dummyStr = "";
            List<int> existingChannelIDList = new List<int>();

            if (GetChannelsForProduction(productionID, ref dummyStr, ref existingChannelIDList, out errmsg) == false)
                return false;

            // 1 create new channels for production
            foreach (int channelID in channelIDList)
            {
                if (existingChannelIDList.Contains(channelID))
                    continue;

                // Add this channel to production
                if (AddChannelToProduction(productionID, channelID, out errmsg) == false)
                    return false;
            }

            // 2: Remove channels not used anymore
            foreach (int existingChannelID in existingChannelIDList)
            {
                if (channelIDList.Contains(existingChannelID) == false)
                    if (RemoveChannelFromProduction(productionID, existingChannelID, out errmsg) == false)
                        return false;
            }

            return true;
        }

        private bool AddChannelToProduction(int productionID, int channelID, out string errmsg)
        {
            errmsg = "";

            SqlCommand command = new SqlCommand("spAddChannelToProduction", connection)
            {
                CommandType = CommandType.StoredProcedure
            };

            command.Parameters.Clear();
            SqlParameter param = command.Parameters.Add("@ProductionID", SqlDbType.Int);
            param.Value = productionID;
            param = command.Parameters.Add("@ChannelID", SqlDbType.Int);
            param.Value = channelID;

            try
            {
                if (connection.State == ConnectionState.Closed || connection.State == ConnectionState.Broken)
                    connection.Open();

                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                errmsg = ex.Message;
                return false;
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                    connection.Close();
                command.Dispose();
            }

            return true;
        }

        private bool RemoveChannelFromProduction(int productionID, int channelID, out string errmsg)
        {
            errmsg = "";

            string sql = $"DELETE FROM ChannelStatus WHERE ChannelID={channelID} AND MasterCopySeparationSet IN (SELECT DISTINCT MasterCopySeparationSet FROM PageTable WITH (NOLOCK) WHERE ProductionID={productionID})";
            SqlCommand command = new SqlCommand(sql, connection)
            {
                CommandType = CommandType.Text
            };
            try
            {
                if (connection.State == ConnectionState.Closed || connection.State == ConnectionState.Broken)
                    connection.Open();

                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                errmsg = ex.Message;
                return false;
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                    connection.Close();
                command.Dispose();
            }
            return true;
        }

        public bool GetProductionParamters(int productionID, ref DateTime pubDate, ref int publicationID, ref int editionID, out string errmsg)
        {
            errmsg = "";
            publicationID = 0;
            editionID = 0;
            pubDate = DateTime.MinValue;

            string sql = $"SELECT TOP 1 PublicationID,PubDate,EditionID FROM PageTable WITH (NOLOCK) WHERE ProductionID={productionID} ORDER BY EditionID";

            SqlCommand command = new SqlCommand(sql, connection)
            {
                CommandType = CommandType.Text
            };

            SqlDataReader reader = null;

            try
            {
                if (connection.State == ConnectionState.Closed || connection.State == ConnectionState.Broken)
                    connection.Open();

                reader = command.ExecuteReader();

                if  (reader.Read())
                {
                    publicationID = reader.GetInt32(0);
                    pubDate = reader.GetDateTime(1);
                    editionID = reader.GetInt32(2);

                }
            }

            catch (Exception ex)
            {
                errmsg = ex.Message;
                return false;
            }
            finally
            {
                if (reader != null)
                    reader.Close();
                if (connection.State == ConnectionState.Open)
                    connection.Close();
                command.Dispose();
            }

            return true;

        }

        public bool ApproveProduction(int productionID, int approval, out string errmsg)
        {
            errmsg = "";

            string sql = $"UPDATE PageTable SET Approved={approval}  WHERE ProductionID={productionID}";

            SqlCommand command = new SqlCommand(sql, connection)
            {
                CommandType = CommandType.Text
            };

            try
            {
                if (connection.State == ConnectionState.Closed || connection.State == ConnectionState.Broken)
                    connection.Open();

                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                errmsg = ex.Message;
                return false;
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                    connection.Close();
                command.Dispose();
            }
            return true;
        }

        public bool ReleaseProduction(int productionID, int channelID, out string errmsg)
        {
            errmsg = "";

            if (ApproveProduction(productionID, -1, out errmsg) == false)
                return false;

            string sql = $"UPDATE ChannelStatus SET Released=1 WHERE ProductionID={productionID}";
            if (channelID > 0)
                sql += $" AND ChannelID={channelID}";

            SqlCommand command = new SqlCommand(sql, connection)
            {
                CommandType = CommandType.Text
            };

            try
            {
                if (connection.State == ConnectionState.Closed || connection.State == ConnectionState.Broken)
                    connection.Open();

                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                errmsg = ex.Message;
                return false;
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                    connection.Close();
                command.Dispose();
            }
            return true;

        }

        public bool IssueTriggerForProduction(int productionID, int channelID, out string errmsg)
        {
         
            DateTime pubDate = DateTime.MinValue;
            int publicationID = 0;
            int editionID = 0;
            if (GetProductionParamters(productionID, ref pubDate, ref publicationID, ref editionID, out errmsg) == false)
            {
                Utils.WriteLog(false, "ERROR: db.GetProductionParamters() - " + errmsg);
                return false;
            }


            string sql = "INSERT INTO CustomMessageQueue (PublicationID,PubDate,EditionID,SectionID,LocationID,MessageType,EventTime,MiscInt,MiscString) VALUES (@PublicationID,@PubDate,@EditionID,@SectionID,0,1,GETDATE(),@Version,@MiscString)";
           
            SqlCommand command = new SqlCommand(sql, connection)
            {
                CommandType = CommandType.Text
            };

            command.Parameters.Add("@PublicationID", SqlDbType.Int).Value = publicationID;
            command.Parameters.Add("@PubDate", SqlDbType.DateTime).Value = pubDate;
            command.Parameters.Add("@EditionID", SqlDbType.Int).Value = editionID;
            command.Parameters.Add("@SectionID", SqlDbType.Int).Value = 0;
            command.Parameters.Add("@Version", SqlDbType.Int).Value = 9;
            command.Parameters.Add("@MiscString", SqlDbType.VarChar, 50).Value = channelID.ToString();

            try
            {
                if (connection.State == ConnectionState.Closed || connection.State == ConnectionState.Broken)
                    connection.Open();

                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                errmsg = ex.Message;
                return false;
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                    connection.Close();
                command.Dispose();
            }
            return true;
        }

        public bool GetLogFiles(Models.ServiceType type, int instanceNumber,  ref string logFile1, ref string logFile2, ref string logFile3, out string errmsg   )
        {
            errmsg = "";
            logFile1 = "";
            logFile2 = "";
            logFile3 = "";

            string sql = $"SELECT LogFile,LogFile2,LogFile3 FROM ServiceConfigurations WHERE ServiceType = {(int)type} AND InstanceNumber={instanceNumber}";

            SqlCommand command = new SqlCommand(sql, connection)
            {
                CommandType = CommandType.Text
            };

            SqlDataReader reader = null;

            try
            {
                if (connection.State == ConnectionState.Closed || connection.State == ConnectionState.Broken)
                    connection.Open();

                reader = command.ExecuteReader();

                if (reader.Read())
                {
                    logFile1 = reader.GetString(0);
                    logFile2 = reader.GetString(1);
                    logFile3 = reader.GetString(2);
                }
            }

            catch (Exception ex)
            {
                errmsg = ex.Message;
                return false;
            }
            finally
            {
                if (reader != null)
                    reader.Close();
                if (connection.State == ConnectionState.Open)
                    connection.Close();
                command.Dispose();
            }

            return true;

        }

        public bool GetConvertQueueFilesLog(ref List<Models.FileLog> fileLogs, out string errmsg)
        {
            errmsg = "";
            fileLogs.Clear();

            SqlCommand command = new SqlCommand("spConvertQueue", connection)
            {
                CommandType = CommandType.StoredProcedure
            };

            SqlDataReader reader = null;

            try
            {
                if (connection.State == ConnectionState.Closed || connection.State == ConnectionState.Broken)
                    connection.Open();

                reader = command.ExecuteReader();

                while (reader.Read())
                {
                    fileLogs.Add(new Models.FileLog()
                    {
                        Queue = reader.GetString(0),
                        FileName = reader.GetString(1)
                    });
                }
            }

            catch (Exception ex)
            {
                errmsg = ex.Message;
                return false;
            }
            finally
            {
                if (reader != null)
                    reader.Close();
                if (connection.State == ConnectionState.Open)
                    connection.Close();
                command.Dispose();
            }

            return true;

        }

        public bool GetTransmitQueueFilesLog(ref List<Models.FileLog> fileLogs, out string errmsg)
        {
            errmsg = "";
            fileLogs.Clear();

            SqlCommand command = new SqlCommand("spTransmitLookUpNextJob", connection)
            {
                CommandType = CommandType.StoredProcedure
            };

            command.Parameters.Add("@LocationID", SqlDbType.Int).Value = 1;
            command.Parameters.Add("@TransmitAfterProof", SqlDbType.Int).Value = 1;
            command.Parameters.Add("@TransmitAfterApproval", SqlDbType.Int).Value = 1;
            command.Parameters.Add("@HandleErrors", SqlDbType.Int).Value = 1;
            command.Parameters.Add("@ExcludeMerge", SqlDbType.Int).Value = 1;


            SqlDataReader reader = null;

            try
            {
                if (connection.State == ConnectionState.Closed || connection.State == ConnectionState.Broken)
                    connection.Open();

                reader = command.ExecuteReader();

                while (reader.Read())
                {
                    fileLogs.Add(new Models.FileLog()
                    {
                        FileName = reader.GetString(10),
                        Queue = reader.GetString(17)
                    });
                }
            }

            catch (Exception ex)
            {
                errmsg = ex.Message;
                return false;
            }
            finally
            {
                if (reader != null)
                    reader.Close();
                if (connection.State == ConnectionState.Open)
                    connection.Close();
                command.Dispose();
            }

            return true;

        }



        public bool GetQueueFilesLog(int type, ref List<Models.FileLog> fileLogs, out string errmsg)
        {
            errmsg = "";
            fileLogs.Clear();

            string sql = $"SELECT DISTINCT QueueName,FileName FROM [QueuedFiles] WHERE Type={type}";

            SqlCommand command = new SqlCommand(sql, connection)
            {
                CommandType = CommandType.Text
            };

            SqlDataReader reader = null;

            try
            {
                if (connection.State == ConnectionState.Closed || connection.State == ConnectionState.Broken)
                    connection.Open();

                reader = command.ExecuteReader();

                while (reader.Read())
                {
                    fileLogs.Add(new Models.FileLog() 
                    { 
                        Queue = reader.GetString(0), 
                        FileName = reader.GetString(1)
                    });
                }
            }

            catch (Exception ex)
            {
                errmsg = ex.Message;
                return false;
            }
            finally
            {
                if (reader != null)
                    reader.Close();
                if (connection.State == ConnectionState.Open)
                    connection.Close();
                command.Dispose();
            }

            return true;


        }
        #endregion
    }
}