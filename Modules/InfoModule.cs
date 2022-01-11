using Discord;
using Discord.Commands;
using Discord.Rest;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCUR_bot
{
	public class InfoModule : ModuleBase<SocketCommandContext>
	{

		[Command("help")]
		[Alias("help","h")]
		[Summary("List of commands")]
		public async Task HelpAsync()
		{
			List<CommandInfo> commands = Program._commands.Commands.ToList();
			EmbedBuilder embedBuilder = new EmbedBuilder();
            for (int i = 0; i < commands.Count/2; i++)
            {
				// Get the command Summary attribute information
				string embedFieldText = commands[i].Summary ?? "No description available\n";
				string aliasses = string.Join(", ", commands[i].Aliases.ToList());
				embedBuilder.AddField($"{commands[i].Name} [{aliasses}]", embedFieldText);
			}
			await ReplyAsync("Here's a list of commands and their description: ", false, embedBuilder.Build());
		}


		[Command("say")]
		[Alias("say", "s")]
		[Summary("Echoes a message.")]
		public async Task SayAsync(
			[Remainder][Summary("The text to echo")]
		string msg)
		{
			foreach (var item in keyValuePairsSay)
			{
				if (msg.ToLower().Contains(item.Key))
				{
					msg = item.Value;
				}
			}
			await ReplyAsync(msg);
		}



		// square 20 -> 400
		[Command("square")]
		[Alias("square", "sq")]
		[Summary("Squares a number.")]
		public async Task SquareAsync(
			[Summary("The number to square.")]
		int num)
		{
			// We can also access the channel from the Command Context.
			await Context.Channel.SendMessageAsync($"{num}^2 = {Math.Pow(num, 2)}");
		}

		// GetPhotoOfe smth -> photo of smth
		[Command("GetPhotoOf")]
		[Alias("GetPhotoOf", "gpof")]
		[Summary("Get random photo")]
		public async Task GetPhotoOf(
			[Summary("The photo of ...")]
		string name)
		{
			var image = GetRandomPhoto.GetRandomPhotoOf(name);
			await Context.Channel.SendMessageAsync(image);
		}

		// GetPhotoOfe smth -> photo of smth
		[Command("VoteBan")]
		[Alias("VoteBan", "VB")]
		[Summary("Voting for ban ...(sorry for this)")]
		public async Task VoteBan(SocketGuildUser user)
		{
            if (user.Status != UserStatus.Offline)
            {
				await Context.Channel.SendMessageAsync("User is offline it is impossible");
				return;
			}
			var user_role = Context.User as SocketGuildUser;
			var role = (user as IGuildUser).Guild.Roles.FirstOrDefault(x => x.Name == "🏆🎓🥇БОС КАЧАЛКІ🥇🎓🏆");
			if (user.Roles.Contains(role))
			{
				await Context.Channel.SendMessageAsync("Do you want ban?");
				return;
			}
			int voteNeed = 0;
			foreach (var item in Context.Guild.VoiceChannels)
            {
				voteNeed += item.Users.Count;
            }
			var message = await Context.Channel.SendMessageAsync(
				$"Voting for ban {user.Username}#{user.Id} started!{Environment.NewLine}" +
				$"You have 15s to try do it! {Environment.NewLine}Needed {voteNeed} vote");
			voteNeed--;
			await message.AddReactionAsync(new Emoji("✅"));
			await Task.Delay(15000);
			await Ban(message, user, voteNeed);
		}

        public async Task Ban(RestUserMessage message, SocketGuildUser user, int count)
		{
			var emoji_check = new Emoji("✅");
			var reactions = await message.GetReactionUsersAsync(emoji_check, count).CountAsync();
			if (reactions >= count)
            {
				await Context.Channel.SendMessageAsync($"{user.Username} have a good mute! hihihihi");
				await user.ModifyAsync(x =>
				{
					x.Mute = true;
				});
			}
            else
            {
				await Context.Channel.SendMessageAsync($"Try again((");
			}
		}

		private Dictionary<string, string> keyValuePairsSay = new Dictionary<string, string>
		{
			{"nazar","Nazar is my master!"},
			{"назар","Nazar is my master!"},
			{"омелюх","Nazar is my master!"},
			{"омешлюх","Nazar is my master!"},
			{"vlad","It is about Ivanenko?" + Environment.NewLine},
			{"влад","It is about Ivanenko?" + Environment.NewLine},
			{"illia","Dont touch ukrainian cossack" + Environment.NewLine},
			{"ілля","Dont touch ukrainian cossack" + Environment.NewLine},
			{"гармалюк","щур" + Environment.NewLine},
			{"гармащур","щур" + Environment.NewLine},
			{"слава","Героям слава!" + Environment.NewLine}
		};
	}
}
