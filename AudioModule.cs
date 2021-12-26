using Discord;
using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SCUR_bot
{
    public class AudioModule : ModuleBase<ICommandContext>
    {
        private readonly AudioService _service;
        public AudioModule(AudioService service)
        {
            _service = service;
        }

        [Command("join", RunMode = RunMode.Async)]
        public async Task JoinCmd()
        {
            await _service.JoinAudio(Context.Guild, (Context.User as IVoiceState).VoiceChannel);
        }

        [Command("leave", RunMode = RunMode.Async)]
        public async Task LeaveCmd()
        {
            await _service.LeaveAudio(Context.Guild);
        }

        [Command("play", RunMode = RunMode.Async)]
        public async Task PlayCmd([Remainder] string song)
        {
            await _service.SendAudioAsync(Context.Guild, Context.Channel, song);
        }

        [Command("Speech", RunMode = RunMode.Async)]
        [Alias("Sp", "Spech")]
        [Summary("Synthesize the voice")]
        public async Task PlaySpeech([Remainder] string text)
        {
            await SpeechService.TextToWav(text);
            await JoinCmd();
            await _service.SendSpeechAsync(Context.Guild, Context.Channel, Program.SpeechDirectoryWav);
            await LeaveCmd();
        }

    }
}
