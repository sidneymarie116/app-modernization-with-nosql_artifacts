using Contoso.Apps.Movies.Data.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;
using DataGenerator.Common;

namespace DataGenerator
{
    public class EruptionGenerator
    {
        public readonly List<EventHubElement> EventHubs;
        private readonly Random r;

        public EruptionGenerator()
        {
            this.EventHubs = GetEventHubsConfiguration();
            this.r = new Random();
        }

        public async void DoWork()
        {
            try
            {
                int loop = 1;
                var eventHubs = EventHubs;
                EventHubElement eventHub = eventHubs.First();
                //loop...
                while (true)
                {
                    //Thread.Sleep(50);
                    if (eventHubs.Count > 1)
                    {
                        if ((loop % 2 == 0))
                        {
                            eventHub = eventHubs[1];
                        }
                        else
                        {
                            eventHub = eventHubs.First();
                        }
                    }

                    GenerateAction(eventHub);

                    loop++;
                }
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(ex.Message);
                Console.ResetColor();
                Console.ReadLine();
            }
        }

        private List<EventHubElement> GetEventHubsConfiguration()
        {
            var eventHubs = new List<EventHubElement>();
            if (!string.IsNullOrEmpty(AppSettings.Configuration["EVENT_HUB_1_CONNECTION_STRING"]))
            {
                eventHubs.Add(new EventHubElement { ConnectionString = AppSettings.Configuration["EVENT_HUB_1_CONNECTION_STRING"], Region = "Region1" });
            }

            if (!string.IsNullOrEmpty(AppSettings.Configuration["EVENT_HUB_2_CONNECTION_STRING"]))
            {
                eventHubs.Add(new EventHubElement { ConnectionString = AppSettings.Configuration["EVENT_HUB_2_CONNECTION_STRING"], Region = "Region2" });
            }

            return eventHubs;
        }

        private void GenerateAction(EventHubElement eventHub)
        {
            // Event hub client
            var eventHubHelper = new EventHubHelper(eventHub.EventHubName, eventHub.ConnectionString);

            // Send order to event hub
            var region = GenerateEruption(eventHub.Region);
            Console.WriteLine($"Sending eruption event to {eventHub.Region}");
            eventHubHelper.SendMessageToEventHub(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(region))).GetAwaiter().GetResult();

            // Close connection
            eventHubHelper.CloseEventHub().GetAwaiter().GetResult();
        }

        private Eruption GenerateEruption(string region)
        {
            var volcano = GetRandomVolcano();
            // Get Random User
            Console.WriteLine($"{volcano.VolcanoName} volcano erupted");

            var eruption = new Eruption
            {
                Region = region,
                OrderDate = DateTime.UtcNow,
                VolcanoName = volcano.VolcanoName
            };

            return eruption;
        }

        private Volcano GetRandomVolcano()
        {
            var volcanoes = Volcano.GetVolcanoesFromJson();
            return volcanoes.Skip(r.Next(volcanoes.Count)).Take(1).FirstOrDefault();
        }
    }
}
