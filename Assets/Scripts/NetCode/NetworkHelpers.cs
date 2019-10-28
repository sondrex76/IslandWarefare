using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;
using System;

public class NetworkHelpers : MonoBehaviour
{

    private string urlGetPlayersInSegment = "https://F5079.playfabapi.com/Server/GetPlayersInSegment";



    //Send with a function that takes an int to get a callback when it's done getting numbers of players from server
    public async Task GetAllPlayerSegments(Action<int> onComplete)
    {
        var httpClient = new HttpClient();
        httpClient.BaseAddress = new System.Uri(urlGetPlayersInSegment);

        //Setting the headers
        httpClient.DefaultRequestHeaders.Accept.Add(
            new MediaTypeWithQualityHeaderValue("application/json"));

        httpClient.DefaultRequestHeaders.Add(
            "X-SecretKey", "BFAUNKU49AITMY3KQCNNZ7YONC7BCS87NFOPP6Q9CQEYWCTGUX");

        //Setting content to be posted
        var values = new Dictionary<string, string>
        {
        {"SegmentId", "2C3F0B4487C23F69"},
        {"SecondsToLive", "5"},
        {"MaxBatchSize", "500"}
        };

        var json = JsonConvert.SerializeObject(values);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        //Sends POST request to endpoint
        HttpResponseMessage httpResponse = await httpClient.PostAsync(urlGetPlayersInSegment, content);
        //Get the response as a string
        var responseString = await httpResponse.Content.ReadAsStringAsync();
        Debug.Log(responseString);

        //Parse the string into Objects
        dynamic response = JsonConvert.DeserializeObject<RootObject>(responseString);

        onComplete(response.data.ProfilesInSegment);
    }





    //Response structure for playersinsegment
    public class Data
    {
        public int ProfilesInSegment { get; set; }
        public List<object> PlayerProfiles { get; set; }
    }

    public class RootObject
    {
        public int code { get; set; }
        public string status { get; set; }
        public Data data { get; set; }
    }




}
