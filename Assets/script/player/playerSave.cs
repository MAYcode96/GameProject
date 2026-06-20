using UnityEngine;
using System.Collections;

public class AutoSavePlayer : MonoBehaviour
{
    
    public float memoryUpdateInterval = 1f;
    public float diskWriteInterval = 5f;

    private float diskTimer;

    void Start()
    {
        diskTimer = diskWriteInterval;

        gameObject.tag = "player";

        StartCoroutine(AutoSaveRoutine());
    }
    //AUTO SAVE POSITION
    IEnumerator AutoSaveRoutine()
    {
        yield return new WaitForSeconds(1.0f);

        while (true)
        {
            if (GameManager.Instance != null)
            {
                diskTimer -= memoryUpdateInterval;

                if (diskTimer <= 0)
                {
                    
                    GameManager.Instance.SavePlayerPosition(transform, true);
                    diskTimer = diskWriteInterval; 
                }
                else
                {
                  
                    GameManager.Instance.SavePlayerPosition(transform, false); 
                }
            }

            yield return new WaitForSeconds(memoryUpdateInterval);
        }
    }
}