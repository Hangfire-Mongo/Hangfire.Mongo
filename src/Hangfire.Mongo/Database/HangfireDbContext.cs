﻿using System;
using Hangfire.Mongo.Dto;
using MongoDB.Driver;

namespace Hangfire.Mongo.Database
{
    /// <summary>
    /// Represents Mongo database context for Hangfire
    /// </summary>
    public sealed class HangfireDbContext
    {
        private readonly string _prefix;

        /// <summary>
        /// MongoClient used for this db context instance
        /// </summary>
        public MongoClient Client { get; }

        /// <summary>
        /// Database instance used for this db context instance
        /// </summary>
        public IMongoDatabase Database { get; }

        internal HangfireDbContext(string connectionString, string databaseName, string prefix = "hangfire")
            :this(new MongoClient(connectionString), databaseName, prefix)
        {
            
        }
        /// <summary>
        /// Constructs context with Mongo client and database name
        /// </summary>
        /// <param name="mongoClient"></param>
        /// <param name="databaseName"></param>
        /// <param name="prefix"></param>
        public HangfireDbContext(MongoClient mongoClient, string databaseName, string prefix = "hangfire")
        {
            _prefix = prefix;
            Client = mongoClient;
            Database = mongoClient.GetDatabase(databaseName);
            ConnectionId = Guid.NewGuid().ToString();
        }


        /// <summary>
        /// Mongo database connection identifier
        /// </summary>
        public string ConnectionId { get; private set; }

        /// <summary>
        /// Reference to tailable collection which contains signal dtos for enqueued job items
        /// </summary>
        public IMongoCollection<NotificationDto> Notifications =>
            Database.GetCollection<NotificationDto>(_prefix + ".notifications");
        
        /// <summary>
        /// Reference to job graph collection
        /// </summary>
        public IMongoCollection<BaseJobDto> JobGraph => Database.GetCollection<BaseJobDto>(_prefix + ".jobGraph");

        /// <summary>
        /// Reference to collection which contains distributed locks
        /// </summary>
        public IMongoCollection<DistributedLockDto> DistributedLock => Database
            .GetCollection<DistributedLockDto>(_prefix + ".locks");

        /// <summary>
        /// Reference to collection which contains schemas
        /// </summary>
        public IMongoCollection<SchemaDto> Schema => Database.GetCollection<SchemaDto>(_prefix + ".schema");

        /// <summary>
        /// Reference to collection which contains servers information
        /// </summary>
        public IMongoCollection<ServerDto> Server => Database.GetCollection<ServerDto>(_prefix + ".server");
    }
}