using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Jaimes.Processor
{
    class Program
    {
        private static readonly IModel _channel;

        static Program()
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };

            using (var connection = factory.CreateConnection())
            {
                _channel = connection.CreateModel();
            }
        }

        static void Main(string[] args)
        {
            GenericHandler handler1 = new GenericHandler(".\\fila_origem", "exchange_rabbit", "");

            for (; ; )
            {
                Console.WriteLine("Lendo as mensagens");

                handler1.ReadQueue(_channel);

                Thread.Sleep(500);
            }
        }
    }
}
