using System;
using System.Text;
using System.Web;
using System.Xml;
using System.Net;

namespace Engage.Dnn.Locator
{
    using System.Globalization;

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
            locationResult.StatusCode = YahooStatusCode.BadRequest;
            cityStateResult.StatusCode = YahooStatusCode.BadRequest;

            if (!String.IsNullOrEmpty(apiKey))
            {
                if (!String.IsNullOrEmpty(location))
                {
                    locationResult = SearchYahoo("&location=" + HttpUtility.UrlEncode(location), apiKey);
                }

                if (locationResult.AccuracyCode < YahooAccuracyCode.Zip)
                {
                    string cityStateParams = GetCityStateParams(street, city, state, zip);
                    if (!String.IsNullOrEmpty(cityStateParams))
                    {
                        cityStateResult = SearchYahoo(cityStateParams, apiKey);
                    }
                    
                    if (locationResult.StatusCode == YahooStatusCode.Success && cityStateResult.StatusCode != YahooStatusCode.Success)
                    {
                        return locationResult;
                    }
                    
                    if (locationResult.StatusCode != YahooStatusCode.Success && cityStateResult.StatusCode == YahooStatusCode.Success)
                    {
                        return cityStateResult;
                    }
                    
                    if (locationResult.StatusCode == YahooStatusCode.Success && cityStateResult.StatusCode == YahooStatusCode.Success)
                    {
                        return locationResult.AccuracyCode > cityStateResult.AccuracyCode ? locationResult : cityStateResult;
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

                        result.AccuracyCode = GetYahooAccuracyCode(resultsReader.GetAttribute("precision"));
                        resultsReader.ReadStartElement("Result");

                        resultsReader.ReadStartElement("Latitude");
                        result.Latitude = double.Parse(resultsReader.ReadString());
                        resultsReader.ReadEndElement();

                        resultsReader.ReadStartElement("Longitude");
                        result.Longitude = double.Parse(resultsReader.ReadString());

                        result.StatusCode = YahooStatusCode.Success;
                    }
                    else
                    {
                        result.AccuracyCode = YahooAccuracyCode.Unknown;
                    }
                }
            }
            catch (WebException exc)
            {
                result.AccuracyCode = YahooAccuracyCode.Unknown;
                if (exc.Status == WebExceptionStatus.ProtocolError)
                {
                    HttpWebResponse response = (HttpWebResponse)exc.Response;
                    switch (response.StatusCode)
                    {
                        // 400
                        case HttpStatusCode.BadRequest:
                            result.StatusCode = YahooStatusCode.BadRequest;
                            break;

                        // 403
                        case HttpStatusCode.Forbidden:
                            result.StatusCode = YahooStatusCode.Forbidden;
                            break;

                        // 503
                        case HttpStatusCode.ServiceUnavailable: 
                            result.StatusCode = YahooStatusCode.ServiceUnavailable;
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

                    result.statusCode = (GoogleStatusCode)int.Parse(csvResults[0], CultureInfo.InvariantCulture);
                    result.accuracyCode = (GoogleAccuracyCode)int.Parse(csvResults[1], CultureInfo.InvariantCulture);
                    result.latitude = double.Parse(csvResults[2], CultureInfo.InvariantCulture);
                    result.longitude = double.Parse(csvResults[3], CultureInfo.InvariantCulture);

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
