# PyraChat
## Modern IRC Client and Framework
PyraChat uses WPF to provide a smooth and modern look that many older IRC clients lack today.

Supports most of [RFC 1459](https://tools.ietf.org/html/rfc1459), [RFC 2812](https://tools.ietf.org/html/rfc2812), and a bit of [IRCv3](http://ircv3.net/), with modifications to better support modern IRC servers.

### Framework
PyraChat is built on our own IRC framework using .NET. It can be utilized in your own client or bot.

````
var irc = new Client("irc.example.com", 6667, new User("Nick", "Real Name", "Ident"));
irc.IRCMessage += message => Console.WriteLine(message.Text);
irc.Connect += () =>
{
    irc.Send(new JoinMessage("#pyrachat-example"));
    irc.Send(new PrivateMessage("#pyrachat-example", "Hello World!"));
};
````
