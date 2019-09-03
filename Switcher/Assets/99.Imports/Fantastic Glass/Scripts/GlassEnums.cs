using UnityEngine;
using System.Collections;

namespace FantasticGlass
{
    public enum DepthQuality_GlassManager
    {
        Simple = 0,
        Complex = 1,
        ObjectDefined = 3
    }

    public enum DepthQuality_GlassObject
    {
        Simple = 0,
        Complex = 1,
    }

    public enum GlassPhysicalObjectType
    {
        box,
        sphere,
        mesh,
        convexMesh
    }

    public enum GlassPrimitiveType
    {
        none,
        cube,
        sphere,
        capsule,
        cylinder,
        quad,
        plane
    }

    public enum GlassFace
    {
        front = 0,
        back = 1
    }

    public enum GlassDepthLayer
    {
        front=0,
        back=1,
        other=2
    }

    public enum GlassExtinctionAppearance
    {
        AsApplied,
        AsItAppears
    }

    public enum GlassMeshScaleFix
    {
        fbx,
        custom,
        None
    }


    /// <summary>
    /// Glass mesh scale fix lookup.
    /// </summary>
    public class GlassMeshScaleFixLookup
    {
        public static float scale_fbx = 100f;
        public static float scale_none = 1f;

        public GlassMeshScaleFixLookup()
        {
        }

        public static void GetScale(GlassMeshScaleFix fixType, ref float currentScale)
        {
            switch (fixType)
            {
                case GlassMeshScaleFix.fbx:
                    currentScale = scale_fbx;
                    break;
                case GlassMeshScaleFix.None:
                    currentScale = scale_none;
                    break;
                default:
                    break;
            }
        }

        public static void GetEnum(ref GlassMeshScaleFix fixType, float currentScale)
        {
            if (currentScale == scale_fbx)
                fixType = GlassMeshScaleFix.fbx;
            else if (currentScale == scale_none)
                fixType = GlassMeshScaleFix.None;
            else
                fixType = GlassMeshScaleFix.custom;
        }
    }

	public enum GlassFade_FadeType
	{
		FadeIn,
		FadeOut
	}

	public enum MeshAnimator_FilterType
	{
		none,
		contains
	}

	public enum GlassFade_GravityChange
	{
		noChange,
		enableGravity,
		disableGravity
	}

	public enum OptimumCameraChoice
	{
		NoChoice,
		OptimiseOnce,
		OptimiseAlways,
		DoNotOptimise
	}

    public enum StandardShaderRenderMode
    {
        Opaque,
        Fade,
        Transparent,
        Cutout,
        NoChange
    }

}