using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;

namespace SCUR_bot
{
    public class Program
    {
        private DiscordSocketClient _client;
        public static CommandService _commands;
        private LogginService _logginService;
        private IServiceProvider _services;
        private CommandHandler _commandHandler;
        public static Task Main(string[] args)
            => new Program().MainAsync();

        public async Task MainAsync()
        {
            _client = new DiscordSocketClient(new DiscordSocketConfig
            {
                MessageCacheSize = 50
            });

            _commands = new CommandService(new CommandServiceConfig
            {
                LogLevel = LogSeverity.Info,
                CaseSensitiveCommands = false,
                IgnoreExtraArgs = true
            });

            _logginService = new LogginService(_client, _commands);

            _services = new ServiceCollection()
               .AddSingleton(_client)
               .AddSingleton(_commands)
               .AddSingleton(_logginService)
               .BuildServiceProvider();

            _commandHandler = new CommandHandler(_client, _commands, _services);

            await _commands.AddModulesAsync(assembly: Assembly.GetEntryAssembly(), 
                services: _services);


            await _commandHandler.InstallCommandsAsync();

            var token = File.ReadAllText("token.txt");

            await _client.LoginAsync(TokenType.Bot, token);

            await _client.StartAsync();

            // Block this task until the program is closed.
            await Task.Delay(-1);
        }

    }
}
