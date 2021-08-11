// Default URL for triggering event grid function in the local environment.
// http://localhost:7071/runtime/webhooks/EventGrid?functionName={functionname}
using System;
using System.Threading.Tasks;
using System.Net.Http;
using Microsoft.Azure.EventGrid.Models;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.EventGrid;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace updateMaps
{
    public static class ProcessDTUpdatetoMaps
    {
        // Read maps credentials from application settings on function startup
        // Stored as variables in Digitaltwins resource
        private static string statesetID = Environment.GetEnvironmentVariable("statesetID");
        private static string subscriptionKey = Environment.GetEnvironmentVariable("subscription-key");
        private static HttpClient httpClient = new HttpClient();

        [FunctionName("ProcessDTUpdatetoMaps")]
        public static async Task Run([EventGridTrigger] EventGridEvent eventGridEvent, ILogger log)
        {
            // Get messageObject from eventGrid
            JObject message = (JObject)JsonConvert.DeserializeObject(eventGridEvent.Data.ToString());
            log.LogInformation($"Reading event from twinID: {eventGridEvent.Subject}: {eventGridEvent.EventType}: {message["data"]}");

            //Verify that messages contains zone updates
            if (message["data"]["modelId"].ToString() == "dtmi:digtwin:Zone;1")
            {
                // Logic to retrive feature id
                // Connect digital twin "zone" to unit/feature in Azure maps
                // string featureID = "UNIT25";
                string featureID = "";
                if (eventGridEvent.Subject == "zoneB")
                {
                    featureID = "UNIT13";
                }
                else if (eventGridEvent.Subject == "zoneA") {
                    featureID = "UNIT14";
                }

                log.LogInformation($"Update came from: { eventGridEvent.Subject }, and routed to: {featureID} ");
                // Iterate through the properties that have changed
                foreach (var operation in message["data"]["patch"])
                {
                    if (operation["op"].ToString() == "replace" && operation["path"].ToString() == "/Temperature")
                    {
                        // Update the maps feature stateset
                        var postcontent = new JObject(
                            new JProperty(
                                "States",
                                new JArray(
                                    new JObject(
                                        new JProperty("keyName", "Temperature"),
                                        new JProperty("value", operation["value"]),
                                        new JProperty("eventTimestamp", DateTime.UtcNow.ToString("s"))))));

                        var response = await httpClient.PutAsync(
                            $"https://us.atlas.microsoft.com/featurestatesets/{statesetID}/featureStates/{featureID}?api-version=2.0&subscription-key={subscriptionKey}",
                            new StringContent(postcontent.ToString()));

                        log.LogInformation(await response.Content.ReadAsStringAsync());
                    }
                }
            }
        }
    }
}