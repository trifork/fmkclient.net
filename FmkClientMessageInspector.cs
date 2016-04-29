using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace fmkclient.net
{
    using System.ServiceModel;
    using System.ServiceModel.Channels;
    using System.ServiceModel.Dispatcher;

    class FmkClientMessageInspector : IClientMessageInspector
    {
        public object BeforeSendRequest(ref Message request, IClientChannel channel)
        {
            // TODO: log the request.

            // If you return something here, it will be available in the 
            // correlationState parameter when AfterReceiveReply is called.
//            request.Headers.Action = "http://www.dkma.dk/medicinecard/xml.schema/2012/06/01#GetMedicineCard";
            
            return null;
        }

        public void AfterReceiveReply(ref Message reply, object correlationState)
        {
            // TODO: log the reply.

            // If you returned something in BeforeSendRequest
            // it will be available in the correlationState parameter.
        }
    }
}
