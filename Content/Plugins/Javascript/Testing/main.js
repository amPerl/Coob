﻿var latestVersion = 3;
﻿var serverName = "Server Name";
﻿var MOTD = "Welcome to " + serverName + "!";
﻿
AddHook("OnInitialize", function (args) {
    //args.WorldSeed = 1234558078;
});

AddHook("OnClientConnect", function (args) {
    var ip = args.IP;
    LogInfo("Client connecting from " + ip);
});

AddHook("OnClientVersion", function (args) {
    LogInfo(args.Client.IP + " version: " + args.Version + ".");
});

AddHook("OnClientJoin", function (args) {
    var client = args.Client;

    LogInfo("Client #" + client.ID + ", " + client.Entity.Name + " has joined");
    world.SendServerMessage(client.Entity.Name + " has joined.");
    client.SendServerMessage(MOTD)
});

AddHook("OnClientDisconnect", function (args) {
    var client = args.Client;
    var reason = args.Reason;
    
    LogInfo("OnClientDisconnect");
    LogInfo(client.Entity.Name + " disconnected.");
    LogInfo("Clients count: " + coob.Clients.Count);
    world.SendServerMessage(client.Entity.Name + " disconnected. (" + reason + ")");
});

AddHook("OnEntityUpdate", function (args) {

});

AddHook("OnWorldUpdate", function (args)
{
    var dt = args.DeltaTime;
});

AddHook("OnChatMessage", function (args) {
    var client = args.Client;
    var message = args.Message;
   
    LogInfo("<" + client.Entity.Name + "> " + message);

    var day = 1;
    var time = parseFloat(message);

    if (!isNaN(time)) {
        world.SetTime(day, time);
        world.SendServerMessage("Time set to " + time + " hours.");
        args.Canceled = true;
    }
});

AddHook("OnQuit", function (args) {

});
