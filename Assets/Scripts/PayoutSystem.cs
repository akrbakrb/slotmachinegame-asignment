using System.Collections;
using TMPro;
using UnityEngine;


public class PayoutSystem : MonoBehaviour
{
    public SlotMachineManager slotMachineManager;

    [Header("Player Balance")]
    public int startingCredits = 1000;// Amount of credits the player starts with


    [Header("UI")]
    public TMP_Text cashText;

    // Read-only properties
    public int CurrentCredits { get; private set; }
    public int CurrentBet { get; private set; }

    private Coroutine flashRoutine;// Stores currently running flash effect

    private void Start()
    {
        CurrentCredits = startingCredits;
        UpdateUI();
    }

    /// <summary>
    /// Deducts credits and places a bet.
    /// Returns false if the bet is invalid or player lacks credits.
    /// </summary>
    public bool PlaceBet(int amount)
    {
        if (amount <= 0) return false;
        if (CurrentCredits < amount) return false;

        CurrentBet = amount;
        CurrentCredits -= amount;

        Flash(Color.red); //  losing money flash
        UpdateUI();
        return true;
    }


    /// <summary>
    /// Processes the slot machine result and awards winnings.
    /// </summary>
    public void ProcessResult(SlotResult result)
    {
        int oldCredits = CurrentCredits; // Save balance before payout


        switch (result)
        {
            case SlotResult.SmallWin:
                CurrentCredits += CurrentBet * 2; // Pays 2x bet
                break;

            case SlotResult.Jackpot:
                CurrentCredits += CurrentBet * 3; // Pays 3x bet
                break;
        }

        // Reset bet after round ends

        // Visual feedback for balance changes
        CurrentBet = 0;

        if (CurrentCredits > oldCredits)
            Flash(Color.green);// Win
        else if (CurrentCredits < oldCredits)
            Flash(Color.red);// Loss

        UpdateUI();
    }

    void UpdateUI()
    {
        cashText.text = CurrentCredits.ToString();
    }
    /// <summary>
    /// Starts a color flash effect on the credit text.
    /// Stops any previous flash before starting a new one.
    /// </summary>
    void Flash(Color flashColor)
    {
        if (flashRoutine != null)
            StopCoroutine(flashRoutine);

        flashRoutine = StartCoroutine(FlashRoutine(flashColor));
    }

    /// <summary>
    /// Changes text color temporarily, then restores original color.
    /// </summary>
    IEnumerator FlashRoutine(Color flashColor)
    {
        Color originalColor = cashText.color;

        cashText.color = flashColor;

        yield return new WaitForSeconds(1f);

        cashText.color = originalColor;

        flashRoutine = null;
    }

    public void RestartGame()
    {
        if (slotMachineManager != null && slotMachineManager.IsSpinning)
        return; // Don't restart while spinning
        
        CurrentCredits = startingCredits;
        CurrentBet = 0;

        UpdateUI();
    }
}