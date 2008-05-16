using System;
using System.Text;
using System.Web;
using System.Xml;
using System.Net;
using Engage.Dnn.Locator.Providers.MapProviders;

namespace Engage.Dnn.Locator
{
    //NUKE THIS CLASS - behavior should be on mapprovider class(es).hk!
    public static class SearchUtility
    {
        public static bool IsYahooMoreAccurateThanGoogle(YahooAccuracyCode yahoo, GoogleAccuracyCode google)
        {
            if ((int)yahoo > -1) //if yahoo is positive, then they can just be compared based on number
            {
                return (int)yahoo > (int)google;
            }
            //otherwise, yahoo has extra zip accuracy.  Google wins if it is more accurate than zip.
            return google > GoogleAccuracyCode.PostalCode;
        }

        public static YahooGeocodeResult SearchYahoo(string location, string street, string city, string state, string zip, string apiKey)
        {
            YahooGeocodeResult locationResult = new YahooGeocodeResult();
            YahooGeocodeResult cityStateResult = new YahooGeocodeResult();
            locationResult.statusCode = YahooStatusCode.BadRequest;
            cityStateResult.statusCode = YahooStatusCode.BadRequest;

            if (!String.IsNullOrEmpty(apiKey))
            {
                if (!String.IsNullOrEmpty(location))
                {
                    locationResult = SearchYahoo("&location=" + HttpUtility.UrlEncode(location), apiKey);
                }

                if (locationResult.accuracyCode < YahooAccuracyCode.Zip)
                {
                    string cityStateParams = GetCityStateParams(street, city, state, zip);
                    if (!String.IsNullOrEmpty(cityStateParams))
                    {
                        cityStateResult = SearchYahoo(cityStateParams, apiKey);
                    }
                    
                    if (locationResult.statusCode == YahooStatusCode.Success && cityStateResult.statusCode != YahooStatusCode.Success)
                    {
                        return locationResult;
                    }
                    else if (locationResult.statusCode != YahooStatusCode.Success && cityStateResult.statusCode == YahooStatusCode.Success)
                    {
                        return cityStateResult;
                    }
                    else if (locationResult.statusCode == YahooStatusCode.Success && cityStateResult.statusCode == YahooStatusCode.Success)
                    {
                        return locationResult.accuracyCode > cityStateResult.accuracyCode ? locationResult : cityStateResult;
                    }
                }
            }
            return locationResult;
        }

        private static YahooGeocodeResult SearchYahoo(string queryParams, string apiKey)
        {
            YahooGeocodeResult result = new YahooGeocodeResult();

            Uri searchUrl = new Uri(YahooProvider.SearchUrl + queryParams + "&appid=" + apiKey);
            try
            {
                using (XmlReader resultsReader = XmlReader.Create(searchUrl.ToString()))
                {
                    resultsReader.Read();
                    if (resultsReader.IsStartElement("ResultSet"))
                    {
                        resultsReader.ReadStartElement("ResultSet");

                        result.accuracyCode = GetYahooAccuracyCode(resultsReader.GetAttribute("precision"));
                        resultsReader.ReadStartElement("Result");

                        resultsReader.ReadStartElement("Latitude");
                        result.latitude = double.Parse(resultsReader.ReadString());
                        resultsReader.ReadEndElement();

                        resultsReader.ReadStartElement("Longitude");
                        result.longitude = double.Parse(resultsReader.ReadString());

                        result.statusCode = YahooStatusCode.Success;
                    }
                    else
                    {
                        result.accuracyCode = YahooAccuracyCode.Unknown;
                    }
                }
            }
            catch (WebException exc)
            {
                result.accuracyCode = YahooAccuracyCode.Unknown;
                if (exc.Status == WebExceptionStatus.ProtocolError)
                {
                    HttpWebResponse response = (HttpWebResponse)exc.Response;
                    switch (response.StatusCode)
                    {
                        case HttpStatusCode.BadRequest: //400
                            result.statusCode = YahooStatusCode.BadRequest;
                            break;
                        case HttpStatusCode.Forbidden: //403
                            result.statusCode = YahooStatusCode.Forbidden;
                            break;
                        case HttpStatusCode.ServiceUnavailable: //503
                            result.statusCode = YahooStatusCode.ServiceUnavailable;
                            break;
                    }
                }
            }
            return result;
        }

        public static GoogleGeocodeResult SearchGoogle(string location, string apiKey)
        {
            GoogleGeocodeResult result = new GoogleGeocodeResult();
            if (!String.IsNullOrEmpty(apiKey))
            {
                try
                {
                    WebClient client = new WebClient();
                    string[] csvResults = client.DownloadString(GoogleProvider.SearchUrl + "&q=" + HttpUtility.UrlEncode(location) + "&key=" + apiKey).Split(',');

                    result.statusCode = (GoogleStatusCode)int.Parse(csvResults[0]);
                    result.accuracyCode = (GoogleAccuracyCode)int.Parse(csvResults[1]);
                    result.latitude = double.Parse(csvResults[2]);
                    result.longitude = double.Parse(csvResults[3]);

                    return result;
                }
                catch (WebException)
                {
                    result.statusCode = GoogleStatusCode.ServerError;
                    return result;
                }
            }
            result.statusCode = GoogleStatusCode.MissingAddress;
            return result;
        }

        private static string GetCityStateParams(string street, string city, string state, string zip)
        {
            StringBuilder queryParams = new StringBuilder();

            if (!string.IsNullOrEmpty(street))
            {
                queryParams.Append("&street=" + HttpUtility.UrlEncode(street));
            }
            if (!string.IsNullOrEmpty(city))
            {
                queryParams.Append("&city=" + HttpUtility.UrlEncode(city));
            }
            if (!string.IsNullOrEmpty(state))
            {
                queryParams.Append("&state=" + HttpUtility.UrlEncode(state));
            }
            if (!string.IsNullOrEmpty(zip))
            {
                queryParams.Append("&zip=" + HttpUtility.UrlEncode(zip));
            }

            return queryParams.ToString();
        }

        private static YahooAccuracyCode GetYahooAccuracyCode(string accuracyValue)
        {
            YahooAccuracyCode accuracyCode;
            switch (accuracyValue)
            {
                case "country":
                    accuracyCode = YahooAccuracyCode.Country;
                    break;
                case "state":
                    accuracyCode = YahooAccuracyCode.State;
                    break;
                case "city":
                    accuracyCode = YahooAccuracyCode.City;
                    break;
                case "zip":
                    accuracyCode = YahooAccuracyCode.Zip;
                    break;
                case "zip+2":
                    accuracyCode = YahooAccuracyCode.ZipPlus2;
                    break;
                case "zip+4":
                    accuracyCode = YahooAccuracyCode.ZipPlus4;
                    break;
                case "street":
                    accuracyCode = YahooAccuracyCode.Street;
                    break;
                case "address":
                    accuracyCode = YahooAccuracyCode.Address;
                    break;
                default:
                    accuracyCode = YahooAccuracyCode.Unknown;
                    break;
            }
            return accuracyCode;
        }
    }
   
}
