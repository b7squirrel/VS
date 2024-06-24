using System.Collections;
using UnityEngine;
using UnityEngine.TextCore.Text;

/// <summary>
/// ���� ũ���� ����, �ش� ���������� ���� ���
/// </summary>
public class MusicCreditManager : MonoBehaviour
{
    [SerializeField] AudioCreditData creditData;
    [SerializeField] AudioClip panelUpSound;
    [SerializeField] AudioClip panelDownSound;
    MusicCreditUI creditUI;

    public void Init()
    {
        // �������� �ѹ� ��������
        PlayerDataManager playerDataManager = FindObjectOfType<PlayerDataManager>();
        StageEvenetManager eventManager = FindObjectOfType<StageEvenetManager>();
        StageMusicType musicType = eventManager.GetStageMusicType();
        int index = 0;
        for (int i = 0; i < creditData.AudioCredits.Length; i++)
        {
            if (creditData.AudioCredits[i].MusicType == musicType)
                index = i;
        }

        //int index = playerDataManager.GetCurrentStageNumber();

        // ���� ũ���� UI ǥ��
        if (creditUI == null) creditUI = FindObjectOfType<MusicCreditUI>();
        string title = creditData.AudioCredits[index].Title;
        string credit = title + " - " + creditData.AudioCredits[index].Credit;
        StartCoroutine(ShowCreditUI(credit, index));
    }

    void PlayBGM(int _index)
    {
        AudioClip stageMusic = creditData.AudioCredits[_index].Clip;
        MusicManager musicManager = FindObjectOfType<MusicManager>();
        musicManager.InitBGM(stageMusic);
    }
    IEnumerator ShowCreditUI(string _credit, int _index)
    {
        yield return new WaitForSeconds(2f);
        creditUI.CreditFadeIn(_credit);

        yield return new WaitForSeconds(.5f); // �г��� �ö���� ���� ���� ���
        PlayPanelUpSound();

        yield return new WaitForSeconds(1f); // �г� ����� ������ ���ÿ� ��ġ�鼭 ������ �ʰ�
        PlayBGM(_index);

        yield return new WaitForSeconds(5f); // 5�� �Ŀ� �г� ����
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
    // �ִϸ��̼� �̺�Ʈ
    public void PlayPanelDownSound()
    {
        SoundManager.instance.Play(panelDownSound);
    }
}