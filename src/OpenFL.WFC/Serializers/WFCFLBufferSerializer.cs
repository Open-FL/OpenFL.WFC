using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

using OpenFL.Core.Buffers.BufferCreators.BuiltIn.FromFile;
using OpenFL.Core.ElementModifiers;
using OpenFL.Serialization.Serializers.Internal;
using OpenFL.WFC.BufferCreators;

using Utility.Serialization;

namespace OpenFL.WFC.Serializers
{
    public class WFCFLBufferSerializer : FLBaseSerializer
    {

        public override object Deserialize(PrimitiveValueWrapper s)
        {
            string name = ResolveId(s.ReadInt());
            FLBufferModifiers bmod = new FLBufferModifiers(name, s.ReadArray<string>());
            bool force = s.ReadBool();
            int n = s.ReadInt();
            int width = s.ReadInt();
            int height = s.ReadInt();
            int symmetry = s.ReadInt();
            int ground = s.ReadInt();
            int limit = s.ReadInt();
            bool pIn = s.ReadBool();
            bool pOut = s.ReadBool();


            MemoryStream ms = new MemoryStream(s.ReadBytes());

            Bitmap bmp = (Bitmap) Image.FromStream(ms);


            WFCParameterObject obj = new WFCParameterObject(
                                                            new SerializableFromBitmapFLBuffer(
                                                                                               "wfc-bin",
                                                                                               bmp,
                                                                                               bmod,
                                                                                               bmod.IsArray
                                                                                                   ? s.ReadInt()
                                                                                                   : 0
                                                                                              ),
                                                            n,
                                                            width,
                                                            height,
                                                            symmetry,
                                                            ground,
                                                            limit,
                                                            pIn,
                                                            pOut,
                                                            force
                                                           );
            return new SerializableWaveFunctionCollapseFLBuffer(name, obj, bmod);
        }


        public override void Serialize(PrimitiveValueWrapper s, object input)
        {
            SerializableWaveFunctionCollapseFLBuffer obj = (SerializableWaveFunctionCollapseFLBuffer) input;
            s.Write(ResolveName(obj.Name));
            s.WriteArray(obj.Modifiers.GetModifiers().ToArray());
            s.Write(obj.Parameter.Force);
            s.Write(obj.Parameter.N);
            s.Write(obj.Parameter.Width);
            s.Write(obj.Parameter.Height);
            s.Write(obj.Parameter.Symmetry);
            s.Write(obj.Parameter.Ground);
            s.Write(obj.Parameter.Limit);
            s.Write(obj.Parameter.PeriodicInput);
            s.Write(obj.Parameter.PeriodicOutput);

            MemoryStream ms = new MemoryStream();

            Bitmap bmp = obj.Parameter.SourceImage.Bitmap;

            bmp.Save(ms, ImageFormat.Png);

            s.Write(ms.GetBuffer(), (int) ms.Position);
            if (obj.IsArray)
            {
                s.Write(obj.Parameter.SourceImage.Size);
            }

            bmp.Dispose();
        }

    }
}