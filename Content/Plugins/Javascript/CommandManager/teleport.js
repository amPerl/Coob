var cmdman = GetGlobal("CommandManager");
var command = GetGlobal("CommandManager_Command");
cmdman.AddCommand(new command(
    "tp",
    "Teleport to the given coordinates or player.",
    "Not enough arguments. You need to specify X Y Z or player name.",
    1,
    function (client) {
        var x, y, z;
        if (this.args.length == 3) {
            LogInfo("Parsing XYZ for tp command.");
            x = parseInt(this.args[0]);
            y = parseInt(this.args[1]);
            z = parseInt(this.args[2]);

            if (isNaN(x) || isNaN(y) || isNaN(z)) {
                client.SendServerMessage("Invalid coordinates given.");
                return;
            }
            else {
                client.SendServerMessage("Teleported to " + x + "," + y + "," + z + ".");
            }
        }
        else {
            LogInfo("Parsing name for tp command.");
            var playerName = this.JoinArgs();
            LogInfo("Name: " + playerName);
            var target = coob.GetClient(playerName);

            if (target == null) {
                client.SendServerMessage("There's no player with the name \"" + playerName + "\".");
                return;
            }

            var targetPosition = target.Entity.Position;
            x = targetPosition.X;
            y = targetPosition.Y;
            z = targetPosition.Z;
            client.SendServerMessage("Teleported to " + target.Entity.Name);
        }

        // Todo: actually change the position. ;f
    }
));