using System;
using System.Collections.Generic;
using System.Text;
using Confluent.Kafka;
using Confluent.Kafka.Serialization;
using Newtonsoft.Json;

namespace ProducerTest {
    internal class Program {
        private static readonly Dictionary<string, object> Config = new Dictionary<string, object> {
            { "bootstrap.servers", "127.0.0.1:9092" },
            { "acks", "all" },
            { "batch.num.messages", 1 },
            { "linger.ms", 0 },
            { "compression.codec", "gzip" }
        };

        private static void Main(string[] args) {
            try {
                Console.Title = "Producer client.";
                // banner
                Console.WriteLine("Kafka Producer Sample.");
                Console.WriteLine("Press CTRL+C to exit");

                bool cancelled = false;
                Console.CancelKeyPress += (_, e) => {
                    e.Cancel = true; // prevent the process from terminating.
                    cancelled = true;
                };

                while (!cancelled) {
                    // get name
                    Console.WriteLine("\nWhat is your name?");
                    string name = Console.ReadLine();

                    // get message
                    Console.WriteLine("\nWhat is your message?");
                    string message = Console.ReadLine();

                    // set time stamp
                    DateTime msgTimeStamp = DateTime.Now;

                    Msg msg = new Msg {
                        User = name,
                        Message = message,
                        TimeStamp = msgTimeStamp
                    };

                    // send to Kafka
                    using (Producer<Null, string> producer = new Producer<Null, string>(Config, null, new StringSerializer(Encoding.UTF8))) {
                        // convert the Msg model to json
                        string jsonMsg = JsonConvert.SerializeObject(msg);

                        // push to Kafka
                        Message<Null, string> dr = producer.ProduceAsync("topic_messages", null, jsonMsg).Result;
                        producer.Flush(1);
                    }

                    // produce message to Kafka
                    Console.WriteLine("Message published to Kafka.");
                    Console.WriteLine("\n");
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
