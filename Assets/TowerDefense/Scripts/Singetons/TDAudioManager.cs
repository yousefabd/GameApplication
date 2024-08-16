using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TDAudioManager : MonoBehaviour
{
    [SerializeField] private AudioRefsSO audioRefs;

    private void Start()
    {
        TDGameManager.Instance.OnSecondPassed += Instance_OnSecondPassed;
        TDUnitSpawner.Instance.OnUnitDestroyed += Instance_OnUnitDestroyed;
        TDCurrencyManager.Instance.OnBuy += Instance_OnBuy;
    }

    private void Instance_OnBuy()
    {
        PlaySound(audioRefs.buy, Camera.main.transform.position);
    }

    private void Instance_OnUnitDestroyed(float arg1, Vector3 position)
    {
        PlaySound(audioRefs.goblinDeaths[Random.Range(0, audioRefs.goblinDeaths.Length)],position);
    }

    private void Instance_OnSecondPassed(float currentTime)
    {
        if(currentTime < 1.5f)
        {
            PlaySound(audioRefs.countdown[1], Camera.main.transform.position);
        }
        else
        {
            PlaySound(audioRefs.countdown[0], Camera.main.transform.position);
        }
    }

    private void PlaySound(AudioClip audioClip,Vector3 position,float volume = 1f)
    {
        AudioSource.PlayClipAtPoint(audioClip,position,volume);
    }
}
