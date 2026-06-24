using System.Collections;
using UnityEngine;
using TMPro;

public class SlotMachineManager : MonoBehaviour
{
    [Header("Spin Indicator")]
    public GameObject offImage;
    public GameObject onImage;

    [Header("Reels")]
    public ReelScroller reel1;
    public ReelScroller reel2;
    public ReelScroller reel3;

    [Header("UI")]
    public TMP_Text resultText;

    [Header("Payout")]
    public PayoutSystem payoutSystem;

    [Header("Spin Settings")]
    public float reel1Time = 2f;
    public float reel2Time = 2.5f;
    public float reel3Time = 3f;

    private bool isSpinning;

    public void SpinWithBet(int betAmount)
    {
        if (isSpinning)
            return;

        if (payoutSystem == null || resultText == null)
        {
            Debug.LogError("Missing references in SlotMachineManager!");
            return;
        }

        if (!payoutSystem.PlaceBet(betAmount))
        {
            resultText.text = "NOT ENOUGH CASH";
            return;
        }

        StartCoroutine(SpinRoutine());
    }

    private IEnumerator SpinRoutine()
    {
        isSpinning = true;

        PinHandle(true);
        resultText.text = "SPINNING...";

        // Start all reels together
        StartCoroutine(reel1.Spin(reel1Time));
        StartCoroutine(reel2.Spin(reel2Time));
        StartCoroutine(reel3.Spin(reel3Time));

        // Wait until all reels stop
        yield return new WaitUntil(() =>
            !reel1.IsSpinning &&
            !reel2.IsSpinning &&
            !reel3.IsSpinning);

        CheckResult();

        PinHandle(false);
        isSpinning = false;
    }

    private void CheckResult()
    {
        int r1 = reel1.ResultIndex;
        int r2 = reel2.ResultIndex;
        int r3 = reel3.ResultIndex;

        Debug.Log($"Results: {r1}, {r2}, {r3}");

        SlotResult result;

        if (r1 == r2 && r2 == r3)
        {
            resultText.text = "🔥 JACKPOT!";
            result = SlotResult.Jackpot;
        }
        else if (r1 == r2 || r2 == r3 || r1 == r3)
        {
            resultText.text = "✨ SMALL WIN!";
            result = SlotResult.SmallWin;
        }
        else
        {
            resultText.text = "TRY AGAIN";
            result = SlotResult.TryAgain;
        }

        payoutSystem.ProcessResult(result);
    }


    // Added Handle animation
    private void PinHandle(bool spinning)
    {
        if (offImage != null)
            offImage.SetActive(!spinning);

        if (onImage != null)
            onImage.SetActive(spinning);
    }
}