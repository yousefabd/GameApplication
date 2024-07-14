using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TDPlayer : MonoBehaviour
{
    [SerializeField] private Transform gameOver;
    [SerializeField] private TextMeshProUGUI wavesNumberText;
    [SerializeField] private Transform slash;
    private float damageRadius = 0.2f;
    private float damageValue = 20f;
    private void Awake()
    {
        gameOver.gameObject.SetActive(false);
    }
    private void Start()
    {
        TDCastle.Instance.OnGameOver += Castle_OnGameOver;
        ScreenInteractionManager.Instance.OnAreaSelected += ScreenInteractionManager_OnAreaSelected;
    }

    private void ScreenInteractionManager_OnAreaSelected(Vector3 arg1, Vector3 mousePosition)
    {
        DamageUnit(mousePosition);
    }

    private void Castle_OnGameOver(int wavesNum)
    {
        gameOver.gameObject.SetActive(true);
        wavesNumberText.text = wavesNum.ToString();
        Time.timeScale = 0f;
    }
    private void DamageUnit(Vector3 position)
    {
        Instantiate(slash, position, Quaternion.identity);
        Collider2D collider=Physics2D.OverlapCircle(position, damageRadius);
        if (collider == null)
            return;
        if(collider.TryGetComponent(out Unit unit))
        {
            if (unit.GetTeam() == Team.GOBLINS)
                unit.Damage(unit.transform.position, damageValue);
        }
    }
}
