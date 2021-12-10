using Contoso.Apps.Movies.Data.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;

namespace DataGenerator
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                var eruptionGenerator = new EruptionGenerator();
                var eventHubs = eruptionGenerator.EventHubs;
                if (eventHubs.Count == 0)
                {
                    throw new Exception("At least one eventhub connection string needs to be added. Please add and try again.");
                }

                DoWork(eruptionGenerator);
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(ex.Message);
                Console.ResetColor();
                Console.ReadLine();
            }
            
        }

        static void DoWork(EruptionGenerator eruptionGenerator)
        {
            var t = new Thread(eruptionGenerator.DoWork);
            t.Start();
        }
    }
}
