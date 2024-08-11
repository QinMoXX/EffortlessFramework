using AOT.Framework.Network;

namespace Server.Service;

using MongoDB.Driver;
using System;

public class DBService : IService
{
    private IMongoDatabase database;
    private MongoClient client;

    // 数据库连接字符串
    private string connectionString = "mongodb://localhost:27017"; // 根据需要修改
    private string databaseName = "EFDB"; // 根据需要修改

    public void Init()
    {
        // 初始化 MongoDB 客户端
        Console.WriteLine("Database initialized.");
    }

    public void Start()
    {
        // 启动服务，通常在这里可以进行一些启动时的操作
        client = new MongoClient(connectionString);
        database = client.GetDatabase(databaseName);
        Console.WriteLine("DBService started.");
    }

    public void Update()
    {
        // 更新逻辑，例如定期检查数据库状态或执行某些操作
    }

    public void Destroy()
    {
        // 清理资源
        client = null;
        database = null;
        Console.WriteLine("DBService destroyed.");
    }

    // 示例方法：插入数据
    public void InsertData<T>(string collectionName, T data)
    {
        var collection = database.GetCollection<T>(collectionName);
        collection.InsertOne(data);
        Console.WriteLine("Data inserted into collection: " + collectionName);
    }

    // 示例方法：查询数据
    public T FindData<T>(string collectionName, FilterDefinition<T> filter)
    {
        var collection = database.GetCollection<T>(collectionName);
        return collection.Find(filter).FirstOrDefault();
    }
}
