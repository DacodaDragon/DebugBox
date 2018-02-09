using System;
using ProtoBox.Console.Commands;
public abstract class ParamTypeConverter
{
    public virtual Type compatibleType { get; protected set; }
    public virtual Param param { get; protected set; }

    protected void Convert(Param param)
    {

    }
}

public class IntArray : ParamTypeConverter
{
    public override Type compatibleType { get {return typeof(int[]); } }
}
