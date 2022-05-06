using Robust.Shared.Serialization;

namespace Content.Shared.CharacterAppearance
{
    [Serializable, NetSerializable]
    public enum HumanoidVisualLayers : byte
    {
        Tail,
        Hair,
        FacialHair,
        Chest,
        Head,
        Snout,
        Frills,
        Horns,
        EarsInner,
        EarsOuter,
        HeadSide, // side parts (i.e., frills)
        HeadTop,  // top parts (i.e., ears)
        Eyes,
        RArm,
        LArm,
        RHand,
        LHand,
        RLeg,
        LLeg,
        RFoot,
        LFoot,
        TailFront,
        FelinidTailFront,
        Handcuffs,
        StencilMask,
        Fire,
    }
}
