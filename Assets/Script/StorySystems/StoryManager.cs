using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class StoryManager : MonoBehaviour
{
    public enum StoryKindEnum
    {
        Story,
        Movement,
        Collect,
    }
    [Serializable]
    public class StoryTextData
    {
        public StoryTextDataBase storyTextData;
        public float distance;
        [HideInInspector]
        public bool actived;

        public void Actived()
        {
            actived = true;
        }
    }
    MainSystem mainSystem;
    StorySystem _storySystem;
    public List<StoryTextData> _storyData = new();
    [SerializeField]
    List<StoryTextDataBase> _movementEventData = new();
    [SerializeField]
    List<StoryTextDataBase> _collectStoryData = new();

    private void Start()
    {
        _storyData.Sort((x, y) => x.distance.CompareTo(y.distance));
    }

    public void Initialized(MainSystem mainSystem)
    {
        this.mainSystem = mainSystem;
        if (SaveDataManager._mainSaveData.storyProgress != null && _storyData.Count <= SaveDataManager._mainSaveData.storyProgress.Count)
        {
            for (int i = 0; i < _storyData.Count; i++)
            {
                _storyData[i].actived = SaveDataManager._mainSaveData.storyProgress[i];
            }
        }
    }

    public void SetStoryData(StoryKindEnum storyKind)
    {
        mainSystem._worldManager.WeatherSet(WorldManager.Weather.snowy);
        _storySystem = FindAnyObjectByType<StorySystem>();
        switch (storyKind)
        {
            case StoryKindEnum.Story:
            case StoryKindEnum.Movement:
                bool storyActive = false;
                for (int i = 0; i < _storyData.Count; i++)
                {
                    if (SaveDataManager._mainSaveData.distance >= _storyData[i].distance && !_storyData[i].actived)
                    {
                        _storyData[i].Actived();
                        mainSystem._userDataManager.saveData.storyProgress = _storyData.Select(x => x.actived).ToList();
                        storyActive = true;
                        _storySystem.TextDataLoad(_storyData[i].storyTextData);
                        break;
                    }
                }
                if (storyActive) break;
                if (storyKind == StoryKindEnum.Movement)
                {
                    int indexM = UnityEngine.Random.Range(0, _movementEventData.Count);
                    _storySystem.TextDataLoad(_movementEventData[indexM]);
                }
                break;

            case StoryKindEnum.Collect:
                int indexC = UnityEngine.Random.Range(0, _collectStoryData.Count);
                _storySystem.TextDataLoad(_collectStoryData[indexC]);
                break;
        }
        //最初のテキストを呼び出す
        StartCoroutine(_storySystem.NextText());
    }
}