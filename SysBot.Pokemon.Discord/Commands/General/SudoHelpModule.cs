using Discord;
using Discord.Commands;
using PKHeX.Core;
using SysBot.Base;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SysBot.Pokemon.Discord
{
    public class SudoHelpModule<T> : ModuleBase<SocketCommandContext> where T : PKM, new()
    {
        private readonly CommandService _service;

        public SudoHelpModule(CommandService service)
        {
            _service = service;
            LogUtil.LogText($"Initialized HelpModule with service: {_service.GetType().Name}");
        }

        [Command("sudohelp")]
        [Alias("sh")]
        [Summary("Lists available commands.")]
        [RequireSudo]
        public async Task SudoHelpAsync()
        {
            LogUtil.LogText("SudoHelpAsync method called");

            var app = await Context.Client.GetApplicationInfoAsync().ConfigureAwait(false);
            var owner = app.Owner.Id;
            var uid = Context.User.Id;

            List<string> descriptions = new List<string>();

            foreach (var module in _service.Modules)
            {
                // Include only certain modules
                if (!ShouldIncludeModule(module))
                    continue;

                string description = "";
                HashSet<string> mentioned = new HashSet<string>();
                foreach (var cmd in module.Commands)
                {
                    var name = cmd.Name;
                    if (mentioned.Contains(name))
                        continue;
                    if (cmd.Attributes.Any(z => z is RequireOwnerAttribute) && owner != uid)
                        continue;
                    if (cmd.Attributes.Any(z => z is RequireSudoAttribute) && !SysCordSettings.Manager.CanUseSudo(uid))
                        continue;

                    mentioned.Add(name);
                    var result = await cmd.CheckPreconditionsAsync(Context).ConfigureAwait(false);
                    if (result.IsSuccess)
                        description += $"{cmd.Aliases[0]}\n";
                }
                if (string.IsNullOrWhiteSpace(description))
                    continue;

                var moduleName = module.Name;
                var gen = moduleName.IndexOf('`');
                if (gen != -1)
                    moduleName = moduleName[..gen];

                descriptions.Add($"**{moduleName}**\n{description}\n");
            }

            var pageContent = ExtraCommandUtil<T>.ListUtilPrep(descriptions, 250); // Adjust page length as needed
            await ExtraCommandUtil<T>.ListUtil(Context, "__List of Available Sudo/Owner Commands!__", pageContent);
        }

        private bool ShouldIncludeModule(ModuleInfo module)
        {
            var includedModules = new List<string> { "LogModule", "PingModule", "RemoteControlModule", "BotModule", "EchoModule", "HubModule", "OwnerModule", "SudoModule", "TradeStartModule" };
            string moduleNameWithoutGeneric = module.Name.Split('`')[0];
            return includedModules.Contains(moduleNameWithoutGeneric);
        }

        [Command("help")]
        [Summary("Lists information about a specific command.")]
        public async Task HelpAsync([Summary("The command you want help for")] string command)
        {
            LogUtil.LogText($"HelpAsync method (with parameter: {command}) called");

            var result = _service.Search(Context, command);

            if (!result.IsSuccess)
            {
                LogUtil.LogText($"Command {command} not found");
                await ReplyAsync($"Sorry, I couldn't find a command like **{command}**.").ConfigureAwait(false);
                return;
            }

            var builder = new EmbedBuilder
            {
                Color = new Color(114, 137, 218),
                Description = $"Here are some commands like **{command}**:",
            };

            foreach (var match in result.Commands)
            {
                var cmd = match.Command;

                builder.AddField(x =>
                {
                    x.Name = string.Join(", ", cmd.Aliases);
                    x.Value = GetCommandSummary(cmd);
                    x.IsInline = false;
                });
            }

            LogUtil.LogText("Sending reply with specific command help embed");
            await ReplyAsync("Help has arrived!", false, builder.Build()).ConfigureAwait(false);
        }

        private static string GetCommandSummary(CommandInfo cmd)
        {
            return $"Summary: {cmd.Summary}\nParameters: {GetParameterSummary(cmd.Parameters)}";
        }

        private static string GetParameterSummary(IReadOnlyList<ParameterInfo> p)
        {
            if (p.Count == 0)
                return "None";
            return $"{p.Count}\n- " + string.Join("\n- ", p.Select(GetParameterSummary));
        }

        private static string GetParameterSummary(ParameterInfo z)
        {
            var result = z.Name;
            if (!string.IsNullOrWhiteSpace(z.Summary))
                result += $" ({z.Summary})";
            return result;
        }
    }
}
