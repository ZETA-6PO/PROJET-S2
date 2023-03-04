using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//every scripts that need to pass data between scene or so need to implement it
public interface IDataPersistence
{
    void LoadData(GameData data);
    void SaveData(ref GameData data);
}
