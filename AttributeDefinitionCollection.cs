using DotNetNuke.UI;
using System.Collections;

namespace Engage.Dnn.Locator
{

    public class AttributeDefinitionCollection : CollectionBase
    {
        //int _index = DotNetNuke.Common.Utilities.Null.NullInteger;
        //string _name = DotNetNuke.Common.Utilities.Null.NullString;

        #region "Constructors"

        public AttributeDefinitionCollection() : base()
        {
        }

        public AttributeDefinitionCollection(ArrayList definitionsList)
        {
            AddRange(definitionsList);
        }

        public AttributeDefinitionCollection(AttributeDefinitionCollection collection)
        {
            AddRange(collection);
        }

        #endregion

        #region "Public Properties"

        public AttributeDefinition this [int index]
        {
            get { return (AttributeDefinition)List[index]; }
            set { List[index] = value; }
        }

        public AttributeDefinition this [string name]
        {
            get { return GetByName(name); }
        }

        #endregion

        #region "Public Methods"
        public int Add(AttributeDefinition value)
        {
            return List.Add(value);
        }

        public void AddRange(ArrayList definitions)
        {
            foreach (AttributeDefinition definition in definitions)
            {
                Add(definition);
            }
        }

        public void AddRange(AttributeDefinitionCollection definitions)
        {
            foreach (AttributeDefinition definition in definitions)
            {
                Add(definition);
            }
        }

        public bool Contains(AttributeDefinition value)
        {
            return List.Contains(value);
        }

        public AttributeDefinition GetById(int id)
        {

            AttributeDefinition item = null;
            foreach (AttributeDefinition definition in InnerList)
            {
                if (definition.AttributeDefinitionId == id)
                {
                    //Found Profile property
                    item = definition;
                }
            }
            return item;

        }
        public AttributeDefinition GetByName(string name)
        {

            AttributeDefinition item = null;
            foreach (AttributeDefinition definition in InnerList)
            {
                if (definition.AttributeName == name)
                {
                    //Found Profile property
                    item = definition;
                }
            }
            return item;

        }

        public int IndexOf(AttributeDefinition value)
        {
            return List.IndexOf(value);
        }

        public void Insert(int index, AttributeDefinition value)
        {
            List.Insert(index, value);
        }

        public void Remove(AttributeDefinition value)
        {
            List.Remove(value);
        }
        public void Sort()
        {
            InnerList.Sort(new AttributeDefinitionComparer());
        }

        #endregion

    }
}