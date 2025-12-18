//using System;
//using System.Threading;
//using Random = System.Random;

//namespace Assets._Project.Scripts.UtilScripts
//{
//    [Serializable]
//    public struct RandomId
//    {
//        [UnityEngine.SerializeField]
//        private long _id;

//        private RandomId(long id)
//        {
//            _id = id;
//        }

//        //thread safe
//        private static readonly ThreadLocal<Random> threadRandom = new(() =>
//        {
//            return new Random(Guid.NewGuid().GetHashCode());
//        });

//        public static RandomId Get()
//        {
//            var random = threadRandom.Value;

//            long id = GenerateDigits(random);

//            return new RandomId(id);
//        }

//        private static long GenerateDigits(Random random)
//        {
//            // exlude 0 to be a valid number
//            long result = random.Next(1, 10);

//            for (int i = 0; i < 17; i++)
//            {
//                result = result * 10 + random.Next(10);
//            }

//            return result;
//        }



//        public readonly bool IsDefault => this == Default;
//        public static RandomId Default => new RandomId(0);


//        public override readonly bool Equals(object obj)
//        {
//            return obj is RandomId other && _id == other._id;
//        }

//        public override readonly int GetHashCode()
//        {
//            return _id.GetHashCode();
//        }

//        public override readonly string ToString()
//        {
//            return _id.ToString();
//        }

//        public static bool operator ==(RandomId left, RandomId right)
//        {
//            return left._id == right._id;
//        }

//        public static bool operator !=(RandomId left, RandomId right)
//        {
//            return !(left == right);
//        }





//        public class RandomIdJsonConverter : Newtonsoft.Json.JsonConverter<RandomId>
//        {
//            public override void WriteJson(Newtonsoft.Json.JsonWriter writer, RandomId value, Newtonsoft.Json.JsonSerializer serializer)
//            {
//                if(value.IsDefault)
//                {
//                    writer.WriteNull();
//                    return;
//                }
//                writer.WriteValue(value._id);
//            }

//            public override RandomId ReadJson(Newtonsoft.Json.JsonReader reader, Type objectType, RandomId existingValue, bool hasExistingValue, Newtonsoft.Json.JsonSerializer serializer)
//            {
//                if (reader.TokenType == Newtonsoft.Json.JsonToken.Null)
//                {
//                    return RandomId.Default;
//                }
                
//                if (long.TryParse(reader.Value.ToString(), out long id))
//                {
//                    return new RandomId(id);
//                }
//                throw new Newtonsoft.Json.JsonSerializationException($"Cannot convert {reader.Value} to RandomId");
//            }
//        }
//    }
//}
