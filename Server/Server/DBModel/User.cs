using System.Runtime.Serialization;
using MongoDB.Bson.Serialization.Attributes;

namespace Server.DBModel;

[DataContract]
public class User
{
    [BsonId]
    [DataMember]
    public MongoDB.Bson.ObjectId _id { get; set; }
    
    [DataMember]
    public Int32 UID{ get; set; }
    
    [DataMember]
    public string UserName{ get; set; }
    
    [DataMember]
    public string Password{ get; set; }
    
    [DataMember]
    public long CreateTime{ get; set; }
    
    [DataMember]
    public long LastLoginTime{ get; set; }
}