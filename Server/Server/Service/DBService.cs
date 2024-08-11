using AOT.Framework;
using AOT.Framework.Network;
using Cysharp.Threading.Tasks;

namespace Server.Service;

using MongoDB.Driver;
using System;

public class DBService : SingletonInstance<DBService>, IService
{
    private IMongoDatabase database;
    private MongoClient client;

    // 数据库连接字符串
    private const string ConnectionString = "mongodb://localhost:27017/"; // 根据需要修改
    private const string DatabaseName = "EFDB"; // 根据需要修改

    public void Init()
    {
        // 初始化 MongoDB 客户端
        Console.WriteLine("Database initialized.");
    }

    public void Start()
    {
        // 启动服务，通常在这里可以进行一些启动时的操作
        client = new MongoClient(ConnectionString);
        database = client.GetDatabase(DatabaseName);
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

    /// <summary>
    /// 插入数据
    /// </summary>
    /// <param name="collectionName">集合名称</param>
    /// <param name="data">数据</param>
    /// <typeparam name="T">数据类型</typeparam>
    public void InsertData<T>(string collectionName, T data)
    {
        var collection = database.GetCollection<T>(collectionName);
        collection.InsertOne(data);
    }
    
    /// <summary>
    /// 异步插入数据
    /// </summary>
    /// <param name="collectionName">集合名称</param>
    /// <param name="data">数据</param>
    /// <typeparam name="T">数据类型</typeparam>
    public async UniTask InsertDataAsync<T>(string collectionName, T data)
    {
        var collection = database.GetCollection<T>(collectionName);
        await collection.InsertOneAsync(data);
    }

    /// <summary>
    /// 查询数据
    /// </summary>
    /// <param name="collectionName">集合名称</param>
    /// <param name="filter">过滤条件</param>
    /// <typeparam name="T">数据类型</typeparam>
    /// <returns></returns>
    public T FindData<T>(string collectionName, FilterDefinition<T> filter)
    {
        var collection = database.GetCollection<T>(collectionName);
        return collection.Find(filter).FirstOrDefault();
    }

    /// <summary>
    /// 异步查询数据
    /// </summary>
    /// <param name="collectionName">集合名称</param>
    /// <param name="filter">过滤条件</param>
    /// <param name="cancellationToken">取消令牌</param>
    /// <typeparam name="T">数据类型</typeparam>
    /// <returns></returns>
    public async UniTask<T> FindDataAsync<T>(string collectionName, FilterDefinition<T> filter, CancellationToken cancellationToken = default)
    {
        var collection = database.GetCollection<T>(collectionName);
        T result = await collection.Find(filter).FirstOrDefaultAsync(cancellationToken);
        return result;
    }

    /// <summary>
    /// 更新数据
    /// </summary>
    /// <param name="collectionName">集合名称</param>
    /// <param name="filter">过滤条件</param>
    /// <param name="update">更新定义</param>
    /// <typeparam name="T">数据类型</typeparam>
    public void UpdateData<T>(string collectionName, FilterDefinition<T> filter, UpdateDefinition<T> update)
    {
        var collection = database.GetCollection<T>(collectionName);
        collection.UpdateOne(filter, update);
    }

    /// <summary>
    /// 异步更新数据
    /// </summary>
    /// <param name="collectionName">集合名称</param>
    /// <param name="filter">过滤条件</param>
    /// <param name="update">更新定义</param>
    /// <typeparam name="T">数据类型</typeparam>
    public async UniTask UpdateDataAsync<T>(string collectionName, FilterDefinition<T> filter,
        UpdateDefinition<T> update)
    {
        var collection = database.GetCollection<T>(collectionName);
        await collection.UpdateOneAsync(filter, update);
    }
}
