using System;

namespace DotNetCoreDecorators
{
    public interface IDomainObjectTimeStamp
    {
        
        string Id { get; }
        DateTime TimeStamp { get; }
    }
}