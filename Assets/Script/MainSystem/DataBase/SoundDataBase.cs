using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu]
public class SoundDataBase : ScriptableObject
{
    public List<AudioClip> dataList = new();
}
