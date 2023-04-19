using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface Memory
{
    public void StartCutscene();
    public IEnumerator Cutscene();

    public void RecordMemory();
}
