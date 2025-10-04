internal class Command {
    public const string ProcessList = "PROCLIST";
    public const string Run = "RUN";
    public const string Kill = "KILL";
    public string? Text { get; set; }
    public string? Param { get; set; }

}
