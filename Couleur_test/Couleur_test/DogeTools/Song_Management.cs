using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameStateManagement;
using System.IO.IsolatedStorage;
using Microsoft.Xna.Framework.Media;

namespace Couleur_test.DogeTools
{
    class Song_Management
    {
        Song intro, ingame1, ingame2;
		string music_int = (string)IsolatedStorageSettings.ApplicationSettings["music"];
		bool music;
        ScreenManager _screen;
        int currentSong = 0;
        List<Song> bgMusicList;

        public Song_Management(ScreenManager screen)
        {
            _screen = screen;
            Initialization();
        }

        private void Initialization()
        {
			if (music_int == "1") {
				music = true;
			} else {
				music = false;
			}

            intro = _screen.Game.Content.Load<Song>("Music/intro");
            ingame1 = _screen.Game.Content.Load<Song>("Music/ingame1");
            ingame2 = _screen.Game.Content.Load<Song>("Music/ingame2");

            bgMusicList = new List<Song>();
            bgMusicList.Add(ingame1);
            bgMusicList.Add(ingame2);

            if (music)
            {
                MediaPlayer.Volume = 1;
            }
            else
            {
                MediaPlayer.Volume = 0;
            }
        }

        public void Change_Intro()
        {
            MediaPlayer.Stop();
            MediaPlayer.Play(intro);
            MediaPlayer.IsRepeating = true;
        }

        public void Change_Ingame()
        {
            MediaPlayer.IsRepeating = false;
            MediaPlayer.Stop();
            MediaPlayer.Play(ingame1);
        }

        public void Change_Mute()
        {
            if (music)
            {
                music = false;
				IsolatedStorageSettings.ApplicationSettings["music"] = "0";
                MediaPlayer.Volume = 0;
            }
            else
            {
                music = true;
				IsolatedStorageSettings.ApplicationSettings["music"] = "1";
                MediaPlayer.Volume = 1;
            }
        }

        public void ToPause()
        {
            MediaPlayer.Pause();
        }

        public void ToResume()
        {
            MediaPlayer.Resume();
        }

        public void UpdateSong()
        {
            if (MediaPlayer.State != MediaState.Playing)
            {
                currentSong++;
                if (currentSong > bgMusicList.Count - 1)
                {
                    currentSong = 0;
                }
                MediaPlayer.Play(bgMusicList[currentSong]);
            }
        }
    }
}
