using UnityEngine;
using System.Collections;

public class ScannerCameraEffect : MonoBehaviour
{
	public Vector3 m_Origin;
	public Material m_Material;
	private float m_ScanDistance;
	private Camera m_Camera;

	public void SetScanDistance (float dist)
	{
		m_ScanDistance = dist;
	}
	public Material GetMaterial ()
	{
		return m_Material;
	}
	void OnEnable ()
	{
		m_Camera = GetComponent<Camera>();
		m_Camera.depthTextureMode = DepthTextureMode.Depth;
	}
	[ImageEffectOpaque]
	void OnRenderImage (RenderTexture src, RenderTexture dst)
	{
		m_Material.SetVector ("_WorldSpaceScannerPos", m_Origin);
		m_Material.SetFloat ("_ScanDistance", m_ScanDistance);
		RaycastCornerBlit (src, dst);
	}
	void RaycastCornerBlit (RenderTexture src, RenderTexture dst)
	{
		// calculate frustum corners
		float camFar = m_Camera.farClipPlane;
		float fovWHalf = m_Camera.fieldOfView * 0.5f;
		float camAspect = m_Camera.aspect;

		Vector3 toRight = m_Camera.transform.right * Mathf.Tan (fovWHalf * Mathf.Deg2Rad) * camAspect;
		Vector3 toTop = m_Camera.transform.up * Mathf.Tan (fovWHalf * Mathf.Deg2Rad);
		Vector3 topLeft = m_Camera.transform.forward - toRight + toTop;
		float camScale = topLeft.magnitude * camFar;

		topLeft.Normalize ();
		topLeft *= camScale;

		Vector3 topRight = m_Camera.transform.forward + toRight + toTop;
		topRight.Normalize ();
		topRight *= camScale;

		Vector3 bottomRight = m_Camera.transform.forward + toRight - toTop;
		bottomRight.Normalize ();
		bottomRight *= camScale;

		Vector3 bottomLeft = m_Camera.transform.forward - toRight - toTop;
		bottomLeft.Normalize ();
		bottomLeft *= camScale;

		// post process pass
		RenderTexture.active = dst;
		m_Material.SetTexture ("_MainTex", src);

		GL.PushMatrix ();
		GL.LoadOrtho ();

		m_Material.SetPass (0);

		GL.Begin (GL.QUADS);
		GL.MultiTexCoord2 (0, 0f, 0f);
		GL.MultiTexCoord (1, bottomLeft);
		GL.Vertex3 (0f, 0f, 0f);

		GL.MultiTexCoord2 (0, 1f, 0f);
		GL.MultiTexCoord (1, bottomRight);
		GL.Vertex3 (1f, 0f, 0f);

		GL.MultiTexCoord2 (0, 1f, 1f);
		GL.MultiTexCoord (1, topRight);
		GL.Vertex3 (1f, 1f, 0f);

		GL.MultiTexCoord2 (0, 0f, 1f);
		GL.MultiTexCoord (1, topLeft);
		GL.Vertex3 (0f, 1f, 0f);

		GL.End();
		GL.PopMatrix();
	}
}
