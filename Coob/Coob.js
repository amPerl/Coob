var latestVersion = 3;

function onClientConnect(ip) {
	LogInfo("Client connecting from " + ip);
	return true;
}

function onClientVersion(version, client) {
	return true;
}

function onClientJoin(client) {
	var id = client.ID;
	var entity = client.Entity;
	var name = entity.Name;

	LogInfo("Client #" + id + ", " + name + " has joined.");
	return true;
}

function onEntityUpdate(entity, changed, client) {

	return true;
}

function onChatMessage(message, client) {
	LogInfo("<" + client.Entity.Name + "> " + message);

	if (message.StartsWith("/help")) {
		client.SendServerMessage("There are no commands yet, sorry");
		return false;
	}

	return true;
}