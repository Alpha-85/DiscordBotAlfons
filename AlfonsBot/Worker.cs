using AlfonsBot.Common.Settings;
using AlfonsBot.Services;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using System.Reflection;


namespace AlfonsBot;

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;
    private readonly IConfiguration _configuration;

    private DiscordClient _discordClient;

    public Worker(ILogger<Worker> logger, IConfiguration configuration)
    {
        this._logger = logger;
        this._configuration = configuration;
    }

    public override async Task StartAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Starting discord bot");

        var discordBotToken = _configuration["DiscordBotToken"];
        _discordClient = new DiscordClient(new DiscordConfiguration()
        {
            Token = discordBotToken,
            TokenType = TokenType.Bot,
            Intents = DiscordIntents.AllUnprivileged
        });

        RegisterServices();
        await _discordClient.ConnectAsync();
        await Task.Delay(-1, cancellationToken);
    }



    public override async Task StopAsync(CancellationToken cancellationToken)
    {
        await _discordClient.DisconnectAsync();
        _discordClient.Dispose();
        _logger.LogInformation("Discord bot stopped");
    }

    private void RegisterServices()
    {
        var services = new ServiceCollection()
            .AddScoped<HttpClient>()
            .AddScoped<Random>()
            .AddScoped<ComedyService>()
            .AddScoped<RecipeService>()
            .Configure<UserSettings>(opt => _configuration.GetSection("UserSettings").Bind(opt))
            .BuildServiceProvider();


        var commands = _discordClient.UseCommandsNext(new CommandsNextConfiguration()
        {
            StringPrefixes = new[] { "!" },
            UseDefaultCommandHandler = true,
            Services = services
        });

        commands.RegisterCommands(Assembly.GetExecutingAssembly());
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken) => Task.CompletedTask;

}





