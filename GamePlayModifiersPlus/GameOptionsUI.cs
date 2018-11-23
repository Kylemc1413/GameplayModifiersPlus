using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/*
 * Created by Moon on 10/14/2018
 * Helper class to assist in adding items to the Game Option list
 * 
 * IMPORTANT!!! => You *MUST* use this in "ActiveSceneChanged".
 * This class registers a callback with "SceneLoaded" for its own purposes.
 * If you use this in "SceneLoaded", the internal callback will never be called.
 * 
 * Notes:
 * This can be used in multiple plugins at the same time.
 * 
 * Yes, I know my use of reflection is a big, big, avoidable mess. I know.
 * I see it too and it makes me cringe because this could all be so much
 * simpler without dealing with multiple plugins. But you know what?
 * This is how I did it. ~~I had fun~~. It works.
 * Let's just agree to not ask questions, eh?
 * 
 * The plugin with the latest version of the helper will be used to display
 * the menu. For example, right now, the custom options pages look exactly
 * like the default options page. They have the divider and the Defaults
 * button. We'll call this "Version 1". I'm thinking of making the custom
 * options pages cover the whole panel, so that option names can be longer
 * without wrapping. We'll call that "Update 2". If one plugin is using
 * "Update 1" and another is using "Update 2", the one using "Update 2"
 * will be the one that Build()s the menu, and the plugin using
 * "Update 1" will have its options displayed covering the whole panel.
 */

namespace GamePlayModifiersPlus
{
    abstract class GameOption
    {
        public GameObject gameObject;
        public string optionName;
        public abstract void Instantiate();
    }

    class ToggleOption : GameOption
    {
        public event Action<bool> OnToggle;
        public bool GetValue = false;

        public ToggleOption(string optionName)
        {
            this.optionName = optionName;
        }

        public override void Instantiate()
        {
            //We have to find our own target
            //TODO: Clean up time complexity issue. This is called for each new option
            StandardLevelDetailViewController _sldvc = Resources.FindObjectsOfTypeAll<StandardLevelDetailViewController>().First();
            GameplayOptionsViewController _govc = _sldvc.GetField<GameplayOptionsViewController>("_gameplayOptionsViewController");
            RectTransform container = (RectTransform)_govc.transform.Find("Switches").Find("Container");

            //TODO: Can probably slim this down a bit
            gameObject = UnityEngine.Object.Instantiate(container.Find("NoEnergy").gameObject, container);
            gameObject.GetComponentInChildren<TextMeshProUGUI>().text = optionName;
            gameObject.name = optionName;
            gameObject.layer = container.gameObject.layer;
            gameObject.transform.parent = container;
            gameObject.transform.localPosition = Vector3.zero;
            gameObject.transform.localScale = Vector3.one;
            gameObject.transform.rotation = Quaternion.identity;
            gameObject.SetActive(false); //All options start disabled

            gameObject.GetComponentInChildren<HMUI.Toggle>().isOn = GetValue;
            gameObject.GetComponentInChildren<HMUI.Toggle>().didSwitchEvent += (_, e) => { OnToggle?.Invoke(e); };
        }
    }

    class MultiSelectOption : GameOption
    {
        private Dictionary<float, string> _options = new Dictionary<float, string>();
        public Func<float> GetValue;
        public event Action<float> OnChange;

        public MultiSelectOption(string optionName)
        {
            this.optionName = optionName;
        }

        public override void Instantiate()
        {
            //We have to find our own target
            //TODO: Clean up time complexity issue. This is called for each new option
            StandardLevelDetailViewController _sldvc = Resources.FindObjectsOfTypeAll<StandardLevelDetailViewController>().First();
            GameplayOptionsViewController _govc = _sldvc.GetField<GameplayOptionsViewController>("_gameplayOptionsViewController");
            RectTransform container = (RectTransform)_govc.transform.Find("Switches").Find("Container");

            var volumeSettings = Resources.FindObjectsOfTypeAll<VolumeSettingsController>().FirstOrDefault();
            gameObject = UnityEngine.Object.Instantiate(volumeSettings.gameObject, container);
            gameObject.name = optionName;
            gameObject.GetComponentInChildren<TMP_Text>().text = optionName;

            //Slim down the toggle option so it fits in the space we have before the divider
            (gameObject.transform as RectTransform).sizeDelta = new Vector2(50, (gameObject.transform as RectTransform).sizeDelta.y);

            //This magical nonsense is courtesy of Taz and his SettingsUI class
            VolumeSettingsController volume = gameObject.GetComponent<VolumeSettingsController>();
            ListViewController newListSettingsController = (ListViewController)ReflectionUtil.CopyComponent(volume, typeof(ListSettingsController), typeof(ListViewController), gameObject);
            UnityEngine.Object.DestroyImmediate(volume);

            newListSettingsController.values = _options.Keys.ToList();
            newListSettingsController.SetValue = OnChange;
            newListSettingsController.GetValue = () =>
            {
                if (GetValue != null) return GetValue.Invoke();
                return _options.Keys.ElementAt(0);
            };
            newListSettingsController.GetTextForValue = (v) =>
            {
                if (_options.ContainsKey(v)) return _options[v];
                return "UNKNOWN";
            };

            //Initialize the controller, as if we had just opened the settings menu
            newListSettingsController.Init();
            gameObject.SetActive(false);
        }

        public void AddOption(float value)
        {
            _options.Add(value, Convert.ToString(value));
        }

        public void AddOption(float value, string option)
        {
            _options.Add(value, option);
        }
    }

    class GameOptionsUI : MonoBehaviour
    {
        //The version of this helper.
        //The plugin using the latest version
        //will be used to Build() the options menu
        public const int versionConst = 001;
        public int versionCode = versionConst; //Non-static so it resets on each creation

        //The function to call to build the UI
        private static Action _buildFunc = Build;

        //Handle instances (each instance dies on new scene load, new ones are created on access)
        private static GameOptionsUI _instance;
        private static GameOptionsUI Instance
        {
            get
            {
                if (!_instance)
                {
                    var existingObject = GameObject.Find("GameOptionsUI");
                    if (existingObject != null)
                    {
                        var existingComponent = existingObject.GetComponent("GameOptionsUI");

                        //The following `if` is necessary because Unity overrides == to return null
                        //when a MonoBehavior isn't active. It's annoying, and getting in my way.
                        //Hence this workaround.
                        if (IsReallyNull(_instance))
                        {
                            _instance = new GameOptionsUI();
                        }

                        //Keep the override's options list up to date with the global one
                        var existingOptions = existingComponent.GetField<IList<object>>("customOptions");
                        _instance.customOptions = existingOptions;

                        //If this version is newer, we will override the build
                        if (existingComponent.GetField<int>("versionCode") < versionConst)
                        {
                            existingComponent.SetField("versionCode", _instance.versionCode);
                            existingComponent.InvokeMethod("SetBuild", (Action)Build);
                        }
                    }
                    else
                    {
                        _instance = new GameObject("GameOptionsUI").AddComponent<GameOptionsUI>();

                        //If we're here, this is the class that will handle the ".Build()"
                        //when necessary. Only the first instance created will make it here.
                        UnityAction<Scene, LoadSceneMode> buildWhenLoaded = (Scene arg0, LoadSceneMode _) => {
                            if (arg0.name == "Menu")
                            {
                                //Whatever instance which is currently the most recent will be called
                                _buildFunc.Invoke();
                            }
                        };
                        SceneManager.sceneLoaded -= buildWhenLoaded;
                        SceneManager.sceneLoaded += buildWhenLoaded;
                    }
                }

                return _instance;
            }
        }

        //Helper. Checks to see if we can access the custom options.
        //If so, the instance exists.
        //Hacky. Shut up.
        public static bool IsReallyNull(GameOptionsUI toTest)
        {
            IList<object> sample = null;
            try
            {
                sample = toTest.GetField<IList<object>>("customOptions");
            }
            catch { }
            return sample == null;
        }

        //Set the build method to call
        public static void SetBuild(Action buildFunc)
        {
            _buildFunc = buildFunc;
        }

        //Index of current list
        private int _listIndex = 0;

        //Holds all the custom options the user specifies
        //We specify type "object" so that the list doesn't
        //encounter casting issues when it's grabbed for reflection,
        //in the above code. WE WILL ASSUME THAT THIS IS A LIST
        //OF GAMEOPTIONS
        private IList<object> customOptions = new List<object>();

        //Returns a list of options for the current page index
        private IList<object> GetOptionsForPage(int page)
        {
            //Default options
            if (page == 0) return null;

            page--; //If the page isn't 0, we should pick from the 0th pagination of our list

            //Get 4 custom options and return them
            return customOptions.Skip(4 * page).Take(4).ToList();
        }

        //Sets the active value for our game options depending on the active page
        private void ChangePage(int page, Transform container, params Transform[] defaults)
        {
            var options = Instance.GetOptionsForPage(Instance._listIndex);
            bool defaultsActive = options == null;
            defaults?.ToList().ForEach(x => x.gameObject.SetActive(defaultsActive));

            //Custom options
            Instance.customOptions?.ToList().ForEach(x => x.GetField<GameObject>("gameObject").SetActive(false));
            if (!defaultsActive) options?.ToList().ForEach(x => x.GetField<GameObject>("gameObject").SetActive(true));
        }

        public static MultiSelectOption CreateListOption(string optionName)
        {
            MultiSelectOption ret = new MultiSelectOption(optionName);
            Instance.customOptions.Add(ret);
            return ret;
        }

        public static ToggleOption CreateToggleOption(string optionName)
        {
            ToggleOption ret = new ToggleOption(optionName);
            Instance.customOptions.Add(ret);
            return ret;
        }

        public static void Build()
        {
            //Grab necessary references
            StandardLevelDetailViewController _sldvc = Resources.FindObjectsOfTypeAll<StandardLevelDetailViewController>().First();
            GameplayOptionsViewController _govc = _sldvc.GetField<GameplayOptionsViewController>("_gameplayOptionsViewController");

            //Get reference to the switch container
            RectTransform container = (RectTransform)_govc.transform.Find("Switches").Find("Container");
            container.sizeDelta = new Vector2(container.sizeDelta.x, container.sizeDelta.y + 7f); //Grow container so it aligns properly with text

            //Get references to the original switches, so we can later duplicate then destroy them
            Transform noEnergyOriginal = container.Find("NoEnergy");
            Transform noObstaclesOriginal = container.Find("NoObstacles");
            Transform mirrorOriginal = container.Find("Mirror");
            Transform staticLightsOriginal = container.Find("StaticLights");

            //Get references to other UI elements we need to hide
            //Transform divider = (RectTransform)_govc.transform.Find("Switches").Find("Separator");
            //Transform defaults = (RectTransform)_govc.transform.Find("Switches").Find("DefaultsButton");

            //Future duplicated switches
            Transform noEnergy = null;
            Transform noObstacles = null;
            Transform mirror = null;
            Transform staticLights = null;

            //Future down button
            Button _pageDownButton = null;

            //Create up button
            Button _pageUpButton = Instantiate(Resources.FindObjectsOfTypeAll<Button>().First(x => (x.name == "PageUpButton")), container);
            _pageUpButton.transform.parent = container;
            _pageUpButton.transform.localScale = Vector3.one;
            (_pageUpButton.transform as RectTransform).sizeDelta = new Vector2((_pageUpButton.transform.parent as RectTransform).sizeDelta.x, 3.5f);
            _pageUpButton.onClick.AddListener(delegate ()
            {
                Instance.ChangePage(--Instance._listIndex, container, noEnergy, noObstacles, mirror, staticLights);

                //Nice responsive scroll buttons
                if (Instance._listIndex <= 0) _pageUpButton.interactable = false;
                if (Instance.customOptions.Count > 0) _pageDownButton.interactable = true;
            });
            _pageUpButton.interactable = false;

            //Duplicate and delete default toggles so that the up button is on the top
            noEnergy = Instantiate(noEnergyOriginal, container);
            noObstacles = Instantiate(noObstaclesOriginal, container);
            mirror = Instantiate(mirrorOriginal, container);
            staticLights = Instantiate(staticLightsOriginal, container);

            //Create custom options
            foreach (object option in Instance.customOptions)
            {
                //Due to possible "different" types (due to cross-plugin support), we need to do this through reflection
                option.InvokeMethod("Instantiate");
            }

            //Destroy original toggles and set their corresponding references to the new toggles
            DestroyImmediate(noEnergyOriginal.gameObject);
            DestroyImmediate(noObstaclesOriginal.gameObject);
            DestroyImmediate(mirrorOriginal.gameObject);
            DestroyImmediate(staticLightsOriginal.gameObject);

            _govc.SetField("_noEnergyToggle", noEnergy.gameObject.GetComponentInChildren<HMUI.Toggle>());
            _govc.SetField("_noObstaclesToggle", noObstacles.gameObject.GetComponentInChildren<HMUI.Toggle>());
            _govc.SetField("_mirrorToggle", mirror.gameObject.GetComponentInChildren<HMUI.Toggle>());
            _govc.SetField("_staticLightsToggle", staticLights.gameObject.GetComponentInChildren<HMUI.Toggle>());

            //Create down button
            _pageDownButton = Instantiate(Resources.FindObjectsOfTypeAll<Button>().First(x => (x.name == "PageDownButton")), container);
            _pageDownButton.transform.parent = container;
            _pageDownButton.transform.localScale = Vector3.one;
            (_pageDownButton.transform as RectTransform).sizeDelta = new Vector2((_pageDownButton.transform.parent as RectTransform).sizeDelta.x, (_pageDownButton.transform as RectTransform).sizeDelta.y);
            _pageDownButton.onClick.AddListener(delegate ()
            {
                Instance.ChangePage(++Instance._listIndex, container, noEnergy, noObstacles, mirror, staticLights);

                //Nice responsive scroll buttons
                if (Instance._listIndex >= 0) _pageUpButton.interactable = true;
                if (((Instance.customOptions.Count + 4 - 1) / 4) - Instance._listIndex <= 0) _pageDownButton.interactable = false;
            });
            _pageDownButton.interactable = Instance.customOptions.Count > 0;

            //Unfortunately, due to weird object creation for versioning, this doesn't always
            //happen when the scene changes
            Instance._listIndex = 0;
        }
    }

    //This magic is courtesy of ProgressCounter, and slightly modified
    public class ListViewController : ListSettingsController
    {
        public Func<float> GetValue = () => default(float);
        public Action<float> SetValue = (_) => { };
        public Func<float, string> GetTextForValue = (_) => "?";

        public List<float> values;

        protected override void GetInitValues(out int idx, out int numberOfElements)
        {
            numberOfElements = values.Count;
            var value = GetValue();

            numberOfElements = values.Count();
            idx = values.FindIndex(v => v.Equals(value));
        }

        protected override void ApplyValue(int idx)
        {
            SetValue(values[idx]);
        }

        protected override string TextForValue(int idx)
        {
            return GetTextForValue(values[idx]);
        }

        public override void IncButtonPressed()
        {
            base.IncButtonPressed();
            ApplySettings();
        }

        public override void DecButtonPressed()
        {
            base.DecButtonPressed();
            ApplySettings();
        }
    }
}