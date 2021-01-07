// GENERATED AUTOMATICALLY FROM 'Assets/Dragon/GraphicDragon/Dragon Warrior Files/PlayerInput.inputactions'

using System;
using UnityEngine;
using UnityEngine.Experimental.Input;


[Serializable]
public class PlayerInput : InputActionAssetReference
{
    public PlayerInput()
    {
    }
    public PlayerInput(InputActionAsset asset)
        : base(asset)
    {
    }
    private bool m_Initialized;
    private void Initialize()
    {
        // Gameplay
        m_Gameplay = asset.GetActionMap("Gameplay");
        m_Gameplay_Jump = m_Gameplay.GetAction("Jump");
        m_Gameplay_Crouch = m_Gameplay.GetAction("Crouch");
        m_Gameplay_Attack = m_Gameplay.GetAction("Attack");
        m_Gameplay_Skill = m_Gameplay.GetAction("Skill");
        m_Gameplay_Dodge = m_Gameplay.GetAction("Dodge");
        m_Gameplay_JumpD = m_Gameplay.GetAction("JumpD");
        m_Gameplay_CrouchD = m_Gameplay.GetAction("CrouchD");
        m_Gameplay_AttackD = m_Gameplay.GetAction("AttackD");
        m_Gameplay_SkillD = m_Gameplay.GetAction("SkillD");
        m_Initialized = true;
    }
    private void Uninitialize()
    {
        m_Gameplay = null;
        m_Gameplay_Jump = null;
        m_Gameplay_Crouch = null;
        m_Gameplay_Attack = null;
        m_Gameplay_Skill = null;
        m_Gameplay_Dodge = null;
        m_Gameplay_JumpD = null;
        m_Gameplay_CrouchD = null;
        m_Gameplay_AttackD = null;
        m_Gameplay_SkillD = null;
        m_Initialized = false;
    }
    public void SetAsset(InputActionAsset newAsset)
    {
        if (newAsset == asset) return;
        if (m_Initialized) Uninitialize();
        asset = newAsset;
    }
    public override void MakePrivateCopyOfActions()
    {
        SetAsset(ScriptableObject.Instantiate(asset));
    }
    // Gameplay
    private InputActionMap m_Gameplay;
    private InputAction m_Gameplay_Jump;
    private InputAction m_Gameplay_Crouch;
    private InputAction m_Gameplay_Attack;
    private InputAction m_Gameplay_Skill;
    private InputAction m_Gameplay_Dodge;
    private InputAction m_Gameplay_JumpD;
    private InputAction m_Gameplay_CrouchD;
    private InputAction m_Gameplay_AttackD;
    private InputAction m_Gameplay_SkillD;
    public struct GameplayActions
    {
        private PlayerInput m_Wrapper;
        public GameplayActions(PlayerInput wrapper) { m_Wrapper = wrapper; }
        public InputAction @Jump { get { return m_Wrapper.m_Gameplay_Jump; } }
        public InputAction @Crouch { get { return m_Wrapper.m_Gameplay_Crouch; } }
        public InputAction @Attack { get { return m_Wrapper.m_Gameplay_Attack; } }
        public InputAction @Skill { get { return m_Wrapper.m_Gameplay_Skill; } }
        public InputAction @Dodge { get { return m_Wrapper.m_Gameplay_Dodge; } }
        public InputAction @JumpD { get { return m_Wrapper.m_Gameplay_JumpD; } }
        public InputAction @CrouchD { get { return m_Wrapper.m_Gameplay_CrouchD; } }
        public InputAction @AttackD { get { return m_Wrapper.m_Gameplay_AttackD; } }
        public InputAction @SkillD { get { return m_Wrapper.m_Gameplay_SkillD; } }
        public InputActionMap Get() { return m_Wrapper.m_Gameplay; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled { get { return Get().enabled; } }
        public InputActionMap Clone() { return Get().Clone(); }
        public static implicit operator InputActionMap(GameplayActions set) { return set.Get(); }
    }
    public GameplayActions @Gameplay
    {
        get
        {
            if (!m_Initialized) Initialize();
            return new GameplayActions(this);
        }
    }
}
