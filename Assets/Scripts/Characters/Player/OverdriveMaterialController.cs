using UnityEngine;

/// <summary>
/// 能量爆发材质控制器
/// </summary>
public class OverdriveMaterialController : MonoBehaviour
{
    /// <summary>
    /// 能量爆发材质
    /// </summary>
    [SerializeField] Material overdriveMaterial;
    
    /// <summary>
    /// 默认材质
    /// </summary>
    Material defaultMaterial;
    
    /// <summary>
    /// 粒子系统的Render
    /// </summary>
    new Renderer renderer;
    
    void Awake()
    {
        renderer = GetComponent<Renderer>();

        defaultMaterial = renderer.material;
    }
    
    void OnEnable()
    {
        PlayerOverdrive.on += PlayerOverdriveOn;

        PlayerOverdrive.off += PlayerOverdriveOff;
    }

    void OnDisable()
    {
        PlayerOverdrive.on -= PlayerOverdriveOn;
        PlayerOverdrive.off -= PlayerOverdriveOff;
    }

    /// <summary>
    /// 能量爆发开启事件处理器
    /// </summary>
    void PlayerOverdriveOn() => renderer.material = overdriveMaterial;//更换渲染器的材质为能量爆发材质
    
    /// <summary>
    /// 能量爆发关闭事件处理器
    /// </summary>
    void PlayerOverdriveOff() => renderer.material = defaultMaterial;//渲染器的材质切回默认材质
}