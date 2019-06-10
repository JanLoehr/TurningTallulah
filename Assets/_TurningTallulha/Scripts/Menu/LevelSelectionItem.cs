using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelSelectionItem : MonoBehaviour
{
    public Text LevelName;

    public GameObject Crop1;
    public GameObject Crop2;
    public GameObject Crop3;

    public GameObject Star;
    public Text StarText;

    public StringEvent ButtonClicked;

    private bool _isEnabled = true;

    // Start is called before the first frame update
    void Start()
    {

    }

    public void Init(string name, TurningTallulah.Database.LevelData data, bool isEnabled)
    {
        LevelName.text = name;

        Crop1.SetActive(false);
        Crop2.SetActive(false);
        Crop3.SetActive(false);
        Star.SetActive(false);

        if (data != null)
        {
            if (data.Score > 50)
            {
                Crop1.SetActive(true);
            }

            if (data.Score > 80)
            {
                Crop2.SetActive(true);
            }

            if (data.Score >= 100)
            {
                Crop3.SetActive(true);
            }

            if (data.Stars > 0)
            {
                Star.SetActive(true);
                StarText.text = data.Stars.ToString();
            }
        }

        if (_isEnabled != isEnabled)
        {
            SetAlpha(isEnabled ? 1 : .5f);
        }

        _isEnabled = isEnabled;
    }

    private void SetAlpha(float alpha)
    {
        Image[] images = GetComponentsInChildren<Image>();
        Text[] texts = GetComponentsInChildren<Text>();

        for (int i = 0; i < images.Length; i++)
        {
            if (images[i].gameObject.name != "Button")
            {
                images[i].color = new Color(images[i].color.r, images[i].color.g, images[i].color.b, alpha); 
            }
        }
        for (int i = 0; i < texts.Length; i++)
        {
            if (images[i].gameObject.name != "Button")
            {
                texts[i].color = new Color(texts[i].color.r, texts[i].color.g, texts[i].color.b, alpha); 
            }
        }
    }

    public void OnClick()
    {
        if (_isEnabled)
        {
            ButtonClicked.Invoke(LevelName.text); 
        }
    }
}
