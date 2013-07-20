var latestVersion = 3;

function onClientConnect(ip) {
    LogInfo("Client connecting from " + ip);
    return true;
}

function onClientVersion(version, client) {
    return true;
}

function onClientJoin(client) {
    LogInfo("Client #" + client.ID + ", " + client.Entity.Name + " has joined.");
    return true;
}

function onEntityUpdate(entity, changed, client) {
    return true;
}

function onChatMessage(message, client) {
    LogInfo("<" + client.Entity.Name + "> " + message);
    
    // Testing time
    var day = 1;
    var time = parseFloat(message);
    
    if (!isNaN(time)) {
        coob.SetTime(day, time);
        coob.SendServerMessage("Time set to " + time + " hours.");
    }

    return true;
}