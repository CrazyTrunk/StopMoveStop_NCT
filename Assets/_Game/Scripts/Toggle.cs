using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Toggle : MonoBehaviour
{
    [SerializeField] private GameObject onObject;
    [SerializeField] private GameObject offObject;
    [SerializeField] private Button toggleButton;
    public enum FeatureType
    {
        Sound = 1,
        Vibration = 2,
        Minimap = 3
    }
    [SerializeField]private FeatureType featureType;
    private PlayerData playerData;
    private void OnEnable()
    {
        playerData = GameManager.Instance.GetPlayerData();
        ToggleFeature(featureType);
        toggleButton.onClick.AddListener(() => HandleToggle(featureType));
    }
    private void OnDisable()
    {
        toggleButton.onClick.RemoveAllListeners();
    }
    private void HandleToggle(FeatureType featureType)
    {
        switch (featureType)
        {
            case FeatureType.Sound:
                playerData.isSoundOn = !playerData.isSoundOn;
                AudioManager.Instance.ToggleMusicAndSound(playerData.isSoundOn);
                break;
            case FeatureType.Vibration:
                playerData.isVibrance = !playerData.isVibrance;
                break;
            case FeatureType.Minimap:
                playerData.isUsingMinimap = !playerData.isUsingMinimap;
                break;
        }
        ToggleFeature(featureType);
        GameManager.Instance.UpdatePlayerData(playerData);
        GameManager.Instance.SaveToJson(playerData, FilePathGame.CHARACTER_PATH);
    }

    public void ToggleFeature(FeatureType featureType)
    {
        switch (featureType)
        {
            case FeatureType.Sound:
                if (playerData.isSoundOn)
                {
                    onObject.SetActive(true);
                    offObject.SetActive(false);
                }
                else
                {
                    onObject.SetActive(false);
                    offObject.SetActive(true);
                }
                break;
            case FeatureType.Vibration:
                if (playerData.isVibrance)
                {
                    onObject.SetActive(true);
                    offObject.SetActive(false);
                }
                else
                {
                    onObject.SetActive(false);
                    offObject.SetActive(true);
                }
                break;
            case FeatureType.Minimap:
                if (playerData.isUsingMinimap)
                {
                    onObject.SetActive(true);
                    offObject.SetActive(false);
                }
                else
                {
                    onObject.SetActive(false);
                    offObject.SetActive(true);
                }
                break;
        }
    }
}
