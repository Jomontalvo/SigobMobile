namespace SigobMobile.Models
{
    using System.Collections.Generic;
    using Common.Models;

    /// <summary>
    /// Group Trace Route Model
    /// </summary>
    public class TraceRouteGroup : List<DocumentTraceRoute>
    {
        public CopyTraceRoute Copy  { get; private set; }
        public TraceRouteGroup(CopyTraceRoute copy, List<DocumentTraceRoute> traceRoutes) : base(traceRoutes)
        {
            Copy = copy;
        }
    }
}
