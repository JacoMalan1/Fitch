using System.Media;
using System.IO;
using System;
using OpenTK.Audio.OpenAL;

namespace Fitch
{

    public class Sound
    {

        public static unsafe void Init()
        {

            try
            {
                var device = Alc.OpenDevice(null);
                var context = Alc.CreateContext(device, (int*)null);
                Alc.MakeContextCurrent(context);
            }
            catch
            {

                Console.WriteLine("Error opening device.");

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
