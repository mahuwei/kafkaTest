using System;
using System.Text;

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
    }
}
