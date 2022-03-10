using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class EdgeOutlinesPass : ScriptableRenderPass
{
    private readonly Material material;
    private RenderTargetIdentifier cameraColorTarget;
    private RenderTargetHandle temporaryBuffer;

    public EdgeOutlinesPass(RenderPassEvent renderPassEvent, EdgeOutlinesSettings edgeOutlinesSettings)
    {
        this.renderPassEvent = renderPassEvent;

        material = new Material(Shader.Find("Hidden/EdgeDetectionOutlines"));
        material.SetFloat("_Thickness", edgeOutlinesSettings.width);
        material.SetColor("_Color", edgeOutlinesSettings.color);
        material.SetFloat("_DepthThreshold", edgeOutlinesSettings.depthThreshold);
        material.SetFloat("_SteepAngleThreshold", edgeOutlinesSettings.steepAngleThreshold);
        material.SetFloat("_SteepAngleMultiplier", edgeOutlinesSettings.steepAngleMultiplier);
        material.SetFloat("_NormalThreshold", edgeOutlinesSettings.normalThreshold);
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
            "_EdgeDetecionOutlinesBlit")))
        {
            RenderTextureDescriptor opaqueDescriptor = renderingData.cameraData.cameraTargetDescriptor;

            cmd.GetTemporaryRT(temporaryBuffer.id, opaqueDescriptor, FilterMode.Point);
            Blit(cmd, cameraColorTarget, temporaryBuffer.Identifier(), material, 0);
            Blit(cmd, temporaryBuffer.Identifier(), cameraColorTarget);
        }

        context.ExecuteCommandBuffer(cmd);
        CommandBufferPool.Release(cmd);
    }

    public override void OnCameraCleanup(CommandBuffer cmd)
    {
        cmd.ReleaseTemporaryRT(temporaryBuffer.id);
    }
}

