using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ReelScroller : MonoBehaviour
{
    [Header("UI Slots")]
    public Image top; // Top visible symbol
    public Image middle; // Middle visible symbol (winning position)
    public Image bottom;// Bottom visible symbol

    [Header("Symbols")]
    public Sprite[] symbols;// All possible slot machine symbols

    // Stores the final symbol index after the reel stops
    public int ResultIndex { get; private set; }

    private bool spinning;
    public bool IsSpinning => spinning;


    /// <summary>
    /// Spins the reel for the specified duration.
    /// Continuously scrolls symbols and then selects a final result.
    /// </summary>
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

        // -------------------------------
        // RANDOM NUMBER GENERATION
        // -------------------------------
        // Generates a random index between 0 and symbols.Length - 1.
        // This random number determines the final symbol that the reel
        // lands on and is later used by SlotMachineManager to check wins.
        ResultIndex = Random.Range(0, symbols.Length);

        top.sprite = RandomSymbol();
        middle.sprite = symbols[ResultIndex];
        bottom.sprite = RandomSymbol();

        spinning = false;
    }


    /// <summary>
    /// Returns a randomly selected symbol from the symbols array.
    /// Used while the reel is spinning to create the animation effect.
    /// </summary>
    private Sprite RandomSymbol()
    {
        return symbols[Random.Range(0, symbols.Length)];
    }
}