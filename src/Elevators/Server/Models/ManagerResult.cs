using System.Net;

namespace Server.Models;

public record ManagerResult(HttpStatusCode StatusCode, object? Data = null);