// <auto-generated/>
using Microsoft.Kiota.Abstractions.Serialization;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System;
namespace OpenHAB.Core.Rest.Models {
    public class ConfigDescriptionDTO : IAdditionalDataHolder, IParsable {
        /// <summary>Stores additional data not described in the OpenAPI description found when deserializing. Can be used for serialization as well.</summary>
        public IDictionary<string, object> AdditionalData { get; set; }
        /// <summary>The parameterGroups property</summary>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public List<ConfigDescriptionParameterGroupDTO>? ParameterGroups { get; set; }
#nullable restore
#else
        public List<ConfigDescriptionParameterGroupDTO> ParameterGroups { get; set; }
#endif
        /// <summary>The parameters property</summary>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public List<ConfigDescriptionParameterDTO>? Parameters { get; set; }
#nullable restore
#else
        public List<ConfigDescriptionParameterDTO> Parameters { get; set; }
#endif
        /// <summary>The uri property</summary>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public string? Uri { get; set; }
#nullable restore
#else
        public string Uri { get; set; }
#endif
        /// <summary>
        /// Instantiates a new <see cref="ConfigDescriptionDTO"/> and sets the default values.
        /// </summary>
        public ConfigDescriptionDTO() {
            AdditionalData = new Dictionary<string, object>();
        }
        /// <summary>
        /// Creates a new instance of the appropriate class based on discriminator value
        /// </summary>
        /// <returns>A <see cref="ConfigDescriptionDTO"/></returns>
        /// <param name="parseNode">The parse node to use to read the discriminator value and create the object</param>
        public static ConfigDescriptionDTO CreateFromDiscriminatorValue(IParseNode parseNode) {
            _ = parseNode ?? throw new ArgumentNullException(nameof(parseNode));
            return new ConfigDescriptionDTO();
        }
        /// <summary>
        /// The deserialization information for the current model
        /// </summary>
        /// <returns>A IDictionary&lt;string, Action&lt;IParseNode&gt;&gt;</returns>
        public virtual IDictionary<string, Action<IParseNode>> GetFieldDeserializers() {
            return new Dictionary<string, Action<IParseNode>> {
                {"parameterGroups", n => { ParameterGroups = n.GetCollectionOfObjectValues<ConfigDescriptionParameterGroupDTO>(ConfigDescriptionParameterGroupDTO.CreateFromDiscriminatorValue)?.ToList(); } },
                {"parameters", n => { Parameters = n.GetCollectionOfObjectValues<ConfigDescriptionParameterDTO>(ConfigDescriptionParameterDTO.CreateFromDiscriminatorValue)?.ToList(); } },
                {"uri", n => { Uri = n.GetStringValue(); } },
            };
        }
        /// <summary>
        /// Serializes information the current object
        /// </summary>
        /// <param name="writer">Serialization writer to use to serialize this model</param>
        public virtual void Serialize(ISerializationWriter writer) {
            _ = writer ?? throw new ArgumentNullException(nameof(writer));
            writer.WriteCollectionOfObjectValues<ConfigDescriptionParameterGroupDTO>("parameterGroups", ParameterGroups);
            writer.WriteCollectionOfObjectValues<ConfigDescriptionParameterDTO>("parameters", Parameters);
            writer.WriteStringValue("uri", Uri);
            writer.WriteAdditionalData(AdditionalData);
        }
    }
}
