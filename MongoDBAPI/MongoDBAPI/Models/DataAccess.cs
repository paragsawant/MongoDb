using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MongoDBAPI.Models
{
    public class DataAccess
    {
        MongoClient _client;
        MongoServer _server;
        MongoDatabase _db;

        public DataAccess()
        {
            _client = new MongoClient("mongodb://readuser:readuserpassword@cluster0-shard-00-00-mp5dc.mongodb.net:27017,cluster0-shard-00-01-mp5dc.mongodb.net:27017,cluster0-shard-00-02-mp5dc.mongodb.net:27017/test?ssl=true&replicaSet=Cluster0-shard-0&authSource=admin");
            _server = _client.GetServer();
            _db = _server.GetDatabase("testdb");
        }

        public IEnumerable<Product> GetProducts()
        {
            return _db.GetCollection<Product>("Products").FindAll();
        }


        public Product GetProduct(ObjectId id)
        {
            var res = Query<Product>.EQ(p => p.Id, id);
            var result = _db.GetCollection<Product>("Products");

            return _db.GetCollection<Product>("Products").FindOne(res);
        }

        public Product Create(Product p)
        {
            _db.GetCollection<Product>("Products").Save(p);
            return p;
        }

        public void Update(ObjectId id, Product p)
        {
            p.Id = id;
            var res = Query<Product>.EQ(pd => pd.Id, id);
            var operation = Update<Product>.Replace(p);
            _db.GetCollection<Product>("Products").Update(res, operation);
        }

        public void Remove(ObjectId id)
        {
            var res = Query<Product>.EQ(e => e.Id, id);
            var operation = _db.GetCollection<Product>("Products").Remove(res);
        }
    }
}
