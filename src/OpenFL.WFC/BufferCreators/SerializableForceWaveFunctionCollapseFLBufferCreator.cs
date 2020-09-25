using OpenFL.Core.Buffers.BufferCreators;
using OpenFL.Core.DataObjects.SerializableDataObjects;
using OpenFL.Core.ElementModifiers;

namespace OpenFL.WFC.BufferCreators
{
    public class SerializableForceWaveFunctionCollapseFLBufferCreator : ASerializableBufferCreator
    {

        public override SerializableFLBuffer CreateBuffer(
            string name, string[] args, FLBufferModifiers modifiers,
            int arraySize)
        {
            return WFCParameterObject.CreateBuffer(name, args, true, modifiers, arraySize);
        }


        public override bool IsCorrectBuffer(string bufferKey)
        {
            return bufferKey == "wfcf";
        }

    }
}