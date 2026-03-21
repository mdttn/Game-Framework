using System.Collections;
using TMPro;
using UnityEngine;

public class Tutorial : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI displayer;

    [Space]
    [SerializeField] private float messageDisplayTime;
    [SerializeField] private string[] messages;
    
    private IEnumerator showMessages;

#if UNITY_EDITOR
    private void OnValidate()
    {
        messageDisplayTime = Mathf.Clamp(messageDisplayTime, 1f, float.MaxValue);
    }

#endif

    private void Awake()
    {
        SetDisplayerText(string.Empty);
    }

    private void Start()
    {
        showMessages = ShowMessages();
        StartCoroutine(showMessages);
    }

    private void OnDisable()
    {
        SetDisplayerText(string.Empty);
        if(showMessages != null) StopCoroutine(showMessages);
    }

    private void SetDisplayerVisibility(bool isVisible)
    {
        if (displayer != null) 
            displayer.gameObject.SetActive(isVisible);
    }

    private void SetDisplayerText(string text) {
        if(displayer != null)  displayer.text = text;
    }

    private IEnumerator ShowMessages()
    {
        WaitForSeconds wait = new WaitForSeconds(messageDisplayTime);
        SetDisplayerVisibility(true);

        for (int i = 0; i < messages.Length; i++)
        {
            SetDisplayerText(messages[i]);
            yield return wait;
        }

        SetDisplayerVisibility(false);
    }
}
