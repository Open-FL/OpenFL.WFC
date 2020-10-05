using System.Drawing;

using OpenFL.Core.Buffers;
using OpenFL.Core.DataObjects.SerializableDataObjects;
using OpenFL.Core.ElementModifiers;

namespace OpenFL.WFC.BufferCreators
{
    public class SerializableWaveFunctionCollapseFLBuffer : SerializableFLBuffer
    {

        public readonly int Size;

        public SerializableWaveFunctionCollapseFLBuffer(
            string name, WFCParameterObject parameter,
            FLBufferModifiers modifiers) : base(name, modifiers)
        {
            Parameter = parameter;
        }

        public WFCParameterObject Parameter { get; }

        public override FLBuffer GetBuffer()
        {
            if (IsArray)
            {
                return new LazyLoadingFLBuffer(
                                               root =>
                                               {
                                                   WFCOverlayMode wfc = new WFCOverlayMode(
                                                        Parameter.SourceImage.Bitmap,
                                                        Parameter.N,
                                                        Parameter.Width,
                                                        Parameter.Height,
                                                        Parameter.PeriodicInput,
                                                        Parameter.PeriodicOutput,
                                                        Parameter.Symmetry,
                                                        Parameter.Ground
                                                       );
                                                   if (Parameter.Force)
                                                   {
                                                       do
                                                       {
                                                           wfc.Run(Parameter.Limit);
                                                       } while (!wfc.Success);
                                                   }
                                                   else
                                                   {
                                                       wfc.Run(Parameter.Limit);
                                                   }


                                                   Bitmap bmp = wfc.Graphics();
                                                   return new FLBuffer(root.Instance, bmp, "WFCBuffer." + Name);
                                               },
                                               Modifiers.InitializeOnStart
                                              );
            }

            LazyLoadingFLBuffer info = new LazyLoadingFLBuffer(
                                                               root =>
                                                               {
                                                                   Bitmap bmp;
                                                                   WFCOverlayMode wfc = new WFCOverlayMode(
                                                                        Parameter
                                                                            .SourceImage
                                                                            .Bitmap,
                                                                        Parameter.N,
                                                                        Parameter
                                                                            .Width,
                                                                        Parameter
                                                                            .Height,
                                                                        Parameter
                                                                            .PeriodicInput,
                                                                        Parameter
                                                                            .PeriodicOutput,
                                                                        Parameter
                                                                            .Symmetry,
                                                                        Parameter
                                                                            .Ground
                                                                       );
                                                                   if (Parameter.Force)
                                                                   {
                                                                       do
                                                                       {
                                                                           wfc.Run(Parameter.Limit);
                                                                           bmp = new Bitmap(
                                                                                wfc.Graphics(),
                                                                                new Size(
                                                                                     root.Dimensions.x,
                                                                                     root.Dimensions.y
                                                                                    )
                                                                               ); //Apply scaling
                                                                       } while (!wfc.Success);
                                                                   }
                                                                   else
                                                                   {
                                                                       wfc.Run(Parameter.Limit);
                                                                       bmp = new Bitmap(
                                                                            wfc.Graphics(),
                                                                            new Size(
                                                                                 root.Dimensions.x,
                                                                                 root.Dimensions.y
                                                                                )
                                                                           ); //Apply scaling
                                                                   }


                                                                   return new FLBuffer(
                                                                        root.Instance,
                                                                        bmp,
                                                                        "WFCBuffer." + Name
                                                                       );
                                                               },
                                                               Modifiers.InitializeOnStart
                                                              );
            return info;
        }

        public override string ToString()
        {
            return base.ToString() + $"wfc{(Parameter.Force ? "f" : "")} {Parameter}";
        }

    }
}