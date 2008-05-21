//Engage: Locator - http://www.engagemodules.com
//Copyright (c) 2004-2007
//by Engage Software ( http://www.engagesoftware.com )

//THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED 
//TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL 
//THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF 
//CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER 
//DEALINGS IN THE SOFTWARE.

using System;
using System.Collections;
using System.Data;
using System.IO;
using System.Globalization;
using System.Web.UI.WebControls;
using DotNetNuke.Common.Utilities;
using DotNetNuke.Services.Exceptions;
using DotNetNuke.Services.FileSystem;
using DotNetNuke.Services.Localization;
using DotNetNuke.UI.UserControls;
using DotNetNuke.UI.Utilities;
using Engage.Dnn.Locator.Data;
using Engage.Dnn.Locator.Components;
using Engage.Dnn.Locator.Maps;
using FileInfo=DotNetNuke.Services.FileSystem.FileInfo;
using Globals=DotNetNuke.Common.Globals;

namespace Engage.Dnn.Locator
{
    partial class Import : ModuleBase
    {
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            if (DotNetNuke.Framework.AJAX.IsInstalled())
            {
                DotNetNuke.Framework.AJAX.RegisterScriptManager();
            }

            lbSettings.Visible = IsEditable;
            lbManageComments.Visible = IsEditable;
            lbManageLocations.Visible = IsEditable;
            lbLocationTypes.Visible = IsEditable;
        }


        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                if (fileImport.PostedFile == null || fileImport.PostedFile.ContentLength < 1)
                {
                    lblMessage.Text = Localization.GetString("NoFileSelected", LocalResourceFile);
                    return;
                }
                if (!fileImport.PostedFile.FileName.EndsWith(".csv", StringComparison.OrdinalIgnoreCase))
                {
                    lblMessage.Text = Localization.GetString("UseCSVFile", LocalResourceFile); 
                    return;
                }
                else
                {
                    lblMessage.Text = Localization.GetString("FileExists", LocalResourceFile);
                }

                VerifyFolders();
                UploadFile();
                DataImportScheduler import = new DataImportScheduler();
            }
            catch (Exception exc)
            {
                Exceptions.ProcessModuleLoadException(this, exc);
            }
        }


        protected void UploadFile()
        {
            System.Web.HttpPostedFile postedFile = fileImport.PostedFile;
            FolderInfo fi = FileSystemUtils.GetFolder(PortalId, "Location Import");
            ArrayList al = FileSystemUtils.GetFilesByFolder(PortalId, fi.FolderID);
            FileController fc = new FileController();
            DataProvider provider = DataProvider.Instance();
            string fileName = Path.GetFileName(postedFile.FileName);
            string newPath = fi.PhysicalPath + fileName;
            foreach (FileInfo s in al)
            {
                if (s.FileName == fileName)
                {
                    lblMessage.Text = Localization.GetString("FileExists", LocalResourceFile); 
                    return;
                }
            }
            
            postedFile.SaveAs(newPath);
            string ext = Path.GetExtension(postedFile.FileName).Substring(1);
            int fileId = fc.AddFile(PortalId, fileName,ext , postedFile.ContentLength, Null.NullInteger, Null.NullInteger, postedFile.ContentType, fi.FolderPath, fi.FolderID, true);

            provider.InsertFileInfo(fileId, UserId, TabModuleId, PortalId, DateTime.Now, 0, 0);
            lblMessage.Text = Localization.GetString("FileUploaded", LocalResourceFile); 

        }

        protected void VerifyFolders()
        {
            string loc = "Location Import";
            string home = PortalSettings.HomeDirectoryMapPath;
            string homeImport = PortalSettings.HomeDirectoryMapPath + loc;
            string working = "Working";
            string completed = "Completed";
            string error = "Error";
            
            
            if (FileSystemUtils.GetFolder(PortalId, loc) == null)
            {
                FileSystemUtils.AddFolder(PortalSettings, home, loc);
            }

            if (FileSystemUtils.GetFolder(PortalId, loc + "/" + working) == null)
            {
                FileSystemUtils.AddFolder(PortalSettings, homeImport, working);
            }

            if (FileSystemUtils.GetFolder(PortalId, loc + "/" + completed) == null)
            {
                FileSystemUtils.AddFolder(PortalSettings, homeImport, completed);
            }

            if (FileSystemUtils.GetFolder(PortalId, loc + "/" + error) == null)
            {
                FileSystemUtils.AddFolder(PortalSettings, homeImport, error);
            }
        }

        protected void btnBack_Click(object sender, EventArgs e)
        {
            Response.Redirect(Globals.NavigateURL(TabId));
        }

        protected void Page_Load(object sender, EventArgs e)
        {           
            string error = String.Empty;

            if (MainDisplay.IsConfigured(TabModuleId, ref error))
            {

                if (!Page.IsPostBack)
                {
                }
            }
            else
            {
                lblConfigured.Visible = true;
                lblConfigured.Text = lblConfigured.Text + " " + error;
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect(Globals.NavigateURL(TabId));
        }

        //This should be in ModuleBase and used by all.
        #region Global Admin Event Handlers

        protected void lbSettings_OnClick(object sender, EventArgs e)
        {
            string href = EditUrl("ModuleId", ModuleId.ToString(CultureInfo.InvariantCulture), "Module");
            Response.Redirect(href, true);
        }

        protected void lblManageLocations_OnClick(object sender, EventArgs e)
        {
            string href = EditUrl("ManageLocations");
            Response.Redirect(href, true);
        }

        protected void lblImportFile_OnClick(object sender, EventArgs e)
        {
            string href = EditUrl("Import");
            Response.Redirect(href, true);
        }

        protected void lblManageComments_OnClick(object sender, EventArgs e)
        {
            string href = EditUrl("ManageComments");
            Response.Redirect(href, true);
        }

        protected void lblManageTypes_OnClick(object sender, EventArgs e)
        {
            string href = EditUrl("AttributeDefinitions");
            Response.Redirect(href, true);
        }

        #endregion
    }
}
