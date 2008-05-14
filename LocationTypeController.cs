using System.Collections.Generic;
using DotNetNuke.Common.Lists;
using DotNetNuke.Entities.Modules;
using System.Windows.Forms;
using Engage.Dnn.Locator.Data;
using System.Data;
using System;
using Microsoft.ApplicationBlocks.Data;
using DotNetNuke.Common.Utilities;
using System.Collections;
using System.Data.SqlClient;
using Engage.Dnn.Locator.DataTemp;

namespace Engage.Dnn.Locator
{
    public class LocationTypeController
    {
        public const string CacheKey = "AttributeDefinitions{0}";
        public const int CacheTimeOut = 20;


        #region "Private Shared Members"

        private static AttributesDataProvider provider = AttributesDataProvider.Instance();
        //private static DotNetNuke.Security.Profile.ProfileProvider profileProvider = DotNetNuke.Security.Profile.ProfileProvider.Instance();
        private static int _orderCounter;

        #endregion

        #region "Private Shared Methods"

        /// -----------------------------------------------------------------------------
        /// <summary>
        /// Adds a single default property definition
        /// </summary>
        /// <param name="PortalId">Id of the Portal</param>
        /// <param name="category">Category of the Property</param>
        /// <param name="name">Name of the Property</param>
        /// -----------------------------------------------------------------------------
        //private static void AddDefaultDefinition(int PortalId, string category, string name, string strType, int length, ListEntryInfoCollection types)
        //{

        //    ListEntryInfo typeInfo = types.Item("DataType." + strType);
        //    if (typeInfo == null)
        //    {
        //        typeInfo = types.Item("DataType.Unknown");
        //    }

        //    LocationTypeAttributeDefinition attributeDefinition = new LocationTypeAttributeDefinition();
        //    attributeDefinition.DataType = typeInfo.EntryID;
        //    attributeDefinition.DefaultValue = "";
        //    attributeDefinition.LocationTypeId = Null.NullInteger;
        //    attributeDefinition.PortalId = PortalId;
        //    //attributeDefinition.AttributeCategory = category;
        //    attributeDefinition.AttributeName = name;
        //    attributeDefinition.Required = false;
        //    attributeDefinition.Visible = true;
        //    attributeDefinition.Length = length;

        //    _orderCounter += 2;

        //    attributeDefinition.ViewOrder = _orderCounter;

        //    AddAttributeDefinition(attributeDefinition);

        //}

        /// -----------------------------------------------------------------------------
        /// <summary>
        /// Fills a LocationTypeAttributeDefinitionCollection from a DataReader
        /// </summary>
        /// <param name="dr">An IDataReader object</param>
        /// <returns>The LocationTypeAttributeDefinitionCollection</returns>
        /// -----------------------------------------------------------------------------
        private static LocationTypeAttributeDefinitionCollection FillCollection(IDataReader dr)
        {
            ArrayList arrDefinitions = CBO.FillCollection(dr, typeof(LocationTypeAttributeDefinition));
            LocationTypeAttributeDefinitionCollection definitionsCollection = new LocationTypeAttributeDefinitionCollection();
            foreach (LocationTypeAttributeDefinition definition in arrDefinitions)
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

        private static LocationTypeAttributeDefinition FillAttributeDefinitionInfo(IDataReader dr)
        {
            LocationTypeAttributeDefinition definition = null;

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

        private static LocationTypeAttributeDefinition FillAttributeDefinitionInfo(IDataReader dr, bool checkForOpenDataReader)
        {
            LocationTypeAttributeDefinition definition = null;

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
                definition = new LocationTypeAttributeDefinition();
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

        private static List<LocationTypeAttributeDefinition> FillAttributeDefinitionInfoCollection(IDataReader dr)
        {
            List<LocationTypeAttributeDefinition> arr = new List<LocationTypeAttributeDefinition>();
            try
            {
                LocationTypeAttributeDefinition obj;
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

        private static List<LocationTypeAttributeDefinition> GetAttributeDefinitions(int locationTypeId)
        {
            List<LocationTypeAttributeDefinition> definitions = FillAttributeDefinitionInfoCollection(provider.GetAttributeDefinitionsByLTID(locationTypeId));
            return definitions;
        }

        #endregion

        #region "Profile Methods"

        /// -----------------------------------------------------------------------------
        /// <summary>
        /// Gets the Profile Information for the User
        /// </summary>
        /// <remarks></remarks>
        /// <param name="objUser">The user whose Profile information we are retrieving.</param>
        /// <history>
        /// 	[cnurse]	12/13/2005	Created
        /// </history>
        /// -----------------------------------------------------------------------------
        //public static void GetUserProfile(ref UserInfo objUser)
        //{

        //    profileProvider.GetUserProfile(objUser);

        //}

        /// -----------------------------------------------------------------------------
        /// <summary>
        /// Updates a User's Profile
        /// </summary>
        /// <param name="objUser">The use to update</param>
        /// <remarks>
        /// </remarks>
        /// <history>
        /// 	[cnurse]	02/18/2005	Documented
        /// </history>
        /// -----------------------------------------------------------------------------
        //public static void UpdateUserProfile(UserInfo objUser)
        //{
        //    //Update the User Profile
        //    if (objUser.Profile.IsDirty)
        //    {
        //        profileProvider.UpdateUserProfile(objUser);
        //    }

        //    //Remove the UserInfo from the Cache, as it has been modified
        //    DataCache.ClearUserCache(objUser.PortalID, objUser.Username);
        //}

        /// -----------------------------------------------------------------------------
        /// <summary>
        /// Updates a User's Profile
        /// </summary>
        /// <param name="objUser">The use to update</param>
        /// <param name="profileProperties">The collection of profile properties</param>
        /// <returns>The updated User</returns>
        /// <history>
        /// 	[cnurse]	03/02/2006  Created
        /// </history>
        /// -----------------------------------------------------------------------------
        //public static UserInfo UpdateUserProfile(UserInfo objUser, LocationTypeAttributeDefinitionCollection profileProperties)
        //{

        //    bool updateUser = Null.NullBoolean;

        //    //Iterate through the Definitions
        //    foreach (LocationTypeAttributeDefinition attributeDefinition in profileProperties)
        //    {

        //        string attributeName = attributeDefinition.AttributeName;
        //        string attributeValue = attributeDefinition.AttributeValue;

        //        if (attributeDefinition.IsDirty)
        //        {
        //            //Update Profile
        //            objUser.Profile.SetLocationTypeAttribute(attributeName, attributeValue);

        //            if (attributeName.ToLower == "firstname" || attributeName.ToLower == "lastname")
        //            {
        //                updateUser = true;
        //            }
        //        }
        //    }

        //    UpdateUserProfile(objUser);

        //    if (updateUser)
        //    {
        //        UserController.UpdateUser(objUser.PortalID, objUser);
        //    }

        //    return objUser;

        //}

        /// -----------------------------------------------------------------------------
        /// <summary>
        /// Validates the Profile properties for the User (determines if all required properties
        /// have been set)
        /// </summary>
        /// <param name="portalId">The Id of the portal.</param>
        /// <param name="objProfile">The profile.</param>
        /// -----------------------------------------------------------------------------
        //public static bool ValidateProfile(int portalId, UserProfile objProfile)
        //{

        //    bool isValid = true;

        //    foreach (LocationTypeAttributeDefinition attributeDefinition in objProfile.ProfileProperties)
        //    {

        //        if (attributeDefinition.Required & attributeDefinition.AttributeValue == Null.NullString)
        //        {
        //            isValid = false;
        //            break; // TODO: might not be correct. Was : Exit For
        //        }
        //    }

        //    return isValid;

        //}

        #endregion

        #region "Attribute Definition Methods"

        /// -----------------------------------------------------------------------------
        /// <summary>
        /// Adds a Property Defintion to the Data Store
        /// </summary>
        /// <param name="definition">An LocationTypeAttributeDefinition object</param>
        /// <returns>The Id of the definition (or if negative the errorcode of the error)</returns>
        /// -----------------------------------------------------------------------------
        public static int AddAttributeDefinition(LocationTypeAttributeDefinition definition)
        {
            if (definition.Required)
            {
                definition.Visible = true;
            }

            int intDefinition = provider.AddAttributeDefinition(definition.PortalId, definition.LocationTypeId, definition.DataType, definition.DefaultValue, definition.AttributeName, definition.Required, definition.ValidationExpression, definition.ViewOrder, definition.Visible, definition.Length);

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
        public static void DeleteAttributeDefinition(LocationTypeAttributeDefinition definition)
        {
            provider.DeleteAttributeDefinition(definition.AttributeDefinitionId);
            ClearLocationTypeDefinitionCache(definition.PortalId);
        }

        /// -----------------------------------------------------------------------------
        /// <summary>
        /// Gets a Property Defintion from the Data Store by id
        /// </summary>
        /// <param name="definitionId">The id of the LocationTypeAttributeDefinition object to retrieve</param>
        /// <returns>The LocationTypeAttributeDefinition object</returns>
        /// -----------------------------------------------------------------------------
        public static LocationTypeAttributeDefinition GetAttributeDefinition(int definitionId, int portalId)
        {
            LocationTypeAttributeDefinition definition = null;
            bool bFound = Null.NullBoolean;
            
            foreach ( LocationTypeAttributeDefinition singleDefinition in GetAttributeDefinitions(portalId)) {
                if (singleDefinition.AttributeDefinitionId == definitionId) {
                    bFound = true;
                    definition = singleDefinition;
                    break; // TODO: might not be correct. Was : Exit For
                }
            }
            
            if (!bFound) {
                //Try Database
                definition = FillAttributeDefinitionInfo(provider.GetAttributeDefinition(definitionId));
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
        public static LocationTypeAttributeDefinition GetAttributeDefinitionByName(int portalId, string name)
        {
            LocationTypeAttributeDefinition definition = null;
            bool bFound = Null.NullBoolean;
            
            foreach ( LocationTypeAttributeDefinition singleDefinition in GetAttributeDefinitions(portalId)) {
                if (singleDefinition.AttributeName == name) {
                    bFound = true;
                    definition = singleDefinition;
                    break; // TODO: might not be correct. Was : Exit For
                }
            }
            
            if (!bFound) {
                //Try Database
                definition = FillAttributeDefinitionInfo(provider.GetAttributeDefinitionByName(portalId, name));
            }
            
            return definition;
        }


        public static LocationTypeAttributeDefinitionCollection GetAttributeDefinitionsById(int portalId)
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
        public static LocationTypeAttributeDefinitionCollection GetAttributeDefinitionsById(int locationTypeId, bool clone)
        {
            string key = String.Format(CacheKey, locationTypeId);

            //Try fetching the List from the Cache
            LocationTypeAttributeDefinitionCollection attributes = (LocationTypeAttributeDefinitionCollection)DataCache.GetCache(key);
                       
            if (attributes == null)
            {
                foreach (LocationTypeAttributeDefinition definition in GetAttributeDefinitions(locationTypeId))
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

                if (CacheTimeOut > 0)
               {
                   DataCache.SetCache(key, attributes, TimeSpan.FromMinutes(CacheTimeOut));
               }
            }

            return attributes;
        }

        /// -----------------------------------------------------------------------------
        /// <summary>
        /// Updates a Property Defintion in the Data Store
        /// </summary>
        /// <param name="definition">The LocationTypeAttributeDefinition object to update</param>
        /// -----------------------------------------------------------------------------
        public static void UpdateAttributeDefinition(LocationTypeAttributeDefinition definition)
        {
            if (definition.Required)
            {
                definition.Visible = true;
            }
            provider.UpdateAttributeDefinition(definition.AttributeDefinitionId, definition.DataType, definition.DefaultValue, definition.AttributeName, definition.Required, definition.ValidationExpression, definition.ViewOrder, definition.Visible, definition.Length);

            ClearLocationTypeDefinitionCache(definition.PortalId);
        }

        #endregion

        #region "Obsolete Methods"

        [Obsolete("This method has been deprecated.  Please use GetAttributeDefinition(ByVal definitionId As Integer, ByVal portalId As Integer) instead")]
        public static LocationTypeAttributeDefinition GetAttributeDefinition(int definitionId)
        {
            return (LocationTypeAttributeDefinition)CBO.FillObject(provider.GetAttributeDefinition(definitionId), typeof(LocationTypeAttributeDefinition));
        }

        #endregion

    }

}