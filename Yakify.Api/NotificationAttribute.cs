namespace Yakify.Api;

[AttributeUsage(AttributeTargets.Method)]
public class NotificationAttribute(Type hubType): Attribute
{
    public Type HubType => hubType;
}