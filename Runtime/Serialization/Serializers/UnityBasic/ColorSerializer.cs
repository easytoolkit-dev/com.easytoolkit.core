using UnityEngine;

namespace EasyToolKit.Core.Serialization
{
    [SerializerConfiguration(SerializerPriorityLevel.UnityBasic)]
    public class ColorSerializer : EasySerializer<Color>
    {
        private static readonly EasySerializer<float> FloatSerializer = GetSerializer<float>();

        public override void Process(string name, ref Color value, IArchive archive)
        {
            FloatSerializer.Process("R", ref value.r, archive);
            FloatSerializer.Process("G", ref value.g, archive);
            FloatSerializer.Process("B", ref value.b, archive);
            FloatSerializer.Process("A", ref value.a, archive);
        }
    }
}
