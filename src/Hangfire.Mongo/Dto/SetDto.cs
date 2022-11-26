﻿using MongoDB.Bson;

namespace Hangfire.Mongo.Dto
{
#pragma warning disable 1591
    public class SetDto : KeyJobDto
    {
        public SetDto()
        {

        }
        public SetDto(BsonDocument doc) : base(doc)
        {
            if(doc == null)
            {
                return;
            }

            Score = doc[nameof(Score)].AsDouble;
            Value = doc[nameof(Value)].StringOrNull();
            SetType = doc[nameof(SetType)].StringOrNull();
        }
        public double Score { get; set; }

        public string Value { get; set; }

        public string SetType { get; set; }

        protected override void Serialize(BsonDocument document)
        {
            base.Serialize(document);
            document[nameof(Score)] = BsonValue.Create(Score);
            document[nameof(Value)] = BsonValue.Create(Value);
            document[nameof(SetType)] = BsonValue.Create(SetType);
            document["_t"].AsBsonArray.Add(nameof(SetDto));
        }
        
    }
#pragma warning restore 1591
}