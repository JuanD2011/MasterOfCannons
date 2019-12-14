using UnityEngine;
using UnityEngine.EventSystems;

public class UIInteractuableSound : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] ButtonSoundType m_ButtonSoundType = ButtonSoundType.Default;

    public void OnPointerClick(PointerEventData eventData)
    {
        PlaySound();
    }

    private void PlaySound()
    {
        switch (m_ButtonSoundType)
        {
            case ButtonSoundType.Default:
                AudioManager.Instance.PlaySFx(AudioManager.Instance.audioClips.defaultButton, 1f, false);
                break;
            case ButtonSoundType.Accept:
                AudioManager.Instance.PlaySFx(AudioManager.Instance.audioClips.acceptButton, 1f, false);
                break;
            case ButtonSoundType.Back:
                AudioManager.Instance.PlaySFx(AudioManager.Instance.audioClips.cancelButton, 1f, false);
                break;
            default:
                break;
        }
    }
}
