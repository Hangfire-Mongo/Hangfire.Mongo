﻿using System;
using MongoDB.Bson;
using MongoDB.Bson.IO;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Attributes;

namespace Hangfire.Mongo.Dto
{
#pragma warning disable 1591
    public abstract class ExpiringJobDto : BaseJobDto
    {
        public DateTime? ExpireAt { get; set; }
        protected ExpiringJobDto()
        {

        }

        protected ExpiringJobDto(BsonDocument doc) : base(doc)
        {
            if (doc == null)
            {
                return;
            }
            if (doc.TryGetElement(nameof(ExpireAt), out var expireAt))
            {
                ExpireAt = expireAt.Value.ToNullableUniversalTime();
            }
        }

        protected override void Serialize(BsonDocument document)
        {
            document[nameof(ExpireAt)] = BsonValue.Create(ExpireAt?.ToUniversalTime());
            document["_t"].AsBsonArray.Add(nameof(ExpiringJobDto));
        }
    }

#pragma warning restore 1591
}