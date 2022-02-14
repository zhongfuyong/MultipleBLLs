using AlliancerLinkToDispenserHostReadConfig;
using Makaretu.Dns;
using System;
using System.Data.SqlClient;
using System.Threading;
using Tmds.MDns;

namespace ConsoleApp1
{
    public class Program
    {
        static void Main(string[] args)
        {
            SqlConnection Con = new SqlConnection("Data Source = 192.168.3.54 ; Initial Catalog = SedoMaster ; User ID = sa ; Password = sed0@dmin");
            ReadDispenserConfig readDispenserConfig = new ReadDispenserConfig(@"C:\Users\Administrator\source\repos\AlliancerLinkToDispenserHost\Source\Config\NecessaryConfig\FieldDate.txt", Con, @"C:\Users\Administrator\Desktop\ERP\EventTriggerData.txt");
            InputOutputSet_Host inputOutputSet_Host = readDispenserConfig.ArrayToEntityClass(@"C:\Users\Administrator\Desktop\ERP\1.bin");

            while (true)
            {
                Console.WriteLine("111111");
                Thread.Sleep(3000);
            }

        }
    }
}
