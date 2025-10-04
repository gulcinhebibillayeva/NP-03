using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Text.Json;

var ip = IPAddress.Parse("127.0.0.1");
var port = 27001;
var listener = new TcpListener(ip, port);
listener.Start();
while (true)
{
    var client = listener.AcceptTcpClient();
    var stream = client.GetStream();
    var bw = new BinaryWriter(stream);
    var br = new BinaryReader(stream);

    while (true)
    {
        var input = br.ReadString();
        Console.WriteLine(input);
        var command = JsonSerializer.Deserialize<Command>(input);
        switch (command.Text)
        {
            case Command.ProcessList:
                var processes = Process.GetProcesses();
                var processNames = JsonSerializer.Serialize(processes.Select(p => p.ProcessName));
                bw.Write(processNames);
                break;
            case Command.Run:
                var process = Process.Start(command.Param!);
                bw.Write($"{process.ProcessName} started");
                break;
            case Command.Kill:
  
                var processes2 = Process.GetProcessesByName(command.Param);
                foreach (var proc in processes2)
                {
                    try
                    {
                        proc.Kill();
                        bw.Write($"{proc.ProcessName} killed");
                    }
                    catch (Exception ex)
                    {
                        bw.Write($"{proc.ProcessName} failed");
                    }
                }
                break;
        }
    }
}