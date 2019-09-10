using UnityEngine;
using System.Collections;

public class IntersectionFx : MonoBehaviour
{
	[Header("Parameters")]
	[Range(1f, 10f)] public float m_IntersectionMax = 3f;
	[Range(0f, 1f)] public float m_IntersectionDamper = 0.3f;
	public Color m_MainColor = Color.yellow;
	public Color m_IntersectionColor = Color.red;
//	public Color m_RimColor = Color.black;
//	[Range(1f, 8f)] public float m_RimPower = 3f;
	[Header("Auto")]
	public Material[] m_Mats;

	void Start ()
	{
		Renderer rd = GetComponent<Renderer> ();
		m_Mats = rd.materials;
	}
	void Update()
	{
		for (int i = 0; i < m_Mats.Length; i++)
		{
			m_Mats[i].SetFloat ("_IntersectionMax", m_IntersectionMax);
			m_Mats[i].SetFloat ("_IntersectionDamper", m_IntersectionDamper);
			m_Mats[i].SetColor ("_MainColor", m_MainColor);
			m_Mats[i].SetColor ("_IntersectionColor", m_IntersectionColor);
//			m_Mats[i].SetColor ("_RimColor", m_RimColor);
//			m_Mats[i].SetFloat ("_RimPower", m_RimPower);
		}
	}
}
