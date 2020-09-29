using System;
using System.Reflection;

using OpenFL.Core.Buffers.BufferCreators;
using OpenFL.Serialization.Serializers.Internal;
using OpenFL.WFC.BufferCreators;
using OpenFL.WFC.Serializers;

using PluginSystem.Core.Pointer;
using PluginSystem.Utility;

namespace OpenFL.WFC
{
    public class WFCBufferCreatorPlugin : APlugin<BufferCreator>
    {

        public override void OnLoad(PluginAssemblyPointer ptr)
        {
            base.OnLoad(ptr);

            Type[] ts = Assembly.GetExecutingAssembly().GetExportedTypes();

            Type target = typeof(ASerializableBufferCreator);

            for (int i = 0; i < ts.Length; i++)
            {
                if (target != ts[i] && target.IsAssignableFrom(ts[i]))
                {
                    ASerializableBufferCreator bc = (ASerializableBufferCreator) Activator.CreateInstance(ts[i]);
                    PluginHost.AddBufferCreator(bc);
                }
            }
        }

    }

    public class CLSerializers : APlugin<SerializableFLProgramSerializer>
    {

        public override void OnLoad(PluginAssemblyPointer ptr)
        {
            base.OnLoad(ptr);
            WFCFLBufferSerializer wfcbuf = new WFCFLBufferSerializer();
            PluginHost.BufferSerializer.AddSerializer(typeof(SerializableWaveFunctionCollapseFLBuffer), wfcbuf);
        }

    }
}