using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.huayunfly.servicetransit
{
    public interface ISimpleRequest
    {
        DateTime Timestamp { get; }
        string CustomId { get; }
    }
}
