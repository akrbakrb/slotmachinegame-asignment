using System.Collections;
using TMPro;
using UnityEngine;

public class PayoutSystem : MonoBehaviour
{
    [Header("Player Balance")]
    public int startingCredits = 1000;

    [Header("UI")]
    public TMP_Text cashText;

    public int CurrentCredits { get; private set; }
    public int CurrentBet { get; private set; }

    private Coroutine flashRoutine;

    private void Start()
    {
        CurrentCredits = startingCredits;
        UpdateUI();
    }

    public bool PlaceBet(int amount)
    {
        if (amount <= 0) return false;
        if (CurrentCredits < amount) return false;

        CurrentBet = amount;
        CurrentCredits -= amount;

        Flash(Color.red); // 💸 losing money flash
        UpdateUI();
        return true;
    }

    public void ProcessResult(SlotResult result)
    {
        int oldCredits = CurrentCredits;

        switch (result)
        {
            case SlotResult.SmallWin:
                CurrentCredits += CurrentBet * 2;
                break;

            case SlotResult.Jackpot:
                CurrentCredits += CurrentBet * 3;
                break;
        }

        CurrentBet = 0;

        if (CurrentCredits > oldCredits)
            Flash(Color.green);
        else if (CurrentCredits < oldCredits)
            Flash(Color.red);

        UpdateUI();
    }

    void UpdateUI()
    {
        cashText.text = CurrentCredits.ToString();
    }

    // ✨ FLASH EFFECT
    void Flash(Color flashColor)
    {
        if (flashRoutine != null)
            StopCoroutine(flashRoutine);

        flashRoutine = StartCoroutine(FlashRoutine(flashColor));
    }

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
    CurrentCredits = startingCredits;
    CurrentBet = 0;

    UpdateUI();
}
}