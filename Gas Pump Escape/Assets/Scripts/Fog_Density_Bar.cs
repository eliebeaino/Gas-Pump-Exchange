using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Fog_Density_Bar : MonoBehaviour
{
    FogController fogController;
    [SerializeField] Image fogBar;
    float fogBaseLvl;

    // cache componenet references
    private void Awake()
    {
        fogController = GetComponentInParent<FogController>();
    }

    void Start()
    {
        // set starting values for fog
        fogBaseLvl = fogController.GetFogValue();
        fogBar.fillAmount = fogBaseLvl;
    }

    // constantly updating fog bar
    private void Update()
    {
        UpdateFogBar();
    }

    // Update health bars when hit
    public void UpdateFogBar()
    {
        // get current fog level
        float fog = fogController.GetFogValue();
        // fill the fog bar
        fogBar.fillAmount = fog;
    }
}
