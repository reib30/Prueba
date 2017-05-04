using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using System.Threading;
using log4net;
using SCADATool.BusinessLayer;

namespace SCADATool.Update
{
    public partial class frmUpdateWindfarm : FormEvent
    {
        #region [ Constants ]
        private const String C01_Txt_BackupRPT = "C01_Txt_BackupRPT";      
        private const String C01_Txt_BackupRPTOK = "C01_Txt_BackupRPTOK";        
        private const String C01_Txt_FailBackupRPT = "C01_Txt_FailBackupRPT";



        private const String TitlesForm_UpdateName = "TitlesForm_UpdateName";
        private const String U01_Txt_ErrVersionActualBigger = "U01_Txt_ErrVersionActualBigger";
        private const String U01_Txt_ErrAlreadyUpdated = "U01_Txt_ErrAlreadyUpdated";
        private const String U01_Txt_StartingRestore = "U01_Txt_StartingRestore";
        private const String U01_Txt_RestoreFinished = "U01_Txt_RestoreFinished";
        private const String U01_Txt_StartingUpdate = "U01_Txt_StartingUpdate";
        private const String U01_Txt_UpdateFinished = "U01_Txt_UpdateFinished";

        private const String C01_Txt_DefUseVar = "C01_Txt_DefUseVar";
        private const String C01_Txt_DefUseVarOK = "C01_Txt_DefUseVarOK";
        private const String C01_Txt_FailDefUseVar = "C01_Txt_FailDefUseVar";

        private const String C01_Txt_Nature = "C01_Txt_Nature";
        private const String C01_Txt_NatureOK = "C01_Txt_NatureOK";
        private const String C01_Txt_FailNature = "C01_Txt_FailNature";
      

        private const String C01_Txt_BackupCFG = "C01_Txt_BackupCFG";
        private const String C01_Txt_BackupDAT = "C01_Txt_BackupDAT";
        private const String C01_Txt_BackupLOG = "C01_Txt_BackupLOG";
        private const String C01_Txt_BackupNMW = "C01_Txt_BackupNMW";
        private const String C01_Txt_BackupHDA = "C01_Txt_BackupHDA";


        private const String C01_Txt_BackupCFGOK = "C01_Txt_BackupCFGOK";
        private const String C01_Txt_BackupDATOK = "C01_Txt_BackupDATOK";
        private const String C01_Txt_BackupLOGOK = "C01_Txt_BackupLOGOK";
        private const String C01_Txt_BackupNMWOK = "C01_Txt_BackupNMWOK";
        private const String C01_Txt_BackupHDAOK = "C01_Txt_BackupHDAOK";


        private const String C01_Txt_FailBackupCFG = "C01_Txt_FailBackupCFG";
        private const String C01_Txt_FailBackupLOG = "C01_Txt_FailBackupLOG";
        private const String C01_Txt_FailBackupDAT = "C01_Txt_FailBackupDAT";

        private const String C01_Txt_FailBackupNMW = "C01_Txt_FailBackupNMW";
        private const String C01_Txt_FailBackupHDA = "C01_Txt_FailBackupHDA";

        private const String Msg_UpdateVersionsDB = "C01_Txt_UpdatingVersions";

        private const String Msg_FailRestore_CFG = "C01_Txt_FailRestoreCFG";
        private const String Msg_FailRestore_DAT = "C01_Txt_FailRestoreDAT";
        private const String Msg_FailRestore_LOG = "C01_Txt_FailRestoreLOG";
        private const String Msg_FailRestore_HDA = "C01_Txt_FailRestoreHDA";
        private const String Msg_FailRestore_NMW = "C01_Txt_FailRestoreNMW";

        private const String Msg_EndUpdateVersionDB = "C01_Txt_EndUpdateVersionDB";
        private const String Msg_FailUpdateDB = "C01_Txt_FailUpdateDB";
        private const String C01_Txt_FailSaveVersionDB = "C01_Txt_FailSaveVersionDB";
        private const String C01_Txt_FailUpdated = "C01_Txt_FailUpdated";
        #endregion

        #region [ Properties ]

        private const String C01_Txt_Backup = "C01_Txt_Backup";
        private const String C01_Txt_BackupOK = "C01_Txt_BackupOK";
        private const String C01_Txt_FailBackup = "C01_Txt_FailBackup";  

        String m_sOriginalVersion = String.Empty;
        String m_sActualVersion = String.Empty;
        String m_sNewVersion = String.Empty;
        List<clsDBWindFarmBackup> listBackup = new List<clsDBWindFarmBackup>();
 
        private ILog logger;
        /// <summary>
        /// The logger that traces the object created
        /// </summary>        
        public ILog Logger
        {
            get { return this.logger; }
            set { this.logger = value; }
        }
        #endregion

        #region [ Load ]
        public frmUpdateWindfarm(ILog logger)
        {
            this.Logger = logger;
            InitializeComponent();

        }
        public frmUpdateWindfarm()
        {
            InitializeComponent();
        }

        ~frmUpdateWindfarm()
        {
        }
        /// <summary>
        /// Load the form and the resources
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LoadForm(object sender, EventArgs e)
        {
            LoadResources();
            ConfigurationListView();
        }
        private void ConfigurationListView()
        {
            U01_ltResult.View = View.Details;
            U01_ltResult.HeaderStyle = ColumnHeaderStyle.None;
            ColumnHeader h = new ColumnHeader();
            h.Width = U01_ltResult.ClientSize.Width - SystemInformation.VerticalScrollBarWidth;

            U01_ltResult.Columns.Add(h);

        }
        /// <summary>
        /// Load the Strings for the objects of the form
        /// </summary>
        public override void LoadResources()
        {
            // Load texts, create an array with the object to modify
            ArrayList lstObj = new ArrayList();
            //lstObj.Add(U01_lbDescription);
            lstObj.Add(U01_btExecute);
            lstObj.Add(U01_lbAcronimWindfarm);
            lstObj.Add(U01_lbNameWindfarm);
            lstObj.Add(U00_lbActualVersion);
            lstObj.Add(U00_lbNewVersion); ;
            lstObj.Add(U01_grGeneral);
            lstObj.Add(U01_grUpdateVersion);
            lstObj.Add(U00_lbNewVersion);
            lstObj.Add(U00_grInformation);
            lstObj.Add(G01_lbDirectory);
            lstObj.Add(this);  // fill out the tag of the form to identify the text
            Utils.LoadResources(lstObj);
            ParentMain.Text = String.Format(Utils.GetString(TitlesForm_UpdateName), clsGlobalVars.pwndWFSelect.Acronym, clsGlobalVars.pwndWFSelect.SQLIP);         
            
            LoadDadesGeneral();
            LoadLabelInformation();
        }
        #endregion            

        #region EVENTS
        private void U01_btExecute_Click(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;
            U01_btExecute.Enabled = false;
            Thread t = new Thread(new ThreadStart(ThreadUpdateDB));
            t.IsBackground = true;
            t.Start();
        }
        private void ThreadUpdateDB()
        {
            String sError, sTxt;
            long nError = 0;
            int  nPosActualVersion, nPosNewVersion;
            int valueProgressBar = 0;
            try
            {
                progressBar1.Visible = true;
                progressBar1.Value = 1;

                AddStringToListEvent(false,"Starting update database.....");
                logger.Debug("Starting update database");

                U01_btExecute.Enabled = false;
                m_sActualVersion = ParentMain.ActualVersion;
                m_sOriginalVersion = ParentMain.ActualVersion;
                m_sNewVersion = ParentMain.UpdateVersion;
                nPosActualVersion = GetVersionNumber(m_sActualVersion);
                nPosNewVersion = GetVersionNumber(m_sNewVersion);

                // Compare the actual version and the new version 
                // if ActualVersion > NewVersion --> Error
                if (nPosActualVersion > nPosNewVersion)
                {
                    sError = String.Format(Utils.GetString(U01_Txt_ErrVersionActualBigger), m_sActualVersion, m_sNewVersion);
                    AddStringToListEvent(true,sError);
                    logger.Fatal(sError);
                    ParentMain.ErrorInForm = ErrorsSetup.Err_VersionBiggerDB;
                }
                // if ActualVersion == NewVersion --> Already updated
                //else if (nPosActualVersion == nPosNewVersion)
                //{
                //    sError = String.Format(Utils.GetString(U01_Txt_ErrAlreadyUpdated), m_sActualVersion);
                //    logger.Fatal(sError);
                //    AddStringToListEvent(true,sError);
                //}
                else
                {
                    Cursor.Current = Cursors.WaitCursor;
                    int errorBackupRestore = 0;

                    if (nPosActualVersion == nPosNewVersion)
                    {
                        nPosActualVersion = nPosActualVersion - 1;
                        m_sActualVersion = ParentMain.VersionDBFile.NewVersions[nPosActualVersion].Name;
                    }

                    if (ckBackupRestore.Checked)
                    {
                        errorBackupRestore = MakeBackUp(m_sActualVersion);
                    }
                    //nError = MakeBackUp();
                    if ( errorBackupRestore==0 && m_dbManagerSetup.mensMultiErr.Count == 0)
                    {                       
                        //UPDATE VERSIONS
                        AddStringToListEvent(false,Utils.GetString(Msg_UpdateVersionsDB));
                        
                        
                        valueProgressBar = 60 / (nPosNewVersion - nPosActualVersion);
                        for (int nVersion = nPosActualVersion + 1; nVersion <= nPosNewVersion; nVersion++)
                        {
                            sTxt = String.Format(Utils.GetString(U01_Txt_StartingUpdate), ParentMain.VersionDBFile.NewVersions[nVersion].Name);
                            AddStringToListEvent(false,sTxt);

                            nError = m_dbManagerSetup.ExecuteUpdateVersion(ParentMain.VersionDBFile.NewVersions[nVersion].Name);

                            if (nError != 0 || m_dbManagerSetup.mensMultiErr.Count > 0)
                            {
                              
                                ParentMain.ErrorInForm = ErrorsSetup.Err_UpdatingVersion;
                                m_dbManagerSetup.Error.SetError(ErrorsSetup.Err_UpdatingVersion);
                                sError = String.Format(Utils.GetString(C01_Txt_FailUpdated), ParentMain.VersionDBFile.NewVersions[nVersion].Name);
                                Logger.Fatal(sError + "_Error : " + (nError == 0 ? m_dbManagerSetup.GetMensajeMultiErr() : m_dbManagerSetup.Error.GetMessageError()));
                                nError = 1;
                                AddStringToListEvent(true,sError);
                                break;
                            }
                            else
                            {
                                progressBar1.Value += valueProgressBar;
                                m_sActualVersion = ParentMain.VersionDBFile.NewVersions[nVersion].Name;
                                sTxt = String.Format(Utils.GetString(U01_Txt_UpdateFinished), ParentMain.VersionDBFile.NewVersions[nVersion].Name);
                                Logger.Debug(sTxt);
                                AddStringToListEvent(false,sTxt);
                              
                            }
                        }
                        //no error -> save the new version in the database
                        if (nError == 0 && m_dbManagerSetup.mensMultiErr.Count == 0)
                        {
                            //DefUseVar
                            AddStringToListEvent(false,Utils.GetString(C01_Txt_DefUseVar));
                            nError = DefUseVar();

                            if (nError == 0 && m_dbManagerSetup.mensMultiErr.Count == 0)
                            {
                                progressBar1.Value = 85;
                                Logger.Debug(Utils.GetString(C01_Txt_DefUseVarOK));
                                AddStringToListEvent(false,Utils.GetString(C01_Txt_DefUseVarOK));


                                //nature
                                AddStringToListEvent(false, Utils.GetString(C01_Txt_Nature));
                                nError = UpdateNature();
                                if (nError == 0 && m_dbManagerSetup.mensMultiErr.Count == 0)
                                {
                                    progressBar1.Value = 90;
                                    Logger.Debug(Utils.GetString(C01_Txt_NatureOK));
                                    AddStringToListEvent(false, Utils.GetString(C01_Txt_NatureOK));

                                    // Save actual version in DB
                                    nError = m_dbManagerSetup.SetActualVersion(clsGlobalVars.pwndWFSelect, m_sActualVersion,
                                                ParentMain.VersionDBFile.NewVersions[ParentMain.VersionDBFile.NewVersions.Count - 1].DateVersion);
                                    if (nError == 0)
                                    {
                                        progressBar1.Value = 98;
                                        ParentMain.ActualVersion = m_sActualVersion;
                                        Logger.Debug(Utils.GetString(Msg_EndUpdateVersionDB));
                                        AddStringToListEvent(false, Utils.GetString(Msg_EndUpdateVersionDB));
                                        LoadDadesGeneral();
                                        OnEventEnableNext();

                                    }
                                    else
                                    {
                                        // FAIL
                                        Logger.Fatal(Utils.GetString(C01_Txt_FailSaveVersionDB));
                                        AddStringToListEvent(true, Utils.GetString(C01_Txt_FailSaveVersionDB));
                                        RecoveryDades();
                                    }
                                }
                                else
                                {
                                    Logger.Fatal(Utils.GetString(C01_Txt_FailNature));
                                    AddStringToListEvent(true, Utils.GetString(C01_Txt_FailNature));
                                    RecoveryDades();
                                }

                            }
                            else
                            {
                                Logger.Fatal(Utils.GetString(C01_Txt_FailDefUseVar));
                                AddStringToListEvent(true,Utils.GetString(C01_Txt_FailDefUseVar));
                                RecoveryDades();
                            }
                           
                        }
                        else
                        {                           
                            RecoveryDades();
                        }
                    }
                    else
                    {                      
                        //RecoveryDades();
                    }
                   

                } 
               
            }
            catch (Exception ex)
            {
                logger.Fatal("ERROR : "+ ex.Message);
                Console.WriteLine("U01_btExecute_Click : " + ex.Message);
                RecoveryDades();

            }
            finally
            {
                progressBar1.Visible = false;
                Invoke(new MethodInvoker(EnableButton));
                Cursor.Current = Cursors.Default;
            }
           
        }
        /// <summary>
        /// Cancel the process of th backgroundworker
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void EnableButton()
        {
            //OnEventEnableNext(); 

            Cursor = Cursors.Default;

        }
        private void RecoveryDades()
        {
            if (ckBackupRestore.Checked)
            {
                AddStringToListEvent(true, Utils.GetString(Msg_FailUpdateDB));
                ParentMain.ErrorInForm = ErrorsSetup.Err_UpdatingVersion;
                RestoreAllDB();
                MessageBox.Show(Utils.GetString(Msg_FailUpdateDB));
            }

        }
      
        #endregion

        #region [ Methods ]
        private void LoadLabelInformation()
        {
            string Message = string.Empty;

            U01_lbInforTime.Text = "The Update process may take several minutes depending on the size and version of the database.";
            Message = "Development of the upgrade process :" + "\r\n";
            Message += " - Backup of the selected database." + "\r\n";
            Message += " - Execution of the updates to the database." + "\r\n";
            Message += " - If the process has failed to be restored database." + "\r\n";
            U01_lbSteps.Text = Message;
        }
        /// <summary>
        /// Save the version in the database that has been updated
        /// </summary>
        /// <param name="sActualVersion"></param>
        /// <returns></returns>
        public long SaveVersionInDB()
        {
            return m_dbManagerSetup.SetActualVersion(clsGlobalVars.pwndWFSelect, m_sActualVersion,
                                                    ParentMain.VersionDBFile.NewVersions[ParentMain.VersionDBFile.NewVersions.Count - 1].DateVersion);
        }

        

        private void AddStringToListEvent(bool esError,String sText)
        {
            //Graphics g = U01_ltResult.CreateGraphics();

            //// Determine the size for HorizontalExtent using the MeasureString method using the last item in the list.
            //int hzSize = (int)g.MeasureString(sText, U01_ltResult.Font).Width;
            //U01_ltResult.HorizontalExtent = hzSize;
            ListViewItem item = new ListViewItem(sText);
            item.ForeColor = esError ? Color.Red : Color.Black;
          
            U01_ltResult.Items.Add(item);
            U01_ltResult.Refresh();
            U01_ltResult.Items[U01_ltResult.Items.Count - 1].Selected = true;
            U01_ltResult.Select();
            U01_ltResult.Items[U01_ltResult.Items.Count - 1].Focused = true;
            U01_ltResult.EnsureVisible(U01_ltResult.Items.Count - 1);
        }
        private void LoadDadesGeneral()
        {
            U00_txActualVersion.Text = ParentMain.ActualVersion;
            U00_txNewVersion.Text = ParentMain.UpdateVersion;
            U01_txNameWindfarm.Text = clsGlobalVars.pwndWFSelect.Name;
            U01_txAcronimWindfarm.Text = clsGlobalVars.pwndWFSelect.Acronym;
            G01_txDirectory.Text = string.Format(@"{0}WindAccess\Bdd\",ParentMain.ListArgs.ContainsKey(CommonConst.Arg.Directory_GUIScreen) ? ParentMain.ListArgs[CommonConst.Arg.Directory_GUIScreen].ToString() : @"F:\");
            ckBackupRestore.Checked = ParentMain.ListArgs.ContainsKey(CommonConst.Arg.BackupRestore) ? Convert.ToBoolean(ParentMain.ListArgs[CommonConst.Arg.BackupRestore]) : false;   
        }


        private int MakeBackUp(string m_sActualVersion)
        {
            String sFile = String.Empty;
            String strErr= String.Empty;
            String sDirectory = String.Empty;
          
           listBackup = new List<clsDBWindFarmBackup>();
            int NumeroIntentos = 3;
           
           
            int nError = 1;
            try
            {
                // make a backup before starting the upgrade
                AddStringToListEvent(false, "Creating databases backup....");
                sDirectory = ParentMain.ListArgs.ContainsKey(CommonConst.Arg.Directory_GUIScreen) ? ParentMain.ListArgs[CommonConst.Arg.Directory_GUIScreen].ToString() : @"F:\";
                List<VersionInfo> versionUpdate = ParentMain.VersionDBFile.UpdateVersions(m_sActualVersion);
              //  listBackup = clsDBWindFarmBackup.GetListDBWF(clsGlobalVars.pwndWFSelect.Acronym);
                listBackup = clsDBWindFarmBackup.GetListDBWF_UpdateVersions(clsGlobalVars.pwndWFSelect.Acronym, versionUpdate, ParentMain.ActualVersion, ParentMain.UpdateVersion);

                if (listBackup.Count > 0)
                {
                    for (int i = 0; i < NumeroIntentos; i++)
                    {
                        foreach (clsDBWindFarmBackup item in listBackup)
                        {
                            if (!item.Backup)
                            {
                                sFile = String.Format(CommonConst.csPathBackupDB, sDirectory, item.Name);
                                if (m_dbManagerSetup.BackUp_DB(item.Name, sFile, clsGlobalVars.pwndWFSelect) != 0)
                                {
                                    logger.Fatal(String.Format(Utils.GetString(C01_Txt_FailBackup), item.Name));
                                    AddStringToListEvent(true, String.Format(Utils.GetString(C01_Txt_FailBackup), item.Name));
                                }
                                else
                                {
                                    item.Backup = true;
                                    AddStringToListEvent(false, String.Format(Utils.GetString(C01_Txt_BackupOK), item.Name));
                                    logger.Debug(String.Format(Utils.GetString(C01_Txt_BackupOK), item.Name));
                                }
                            }
                        }

                        if (clsDBWindFarmBackup.AllDBBackup(listBackup))
                        {
                            AddStringToListEvent(false, "Backup created");
                            progressBar1.Value = 30;
                            nError = 0;
                            break;
                        }
                        i++;
                    }
                }
            }
            catch (Exception ex)
            {
                nError = 1;
                logger.Fatal("MakeBackup :" + ex.Message);
                Console.WriteLine("MakeBackup :" + ex.Message);
            }
            return nError;
        }

    
        internal long RestoreAllDB()
        {
            String strErr = String.Empty;
            String sFile = String.Empty;
            String sDirectory = String.Empty;           
            
            long nError = 1;  
            int NumeroDB = 0;
            
            try
            {
                AddStringToListEvent(false, "Restoring databases ....");
                sDirectory = ParentMain.ListArgs.ContainsKey(CommonConst.Arg.Directory_GUIScreen) ? ParentMain.ListArgs[CommonConst.Arg.Directory_GUIScreen].ToString() : @"F:\";

                foreach (clsDBWindFarmBackup item in listBackup)
                {
                    if (item.Backup)
                    {
                        sFile = String.Format(CommonConst.csPathBackupDB, sDirectory, item.Name);
                        nError = m_dbManagerSetup.Restore_DB(item.Name, sFile, clsGlobalVars.pwndWFSelect);

                        if (nError == 0 && m_dbManagerSetup.mensMultiErr.Count == 0)
                        {
                            NumeroDB++;
                            AddStringToListEvent(false, "Restore " + item.Name + "....OK");
                            logger.Debug("Restore " + item.Name + "....OK");
                        }
                        else
                        {
                            Logger.Fatal("_ERROR: " + (string.IsNullOrEmpty(m_dbManagerSetup.GetMensajeMultiErr()) ? m_dbManagerSetup.Error.GetMessageError() : m_dbManagerSetup.GetMensajeMultiErr()));
                            ParentMain.ErrorInForm = ErrorsSetup.Err_ExecutingRestore;
                            AddStringToListEvent(true, "_ERROR " + "Restore " + item.Name);

                        }
                    }
                    
                }

                if (NumeroDB == listBackup.Count)
                {
                    AddStringToListEvent(false, "Restore All");
                    progressBar1.Value = 30;
                    nError = 0;

                }  
               
            }
            catch (Exception ex)
            {
                ParentMain.ErrorInForm = ErrorsSetup.Err_ExecutingRestore;
                U01_ltResult.Items.Add("Restore All DB :" + ex.Message);
                Logger.Fatal("Restore All DB :" + ex.Message);
            }
            return nError;
        }

        private long DefUseVar()
        {
            long nError = 0;
            String sFile = String.Empty;
            try
            {
                sFile = Application.StartupPath + "\\" + CommonConst.csScriptsDirectory + "\\" +
                CommonConst.csScriptsConfig_DefUserVars + "\\" + CommonConst.csDefUseVar_Config;

                nError = m_dbManagerSetup.ExecuteSQLFile(sFile);
                if (nError != 0 || m_dbManagerSetup.mensMultiErr.Count > 0)
                {
                    Logger.Fatal("_Error : " + (nError == 0 ? m_dbManagerSetup.GetMensajeMultiErr() : m_dbManagerSetup.Error.GetMessageError()));
                } 
            }
            catch (Exception ex)
            {
                Console.WriteLine("DefUseVar : " + ex.Message);
                logger.Fatal("DefUseVar : " + ex.Message);
                nError = 1;
            }
            return nError;
        }
        private long UpdateNature()
        {
            long nError = 0;
            String sFile = String.Empty;
            
            try
            {
                 sFile = Application.StartupPath + "\\" + CommonConst.csScriptsDirectory + "\\" +
                    CommonConst.csScriptsNatureDirectory + "\\" + CommonConst.csActualizarNature;


                 nError = m_dbManagerSetup.ExecuteSQLFile(sFile, 900000);
                if (nError != 0 || m_dbManagerSetup.mensMultiErr.Count > 0)
                {

                    Logger.Fatal("_Error : " + (nError == 0 ? m_dbManagerSetup.GetMensajeMultiErr() : m_dbManagerSetup.Error.GetMessageError()));
                } 
            }
            catch (Exception ex)
            {
                Console.WriteLine("UpdateNature : " + ex.Message);
                logger.Fatal("UpdateNature : " + ex.Message);
                nError = 1;
            }
            return nError;
        }
        #endregion
        
    }
}