using Robust.Shared.Prototypes;
using Robust.Shared.Serialization.TypeSerializers.Implementations.Custom.Prototype;

namespace Content.Shared._ALTSpace.Roadmap
{
    /// <summary>
    /// Прототип для отображения планов разработки в меню лобби.
    /// </summary>
    [Prototype("roadmap")]
    public sealed partial class RoadmapPrototype : IPrototype
    {
        [IdDataField]
        public string ID { get; private set; } = default!;

        [DataField("title")]
        public string Title { get; private set; } = string.Empty;

        [DataField("description")]
        public string Description { get; private set; } = string.Empty;

        /// <summary>
        /// Прогресс от 0 до 100
        /// </summary>
        [DataField("progress")]
        public float Progress { get; private set; } = 0;

        [DataField("category")]
        public string Category { get; private set; } = "General";
    }
}