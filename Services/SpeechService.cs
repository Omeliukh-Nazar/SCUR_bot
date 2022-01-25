using Microsoft.CognitiveServices.Speech;
using Microsoft.CognitiveServices.Speech.Audio;
using NAudio.Lame;
using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace SCUR_bot
{
    public class SpeechService
    {
        internal static async Task TextToWav(string msg)
        {
            string key = File.ReadAllText("Subscription.txt");
            var config = SpeechConfig.FromSubscription(key, "westeurope");
            config.SpeechSynthesisLanguage = "uk-UA";
            config.SpeechSynthesisVoiceName = "uk-UA-PolinaNeural";

            using var synthesizer = new SpeechSynthesizer(config);
            var result = await synthesizer.SpeakTextAsync(msg);
            
            using var stream = AudioDataStream.FromResult(result);
            await stream.SaveToWaveFileAsync(Program.SpeechDirectoryWav);
        }

    }
}
