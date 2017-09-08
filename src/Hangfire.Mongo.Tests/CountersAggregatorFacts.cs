﻿using System;
using System.Threading;
using Hangfire.Mongo.Dto;
using Hangfire.Mongo.Tests.Utils;
using MongoDB.Bson;
using Xunit;

namespace Hangfire.Mongo.Tests
{
    [Collection("Database")]
    public class CountersAggregatorFacts
    {
        [Fact, CleanDatabase]
        public void CountersAggregatorExecutesProperly()
        {
            var storage = ConnectionUtils.CreateStorage();
            using (var connection = (MongoConnection)storage.GetConnection())
            {
                // Arrange
                connection.Database.StateData.InsertOne(new CounterDto
                {
                    Key = "key",
                    Value = 1L,
                    ExpireAt = DateTime.UtcNow.AddHours(1)
                });

                var aggregator = new CountersAggregator(storage, TimeSpan.Zero);

                // Act
                using (var cts = new CancellationTokenSource())
                {
                    cts.Cancel();
                    aggregator.Execute(cts.Token);
                }

                // Assert
                Assert.Equal(1, connection.Database.StateData.OfType<AggregatedCounterDto>().Count(new BsonDocument()));
            }
        }
    }
}