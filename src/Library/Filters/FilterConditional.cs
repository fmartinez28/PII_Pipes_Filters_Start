using CognitiveCoreUCU;
using System.Drawing;

namespace CompAndDel.Filters;

public class FilterConditional{
    public bool Filter(string path)
    {
        CognitiveFace face = new CognitiveFace(false);          //No hace falta marcar caras
        face.Recognize(path);
        return face.FaceFound;
    }
}