using UnityEngine;
using Unity;
using UnityEngine.Rendering;
public class BendingManager : MonoBehaviour
{
    #region MonoBehaviour

    private void OnEnable()
    {
        RenderPipelineManager.beginCameraRendering += OnBeginCameraRendering;
        RenderPipelineManager.endCameraRendering += OnEndCameraRendering;
    }

    private void OnDisable()
    {
        RenderPipelineManager.beginCameraRendering -= OnBeginCameraRendering;
        RenderPipelineManager.endCameraRendering -= OnEndCameraRendering;
    }

    #endregion

    #region Methods

    private static void OnBeginCameraRendering(ScriptableRenderContext ctx, Camera cam)
    {
        cam.cullingMatrix = Matrix4x4.Ortho(-99, 99, -99, 99, 0.1f, 99) *
                            cam.worldToCameraMatrix;
    }

    private static void OnEndCameraRendering(ScriptableRenderContext ctx, Camera cam)
    {
        //cam.ResetCullingMatrix();
    }

    #endregion
}
