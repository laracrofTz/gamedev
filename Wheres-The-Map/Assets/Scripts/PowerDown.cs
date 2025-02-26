using UnityEngine;

public class PowerDown : MonoBehaviour
{

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnMouseDown()
    {
        //GameEvents.OnPlayPowerDownSound();
        GameEvents.PowerDownClicked();
    }
}
