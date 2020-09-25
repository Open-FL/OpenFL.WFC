using OpenCL.Wrapper.Exceptions;

using OpenFL.Core.Buffers.BufferCreators.BuiltIn.FromFile;
using OpenFL.Core.DataObjects.SerializableDataObjects;
using OpenFL.Core.ElementModifiers;
using OpenFL.Core.Exceptions;

using Utility.IO.Callbacks;

namespace OpenFL.WFC.BufferCreators
{
    public class WFCParameterObject
    {

        public WFCParameterObject(
            SerializableFromBitmapFLBuffer input, int n, int width, int height, int symmetry,
            int ground, int limit, bool periodicInput,
            bool periodicOutput, bool force)
        {
            N = n;
            Width = width;
            Height = height;
            Symmetry = symmetry;
            Ground = ground;
            Limit = limit;
            PeriodicInput = periodicInput;
            PeriodicOutput = periodicOutput;
            Force = force;
            SourceImage = input;
        }

        public int N { get; set; }

        public int Width { get; set; }

        public int Height { get; set; }

        public int Symmetry { get; set; }

        public int Ground { get; set; }

        public int Limit { get; set; }

        public bool PeriodicInput { get; set; }

        public bool PeriodicOutput { get; set; }

        public bool Force { get; set; }

        public SerializableFromBitmapFLBuffer SourceImage { get; set; }


        public override string ToString()
        {
            return $"{SourceImage} {N} {Width} {Height} {PeriodicInput} {PeriodicOutput} {Symmetry} {Ground} {Limit}";
        }


        public static SerializableFLBuffer CreateBuffer(
            string name, string[] args, bool force,
            FLBufferModifiers modifiers, int size)
        {
            if (modifiers.IsArray)
            {
                throw new FLInvalidFunctionUseException(
                                                        "wfc",
                                                        "Invalid WFC Define statement. WFC can not be used on arrays"
                                                       );
            }

            if (args.Length < 9)
            {
                throw new FLInvalidFunctionUseException("wfc", "Invalid WFC Define statement");
            }

            if (!int.TryParse(args[1], out int n))
            {
                throw new FLInvalidFunctionUseException("wfc", "Invalid WFC Define statement");
            }

            if (!int.TryParse(args[2], out int widh))
            {
                throw new FLInvalidFunctionUseException("wfc", "Invalid WFC Define statement");
            }

            if (!int.TryParse(args[3], out int heigt))
            {
                throw new FLInvalidFunctionUseException("wfc", "Invalid WFC Define statement");
            }

            if (!bool.TryParse(args[4], out bool periodicInput))
            {
                throw new FLInvalidFunctionUseException("wfc", "Invalid WFC Define statement");
            }

            if (!bool.TryParse(args[5], out bool periodicOutput))
            {
                throw new FLInvalidFunctionUseException("wfc", "Invalid WFC Define statement");
            }

            if (!int.TryParse(args[6], out int symmetry))
            {
                throw new FLInvalidFunctionUseException("wfc", "Invalid WFC Define statement");
            }

            if (!int.TryParse(args[7], out int ground))
            {
                throw new FLInvalidFunctionUseException("wfc", "Invalid WFC Define statement");
            }

            if (!int.TryParse(args[8], out int limit))
            {
                throw new FLInvalidFunctionUseException("wfc", "Invalid WFC Define statement");
            }

            string fn = args[0].Trim().Replace("\"", "");
            if (IOManager.FileExists(fn))
            {
                SerializableFromFileFLBuffer input =
                    new SerializableFromFileFLBuffer("WFCInputBuffer", fn, modifiers, size);
                WFCParameterObject wfcPO = new WFCParameterObject(
                                                                  input,
                                                                  n,
                                                                  widh,
                                                                  heigt,
                                                                  symmetry,
                                                                  ground,
                                                                  limit,
                                                                  periodicInput,
                                                                  periodicOutput,
                                                                  force
                                                                 );
                return new SerializableWaveFunctionCollapseFLBuffer(name, wfcPO, modifiers);
            }

            throw
                new FLInvalidFunctionUseException(
                                                  "wfc",
                                                  "Invalid WFC Image statement",
                                                  new InvalidFilePathException(fn)
                                                 );
        }

    }
}