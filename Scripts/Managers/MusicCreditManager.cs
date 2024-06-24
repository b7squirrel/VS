using System.Collections;
using UnityEngine;

/// <summary>
/// 음악 크레딧 관리, 해당 스테이지의 음악 재생
/// </summary>
public class MusicCreditManager : MonoBehaviour
{
    [SerializeField] AudioCreditData creditData;
    [SerializeField] AudioClip panelUpSound;
    [SerializeField] AudioClip panelDownSound;
    MusicCreditUI creditUI;

    public void Init()
    {
        // 스테이지 넘버 가져오기
        PlayerDataManager playerDataManager = FindObjectOfType<PlayerDataManager>();
        int index = playerDataManager.GetCurrentStageNumber();

        // 음악 크레딧 UI 표시
        if (creditUI == null) creditUI = FindObjectOfType<MusicCreditUI>();
        string title = creditData.AudioCredits[index - 1].Title;
        string credit = title + " - " + creditData.AudioCredits[index - 1].Credit;
        StartCoroutine(ShowCreditUI(credit, index));
    }

    void PlayBGM(int _index)
    {
        AudioClip stageMusic = creditData.AudioCredits[_index - 1].Clip;
        MusicManager musicManager = FindObjectOfType<MusicManager>();
        musicManager.InitBGM(stageMusic);
    }
    IEnumerator ShowCreditUI(string _credit, int _index)
    {
        yield return new WaitForSeconds(2f);
        PlayPanelUpSound();

        PlayBGM(_index);
        creditUI.CreditFadeIn(_credit);
        yield return new WaitForSeconds(5f);
        HideCreditUI();

    }
    void HideCreditUI()
    {
        PlayPanelDownSound();
        creditUI.CreditFadeOut();
    }
    void PlayPanelUpSound()
    {
        SoundManager.instance.Play(panelUpSound);

    }
    // 애니메이션 이벤트
    public void PlayPanelDownSound()
    {
        SoundManager.instance.Play(panelDownSound);
    }
}