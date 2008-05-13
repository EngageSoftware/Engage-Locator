using DotNetNuke.UI;
using System.Collections;

namespace Engage.Dnn.Locator
{

    /// -----------------------------------------------------------------------------
    /// Project:    DotNetNuke
    /// Namespace:  DotNetNuke.Entities.Profile
    /// Class:      LocationTypeAttributeDefinitionCollection
    /// -----------------------------------------------------------------------------
    /// <summary>
    /// The LocationTypeAttributeDefinitionCollection class provides Business Layer methods for 
    /// a collection of property Definitions
    /// </summary>
    /// <remarks>
    /// </remarks>
    /// <history>
    ///     [cnurse]	01/31/2006	created
    /// </history>
    /// -----------------------------------------------------------------------------
    public class LocationTypeAttributeDefinitionCollection : CollectionBase
    {
        //int _index = DotNetNuke.Common.Utilities.Null.NullInteger;
        //string _name = DotNetNuke.Common.Utilities.Null.NullString;

        #region "Constructors"

        /// -----------------------------------------------------------------------------
        /// <summary>
        /// Constructs a new default collection
        /// </summary>
        /// <history>
        ///     [cnurse]	01/31/2006	created
        /// </history>
        /// -----------------------------------------------------------------------------
        public LocationTypeAttributeDefinitionCollection() : base()
        {
        }

        /// -----------------------------------------------------------------------------
        /// <summary>
        /// Constructs a new Collection from an ArrayList of LocationTypeAttributeDefinition objects
        /// </summary>
        /// <param name="definitionsList">An ArrayList of LocationTypeAttributeDefinition objects</param>
        /// <history>
        ///     [cnurse]	01/31/2006	created
        /// </history>
        /// -----------------------------------------------------------------------------
        public LocationTypeAttributeDefinitionCollection(ArrayList definitionsList)
        {
            AddRange(definitionsList);
        }

        /// -----------------------------------------------------------------------------
        /// <summary>
        /// Constructs a new Collection from a LocationTypeAttributeDefinitionCollection
        /// </summary>
        /// <param name="collection">A LocationTypeAttributeDefinitionCollection</param>
        /// <history>
        ///     [cnurse]	01/31/2006	created
        /// </history>
        /// -----------------------------------------------------------------------------
        public LocationTypeAttributeDefinitionCollection(LocationTypeAttributeDefinitionCollection collection)
        {
            AddRange(collection);
        }

        #endregion

        #region "Public Properties"

        ///// -----------------------------------------------------------------------------
        ///// <summary>
        ///// Gets and sets an item in the collection.
        ///// </summary>
        ///// <remarks>This overload returns the item by its index. </remarks>
        ///// <param name="index">The index to get</param>
        ///// <returns>A LocationTypeAttributeDefinition object</returns>
        ///// <history>
        /////     [cnurse]	01/31/2006	created
        ///// </history>
        ///// -----------------------------------------------------------------------------
        public LocationTypeAttributeDefinition this [int index]
        {
            get { return (LocationTypeAttributeDefinition)List[index]; }
            set { List[index] = value; }
        }

        ///// -----------------------------------------------------------------------------
        ///// <summary>
        ///// Gets an item in the collection.
        ///// </summary>
        ///// <remarks>This overload returns the item by its name</remarks>
        ///// <param name="name">The name of the Property to get</param>
        ///// <returns>A LocationTypeAttributeDefinition object</returns>
        ///// <history>
        /////     [cnurse]	01/31/2006	created
        ///// </history>
        ///// -----------------------------------------------------------------------------
        public LocationTypeAttributeDefinition this [string name]
        {
            get { return GetByName(name); }
        }

        #endregion

        #region "Public Methods"

        /// -----------------------------------------------------------------------------
        /// <summary>
        /// Adds a property Definition to the collectio.
        /// </summary>
        /// <param name="value">A LocationTypeAttributeDefinition object</param>
        /// <returns>The index of the property Definition in the collection</returns>
        /// <history>
        ///     [cnurse]	01/31/2006	created
        /// </history>
        /// -----------------------------------------------------------------------------
        public int Add(LocationTypeAttributeDefinition value)
        {
            return List.Add(value);
        }

        /// -----------------------------------------------------------------------------
        /// <summary>
        /// Add an ArrayList of LocationTypeAttributeDefinition objects
        /// </summary>
        /// <param name="definitionsList">An ArrayList of LocationTypeAttributeDefinition objects</param>
        /// <history>
        ///     [cnurse]	01/31/2006	created
        /// </history>
        /// -----------------------------------------------------------------------------
        public void AddRange(ArrayList definitionsList)
        {
            foreach (LocationTypeAttributeDefinition objLocationTypeAttributeDefinition in definitionsList)
            {
                Add(objLocationTypeAttributeDefinition);
            }
        }

        /// -----------------------------------------------------------------------------
        /// <summary>
        /// Add an existing LocationTypeAttributeDefinitionCollection
        /// </summary>
        /// <param name="collection">A LocationTypeAttributeDefinitionCollection</param>
        /// <history>
        ///     [cnurse]	01/31/2006	created
        /// </history>
        /// -----------------------------------------------------------------------------
        public void AddRange(LocationTypeAttributeDefinitionCollection collection)
        {
            foreach (LocationTypeAttributeDefinition objLocationTypeAttributeDefinition in collection)
            {
                Add(objLocationTypeAttributeDefinition);
            }
        }

        /// -----------------------------------------------------------------------------
        /// <summary>
        /// Determines whether the collection contains a property definition
        /// </summary>
        /// <param name="value">A LocationTypeAttributeDefinition object</param>
        /// <returns>A Boolean True/False</returns>
        /// <history>
        ///     [cnurse]	01/31/2006	created
        /// </history>
        /// -----------------------------------------------------------------------------
        public bool Contains(LocationTypeAttributeDefinition value)
        {
            return List.Contains(value);
        }

        /// -----------------------------------------------------------------------------
        /// <summary>
        /// Gets a sub-collection of items in the collection by category.
        /// </summary>
        /// <param name="category">The category to get</param>
        /// <returns>A LocationTypeAttributeDefinitionCollection object</returns>
        /// <history>
        ///     [cnurse]	01/31/2006	created
        /// </history>
        /// -----------------------------------------------------------------------------
        public LocationTypeAttributeDefinitionCollection GetByCategory(string category)
        {

            LocationTypeAttributeDefinitionCollection collection = new LocationTypeAttributeDefinitionCollection();
            //foreach (LocationTypeAttributeDefinition locationTypeAttribute in InnerList)
            //{
            //    if (locationTypeAttribute.AttributeCategory == category)
            //    {
            //        //Found Profile property that satisfies category
            //        collection.Add(locationTypeAttribute);
            //    }
            //}
            return collection;

        }

        /// -----------------------------------------------------------------------------
        /// <summary>
        /// Gets an item in the collection by Id.
        /// </summary>
        /// <param name="id">The id of the Property to get</param>
        /// <returns>A LocationTypeAttributeDefinition object</returns>
        /// <history>
        ///     [cnurse]	01/31/2006	created
        /// </history>
        /// -----------------------------------------------------------------------------
        public LocationTypeAttributeDefinition GetById(int id)
        {

            LocationTypeAttributeDefinition locationTypeItem = null;
            foreach (LocationTypeAttributeDefinition locationTypeAttribute in InnerList)
            {
                if (locationTypeAttribute.AttributeDefinitionId == id)
                {
                    //Found Profile property
                    locationTypeItem = locationTypeAttribute;
                }
            }
            return locationTypeItem;

        }


        /// -----------------------------------------------------------------------------
        /// <summary>
        /// Gets an item in the collection by name.
        /// </summary>
        /// <param name="name">The name of the Property to get</param>
        /// <returns>A LocationTypeAttributeDefinition object</returns>
        /// <history>
        ///     [cnurse]	01/31/2006	created
        /// </history>
        /// -----------------------------------------------------------------------------
        public LocationTypeAttributeDefinition GetByName(string name)
        {

            LocationTypeAttributeDefinition locationTypeItem = null;
            foreach (LocationTypeAttributeDefinition locationTypeAttribute in InnerList)
            {
                if (locationTypeAttribute.AttributeName == name)
                {
                    //Found Profile property
                    locationTypeItem = locationTypeAttribute;
                }
            }
            return locationTypeItem;

        }

        /// -----------------------------------------------------------------------------
        /// <summary>
        /// Gets the index of a property Definition
        /// </summary>
        /// <param name="value">A LocationTypeAttributeDefinition object</param>
        /// <returns>The index of the property Definition in the collection</returns>
        /// <history>
        ///     [cnurse]	01/31/2006	created
        /// </history>
        /// -----------------------------------------------------------------------------
        public int IndexOf(LocationTypeAttributeDefinition value)
        {
            return List.IndexOf(value);
        }

        /// -----------------------------------------------------------------------------
        /// <summary>
        /// Inserts a property Definition into the collectio.
        /// </summary>
        /// <param name="value">A LocationTypeAttributeDefinition object</param>
        /// <param name="index">The index to insert the item at</param>
        /// <history>
        ///     [cnurse]	01/31/2006	created
        /// </history>
        /// -----------------------------------------------------------------------------
        public void Insert(int index, LocationTypeAttributeDefinition value)
        {
            List.Insert(index, value);
        }

        /// -----------------------------------------------------------------------------
        /// <summary>
        /// Removes a property definition from the collection
        /// </summary>
        /// <param name="value">The LocationTypeAttributeDefinition object to remove</param>
        /// <history>
        ///     [cnurse]	01/31/2006	created
        /// </history>
        /// -----------------------------------------------------------------------------
        public void Remove(LocationTypeAttributeDefinition value)
        {
            List.Remove(value);
        }

        /// -----------------------------------------------------------------------------
        /// <summary>
        /// Sorts the collection using the LocationTypeAttributeDefinitionComparer (ie by ViewOrder)
        /// </summary>
        /// <history>
        ///     [cnurse]	01/31/2006	created
        /// </history>
        /// -----------------------------------------------------------------------------
        public void Sort()
        {
            InnerList.Sort(new LocationTypeAttributeDefinitionComparer());
        }

        #endregion

    }
}