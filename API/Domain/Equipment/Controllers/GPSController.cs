using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using API.Data;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using API.Models;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;
using System.Web.Http.Cors;
using API.Utilities.Auth;
using System.Threading;
using System.Xml;
using System.Runtime.Caching;

namespace API.Controllers
{
    [JWTAuthorization]
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class GPSController : ApiController
    {
        [HttpGet]
        [Route("api/gps/")]
        public async Task<HttpResponseMessage> AllGPSData()
        {
            var gpsdata = await CallTierraService();
            if (gpsdata != null)
                return Request.CreateResponse(HttpStatusCode.OK, gpsdata);
            return Request.CreateResponse(HttpStatusCode.NoContent, "gps data not found");
        }

        [HttpGet]
        [Route("api/gps/{serialNumber}/")]
        public async Task<HttpResponseMessage> GetEquipmentGPSdata(string serialNumber)
        {
            EquipmentGPSData gpsdata = await GetGPSData(serialNumber);
            if (gpsdata != null)
                return Request.CreateResponse(HttpStatusCode.OK, gpsdata);
            return Request.CreateResponse(HttpStatusCode.NoContent, "Machine gps data not found");
        }

        [HttpGet]
        [Route("api/gps/{serialNumber}/location")]
        public async Task<HttpResponseMessage> GetEquipmentGPSlocation(string serialNumber)
        {
            EquipmentGPSData gpsdata = await GetGPSData(serialNumber);
            if(gpsdata != null)
                return Request.CreateResponse(HttpStatusCode.OK, gpsdata.Location);
            return Request.CreateResponse(HttpStatusCode.NoContent, "Machine location data not found");
        }

        [HttpGet]
        [Route("api/gps/{serialNumber}/equipmentheader")]
        public async Task<HttpResponseMessage> GetEquipmentGPSequipmentheader(string serialNumber)
        {
            EquipmentGPSData gpsdata = await GetGPSData(serialNumber);
            if (gpsdata != null)
                return Request.CreateResponse(HttpStatusCode.OK, gpsdata.EquipmentHeader);
            return Request.CreateResponse(HttpStatusCode.NoContent, "Machine equipmentheader data not found");
        }

        [HttpGet]
        [Route("api/gps/{serialNumber}/cumulativeoperatinghours/")]
        public async Task<HttpResponseMessage> GetEquipmentGPScumulativeoperatinghours(string serialNumber)
        {
            EquipmentGPSData gpsdata = await GetGPSData(serialNumber);
            if (gpsdata != null)
                return Request.CreateResponse(HttpStatusCode.OK, gpsdata.CumulativeOperatingHours);
            return Request.CreateResponse(HttpStatusCode.NoContent, "Machine cumulativeoperatinghours data not found");
        }

        [HttpGet]
        [Route("api/gps/{serialNumber}/fuelusedlast24/")]
        public async Task<HttpResponseMessage> GetEquipmentGPSfuelusedlast24(string serialNumber)
        {
            EquipmentGPSData gpsdata = await GetGPSData(serialNumber);
            if (gpsdata != null)
                return Request.CreateResponse(HttpStatusCode.OK, gpsdata.FuelUsedLast24);
            return Request.CreateResponse(HttpStatusCode.NoContent, "Machine fuelusedlast24 data not found");
        }
 
        [HttpGet]
        [Route("api/gps/{serialNumber}/distance")]
        public async Task<HttpResponseMessage> GetEquipmentGPSdistance(string serialNumber)
        {
            EquipmentGPSData gpsdata = await GetGPSData(serialNumber);
            if (gpsdata != null)
                return Request.CreateResponse(HttpStatusCode.OK, gpsdata.Distance);
            return Request.CreateResponse(HttpStatusCode.NoContent, "Machine distance data not found");
        }

        [HttpGet]
        [Route("api/gps/serialnumbers")]
        public async Task<HttpResponseMessage> GetEquipmentSerialNumbers()
        {
            JObject gpsdata = await GetSerialNumbers();
            if (gpsdata != null)
                return Request.CreateResponse(HttpStatusCode.OK, gpsdata);
            return Request.CreateResponse(HttpStatusCode.NoContent, "Serial Numbers not found");
        }

        [OverrideActionFilters]
        [CustomerAuthorization]
        [HttpGet]
        [Route("api/gps/customer/{serialNumber}")]
        public async Task<HttpResponseMessage> GetEquipmentGPSdataCustomer(string serialNumber)
        {
            EquipmentGPSData gpsdata = await GetGPSData(serialNumber);
            if (gpsdata != null)
                return Request.CreateResponse(HttpStatusCode.OK, gpsdata);
            return Request.CreateResponse(HttpStatusCode.NoContent, "Machine gps data not found");
        }

        private async Task<EquipmentGPSData> GetGPSData(string serialNumber)
        {
            var db = DAL.GetInstance();
            var GPSData = await CallTierraService();
            var machine = GetMachineJObject(serialNumber, GPSData);
            if (machine != null)
            {
                EquipmentGPSData gpsdata = await JTokenToGPSData(machine);
                return gpsdata;
            }
            return null;
        }

        private async Task<JObject> CallTierraService()
        {

            ObjectCache cache = MemoryCache.Default;

            var tierraServiceData = cache.Get("tierraServiceData") as JObject;
            if (tierraServiceData != null)
                return tierraServiceData;

            else
            {
                string _requestUri = "https://tierraservice.com:4500/TierraAEMP/";

                var client = new HttpClient();

                //Basic V29ybGRXaWRlUmVudGFsX0FQSTp3d3JfYXBpXzIwMTI=

                client.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue(
                        "Basic",
                        "V29ybGRXaWRlUmVudGFsX0FQSTp3d3JfYXBpXzIwMTI=");

                ServicePointManager.ServerCertificateValidationCallback = (a, b, c, d) => true;
                HttpResponseMessage response = await client.GetAsync(_requestUri);
                ServicePointManager.ServerCertificateValidationCallback = (a, b, c, d) => false;

                response.EnsureSuccessStatusCode();
                var result = await response.Content.ReadAsStringAsync();

                XmlDocument doc = new XmlDocument();
                doc.LoadXml(result);

                JObject jObj = JObject.FromObject(doc);

                CacheItemPolicy policy = new CacheItemPolicy { AbsoluteExpiration = DateTimeOffset.Now.AddHours(1) };
                cache.Add("tierraServiceData", jObj, policy);

                return jObj;
            }
        }

        private async Task<JObject> GetSerialNumbers()
        {
            var db = DAL.GetInstance();
            var GPSData = await CallTierraService();
            var SerialNumbers = GetGPSEnabledSerialNumbers(GPSData);

            return SerialNumbers;
        }

        private JToken GetMachineJObject(string serialNumber, JObject GPSData)
        {
            foreach (JToken machine in GPSData.SelectTokens("Fleet.Equipment[*]"))
            {
                var SerialNumber = machine.SelectToken("EquipmentHeader.SerialNumber").ToString();
                if (SerialNumber.ToLower() == serialNumber.ToLower())
                {
                    return machine;
                }
            }
            return null;
        }

        private JObject GetGPSEnabledSerialNumbers(JObject GPSData)
        {
            JObject SerialNumbers = new JObject();
            JArray JA = new JArray();
            foreach (JToken machine in GPSData.SelectTokens("Fleet.Equipment[*]"))
            {
                var SerialNumber = machine.SelectToken("EquipmentHeader.SerialNumber");
                if (SerialNumber.Type != JTokenType.Null) JA.Add(SerialNumber);
            }
            JProperty JP = new JProperty("GPSEnabledMachines", JA);
            SerialNumbers.Add(JP);
            return SerialNumbers;
        }

        private async Task<EquipmentGPSData> JTokenToGPSData(JToken machine)
        {
            EquipmentGPSData gpsdata = machine.ToObject<EquipmentGPSData>();

            string exactAddress = await GetLATLONGAddressFromGoogle(gpsdata.Location.Latitude.ToString(), gpsdata.Location.Longitude.ToString());
            var nearbyAddress = await GetNearbyAddressFromGoogle(gpsdata.Location.Latitude.ToString(), gpsdata.Location.Longitude.ToString());

            if (nearbyAddress.Key == null) gpsdata.NearestPlace = "-None-";
            else
               gpsdata.NearestPlace =  nearbyAddress.Key;

           // if (nearbyAddress.Value == null) gpsdata.Address = exactAddress;
           // else
           //     gpsdata.Address = nearbyAddress.Value;
            gpsdata.Address = exactAddress;
            gpsdata.CumulativeOperatingHours.Hour = gpsdata.CumulativeOperatingHours.Hour.Replace("P", "").Replace("DT", "D ").Replace("H", "H ").Replace("M", "M ");

            //Cannot serialize @datetime, grab after serialization
            var datetime = machine.SelectToken("Location.@datetime").ToString();
            gpsdata.Location.datetime = datetime;
            datetime = machine.SelectToken("CumulativeOperatingHours.@datetime").ToString();
            gpsdata.CumulativeOperatingHours.datetime = datetime;
            datetime = machine.SelectToken("FuelUsedLast24.@datetime").ToString();
            gpsdata.FuelUsedLast24.datetime = datetime;
            datetime = machine.SelectToken("Distance.@datetime").ToString();
            gpsdata.Distance.datetime = datetime;

            return gpsdata;
        }

        private async Task<string> GetLATLONGAddressFromGoogle(string latitude, string longitude)
        {
            string _requestUri = "https://maps.googleapis.com/maps/api/geocode/json?latlng="+latitude+","+longitude+"&key=AIzaSyCSqh_rMtl2ln5eLZinKLl4wGsWByyR8pg";
           
            var client = new HttpClient();
            try
            {
                
                ServicePointManager.ServerCertificateValidationCallback = (a, b, c, d) => true;
                HttpResponseMessage response = await client.GetAsync(_requestUri);
                ServicePointManager.ServerCertificateValidationCallback = (a, b, c, d) => false;

                response.EnsureSuccessStatusCode();
                var result = await response.Content.ReadAsStringAsync();

                JObject jObj = JObject.Parse(result);
                if (jObj != null)
                    return jObj.SelectToken("results[0].formatted_address").ToString();
                else return string.Empty;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }

        }

        private async Task<KeyValuePair<string,string>> GetNearbyAddressFromGoogle(string latitude, string longitude)
        {
            string _requestUri = "https://maps.googleapis.com/maps/api/place/nearbysearch/json?location=" + latitude + "," + longitude + "&radius=500&key=AIzaSyCSqh_rMtl2ln5eLZinKLl4wGsWByyR8pg&type=establishment";
            var client = new HttpClient();
            try
            {

                ServicePointManager.ServerCertificateValidationCallback = (a, b, c, d) => true;
                HttpResponseMessage response = await client.GetAsync(_requestUri);
                ServicePointManager.ServerCertificateValidationCallback = (a, b, c, d) => false;

                response.EnsureSuccessStatusCode();
                var result = await response.Content.ReadAsStringAsync();

                JObject jObj = JObject.Parse(result);


                if (jObj != null)
                {
                    string name = jObj.SelectToken("results[0].name").ToString();
                    string address = jObj.SelectToken("results[0].vicinity").ToString();
                    return new KeyValuePair<string, string>(name, address);
                }
                else return new KeyValuePair<string, string>();
            }
            catch (Exception ex)
            {
                return new KeyValuePair<string, string>();
            }

        }

       
    }
}