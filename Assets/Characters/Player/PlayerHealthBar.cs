﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.Characters
{
    [RequireComponent(typeof(Image))]
    public class PlayerHealthBar : MonoBehaviour
    {

        Image healthOrb;
        Player player;

        // Use this for initialization
        void Start()
        {
            player = FindObjectOfType<Player>();
            healthOrb = GetComponent<Image>();
        }

        // Update is called once per frame
        void Update()
        {
            if (player)
            {
                healthOrb.fillAmount = player.healthAsPercentage;
            }
        }
    }
}