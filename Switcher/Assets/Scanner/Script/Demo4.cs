﻿using UnityEngine;
using System.Collections;

public class Demo4 : MonoBehaviour
{
	[Header("Parameters")]
	public ScannerObject.FxType m_FxType = ScannerObject.FxType.FT_None;
	public Vector4 m_Dir = new Vector4 (1, 0, 0, 0);
	[Range(0.1f, 2f)] public float m_Amplitude = 1f;
	[Range(1f, 16f)] public float m_Exp = 3f;
	[Range(8f, 64f)] public float m_Interval = 20f;
	[Range(1f, 32f)] public float m_Speed = 10f;
	[Header("Internal")]
	public ScannerObject[] m_Fxs;

	void Start ()
	{
		m_Fxs = GameObject.FindObjectsOfType<ScannerObject> ();
		for (int i = 0; i < m_Fxs.Length; i++)
			m_Fxs[i].Initialize ();
	}
	void Update ()
	{
		for (int i = 0; i < m_Fxs.Length; i++)
		{
			m_Fxs[i].ApplyFx (m_FxType);
			m_Fxs[i].UpdateSelfParameters ();
			m_Fxs[i].ApplyDirectionalScan (m_Dir);
			m_Fxs[i].SetMaterialsVector ("_LightSweepVector", m_Dir);
			m_Fxs[i].SetMaterialsFloat ("_LightSweepAmp", m_Amplitude);
			m_Fxs[i].SetMaterialsFloat ("_LightSweepExp", m_Exp);
			m_Fxs[i].SetMaterialsFloat ("_LightSweepInterval", m_Interval);
			m_Fxs[i].SetMaterialsFloat ("_LightSweepSpeed", m_Speed);
		}
	}
}