using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class BlurOutilnesFeature : ScriptableRendererFeature
{
    [SerializeField] BlurOutlineSettings OutlinesSettings = new BlurOutlineSettings();

    [SerializeField] private LayerMask outlinesLayerMask = ~0;
    [SerializeField] private bool useDepthMask = true;
    [SerializeField] private bool useAlphaCutoff = false;
    [SerializeField, Range(0, 1)] private float cutoff = 0.5f;

    DepthMaskPass depthMaskPass;
    ObjectsPass objectsPass;
    BlurOutlinePass blurOutlinePass;

    public override void Create()
    {
        RenderPassEvent renderPassEvent = RenderPassEvent.AfterRenderingSkybox;

        if (useDepthMask) depthMaskPass = new DepthMaskPass(renderPassEvent, outlinesLayerMask, "_SceneMask_Blur");
        objectsPass = new ObjectsPass(renderPassEvent, outlinesLayerMask, useDepthMask, useAlphaCutoff, cutoff);
        blurOutlinePass = new BlurOutlinePass(renderPassEvent, OutlinesSettings);

    }

    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
    {
        if (useDepthMask) renderer.EnqueuePass(depthMaskPass);
        renderer.EnqueuePass(objectsPass);
        renderer.EnqueuePass(blurOutlinePass);
    }
}
