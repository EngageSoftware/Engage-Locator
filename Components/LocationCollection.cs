// <copyright file="LocationCollection.cs" company="Engage Software">
// Engage: Locator - http://www.engagemodules.com
// Copyright (c) 2004-2009
// by Engage Software ( http://www.engagesoftware.com )
// </copyright>
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED 
// TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL 
// THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF 
// CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER 
// DEALINGS IN THE SOFTWARE.

namespace Engage.Dnn.Locator
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;

    using JavaScript;

    /// <summary>
    /// A strongly-typed collection of <see cref="Location"/>s, 
    /// providing access to the <see cref="TotalRecords"/> a given query accesses
    /// </summary>
    public class LocationCollection : Collection<Location>
    {
        /// <summary>
        /// Backing field for <see cref="TotalRecords"/>
        /// </summary>
        private int totalRecords;

        /// <summary>
        /// Initializes a new instance of the <see cref="LocationCollection"/> class.
        /// </summary>
        public LocationCollection()
            : this(0)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LocationCollection"/> class.
        /// </summary>
        /// <param name="totalRecords">The total number of locations that this query returns, without regard to paging.</param>
        public LocationCollection(int totalRecords)
        {
            this.totalRecords = totalRecords;
        }

        /// <summary>
        /// Gets or sets the total number of locations available to the query that populated this collection.  
        /// That is, the number of locations in this collection, unless this collection is part of a paged query; 
        /// then the total number of locations without regard to paging.
        /// </summary>
        /// <value>The total number of locations that this query returns, without regard to paging.</value>
        public int TotalRecords
        {
            get { return this.totalRecords; }
            set { this.totalRecords = value; }
        }

        /// <summary>
        /// Converts this <see cref="LocationCollection"/> to a generic <see cref="ICollection{T}"/> of <see cref="JavaScript.Location"/>
        /// </summary>
        /// <returns>An <see cref="ICollection{T}"/> that represents this list as lightweight, JSON-compatible objects</returns>
        public ICollection<JavaScript.Location> AsJson()
        {
            Collection<JavaScript.Location> jsonLocations = new Collection<JavaScript.Location>();
            foreach (Location location in this)
            {
                jsonLocations.Add(new JavaScript.Location(location.Latitude, location.Longitude, location.FullAddress + Environment.NewLine + location.Address3));
            }

            return jsonLocations;
        }

        /// <summary>
        /// Inserts an element into the <see cref="T:System.Collections.ObjectModel.Collection`1"/> at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index at which <paramref name="item"/> should be inserted.</param>
        /// <param name="item">The object to insert. The value can be null for reference types.</param>
        /// <exception cref="T:System.ArgumentOutOfRangeException">
        /// <paramref name="index"/> is less than zero.
        /// -or-
        /// <paramref name="index"/> is greater than <see cref="P:System.Collections.ObjectModel.Collection`1.Count"/>.
        /// </exception>
        protected override void InsertItem(int index, Location item)
        {
            base.InsertItem(index, item);

            // make sure TotalRecords is a valid value
            if (this.Count > this.totalRecords)
            {
                this.totalRecords = this.Count;
            }
        }
    }
}