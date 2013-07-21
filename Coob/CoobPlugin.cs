using System;


public class CoobPlugin
{
    public void Main()
    {

    }

    public bool onInitialize()
    {
        return true;
    }

    public bool onQuit()
    {
        return true;
    }

    public bool onClientConnect(string ip)
    {
        return true;
    }

    public bool onClientDisconnect()
    {
        return true;
    }

    public bool onClientVersion(int version, object client)
    {
        return true;
    }

    public bool onClientJoin(object client)
    {
        Console.WriteLine("Join from {0} noticed by plugin!", client.ToString());
        return true;
    }

    public bool onEntityUpdate(object entity, object changed, object client)
    {
        return true;
    }

    public bool onChatMessage(string message, object client)
    {
        return true;
    }
}
