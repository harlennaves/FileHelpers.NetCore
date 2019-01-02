using System;

namespace FileHelpers.NetCore.Fluent.Exceptions
{
    public class BadFluentConfigurationException : Exception
    {
        public BadFluentConfigurationException() :
            base("Generic fluent configuration exception")
        {

        }

        public BadFluentConfigurationException(string message)
            : base(message)
        {

        }

        
    }
}
