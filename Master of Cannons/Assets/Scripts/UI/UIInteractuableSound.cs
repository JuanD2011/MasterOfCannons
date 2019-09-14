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
                AudioManager.instance.PlaySFx(AudioManager.instance.audioClips.defaultButton, 1f, false);
                break;
            case ButtonSoundType.Accept:
                AudioManager.instance.PlaySFx(AudioManager.instance.audioClips.acceptButton, 1f, false);
                break;
            case ButtonSoundType.Back:
                AudioManager.instance.PlaySFx(AudioManager.instance.audioClips.cancelButton, 1f, false);
                break;
            default:
                break;
        }
    }
}
