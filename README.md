# Telegra_Csharp_C2
Command and Control for C# Writing

Author:  Leiothrix

Telegram:  @Leiothrix

Twitter:  @wh4am1

Team:  QQ愛&Love

![logo](https://github.com/sf197/Telegra_Csharp_C2/tree/master/images/logo.png)

# install
###### Nuget download these package

```Csharp
using System.IO;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types.InputFiles;
using AForge.Video;
using AForge.Controls;
using AForge.Video.DirectShow;
```

###### Add related classes in 'References'

```c#
System.Drawing;
System.Windows.Forms;
```



# How to used

###### Modify your Token to the program

```csharp
 static void Main(){
            botClient = new TelegramBotClient("token");		//Your Token
            botClient.OnMessage += Bot_OnMessage;
            botClient.StartReceiving();
            Thread.Sleep(int.MaxValue);
}
```

![1566355269804](https://github.com/sf197/Telegra_Csharp_C2/tree/master/images/1.png)

###### have good fun.

# Proxy

###### TelegramBotClient allows you to use a proxy for Bot API connections. 



#### HTTP

###### You can pass an IWebProxy to bot client for HTTP Proxies.

```csharp
	// using System.Net;

	var httpProxy = new WebProxy("https://example.org", 8080)
	{
     	Credentials = new NetworkCredential("USERNMAE", "PASSWORD")
	};
	var botClient = new TelegramBotClient("YOUR_API_TOKEN", httpProxy);
```



#### SOCKS 5

###### You can use an external NuGet package: [HttpToSocks5Proxy](https://www.nuget.org/packages/HttpToSocks5Proxy/) provided 

```csharp
// using MihaZupan;

var proxy = new HttpToSocks5Proxy(Socks5ServerAddress, Socks5ServerPort);

// Or if you need credentials for your proxy server:
var proxy = new HttpToSocks5Proxy(
  Socks5ServerAddress, Socks5ServerPort, "USERNAME", "PASSWORD"
);

// Allows you to use proxies that are only allowing connections to Telegram
// Needed for some proxies
proxy.ResolveHostnamesLocally = true;

var botClient = new TelegramBotClient("YOUR_API_TOKEN", proxy);
```



还在完善！还在完善！还在完善！
