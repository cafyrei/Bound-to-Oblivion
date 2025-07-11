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
        public static SoundEffect _pauseMenuClicked;
        public static SoundEffect _gameOverSFX;
        public static SoundEffect _runGrassSFX;
        public static SoundEffect _gatesOpenedrSFX;
        public static SoundEffect _jumpLandSFX;
        public static SoundEffect _teleportingSFX;

        // Music
        private static Song _menuBackgroundsfx;
        private static Song _menuGamestagesfx;

        public static void Load(ContentManager content)
        {
            // Load SFX
            _attackSound1 = content.Load<SoundEffect>("Sound/sword_slash1");
            _menuHover = content.Load<SoundEffect>("Sound/menu_hover");
            _menuClicked = content.Load<SoundEffect>("Sound/menu_start");
            _pauseMenuClicked = content.Load<SoundEffect>("Sound/kotohit");
            _gameOverSFX = content.Load<SoundEffect>("Sound/game_over");

            _jumpLandSFX = content.Load<SoundEffect>("Sound/jump_land");
            _runGrassSFX = content.Load<SoundEffect>("Sound/run_grass");
            _gatesOpenedrSFX = content.Load<SoundEffect>("Sound/gates_opened"); 
            _teleportingSFX = content.Load<SoundEffect>("Sound/tp"); 


            // Load BGM
            _menuBackgroundsfx = content.Load<Song>("Music/missing_wind");
            _menuGamestagesfx = content.Load<Song>("Music/main_gameSound");
            MediaPlayer.IsRepeating = true;
            MediaPlayer.Volume = 0.4f;
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

        public static void PlayGameStageBGM()
        {
            if (MediaPlayer.Queue.ActiveSong != _menuGamestagesfx)
            {
                MediaPlayer.Play(_menuGamestagesfx);
            }
        }

        public static void PlaySFX(SoundEffect sfx, float volume = 1f)
        {
            if (sfx == null) return;
            var instance = sfx.CreateInstance();
            instance.Volume = MathHelper.Clamp(volume, 0f, 1f);
            instance.Play();
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
