using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace fmkclient.net
{
    using System.ServiceModel.Channels;
    using System.ServiceModel.Description;
    using System.ServiceModel.Dispatcher;

    class FmkMonitoringEndpointBehaviour : IEndpointBehavior
    {
        public void Validate(ServiceEndpoint endpoint)
        {
        }

        public void AddBindingParameters(ServiceEndpoint endpoint, BindingParameterCollection bindingParameters)
        {
        }

        public void ApplyDispatchBehavior(ServiceEndpoint endpoint, EndpointDispatcher endpointDispatcher)
        {
        }

        public void ApplyClientBehavior(ServiceEndpoint endpoint, ClientRuntime clientRuntime)
        {
            var myClientMessageInspector = new FmkClientMessageInspector();
            clientRuntime.MessageInspectors.Add(myClientMessageInspector);
        }
    }
}
