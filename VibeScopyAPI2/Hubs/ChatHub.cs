using Microsoft.AspNetCore.SignalR;

public class ChatHub : Hub
{
    private readonly Dictionary<string, List<string>> channels = new Dictionary<string, List<string>>();
    private static Dictionary<string, string> userConnections = new Dictionary<string, string>();

    //TODO: Faire une sécurité pour empêcher n'importe qui d'accéder à un canal
    public async Task JoinChannel(string channelName)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, channelName);
    }

    public async Task QuitChannel(string channelName)
    {
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, channelName);
    }

    [HubMethodName(nameof(SendMessage))]
    public async Task<string> SendMessage(string user, string message)
    {
        await Clients.All.SendAsync("ReceiveMessage", user, message);
        return "lol";
    }

    // Méthode pour ajouter une paire de personnes à un canal
    public void AddToChannel(string channelName, string user1, string user2)
    {
        if (!channels.ContainsKey(channelName))
            channels[channelName] = new List<string>();

        channels[channelName].Add(user1);
        channels[channelName].Add(user2);
    }

    [HubMethodName(nameof(aClientProvidedFunction))]
    public async Task aClientProvidedFunction(string user, string message)
    {
        var lol = 0;
    }
    [HubMethodName(nameof(MethodOneSimpleParameterSimpleReturnValueAsync))]
    public async Task<string> MethodOneSimpleParameterSimpleReturnValueAsync(string p1)
    {
        Console.WriteLine($"'MethodOneSimpleParameterSimpleReturnValue' invoked. Parameter value: '{p1}");
        await Clients.Caller.SendAsync("aClientProvidedFunction", null);
        await aClientProvidedFunction("","");
        return p1;
    }

    [HubMethodName(nameof(Connect))]
    public async Task<string> Connect(string uid)
    {
        // Associer l'UID à la connexion SignalR de l'utilisateur
        userConnections[uid] = Context.ConnectionId;
        return "xd";
    }

    [HubMethodName(nameof(SendMessageToUser))]
    public async Task<string> SendMessageToUser(string uid, string message)
        {
            if (userConnections.ContainsKey(uid))
            {
                // Envoyer le message uniquement à la connexion de l'utilisateur spécifique
                Clients.Client(userConnections[uid]).SendAsync("ReceiveMessage", message);
            }
        return "xd";
        }

    [HubMethodName(nameof(OnDisconnectedAsync))]
    public override Task OnDisconnectedAsync(Exception exception)
    {
        // Retirer l'association de l'UID lorsqu'un utilisateur se déconnecte
        var uid = userConnections.FirstOrDefault(x => x.Value == Context.ConnectionId).Key;
        if (uid != null)
        {
            userConnections.Remove(uid);
        }
        return base.OnDisconnectedAsync(exception);
    }

}