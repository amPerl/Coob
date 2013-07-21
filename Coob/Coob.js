var latestVersion = 3;

AddHook("OnInitialize", function (args)
{
    args.WorldSeed = 1234567890;
});

AddHook("OnClientConnect", function (args)
{
    var ip = args.IP;
    LogInfo("Client connecting from " + ip);
});

AddHook("OnClientVersion", function (args)
{
    LogInfo(args.Client.IP + " version: " + args.Version + ".");
});

AddHook("OnClientJoin", function (args)
{
    var client = args.Client;

    LogInfo("Client #" + client.ID + ", " + client.Entity.Name + " has joined");
});

AddHook("OnEntityUpdate", function (args)
{
    
});

AddHook("OnChatMessage", function (args)
{
    var client = args.Client;
    var message = args.Message;
    
    LogInfo("<" + client.Entity.Name + "> " + message);
    
    var day = 1;
    var time = parseFloat(message);
    
    if (!isNaN(time)) {
        coob.SetTime(day, time);
        coob.SendServerMessage("Time set to " + time + " hours.");
        args.Canceled = true;
    }
});

AddHook("OnQuit", function (args)
{
    
});