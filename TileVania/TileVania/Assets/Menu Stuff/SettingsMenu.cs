using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
    public Dropdown ResolutionsDropDown; // cara o lance das resoluções é uma bagunça, mas vai com calma

    Resolution[] Resolutions; // isso serve p guardar as resoluções de tela do pc de cada pessoa
    private void Start()
    {
        Resolutions = Screen.resolutions;
        ResolutionsDropDown.ClearOptions(); // isso aqui é pra esvaziar o menu onde vai ficar as resoluções de tela, já que elas dependem de cada pessoa
        QualitySettings.SetQualityLevel(5); // qualidade gráfica, 5 é o default que é ULTRA


        List<string> Options = new List<string>();

        int CurrentResolutionIndex = 0;

        for (int i = 0; i < Resolutions.Length; i++)
        {
            string option = Resolutions[i].width + "x" + Resolutions[i].height;
            Options.Add(option); 

            if ((Resolutions[i].width == Screen.currentResolution.width) && (Resolutions[i].height == Screen.currentResolution.height))
            {
                CurrentResolutionIndex = i;
            }
        }

        ResolutionsDropDown.AddOptions(Options);
        ResolutionsDropDown.value = CurrentResolutionIndex;
        ResolutionsDropDown.RefreshShownValue();
    }

    public void SetResolution(int ResolutionIndex)
    {
        Resolution resolution = Resolutions[ResolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);

    }

    public AudioMixer audioMixer;
    public void SetVolume(float VolumeSlider)
    {
        audioMixer.SetFloat("MainVolume", VolumeSlider);    // o slider que controla o main volume
    }

    public void SetQuality (int QualityIndex)
    {
        QualitySettings.SetQualityLevel(QualityIndex);
        Debug.Log(QualityIndex);
    }

    public void SetFullScreen (bool IsFullScreen)
    {
        Screen.fullScreen = IsFullScreen;
    }
}
