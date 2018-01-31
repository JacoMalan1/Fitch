using System.Media;
using System.IO;

namespace Fitch
{
    public enum SoundEffect
    {

        LevelMusic,
        Jump

    }

    class Sound
    {

        public static void Play(SoundEffect effect)
        {

            string fileName = "";

            switch (effect)
            {
                case SoundEffect.LevelMusic:
                    fileName = "levelMusic.wav";
                    break;

                default:
                    break;
            }

            fileName = "Content/" + fileName;

            if (!File.Exists(fileName))
                return;

            SoundPlayer s = new SoundPlayer(fileName);
            s.PlayLooping();

        }

    }
}
