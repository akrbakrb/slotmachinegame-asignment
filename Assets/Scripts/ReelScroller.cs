using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ReelScroller : MonoBehaviour
{
    [Header("UI Slots")]
    public Image top;
    public Image middle;
    public Image bottom;

    [Header("Symbols")]
    public Sprite[] symbols;

    public int ResultIndex { get; private set; }

    private bool spinning;
    public bool IsSpinning => spinning;

    public IEnumerator Spin(float duration)
    {
        spinning = true;

        float elapsed = 0f;

        while (elapsed < duration)
        {
            // Move symbols downward
            bottom.sprite = middle.sprite;
            middle.sprite = top.sprite;
            top.sprite = RandomSymbol();

            // Slow down near the end
            float progress = elapsed / duration;
            float delay = Mathf.Lerp(0.03f, 0.15f, progress);

            yield return new WaitForSeconds(delay);

            elapsed += delay;
        }

        // Final result
        ResultIndex = Random.Range(0, symbols.Length);

        top.sprite = RandomSymbol();
        middle.sprite = symbols[ResultIndex];
        bottom.sprite = RandomSymbol();

        spinning = false;
    }

    private Sprite RandomSymbol()
    {
        return symbols[Random.Range(0, symbols.Length)];
    }
}