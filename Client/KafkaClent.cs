using System;
using KafkaNet;
using KafkaNet.Model;
using KafkaNet.Protocol;

namespace Client {
    public class KafkaClent {
        private readonly KafkaOptions _options;
        private readonly BrokerRouter _router;
        private const string CsTopic = "testtopic";

        public KafkaClent() {
            _options = new KafkaOptions(new Uri("http://127.0.0.1:9092"));
            _router = new BrokerRouter(_options);
        }

        public void Producer(string msg) {
            using (Producer client = new Producer(_router)) {
                client.SendMessageAsync(CsTopic, new[] { new Message(msg) }).Wait();
            }
        }

        public void Consumer() {
            using (var consumer = new Consumer(new ConsumerOptions(CsTopic, _router))) {
                // Consume returns a blocking IEnumerable (ie: never ending stream)
                foreach (var message in consumer.Consume()) {
                    Console.WriteLine(" Response: P{0},O{1} : {2}",
                        message.Meta.PartitionId, message.Meta.Offset, message.Value);
                }
            }
        }
    }
}
