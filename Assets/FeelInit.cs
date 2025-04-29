using MoreMountains.Feedbacks;
using System.Collections;
using UnityEngine;


public class FeelInit : MonoBehaviour
{
    public MMF_Player Name;
    public MMF_Player Subtitle;

    public GameObject instructions;

    private void Awake()
    {
        instructions.SetActive(false);
        Name.gameObject.SetActive(false);
        Subtitle.gameObject.SetActive(false);
    }

    void Start()
    {
        StartCoroutine("PlayFeedbacksOverTime");
    }

    private IEnumerator PlayFeedbacksOverTime()
    {
        Name.gameObject.SetActive(true);
        Name.ResetFeedbacks();
        Name.Initialization();
        Name.PlayFeedbacks();
        yield return new WaitForSecondsRealtime(4f);
        instructions.SetActive(true);
        Subtitle.gameObject.SetActive(true);
        Subtitle.PlayFeedbacks();
    }

}
