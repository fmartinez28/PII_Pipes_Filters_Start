using TwitterUCU;
using System.Drawing;

namespace CompAndDel.Filters;

public class TwitterFilter{

    TwitterImage sender = new();
    public IPicture Filter(string path, IPicture picture)
    {
        sender.PublishToTwitter("Enviado por el filtro", path);
        return picture;
    }
}