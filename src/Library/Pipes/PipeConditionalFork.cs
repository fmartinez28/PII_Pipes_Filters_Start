using System.Collections.Generic;
using System.Linq;
using System.Text;
using CompAndDel;
using CompAndDel.Filters;


namespace CompAndDel.Pipes{
    public class PipeConditionalFork : IPipe
    {
        protected FilterConditional filtro = new FilterConditional();           //Como sólo se utiliza para este filtro en particular
        protected IPipe nextPipeTrue;
        protected IPipe nextPipeFalse;

        protected bool eval;
        
        /// <summary>
        /// La cañería recibe una imagen, le aplica un filtro con respuesta bool y la envía a una secuencia bifurcada
        /// </summary>
        /// <param name="nextPipeIfTrue">Siguiente cañería si el filtro retorna true</param>
        /// <param name="nextPipeElse">Siguiente cañería si el filtro retorna false</param>
        public PipeConditionalFork(IPipe nextPipeIfTrue, IPipe nextPipeElse)
        {
            this.nextPipeTrue = nextPipeIfTrue;
            this.nextPipeFalse = nextPipeElse;
        }
        /// <summary>
        /// Devuelve el proximo IPipe
        /// </summary>
        public IPipe Next
        {
            get 
            {
                return (eval ? nextPipeTrue : nextPipeFalse);
            }
        }
        /// <summary>
        /// Devuelve el IFilter que aplica este pipe
        /// </summary>
        public FilterConditional Filter
        {
            get { return this.filtro; }
        }
        /// <summary>
        /// Recibe una dirección en el disco correspondiente a una imagen,
        /// la evalúa en un filtro y retorna un valor lógico
        /// </summary>
        /// <param name="path">Ubicación local de la imagen</param>
        public void Evaluate(string path)
        {
            this.eval = this.filtro.Filter(path);
        }

        public IPicture Send(IPicture picture){
            return this.Next.Send(picture);
        }
    }
}