using ConsoleApp1;
using Telegram.Bot;
var botClient = new TelegramBotClient("6461579255:AAFq_NIMcyQ2d5Kp0zwUD1r6DmqaeAp6kGY");

// Create a new bot instance
var metBot = new BotEngine(botClient);

// Listen for messages sent to the bot
await metBot.ListenForMessagesAsync();
