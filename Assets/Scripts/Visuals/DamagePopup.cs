using TMPro;
using UnityEngine;

public class DamagePopup : MonoBehaviour
{
    [SerializeField] private TextMeshPro textMesh;
    private Color textColor;
    private float disappearTimer;
    public static DamagePopup Create(Vector3 position, Transform prefab, int damageAmount)
    {
        Transform damagePopupTransform = Instantiate(prefab, position, Quaternion.identity);
        DamagePopup damagePopup = damagePopupTransform.GetComponent<DamagePopup>();
        damagePopup.Setup(damageAmount);
        return damagePopup;
    }
    private void Awake()
    {
        textMesh = GetComponent<TextMeshPro>();
    }
    private void Setup(int damageAmount)
    {
        textMesh.SetText(damageAmount.ToString());
        textColor = textMesh.color;
        disappearTimer = 0.5f;
    }
    private void Update()
    {
        float moveYSpeed = 0.5f;
        transform.position += new Vector3(0, moveYSpeed) * Time.deltaTime;
        disappearTimer -= Time.deltaTime;
        if (disappearTimer < 0f)
        {
            float disappearSpeed = 3f;
            textColor.a -= disappearSpeed * Time.deltaTime;
            textMesh.color = textColor;
            if (textColor.a < 0f)
            {
                Destroy(gameObject);
            }
        }
    }
}
