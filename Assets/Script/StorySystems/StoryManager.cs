using System;
using System.Collections.Generic;
using UnityEngine;

public class StoryManager : MonoBehaviour
{
    public enum StoryKind
    {
        Story,
        Movement,
        Collect,
    }
    [Serializable]
    struct StoryTextData
    {
        public StoryTextDataBase storyTextData;
        public float distance;
    }

    StorySystem _storySystem;
    [SerializeField]
    List<StoryTextData> _storyData = new();
    [SerializeField]
    List<StoryTextDataBase> _MovementEventData = new();
    [SerializeField]
    List<StoryTextDataBase> _CollectStoryData = new();

    private void Start()
    {
        _storyData.Sort((x, y) => x.distance.CompareTo(y.distance));
    }

    public void SetStoryData(StoryKind storyKind)
    {
        _storySystem = FindAnyObjectByType<StorySystem>();
        switch (storyKind)
        {
            case StoryKind.Story:
                foreach (StoryTextData data in _storyData)
                {
                    if (SaveDataManager._mainSaveData.Value.distance <= data.distance)
                    {
                        _storySystem.TextDataLoad(data.storyTextData);
                        break;
                    }
                }
                break;
            case StoryKind.Movement:
                int indexM = UnityEngine.Random.Range(0, _MovementEventData.Count);
                _storySystem.TextDataLoad(_MovementEventData[indexM]);
                break;
            case StoryKind.Collect:
                int indexC = UnityEngine.Random.Range(0, _CollectStoryData.Count);
                _storySystem.TextDataLoad(_CollectStoryData[indexC]);
                break;
        }
        //最初のテキストを呼び出す
        StartCoroutine(_storySystem.NextText());
    }
}
