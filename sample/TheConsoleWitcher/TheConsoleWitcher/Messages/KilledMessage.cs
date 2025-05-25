using TheConsoleWitcher.Messages.Abstractions;

namespace TheConsoleWitcher.Messages;

internal sealed record KilledMessage(
    string AttackerName,
    string TargetName) :
    IMessage;