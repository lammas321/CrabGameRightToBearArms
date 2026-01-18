using ChatCommands;
using CrabDevKit.Utilities;
using System.Collections.Generic;
using System.Linq;
using static ChatCommands.CommandArgumentParser;

namespace RightToBearArms
{
    public class ItemsCommand : BaseCommand
    {
        public ItemsCommand()
        {
            id = "items";
            description = "Lists all of the items that can be given.";
            args = new([
                new(
                    [typeof(int)],
                    "page",
                    false
                )
            ]);
        }

        public BaseCommandResponse ShowItemsPage(BaseExecutionMethod executionMethod, int page = 1)
        {
            if (page < 1)
                return new BasicCommandResponse(["You didn't specify a valid page number."], CommandResponseType.Private);

            int currentPage = 1;
            int currentLine = 0;
            List<string> lines = [string.Empty];

            ItemData[] items = [.. ItemManager.idToItem.Values];
            for (int i = 0; i < items.Length; i++)
            {
                ItemData item = items[i];
                string str = item.itemName;
                if (i != items.Length - 1)
                    str += ", ";

                if (lines[currentLine].Length + str.Length - 1 > executionMethod.MaxResponseLength)
                {
                    currentLine++;
                    if (currentLine == 3)
                    {
                        if (currentPage + 1 > page)
                            break;

                        currentPage++;
                        currentLine = 0;
                        lines = [];
                    }

                    lines.Add(string.Empty);
                }

                lines[currentLine] += str;
            }

            if (page != currentPage)
                return new BasicCommandResponse(["You didn't specify a valid page number."], CommandResponseType.Private);

            return new StyledCommandResponse($"Items Page #{page}", [.. lines], CommandResponseType.Private);
        }

        public override BaseCommandResponse Execute(BaseExecutionMethod executionMethod, object executorDetails, string args, bool ignorePermissions = false)
        {
            if (args.Length == 0)
                return ShowItemsPage(executionMethod, 1);

            ParsedResult<int> pageResult = Api.CommandArgumentParser.Parse<int>(args);
            if (pageResult.successful)
                return ShowItemsPage(executionMethod, pageResult.result);

            return new BasicCommandResponse(["You didn't specify a valid page number."], CommandResponseType.Private);
        }
    }

    public class GiveCommand : BaseCommand
    {
        public GiveCommand()
        {
            id = "give";
            description = "Gives players an item.";
            args = new([
                new(
                    [typeof(DefaultCommandArgumentParsers.OnlineClientId[]), typeof(DefaultCommandArgumentParsers.OnlineClientId)],
                    "player(s)",
                    true
                ),
                new(
                    [typeof(ItemData)],
                    "item",
                    true
                ),
                new(
                    [typeof(int)],
                    "ammo"
                )
            ]);
        }

        public override BaseCommandResponse Execute(BaseExecutionMethod executionMethod, object executorDetails, string args, bool ignorePermissions = false)
        {
            if (GameManager.Instance == null)
                return new BasicCommandResponse(["You cannot give items to players right now."], CommandResponseType.Private);

            if (args.Length == 0)
                return new BasicCommandResponse(["A player selector or player is required for the first argument."], CommandResponseType.Private);

            IEnumerable<ulong> clientIds;
            ParsedResult<DefaultCommandArgumentParsers.OnlineClientId[]> playersResult = Api.CommandArgumentParser.Parse<DefaultCommandArgumentParsers.OnlineClientId[]>(args);
            if (playersResult.successful)
            {
                clientIds = playersResult.result.Select(clientId => (ulong)clientId);
                args = playersResult.newArgs;
            }
            else
            {
                ParsedResult<DefaultCommandArgumentParsers.OnlineClientId> playerResult = Api.CommandArgumentParser.Parse<DefaultCommandArgumentParsers.OnlineClientId>(args);
                if (playerResult.successful)
                    clientIds = [playerResult.result];
                else
                    return new BasicCommandResponse(["You did not select any players."], CommandResponseType.Private);
                args = playerResult.newArgs;
            }

            if (args.Length == 0)
                return new BasicCommandResponse(["An item is required for the second argument."], CommandResponseType.Private);

            ParsedResult<ItemData> itemResult = Api.CommandArgumentParser.Parse<ItemData>(args);
            if (!itemResult.successful)
                return new BasicCommandResponse(["You didn't specify an existing item."], CommandResponseType.Private);

            int ammo = itemResult.result.currentAmmo;
            ParsedResult<int> ammoResult = Api.CommandArgumentParser.Parse<int>(itemResult.newArgs);
            if (ammoResult.successful)
                if (ammo >= 0)
                    ammo = ammoResult.result;
                else
                    return new BasicCommandResponse(["You didn't specify a positive ammo count."], CommandResponseType.Private);

            foreach (ulong clientId in clientIds)
                if (GameManager.Instance.activePlayers.ContainsKey(clientId) && !GameManager.Instance.activePlayers[clientId].dead)
                    GiveUtil.GiveItem(clientId, itemResult.result.itemID, ammo);
            return new BasicCommandResponse([], CommandResponseType.Private);
        }
    }
}