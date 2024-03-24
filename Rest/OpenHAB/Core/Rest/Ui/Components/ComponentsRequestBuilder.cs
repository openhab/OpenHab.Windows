// <auto-generated/>
using Microsoft.Kiota.Abstractions;
using OpenHAB.Core.Rest.Ui.Components.Item;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System;
namespace OpenHAB.Core.Rest.Ui.Components {
    /// <summary>
    /// Builds and executes requests for operations under \ui\components
    /// </summary>
    public class ComponentsRequestBuilder : BaseRequestBuilder {
        /// <summary>Gets an item from the openHAB.Core.Rest.ui.components.item collection</summary>
        /// <param name="position">Unique identifier of the item</param>
        /// <returns>A <see cref="WithNamespaceItemRequestBuilder"/></returns>
        public WithNamespaceItemRequestBuilder this[string position] { get {
            var urlTplParams = new Dictionary<string, object>(PathParameters);
            urlTplParams.Add("namespace", position);
            return new WithNamespaceItemRequestBuilder(urlTplParams, RequestAdapter);
        } }
        /// <summary>
        /// Instantiates a new <see cref="ComponentsRequestBuilder"/> and sets the default values.
        /// </summary>
        /// <param name="pathParameters">Path parameters for the request</param>
        /// <param name="requestAdapter">The request adapter to use to execute the requests.</param>
        public ComponentsRequestBuilder(Dictionary<string, object> pathParameters, IRequestAdapter requestAdapter) : base(requestAdapter, "{+baseurl}/ui/components", pathParameters) {
        }
        /// <summary>
        /// Instantiates a new <see cref="ComponentsRequestBuilder"/> and sets the default values.
        /// </summary>
        /// <param name="rawUrl">The raw URL to use for the request builder.</param>
        /// <param name="requestAdapter">The request adapter to use to execute the requests.</param>
        public ComponentsRequestBuilder(string rawUrl, IRequestAdapter requestAdapter) : base(requestAdapter, "{+baseurl}/ui/components", rawUrl) {
        }
    }
}
