﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Reflection;
using BS_Utils.Utilities;
using IPA.Utilities;
using HMUI;
namespace GamePlayModifiersPlus.TwitchStuff
{
    public class GMPDisplay : MonoBehaviour
    {
        private bool initialized = false;
        CurvedTextMeshPro chargeText;
        CurvedTextMeshPro chargeCountText;
        public CurvedTextMeshPro cooldownText;
        public CurvedTextMeshPro activeCommandText;
        public List<string> activeCommands = new List<string>();
        public List<string> activeCooldowns = new List<string>();
        ImageView chargeCounter;

        public static void ResetActives()
        {
            var display = Plugin.chatPowers.GetComponent<GMPDisplay>();
            if(display)
            {
                display.activeCommands.Clear();
                display.activeCooldowns.Clear();
                display.UpdateTextDisplays();
            }
        }
        public static void AddActiveCommand(string command)
        {
            var display = Plugin.chatPowers.GetComponent<GMPDisplay>();
            if (display && !display.activeCommands.Contains(command))
                display.activeCommands.Add(command);
            display?.UpdateTextDisplays();
        }
        public static void RemoveActiveCommand(string command)
        {
            var display = Plugin.chatPowers.GetComponent<GMPDisplay>();
            if (display && display.activeCommands.Contains(command))
                display.activeCommands.Remove(command);
            display?.UpdateTextDisplays();
        }
        public static void AddActiveCooldown(string cooldown)
        {
            var display = Plugin.chatPowers.GetComponent<GMPDisplay>();
            if (display && !display.activeCooldowns.Contains(cooldown))
                display.activeCooldowns.Add(cooldown);
            display?.UpdateTextDisplays();
        }
        public static void RemoveActiveCooldown(string cooldown)
        {
            var display = Plugin.chatPowers.GetComponent<GMPDisplay>();
            if (display && display.activeCooldowns.Contains(cooldown))
                display.activeCooldowns.Remove(cooldown);
            display?.UpdateTextDisplays();
        }

        public void UpdateTextDisplays()
        {
            activeCommandText.text = string.Join(" | ", activeCommands);
            cooldownText.text = string.Join(" | ", activeCooldowns);
        }
        private void Awake()
        {
            StartCoroutine(Init());
        }

        IEnumerator Init()
        {
            yield return new WaitForSeconds(0.1f);
            GameObject display = new GameObject("GMPDisplay");
            GameObject textObj = new GameObject("GMPDisplayText");
            if (Config.uiOnTop)
            {
                textObj.transform.position = new Vector3(0.1f, 3f, 7f);
                textObj.transform.localScale *= 1.5f;
            }

            else
            {
                textObj.transform.position = new Vector3(0.2f, -1f, 7f);
                textObj.transform.localScale *= 2.0f;
            }
            textObj.transform.SetParent(display.transform);

            var counterImage = Resources.FindObjectsOfTypeAll<ScoreMultiplierUIController>().First().
                GetField<Image, ScoreMultiplierUIController>("_multiplierProgressImage");


            GameObject canvasobj = new GameObject("GMPDisplayCanvas");
            Canvas canvas = canvasobj.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.WorldSpace;
            CanvasScaler cs = canvasobj.AddComponent<CanvasScaler>();
            cs.scaleFactor = 10.0f;
            cs.dynamicPixelsPerUnit = 10f;
            GraphicRaycaster gr = canvasobj.AddComponent<GraphicRaycaster>();
            canvasobj.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 1f);
            canvasobj.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 1f);

            GameObject counter = new GameObject("GMPDisplayCounter");
            chargeCounter = counter.AddComponent<ImageView>();
            counter.transform.parent = canvasobj.transform;
            counter.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 0.5f);
            counter.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 0.5f);
            counter.transform.localScale = new Vector3(1f, 1f, 1f);

            chargeCounter.sprite = counterImage.sprite;
            chargeCounter.type = Image.Type.Filled;
            chargeCounter.fillMethod = Image.FillMethod.Radial360;
            chargeCounter.fillOrigin = (int)Image.Origin360.Top;
            chargeCounter.fillClockwise = true;
            chargeCounter.fillAmount = GameModifiersController.charges / Config.maxCharges;
            chargeCounter.color = Color.green;

            GameObject background = new GameObject("GMPDisplayBackGround");
            var bg = background.AddComponent<ImageView>();
            background.transform.parent = canvasobj.transform;
            background.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 0.5f);
            background.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 0.5f);
            background.transform.localScale = new Vector3(1f, 1f, 1f);

            bg.sprite = counterImage.sprite;
            bg.CrossFadeAlpha(0.05f, 1f, false);

            canvasobj.GetComponent<RectTransform>().SetParent(textObj.transform, false);
            canvasobj.transform.localPosition = new Vector3(-0.1f, -.1f, 0f);

            chargeText = BeatSaberMarkupLanguage.BeatSaberUI.CreateText(canvas.transform as RectTransform, "Charges", new Vector2(-0.25f, 0.5f)) as CurvedTextMeshPro;
            chargeText.fontSize = 3;
            chargeText.transform.localScale *= .08f;
            chargeText.color = Color.white;
            //    chargeText.font = Resources.Load<TMP_FontAsset>("Teko-Medium SDF No Glow");
            chargeText.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 1f);
            chargeText.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 1f);
            chargeText.GetComponent<RectTransform>().SetParent(canvas.transform, false);


            chargeCountText = BeatSaberMarkupLanguage.BeatSaberUI.CreateText(canvas.transform as RectTransform, GameModifiersController.charges.ToString(), new Vector2(0, 0)) as CurvedTextMeshPro;
            chargeCountText.text = GameModifiersController.charges.ToString();
            chargeCountText.alignment = TextAlignmentOptions.Center;
            chargeCountText.transform.localScale *= .08f;
            chargeCountText.fontSize = 2.5f;
            chargeCountText.color = Color.white;
            chargeCountText.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 1f);
            chargeCountText.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 1f);
            chargeCountText.GetComponent<RectTransform>().SetParent(canvas.transform, false);
            //   chargeCountText.transform.localPosition = new Vector3(-0.0925f, -.13f, 0f);

            cooldownText = BeatSaberMarkupLanguage.BeatSaberUI.CreateText(canvas.transform as RectTransform, GameModifiersController.charges.ToString(), new Vector2(-1f, 0.015f)) as CurvedTextMeshPro;
            cooldownText.text = "";
            cooldownText.alignment = TextAlignmentOptions.MidlineRight;
            cooldownText.fontSize = 2.5f;
            cooldownText.transform.localScale *= .08f;
            cooldownText.color = Color.red;
            //     cooldownText.font = Resources.Load<TMP_FontAsset>("Teko-Medium SDF No Glow");
            cooldownText.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 10f);
            cooldownText.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 1f);
            cooldownText.GetComponent<RectTransform>().SetParent(canvas.transform, false);

            activeCommandText = BeatSaberMarkupLanguage.BeatSaberUI.CreateText(canvas.transform as RectTransform, GameModifiersController.charges.ToString(), new Vector2(1f, 0.015f)) as CurvedTextMeshPro;
            activeCommandText.text = "";
            activeCommandText.alignment = TextAlignmentOptions.MidlineLeft;
            activeCommandText.fontSize = 2.5f;
            activeCommandText.transform.localScale *= .08f;
            activeCommandText.color = Color.yellow;
            //     activeCommandText.font = Resources.Load<TMP_FontAsset>("Teko-Medium SDF No Glow");
            activeCommandText.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 10f);
            activeCommandText.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 1f);
            activeCommandText.GetComponent<RectTransform>().SetParent(canvas.transform, false);

            GameObject coreGameHUD = Resources.FindObjectsOfTypeAll<CoreGameHUDController>()?.FirstOrDefault(x => x.isActiveAndEnabled)?.gameObject ?? null;
            FlyingGameHUDRotation flyingGameHUD = Resources.FindObjectsOfTypeAll<FlyingGameHUDRotation>().FirstOrDefault(x => x.isActiveAndEnabled);
            if(coreGameHUD != null)
                display.transform.SetParent(coreGameHUD.transform, true);
            //      textObj.transform.position = new Vector3(0, 0f, 0);
                  display.transform.localPosition = new Vector3(0, 0f, 0f);
                  display.transform.localRotation = Quaternion.identity;
            if (flyingGameHUD != null)
            {
                display.transform.localPosition = new Vector3(0, 0.75f, 6f);
                display.transform.eulerAngles = new Vector3(345f, 0f, 0f);
            }
            var canvasSettings = canvasobj.AddComponent<CurvedCanvasSettings>();
            canvasSettings.SetRadius(0);
            display.SetActive(GMPUI.chatIntegration);
            initialized = true;
        }

        void Update()
        {
            if (!initialized) return;
            chargeCounter.fillAmount = Mathf.Lerp(chargeCounter.fillAmount, (float)GameModifiersController.charges / Config.maxCharges, .03f);
            chargeCountText.text = GameModifiersController.charges.ToString();
        }

        public void Destroy()
        {
            Destroy(GameObject.Find("GMPDisplayCanvas"));
            Destroy(GameObject.Find("GMPDisplayCounter"));
            Destroy(GameObject.Find("GMPDisplayBackGround"));
            Destroy(GameObject.Find("GMPDisplayText"));
            Destroy(GameObject.Find("GMPDisplayCoolDown"));
            Destroy(GameObject.Find("GMPDisplayActiveCommands"));
            Destroy(GameObject.Find("GMPDisplay"));
        }
    }



}