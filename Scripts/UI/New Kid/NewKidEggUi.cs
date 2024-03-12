using UnityEngine;

public class NewKidEggUi : MonoBehaviour
{
    [SerializeField] AudioClip newFriendTextSound;

    // animation event
    // Egg Panel Manager에 애니메이션이 끝났음을 알리고 자신을 비활성화
    public void AnimFinished()
    {
        GameManager.instance.eggPanelManager.EggAnimFinished();
        GameManager.instance.eggPanelManager.EggImageUp(false);
    }

    // animation event
    // New Friend 텍스트가 삥 하는 소리
    public void PlayNewFriendSound()
    {
        SoundManager.instance.Play(newFriendTextSound);
    }
}