﻿using BS_Utils.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using System.Collections;
using IPA.Utilities;
using System.Collections.Generic;
namespace GamePlayModifiersPlus.Utilities
{
    public static class GameObjects
    {
        public static StandardLevelGameplayManager pauseManager;
        public static NoteCutSoundEffectManager soundEffectManager;
        public static BasicBeatmapObjectManager beatmapObjectManager;
        public static BeatmapObjectSpawnController spawnController;
        public static GameEnergyCounter energyCounter;
        public static GameEnergyUIPanel _energyPanel;
        public static GameEnergyUIPanel energyPanel
        {
            get
            {
                if (_energyPanel == null)
                    _energyPanel = Resources.FindObjectsOfTypeAll<GameEnergyUIPanel>().LastOrDefault();
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


        public static IEnumerator FetchObjects()
        {
            yield return new WaitForSeconds(0.1f);
            soundEffectManager = Resources.FindObjectsOfTypeAll<NoteCutSoundEffectManager>().LastOrDefault();
            beatmapObjectManager = Resources.FindObjectsOfTypeAll<BeatmapObjectExecutionRatingsRecorder>().LastOrDefault().GetPrivateField<BeatmapObjectManager>("_beatmapObjectManager") as BasicBeatmapObjectManager;
            spawnController = Resources.FindObjectsOfTypeAll<BeatmapObjectSpawnController>().LastOrDefault();
            energyCounter = Resources.FindObjectsOfTypeAll<GameEnergyCounter>().LastOrDefault();
            ColorManager = Resources.FindObjectsOfTypeAll<NoteCutCoreEffectsSpawner>().LastOrDefault().GetField<ColorManager, NoteCutCoreEffectsSpawner>("_colorManager");
            pauseManager = Resources.FindObjectsOfTypeAll<StandardLevelGameplayManager>().LastOrDefault();
            AudioTimeSync = Resources.FindObjectsOfTypeAll<AudioTimeSyncController>().LastOrDefault();
            if (AudioTimeSync != null)
            {
                songAudio = AudioTimeSync.GetField<AudioSource>("_audioSource");
                if (songAudio == null)
                    Plugin.Log("Audio null");
            }
            Mixer = soundEffectManager.GetField<AudioManagerSO>("_audioManager");
            GameModifiersController.SetupSpawnCallbacks();
        }
        public static void Load()
        {
            SharedCoroutineStarter.instance.StartCoroutine(FetchObjects());
          

        }


    }
}
