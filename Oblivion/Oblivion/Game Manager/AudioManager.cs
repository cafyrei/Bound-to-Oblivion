using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Media;

namespace Oblivion
{
    public static class AudioManager
    {
        // Sound Effects
        private static SoundEffect _attackSound1;
        public static SoundEffect _menuHover;
        public static SoundEffect _menuClicked;

        // Music
        private static Song _menuBackgroundsfx;

        public static void Load(ContentManager content)
        {
            // Load SFX
            _attackSound1 = content.Load<SoundEffect>("Sound/sword_slash1");
            _menuHover = content.Load<SoundEffect>("Sound/menu_hover");
            _menuClicked = content.Load<SoundEffect>("Sound/menu_start");

            // Load BGM
            _menuBackgroundsfx = content.Load<Song>("Music/missing_wind");
            MediaPlayer.IsRepeating = true;
            MediaPlayer.Volume = 0.5f;
        }

        // Public playback methods
        public static void PlayAttack()
        {
            _attackSound1?.Play();
        }

        public static void PlayMenuBGM()
        {
            if (MediaPlayer.State != MediaState.Playing)
                MediaPlayer.Play(_menuBackgroundsfx);
        }

        public static void StopMusic()
        {
            MediaPlayer.Stop();
        }

        public static void SetVolume(float volume)
        {
            MediaPlayer.Volume = MathHelper.Clamp(volume, 0f, 1f);
        }
    }
}
