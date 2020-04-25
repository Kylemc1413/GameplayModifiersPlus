using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
namespace GamePlayModifiersPlus.Utilities
{
    public static class GameObjects
    {
        public static StandardLevelGameplayManager pauseManager;
        public static NoteCutSoundEffectManager soundEffectManager;
        public static BeatmapObjectManager beatmapObjectManager;
        public static BeatmapObjectSpawnController spawnController;
        public static GameEnergyCounter energyCounter;
        public static GameEnergyUIPanel _energyPanel;
        public static GameEnergyUIPanel energyPanel
        {
            get
            {
                if (_energyPanel == null)
                    _energyPanel = Resources.FindObjectsOfTypeAll<GameEnergyUIPanel>().First();
                return _energyPanel;
            }
            set
            {
                _energyPanel = value;
            }
        }
        public static ColorManager ColorManager;
        public static AudioTimeSyncController AudioTimeSync { get; private set; }
        public static AudioManagerSO Mixer { get; private set; }
        public static AudioSource songAudio;


        public static void Load()
        {
            soundEffectManager = Resources.FindObjectsOfTypeAll<NoteCutSoundEffectManager>().FirstOrDefault();
            beatmapObjectManager = Resources.FindObjectsOfTypeAll<BeatmapObjectManager>().FirstOrDefault();
            spawnController = Resources.FindObjectsOfTypeAll<BeatmapObjectSpawnController>().FirstOrDefault();
            energyCounter = Resources.FindObjectsOfTypeAll<GameEnergyCounter>().First();
            ColorManager = Resources.FindObjectsOfTypeAll<ColorManager>().Last();
            pauseManager = Resources.FindObjectsOfTypeAll<StandardLevelGameplayManager>().FirstOrDefault();
            AudioTimeSync = Resources.FindObjectsOfTypeAll<AudioTimeSyncController>().FirstOrDefault();
            if (AudioTimeSync != null)
            {
                songAudio = AudioTimeSync.GetField<AudioSource>("_audioSource");
                if (songAudio == null)
                    Plugin.Log("Audio null");
            }
            var gameCoreSceneSetup = Resources.FindObjectsOfTypeAll<GameplayCoreSceneSetup>().FirstOrDefault();
            Mixer = gameCoreSceneSetup.GetPrivateField<AudioManagerSO>("_audioMixer");

        }


    }
}
