using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains;
using MoreMountains.Feedbacks;


public class FeelInit : MonoBehaviour
{
    public MMF_Player Name;
    public MMF_Player Subtitle;


    void Start()
    {
        Name.PlayFeedbacks();
        Subtitle.PlayFeedbacks();
    }

}
