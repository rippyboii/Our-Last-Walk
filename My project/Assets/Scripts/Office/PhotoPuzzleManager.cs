using System.Collections;
using UnityEngine;

// Attach to the PuzzleManager GameObject.
// Validates photo order and triggers the flashback when all 4 are correct.
public class PhotoPuzzleManager : MonoBehaviour
{
    private readonly string[] correctOrder = { "dating", "child", "divorce", "dog" };

    public WallFrameSlot[] slots;             // assign all 4 in Inspector (index 0 = leftmost)
    public FlashbackManager flashbackManager;

    private bool solved;

    public void CheckSolution()
    {
        if (solved) return;

        // TEST: only require child photo in slot 1
        if (slots[1].currentPhotoId != "child") return;

        solved = true;
        foreach (WallFrameSlot slot in slots)
            slot.SetLocked(true);

        StartCoroutine(TriggerFlashback());
    }

    IEnumerator TriggerFlashback()
    {
        yield return new WaitForSeconds(0.5f);
        flashbackManager.PlayFlashback();
    }
}
