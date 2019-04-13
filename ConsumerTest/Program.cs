using System;
using System.Collections.Generic;
using System.Text;
using Confluent.Kafka;
using Confluent.Kafka.Serialization;

namespace ConsumerTest {
    internal class Program {
        private static readonly Dictionary<string, object> Config = new Dictionary<string, object>
        {
            { "group.id", "msg-id" },
            { "bootstrap.servers", "127.0.0.1:9092" },
            { "auto.offset.reset", "earliest" }
        };

        private static void Main(string[] args) {
            Console.Title = "Consumer client.";

            try {
                using (Consumer<string, string> consumer = new Consumer<string, string>(Config, new StringDeserializer(Encoding.UTF8), new StringDeserializer(Encoding.UTF8))) {
                    consumer.OnMessage += (key, msg) => {
                        Console.WriteLine($"Read '{msg.Value}' from: {msg.TopicPartitionOffset}");
                    };

                    consumer.OnError += (_, error)
                        => Console.WriteLine($"Error: {error}");

                    consumer.OnConsumeError += (key, msg)
                        => Console.WriteLine($"Consume error ({msg.TopicPartitionOffset}): {msg.Error}");

                    consumer.Subscribe("topic_messages");

                    while (true) {
                        consumer.Poll(TimeSpan.FromMilliseconds(100));
                    }
                }
            }
            catch (System.Exception ex) {
                Console.WriteLine(ex.ToString());
            }
        }
    }

    public class Msg {
        public string User { get; set; }

        public string Message { get; set; }

        public DateTime? TimeStamp { get; set; }
    }
}
