using System.Collections.Generic;
using UnityEngine;
using System;
using Param = ProtoBox.Console.Commands.Param;

namespace ProtoBox.Console.ParameterHandling
{
    public delegate void ParamInput(Type type, object value);
    public class ParamHandler
    {
        public event ParamInput OnParamHandled;
        private const string ERR_CONVERT_DOES_NOT_EXIST = "Conversion from {0} to {1} doesn't exist in ParamHandler!";

        /// <summary>
        /// list of current available converters
        /// </summary>
        List<ParamConverter> converters = new List<ParamConverter>
        {
            new IntArrayConverter(),
            new FloatArrayConverter(),
            new ResolutionArrayConverter(),
            new BooleanConverter()
        };

        ParamConverter EnumConverter = new Enum();

        /// <summary>
        /// Displays parameter as input field in game
        /// </summary>
        /// <param name="param"></param>
        public void DisplayParam(Param param)
        {
            if ((param.isArray && param.elementType != typeof(string)) || param.isBool)
            {
                string[] choices;
                if (!TryConvert(param, out choices))
                    throw new Exception(
                        string.Format(ERR_CONVERT_DOES_NOT_EXIST, param.type.Name, typeof(string[]).Name));

                // Display after parsing
                if (OnParamHandled != null)
                {
                    OnParamHandled.Invoke(typeof(string[]), choices);
                }

                return;
            }

            if (param.isEnum)
            {
                if (OnParamHandled != null)
                {
                    OnParamHandled.Invoke(typeof(string[]), EnumConverter.Convert(param));
                }
            }
        }

        /// <summary>
        /// Tries to convert a parameter array type to string[]
        /// </summary>
        /// <param name="param">parameter to convert</param>
        /// <param name="converted">out converted string[]</param>
        /// <returns>success</returns>
        private bool TryConvert(Param param, out string[] converted)
        {
            converted = null;
            for (int i = 0; i < converters.Count; i++)
            {
                if (converters[i].type.IsArray)
                {
                    if (converters[i].type.GetElementType() == param.elementType)
                    {
                        converted = converters[i].Convert(param);
                        return true;
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// Abstract version of typeconverter
        /// </summary>
        public abstract class ParamConverter
        {
            public virtual Type type { get; protected set; }
            public virtual string[] Convert(Param param)
            {
                return null;
            }
        }

        /// <summary>
        /// converts int[] param to string[]
        /// </summary>
        public class IntArrayConverter : ParamConverter
        {
            public IntArrayConverter() { type = typeof(int[]); }
            public override string[] Convert(Param param)
            {
                int[] values = (int[])param.value;
                string[] svalues = new string[values.Length];
                for (int i = 0; i < values.Length; i++)
                {
                    svalues[i] = values[i].ToString();
                }
                return svalues;
            }
        }

        /// <summary>
        /// converts float[] param to string[]
        /// </summary>
        public class FloatArrayConverter : ParamConverter
        {
            public FloatArrayConverter() { type = typeof(float[]); }
            public override string[] Convert(Param param)
            {
                float[] values = (float[])param.value;
                string[] svalues = new string[values.Length];
                for (int i = 0; i < values.Length; i++)
                {
                    svalues[i] = values[i].ToString();
                }
                return svalues;
            }
        }

        /// <summary>
        /// converts float[] param to string[]
        /// </summary>
        public class BooleanConverter : ParamConverter
        {
            public BooleanConverter() { type = typeof(bool); }
            public override string[] Convert(Param param)
            { return new string[] { "true", "false" }; }
        }

        /// <summary>
        /// converts float[] param to string[]
        /// </summary>
        public class ResolutionArrayConverter : ParamConverter
        {
            public ResolutionArrayConverter() { type = typeof(Resolution[]); }
            public override string[] Convert(Param param)
            {
                Resolution[] values = (Resolution[])param.value;
                string[] svalues = new string[values.Length];
                for (int i = 0; i < values.Length; i++)
                {
                    svalues[i] = values[i].width + "x" + values[i].height;
                }
                return svalues;
            }
        }

        /// <summary>
        /// converts enum param to string[]
        /// </summary>
        public class Enum : ParamConverter
        {
            public override string[] Convert(Param param)
            {
                return System.Enum.GetNames(param.type);
            }
        }
    }
} 
