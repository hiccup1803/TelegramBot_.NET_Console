using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace ConsoleApp1
{
    internal class BotEngine
    {
        private readonly TelegramBotClient _botClient;
        public BotEngine(TelegramBotClient botClient)
        {
            _botClient = botClient;
        }
        public async Task ListenForMessagesAsync()
        {
            var cts = new CancellationTokenSource();

            var receiverOptions = new ReceiverOptions
            {
                AllowedUpdates = Array.Empty<UpdateType>() // receive all update types
            };
            _botClient.StartReceiving(
                updateHandler: HandleUpdateAsync,
                pollingErrorHandler: HandlePollingErrorAsync,
                receiverOptions: receiverOptions,
                cancellationToken: cts.Token
            );

            var me = await _botClient.GetMeAsync();

            Console.WriteLine($"Start listening for @{me.Username}");
            Console.ReadLine();
        }

        private async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            // Only process Message updates
            if (!(update.Message is var message))
            {
                return;
            }

            // Only process text messages
            if (!(message.Text is var messageText))
            {
                return;
            }

            Console.WriteLine($"Received a '{messageText}' message in chat {message.Chat.Id}.");
            var userId = message.Chat.Id;
            Console.WriteLine($"User ID: {userId}");
            SendMessageAsync(messageText, userId);
        }

        private Task HandlePollingErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
        {
            var ErrorMessage = exception switch
            {
                ApiRequestException apiRequestException
                    => $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
                _ => exception.ToString()
            };

            Console.WriteLine(ErrorMessage);
            return Task.CompletedTask;
        }

        public void SendMessageAsync(string messageText, long userId)
        {
            _botClient.SendTextMessageAsync(userId, messageText);
        }
    }
}
