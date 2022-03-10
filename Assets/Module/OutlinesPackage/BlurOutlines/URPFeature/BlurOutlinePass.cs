using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using System;

public class BlurOutlinePass : ScriptableRenderPass
{
    private readonly Material material;
    private RenderTargetIdentifier cameraColorTarget;

    private RenderTargetHandle temporaryBuffer;

    private int MaxWidth = 32;
    private float[] gaussSamples;

    private float[] GetGaussSamples(int width, float[] samples)
    {
        // NOTE: According to '3 sigma' rule there is no reason to have StdDev less then width / 3.
        // In practice blur looks best when StdDev is within range [width / 3,  width / 2].
        var stdDev = width * 0.5f;

        if (samples is null)
        {
            samples = new float[MaxWidth];
        }

        for (var i = 0; i < width; i++)
        {
            samples[i] = Gauss(i, stdDev);
        }

        return samples;
    }

    private float Gauss(float x, float stdDev)
    {
        var stdDev2 = stdDev * stdDev * 2;
        var a = 1 / Mathf.Sqrt(Mathf.PI * stdDev2);
        var gauss = a * Mathf.Pow((float)Math.E, -x * x / stdDev2);

        return gauss;
    }

    public BlurOutlinePass(RenderPassEvent renderPassEvent, BlurOutlineSettings blurOutlineSettings)
    {
        this.renderPassEvent = renderPassEvent;

        material = new Material(Shader.Find("Hidden/BlurOutlines"));
        material.SetFloat("_Intensity", blurOutlineSettings.intensity);
        material.SetColor("_Color", blurOutlineSettings.color);
        material.SetFloat("_Width", blurOutlineSettings.width);

        gaussSamples = GetGaussSamples(32, gaussSamples);
        material.SetFloatArray("_GaussSamples", gaussSamples);

        temporaryBuffer.Init("_BluerredObjectsTex");
    }

    public override void OnCameraSetup(CommandBuffer cmd, ref RenderingData renderingData)
    {
        cameraColorTarget = renderingData.cameraData.renderer.cameraColorTarget;
    }

    public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
    {
        if (!material) return;

        CommandBuffer cmd = CommandBufferPool.Get();
        using (new ProfilingScope(cmd, new ProfilingSampler(
            "_BlurOutlinesBlit")))
        {
            RenderTextureDescriptor opaqueDescriptor = renderingData.cameraData.cameraTargetDescriptor;

            cmd.GetTemporaryRT(temporaryBuffer.id, opaqueDescriptor, FilterMode.Point);

            Blit(cmd, cameraColorTarget, temporaryBuffer.Identifier(), material, 0);
            Blit(cmd, temporaryBuffer.Identifier(), cameraColorTarget, material, 1);
        }

        context.ExecuteCommandBuffer(cmd);
        CommandBufferPool.Release(cmd);
    }

    public override void OnCameraCleanup(CommandBuffer cmd)
    {
        cmd.ReleaseTemporaryRT(temporaryBuffer.id);
    }
}

