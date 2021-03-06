
using UnityEngine;
using UnityEngine.UI;

public class UIDynamicRawImage : RawImage
{
    public DynamicAtlasGroup mGroup = DynamicAtlasGroup.Size_1024;
    [SerializeField]
    private string _currentPath = null;
    private DynamicAtlasGroup m_Group;
    private DynamicAtlas m_Atlas;
    private OnCallBack onCallBack;
    private bool isSetImage = false;
    protected override void Start()
    {
        base.Start();
#if UNITY_EDITOR
        if (Application.isPlaying)
        {
            OnPreDoImage();
        }
#else
       OnPreDoImage();
#endif
    }

    void OnPreDoImage()
    {
        if (texture != null && isSetImage == false) //说明这个rawimage事先挂了texture，此时需要给copy到图集中
        {
            SetGroup(mGroup);
            PreSetImage();
        }
    }
    public void SetGroup(DynamicAtlasGroup group)
    {
        if (m_Atlas != null)
        {
            return;
        }
        m_Group = group;
        m_Atlas = DynamicAtlasManager.Instance.GetDynamicAtlas(group);
    }

    public DynamicAtlasGroup GetGroup()
    {
        return m_Group;
    }
    public void OnGetImageCallBack(Texture tex, Rect rect, string path)
    {
        this.texture = tex;
        uvRect = rect;
        gameObject.SetActiveVirtual(true);
        if (onCallBack != null)
        {
            onCallBack();
        }
    }
    public void OnGetMaterialCallBack(Material material, Rect rect, string path)
    {
        this.texture = null;
        this.material = material;
        uvRect = rect;
        gameObject.SetActiveVirtual(true);
        if (onCallBack != null)
        {
            onCallBack();
        }
    }

    private void PreSetImage()
    {
        if (string.IsNullOrEmpty(currentPath))
        {
            return;
        }
        if (AtlasConfig.kUsingCopyTexture)
        {
            m_Atlas.SetTexture(currentPath, texture, OnGetImageCallBack);
        }
        else
        {
            m_Atlas.SetTexture(currentPath, texture, OnGetMaterialCallBack);
        }

        //此时可以卸载自己的引用计数
    }
    public void SetImage(string path, OnCallBack callBack = null)
    {
        isSetImage = true;
        gameObject.SetActiveVirtual(false);
        SetImageNoHide(path, callBack);
    }
    public void SetImageNoHide(string path, OnCallBack callBack = null)
    {
        onCallBack = callBack;
        currentPath = path;
        if (m_Atlas == null)
        {
            SetGroup(mGroup);
        }
        if (AtlasConfig.kUsingCopyTexture)
        {
            m_Atlas.GetImage(path, OnGetImageCallBack);
        }
        else
        {
            m_Atlas.GetImage(path, OnGetMaterialCallBack);
        }
    }
    public void OnDispose()
    {
        if (string.IsNullOrEmpty(currentPath) == false && m_Atlas != null)
        {
            m_Atlas.RemoveImage(currentPath, false);
            //m_Atlas.RemoveImage(this._currentPath, true);
            currentPath = null;
        }
        isSetImage = false;
    }
    protected override void OnDestroy()
    {
        base.OnDestroy();
        OnDispose();
    }
    public string currentPath
    {
        set
        {
            _currentPath = value;
        }
        get
        {
            return _currentPath;
        }
    }
}
