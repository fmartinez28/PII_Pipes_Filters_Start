using CognitiveCoreUCU;
using System.Drawing;

namespace CompAndDel.Filters;

public class FilterConditional{
    public bool Filter(IPicture picture)
    {
        CognitiveFace face = new CognitiveFace(false);          //No hace falta marcar caras
        face.Recognize(picture.Path());
        return face.FaceFound;
    }
}