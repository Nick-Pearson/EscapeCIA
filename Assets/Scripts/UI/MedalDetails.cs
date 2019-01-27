using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MedalDetails : MonoBehaviour
{
    public Text Title;
    public Text Description;
    public Image Icon;
    
    public void SetDetails(MedalBase Medal)
    {
        Title.text = Medal.MedalName;
        Description.text = Medal.MedalDescription;
        Icon.sprite = Medal.MedalIcon;
    }
}
