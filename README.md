<div align="center">
    <h1>Elevators 🛗</h1>
</div>
<br />

### Ascend to Victory or be the Last One Standing in Elevators – The Ultimate Multiplayer Thrill! 🎮

Prepare for an electrifying twist on the classic rock-paper-scissors game with Elevators! Your mission is simple: reach the top floor or be the last player standing. With three dynamic choices at your fingertips – CUT, IDLE, and UP – you hold the power to outsmart, outmaneuver, and outplay your opponents. Will you strategically advance with UP, cunningly counter with CUT, or play it safe with IDLE? The adrenaline rush is guaranteed as you battle it out with friends or foes in this exhilarating mind game. Are you ready to ride the Elevators and claim victory?

### What technologies are being used? 💻

- The frontend is built using [Blazor WebAssembly](https://dotnet.microsoft.com/en-us/apps/aspnet/web-apps/blazor), powered by the robust [ASP.NET](https://dotnet.microsoft.com/en-us/apps/aspnet) backend. This combination ensures a seamless and interactive web interface.
- Additionally, there is also integrated a feature-rich Discord bot using [DSharp+](https://dsharpplus.github.io/DSharpPlus/). This bot allows you to take control of games, manage game sessions, and receive real-time updates.
- Everything is tightly integrated with a [MySQL](https://www.mysql.com/) database to manage and store data efficiently.
- The versatility of [Tailwind CSS](https://tailwindui.com) has also been integrated to style and design the web components.
- Stay tuned for more exciting enhancements!

### How to start the database and Adminer 🐬

Mysql port: <b>3037</b><br>
Adminer port: <b>8080</b>

Firstly you of course have to go to the directory which contains all the docker files and then type the command bellow. The common issue with it not booting is that you probably forgot to open the docker first.
```bash
docker-compose up -d
```

If you've made some important changes to the database and now want to store the data, you <b>HAVE TO</b> type out the command below or all your progress will be lost.
```bash
docker exec elevators-mysql /usr/bin/mysqldump -u root --password=example --all-databases > ./mysql-dump/backup.sql
```

Then you of course just put the docker back to bed.
```bash
docker-compose down
```