using System;
using System.Collections.Generic;
using System.Data;
using DotNetNuke.Common.Lists;
using DotNetNuke.Entities.Modules;
using DotNetNuke.Common.Utilities;
using Engage.Dnn.Locator.Data;
using Microsoft.ApplicationBlocks.Data;

namespace Engage.Dnn.Locator
{
    public class LocationType
    {
        public const string CacheKey = "AttributeDefinitions{0}";
        public const int CacheTimeOut = 20;

        public static DataTable GetLocationTypes()
        {
            return DataProvider.Instance().GetLocationTypes();
        }

        public static bool GetLocationTypeInUse(string location)
        {
            bool inuse = false;
            int count;

            count = DataProvider.Instance().GetLocationTypeCount(location);
            if (count > 0) inuse = true;

            return inuse;
        }

        public static string GetLocationTypeName(int id)
        {
            DataTable dt = DataProvider.Instance().GetLocationTypeName(id);
            string name = string.Empty;
            if(dt.Rows.Count == 1)
            {
                name = dt.Rows[0]["LocationTypeName"].ToString();
                
            }

            return name;
        }

        #region Properties

        [System.Diagnostics.DebuggerBrowsable(System.Diagnostics.DebuggerBrowsableState.Never)]
        private int _locationId;
        public int LocationId
        {
            [System.Diagnostics.DebuggerStepThroughAttribute()]
            get { return _locationId; }
            [System.Diagnostics.DebuggerStepThroughAttribute()]
            set { _locationId = value; }
        }

        [System.Diagnostics.DebuggerBrowsable(System.Diagnostics.DebuggerBrowsableState.Never)]
        private string _locationTypeName;        
        public string LocationTypeName
        {
            [System.Diagnostics.DebuggerStepThroughAttribute()]
            get { return _locationTypeName; }
            [System.Diagnostics.DebuggerStepThroughAttribute()]
            set { _locationTypeName = value; }
        }

        #endregion

        #region Custom Attributes

        private static AttributeDefinitionCollection FillCollection(IDataReader dr)
        {
            List<AttributeDefinition> definitions = CBO.FillCollection<AttributeDefinition>(dr);
            AttributeDefinitionCollection definitionsCollection = new AttributeDefinitionCollection();
            foreach (AttributeDefinition definition in definitions)
            {
                //Clear the Is Dirty Flag
                definition.ClearIsDirty();

                //Initialise the Visibility
                object setting = UserModuleBase.GetSetting(definition.PortalId, "LocationType_DefaultVisibility");
                if ((setting != null))
                {
                    definition.Visibility = (DotNetNuke.Entities.Users.UserVisibilityMode)setting;
                }

                //Add to collection
                definitionsCollection.Add(definition);
            }
            return definitionsCollection;
        }

        private static AttributeDefinition FillAttributeDefinitionInfo(IDataReader dr)
        {
            AttributeDefinition definition = null;

            try
            {
                definition = FillAttributeDefinitionInfo(dr, false);
            }
            catch
            {
            }
            finally
            {
                if ((dr != null))
                {
                    dr.Close();
                }
            }

            return definition;
        }

        private static AttributeDefinition FillAttributeDefinitionInfo(IDataReader dr, bool checkForOpenDataReader)
        {
            AttributeDefinition definition = null;

            // read datareader
            bool canContinue = true;
            if (checkForOpenDataReader)
            {
                canContinue = false;
                if (dr.Read())
                {
                    canContinue = true;
                }
            }
            if (canContinue)
            {
                definition = new AttributeDefinition();
                definition.AttributeDefinitionId = Convert.ToInt32(Null.SetNull(dr["AttributeDefinitionId"], definition.AttributeDefinitionId));
                definition.PortalId = Convert.ToInt32(Null.SetNull(dr["PortalId"], definition.PortalId));
                definition.LocationTypeId = Convert.ToInt32(Null.SetNull(dr["LocationTypeId"], definition.LocationTypeId));
                definition.DataType = Convert.ToInt32(Null.SetNull(dr["DataType"], definition.DataType));
                definition.DefaultValue = Convert.ToString(Null.SetNull(dr["DefaultValue"], definition.DefaultValue));
                //definition.AttributeCategory = Convert.ToString(Null.SetNull(dr["AttributeCategory"], definition.AttributeCategory));
                definition.AttributeName = Convert.ToString(Null.SetNull(dr["AttributeName"], definition.AttributeName));
                definition.Length = Convert.ToInt32(Null.SetNull(dr["Length"], definition.Length));
                definition.Required = Convert.ToBoolean(Null.SetNull(dr["Required"], definition.Required));
                definition.ValidationExpression = Convert.ToString(Null.SetNull(dr["ValidationExpression"], definition.ValidationExpression));
                definition.ViewOrder = Convert.ToInt32(Null.SetNull(dr["ViewOrder"], definition.ViewOrder));
                definition.Visible = Convert.ToBoolean(Null.SetNull(dr["Visible"], definition.Visible));
            }

            return definition;
        }

        private static List<AttributeDefinition> FillAttributeDefinitionInfoCollection(IDataReader dr)
        {
            List<AttributeDefinition> arr = new List<AttributeDefinition>();
            try
            {
                AttributeDefinition obj;
                while (dr.Read())
                {
                    // fill business object
                    obj = FillAttributeDefinitionInfo(dr, false);
                    // add to collection
                    arr.Add(obj);
                }
            }
            catch (Exception exc)
            {
                DotNetNuke.Services.Exceptions.Exceptions.LogException(exc);
            }
            finally
            {
                // close datareader
                if ((dr != null))
                {
                    dr.Close();
                }
            }

            return arr;
        }

        private static List<AttributeDefinition> GetAttributeDefinitions(int locationTypeId)
        {
            List<AttributeDefinition> definitions = FillAttributeDefinitionInfoCollection(DataProvider.Instance().GetAttributeDefinitionsById(locationTypeId));
            return definitions;
        }

        #endregion

        #region "Attribute Definition Methods"

        /// -----------------------------------------------------------------------------
        /// <summary>
        /// Adds a Property Defintion to the Data Store
        /// </summary>
        /// <param name="definition">An LocationTypeAttributeDefinition object</param>
        /// <returns>The Id of the definition (or if negative the errorcode of the error)</returns>
        /// -----------------------------------------------------------------------------
        public static int AddAttributeDefinition(AttributeDefinition definition)
        {
            if (definition.Required)
            {
                definition.Visible = true;
            }

            int intDefinition = DataProvider.Instance().AddAttributeDefinition(definition.PortalId, definition.LocationTypeId, definition.DataType, definition.DefaultValue, definition.AttributeName, definition.Required, definition.ValidationExpression, definition.ViewOrder, definition.Visible, definition.Length);

            ClearLocationTypeDefinitionCache(definition.PortalId);

            return intDefinition;
        }

        /// -----------------------------------------------------------------------------
        /// <summary>
        /// Clears the LocationType Definitions Cache
        /// </summary>
        /// <param name="PortalId">Id of the Portal</param>
        /// -----------------------------------------------------------------------------
        public static void ClearLocationTypeDefinitionCache(int PortalId)
        {
            DataCache.ClearDefinitionsCache(PortalId);
        }

        /// -----------------------------------------------------------------------------
        /// <summary>
        /// Deletes a Property Defintion from the Data Store
        /// </summary>
        /// <param name="definition">The LocationTypeAttributeDefinition object to delete</param>
        /// -----------------------------------------------------------------------------
        public static void DeleteAttributeDefinition(AttributeDefinition definition)
        {
            DataProvider.Instance().DeleteAttributeDefinition(definition.AttributeDefinitionId);
            ClearLocationTypeDefinitionCache(definition.PortalId);
        }

        /// -----------------------------------------------------------------------------
        /// <summary>
        /// Gets a Property Defintion from the Data Store by id
        /// </summary>
        /// <param name="definitionId">The id of the LocationTypeAttributeDefinition object to retrieve</param>
        /// <returns>The LocationTypeAttributeDefinition object</returns>
        /// -----------------------------------------------------------------------------
        public static AttributeDefinition GetAttributeDefinition(int definitionId, int portalId)
        {
            AttributeDefinition definition = null;
            bool bFound = Null.NullBoolean;

            foreach (AttributeDefinition singleDefinition in GetAttributeDefinitions(portalId))
            {
                if (singleDefinition.AttributeDefinitionId == definitionId)
                {
                    bFound = true;
                    definition = singleDefinition;
                    break; // TODO: might not be correct. Was : Exit For
                }
            }

            if (!bFound)
            {
                //Try Database
                definition = FillAttributeDefinitionInfo(DataProvider.Instance().GetAttributeDefinition(definitionId));
            }

            return definition;
        }

        /// -----------------------------------------------------------------------------
        /// <summary>
        /// Gets a Property Defintion from the Data Store by name
        /// </summary>
        /// <param name="portalId">The id of the Portal</param>
        /// <param name="name">The name of the LocationTypeAttributeDefinition object to retrieve</param>
        /// <returns>The LocationTypeAttributeDefinition object</returns>
        /// -----------------------------------------------------------------------------
        public static AttributeDefinition GetAttributeDefinitionByName(int portalId, string name)
        {
            AttributeDefinition definition = null;
            bool bFound = Null.NullBoolean;

            foreach (AttributeDefinition singleDefinition in GetAttributeDefinitions(portalId))
            {
                if (singleDefinition.AttributeName == name)
                {
                    bFound = true;
                    definition = singleDefinition;
                    break; // TODO: might not be correct. Was : Exit For
                }
            }

            if (!bFound)
            {
                //Try Database
                definition = FillAttributeDefinitionInfo(DataProvider.Instance().GetAttributeDefinitionByName(portalId, name));
            }

            return definition;
        }


        public static AttributeDefinitionCollection GetAttributeDefinitionsById(int portalId)
        {
            return GetAttributeDefinitionsById(portalId, true);
        }

        /// -----------------------------------------------------------------------------
        /// <summary>
        /// Gets a collection of Property Defintions from the Data Store by portal
        /// </summary>
        /// <param name="portalId">The id of the Portal</param>
        /// <returns>A LocationTypeAttributeDefinitionCollection object</returns>
        /// -----------------------------------------------------------------------------
        public static AttributeDefinitionCollection GetAttributeDefinitionsById(int locationTypeId, bool clone)
        {
            string key = String.Format(CacheKey, locationTypeId);

            //Try fetching the List from the Cache
            AttributeDefinitionCollection attributes = (AttributeDefinitionCollection)DataCache.GetCache(key);

            if (attributes == null)
            {
                attributes = new AttributeDefinitionCollection();
                int timeOut = CacheTimeOut * Convert.ToInt32(DotNetNuke.Common.Globals.PerformanceSetting);

                foreach (AttributeDefinition definition in GetAttributeDefinitions(locationTypeId))
                {
                    if (clone)
                    {
                        attributes.Add(definition.Clone());
                    }
                    else
                    {
                        attributes.Add(definition);
                    }
                }

                if (timeOut > 0)
                {
                    DataCache.SetCache(key, attributes, TimeSpan.FromMinutes(timeOut));
                }
            }

            return attributes;
        }

        public static void UpdateAttributeDefinition(AttributeDefinition definition)
        {
            if (definition.Required)
            {
                definition.Visible = true;
            }
            DataProvider.Instance().UpdateAttributeDefinition(definition.AttributeDefinitionId, definition.DataType, definition.DefaultValue, definition.AttributeName, definition.Required, definition.ValidationExpression, definition.ViewOrder, definition.Visible, definition.Length);

            ClearLocationTypeDefinitionCache(definition.PortalId);
        }

        #endregion
    }
}