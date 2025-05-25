using TheConsoleWitcher.Messages.Abstractions;

namespace TheConsoleWitcher.Messages;

public sealed record AttackedMessage(
    string AttackerName,
    string TargetName,
    float DamageDealt) :
    IMessage;
