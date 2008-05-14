using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Collections;

namespace Engage.Dnn.Locator
{
    public class AttributeDefinitionComparer : IComparer
    {
        #region "IComparer Interface"

        /// -----------------------------------------------------------------------------
        /// <summary>
        /// Compares two ProfilePropertyDefinition objects
        /// </summary>
        /// <param name="x">A ProfilePropertyDefinition object</param>
        /// <param name="y">A ProfilePropertyDefinition object</param>
        /// <returns>An integer indicating whether x greater than y, x=y or x less than y</returns>
        /// <history>
        ///     [cnurse]	01/31/2006	created
        /// </history>
        /// -----------------------------------------------------------------------------
        int System.Collections.IComparer.Compare(object x, object y)
        {
            return ((AttributeDefinition)x).ViewOrder.CompareTo(((AttributeDefinition)y).ViewOrder);
        }

        #endregion
    }
}
