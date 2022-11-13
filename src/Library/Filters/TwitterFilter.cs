using TwitterUCU;
using System.Drawing;

namespace CompAndDel.Filters;

public class TwitterFilter : IFilter{

    TwitterImage sender = new();
    public IPicture Filter(IPicture picture)
    {
        sender.PublishToTwitter("Enviado por el filtro", picture.Path());
        return picture;
    }
}