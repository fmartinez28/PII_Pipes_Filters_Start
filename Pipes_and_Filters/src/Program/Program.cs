using System;
using CompAndDel.Pipes;
using CompAndDel.Filters;
using System.Collections.Generic;
using TwitterUCU;

namespace CompAndDel
{
    class Program
    {
        static void Main(string[] args)
        {
            string outputFolder = @"..\Saved\";
            
            Ejercicio1(outputFolder);
            Ejercicio2(outputFolder);
            Ejercicio3(outputFolder);
            Ejercicio4(outputFolder);
        }

        public static void Ejercicio1(string path){
            PictureProvider provider = new();
            IPicture loadedPic = provider.GetPicture(@".\beer.jpg");
            PictureProvider p = new();
            IPicture outputImg = LoadPipesAndFilters_Ejercicio1(loadedPic);
            provider.SavePicture(outputImg, $"{path}savedBeer.jpg");    
        }
        public static void Ejercicio2(string path){
            PictureProvider provider = new();
            IPicture loadedPic = provider.GetPicture(@".\luke.jpg");
            List<IPicture> outputImgs = LoadPipesAndFilters_Ejercicio2(loadedPic);
            int sequenceCounter = 1;
            foreach(IPicture pic in outputImgs)                 //Itero y actúo sobre cada transformación de la secuencia
            {
                provider.SavePicture(pic, $"{path}savedLuke{sequenceCounter}.jpg");
                sequenceCounter++;    
            }
        }
        public static void Ejercicio3(string path){
            TwitterImage publisher = new();
            PictureProvider provider = new();
            IPicture loadedPic = provider.GetPicture(@".\luke.jpg");
            List<IPicture> outputImgs = LoadPipesAndFilters_Ejercicio2(loadedPic);
            int sequenceCounter = 1;
            foreach (IPicture pic in outputImgs)                 //Itero y actúo sobre cada transformación de la secuencia
            {
                string currentPath = $"{path}savedLuke{sequenceCounter}.jpg";
                provider.SavePicture(pic, currentPath);
                publisher.PublishToTwitter("Depende", currentPath);
                sequenceCounter++;    
            }
        }
        public static void Ejercicio4(string path){
            TwitterImage publisher = new();
            PictureProvider provider = new();
            string imgPath = @".\luke.jpg";
            IPicture loadedPic = provider.GetPicture(imgPath);
            List<IPicture> outputImgs = LoadPipesAndFilters_Ejercicio4(loadedPic);
            int sequenceCounter = 1;
            foreach (IPicture pic in outputImgs)                 //Itero y actúo sobre cada transformación de la secuencia
            {
                string currentPath = $"{path}savedLuke{sequenceCounter}.jpg";
                provider.SavePicture(pic, currentPath);
                publisher.PublishToTwitter("Depende", currentPath);
                sequenceCounter++;    
            }
        }
        public static IPicture LoadPipesAndFilters_Ejercicio1(IPicture picture)
        {
        
        // Cargo Pipes & Filters

        PipeNull nullPipe = new();
        FilterNegative negativeFilter = new();
        PipeSerial secondSerialPipe = new(negativeFilter, nullPipe);
        FilterGreyscale greyscaleFilter = new();
        PipeSerial firstSerialPipe = new(greyscaleFilter, secondSerialPipe);

        // Proceso

        IPicture result = nullPipe.Send(picture);
        result = secondSerialPipe.Send(picture);
        result = firstSerialPipe.Send(picture);

        return result;
        }
        public static List<IPicture> LoadPipesAndFilters_Ejercicio2(IPicture picture)
        {
        
        List<IPicture> imgSequence = new();     //Secuencia de transformaciones de picture
        // Cargo Pipes & Filters

        PipeNull nullPipe = new();
        FilterNegative negativeFilter = new();
        PipeSerial secondSerialPipe = new(negativeFilter, nullPipe);
        FilterGreyscale greyscaleFilter = new();
        PipeSerial firstSerialPipe = new(greyscaleFilter, secondSerialPipe);

        // Proceso
        IPicture result = nullPipe.Send(picture);
        imgSequence.Add(result);
        result = secondSerialPipe.Send(picture);
        imgSequence.Add(result);
        result = firstSerialPipe.Send(picture);
        imgSequence.Add(result);


        return imgSequence;
        }
        public static List<IPicture> LoadPipesAndFilters_Ejercicio4(IPicture picture)
        {
        
        List<IPicture> imgSequence = new();     //Secuencia de transformaciones de picture
        
        // Cargo Pipes & Filters

            //If true
        
        PipeNull finalTrue = new();
        TwitterFilter publish = new();          //FIXME: Crear un filtro si es necesario///FIXED
        PipeSerial serialTrue = new(publish, finalTrue);

            //If false
        
        PipeNull finalFalse = new();
        FilterNegative negativeFalse = new();
        PipeSerial serialFalse = new(negativeFalse, finalFalse);


        PipeConditionalFork conditionalPipe = new(serialTrue, serialFalse); //TODO: Resolver posibilidad de usar más
        //filtros condicionales, que no esté tan hardcodeado.

        FilterGreyscale greyFilter = new();
        PipeSerial start = new(greyFilter, conditionalPipe);

        // Proceso
        
        conditionalPipe.Evaluate(picture);

        IPicture result;

        if (conditionalPipe.Evaluation){                    //Se podría mejorar
            result = finalTrue.Send(picture);
            imgSequence.Add(result);
            result = serialTrue.Send(picture);
            imgSequence.Add(result);
        } else {
            result = finalFalse.Send(picture);
            imgSequence.Add(result);
            result = serialFalse.Send(picture);
            imgSequence.Add(result);
        }

        result = conditionalPipe.Send(picture);
        imgSequence.Add(result);
        result = start.Send(picture);
        imgSequence.Add(result);

        return imgSequence;
        }
    }
}
