// using System;
// using System.Collections.Generic;
// using System.IO;
// using System.Linq;
// using System.Net;
// using System.Threading.Tasks;
// using Microsoft.Azure.Cosmos;
// using Newtonsoft.Json;

// namespace Volcanoes.Apps.Common
// {
//     class Program
//     {
//         // The Azure Cosmos DB endpoint for running this sample.
//         private static readonly string EndpointUri = "https://volcanoescs.documents.azure.com";
//         // The primary key for the Azure Cosmos account.
//         private static readonly string PrimaryKey = "Bpy7dobk8W7uiTYxc7Z9RnVndJmi1BTHCESs6Bbcb30ROE17e6PlomOpB3b6OWJB89650scKOcnjkikon6MFug==";

//         // The Cosmos client instance
//         private CosmosClient cosmosClient;

//         // The database we will create
//         private Database database;

//         // The container we will create.
//         private Container container;

//         // The name of the database and container we will create
//         private string databaseId = "volcanoes";
//         private string containerId = "volcanoes";

//         public static async Task Main(string[] args)
//         {
//             try
//             {
//                 Console.WriteLine("Beginning operations..");
//                 Program p = new Program();

//                 await p.GetStartedDemoAsync();
//                 await p.CreateContainerAsync();
//                 await p.AddItemsToContainerAsync();
//             }
//             catch (CosmosException de)
//             {
//                 Exception baseException = de.GetBaseException();
//                 Console.WriteLine($"{de.StatusCode} error occurred: {de}");
//             }
//             catch (Exception e)
//             {
//                 Console.WriteLine("Error: {0}", e);
//             }
//             finally
//             {
//                 Console.WriteLine("End of demo, press any key to exit.");
//                 Console.ReadKey();
//             }
//         }

//         public async Task GetStartedDemoAsync()
//         {
//             // Create a new instance of the Cosmos Client
//             this.cosmosClient = new CosmosClient(EndpointUri, PrimaryKey, new CosmosClientOptions()
//             {
//                 ConnectionMode = ConnectionMode.Gateway
//             });
//             await this.CreateDatabaseAsync();
//         }

//         /// <summary>
//         /// Create the database if it does not exist
//         /// </summary>
//         private async Task CreateDatabaseAsync()
//         {
//             // Create a new database
//             var result = await this.cosmosClient.CreateDatabaseIfNotExistsAsync(databaseId);
//             this.database = result.Database;
//             var responseString = result.StatusCode == HttpStatusCode.Created ? $"Created Database: {this.database.Id}" : $"Database {this.database.Id} already exists";
//             Console.WriteLine(responseString);
//         }

//         /// <summary>
//         /// Create the container if it does not exist. 
//         /// Specifiy "/LastName" as the partition key since we're storing family information, to ensure good distribution of requests and storage.
//         /// </summary>
//         /// <returns></returns>
//         private async Task CreateContainerAsync()
//         {
//             // Create a new container
//             var result = await this.database.CreateContainerIfNotExistsAsync(containerId, "/Country");
//             this.container = result.Container;
//             var responseString = result.StatusCode == System.Net.HttpStatusCode.Created ? $"Created Container: {this.container.Id}" : $"Container {this.container.Id} already exists";
//         }

//         /// <summary>
//         /// Add Family items to the container
//         /// </summary>
//         private async Task AddItemsToContainerAsync()
//         {
//             try
//             {
//                 var volcanoes = GetVolcanoesFromJson();

//                 foreach (var volcano in volcanoes)
//                 {
//                     try
//                     {
//                         // Read the item to see if it exists.  
//                         var volcanoResponse = await this.container.ReadItemAsync<Volcano>(volcano.id, new PartitionKey(volcano.Country));
//                         Console.WriteLine($"Item in database with id: {volcanoResponse.Resource.id} already exists");
//                     }
//                     catch (CosmosException ex) when (ex.StatusCode == HttpStatusCode.NotFound)
//                     {
//                         // Create an item in the container representing the Andersen family. Note we provide the value of the partition key for this item, which is "Andersen"
//                         var volcanoResponse = await this.container.CreateItemAsync<Volcano>(volcano, new PartitionKey(volcano.Country));

//                         // Note that after creating the item, we can access the body of the item with the Resource property off the ItemResponse. We can also access the RequestCharge property to see the amount of RUs consumed on this request.
//                         Console.WriteLine($"Created item in database with id: {volcanoResponse.Resource.id} Operation consumed {volcanoResponse.RequestCharge} RUs.");
//                     }
//                 }
//             }
//             catch (Exception e)
//             {
//             }
//         }

//         private List<Volcano> GetVolcanoesFromJson()
//         {
//             try
//             {
//                 using (var reader = new StreamReader("VolcanoData.json"))
//                 {
//                     var json = reader.ReadToEnd();
//                     var result = JsonConvert.DeserializeObject<IEnumerable<Volcano>>(json);
//                     return result?.ToList();
//                 }
//             }
//             catch (Exception ex)
//             {
//                 return null;
//             }
//         }
//     }
// }
