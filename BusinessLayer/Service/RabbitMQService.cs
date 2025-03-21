﻿using Microsoft.AspNetCore.Connections;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RabbitMQ.Client;
using BusinessLayer.Interface;
using RabbitMQ.Client.Events;
using Middleware.SMTP;

namespace BusinessLayer.Service
{
    public class RabbitMQService : IRabbitMQService
    {
        private readonly string _hostname = "localhost";  // Change if needed
        private readonly string _queueName = "GreetingAppQueue"; // Queue name
        private readonly IEmailService _smtp;
        public RabbitMQService(IEmailService _smtp)
        {

            this._smtp = _smtp;
        }

        public void SendMessage(string message)
        {
            var factory = new ConnectionFactory() { HostName = _hostname };

            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare(queue: _queueName, durable: false, exclusive: false, autoDelete: false, arguments: null);

                var body = Encoding.UTF8.GetBytes(message);
                channel.BasicPublish(exchange: "", routingKey: _queueName, basicProperties: null, body: body);

            }
        }

        public void ReceiveMessage()
        {
            var factory = new ConnectionFactory() { HostName = _hostname };
            var connection = factory.CreateConnection();
            var channel = connection.CreateModel();

            channel.QueueDeclare(queue: _queueName, durable: false, exclusive: false, autoDelete: false, arguments: null);

            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += async (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);

                // ✅ Extract email & message
                var parts = message.Split(',', 2); // Splitting at first comma
                if (parts.Length == 2)
                {
                    string email = parts[0].Trim();
                    string emailMessage = parts[1].Trim();


                    // ✅ Send email
                    await _smtp.SendEmailAsync(email, "Welcome to Greeting App", emailMessage);
                }
                else
                {
                    return;
                }
            };

            channel.BasicConsume(queue: _queueName, autoAck: true, consumer: consumer);
        }

    }
}
