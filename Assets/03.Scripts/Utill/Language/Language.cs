using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Language
{
    private SystemLanguage m_id = SystemLanguage.English;

    private static Language m_instance;

    private List<Dictionary<string, object>> language_data;      //튜토리얼 블럭 정보

    private Action m_transformHandle;

    private Font m_fonts;

    public SystemLanguage Id
    {
        get
        {
            return this.m_id;
        }
        set
        {
            this.m_id = value;
        }
    }

    public List<Dictionary<string, object>> Language_data
    {
        get
        {
            return this.language_data;
        }
        set
        {
            this.language_data = value;
        }

    }

    public Language()
    {
        Language_data = CSVReader.Read("language");
    }


    public static Language GetInstance()
    {
        if (Language.m_instance == null)
        {
            Language.m_instance = new Language();
        }
        return Language.m_instance;
    }

    //저장 된 전체 언어 번역
    public void Set(SystemLanguage id)
    {
        this.Id = id;

        DataManager.Instance.state_Player.language = (int)this.Id;
        DataManager.Instance.Save_Player_Data();

        Action expr_29 = this.m_transformHandle;
        if (expr_29 == null)
        {
            return;
        }
        expr_29();
    }

    public static string GetText(string m_id)
    {

        SystemLanguage @int = Application.systemLanguage;

        if (DataManager.Instance.state_Player.language != -1)
        {
            @int = (SystemLanguage)DataManager.Instance.state_Player.language;

        }

        Debug.Log(m_id);

        Dictionary<string, object> data = Language.GetInstance().Language_data.Find(x => x["key"].Equals(m_id));

        if (data == null)
        {
            Debug.Log("데이터 없음");

            return null;

        }
        else
        {
            string[] sp = data[Change_language(@int)].ToString().Split('_');

            string da = "";

            for (int i = 0; i < sp.Length; i++)
            {
                if (i == (sp.Length -1))
                {
                    da += sp[i];

                }
                else
                {
                    da += sp[i] + "\n";

                }

            }

            Debug.Log(da);

            return da;

        }

    }

    public Font GetFont()
    {
        if (m_fonts == null)
        {
            this.m_fonts = this.LoadFont();

        }

        //Debug.Log(m_instance.m_fonts.name);
        return m_instance.m_fonts;
    }

    private Font LoadFont()
    {

        SystemLanguage id = Application.systemLanguage;

        if (DataManager.Instance.state_Player.language != -1)
        {
            id = (SystemLanguage)DataManager.Instance.state_Player.language;

        }

        string path;

        if (id == SystemLanguage.Korean)
        {
            path = "font/Binggrae-Bold";
        }
        else
        {
            path = "font/NotoSans-Bold";
        }
        //Debug.Log("언어path " + path);

        return Resources.Load(path) as Font;
    }

    public void AddEvent(Action e)
    {
        this.m_transformHandle += e;
    }

    public void RemoveEvent(Action e)
    {
        this.m_transformHandle -= e;
    }

    public static string Change_language(SystemLanguage sl)
    {

        switch (sl)
        {
            case UnityEngine.SystemLanguage.Afrikaans:
            case UnityEngine.SystemLanguage.Arabic:
            case UnityEngine.SystemLanguage.Basque:
            case UnityEngine.SystemLanguage.Belarusian:
            case UnityEngine.SystemLanguage.Bulgarian:
            case UnityEngine.SystemLanguage.Catalan:
            case UnityEngine.SystemLanguage.Czech:
            case UnityEngine.SystemLanguage.Danish:
            case UnityEngine.SystemLanguage.Dutch:
            case UnityEngine.SystemLanguage.Estonian:
            case UnityEngine.SystemLanguage.Faroese:
            case UnityEngine.SystemLanguage.Finnish:
            case UnityEngine.SystemLanguage.English:
            case UnityEngine.SystemLanguage.Greek:
            case UnityEngine.SystemLanguage.Hebrew:
            case UnityEngine.SystemLanguage.Hungarian:
            case UnityEngine.SystemLanguage.Icelandic:
            case UnityEngine.SystemLanguage.Latvian:
            case UnityEngine.SystemLanguage.Lithuanian:
            case UnityEngine.SystemLanguage.Norwegian:
            case UnityEngine.SystemLanguage.Polish:
            case UnityEngine.SystemLanguage.Romanian:
            case UnityEngine.SystemLanguage.SerboCroatian:
            case UnityEngine.SystemLanguage.Slovak:
            case UnityEngine.SystemLanguage.Slovenian:
            case UnityEngine.SystemLanguage.Swedish:
            case UnityEngine.SystemLanguage.Ukrainian:
            case UnityEngine.SystemLanguage.Unknown:
                return "English";
            case UnityEngine.SystemLanguage.Thai:

                return "Thai";
            case UnityEngine.SystemLanguage.Italian:

                return "Italian";
            case UnityEngine.SystemLanguage.Turkish:

                return "Turkish";

            case UnityEngine.SystemLanguage.French:

                return "French";
            case UnityEngine.SystemLanguage.German:

                return "German";
            case UnityEngine.SystemLanguage.Indonesian:

                return "Indonesian";
            case UnityEngine.SystemLanguage.Japanese:

                return "Japanese";
            case UnityEngine.SystemLanguage.Korean:

                return "Korean";
            case UnityEngine.SystemLanguage.Portuguese:

                return "Portuguese";
            case UnityEngine.SystemLanguage.Russian:

                return "Russian";
            case UnityEngine.SystemLanguage.Spanish:

                return "Spanish";
            case UnityEngine.SystemLanguage.Vietnamese:

                return "Vietnamese";
            case UnityEngine.SystemLanguage.ChineseSimplified:
            case UnityEngine.SystemLanguage.Chinese:

                return "Chinese_Sim";
            case UnityEngine.SystemLanguage.ChineseTraditional:

                return "Chinese_Tra";
            default:

                return "Hindi";
        }

    }
}
