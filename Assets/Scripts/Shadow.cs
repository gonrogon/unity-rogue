using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rogue 
{
    public class Shadow : MonoBehaviour
    {
        private MeshFilter m_filter;

        private MeshRenderer m_renderer;

        private void Awake()
        {
            m_filter   = GetComponent<MeshFilter>();
            m_renderer = GetComponent<MeshRenderer>();
        }

        private void Start()
        {
            
        }
    }
}