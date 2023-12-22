using Butterfly;

public class Client : Controller.Board
{
    public void Start()
    {
        SystemInformation("start");

        gameClient.Client client = new gameClient.Client();

        client.Start("login", "password", 
            server.Header.ADDRESS, server.Header.SSL_PORT, 
                server.Header.TCP_PORT);
    }
}

public class ClientTest
{
    // Записываем пакеты.
    private byte[][] bytes = new byte[1024][];

    public void SendUpDownLeftMove()
    {
    }
}

public class ServerTest
{
    public void Receive()
    {
    }
}