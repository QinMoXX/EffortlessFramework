using System;
using System.Threading;
using System.Threading.Tasks;

public class ConsoleCommandListener
{
    private CancellationTokenSource cancellationTokenSource;

    public void Start()
    {
        cancellationTokenSource = new CancellationTokenSource();
        Task.Run(() => ListenForCommands(cancellationTokenSource.Token));
    }

    public void Stop()
    {
        cancellationTokenSource.Cancel();
        Program.isRunning = false;
    }

    private void ListenForCommands(CancellationToken cancellationToken)
    {
        Console.WriteLine("Console command listener started. Type 'exit' to stop.");

        while (!cancellationToken.IsCancellationRequested)
        {
            if (Console.KeyAvailable) // 检查是否有输入
            {
                string command = Console.ReadLine();
                ProcessCommand(command);
            }
        }

        Console.WriteLine("Console command listener stopped.");
    }

    private void ProcessCommand(string command)
    {
        switch (command.ToLower())
        {
            case "exit":
                Stop();
                break;
            case "status":
                Console.WriteLine("Server is running...");
                break;
            // 添加更多命令处理
            default:
                Console.WriteLine($"Unknown command: {command}");
                break;
        }
    }
}