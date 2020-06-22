using UnityEngine;
using UnityEngine.UI;

using System.Collections;
using System;

public enum Music_Sound
{
    title_bg,
    ingame_bg

}

public enum Effect_Sound
{
    button_circle,
    button_square,
    button_rectangle,
    button_octagon,
    button_close,
    button_soft,
    popup_open,
    popup_result_clear,
    popup_result_fail,
    popup_result_draw,
    map_tiles_created,
    block_touch,
    block_drop,
    destruction,
    score_x2_item,
    refresh_item,
    rotation_item,
    bomb_item,
    undo_item,
    stage_clear_star,
    gift_oppen,
    Product_purchase_complete,
    Good,
    Awesome,
    Excelleent
}

[RequireComponent(typeof(AudioSource))]
public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [HideInInspector] public bool isEffectEnabled = true;   //이펙트 사운드 온오프
    [HideInInspector] public bool isMusicEnabled = true;    //배경음 사운드 온오프

    [SerializeField] public AudioSource effectSource;                        //이펙트 오디오 소스
    [SerializeField] public AudioSource musicSource;                         //배경음 오디오 소스 

    [SerializeField] private AudioClip title_bg;
    [SerializeField] private AudioClip ingame_bg;

    [SerializeField] private AudioClip button_circle;
    [SerializeField] private AudioClip button_square;
    [SerializeField] private AudioClip button_rectangle;
    [SerializeField] private AudioClip button_octagon;
    [SerializeField] private AudioClip button_close;
    [SerializeField] private AudioClip button_soft;
    [SerializeField] private AudioClip popup_open;
    [SerializeField] private AudioClip popup_result_clear;
    [SerializeField] private AudioClip popup_result_fail;
    [SerializeField] private AudioClip popup_result_draw;
    [SerializeField] private AudioClip map_tiles_created;
    [SerializeField] private AudioClip block_touch;
    [SerializeField] private AudioClip block_drop;
    [SerializeField] private AudioClip destruction;
    [SerializeField] private AudioClip score_x2_item;
    [SerializeField] private AudioClip refresh_item;
    [SerializeField] private AudioClip rotation_item;
    [SerializeField] private AudioClip bomb_item;
    [SerializeField] private AudioClip undo_item;
    [SerializeField] private AudioClip stage_clear_star;
    [SerializeField] private AudioClip gift_oppen;
    [SerializeField] private AudioClip Product_purchase_complete;
    [SerializeField] private AudioClip Good;
    [SerializeField] private AudioClip Awesome;
    [SerializeField] private AudioClip Excelleent;

    private void Awake()
    {
        instance = this;
    }

    /// <summary>
    /// Raises the enable event.
    /// </summary>
    void OnEnable()
    {
        InitAudioStatus();
    }

    /// <summary>
    /// 오디오 정보 세팅
    /// </summary>
    public void InitAudioStatus()
    {
        isEffectEnabled = (PlayerPrefs.GetInt("isSoundEnabled", 0) == 0) ? true : false;
        isMusicEnabled = (PlayerPrefs.GetInt("isMusicEnabled", 0) == 0) ? true : false;

        ChangeEffectState();
        ChangeMusicState();

      
    }

    public void Play_Music_Sound(Music_Sound clip)
    {

        switch (clip)
        {
            case Music_Sound.title_bg:
                musicSource.clip = title_bg;

                break;
            case Music_Sound.ingame_bg:
                musicSource.clip = ingame_bg;

                break;
            default:
                break;
        }
        Debug.Log("Musicsdssdsd");
        musicSource.Play();
    }

    /// <summary>
    /// 이펙트 사운드 플레이
    /// </summary>
    /// <param name="clip">Clip.</param>
    public void Play_Effect_Sound(Effect_Sound clip)
    {
        AudioClip effect = null;

        if (isEffectEnabled)
        {
            switch (clip)
            {
                case Effect_Sound.button_circle:
                    effect = button_circle;

                    break;
                case Effect_Sound.button_square:
                    effect = button_square;

                    break;
                case Effect_Sound.button_rectangle:
                    effect = button_rectangle;

                    break;
                case Effect_Sound.button_octagon:
                    effect = button_octagon;

                    break;
                case Effect_Sound.button_close:
                    effect = button_close;

                    break;
                case Effect_Sound.button_soft:
                    effect = button_soft;

                    break;
                case Effect_Sound.popup_open:
                    effect = popup_open;

                    break;
                case Effect_Sound.popup_result_clear:
                    effect = popup_result_clear;

                    break;
                case Effect_Sound.popup_result_fail:
                    effect = popup_result_fail;

                    break;
                case Effect_Sound.popup_result_draw:
                    effect = popup_result_draw;

                    break;
                case Effect_Sound.map_tiles_created:
                    effect = map_tiles_created;

                    break;
                case Effect_Sound.block_touch:
                    effect = block_touch;

                    break;
                case Effect_Sound.block_drop:
                    effect = block_drop;

                    break;
                case Effect_Sound.destruction:
                    effect = destruction;

                    break;
             
                case Effect_Sound.score_x2_item:
                    effect = score_x2_item;

                    break;
                case Effect_Sound.refresh_item:
                    effect = refresh_item;

                    break;
                case Effect_Sound.rotation_item:
                    effect = rotation_item;

                    break;
                case Effect_Sound.bomb_item:
                    effect = bomb_item;

                    break;

                case Effect_Sound.undo_item:
                    effect = undo_item;

                    break;
                case Effect_Sound.stage_clear_star:
                    effect = stage_clear_star;

                    break;
                    
                case Effect_Sound.gift_oppen:
                    effect = gift_oppen;

                    break;
                case Effect_Sound.Product_purchase_complete:
                    effect = Product_purchase_complete;

                    break;
                case Effect_Sound.Good:
                    effect = Good;

                    break;
                case Effect_Sound.Awesome:
                    effect = Awesome;

                    break;
                case Effect_Sound.Excelleent:
                    effect = Excelleent;

                    break;

                default:
                    break;
            }

            effectSource.PlayOneShot(effect);
        }
    }

    /// <summary>
    /// 이펙트 토글 상태
    /// </summary>
    public void ToggleEffectStatus()
    {
        isEffectEnabled = (isEffectEnabled) ? false : true;
        PlayerPrefs.SetInt("isSoundEnabled", (isEffectEnabled) ? 0 : 1);
        ChangeEffectState();
        if (isMusicEnabled)
        {

            if (UIManager.Instance.game_Stat.Equals(Game_Stat.Main))
            {
                FireBaseManager.Instance.LogEvent("Setting_Effect_On");

            }
            else
            {
                switch (GamePlay.instance.gameMode)
                {
                    case GameMode.Classic:
                        FireBaseManager.Instance.LogEvent("Classic_Pause_Effect_On");

                        break;
                    case GameMode.Stage:
                        FireBaseManager.Instance.LogEvent("Stage_Pause_Effect_On");

                        break;
                    case GameMode.Multi:
                        break;
                    default:
                        break;
                }
            }


        }
        else
        {
            if (UIManager.Instance.game_Stat.Equals(Game_Stat.Main))
            {
                FireBaseManager.Instance.LogEvent("Setting_Effect_Off");

            }
            else
            {
                switch (GamePlay.instance.gameMode)
                {
                    case GameMode.Classic:
                        FireBaseManager.Instance.LogEvent("Classic_Pause_Effect_Off");

                        break;
                    case GameMode.Stage:
                        FireBaseManager.Instance.LogEvent("Stage_Pause_Effect_Off");

                        break;
                    case GameMode.Multi:
                        break;
                    default:
                        break;
                }
            }

        }
    }

    /// <summary>
    /// 배경 토글 상태
    /// </summary>
    public void ToggleMusicStatus()
    {
        isMusicEnabled = (isMusicEnabled) ? false : true;
        PlayerPrefs.SetInt("isMusicEnabled", (isMusicEnabled) ? 0 : 1);
        ChangeMusicState();

        if (isMusicEnabled)
        {

            if (UIManager.Instance.game_Stat.Equals(Game_Stat.Main))
            {
                FireBaseManager.Instance.LogEvent("Setting_Music_On");

            }
            else
            {
                switch (GamePlay.instance.gameMode)
                {
                    case GameMode.Classic:
                        FireBaseManager.Instance.LogEvent("Classic_Pause_Music_On");

                        break;
                    case GameMode.Stage:
                        FireBaseManager.Instance.LogEvent("Stage_Pause_Music_On");

                        break;
                    case GameMode.Multi:
                        break;
                    default:
                        break;
                }
            }
           

        }
        else
        {
            if (UIManager.Instance.game_Stat.Equals(Game_Stat.Main))
            {
                FireBaseManager.Instance.LogEvent("Setting_Music_Off");

            }
            else
            {
                switch (GamePlay.instance.gameMode)
                {
                    case GameMode.Classic:
                        FireBaseManager.Instance.LogEvent("Classic_Pause_Music_Off");

                        break;
                    case GameMode.Stage:
                        FireBaseManager.Instance.LogEvent("Stage_Pause_Music_Off");

                        break;
                    case GameMode.Multi:
                        break;
                    default:
                        break;
                }
            }

        }

    }

    /// <summary>
    /// 이펙트 상태에 따른 교체
    /// </summary>
    public void ChangeEffectState()
    {
        effectSource.mute = !isEffectEnabled;
        UIManager.Instance.Set_Music_Sprite();

    }

    /// <summary>
    /// 배경 상태에 따른 교체
    /// </summary>
    public void ChangeMusicState()
    {
        musicSource.mute = !isMusicEnabled;
        UIManager.Instance.Set_Music_Sprite();

    }


}
