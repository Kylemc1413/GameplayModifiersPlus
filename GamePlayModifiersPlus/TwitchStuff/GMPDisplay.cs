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
namespace GamePlayModifiersPlus.TwitchStuff
{
    public class GMPDisplay : MonoBehaviour
    {
        TextMeshPro chargeText;
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
            GameObject textObj = new GameObject("GMPDisplayText");
            chargeText = textObj.AddComponent<TextMeshPro>();
            chargeText.text = "Charges";
            chargeText.fontSize = 3;
            chargeText.color = Color.white;
            chargeText.font = Resources.Load<TMP_FontAsset>("Teko-Medium SDF No Glow");
            chargeText.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 1f);
            chargeText.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 1f);
            if (Plugin.Config.uiOnTop)
                chargeText.rectTransform.position = new Vector3(0.1f, 3.5f, 5f);
            else
            {
                chargeText.rectTransform.position = new Vector3(0.2f, -1f, 7f);
                textObj.transform.localScale *= 2.0f;
            }


            var counterImage = ReflectionUtil.GetPrivateField<Image>(
    Resources.FindObjectsOfTypeAll<ScoreMultiplierUIController>().First(), "_multiplierProgressImage");

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
            chargeCounter.fillAmount = Plugin.charges / Plugin.Config.maxCharges;
            chargeCounter.color = Color.green;

            GameObject background = new GameObject("GMPDisplayBackGround");
            var bg = background.AddComponent<Image>();
            background.transform.parent = canvasobj.transform;
            background.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 0.5f);
            background.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 0.5f);
            background.transform.localScale = new Vector3(1f, 1f, 1f);

            bg.sprite = counterImage.sprite;
            bg.CrossFadeAlpha(0.05f, 1f, false);

            canvasobj.GetComponent<RectTransform>().SetParent(textObj.transform, false);
            canvasobj.transform.localPosition = new Vector3(-0.1f, -.1f, 0f);

            chargeCountText = new GameObject("GMPDisplayChargeCount").AddComponent<TextMeshPro>();
            chargeCountText.text = Plugin.charges.ToString();
            chargeCountText.alignment = TextAlignmentOptions.Center;
            chargeCountText.fontSize = 2.5f;
            chargeCountText.color = Color.white;
            chargeCountText.font = Resources.Load<TMP_FontAsset>("Teko-Medium SDF No Glow");
            chargeCountText.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 1f);
            chargeCountText.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 1f);
            chargeCountText.GetComponent<RectTransform>().SetParent(textObj.transform, false);
            chargeCountText.transform.localPosition = new Vector3(-0.0925f, -.13f, 0f);

            cooldownText = new GameObject("GMPDisplayCoolDown").AddComponent<TextMeshPro>();
            cooldownText.text = "";
            cooldownText.alignment = TextAlignmentOptions.MidlineRight;
            cooldownText.fontSize = 2.5f;
            cooldownText.color = Color.red;
            cooldownText.font = Resources.Load<TMP_FontAsset>("Teko-Medium SDF No Glow");
            cooldownText.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 10f);
            cooldownText.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 1f);
            cooldownText.GetComponent<RectTransform>().SetParent(textObj.transform, false);
            cooldownText.transform.localPosition = new Vector3(-5.5f, -.13f, 0f);

            activeCommandText = new GameObject("GMPDisplayActiveCommands").AddComponent<TextMeshPro>();
            activeCommandText.text = "";
            activeCommandText.alignment = TextAlignmentOptions.MidlineLeft;
            activeCommandText.fontSize = 2.5f;
            activeCommandText.color = Color.yellow;
            activeCommandText.font = Resources.Load<TMP_FontAsset>("Teko-Medium SDF No Glow");
            activeCommandText.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 10f);
            activeCommandText.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 1f);
            activeCommandText.GetComponent<RectTransform>().SetParent(textObj.transform, false);
            activeCommandText.transform.localPosition = new Vector3(5.3f, -.13f, 0f);
        }

        void Update()
        {
            chargeCounter.fillAmount = Mathf.Lerp(chargeCounter.fillAmount, (float)Plugin.charges / Plugin.Config.maxCharges, .03f);
            chargeCountText.text = Plugin.charges.ToString();
        }

        public void Destroy()
        {
            Destroy(GameObject.Find("GMPDisplayCanvas"));
            Destroy(GameObject.Find("GMPDisplayCounter"));
            Destroy(GameObject.Find("GMPDisplayBackGround"));
            Destroy(GameObject.Find("GMPDisplayText"));
            Destroy(GameObject.Find("GMPDisplayCoolDown"));
            Destroy(GameObject.Find("GMPDisplayActiveCommands"));
        }
    }



}