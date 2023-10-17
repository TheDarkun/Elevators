<div align="center">
    <h1>Elevators 🛗</h1>
</div>
<br />

## Ascend to Victory or be the Last One Standing in Elevators – The Ultimate Multiplayer Thrill! 🎮

Prepare for an electrifying twist on the classic rock-paper-scissors game with Elevators! Your mission is simple: reach the top floor or be the last player standing. With three dynamic choices at your fingertips – CUT, IDLE, and UP – you hold the power to outsmart, outmaneuver, and outplay your opponents. Will you strategically advance with UP, cunningly counter with CUT, or play it safe with IDLE? The adrenaline rush is guaranteed as you battle it out with friends or foes in this exhilarating mind game. Are you ready to ride the Elevators and claim victory?

## What technologies are being used? 💻

- The frontend is built using [Blazor WebAssembly](https://dotnet.microsoft.com/en-us/apps/aspnet/web-apps/blazor), powered by the robust [ASP.NET](https://dotnet.microsoft.com/en-us/apps/aspnet) backend. This combination ensures a seamless and interactive web interface.
- Additionally, there is also integrated a feature-rich Discord bot using [DSharp+](https://dsharpplus.github.io/DSharpPlus/). This bot allows you to take control of games, manage game sessions, and receive real-time updates.
- Everything is tightly integrated with a [MySQL](https://www.mysql.com/) database to manage and store data efficiently.
- The versatility of [Tailwind CSS](https://tailwindui.com) has also been integrated to style and design the web components.
- Stay tuned for more exciting enhancements!

## How to test the project by yourself 🧑🏿‍💻

Thank you for having an interest in the project! 💖

Before trying to run the project, you have to go to the <b>Server/appsettings.json</b> and replace all the values.
- Auth:Token is just a random string sequence
- Discord:AppId and Discord:AppSecret are your [Application Info](https://discord.com/developers/applications)
- Discord:BotToken is your bots token

You will need to install the [SDK](https://dotnet.microsoft.com/en-us/download/dotnet/7.0). After that you can either open the solution with an IDE such as [Visual Studio](https://visualstudio.microsoft.com/#vs-section) or [Rider](https://www.jetbrains.com/rider/) and then run either <b>Server: Watch With Discord</b> or <b>Server: Watch Without Discord</b>

If you just want to run the project via terminal, go to the <b>Server</b> folder and run either
```bash
dotnet watch run --launch-profile "Watch With Discord"
```
or
```bash
dotnet watch run --launch-profile "Watch Without Discord"
```

## How to start the database and Adminer 🐬

MySQL port: <b>3307</b><br>
Adminer port: <b>8080</b> 

MySQL credentials:
- <b>user: root</b>
- <b>password: example</b>

Firstly you of course have to go to the directory which contains all the docker files and then type the command bellow. The common issue with it not booting is that you probably forgot to open the docker first.
```bash
docker-compose up -d
```

Then you of course just put the docker back to bed.
```bash
docker-compose down
```

### How to save the data 💾

If you've made some important changes to the database and now want to store the data, you <b>HAVE TO</b> use the <b>save.sh</b> or <b>save.bat</b> script. 

This is intentional, because you really don't want to automatically save random debug stuff.

### What to do if your data hasn't loaded 💀 

If the data did not load, try the <b>load.sh</b> or <b>load.bat</b> to manually load the data. If the issue persists, please create a new [issue](https://github.com/TheDarkun/Elevators/issues).