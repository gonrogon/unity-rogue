using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace Rogue {

public class StockUI : MonoBehaviour
{
    [SerializeField]
    private TMP_Text m_entryPrefab;

    [SerializeField]
    private RectTransform m_content;

    private List<TMP_Text> m_entries = new();

    public void Refresh()
    {
        foreach(TMP_Text entry in m_entries)
        {
            Destroy(entry.gameObject);
        }
        m_entries.Clear();

        Game.Stock.StockSystem stock = Context.Stock;
        /*
        for (int i = 0; i < stock.FreeItemsCount; i++)
        {
            string name = Context.World.Send(stock.GetFreeItem(i), new Game.Msg.Name()).name;
            if (string.IsNullOrEmpty(name))
            {
                continue;
            }

            var text = Instantiate(m_entryPrefab, m_content.transform);
            m_entries.Add(text);

            text.text = name;
        }
        */
    }
}

}