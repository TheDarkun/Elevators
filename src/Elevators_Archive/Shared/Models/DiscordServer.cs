namespace Shared.Models;

public sealed class DiscordServer
{
    public string Id { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Icon { get; set; } = string.Empty;
    public bool Owner { get; set; }
    public long Permissions { get; set; }
}
