using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DamagePopup : MonoBehaviour
{
    private TextMeshPro textMesh;
    private float disappearTimer;
    private Color textColor;

    private const float DISSAPPEAR_TIMER_MAX = 1f;

    private void Awake()
    {
        textMesh = transform.GetComponent<TextMeshPro>();
    }

    public void Setup(int damage)
    {
        textMesh.SetText("-" + damage.ToString());
        textColor = textMesh.color;
        disappearTimer = DISSAPPEAR_TIMER_MAX;
    }

    private void Update()
    {
        float moveYSpeed = 0.3f;
        transform.position += new Vector3(0, moveYSpeed) * Time.deltaTime;

        if (disappearTimer > DISSAPPEAR_TIMER_MAX * 0.5f)
        {
            // text to dan
            float increaseScaleAmount = 1f;
            transform.localScale += Vector3.one * increaseScaleAmount * Time.deltaTime;
        } else
        {
            // text nho dan
            float decreaseScaleAmount = 1f;
            transform.localScale -= Vector3.one * decreaseScaleAmount * Time.deltaTime;
        }

        disappearTimer -= Time.deltaTime;
        if (disappearTimer < 0)
        {
            // Bat dau bien mat
            float disappearSpeed = 3f;
            textColor.a -= disappearSpeed * Time.deltaTime;
            textMesh.color = textColor;
            if ( textColor.a < 0)
            {
                Destroy(gameObject);
            }
        }
    }
}
