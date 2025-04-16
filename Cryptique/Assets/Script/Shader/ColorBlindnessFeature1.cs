using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using System.Collections.Generic;

public class ColorBlindnessFeature : ScriptableRendererFeature
{
    [Header("Référence au ScriptableObject des matériaux de daltonisme")]
    [SerializeField] private ColorBlindnessMaterials materialsObject;

    private ColorBlindnessPass pass;

    public override void Create()
    {
        if (materialsObject != null && materialsObject.materials.Count > 0)
        {
            pass = new ColorBlindnessPass(materialsObject.materials);
        }
        else
        {
            Debug.LogWarning("Le ScriptableObject des matériaux est vide ou non assigné.");
        }
    }

    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
    {
        if (pass != null)
        {
            renderer.EnqueuePass(pass);
        }
    }

    public List<Material> GetMaterials()
    {
        return materialsObject != null ? materialsObject.materials : new List<Material>();
    }

    class ColorBlindnessPass : ScriptableRenderPass
    {
        private List<Material> materials;
        private RTHandle tempTexture;

        public ColorBlindnessPass(List<Material> materials)
        {
            this.materials = materials;
            renderPassEvent = RenderPassEvent.AfterRenderingTransparents;
        }

        public override void Configure(CommandBuffer cmd, RenderTextureDescriptor cameraTextureDescriptor)
        {
            RenderingUtils.ReAllocateIfNeeded(ref tempTexture, cameraTextureDescriptor, FilterMode.Bilinear, name: "_TemporaryColorTexture");
            ConfigureTarget(tempTexture);
        }

        public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
        {
            if (materials == null || materials.Count == 0 || tempTexture == null) return;

            CommandBuffer cmd = CommandBufferPool.Get("ColorBlindnessEffect");

            foreach (var material in materials)
            {
                if (material != null)
                {
                    Blitter.BlitCameraTexture(cmd, renderingData.cameraData.renderer.cameraColorTargetHandle, tempTexture, material, 0);
                }
            }

            context.ExecuteCommandBuffer(cmd);
            CommandBufferPool.Release(cmd);
        }

        public override void FrameCleanup(CommandBuffer cmd)
        {
            if (tempTexture != null)
            {
                tempTexture.Release();
                tempTexture = null;
            }
        }
    }
}
