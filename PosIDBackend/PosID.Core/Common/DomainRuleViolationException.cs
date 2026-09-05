namespace PosID.Core.Common;

public sealed class DomainRuleViolationException(string message) : InvalidOperationException(message);
