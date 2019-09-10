using UnityEngine;
using System.Collections;

public class ScannerObject : MonoBehaviour
{
	public enum FxType { FT_None = 0, FT_Additional, FT_TransparencyAdd, FT_TransparencyBlend, FT_TransparencyTextured, FT_TransparencyStripe };
	[Header("Parameters")]
	public Color m_Color = Color.red;
	public Texture2D m_StripeTex;
	[Range(0.01f, 0.9f)] public float m_StripeWidth = 0.1f;
	[Range(1f, 16f)] public float m_StripeDensity = 5f;
	[Header("Internal")]
	public FxType m_CurrFxType = FxType.FT_None;
	public Shader m_Standard;
	public Shader m_Transparency;
	public Shader m_TransparencyTextured;
	public Shader m_TransparencyStripe;
	public Renderer m_Rd;
	public Shader[] m_BackupShaders;
	public Texture[] m_TexDiffuse;
	public Texture[] m_TexBump;
	
	public void Initialize ()
	{
		// cache the renderer
		m_Rd = GetComponent<Renderer> ();
		
		// cache all original materials
		Material[] mats = m_Rd.materials;
		int len = mats.Length;
		m_BackupShaders = new Shader[len];
		m_TexDiffuse = new Texture[len];
		m_TexBump = new Texture[len];
		for (int i = 0; i < len; i++)
		{
			m_BackupShaders[i] = mats[i].shader;
			m_TexDiffuse[i] = mats[i].GetTexture ("_MainTex");
			if (mats[i].HasProperty ("_BumpMap"))
				m_TexBump[i] = mats[i].GetTexture ("_BumpMap");
		}
		m_Standard = Shader.Find ("Scanner/Standard");
		m_Transparency = Shader.Find ("Scanner/Transparency");
		m_TransparencyTextured = Shader.Find ("Scanner/Transparency Textured");
		m_TransparencyStripe = Shader.Find ("Scanner/Transparency Stripe");
	}
	public void UpdateSelfParameters ()
	{
		Material[] mats = m_Rd.materials;
		for (int i = 0; i < mats.Length; i++)
		{
			mats[i].SetColor ("_LightSweepColor", m_Color);
			mats[i].SetColor ("_StripeColor", m_Color);
			mats[i].SetTexture ("_StripeTex", m_StripeTex);
			mats[i].SetFloat ("_StripeWidth", m_StripeWidth);
			mats[i].SetFloat ("_StripeDensity", m_StripeDensity);
		}
	}
	public void SetMaterialsFloat (string name, float f)
	{
		Material[] mats = m_Rd.materials;
		for (int i = 0; i < mats.Length; i++)
		{
			mats[i].SetFloat (name, f);
		}
	}
	public void SetMaterialsVector (string name, Vector4 v)
	{
		Material[] mats = m_Rd.materials;
		for (int i = 0; i < mats.Length; i++)
		{
			mats[i].SetVector (name, v);
		}
	}
	public void ApplyDirectionalScan (Vector4 dir)
	{
		Material[] mats = m_Rd.materials;
		for (int i = 0; i < mats.Length; i++)
		{
			mats[i].EnableKeyword ("ALS_DIRECTIONAL");
			mats[i].DisableKeyword ("ALS_SPHERICAL");
			mats[i].SetVector ("_LightSweepVector", dir);
		}
	}
	public void ApplySphericalScan ()
	{
		Material[] mats = m_Rd.materials;
		for (int i = 0; i < mats.Length; i++)
		{
			mats[i].EnableKeyword ("ALS_SPHERICAL");
			mats[i].DisableKeyword ("ALS_DIRECTIONAL");
		}
	}
	public void ApplyFx (FxType ft)
	{
		if (ft == m_CurrFxType)
			return;
		m_CurrFxType = ft;		
		
		Material[] mats = m_Rd.materials;
		for (int i = 0; i < mats.Length; i++)
		{
			if (ft == FxType.FT_Additional)
			{
				mats[i].shader = m_Standard;
			}
			else if (ft == FxType.FT_TransparencyAdd)
			{
				mats[i].shader = m_Transparency;
				mats[i].SetInt ("_BlendSrc", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
				mats[i].SetInt ("_BlendDst", (int)UnityEngine.Rendering.BlendMode.One);
			}
			else if (ft == FxType.FT_TransparencyBlend)
			{
				mats[i].shader = m_Transparency;
				mats[i].SetInt ("_BlendSrc", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
				mats[i].SetInt ("_BlendDst", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
			}
			else if (ft == FxType.FT_TransparencyTextured)
			{
				mats[i].shader = m_TransparencyTextured;
			}
			else if (ft == FxType.FT_TransparencyStripe)
			{
				mats[i].shader = m_TransparencyStripe;
			}
			else if (ft == FxType.FT_None)
			{
				mats[i].shader = m_BackupShaders[i];
			}
		}
	}
}
