using Discord;
using Discord.WebSocket;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using FluentScheduler;


class DCBot(Configure configure)
{
    private DiscordSocketClient _client;

    private string CurrentTime => DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
    public async Task MainAsync()
    {
        var DC_config = new DiscordSocketConfig
        {
            GatewayIntents = GatewayIntents.All
        };

        _client = new DiscordSocketClient(DC_config);
        _client.MessageReceived += HandleCommandAsync;
        _client.Log += LogAsync;

        await _client.LoginAsync(TokenType.Bot, configure.ApiKey);
        await _client.StartAsync();

        Schedules();

        await Task.Delay(-1);
    }

    private void Schedules()
    {
        NationalStandardTimer();
        kasperskyViriusReportNotify();
        EveryDay1808();
        // Notify(configure.Channel_sobel_haiyaa_dev__general, $"中央報時\n現在時間:{DateTime.Now}").GetAwaiter().GetResult();
    }

    private void kasperskyViriusReportNotify()
    {
        JobManager.AddJob(async () =>
        {
            string message = $"現在時間：{CurrentTime}\n請A班同仁於1555時將卡巴病毒報告截圖\n**完成後要寫工作日誌！！！**";
            await NotifyAsync(configure.Channel_sobel_haiyaa_dev__general, message);
        }, s => s.ToRunEvery(1).Days().At(15, 55));
        JobManager.AddJob(async () =>
        {
            string message = $"現在時間：{CurrentTime}\n請B班同仁於2355時將卡巴病毒報告截圖\n匯出完後將當天全部的截圖寄outlook給管制官\n**完成後要寫工作日誌！！！**";
            await NotifyAsync(configure.Channel_sobel_haiyaa_dev__general, message);
        }, s => s.ToRunEvery(1).Days().At(23, 55));
        JobManager.AddJob(async () =>
        {
            string message = $"現在時間：{CurrentTime}\n請C班同仁於0755時將卡巴病毒報告截圖\n**完成後要寫工作日誌！！！**";
            await NotifyAsync(configure.Channel_sobel_haiyaa_dev__general, message);
        }, s => s.ToRunEvery(1).Days().At(07, 55));
    }

    private void NationalStandardTimer()
    {
        JobManager.AddJob(async () =>
        {
            await NotifyAsync(configure.Channel_sobel_haiyaa_dev__general, $"中央報時\n現在時間：{CurrentTime}");
        }, s => s.ToRunEvery(1).Hours().At(00));
    }

    private void EveryDay1808()
    {
        JobManager.AddJob(async () =>
        {
            string message = $"檢整當天 [18-22|18-08] 假別，開電子假單";
            await NotifyAsync(configure.Channel_sobel_haiyaa_dev__general, message);
        }, s => s.ToRunEvery(1).Days().At(08, 00));
    }

    private async Task NotifyAsync(string ChannelID, string Message)
    {
        var api = $"https://discord.com/api/v10/channels/{ChannelID}/messages";

        var data = new { content = Message, tts = false, embeds = new List<string>() };

        string json = JsonSerializer.Serialize(data);
        HttpContent contentPost = new StringContent(json, Encoding.UTF8, "application/json");

        try
        {
            var client = new HttpClient();

            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, api);
            request.Headers.Authorization = new AuthenticationHeaderValue("Bot", configure.ApiKey);
            // request.Headers.Add("Authorization", $"Bot {configure.ApiKey}");
            request.Content = contentPost;

            HttpResponseMessage response = await client.SendAsync(request);
            Console.WriteLine(await response.Content.ReadAsStringAsync());
        }
        catch (System.Exception)
        {
            throw;
        }
    }

    private Task LogAsync(LogMessage msg)
    {
        Console.WriteLine(msg.ToString());
        return Task.CompletedTask;
    }

    // command sample -> 「!6」
    private Task HandleCommandAsync(SocketMessage message)
    {

        if (!message.Content.StartsWith('!'))
            return Task.CompletedTask;

        if (message.Author.IsBot)
            return Task.CompletedTask;

        int lengthOfCommand = message.Content.Contains(' ') ? message.Content.IndexOf(' ') : message.Content.Length;
        string command = message.Content.Substring(1, lengthOfCommand - 1);

        switch (command)
        {
            case "6":
                message.Channel.SendMessageAsync($@"屌你老母 {message.Author.Mention}");
                break;
            case "Xi":
                message.Channel.SendMessageAsync($@"別看{message.Author.Mention}今天鬧得歡，小心啊今後拉清單");
                break;
            case "為什麼要演奏春日影":
                message.Channel.SendMessageAsync($@"**因為春日影是一首好歌**");
                break;
            default:
                break;
        }

        return Task.CompletedTask;
    }
}

