namespace Elevators.Store.Game.Actions;

public record StartGameAction(ulong GuildId, ulong ChannelId, int FloorCount);