using Newtonsoft.Json;
using RabbitMQ.Client;
using System;
using System.Messaging;
using System.Text;

namespace Jaimes.Processor
{
    public class GenericHandler
    {
        private readonly String _destinyExchange;
        private readonly String _routingKey;
        private readonly MessageQueue _queue;

        public GenericHandler(String originQueueName, String exchange, String routingKey)
        {
            _destinyExchange = exchange;
            _routingKey = routingKey;

            if (!MessageQueue.Exists(originQueueName))
                throw new InvalidOperationException($"Fila '{originQueueName}' não existe");

            _queue = new MessageQueue(originQueueName);
        }

        public void ReadQueue(IModel channel)
        {
            Message myMessage = _queue.Receive();

            Console.WriteLine($"Mensagem {myMessage.Id} recebida.");

            var stringContent = JsonConvert.SerializeObject(myMessage.Body);

            var body = Encoding.UTF8.GetBytes(stringContent);

            channel.BasicPublish(exchange: _destinyExchange,
                                 routingKey: _routingKey,
                                 basicProperties: null,
                                 body: body);

            Console.WriteLine($"Mensagem {myMessage.Id} enviada para {_destinyExchange}.");
        }
    }
}