using System;
using System.Collections.Generic;
using System.Text;
using Confluent.Kafka;
using Confluent.Kafka.Serialization;

namespace Client {
    internal class Program {
        private const string CsHelp = "help";
        private const string CsQuit = "quit";
        private const string CsSend = "send";
        private const string CsReceive = "receive";
        private static string _helpInfo;
        private static void Main(string[] args) {
            StringBuilder sb = new StringBuilder();
            sb.Append("kafka客户端测试：\n");
            sb.Append($" {CsHelp}:显示帮助信息\n");
            sb.Append(" quit:退出\n");
            sb.Append($" {CsSend} string:发送消息\n");
            sb.Append($" {CsReceive}:接收消息消息\n");
            sb.Append("---------结束------\n");
            _helpInfo = sb.ToString();

            //Test();

            KafkaClent kc = new KafkaClent();
            Console.WriteLine(_helpInfo);
            bool isContinue = true;
            do {
                string input = Console.ReadLine();
                if (string.IsNullOrEmpty(input)) {
                    continue;
                }
                input = input.Replace("  ", " ");
                string[] datas = input.Split(' ');
                string cmd = datas[0].ToLower();
                switch (cmd) {
                    case CsHelp:
                        Console.WriteLine(_helpInfo);
                        break;
                    case CsQuit:
                        isContinue = false;
                        break;
                    case CsSend:
                        if (datas.Length == 1) {
                            Console.WriteLine("没有指定要发送的数据.");
                            continue;
                        }
                        try {
                            kc.Producer(input.Substring(CsQuit.Length + 1));
                            Console.WriteLine("消息发送成功。");
                        }
                        catch (Exception ex) {
                            Console.WriteLine("发送消息发生错误：{0}", ex.Message);
                        }
                        break;
                    case CsReceive:
                        try {
                            Console.WriteLine("接收消息------开始-----");
                            kc.Consumer();
                            Console.WriteLine("接收消息------结束-----");
                        }
                        catch (Exception ex) {
                            Console.WriteLine("接收消息发生错误：{0}", ex.Message);
                        }
                        break;
                    default:
                        continue;
                }

            } while (isContinue);

        }

        private static void Test() {
            // The Kafka endpoint address
            string kafkaEndpoint = "127.0.0.1:9092";

            // The Kafka topic we'll be using
            string kafkaTopic = "testtopic";

            // Create the producer configuration
            Dictionary<string, object> producerConfig = new Dictionary<string, object> { { "bootstrap.servers", kafkaEndpoint } };

            // Create the producer
            using (Producer<Null, string> producer = new Producer<Null, string>(producerConfig, null, new StringSerializer(Encoding.UTF8))) {
                // Send 10 messages to the topic
                for (int i = 0; i < 10; i++) {
                    string message = $"Event {i}";
                    Message<Null, string> result = producer.ProduceAsync(kafkaTopic, null, message).GetAwaiter().GetResult();
                    Console.WriteLine($"Event {i} sent on Partition: {result.Partition} with Offset: {result.Offset}");
                }
            }

            // Create the consumer configuration
            Dictionary<string, object> consumerConfig = new Dictionary<string, object>
            {
                { "group.id", "myconsumer" },
                { "bootstrap.servers", kafkaEndpoint },
                { "auto.offset.reset", "earliest" }
            };

            // Create the consumer
            using (Consumer<Null, string> consumer = new Consumer<Null, string>(consumerConfig, null, new StringDeserializer(Encoding.UTF8))) {
                // Subscribe to the OnMessage event
                consumer.OnMessage += (obj, msg) => {
                    Console.WriteLine($"Received: {msg.Value}");
                };

                // Subscribe to the Kafka topic
                consumer.Subscribe(new List<string>() { kafkaTopic });

                // Handle Cancel Keypress 
                bool cancelled = false;
                Console.CancelKeyPress += (_, e) => {
                    e.Cancel = true; // prevent the process from terminating.
                    cancelled = true;
                };

                Console.WriteLine("Ctrl-C to exit.");

                // Poll for messages
                while (!cancelled) {
                    consumer.Poll();
                }
            }
        }
    }
}
