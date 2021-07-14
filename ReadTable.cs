using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Microsoft.Azure.Cosmos.Table;

namespace Jongmin.Function
{
    public static class ReadTable
    {
        [FunctionName("ReadTable")]
        public static Task<string> Run([HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)]
        HttpRequest req, ILogger log, ExecutionContext context)
        {
            string connStrA = Environment.GetEnvironmentVariable("AzureWebJobsStorage");
            string requestBody = new StreamReader(req.Body).ReadToEnd();
            dynamic data = JsonConvert.DeserializeObject(requestBody);
            string PartitionKeyA = data.PartitionKey;
            string RowKeyA = data.RowKey;

            CloudStorageAccount stoA = CloudStorageAccount.Parse(connStrA);
            CloudTableClient tbC = stoA.CreateCloudTableClient();
            CloudTable TableA = tbC.GetTableReference("tableA");

            string filterA = TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.GreaterThanOrEqual, PartitionKeyA);
            string filterB = TableQuery.GenerateFilterCondition("Rowkey", QueryComparisons.GreaterThanOrEqual, RowKeyA);

            Task<string> response = ReadToTable(TableA, filterA, filterB);
            return response;
        }

        static Task<string> ReadToTable(CloudTable tableA, string filterA, string filterB)
        {
            TableQuery<MemoData> rangeQ = new TableQuery<MemoData>().Where(
                TableQuery.CombineFilters(filterA, TableOperators.And, filterB)
            );
            TableContinuationToken tokenA = null;
            rangeQ.TakeCount = 10000;
            try
            {
                do
                {
                    
                    
                } while (tokenA != null);
            }
            catch (StorageException e)
            {
                Console.WriteLine(e.Message);
                throw;
            }

            return "";
        }

        private class MemoData : TableEntity
        {
            public string content {get; set; }
        }
    }
}