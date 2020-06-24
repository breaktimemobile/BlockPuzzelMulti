using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LanguageComponent : MonoBehaviour
{
    private Text m_text;

    [Tooltip("텍스트 찾을 아이디")]
    public string m_id = "TXT_NO_10001";

    private void Start()
    {
        this.m_text = base.GetComponent<Text>();
        this.Set();
        Language.GetInstance().AddEvent(new Action(this.TransformLanuage));
    }


    private void OnDestroy()
    {
        Language.GetInstance().RemoveEvent(new Action(this.TransformLanuage));
    }

    private void Set()
    {
        if (this.m_text == null)
        {
            return;
        }

        if (m_id == "TXT_NO_10061")
        {
            
            this.m_text.text = transform.parent.name.Split('_')[2] + Language.GetText(m_id);

        }
        else
        {
            this.m_text.text = Language.GetText(m_id);

        }

        this.m_text.font = Language.GetInstance().GetFont();

    }

    public void SetText(string id)
    {
        this.m_id = id;
        this.Set();
    }

    private void TransformLanuage()
    {
        this.SetText(this.m_id);
    }
}
