using Discord;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Timers;
using FluentScheduler;


class DCBot(Configure configure)
{
    private DiscordSocketClient _client;
    public async Task MainAsync()
    {
        var DC_config = new DiscordSocketConfig
        {
            GatewayIntents = GatewayIntents.All
        };

        _client = new DiscordSocketClient(DC_config);
        _client.MessageReceived += CommandHandler;
        _client.Log += Log;

        await _client.LoginAsync(TokenType.Bot, configure.ApiKey);
        await _client.StartAsync();

        Schedules();

        await Task.Delay(-1);
    }

    private void Schedules()
    {
        NationalStandardTimer();
        kasperskyViriusReportNotify();
        // Notify(configure.Channel_sobel_haiyaa_dev__general, $"中央報時\n現在時間:{DateTime.Now}").GetAwaiter().GetResult();
    }

    private void kasperskyViriusReportNotify()
    {
        JobManager.AddJob(async () =>
        {
            string message = $"現在時間:{DateTime.Now}\n請A班同仁於1555時將卡巴病毒報告匯出成PDF";
            await Notify(configure.Channel_sobel_haiyaa_dev__general, message);
        }, s => s.ToRunEvery(1).Days().At(15, 55));
        JobManager.AddJob(async () =>
        {
            string message = $"現在時間:{DateTime.Now}\n請B班同仁於2355時將卡巴病毒報告匯出成PDF\n匯出完後將當天全部的PDF檔案寄outlook給管制官";
            await Notify(configure.Channel_sobel_haiyaa_dev__general, message);
        }, s => s.ToRunEvery(1).Days().At(23, 55));
        JobManager.AddJob(async () =>
        {
            string message = $"現在時間:{DateTime.Now}\n請C班同仁於0755時將卡巴病毒報告匯出成PDF";
            await Notify(configure.Channel_sobel_haiyaa_dev__general, message);
        }, s => s.ToRunEvery(1).Days().At(07, 55));
    }

    private void NationalStandardTimer()
    {
        JobManager.AddJob(async () =>
        {
            await Notify(configure.Channel_sobel_haiyaa_dev__general, $"中央報時\n現在時間:{DateTime.Now}");
        }, s => s.ToRunEvery(30).Seconds());
    }

    private async Task Notify(string ChannelID, string Message)
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

    private Task Log(LogMessage msg)
    {
        Console.WriteLine(msg.ToString());
        return Task.CompletedTask;
    }

    // command sample -> 「!6」
    private Task CommandHandler(SocketMessage message)
    {

        string command = "";
        int lengthOfCommand = -1;

        if (!message.Content.StartsWith('!'))
            return Task.CompletedTask;

        if (message.Author.IsBot)
            return Task.CompletedTask;

        if (message.Content.Contains(' '))
            lengthOfCommand = message.Content.IndexOf(' ');
        else
            lengthOfCommand = message.Content.Length;

        command = message.Content.Substring(1, lengthOfCommand - 1).ToLower();

        if (command.Equals("6"))
        {
            message.Channel.SendMessageAsync($@"屌你老母 {message.Author.Mention}");
        }
        return Task.CompletedTask;
    }
}

