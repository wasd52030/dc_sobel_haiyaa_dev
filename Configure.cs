internal class Configure
{
    public string ApiKey { get; set; }
    public string Channel_sobel_haiyaa_dev__general { get; set; }

    public override string ToString()
    {
        return $"ApiKey={ApiKey} Channel_sobel_haiyaa_dev__general={Channel_sobel_haiyaa_dev__general}";
    }
}

record Channel(int id, string name);