using AdventureSystems;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using static UserDataManager;
public class HomeSystem : SystemBase
{
    HomeUI _homeUI;
    WorldManager _worldManager;
    public UserDataManager _userDataManager;
    public AdventureSystem _adventureSystem;

    bool _onSymphonyClicked;
    public bool OnSymphonyClicked { get => _onSymphonyClicked; set => _onSymphonyClicked = value; }
    [SerializeField]
    List<AudioClip> _symphonyHomeVocies;

    public override void Initialize()
    {
        _userDataManager = mainSystem._userDataManager;
        _homeUI = GetComponentInChildren<HomeUI>();
        _adventureSystem = GetComponentInChildren<AdventureSystem>();
        _homeUI.UIAwake(this);
        mainSystem._worldManager.WeatherSet(WorldManager.Weather.sunny);
        mainSystem._worldManager.Initialize();
        mainSystem.DataSave();
        mainSystem.SoundPlay(MainSystem.AudioPlayKind.BGM, 1);
        StartCoroutine(HomeVoice());
    }

    IEnumerator HomeVoice()
    {
        while (true)
        {
            float time = Time.time;
            if (Time.time - time >= 10 || _onSymphonyClicked)
            {
                _onSymphonyClicked = false;
                PlayHomeVoice();
            }
            yield return null;
        }
    }

    void PlayHomeVoice()
    {
        List<AudioClip> audioClips = new()
        {
            _symphonyHomeVocies[0],
            _symphonyHomeVocies[1],
            _symphonyHomeVocies[2]
        };
        SaveData saveData = mainSystem._userDataManager.saveData;
        if (saveData.hunger <= 60)
        {
            if (saveData.hunger > 30) audioClips.Add(_symphonyHomeVocies[3]);
            else if (saveData.hunger > 10) audioClips.Add(_symphonyHomeVocies[4]);
            else audioClips.Add(_symphonyHomeVocies[5]);
        }
        if (saveData.thirst <= 50) audioClips.Add(audioClips[6]);
        if (saveData.health <= 80)
        {
            if (saveData.health > 50) audioClips.Add(audioClips[7]);
            else audioClips.Add(audioClips[8]);
        }
        int time = saveData.time + WorldManager.timeDifference;
        if (5 <= time && time <= 7) audioClips.Add(audioClips[9]);
        if (18 <= time && time <= 21 && saveData.campLevel < 1) audioClips.Add(audioClips[10]);
        if (22 <= time && time <= 4 && saveData.campLevel < 1) audioClips.Add(audioClips[11]);

        int index = UnityEngine.Random.Range(0, audioClips.Count);
        mainSystem.VoicePlay(audioClips[index]);
    }

    public void Movement(int value)
    {
        _userDataManager.ChangeDistance(AdventureSystem.MovementTimeToDistance(value));
        _userDataManager.ChangeTime(value);
        _userDataManager.ChangeHunger(-AdventureSystem.MovementTimeToHunger(value));
        _userDataManager.ChangeHealth(-AdventureSystem.MovementTimeToHealth(value));
        mainSystem.StoryAction(StoryManager.StoryKindEnum.Movement);
    }

    public void Collect(ItemKind collectWindowKind)
    {
        mainSystem.StoryAction(StoryManager.StoryKindEnum.Collect);
        ItemDataBase.ItemCollectData data = _adventureSystem.itemData
            .itemDataList[Array.IndexOf(Enum.GetValues(typeof(ItemKind)), collectWindowKind)].collectData;
        switch (collectWindowKind)
        {
            case ItemKind.branch:
                _userDataManager.ChangeItemValue(ItemKind.branch, UnityEngine.Random.Range(data.getMinValue, data.getMaxValue + 1));
                break;
            case ItemKind.food:
                _userDataManager.ChangeItemValue(ItemKind.food, UnityEngine.Random.Range(data.getMinValue, data.getMaxValue + 1));
                break;
            case ItemKind.dertyWater:
                _userDataManager.ChangeItemValue(ItemKind.dertyWater, UnityEngine.Random.Range(data.getMinValue, data.getMaxValue + 1));
                break;
        }
        _userDataManager.ChangeTime(data.time);
        _userDataManager.ChangeHunger(-data.hunger);
    }

    public void Bonfire(int value)
    {
        _userDataManager.ChangeTime(1);
        _userDataManager.ChangeHunger(-8);
        _userDataManager.ChangeBonfireLevel(Mathf.Min(AdventureSystem.BonfireBecomeLevel(value) + _userDataManager.saveData.campLevel, 8));
        _userDataManager.ChangeItemValue(ItemKind.branch, -value * 5);
        mainSystem.BackToHome();
    }

    public void Rest(int value)
    {
        _userDataManager.ChangeTime(value);
        _userDataManager.ChangeHealth(AdventureSystem.RestHealHealth(value, _userDataManager.saveData.campLevel));
        mainSystem.BackToHome();
    }

    public void Craft(ItemKind itemKind, int value, List<CraftDataBase.RequireMaterial> list)
    {
        foreach (CraftDataBase.RequireMaterial item in list)
        {
            _userDataManager.ChangeItemValue(item.materialKind, -item.value);
        }
        _userDataManager.ChangeItemValue(itemKind, value);
        Debug.Log(_userDataManager.saveData.itemList[2]);
    }

    public void ItemUse(int index)
    {
        ItemDataBase.ItemInventoryData itemData = _adventureSystem.itemData.itemDataList[index].inventoryData;
        foreach (ItemDataBase.ItemEfficacy efficacy in itemData.itemEfficacy)
        {
            switch (efficacy.statusKind)
            {
                case StatusKind.Health:
                    _userDataManager.ChangeHealth(efficacy.value);
                    break;
                case StatusKind.Hunger:
                    _userDataManager.ChangeHunger(efficacy.value);
                    break;
                case StatusKind.Thirst:
                    _userDataManager.ChangeThirst(efficacy.value);
                    break;
            }
        }
        _userDataManager.ChangeItemValue(_adventureSystem.itemData.itemDataList[index].kind, -1);
        switch (_adventureSystem.itemData.itemDataList[index].kind)
        {
            case ItemKind.food:
                mainSystem.SoundPlay(0, 0);
                break;
            case ItemKind.water:
            case ItemKind.dertyWater:
                mainSystem.SoundPlay(0, 1);
                break;
        }
    }
}