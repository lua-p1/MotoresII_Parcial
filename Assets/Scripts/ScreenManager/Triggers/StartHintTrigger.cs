using System.Collections;
using UnityEngine;
public class StartHintTrigger : MonoBehaviour
{
    private void Start()
    {
        StartCoroutine(ShowHint());
    }
    private IEnumerator ShowHint()
    {
        yield return null;
        ScreenManager.Instance.Push(ScreenNames.StartHint);
    }
}
