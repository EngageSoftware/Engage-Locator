//Copyright (c) 2004-2007
//by Engage Software ( http://www.engagesoftware.com )

//THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED 
//TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL 
//THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF 
//CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER 
//DEALINGS IN THE SOFTWARE.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Reflection;

namespace Engage.Dnn.Locator.Maps
{
    public class MapProviderType
    {
        public static readonly MapProviderType GoogleMaps = new MapProviderType("Google", "Engage.Dnn.Locator.GoogleProvider");
        public static readonly MapProviderType YahooMaps = new MapProviderType("Yahoo", "Engage.Dnn.Locator.YahooProvider");

        private readonly string _name;
        private readonly string _className;

        private MapProviderType(string name, string className)
        {
            _name = name;
            _className = className;
        }

        public static ReadOnlyCollection<MapProviderType> GetMapProviderTypes()
        {
            Type t = typeof(MapProviderType);
            List<MapProviderType> types = new List<MapProviderType>();

            FieldInfo[] fi = t.GetFields();

            foreach (FieldInfo f in fi)
            {
                MapProviderType mp = f.GetValue(t) as MapProviderType;
                if (mp != null)
                {
                    types.Add(mp);
                }
            }
            return types.AsReadOnly();
        }

        public static string GetMapProviderClassByName(string name)
        {
            Type t = typeof(MapProviderType);

            FieldInfo[] fi = t.GetFields();

            foreach (FieldInfo f in fi)
            {
                MapProviderType mp = f.GetValue(t) as MapProviderType;
                if (mp != null)
                {
                    if (mp.Name == name)
                    {
                        return mp.ClassName;
                    }
                }
            }

            return "";
        }

        public string Name
        {
            get { return _name; }
        }

        public string ClassName
        {
            get { return _className; }
        }

        public static MapProviderType GetFromName(string name, Type ct)
        {
            if (name == null) throw new ArgumentNullException("name");
            if (name == string.Empty) throw new ArgumentOutOfRangeException("name");
            if (ct == null) throw new ArgumentNullException("ct");

            Type type = ct;
            while (type.BaseType != null)
            {
                FieldInfo[] fi = type.GetFields();

                foreach (FieldInfo f in fi)
                {
                    Object o = f.GetValue(type);
                    if (o is MapProviderType)
                    {
                        MapProviderType cot = (MapProviderType)o;

                        //this prevents old, bogus classes defined in the code from killing the app
                        //client needs to check the return value
                        try
                        {
                            if (name == cot.Name)
                            {
                                return cot;
                            }
                        }
                        catch (Exception)
                        {
                            //drive on
                        }
                    }
                }

                type = type.BaseType; //check the super type 
            }

            return null;
        }
    }
}
