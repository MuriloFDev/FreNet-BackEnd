using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using App.Controllers;
using App.Model;
using App.Models;

namespace App.Auxiliary
{

    public class ReturnCarrierAvaliableFrenet
    {
        public List<CarrierAvaliable> ShippingSeviceAvailableArray { get; set; }
    }

    public class ReturnQuoteFrenet
    {
        public List<ReturnQuote> ShippingSevicesArray { get; set; }
    }
    
    public class Frenet
    {
        public static async Task<List<CarrierAvaliable>> ListCarrierAvaliable()
        {
            using (HttpClient httpClient = new HttpClient())
            {
                string apiUrl = "https://private-anon-7538bfa64b-frenetapi.apiary-mock.com/shipping/info";
                string apiToken = "C278F315RB676R4F82RAFA6RB1FE5E42D45A";

                httpClient.DefaultRequestHeaders.Add("Authorization", $"{apiToken}");

                HttpResponseMessage response = await httpClient.GetAsync(apiUrl);
                if (response.IsSuccessStatusCode)
                {
                    string jsonResult = await response.Content.ReadAsStringAsync();
                    var result = JsonConvert.DeserializeObject<ReturnCarrierAvaliableFrenet>(jsonResult);

                    return result.ShippingSeviceAvailableArray;
                }
                else
                {
                    // Trate o caso em que a chamada à API não foi bem-sucedida
                    return new List<CarrierAvaliable>();
                }
            }
        }

        public static async Task<List<ReturnQuote>> GetShippingQuotes(ShippingRequest shippingRequest)
        {
            using (HttpClient httpClient = new HttpClient())
            {
                string apiUrl = "https://private-anon-7538bfa64b-frenetapi.apiary-mock.com/shipping/quote";
                string apiToken = "C278F315RB676R4F82RAFA6RB1FE5E42D45A";

                httpClient.DefaultRequestHeaders.Add("Authorization", $"{apiToken}");

                var requestContent = new StringContent(JsonConvert.SerializeObject(shippingRequest), Encoding.UTF8, "application/json");

                HttpResponseMessage response = await httpClient.PostAsync(apiUrl, requestContent);
                
                try
                {
                    if (response.IsSuccessStatusCode)
                    {
                        string jsonResult = await response.Content.ReadAsStringAsync();
                        var result = JsonConvert.DeserializeObject<ReturnQuoteFrenet>(jsonResult);

                        // Return result
                        return result.ShippingSevicesArray;
                    }
                    else
                    {
                        // Return error
                        return new List<ReturnQuote>();
                    }
                }
                catch (Exception error)
                {
                    throw new Exception(error.Message);
                }
                
            }
        }

    }
}
