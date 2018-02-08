using System;

namespace ProtoBox.Console.Commands
{
    public class Param
    {
        public string Name { get; private set; }
        public bool isFloat { get; private set; }
        public bool isBool { get; private set; }
        public bool isString { get; private set; }
        public bool isEnum { get; private set; }
        public bool isArray { get; private set; }

        public Type type;
        public Type elementType;
        public object value;

        public string FormattedString()
        {
            // return formatted parameter <Int32:NameOfParam>
            if (isArray)
                return "<" + elementType.Name + ":" + Name + ">";
            return "<" + type.Name + ":" + Name + ">";
        }

        public Param(Type type, string name, object value = null)
        {
            this.Name = name;
            this.value = value; 
            this.type = type;

            isFloat = (this.type == typeof(float));
            isString = (this.type == typeof(string));
            isBool = (this.type == typeof(bool));
            isEnum = (type.IsEnum);
            isArray = (type.IsArray);


            if (isArray)
                elementType = type.GetElementType();

        }
    }
}
