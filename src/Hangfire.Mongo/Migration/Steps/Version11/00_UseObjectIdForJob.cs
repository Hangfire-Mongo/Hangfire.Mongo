﻿using System.Linq;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Hangfire.Mongo.Migration.Steps.Version11
{
    /// <summary>
    /// Create signal capped collection
    /// </summary>
    internal class UseObjectIdForJob : IMongoMigrationStep
    {
        public MongoSchema TargetSchema => MongoSchema.Version11;

        public long Sequence => 0;

        public bool Execute(IMongoDatabase database, MongoStorageOptions storageOptions, IMongoMigrationBag migrationBag)
        {
            var jobsCollection = database.GetCollection<BsonDocument>(storageOptions.Prefix + ".job");
            SetFieldAsObjectId(jobsCollection, "_id");
            
            var jobQueueCollection = database.GetCollection<BsonDocument>(storageOptions.Prefix + ".jobQueue");
            SetFieldAsObjectId(jobQueueCollection, "JobId");
            
            return true;
        }

        private static void SetFieldAsObjectId(IMongoCollection<BsonDocument> collection, string fieldName)
        {
            var documents = collection.Find(new BsonDocument()).ToList();

            if(!documents.Any()) return;
            
            foreach (var doc in documents)
            {
                var jobIdString = doc[fieldName].ToString();
                doc[fieldName] = ObjectId.Parse(jobIdString);
            }
            collection.DeleteMany(new BsonDocument());
            collection.InsertMany(documents);
        }
    }
}
