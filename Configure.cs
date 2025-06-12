internal class Configure
{
    public string ApiKey { get; set; }
    public string sudo_ntpdate_ChannelID { get; set; }

    public override string ToString()
    {
        return $"ApiKey={ApiKey} sudo_ntpdate_ChannelID={sudo_ntpdate_ChannelID}";
    }
}

record Channel(int id, string name);