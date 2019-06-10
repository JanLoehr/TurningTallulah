using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FinishScreenController : MonoBehaviour
{
    [Header("Object Refs")]
    public Slider Slider;

    public GameObject CropOne;
    public GameObject CropTwo;
    public GameObject CropThree;

    public GameObject StarRef;
    public Text StarText;

    // Start is called before the first frame update
    void Start()
    {

    }

    /// <summary>
    /// This function is called when the behaviour becomes disabled or inactive.
    /// </summary>
    void OnDisable()
    {
        Slider.value = 0;

        CropOne.SetActive(false);
        CropTwo.SetActive(false);
        CropThree.SetActive(false);
        StarRef.SetActive(false);
    }

    public void ShowStats(int cropsReached, int starsCollected)
    {
        StartCoroutine(ShowStatsAsync(cropsReached, starsCollected));
    }

    private IEnumerator ShowStatsAsync(int cropsReached, int starsCollected)
    {
        yield return new WaitForSeconds(.5f);

        float lerp = 0;

        do
        {
            lerp += Time.deltaTime / 3;

            Slider.value = Mathf.Lerp(0, cropsReached, lerp);

            if (Slider.value > 50 && !CropOne.activeSelf)
            {
                CropOne.SetActive(true);
            }
            else if (Slider.value > 75 && !CropTwo.activeSelf)
            {
                CropTwo.SetActive(true);
            }
            else if (Slider.value >= 100 && !CropThree.activeSelf)
            {
                CropThree.SetActive(true);
            }

            yield return null;
        } while (lerp < 1);

        yield return new WaitForSeconds(.5f);

        if (starsCollected > 0)
        {
            StarRef.SetActive(true);
            StarText.text = starsCollected.ToString();
        }
    }
}
