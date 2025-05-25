using Samples.Messages.Abstractions;

namespace Samples.Messages;

internal sealed record KilledMessage(
    string AttackerName,
    string TargetName) :
    IMessage;