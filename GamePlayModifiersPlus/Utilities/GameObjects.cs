namespace GamePlayModifiersPlus.Utilities
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using UnityEngine;
    using System.Collections;
    using IPA.Utilities;
    using System.Collections.Generic;
    using SiraUtil.Zenject;
    using SiraUtil;
    using Zenject;
    public class GameObjects : IDisposable
    {
        public static StandardLevelGameplayManager pauseManager;
        public static BpmController bpmController;
        public static BeatmapCallbacksController callbacksController;
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


        private BasicBeatmapObjectManager _beatmapObjectManager;
        private BeatmapCallbacksController _beatmapCallbacksController;
        private BeatmapObjectSpawnController _beatmapObjectSpawnController;
        private GameEnergyCounter _gameEnergyCounter;
        private ColorManager _colorManager;
        private StandardLevelGameplayManager _standardLevelGameplayManager;
        private AudioTimeSyncController _audioTimeSyncController;
        private NoteCutSoundEffectManager _noteCutSoundEffectManager;
    //    private GameEnergyUIPanel _gameEnergyUIPanel;
        IBpmController _bpmController;

        [Inject]
        public void Construct(BasicBeatmapObjectManager beatmapObjectManager,
            BeatmapCallbacksController beatmapCallbacksController,
            BeatmapObjectSpawnController beatmapObjectSpawnController,
            GameEnergyCounter gameEnergyCounter,
            ColorManager colorManager,
            StandardLevelGameplayManager standardLevelGameplayManager,
            AudioTimeSyncController audioTimeSyncController,
            IBpmController bpmController,
            NoteCutSoundEffectManager noteCutSoundEffectManager)
      //      GameEnergyUIPanel gameEnergyUIPanel)
        {
            _beatmapObjectManager = beatmapObjectManager;
            _beatmapCallbacksController = beatmapCallbacksController;
            _beatmapObjectSpawnController = beatmapObjectSpawnController;
            _gameEnergyCounter = gameEnergyCounter;
            _colorManager = colorManager;
            _standardLevelGameplayManager = standardLevelGameplayManager;
            _audioTimeSyncController = audioTimeSyncController;
            _bpmController = bpmController;
            _noteCutSoundEffectManager = noteCutSoundEffectManager;
      //      _gameEnergyUIPanel = gameEnergyUIPanel;
            SetupObjects();

        }

        public void SetupObjects()
        {
            soundEffectManager = _noteCutSoundEffectManager;
            beatmapObjectManager = _beatmapObjectManager;
            spawnController = _beatmapObjectSpawnController;
            callbacksController = _beatmapCallbacksController;
            energyCounter = _gameEnergyCounter;
            ColorManager = _colorManager;
            pauseManager = _standardLevelGameplayManager;
            AudioTimeSync = _audioTimeSyncController;
            bpmController = _bpmController as BpmController;
   //         energyPanel = _gameEnergyUIPanel;
            songAudio = _audioTimeSyncController.GetField<AudioSource, AudioTimeSyncController>("_audioSource");
            Mixer = _noteCutSoundEffectManager.GetField<AudioManagerSO, NoteCutSoundEffectManager>("_audioManager");
            GameModifiersController.SetupSpawnCallbacks();
        }
        public static IEnumerator FetchObjects()
        {
            yield return new WaitForSeconds(0.1f);
            soundEffectManager = Resources.FindObjectsOfTypeAll<NoteCutSoundEffectManager>().LastOrDefault();
            beatmapObjectManager = Resources.FindObjectsOfTypeAll<BeatmapObjectExecutionRatingsRecorder>().LastOrDefault().GetField<BeatmapObjectManager, BeatmapObjectExecutionRatingsRecorder>("_beatmapObjectManager") as BasicBeatmapObjectManager;
            spawnController = Resources.FindObjectsOfTypeAll<BeatmapObjectSpawnController>().LastOrDefault();
            callbacksController = spawnController?.GetField<BeatmapCallbacksController, BeatmapObjectSpawnController>("_beatmapCallbacksController");
            energyCounter = Resources.FindObjectsOfTypeAll<GameEnergyCounter>().LastOrDefault();
            ColorManager = Resources.FindObjectsOfTypeAll<NoteCutCoreEffectsSpawner>().LastOrDefault().GetField<ColorManager, NoteCutCoreEffectsSpawner>("_colorManager");
            pauseManager = Resources.FindObjectsOfTypeAll<StandardLevelGameplayManager>().LastOrDefault();
            AudioTimeSync = Resources.FindObjectsOfTypeAll<AudioTimeSyncController>().LastOrDefault();
            if (AudioTimeSync != null)
            {
                songAudio = AudioTimeSync.GetField<AudioSource, AudioTimeSyncController>("_audioSource");
                if (songAudio == null)
                    Plugin.Log("Audio null");
            }
            Mixer = soundEffectManager.GetField<AudioManagerSO, NoteCutSoundEffectManager>("_audioManager");
            GameModifiersController.SetupSpawnCallbacks();
        }
        public static void Load()
        {

        }

        public void Dispose()
        {
            
        }
    }
}
