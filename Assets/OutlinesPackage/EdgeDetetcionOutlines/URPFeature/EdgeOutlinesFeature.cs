using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using System.Collections.Generic;

public class EdgeOutlinesFeature : ScriptableRendererFeature
{
    //[SerializeField] private Material edgeDetecionOutlineMaterial;
    [SerializeField] EdgeOutlinesSettings OutlinesSettings = new EdgeOutlinesSettings();

    [SerializeField] private LayerMask outlinesLayerMask = ~0;
    [SerializeField] private bool useDepthMask = true;
    [SerializeField] private bool useAlphaCutoff = false;
    [SerializeField, Range(0, 1)] private float cutoff = 0.5f;

    DepthMaskPass depthMaskPass;
    DepthNormalOnlyPass depthNormalOnlyPass;
    EdgeOutlinesPass edgeOutlinesPass;

    public override void Create()
    {
        RenderPassEvent renderPassEvent = RenderPassEvent.AfterRenderingSkybox;

        if(useDepthMask) depthMaskPass = new DepthMaskPass(renderPassEvent, outlinesLayerMask, "_SceneMask_Edge");
        depthNormalOnlyPass = new DepthNormalOnlyPass(renderPassEvent, outlinesLayerMask, useDepthMask, useAlphaCutoff, cutoff);
        edgeOutlinesPass = new EdgeOutlinesPass(renderPassEvent, OutlinesSettings);

    }

    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
    {
        if (useDepthMask) renderer.EnqueuePass(depthMaskPass);
        renderer.EnqueuePass(depthNormalOnlyPass);
        renderer.EnqueuePass(edgeOutlinesPass);
    }
}


