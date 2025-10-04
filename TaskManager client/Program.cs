using System.Net;
using System.Net.Sockets;
using System.Text.Json;
var ip = IPAddress.Parse("127.0.0.1");
var port = 27001;
var client = new TcpClient();
client.Connect(ip, port);
var stream = client.GetStream();
var bw = new BinaryWriter(stream); 
var br = new BinaryReader(stream);

Command command = null!;
string response = null!;
string str = null!;
while (true)
{
    Console.WriteLine("Write command name or help");
    str = Console.ReadLine()!.ToUpper();
    if (str == "HELP")
    {
        Console.WriteLine();
        Console.WriteLine("Command List:");
        Console.WriteLine(Command.ProcessList);
        Console.WriteLine($"{Command.Run} <process_name>");
        Console.WriteLine($"{Command.Kill} <process_name>");
        Console.ReadLine();
        Console.Clear();


    }
    var input = str.Split(' ');
    switch (input[0]) {
        case Command.ProcessList:
            command = new Command { Text = input[0] };
            bw.Write(JsonSerializer.Serialize(command));
            response = br.ReadString();
            var processList = JsonSerializer.Deserialize<List<string>>(response);
            processList!.ForEach(p => { Console.WriteLine($"    {p}"); });
            break;
        case Command.Run:
            command = new Command { Text = input[0], Param = input[1] };
            bw.Write(JsonSerializer.Serialize(command));
            response = br.ReadString();
            Console.WriteLine(response);
            break;
        case Command.Kill:
            command = new Command { Text = input[0], Param = input[1] };
            bw.Write(JsonSerializer.Serialize(command));
            response = br.ReadString();
            Console.WriteLine(response);
            break;

    }



}

