using UnityEngine;
using System;
using System.Collections;
using System.IO;
using System.Xml.Serialization;

namespace FantasticGlass
{

	/// <summary>
	/// Glass System settings.
	/// </summary>
	[System.Serializable]
	[XmlRoot("Glass_System_Settings")]
	public class GlassSystemSettings
	{
		[XmlAttribute()]
		public int
			lastUsedPreset = default_lastUsedPreset;
		[XmlAttribute()]
		public bool
			enableAlwaysSetOptimumCamera = default_enableAlwaysSetOptimumCamera;
		[XmlAttribute()]
		public bool
			enableAlwaysUseExistingMaterials = default_enableAlwaysUseExistingMaterials;
		[XmlAttribute()]
		public bool
			enableDebugLogging = default_enableDebugLogging;
		[XmlAttribute()]
		public string
			unityDefaultResourcesPath = default_unityDefaultResourcesPath;
		[XmlAttribute()]
		public float
			previewRotationOffset_x = default_previewRotationOffset_x;
		[XmlAttribute()]
		public float
			previewRotationOffset_y = default_previewRotationOffset_y;
		[XmlAttribute()]
		public float
			previewRotationOffset_z = default_previewRotationOffset_z;
		//  N.B. Presumes FBX models as those are required for Asset Store submission:
		[XmlAttribute()]
		public float
			defaultMeshScale = GlassMeshScaleFixLookup.scale_fbx;
		[XmlAttribute()]
		public GlassMeshScaleFix
			defaultMeshScaleFix = GlassMeshScaleFix.fbx;
		[XmlAttribute()]
		public bool
			optimumCamera_enableHDR = default_optimumCamera_enableHDR;
		[XmlAttribute()]
		public RenderingPath
			optimumCamera_renderingPath = default_optimumCamera_renderingPath;
		[XmlAttribute()]
		public bool showSection_optimumCameraSettings = false;
		[XmlAttribute()]
		public bool useCustomGUI = false;
		[XmlAttribute()]
		public string guiSkinPath = "";
		//
		[XmlIgnore()]
		public GUISkin guiSkin = null;

		//  DEFAULT SETTINGS
		public static int default_lastUsedPreset = -1;
		public static bool default_enableAlwaysSetOptimumCamera = false;
		public static bool default_enableAlwaysUseExistingMaterials = true;
		public static bool default_enableDebugLogging = false;
		public static string default_unityDefaultResourcesPath = "unity default resources";
		public static float default_previewRotationOffset_x = 0f;
		public static float default_previewRotationOffset_y = -85f;
		public static float default_previewRotationOffset_z = 0f;
		public static bool default_optimumCamera_enableHDR = true;
		public static RenderingPath default_optimumCamera_renderingPath = RenderingPath.DeferredShading;

		public GlassSystemSettings ()
		{

		}

		public static GlassSystemSettings LoadFromXML (string path)
		{
			if (!File.Exists (path)) {
				Debug.Log ("Glass System Settings:  File does not exist '" + path + "'");
				return null;
			}
			XmlSerializer xmlserialiser = new XmlSerializer (typeof(GlassSystemSettings));
			FileStream filestream = new FileStream (path, FileMode.Open);
			GlassSystemSettings loadedSettings = xmlserialiser.Deserialize (filestream) as GlassSystemSettings;
			filestream.Close ();
			return loadedSettings;
		}

		public static GlassSystemSettings GenerateDefaultSettings()
		{
				GlassSystemSettings defaultSettings = new GlassSystemSettings();
				return defaultSettings;
		}

		public void Save (string path)
		{
			XmlSerializer xmlserialiser = new XmlSerializer (typeof(GlassSystemSettings));
			FileStream fileStream = new FileStream (path, FileMode.Create);
			xmlserialiser.Serialize (fileStream, this);
			fileStream.Close ();
		}

		//

		public bool IsDefault_OptimumCamera ()
		{
			if (optimumCamera_enableHDR != default_optimumCamera_enableHDR)
				return false;
			if (optimumCamera_renderingPath != default_optimumCamera_renderingPath)
				return false;
			return true;
		}

		public void SetDefault_OptimumCamera ()
		{
			optimumCamera_enableHDR = default_optimumCamera_enableHDR;
			optimumCamera_renderingPath = default_optimumCamera_renderingPath;
		}

		//

		public override bool Equals (object o)
		{
			return base.Equals (o);
		}

		public override int GetHashCode ()
		{
			return base.GetHashCode ();
		}

		public override string ToString ()
		{
			return base.ToString ();
		}
	}

}
