using Samples.Messages.Abstractions;

namespace Samples.Messages;

public sealed record AttackedMessage(
    string AttackerName,
    string TargetName,
    float DamageDealt) :
    IMessage;
