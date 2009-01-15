//Copyright (c) 2004-2007
//by Engage Software ( http://www.engagesoftware.com )

//THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED 
//TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL 
//THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF 
//CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER 
//DEALINGS IN THE SOFTWARE.

using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Text;
using System.Threading;
using DotNetNuke.Common.Lists;
using DotNetNuke.Common.Utilities;
using DotNetNuke.Entities.Modules;
using DotNetNuke.Services.Exceptions;
using DotNetNuke.Services.FileSystem;
using Engage.Dnn.Locator.Data;
using Engage.Dnn.Locator.Maps;
using LumenWorks.Framework.IO.Csv;

namespace Engage.Dnn.Locator.Components
{
    using System.Globalization;

    public class DataImportScheduler : DotNetNuke.Services.Scheduling.SchedulerClient
    {
        private const string ImportFolderName = "Location Import";
        private const string WorkingFolderName = "Location Import/Working/";
        private const string CompletedFolderName = "Location Import/Completed/";
        private const string ErrorFolderName = "Location Import/Error/";
        private int portalId;

        public DataImportScheduler(DotNetNuke.Services.Scheduling.ScheduleHistoryItem scheduleHistoryItem)
        {
            ScheduleHistoryItem = scheduleHistoryItem;
        }

        public override void DoWork()
        {
            try
            {
                while (HasFilesToImport)
                {
                    DataTable files = Location.GetFilesToImport();

                    foreach (DataRow file in files.Rows)
                    {
                        this.portalId = Convert.ToInt32(file["PortalId"], CultureInfo.InvariantCulture);

                        FolderInfo fiLocation = FileSystemUtils.GetFolder(this.portalId, ImportFolderName);
                        ArrayList filesLocation = FileSystemUtils.GetFilesByFolder(this.portalId, fiLocation.FolderID);

                        FolderInfo fiLocationWorking = FileSystemUtils.GetFolder(this.portalId, WorkingFolderName);
                        ArrayList filesLocationWorking = FileSystemUtils.GetFilesByFolder(this.portalId, fiLocationWorking.FolderID);

                        int index = DataProvider.Instance().GetLastImportIndex();

                        if (filesLocation.Count > 0)
                        {
                            foreach (DotNetNuke.Services.FileSystem.FileInfo fileInfo in filesLocationWorking)
                            {
                                File.Delete(fileInfo.PhysicalPath);
                            }
                            StageData(filesLocation, fiLocationWorking);
                            CollectData(fiLocationWorking, index, this.portalId);
                        }
                        else if (filesLocationWorking.Count > 0)
                        {
                            CollectData(fiLocationWorking, index, this.portalId);
                        }

                        int id = Convert.ToInt32(file["FileId"], CultureInfo.InvariantCulture);
                        UpdateImportedLocationRow(id);
                    }

                }
            }
            catch (IOException)
            {
                // these occur when trying to access the file and it is open. This will occur alot and we don't want to 
                // write all of them to the log.
                this.FileMove(false);
            }
            catch (Exception ex)
            {
                Exceptions.LogException(ex);
                ScheduleHistoryItem.Succeeded = false;
                ScheduleHistoryItem.AddLogNote("Locator Import Failed: " + ex.ToString());
                FileMove(false);
            }
            finally
            {
                ScheduleHistoryItem.Succeeded = true;
                ScheduleHistoryItem.AddLogNote("Locator Import completed successfully.<br>");
            }

        }

        public void StageData(ArrayList files, FolderInfo folderInfo)
        {
            FileController fc = new FileController();
            if (folderInfo != null && files != null)
            {
                foreach (DotNetNuke.Services.FileSystem.FileInfo fileInfo in files)
                {
                    if (File.Exists(fileInfo.PhysicalPath))
                    {
                        File.Move(fileInfo.PhysicalPath, folderInfo.PhysicalPath + fileInfo.FileName);
                        fc.UpdateFile(fileInfo.FileId, fileInfo.FileName, fileInfo.Extension.Substring(1), fileInfo.Size, Null.NullInteger, Null.NullInteger, fileInfo.ContentType, folderInfo.PhysicalPath, folderInfo.FolderID);
                    }
                }
            }
        }


        public void CollectData(FolderInfo folderInfo, int startIndex, int portalId)
        {
            ArrayList filesWorking = FileSystemUtils.GetFilesByFolder(portalId, folderInfo.FolderID);

            if (filesWorking != null)
            {
                foreach (DotNetNuke.Services.FileSystem.FileInfo fileInfo in filesWorking)
                {
                    ParseCSV(fileInfo, startIndex);
                }
            }
            ImportData(this.portalId);
        }

        public void ParseCSV(DotNetNuke.Services.FileSystem.FileInfo fileInfo, int startIndex)
        {
            using (CsvReader reader = new CsvReader(new StreamReader(fileInfo.PhysicalPath), true))
            {
                int tabModuleId = DataProvider.Instance().GetTabModuleIdByFileId(fileInfo.FileId);
                int lineNumber = 0;

                if (startIndex == 0)
                {
                    DataProvider.Instance().ClearTempLocations();
                }
                else
                {
                    lineNumber = startIndex;
                    reader.MoveTo(startIndex - 1);
                }

                try
                {
                    while (reader.ReadNextRecord())
                    {
                        lineNumber++;
                        Location location = new Location();
                        location.LocationId = Convert.ToInt32(GetValue(reader, "UNIQUE_KEY"), CultureInfo.InvariantCulture);
                        location.ExternalIdentifier = GetValue(reader, "EXTERNAL_IDENTIFIER");
                        location.Name = GetValue(reader, "LOCATION_NAME");
                        string address1 = GetValue(reader, "ADDRESS1");
                        string address2 = GetValue(reader, "ADDRESS2");
                        location.City = GetValue(reader, "CITY");
                        string state = GetValue(reader, "STATE");
                        location.PostalCode = GetValue(reader, "ZIP");
                        location.Phone = GetValue(reader, "PHONE_NUMBER");
                        location.LocationDetails = GetValue(reader, "LOCATION_DETAILS");
                        location.Website = GetValue(reader, "WEBSITE");
                        location.PortalId = this.portalId;
                        string locationType = GetValue(reader, "TYPE_OF_LOCATION");
                        if (locationType == string.Empty)
                        {
                            locationType = "Default";
                        }

                        string country = GetValue(reader, "COUNTRY");
                        DataTable dt = DataProvider.Instance().GetLocationTypes();
                        int locationTypeId = -1;

                        foreach (DataRow dr in dt.Rows)
                        {
                            if (Convert.ToString(dr["LocationTypeName"], CultureInfo.InvariantCulture) == locationType)
                            {
                                locationTypeId = Convert.ToInt32(dr["LocationTypeID"], CultureInfo.InvariantCulture);
                                location.LocationTypeId = locationTypeId;
                            }
                        }

                        if (locationTypeId == -1)
                        {
                            locationTypeId = DataProvider.Instance().InsertLocationType(locationType);
                            location.LocationTypeId = locationTypeId;
                        }

                        location.RegionId = ResolveState(state);
                        if (location.RegionId < 1)
                        {
                            location.RegionId = Convert.ToInt32(null, CultureInfo.InvariantCulture);
                        }

                        location.CountryId = ResolveCountry(country);
                        if (location.CountryId < 1)
                        {
                            ModuleController objModules = new ModuleController();
                            location.CountryId = Dnn.Utility.GetIntSetting(objModules.GetTabModuleSettings(tabModuleId), "DefaultCountry").Value;
                        }

                        location.CsvLineNumber = lineNumber;
                        location.Address = address1;
                        
                        // Address2 is informational only - See Pat Renner.
                        location.Address2 = address2;

                        GeocodeResult geocodeResult = GetGeoCoordinates(tabModuleId, address1, location.City, location.RegionId, location.PostalCode, location.CountryId);
                        if (geocodeResult.Successful)
                        {
                            location.Latitude = geocodeResult.Latitude;
                            location.Longitude = geocodeResult.Longitude;
                        }

                        location.Approved = true;

                        try
                        {
                            location.SaveTemp(geocodeResult.Successful);
                        }
                        catch (SqlException ex)
                        {
                            Exceptions.LogException(ex);
                        }
                    }
                }
                catch (ArgumentException exc)
                {
                    Exceptions.LogException(exc);
                    this.FileMove(false);
                }

                if (lineNumber == 0)
                {
                    this.FileMove(false);
                }
            }
        }

        public static GeocodeResult GetGeoCoordinates(int tabModuleId, string address1, string city, int? regionId, string zip, int countryId)
        {
            DataTable existingLatitudeLongitude = DataProvider.Instance().GetLatitudeLongitude(address1, city);
            if (existingLatitudeLongitude.Rows.Count == 0)
            {
                return GetGeoCodeResults(tabModuleId, address1, city, regionId, zip, countryId);
            }

            double? latitude, longitude;
            if (existingLatitudeLongitude.Rows[0]["Latitude"].ToString() == string.Empty)
            {
                latitude = null;
            }
            else
            {
                latitude = Convert.ToDouble(existingLatitudeLongitude.Rows[0]["Latitude"], CultureInfo.InvariantCulture);
            }

            if (existingLatitudeLongitude.Rows[0]["Longitude"].ToString() == string.Empty)
            {
                longitude = null;
            }
            else
            {
                longitude = Convert.ToDouble(existingLatitudeLongitude.Rows[0]["Longitude"], CultureInfo.InvariantCulture);
            }

            return new ManualGeocodeResult(latitude, longitude);
        }

        private static GeocodeResult GetGeoCodeResults(int tabModuleId, string address, string city, int? regionId, string zip, int countryId)
        {
            MapProvider mapProvider = null;
            Hashtable tabModuleSettings = new ModuleController().GetTabModuleSettings(tabModuleId);

            string displayProvider = Dnn.Utility.GetStringSetting(tabModuleSettings, "DisplayProvider");
            foreach (MapProviderType type in MapProviderType.GetMapProviderTypes())
            {
                if (type.ClassName.Contains(displayProvider))
                {
                    mapProvider = MapProvider.CreateInstance(type.ClassName, Dnn.Utility.GetStringSetting(tabModuleSettings, type.ClassName + ".ApiKey"));
                    break;
                }
            }

            if (mapProvider != null)
            {
                return mapProvider.GeocodeLocation(address, city, regionId, zip, countryId);
            }

            return GeocodeResult.Empty;
        }

        private void ImportData(int portalId)
        {
            string emailFrom = DotNetNuke.Entities.Host.HostSettings.GetHostSetting("HostEmail");
            StringBuilder emailBody = new StringBuilder();
            emailBody.Append("Your locations file has been imported.<br />");
            StringBuilder unsuccessfulLocations = new StringBuilder();

            DataTable locations = DataProvider.Instance().GetImportedLocationStatistics(portalId);
            int successfulCount = 0;
            int unsuccessfulCount = 0;
            foreach (DataRow row in locations.Rows)
            {
                if (row["Successful"].ToString() == "True")
                {
                    successfulCount++;
                }
                else
                {
                    unsuccessfulCount++;
                    unsuccessfulLocations.Append(row["LocationId"] + ", ");
                    unsuccessfulLocations.Append(row["Name"] + "<br />");
                }
            }

            if (unsuccessfulCount > 0)
            {
                emailBody.Append("There were " + successfulCount + " out of " + locations.Rows.Count + " locations imported successfully. <br />");
                emailBody.Append("The following locations (UNIQUE_KEY, LOCATION_NAME) from the csv file were not imported: <br /><br />");
                emailBody.Append(unsuccessfulLocations + "<br />");
            }
            else
            {
                emailBody.Append("All " + successfulCount + " out of " + locations.Rows.Count + " locations were imported successfully.");
            }

            FolderInfo fi = FileSystemUtils.GetFolder(portalId, WorkingFolderName);
            ArrayList files = FileSystemUtils.GetFilesByFolder(portalId, fi.FolderID);

            // how should this be handled correctly
            DataProvider.Instance().ClearLocations(portalId);
            DataProvider.Instance().CopyData();
            DataProvider.Instance().ClearTempLocations();

            const string emailSubject = "Engage: Locator Import Succeeded";

            foreach (DotNetNuke.Services.FileSystem.FileInfo fileInfo in files)
            {
                DataTable dt = DataProvider.Instance().GetEmailByFileId(fileInfo.FileId);
                if (dt.Rows.Count > 0)
                {
                    string email = Convert.ToString(dt.DefaultView[0].Row[3], CultureInfo.InvariantCulture);
                    if (email == string.Empty)
                    {
                        // TODO: Use a reasonable default email
                        email = "local@localhost.com";
                    }

                    if (emailFrom == string.Empty)
                    {
                        emailFrom = "local@localhost.com";
                    }

                    DotNetNuke.Services.Mail.Mail.SendMail(emailFrom, email, string.Empty, emailSubject, emailBody.ToString(), string.Empty, "html", string.Empty, string.Empty, string.Empty, string.Empty);
                }
            }

            this.FileMove(true);
        }

        private static string GetValue(CsvReader reader, string colName)
        {
            if (!String.IsNullOrEmpty(colName))
            {
                return reader[colName];
            }

            return string.Empty;
        }

        public bool FileMove(bool success)
        {
            try
            {
                FolderInfo fiLocationCompleted = FileSystemUtils.GetFolder(this.portalId, CompletedFolderName);

                if (fiLocationCompleted != null)
                {
                    ArrayList filesLocationCompleted = FileSystemUtils.GetFilesByFolder(this.portalId, fiLocationCompleted.FolderID);
                    foreach (DotNetNuke.Services.FileSystem.FileInfo fileInfo in filesLocationCompleted)
                    {
                        File.Delete(fileInfo.PhysicalPath);
                    }

                    FolderInfo fiLocationError = FileSystemUtils.GetFolder(this.portalId, ErrorFolderName);
                    ArrayList filesLocationError = FileSystemUtils.GetFilesByFolder(this.portalId, fiLocationError.FolderID);
                    foreach (DotNetNuke.Services.FileSystem.FileInfo fileInfo in filesLocationError)
                    {
                        File.Delete(fileInfo.PhysicalPath);
                    }

                    FolderInfo fiLocationWorking = FileSystemUtils.GetFolder(this.portalId, WorkingFolderName);
                    ArrayList filesLocationWorking = FileSystemUtils.GetFilesByFolder(this.portalId, fiLocationWorking.FolderID);
                    foreach (DotNetNuke.Services.FileSystem.FileInfo fileInfo in filesLocationWorking)
                    {
                        if (success)
                        {
                            File.Move(fileInfo.PhysicalPath, fiLocationCompleted.PhysicalPath + fileInfo.FileId + fileInfo.FileName);
                        }
                        else
                        {
                            string fileToMove = fiLocationError.PhysicalPath + fileInfo.FileName;
                            if (File.Exists(fileToMove))
                            {
                                File.Move(fileInfo.PhysicalPath, fileToMove);
                            }
                        }
                    }
                }
            }
            catch (IOException exc)
            {
                Exceptions.LogException(exc);
                return false;
            }

            return true;
        }

        private static int ResolveState(string state)
        {
            int id = -1;
            ListController controller = new ListController();
            ListEntryInfoCollection regions = controller.GetListEntryInfoCollection("Region");
            foreach (ListEntryInfo region in regions)
            {
                if (region.Text == state || region.Value == state)
                {
                    id = region.EntryID;
                }
            }

            return id;
        }

        private static int ResolveCountry(string country)
        {
            int id = -1;
            ListController controller = new ListController();
            ListEntryInfoCollection info = controller.GetListEntryInfoCollection("Country"); // controller.GetListEntryInfo("Country", country);
            foreach (ListEntryInfo entry in info)
            {
                if (entry.Text == country)
                {
                    id = entry.EntryID;
                }
            }

            return id;
        }

        private static void UpdateImportedLocationRow(int fileId)
        {
            Location.UpdateImportedLocationRow(fileId);
        }

        private static bool HasFilesToImport
        {
            get
            {
                DataTable dt = Location.GetFilesToImport();
                return dt.Rows.Count > 0;
            }
        }
    }
}