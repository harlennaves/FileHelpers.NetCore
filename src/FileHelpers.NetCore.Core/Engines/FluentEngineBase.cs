using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FileHelpers.Core.Descriptors;
using FileHelpers.Fluent.Exceptions;

namespace FileHelpers.Core.Engines
{
    public abstract class FluentEngineBase : FluentEventEngineBase
    {
        public IRecordDescriptor Descriptor { get; }

        
        protected FluentEngineBase(IRecordDescriptor descriptor)
            : this(descriptor, Encoding.UTF8)
        {

        }

        protected FluentEngineBase(IRecordDescriptor descriptor, Encoding encoding)
        {
            CheckDescriptor(descriptor);
            Encoding = encoding;
            Descriptor = descriptor;
        }
        
        public abstract string Serialize();

    }
}
