using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Reflection;
namespace GamePlayModifiersPlus.Multiplayer
{
    public class MultiGMPDisplay : MonoBehaviour
    {
        public TextMeshPro chargeText;
        TextMeshPro chargeCountText;
        public TextMeshPro cooldownText;
        public TextMeshPro activeCommandText;
        Image chargeCounter;

        private void Awake()
        {
            Init();
        }





        void Init()
        {
            GameObject textObj = new GameObject("MultiGMPDisplayText");
            chargeText = textObj.AddComponent<TextMeshPro>();
            chargeText.text = "Charging...";
            chargeText.alignment = TextAlignmentOptions.Center;
            chargeText.fontSize = 3;
            chargeText.color = Color.white;
            chargeText.font = Resources.Load<TMP_FontAsset>("Teko-Medium SDF No Glow");
            chargeText.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 2f);
            chargeText.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 2f);
            if (Plugin.ChatConfig.uiOnTop)
            {
                chargeText.rectTransform.position = new Vector3(0f, 3f, 7f);
                textObj.transform.localScale *= 1.5f;
            }

            else
            {
                chargeText.rectTransform.position = new Vector3(0f, -1f, 7f);
                textObj.transform.localScale *= 2.0f;
            }


            var counterImage = ReflectionUtil.GetPrivateField<Image>(
    Resources.FindObjectsOfTypeAll<ScoreMultiplierUIController>().First(), "_multiplierProgressImage");

            GameObject canvasobj = new GameObject("MultiGMPDisplayCanvas");
            Canvas canvas = canvasobj.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.WorldSpace;
            CanvasScaler cs = canvasobj.AddComponent<CanvasScaler>();
            cs.scaleFactor = 10.0f;
            cs.dynamicPixelsPerUnit = 10f;
            GraphicRaycaster gr = canvasobj.AddComponent<GraphicRaycaster>();
            canvasobj.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 1f);
            canvasobj.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 1f);

            GameObject counter = new GameObject("MultiGMPDisplayCounter");
            chargeCounter = counter.AddComponent<Image>();
            counter.transform.parent = canvasobj.transform;
            counter.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 0.5f);
            counter.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 0.5f);
            counter.transform.localScale = new Vector3(1f, 1f, 1f);

            chargeCounter.sprite = counterImage.sprite;
            chargeCounter.type = Image.Type.Filled;
            chargeCounter.fillMethod = Image.FillMethod.Radial360;
            chargeCounter.fillOrigin = (int)Image.Origin360.Top;
            chargeCounter.fillClockwise = true;
            chargeCounter.fillAmount = Plugin.charges / MultiMain.Config.maxCharges;
            chargeCounter.color = Color.green;

            GameObject background = new GameObject("MultiGMPDisplayBackGround");
            var bg = background.AddComponent<Image>();
            background.transform.parent = canvasobj.transform;
            background.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 0.5f);
            background.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 0.5f);
            background.transform.localScale = new Vector3(1f, 1f, 1f);

            bg.sprite = counterImage.sprite;
            bg.CrossFadeAlpha(0.05f, 1f, false);

            canvasobj.GetComponent<RectTransform>().SetParent(textObj.transform, false);
            canvasobj.transform.localPosition = new Vector3(0f, -.4f, 0f);

            chargeCountText = new GameObject("MultiGMPDisplayChargeCount").AddComponent<TextMeshPro>();
            chargeCountText.text = Plugin.charges.ToString();
            chargeCountText.alignment = TextAlignmentOptions.Center;
            chargeCountText.fontSize = 2.5f;
            chargeCountText.color = Color.white;
            chargeCountText.font = Resources.Load<TMP_FontAsset>("Teko-Medium SDF No Glow");
            chargeCountText.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 1f);
            chargeCountText.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 1f);
            chargeCountText.GetComponent<RectTransform>().SetParent(textObj.transform, false);
            chargeCountText.transform.localPosition = new Vector3(0f, -.43f, 0f);

            cooldownText = new GameObject("MultiGMPDisplayCoolDown").AddComponent<TextMeshPro>();
            cooldownText.text = "";
            cooldownText.alignment = TextAlignmentOptions.MidlineRight;
            cooldownText.fontSize = 2.5f;
            cooldownText.color = Color.red;
            cooldownText.font = Resources.Load<TMP_FontAsset>("Teko-Medium SDF No Glow");
            cooldownText.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 10f);
            cooldownText.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 1f);
            cooldownText.GetComponent<RectTransform>().SetParent(textObj.transform, false);
            cooldownText.transform.localPosition = new Vector3(-5.4f, -.43f, 0f);

            activeCommandText = new GameObject("MultiGMPDisplayActiveCommands").AddComponent<TextMeshPro>();
            activeCommandText.text = "";
            activeCommandText.alignment = TextAlignmentOptions.MidlineLeft;
            activeCommandText.fontSize = 2.5f;
            activeCommandText.color = Color.yellow;
            activeCommandText.font = Resources.Load<TMP_FontAsset>("Teko-Medium SDF No Glow");
            activeCommandText.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 10f);
            activeCommandText.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 1f);
            activeCommandText.GetComponent<RectTransform>().SetParent(textObj.transform, false);
            activeCommandText.transform.localPosition = new Vector3(5.4f, -.43f, 0f);
        }

        void Update()
        {
            chargeCounter.fillAmount = Mathf.Lerp(chargeCounter.fillAmount, (float)MultiMain.Config.charges / MultiMain.Config.maxCharges, .03f);
            chargeCountText.text = MultiMain.Config.charges.ToString();
        }

        public void DestroyDis()
        {
            Destroy(GameObject.Find("MultiGMPDisplayCanvas"));
            Destroy(GameObject.Find("MultiGMPDisplayCounter"));
            Destroy(GameObject.Find("MultiGMPDisplayBackGround"));
            Destroy(GameObject.Find("MultiGMPDisplayText"));
            Destroy(GameObject.Find("MultiGMPDisplayCoolDown"));
            Destroy(GameObject.Find("MultiGMPDisplayActiveCommands"));
        }
    }



}