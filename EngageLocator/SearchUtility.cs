using System;
using System.Text;
using System.Web;
using System.Xml;
using System.Net;

namespace Engage.Dnn.Locator
{
    public enum GoogleStatusCode
    {
        /// <summary>
        /// No errors occurred; the address was successfully parsed and its geocode has been returned.
        /// </summary>
        Success = 200,
        /// <summary>
        /// A geocoding request could not be successfully processed, yet the exact reason for the failure is not known.
        /// </summary>
        ServerError = 500,
        /// <summary>
        /// The HTTP q parameter was either missing or had no value.
        /// </summary>
        MissingAddress = 601,
        /// <summary>
        /// No corresponding geographic location could be found for the specified address. This may be due to the fact that the address is relatively new, or it may be incorrect.
        /// </summary>
        UnknownAddress = 602,
        /// <summary>
        /// The geocode for the given address cannot be returned due to legal or contractual reasons.
        /// </summary>
        UnavailableAddress = 603,
        /// <summary>
        /// The given key is either invalid or does not match the domain for which it was given.
        /// </summary>
        BadKey = 610
    }

    public enum YahooStatusCode
    {
        Success = 200,
        /// <summary>
        /// The parameters passed to the service did not match as expected. The Message should tell you what was missing or incorrect.
        /// </summary>
        BadRequest = 400,
        /// <summary>
        /// You do not have permission to access this resource, or are over your rate limit.
        /// </summary>
        Forbidden = 403,
        /// <summary>
        /// An internal problem prevented us from returning data to you.
        /// </summary>
        ServiceUnavailable = 503
    }

    public enum GoogleAccuracyCode
    {
        Unknown = 0,
        Country = 1,
        Region = 2,
        Subregion = 3,
        Town = 4,
        PostalCode = 5,
        Street = 6,
        Intersection = 7,
        Address = 8
    }

    public enum YahooAccuracyCode
    {
        Unknown = 0,
        Country = 1,
        State = 2,
        City = 4,
        Zip = 5,
        ZipPlus2 = -1,
        ZipPlus4 = -2,
        Street = 6,
        Address = 8
    }

    public static class SearchUtility
    {
        private const string yahooSearchUrl = "http://api.local.yahoo.com/MapsService/V1/geocode?output=xml";
        private const string googleSearchUrl = "http://maps.google.com/maps/geo?output=csv";

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

            Uri searchUrl = new Uri(yahooSearchUrl + queryParams + "&appid=" + apiKey);
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
                    string[] csvResults = client.DownloadString(googleSearchUrl + "&q=" + HttpUtility.UrlEncode(location) + "&key=" + apiKey).Split(',');

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

    public struct YahooGeocodeResult
    {
        public double latitude;
        public double longitude;

        public YahooAccuracyCode accuracyCode;
        public YahooStatusCode statusCode;
    }

    public struct GoogleGeocodeResult
    {
        public double latitude;
        public double longitude;

        public GoogleAccuracyCode accuracyCode;
        public GoogleStatusCode statusCode;
    }
}
