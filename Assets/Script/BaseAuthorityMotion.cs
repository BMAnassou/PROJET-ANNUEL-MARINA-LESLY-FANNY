using Unity.Netcode;
using Unity.Netcode.Components;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
// This bypases the default custom editor for NetworkTransform
// and lets you modify your custom NetworkTransform's properties
// within the inspector view
[CustomEditor(typeof(BaseAuthorityMotion), true)]
public class BaseAuthorityMotionEditor : Editor
{
}
#endif
 
/// <summary>
/// Base authority motion handler that defaults to
/// owner authoritative mode.
/// </summary>
public class BaseAuthorityMotion : NetworkTransform
{
    public enum AuthorityModes
    {
        Owner,
        Server
    }
 
    public AuthorityModes AuthorityMode = AuthorityModes.Owner;
 
    [Range(0.5f, 10.0f)]
    public float MoveSpeed = 3.0f;
 
    private Vector3 m_MoveDir;
 
    protected override bool OnIsServerAuthoritative()
    {
        return AuthorityMode == AuthorityModes.Server;
    }
 
    private bool IsAuthority()
    {
        return AuthorityMode == AuthorityModes.Owner ? IsOwner : IsServer;
    }
 
    /// <summary>
    /// Is only invoked for the authority, and I went ahead and made it
    /// protected and virtual in the event you wanted to derive from this
    /// class and use it for both player and AI related motion.
    /// Just placed your player input script in here so you can quickly
    /// test the component.
    /// </summary>
    protected virtual void AuthorityUpdate()
    {
        m_MoveDir = Vector3.zero;
 
        if (Input.GetKey(KeyCode.W)) m_MoveDir.z = +1f;
        if (Input.GetKey(KeyCode.A)) m_MoveDir.z = -1f;
        if (Input.GetKey(KeyCode.S)) m_MoveDir.x = -1f;
        if (Input.GetKey(KeyCode.D)) m_MoveDir.x = +1f;
 
        transform.position += m_MoveDir * MoveSpeed * Time.deltaTime;
    }
 
    protected override void Update()
    {
        // The authority side of NetworkTransform.Update returns early and does not
        // apply Interpolation but does dictate & synchronize changes to the transform.
        // Only update the authority when the associated NetworkObject is considered spawned.
        if (IsSpawned && IsAuthority())
        {
            AuthorityUpdate();
            return;
        }
 
        // Always invoke base class when overriding NetworkTransform.Update as non-authority
        // instances handle updating their transform to the authority's updates (which includes
        // interpolating towards transform state updates).
        base.Update();
    }
}