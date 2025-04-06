using Serilog.Events;
using Microsoft.Azure.Cosmos.Table;
using Serilog.Core;

namespace WhatsAppBridge.Utilities;

public class SimpleAzureTableSink : ILogEventSink {

    private readonly CloudTable _table;

    public SimpleAzureTableSink(string connectionString, string tableName) {
        var account = CloudStorageAccount.Parse(connectionString);
        var client = account.CreateCloudTableClient(new TableClientConfiguration());
        _table = client.GetTableReference(tableName);
        _table.CreateIfNotExists();
    }

    public void Emit(LogEvent logEvent) {
        var entity = new DynamicTableEntity {
            PartitionKey = DateTime.UtcNow.ToString("yyyyMMdd"),
            RowKey = Guid.NewGuid().ToString(),
            Properties =
            {
                { "Level", new EntityProperty(logEvent.Level.ToString()) },
                { "Message", new EntityProperty(logEvent.RenderMessage()) },
                { "Time", new EntityProperty(logEvent.Timestamp.UtcDateTime) }
            }
        };
        _table.Execute(TableOperation.Insert(entity));
    }
}




