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
        public const string SpeechDirectoryWav = "E:\\file.wav";

        private DiscordSocketClient _client;
        public static CommandService _commands;
        private LogginService _logginService;
        private AudioService _audioService;
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
                CaseSensitiveCommands = false,
                IgnoreExtraArgs = true
            });

            _logginService = new LogginService(_client, _commands);
            _audioService = new AudioService();

            _services = new ServiceCollection()
               .AddSingleton(_client)
               .AddSingleton(_commands)
               .AddSingleton(_audioService)
               .AddSingleton(_logginService)
               .BuildServiceProvider();

            _commandHandler = new CommandHandler(_client, _commands, _services);

            await _commands.AddModulesAsync(assembly: Assembly.GetEntryAssembly(), 
                services: _services);


            await _commandHandler.InstallCommandsAsync();

            var token = File.ReadAllText("token.txt");

            await _client.LoginAsync(TokenType.Bot, token);

            await _client.StartAsync();

            await Task.Delay(-1);
        }

    }
}
