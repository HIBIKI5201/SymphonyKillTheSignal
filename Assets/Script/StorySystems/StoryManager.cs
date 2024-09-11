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
    class StoryTextData
    {
        public StoryTextDataBase storyTextData;
        public float distance;
        //[HideInInspector]
        public bool actived;

        public void Actived()
        {
            actived = true;
        }
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
            case StoryKind.Movement:
                bool storyActive = false;
                for (int i = 0;  i < _storyData.Count; i++)
                {
                    if (SaveDataManager._mainSaveData.distance >= _storyData[i].distance && !_storyData[i].actived)
                    {
                        _storyData[i].Actived();
                        storyActive = true;
                        _storySystem.TextDataLoad(_storyData[i].storyTextData);
                        break;
                    }
                }
                if (storyActive) break;
                if (storyKind == StoryKind.Movement)
                {
                    int indexM = UnityEngine.Random.Range(0, _MovementEventData.Count);
                    _storySystem.TextDataLoad(_MovementEventData[indexM]);
                }
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
