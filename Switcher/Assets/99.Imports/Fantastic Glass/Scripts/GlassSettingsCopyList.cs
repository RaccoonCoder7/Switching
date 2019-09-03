using UnityEngine;
using System.Collections;


#region Glass Settings Copy List

public class GlassSettingsCopyList
{
    public bool everything = true;
    public bool glassName = true;
    public bool albedo = true;
    public bool distortion = true;
    public bool bump = true;
    public bool aberration = true;
    public bool extinction = true;
    public bool fog = true;
    public bool surface = true;
    public bool model = true;
    public bool materials = true;
    public bool depth = true;
    public bool zFightRadius = true;

    public GlassSettingsCopyList()
    {
        SelectDefault();
    }

    public static GlassSettingsCopyList EmptyList()
    {
        GlassSettingsCopyList emptyList = new GlassSettingsCopyList();
        emptyList.SelectNone();
        return emptyList;
    }

    public static GlassSettingsCopyList DepthList()
    {
        GlassSettingsCopyList depthOnlyList = GlassSettingsCopyList.EmptyList();
        depthOnlyList.depth = true;
        return depthOnlyList;
    }

    public void SelectDefault()
    {
        SelectAll();
        depth = false;
    }

    public void SelectAll()
    {
        everything = true;
        glassName = true;
        albedo = true;
        distortion = true;
        bump = true;
        aberration = true;
        extinction = true;
        fog = true;
        surface = true;
        model = true;
        materials = true;
        depth = true;
        zFightRadius = true;
    }

    public void SelectNone()
    {
        everything = false;
        glassName = false;
        albedo = false;
        distortion = false;
        bump = false;
        extinction = false;
        aberration = false;
        fog = false;
        surface = false;
        model = false;
        materials = false;
        depth = false;
        zFightRadius = false;
    }

    public bool Everything()
    {
        return everything;
    }

    public bool GlassName()
    {
        return (everything || glassName);
    }

    public bool Albedo()
    {
        return (everything || albedo);
    }

    public bool Distortion()
    {
        return (everything || distortion);
    }

    public bool Bump()
    {
        return (everything || bump);
    }

    public bool Aberration()
    {
        return (everything || aberration);
    }

    public bool Extinction()
    {
        return (everything || extinction);
    }

    public bool Fog()
    {
        return (everything || fog);
    }

    public bool Surface()
    {
        return (everything || surface);
    }

    public bool Model()
    {
        return (everything || model);
    }

    public bool Materials()
    {
        return (everything || materials);
    }

    public bool Depth()
    {
        return (everything || depth);
    }

    public bool ZFightRadius()
    {
        return (everything || zFightRadius);
    }
}

#endregion