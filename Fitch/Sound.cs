using System;
using OpenTK.Audio.OpenAL;

namespace Fitch
{

    public class Sound
    {

        public static void Init()
        {

            try
            {
                var device = Alc.OpenDevice(null);
                int[] attrList = null;
                var context = Alc.CreateContext(device, attrList);
                Alc.MakeContextCurrent(context);
            }
            catch (Exception ex)
            {

                Console.WriteLine("Error opening device.");
                Game.logStrings.Add(DateTime.Now.ToLongTimeString() + ": Error opening audio device.\n" + ex.GetType() + " " + ex.Message);

            }

        }

    }

    public struct SoundSource
    {

        private int id;

        public int ID { get { return id; } set { id = value; } }

        public SoundSource(int id)
        {

            this.id = id;

        }
        
    }

}
