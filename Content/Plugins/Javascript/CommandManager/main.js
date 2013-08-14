var CommandManager = function ()
{
    this.commands = {};
    
    this.AddCommand = function (command)
    {
        this.commands[command.command.toString()] = command;
        LogInfo("Registered command: " + this.commands[command.command.toString()].command);
    };
    
    this.HandleCommand = function (command, client)
    {
        command = command.toString();
        var arguments = command.split(/ /g);
        var cmdIdentifier = arguments[0].toString();
        var targetCommand = this.commands[cmdIdentifier];
        
        if (targetCommand != undefined)
        {
            var argCount = arguments.length - 1; // Subtract 1 because first entry is the command identifier.
            
            if (argCount < targetCommand.minArgCount && argCount > 0)
            {
                client.SendServerMessage(targetCommand.notEnoughArgumentsString);
                return true;
            }
            else if (argCount <= 0)
            {
                client.SendServerMessage(targetCommand.helpString);
                return true;
            }

            targetCommand.args = arguments.length > 1 ? arguments.splice(1, arguments.length - 1) : [];
            targetCommand.HandleCommand(client);
        
            return true;
        }

        return false;
    };

    LogInfo("[CommandManager] Initialized.");
};

var Command = function (command, helpString, notEnoughArgumentsString, minArgCount, callback)
{
    this.command = command;
    this.helpString = helpString;
    this.notEnoughArgumentsString = notEnoughArgumentsString;
    this.HandleCommand = callback;
    this.args = [];
    this.minArgCount = minArgCount;
    
    this.JoinArgs = function (start, length)
    {
        var args = this.args;
    
        if (start == undefined)
            start = 0;
    
        if (start < 0)
            start = 0;

        if (start > args.length)
            return "";
    
        if (length == undefined)
            length = args.length - start;
    
        if (start + length >= args.length)
            length = args.length - start;
    
        var result = "";

        for (var i = start; i < start + length; ++i)
        {
            var arg = args[i];
        
            result += arg + " ";
        }
    
        result = result.substr(0, result.length -  1); // Remove last space added above.

        return result;
    };
};

SetGlobal("CommandManager", new CommandManager());
SetGlobal("CommandManager_Command", Command);

// Include commands here
include("CommandManager/teleport.js");

AddHook("OnChatMessage", function (args)
{
    var message = args.Message.toString();
    
    if (message.substr(0, 1) == "/")
    {
        args.Canceled = true;
        if (!commandManager.HandleCommand(message.trim().substring(1), args.Client))
        {
            args.Client.SendServerMessage("Unknown command. (" + message + ")");
        }
    }
});