var latestVersion = 3;
//whitelist
if (System.IO.File.Exists("whitelist.txt")) {
    var whitelists = System.IO.File.ReadAllLines("whitelist.txt");
} else {
    System.IO.File.Create("whitelist.txt");
}
//op
if (System.IO.File.Exists("ops.txt")) {
    var ops = System.IO.File.ReadAllLines("ops.txt");
} else {
    System.IO.File.Create("ops.txt");
}
//ban
if (System.IO.File.Exists("bans.txt")) {
    var ipbans = System.IO.File.ReadAllLines("bans.txt");
} else {
    System.IO.File.Create("bans.txt");
}


function onClientConnect(ip) {
    LogInfo("Client connecting from " + ip);

    if (ipbans.toString() != null) {
        for (var i = 0; i < ipbans.Length; i++) {
            if (ipbans[i] == ip) {
                LogInfo(ip + " is banned, connection refused");
                return false;
            }
        }
    }

    if (ops.toString() != null) {
        for (var i = 0; i < ops.Length; i++) {
            if (ops[i] == ip) {
                LogInfo(ip + " is now logged in as op");
            }
        }
    }

    if (whitelists.toString() != null) {
        for (var i = 0; i < whitelists.Length; i++) {
            if (whitelists[i] == ip) {
                LogInfo(ip + " is whitelisted");
                return true;
            }
            else {
                LogInfo(ip + " is not whitelisted, connection refused");
                return false;
            }
        }
    }
    return true;
}

function onClientDisconnect() {
}

function onClientVersion(version, client) {
}

function onClientJoin(client, ip) {
    LogInfo("Client #" + client.ID + ", " + client.Entity.Name + " has joined");
    return true;
}

function FindInArray(Array, Value) {
    for (var i = 0; i < Array.length; i++) {
        if (Array[i] == Value)
            return i;
    }
    return -1;
}

function onEntityUpdate(entity, changed, client) {
    //    LogInfo("Player's X changed: " + changed.Position.X);
    return true;
}

function onChatMessage(message, client) {
    var Opcheck = FindInArray(ops, client.IP);
    LogInfo("<" + client.Entity.Name + "> " + message);

    if (message.StartsWith("/help")) {
        client.SendServerMessage("/ban - Bans the ip that is given with it.\n/wl - whitelists the ip given with it. \n/op - Op's the ip given with it.");
        return false;
    }

    if (message.StartsWith("/position")) {
        client.SendServerMessage("X: " + client.Entity.Position.X + " / Y: " + client.Entity.Position.Y + " / Z: " + client.Entity.Position.Z);
        return false;
    }

    if (message.StartsWith("/ban ")) {
        if (Opcheck.toString() != -1) {
            var MessageLast = message.split(" ")[1];
            System.IO.File.AppendAllText("bans.txt", MessageLast + "\n");
            LogInfo(MessageLast + " is now banned");
            client.SendServerMessage(MessageLast + " is now banned");
            return false;
        }
        else {
            client.SendServerMessage("You are not allowed to use this command");
        }
        return false;
    }

    if (message.StartsWith("/wl")) {
        if (Opcheck.toString() != -1) {
            var MessageLast = message.split(" ")[1];
            System.IO.File.AppendAllText("whitelist.txt", MessageLast + "\n");
            LogInfo(MessageLast + " is now whitelisted");
            client.SendServerMessage(MessageLast + " is now whitelisted");
            return false;
        }
        else {
            client.SendServerMessage("You are not allowed to use this command");
        }
        return false;
    }

    if (message.StartsWith("/op ")) {
        if (Opcheck.toString() != -1) {
            var MessageLast = message.split(" ")[1];
            System.IO.File.AppendAllText("ops.txt", MessageLast + "\n");
            LogInfo(MessageLast + " is now op'd");
            client.SendServerMessage(MessageLast + " is now op'd");
            return false;
        }
        else {
            client.SendServerMessage("You are not allowed to use this command");
        }
        return false;
    }

    var day = 1;
    var time = parseFloat(message);

    if (!isNaN(time)) {
        coob.SetTime(day, time);
        coob.SendServerMessage("Time set to " + time + " hours.");
    }

    return true;
}
